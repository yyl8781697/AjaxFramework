using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 得到简单数据类型的值  简单数据类型参照SampleDataExtension
    /// </summary>
    internal class GetRequestSampleTypeData:GetRequestDataStrategy
    {
        
        #region 判断是否是匹配类型
        /// <summary>
        /// 是否是匹配类型 这里把剩下的类型都认为实体来操作
        /// </summary>
        /// <param name="paramType">所需判断的类型</param>
        /// <returns>是否为简单类型</returns>
        public override bool IsMatchType(Type paramType)
        {
            base.IsMatchType(paramType);
            return paramType.IsSampleType();
        }
        #endregion

        #region  取得简单数据类型的值
        /// <summary>
        /// 取得简单数据类型的值
        /// </summary>
        /// <param name="paramName">当前参数的名称</param>
        /// <param name="paramType">当前参数的类型</param>
        /// <param name="currentHttpRequest">当前的请求详情</param>
        /// <returns></returns>
        public override object GetValue(string paramName, Type paramType, HttpRequestDescription currentHttpRequest)
        {
            base.GetValue(paramName, paramType, currentHttpRequest);

            //转为相应的数据类型 进行返回
            return paramType.ConvertSampleTypeValue(currentHttpRequest.WebParameters[paramName]);
            
        }
        #endregion
    }
}
