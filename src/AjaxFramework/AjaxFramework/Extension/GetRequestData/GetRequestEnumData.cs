using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 
    /// </summary>
    internal class GetRequestEnumData : GetRequestDataStrategy
    {
        #region 判断是否是匹配类型
        /// <summary>
        /// 是否是匹配类型 这里把剩下的类型都认为实体来操作
        /// </summary>
        /// <param name="paramType">所需判断的类型</param>
        /// <returns>是否为枚举类型</returns>
        public override bool IsMatchType(Type paramType)
        {
            base.IsMatchType(paramType);
            return paramType.IsEnum;
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

            
            string enumValue=currentHttpRequest.WebParameters[paramName];
            if (string.IsNullOrEmpty(enumValue))
            {
                //枚举值为空 
                return null;
            }

            /***************************************
             *  这里重定义枚举的值 是为了转成int整形之后 
             *  能够使用枚举的Int类型区判断他是否存在该定义枚举重
             * ***************************************/
            object objValue = enumValue;
            if (Regex.IsMatch(enumValue, @"^\d+$"))
            {
                objValue = Convert.ToInt32(enumValue);
            }

            if (!Enum.IsDefined(paramType, objValue))
            {
                //不在定义范围内 
                return null;
            }
            
            //转为相应的枚举类型 进行返回
            return Enum.Parse(paramType, enumValue, true);
        }
        #endregion
    }
}
