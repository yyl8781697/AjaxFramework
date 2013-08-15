using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.IO;
using System.Web.Compilation;
using System.Collections;

namespace AjaxFramework
{
    /// <summary>
    /// 反射方法的一个帮助类
    /// </summary>
    internal class MethodHelper
    {
        #region 属性

        /// <summary>
        /// 当前的方法的一些路径信息
        /// </summary>
        private MethodPathInfo _methodPathInfo;

        /// <summary>
        /// 当前的上下文
        /// </summary>
        private HttpContext _context = null;

        /// <summary>
        /// 当前http请求的一个详细描述
        /// </summary>
        private HttpRequestDescription _httpRequestDescription;

        /// <summary>
        /// 初始化后的方法信息
        /// </summary>
        public CustomMethodInfo CurCustomMethodInfo
        {
            get;
            private set;
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <param name="methodPathInfo">方法的相关路径信息</param>
        public MethodHelper(HttpContext context, MethodPathInfo methodPathInfo)
        {
            if (context == null)
            {
                throw new AjaxException("没有得到当前上下文");
            }
            else
            {
                this._context = context;
            }

            if (methodPathInfo.IsValidate)
            {
                this._methodPathInfo = methodPathInfo;
            }
            else
            {
                throw new ArgumentNullException("参数不能为空");
            }
        }
        #endregion

        #region 初始化方法
        /// <summary>
        /// 初始化方法 
        /// </summary>
        /// <exception cref="Ajax404Exception">没有找到页面</exception>
        public void InitMethod()
        {
            CustomMethodInfo customMethodInfo = MethodCache.GetMethodCache(this._methodPathInfo.ToString());

            #region 获取该方法
            if (customMethodInfo == null)
            {
                #region 如果缓存中不存在 将会反射重新获取方法信息 并且记录缓存
                customMethodInfo = ReflectionHelper.GetMethodBaseInfo(this._methodPathInfo, MethodCache.BINDING_ATTR);
                if (customMethodInfo == null)
                {
                    throw new Ajax404Exception(string.Format("没有找到页面{0}", this._methodPathInfo.MethodName));
                }
                else
                {
                    //添加进缓存
                    MethodCache.AddMethodCache(_methodPathInfo.ToString(), customMethodInfo);

                }
                #endregion
            }
            #endregion

            #region 判断该方法是否有 自定义网络访问的webMethodAttr的特性了 如果没有此特性 根本不给此方法
            if (customMethodInfo.CurWebMethodAttr == null)
            {
                //没有该特性
                throw new Ajax404Exception("没有找到此页面");
            }
            else
            {

                //实例化网络请求方法的一些信息  用于传给特性使用
                _httpRequestDescription = new HttpRequestDescription
                {
                    Context = this._context,
                    WebParameters = customMethodInfo.CurWebMethodAttr.GetWebParameters(this._context),
                    CurrentMethodInfo = customMethodInfo
                };

            }
            #endregion

            this.CurCustomMethodInfo=customMethodInfo;
        }
        #endregion

        #region 检查方法的特性
        /// <summary>
        /// 检查方法的特性  
        /// 现在主要是请求权限的验证 参数的验证
        /// </summary>
        /// <returns></returns>
        public bool CheckAttribute()
        {
            bool ret = true;//定义是否通过的标志

            #region 遍历特性进行验证
            foreach (ValidateAttr attr in this.CurCustomMethodInfo.AttrList)
            {
                attr.CurHttpRequest = _httpRequestDescription;
                #region 判断此特性是否能通过验证
                if (!attr.IsValidate())
                {
                    //特性的验证规则失败
                    ret = false;
                    break;
                }
                #endregion
            }
            #endregion

            return ret;
        }
        #endregion

        #region 执行方法
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <returns></returns>
        public object ExecMethod()
        {
            object ret = null;//返回值
            object newInstance = null;//新实例
            try
            {
                ParameterHelper parameterHelper = new ParameterHelper(this._httpRequestDescription);
                //得到了方法中参数的值
                object[] args = parameterHelper.GetParameterValues(this.CurCustomMethodInfo.ParamterInfos);

                newInstance = this.CurCustomMethodInfo.ClassType.CreateInstace();
                //动态执行方法 这里会 新创建一个实例
                ret = this.CurCustomMethodInfo.Method.Invoke(newInstance, args);
                
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    //如果捕获到的异常有内部错误，则抛出此内部错误
                    ex=ex.InnerException;
                }
                //否则直接抛异常错误
                ret = ex;
                throw ex;
            }
            finally {
                //指定了调用方法的回调
                if (InvokeMethodCallback.InvokeCallbackEventHandler != null)
                {
                    //指定该回调
                    InvokeMethodCallback.InvokeCallbackEventHandler(ret, this._context);
                }

                if (newInstance is IAjax)
                {
                    //将当前新实例给释放掉
                    (newInstance as IAjax).Dispose();
                }
            }
            return ret;
        }
        #endregion

    }
}
