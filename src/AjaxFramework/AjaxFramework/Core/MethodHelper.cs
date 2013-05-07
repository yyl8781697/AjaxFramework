using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.IO;
using System.Web.Compilation;
using System.Collections;
using System.Web.SessionState;

namespace AjaxFramework
{
    /// <summary>
    /// 反射方法的一个帮助类
    /// </summary>
    internal class MethodHelper : IRequiresSessionState, IReadOnlySessionState
    {
        #region 属性

        /// <summary>
        /// 用于锁
        /// </summary>
        private object obj = new object();

        /// <summary>
        /// 用于存储程序集
        /// </summary>
        private static IDictionary<string, Assembly> _idictAssemby = new Dictionary<string, Assembly>(16, StringComparer.OrdinalIgnoreCase);

        private static IDictionary<string, Type> _idictClass = new Dictionary<string, Type>(64, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 方法的一个字典类 用于缓存维护方法
        /// </summary>
        private static IDictionary<string, CustomMethodInfo> _idictMethod = new Dictionary<string, CustomMethodInfo>(4096, StringComparer.OrdinalIgnoreCase);

        //非静态 需要Public访问权限 忽略大小写
        private static readonly BindingFlags _bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;

        /// <summary>
        /// 当前请求方法的详细描述
        /// </summary>
        private HttpRequestDescription _httpRequestDescription;

        /// <summary>
        /// 当前的方法的一些路径信息
        /// </summary>
        private MethodPathInfo _methodPathInfo;

        /// <summary>
        /// 当前的上下文
        /// </summary>
        private HttpContext _context = null;

        /// <summary>
        /// 字典的键
        /// </summary>
        private string DictKey {
            get {
                return string.Format("{0}.{1}.{2}", this._methodPathInfo.Assembly, this._methodPathInfo.ClassName, this._methodPathInfo.MethodName);
            }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 静态构造方法 主要欲初始化缓存
        /// </summary>
        static MethodHelper()
        {
            InitCache();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <param name="methodPathInfo">方法的相关路径信息</param>
        public MethodHelper(HttpContext context,MethodPathInfo methodPathInfo)
        {
            object obj = context.Session;
            object obj2 = System.Web.HttpContext.Current.Session;
            if (context == null)
            {
                throw new AjaxException("没有得到当前上下文");
            }
            else {
                this._context = context;
            }

            if (methodPathInfo.IsValidate)
            {
                this._methodPathInfo = methodPathInfo;
            }
            else {
                throw new ArgumentNullException("参数不能为空");
            }
        }
        #endregion

        #region 初始化缓存
        /// <summary>
        /// 初始化缓存
        /// </summary>
        private  static void InitCache()
        {
            ICollection assemblies = BuildManager.GetReferencedAssemblies();

            foreach (Assembly assembly in assemblies)
            {

                if (UrlConfig.ASSEMBLY.Equals(assembly))
                {
                    //如果在指定的ajax的业务的程序集中

                    //添加到程序集的缓存中
                    if (!_idictAssemby.Keys.Contains(GetAssemblyName(assembly)))
                    {
                        _idictAssemby.Add(GetAssemblyName(assembly), assembly);
                    }
                    
                    try
                    {
                        foreach (Type t in assembly.GetExportedTypes())
                        {
                            Type[] allInterface = t.GetInterfaces();
                            foreach (Type interfaceName in allInterface)
                            {
                                if ("IAjax".Equals(interfaceName.Name))
                                {
                                    // 该类有IAjax的接口 则默认添加进缓存 添加到类的缓存中
                                    if (!_idictClass.Keys.Contains(t.FullName))
                                    {
                                        _idictClass.Add(t.FullName, t);
                                    }
                                    
                                }
                            }
                        }
                    }
                    catch { }
                }

                
            }

            //针对方法添加缓存
            foreach (string className in _idictClass.Keys)
            {
                foreach (MethodInfo methodInfo in _idictClass[className].GetMethods(_bindingAttr))
                {
                    try
                    {
                        List<ValidateAttr> attrList = ReflectionHelper.GetAttributes<ValidateAttr>(methodInfo);

                        //有标志的WebMethodAttr属性  添加进方法的缓存
                        WebMethodAttr webMethodAttr = attrList.Find(x => x is WebMethodAttr) as WebMethodAttr;
                        if (webMethodAttr == null)
                        {
                            //没有该特性
                            continue;
                        }
                        CustomMethodInfo customMethonfInfo = new CustomMethodInfo()
                        {
                            AttrList = attrList,
                            Count = 0,
                            LastUpdateTime = DateTime.Now,
                            Method = methodInfo,
                            RetureType = methodInfo.ReturnType,
                            ParamterInfos = methodInfo.GetParameters(),
                            Instance = Activator.CreateInstance(_idictClass[className]),
                            Assembly = _idictClass[className].Assembly

                        };
                        //添加进方法的缓存里面去
                        if (!_idictMethod.Keys.Contains(className+"."+methodInfo.Name))
                        {
                            _idictMethod.Add(className + "." + methodInfo.Name, customMethonfInfo);
                        }
                       
                    }catch{}
                    

                }
            }
        }

        /// <summary>
        /// 得到程序集的名称  不带版本信息的
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static string GetAssemblyName(Assembly assembly)
        {
            return assembly.FullName.Split(',')[0];
        }
        #endregion

        #region 获取方法实例 未完善

        #region 得到方法的最基本的信息
        /// <summary>
        /// 得到方法的最基本的信息
        /// </summary>
        /// <returns></returns>
        private CustomMethodInfo GetMethodBaseInfo()
        {
            CustomMethodInfo customMethodInfo = new CustomMethodInfo();
            try
            {
                //得到程序集
                customMethodInfo.Assembly = Assembly.Load(this._methodPathInfo.Assembly);
                //得到类的类型
                Type t = customMethodInfo.Assembly.GetType(this._methodPathInfo.Assembly+"."+this._methodPathInfo.ClassName,true,true);
                //得到类的实例
                customMethodInfo.Instance = Activator.CreateInstance(t);
                //得到方法
                customMethodInfo.Method = t.GetMethod(this._methodPathInfo.MethodName,_bindingAttr);
                if (customMethodInfo.Method == null)
                { 
                    //没有得到方法 把整个信息置空
                    customMethodInfo = null;
                }
            }
            catch
            {
                throw;
            }
            return customMethodInfo;
        }
        #endregion

        /// <summary>
        /// 得到方法的实例 如果没有得到 则返回空 
        /// </summary>
        /// <param name="errMsg">获取失败返回的错误信息</param>
        /// <returns></returns>
        public CustomMethodInfo GetMethod()
        {
            CustomMethodInfo customMethodInfo = null;

            #region 获取该方法

            if (_idictMethod.Keys.Contains(this.DictKey))
            {
                #region 存在缓存中 直接从缓存中取得
                //如果该方法被访问过 存在该字典中
                customMethodInfo = _idictMethod[this.DictKey];
                //更新执行时间
                customMethodInfo.LastUpdateTime = DateTime.Now;
                //访问次数加1
                customMethodInfo.Count++;
                //将调用后的信息反写进去
                _idictMethod[this.DictKey] = customMethodInfo;
                #endregion
            }
            else {
                #region 如果缓存中不存在 将会反射重新获取方法信息 并且记录缓存
                customMethodInfo = this.GetMethodBaseInfo();
                if (customMethodInfo == null)
                {
                    throw new MethodNotFoundOrInvalidException(string.Format("没有找到方法{0}", this._methodPathInfo.MethodName));
                }
                else
                {
                    #region 初始化方法的一些信息
                    //特性列表
                    customMethodInfo.AttrList = ReflectionHelper.GetAttributes<ValidateAttr>(customMethodInfo.Method);
                    //参数列表
                    customMethodInfo.ParamterInfos = customMethodInfo.Method.GetParameters();
                    //返回值类型
                    
                    customMethodInfo.RetureType = customMethodInfo.Method.ReturnType;
                    //方法最后的更新时间
                    customMethodInfo.LastUpdateTime = DateTime.Now;
                    //方法的执行次数
                    customMethodInfo.Count = 1;
                    #endregion

                    #region 加了双重锁  防止死锁掉 将该方法加入缓存
                    //通过了特性的检测
                    if (!_idictMethod.Keys.Contains(this.DictKey))
                    {
                        lock (obj)
                        {
                            //防止在锁的时候 其他用户已经添加了键值
                            if (!_idictMethod.Keys.Contains(this.DictKey))
                            {
                                //将 此方法的信息记录到静态字典中 以便下次从内存中调用
                                _idictMethod.Add(this.DictKey, customMethodInfo);
                            }
                        }
                    }
                    #endregion

                }
                #endregion
            }
            #endregion

            #region 判断该方法是否有 自定义网络访问的webMethodAttr的特性了
            WebMethodAttr webMethodAttr = customMethodInfo.AttrList.Find(x => x is WebMethodAttr) as WebMethodAttr;
            if (webMethodAttr == null)
            {
                //没有该特性
                throw new MethodNotFoundOrInvalidException("此方法并不是网络方法，无法直接访问");
            }
            else
            {
                
                //实例化网络请求方法的一些信息  用于传给特性使用
                this._httpRequestDescription=new HttpRequestDescription{
                 Context=this._context,
                  WebParameters=webMethodAttr.GetWebParameters(this._context),
                   CurrentMethodInfo=customMethodInfo
                };
            }
            #endregion

            #region 验证该方法的特性
            if (!CheckAttribute(customMethodInfo.AttrList))
            {
                //检测失败 将此方法置空掉
                customMethodInfo = null;
            }
            #endregion

            return customMethodInfo;
        }
        #endregion

        #region 执行方法
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="customMethodInfo">方法的详细信息</param>
        /// <returns></returns>
        public object ExecMethod(CustomMethodInfo customMethodInfo)
        {
            object ret = null;
            try
            {
                ParameterHelper parameterHelper = new ParameterHelper(this._httpRequestDescription.WebParameters);
                //得到了方法中参数的值
                object[] args = parameterHelper.GetParameterValues(customMethodInfo.ParamterInfos);
                //动态执行方法
                ret = customMethodInfo.Method.Invoke(customMethodInfo.Instance, args);
            }
            catch {
                throw;
            }
            return ret;
        }
        #endregion

        #region 检查方法的特性 （未完善）
        /// <summary>
        /// 检查方法的特性  （未完善）
        /// 现在主要是请求权限的验证 参数的验证
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="errMsg">验证失败时带出的消息</param>
        /// <returns></returns>
        private bool CheckAttribute(List<ValidateAttr> attrList)
        {
            bool ret = true;
            if (attrList.Count > 0)
            {
                foreach (ValidateAttr attr in attrList)
                {
                    attr.CurrentHttpRequest = this._httpRequestDescription;
                    #region 判断此特性是否能通过验证
                    if (!attr.IsValidate())
                    {
                        //特性的验证规则失败
                        ret = false;
                        break;
                    }
                    #endregion
                }
            }
            else
            {
                throw new MethodNotFoundOrInvalidException("此方法并不是网络方法，无法直接访问");
            }

            return ret;
        }
        #endregion

        
    }
}
