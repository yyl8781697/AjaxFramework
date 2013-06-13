using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 得到请求的文件类型 即HttpPostFile
    /// </summary>
    internal class GetRequestFileData:GetRequestDataStrategy
    {
        #region 判断是否是匹配类型
        /// <summary>
        /// 是否是匹配类型 
        /// </summary>
        /// <param name="paramType">所需判断的类型</param>
        /// <returns>如果类型为HttpPostFile格式</returns>
        public override bool IsMatchType(Type paramType)
        {
            base.IsMatchType(paramType);
            return typeof(HttpPostedFile) == paramType;
        }
        #endregion

        #region  取得批量Josn的值
        /// <summary>
        /// 取得相应的值
        /// </summary>
        /// <param name="paramName">当前参数的名称</param>
        /// <param name="paramType">当前参数的类型</param>
        /// <param name="currentHttpRequest">当前的请求详情</param>
        /// <returns></returns>
        public override object GetValue(string paramName, Type paramType, HttpRequestDescription currentHttpRequest)
        {
            base.GetValue(paramName, paramType, currentHttpRequest);
            HttpPostedFile file = currentHttpRequest.Context.Request.Files[paramName];
            return file;
        }
        #endregion
    }
}
