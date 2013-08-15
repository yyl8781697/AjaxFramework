using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 得到请求中的实体数据 最好别存在实体嵌套的情况
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
            if (paramType.FullName.StartsWith("System"))
            {
                //以System开头的  认为不是自己的实体
                return false;
            }
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
                if (propertys[i].CanWrite)
                {
                    //如果为可写时   尝试再次使用策略进行转化
                    GetRequestDataContext context = new GetRequestDataContext(propertys[i].Name, propertys[i].PropertyType, currentHttpRequest);
                    
                    object obj = context.GetValue();
                    propertys[i].SetValue(t, obj, null);
                }
                //不是  还不是简单地类型 就不再进行操作 直接舍弃
            }
            return t;
        }
        #endregion
    }
}
