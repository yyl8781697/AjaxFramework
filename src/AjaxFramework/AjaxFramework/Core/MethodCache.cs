using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Compilation;

namespace AjaxFramework
{
    /// <summary>
    /// 方法的一个缓存
    /// </summary>
    public class MethodCache
    {
        #region 成员属性
        /// <summary>
        /// 用于锁
        /// </summary>
        private static object obj = new object();

        /// <summary>
        /// 用于存储程序集
        /// </summary>
        private static IDictionary<string, Assembly> _idictAssemby = new Dictionary<string, Assembly>(16, StringComparer.OrdinalIgnoreCase);

        private static IDictionary<string, Type> _idictClass = new Dictionary<string, Type>(64, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 方法的一个字典类 用于缓存维护方法
        /// </summary>
        private static IDictionary<string, CustomMethodInfo> _idictMethod = new Dictionary<string, CustomMethodInfo>(4096, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 得到当前的所有缓存
        /// </summary>
        public static IDictionary<string, CustomMethodInfo> DictMethod {
            get {
                return _idictMethod;
            }
        }


        /// <summary>
        /// 方法的筛选标志 需要Public访问权限 忽略大小写
        /// </summary>
        public const BindingFlags BINDING_ATTR = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase;

        #endregion

        #region 静态构造方法 主要预初始化缓存
        /// <summary>
        /// 静态构造方法 主要预初始化缓存
        /// </summary>
        static MethodCache()
        {
            InitCache();
        }
        #endregion

        #region 初始化缓存
        /// <summary>
        /// 初始化缓存
        /// </summary>
        private static void InitCache()
        {
            ICollection assemblies = BuildManager.GetReferencedAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                string assemblyName= assembly.GetName().Name;
                if (UrlConfig.ASSEMBLY.Equals(assemblyName))
                {
                    //如果在指定的ajax的业务的程序集中

                    //添加到程序集的缓存中
                    if (!_idictAssemby.Keys.Contains(assemblyName))
                    {
                        _idictAssemby.Add(assemblyName, assembly);
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
                foreach (MethodInfo methodInfo in _idictClass[className].GetMethods(BINDING_ATTR))
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
                            ClassType = _idictClass[className],
                            Assembly = _idictClass[className].Assembly

                        };
                        //添加进方法的缓存里面去
                        if (!_idictMethod.Keys.Contains(className + "." + methodInfo.Name))
                        {
                            _idictMethod.Add(className + "." + methodInfo.Name, customMethonfInfo);
                        }

                    }
                    catch { }


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

        #region 添加一个方法的缓存
        /// <summary>
        /// 添加一个方法的缓存
        /// </summary>
        /// <param name="key">方法对应的键值</param>
        /// <param name="customMethodInfo">缓存中的实体信息</param>
        internal static void AddMethodCache(string key,CustomMethodInfo customMethodInfo)
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
            if (!_idictMethod.Keys.Contains(key))
            {
                lock (obj)
                {
                    //防止在锁的时候 其他用户已经添加了键值
                    if (!_idictMethod.Keys.Contains(key))
                    {
                        //将 此方法的信息记录到静态字典中 以便下次从内存中调用
                        _idictMethod.Add(key, customMethodInfo);
                    }
                }
            }
            #endregion
        }
        #endregion

        #region 得到方法缓存
        /// <summary>
        /// 得到方法缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static CustomMethodInfo GetMethodCache(string key)
        {
            CustomMethodInfo customMethodInfo = null;
            if (_idictMethod.Keys.Contains(key))
            {
                customMethodInfo = _idictMethod[key];
                #region 存在缓存中 更新次数和调用事件
                //更新执行时间
                customMethodInfo.LastUpdateTime = DateTime.Now;
                //访问次数加1
                customMethodInfo.Count++;
                #endregion
            }
            return customMethodInfo;
        }
        #endregion

        #region 移除一个方法缓存
        /// <summary>
        /// 移除一个方法缓存
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveMethodCache(string key)
        {
            #region 加了双重锁  防止死锁掉 将该方法移除缓存
            if (!_idictMethod.Keys.Contains(key))
            {
                lock (obj)
                {
                    //防止在锁的时候 其他用户已经添加了键值
                    if (!_idictMethod.Keys.Contains(key))
                    {
                        //将 此方法的信息记录到静态字典中 以便下次从内存中调用
                        _idictMethod.Remove(key);
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
