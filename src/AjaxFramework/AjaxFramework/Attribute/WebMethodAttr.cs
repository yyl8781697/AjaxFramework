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
        public RequestType CurRequestType { get; set; }

        /// <summary>
        /// 输出的内容类型
        /// <para>常用类型：</para>
        /// <para>text/plain</para>
        /// <para>application/json(默认)</para>
        /// <para>image/*</para>
        /// </summary>
        public string CurContentType
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数 默认post请求，json输出
        /// </summary>
        public WebMethodAttr()
        {
            //设置默认的请求和返回头部
            CurRequestType = RequestType.Post;
            CurContentType = ContentType.JSON;
            base.PriorityLevel = 9999;
        }

        /// <summary>
        /// 构造函数 设置请求类型
        /// </summary>
        /// <param name="requestType">请求类型</param>
        public WebMethodAttr(RequestType requestType)
        {
            CurRequestType = requestType;
            CurContentType = ContentType.JSON;
            base.PriorityLevel = 9999;
        }

        /// <summary>
        /// 构造函数 设置输出文本类型
        /// </summary>
        /// <param name="contentType">输出文本类型 可使用ContentType常量</param>
        public WebMethodAttr(string contentType)
        {
            CurRequestType = RequestType.Post;
            CurContentType = contentType;
            base.PriorityLevel = 9999;
        }

        /// <summary>
        /// 构造函数 设置请求类型 设置输出文本类型
        /// </summary>
        /// <param name="requestType">请求类型</param>
        /// <param name="contentType">输出文本类型 可使用ContentType常量</param>
        public WebMethodAttr(RequestType requestType,string contentType)
        {
            CurRequestType = requestType;
            CurContentType = contentType;
            base.PriorityLevel = 9999;
        }

        #endregion

        #region 验证特征的有效性
        /// <summary>
        /// 验证此方法是否能够由当前的访问方式进行访问
        /// </summary>
        /// <returns></returns>
        public override bool IsValidate()
        {
            base.IsValidate();
            bool ret = false;
            
            if (RequestType.All.Equals(this.CurRequestType))
            {
                ret = true;
            }
            else
            {
                if (base.CurHttpRequest.Context.Request.RequestType.Equals(this.CurRequestType.ToString().ToUpper()))
                {
                    ret = true;
                }
                else
                {
                    throw new Ajax404Exception(string.Format("此页面无法用{0}方式进行访问", base.CurHttpRequest.Context.Request.RequestType));
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
            switch (this.CurRequestType.ToString().ToUpper())
            {
                case "POST":
                    webParameters = new NameValueCollection(context.Request.Form);
                    break;
                case "GET":
                    webParameters = new NameValueCollection(context.Request.QueryString);
                    break;
                default:
                    //默认的话 get和post都可以取
                    webParameters =new NameValueCollection(context.Request.Form);
                    webParameters.Add(context.Request.QueryString);
                    break;
            }
            return webParameters;
        }
        #endregion
    }

    /// <summary>
    /// 网页返回的头部类型
    /// </summary>
    public struct ContentType
    {
        /// <summary>
        /// json格式
        /// </summary>
        public const string JSON = "application/json";

        /// <summary>
        /// 直接输出html格式
        /// </summary>
        public const string HTML = "text/plain";

        /// <summary>
        /// xml格式
        /// </summary>
        public const string XML = "application/xml";

        /// <summary>
        /// 文件格式
        /// </summary>
        public const string FILE = "application/octet-stream";

        /// <summary>
        /// 图片类型 你也可以写其他的  比如image/jpreg  image/png
        /// </summary>
        public const string IMAGE = "image/gif";
    }
}
