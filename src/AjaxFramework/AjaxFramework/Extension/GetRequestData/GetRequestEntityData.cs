using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 得到请求中的实体数据 不知道嵌套实体
    /// </summary>
    internal class GetRequestEntityData:GetRequestDataStrategy
    {

        #region 判断是否是匹配类型
        /// <summary>
        /// 是否是匹配类型 这里把剩下的类型都认为实体来操作
        /// </summary>
        /// <param name="paramType">所需要判断的类型</param>
        /// <returns>true</returns>
        public override bool IsMatchType(Type paramType)
        {
            base.IsMatchType(paramType);
            return true;
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
        public override object GetValue(string paramName,Type paramType,HttpRequestDescription currentHttpRequest)
        {
            base.GetValue(paramName, paramType,currentHttpRequest);

            //得到该实体类的属性
            PropertyInfo[] propertys = paramType.GetPropertyInfos(GetRequestDataStrategy.bindingFlags);

            object t = paramType.CreateInstace();
            for (int i = 0; i < propertys.Length; i++)
            {
                //从键值对里面得到值
                string val = currentHttpRequest.WebParameters[propertys[i].Name];

                if (propertys[i].PropertyType.IsSampleType() && propertys[i].CanWrite)
                {
                    //如果是简单的类型 并且为可写  就将值进行转换
                    object obj = propertys[i].PropertyType.ConvertSampleTypeValue(val);
                    propertys[i].SetValue(t, obj, null);
                }
                //不是  还不是简单地类型 就不再进行操作 直接舍弃
            }
            return t;
        }
        #endregion
    }
}
