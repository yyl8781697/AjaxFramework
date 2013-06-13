using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using LitJson;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 得到简单json数组的值 在后台即表现为List<T> 泛型  现在这个还有问题。。。。。
    /// </summary>
    internal class GetResquestListData:GetRequestDataStrategy
    {
        #region 判断是否是匹配类型
        /// <summary>
        /// 是否是匹配类型 
        /// </summary>
        /// <param name="paramType">所需判断的类型</param>
        /// <returns>如果类型为List<T>格式，则为Json数组格式</returns>
        public override bool IsMatchType(Type paramType)
        {
            return typeof(List<>).Name.Equals(paramType.Name);
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

            string json = currentHttpRequest.WebParameters[paramName];
            Type type = paramType.GetGenericArguments()[0];
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            PropertyInfo[] propertys = type.GetPropertyInfos(GetRequestDataStrategy.bindingFlags);//反射该类型的属性

            var list = typeof(List<>).MakeGenericType(type).CreateInstace();//动态创建所指定的泛型类型

            List<object> jsonData = JsonMapper.ToObject<List<object>>(json);//反序列化Json数据得到JsonData

            MethodInfo mi= typeof(JsonMapper).GetMethod("ToObject");
            DynamicMethodHelper.DynamicMethodDelegate dmd = DynamicMethodHelper.GetDynamicMethodDelegate(mi, typeof(string));
            object obj=dmd(json);

            return null;
        }
        #endregion
    }
}
