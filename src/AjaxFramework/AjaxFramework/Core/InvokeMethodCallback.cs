using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AjaxFramework
{
    /// <summary>
    /// 调用方法回调的事件的委托
    /// </summary>
    /// <param name="ret">方法返回的内容</param>
    /// <param name="context">调用时的上下文</param>
    public delegate void InvokeCallbackEvent(object ret, HttpContext context);

    /// <summary>
    /// 回调调用方法
    /// </summary>
    public class InvokeMethodCallback
    {
        /// <summary>
        /// 方法的事件
        /// </summary>
        public static InvokeCallbackEvent InvokeCallbackEventHandler;
    }
}
