using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections.Specialized;

namespace AjaxFramework
{
    /// <summary>
    /// 网络请求的方法特征 无此特性将无法反射访问该方法 
    /// PriorityLevel=9999;
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class WebMethodAttr : ValidateAttr
    {
        #region 属性
        /// <summary>
        /// 请求类型/方式 get post
        /// </summary>
        public RequestType CurrentReuqestType { get; private set; }

        /// <summary>
        /// 默认的返回头
        /// </summary>
        private string _contentType = "application/json";
        /// <summary>
        /// 输出的内容类型
        /// <para>常用类型：</para>
        /// <para>text/plain</para>
        /// <para>application/json(默认)</para>
        /// <para>image/*</para>
        /// </summary>
        public string ContentType
        {
            get
            {
                return this._contentType;
            }
            set
            {
                this._contentType = value;
            }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        public WebMethodAttr(RequestType reuqestType)
        {
            this.CurrentReuqestType = reuqestType;
            base.PriorityLevel = 9999;
        }

        #region 验证特征的有效性
        /// <summary>
        /// 验证此方法是否能够由当前的访问方式进行访问
        /// </summary>
        /// <returns></returns>
        public override bool IsValidate()
        {
            base.IsValidate();
            bool ret = false;

            if (RequestType.All.Equals(this.CurrentReuqestType))
            {
                ret = true;
            }
            else
            {
                if (base.CurrentHttpRequest.Context.Request.RequestType.Equals(this.CurrentReuqestType.ToString().ToUpper()))
                {
                    ret = true;
                }
                else
                {
                    throw new MethodNotFoundOrInvalidException(string.Format("此方法无法用{0}方式进行访问", base.CurrentHttpRequest.Context.Request.RequestType));
                }
            }

            return ret;
        }
        #endregion

        #region 获取该特性实例下的Http请求的键值对数据
        /// <summary>
        /// 获取该特性实例下的Http请求的键值对数据
        /// </summary>
        /// <param name="context">当前的上下文</param>
        /// <returns></returns>
        public NameValueCollection GetWebParameters(HttpContext context)
        {
            if (context == null)
            {
                throw new AjaxException("请求的上下文为空");
            }
            NameValueCollection webParameters;
            switch (this.CurrentReuqestType.ToString().ToUpper())
            {
                case "POST":
                    webParameters = context.Request.Form;
                    break;
                case "GET":
                    webParameters = context.Request.QueryString;
                    break;
                default:
                    webParameters = context.Request.Params;
                    break;
            }
            return webParameters;
        }
        #endregion

    }
}
