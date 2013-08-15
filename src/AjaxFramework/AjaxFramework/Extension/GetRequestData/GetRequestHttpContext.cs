using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 获取请求数据的当前上下文 用于需要知道一些请求相关的数据或者当前类型无法满足时使用
    /// </summary>
    internal class GetRequestHttpContext : GetRequestDataStrategy
    {
        #region 判断是否是匹配类型
        /// <summary>
        /// 是否是匹配类型 需要为HttpContext类型
        /// </summary>
        /// <param name="paramType">所需要判断的类型</param>
        /// <returns>true</returns>
        public override bool IsMatchType(Type paramType)
        {
            base.IsMatchType(paramType);
            return typeof(HttpContext) == paramType;
        }
        #endregion

        #region 得到实体类的值
        /// <summary>
        /// 得到实体类的值
        /// </summary>
        /// <param name="paramName">当前参数的名称</param>
        /// <param name="paramType">当前参数的类型</param>
        /// <param name="currentHttpRequest">当前的请求详情</param>
        /// <returns></returns>
        public override object GetValue(string paramName, Type paramType, HttpRequestDescription currentHttpRequest)
        {
            base.GetValue(paramName, paramType, currentHttpRequest);
            //在这里直接返回上下文
            return currentHttpRequest.Context;
        }
        #endregion
    }
}
