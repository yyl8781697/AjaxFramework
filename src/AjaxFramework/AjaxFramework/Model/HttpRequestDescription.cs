using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace AjaxFramework
{
    /// <summary>
    /// Http请求的一个详细的描述
    /// </summary>
    public class HttpRequestDescription
    {
        /// <summary>
        /// 请求的上下文
        /// </summary>
        public HttpContext Context { get; set; }

        /// <summary>
        /// 请求的参数 最后以此为此次请求的参数值 原始值为Context里面的请求值 但是可能在后期会修改该参数键值对
        /// </summary>
        public NameValueCollection WebParameters { get; set; }

        /// <summary>
        /// 当前请求的方法的详细信息
        /// </summary>
        public CustomMethodInfo CurrentMethodInfo { get; set; }
    }
}
