using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 得到请求数据的一个抽象策略
    /// </summary>
    internal abstract class GetRequestDataStrategy
    {
        /// <summary>
        /// 取方法的一些标志  实例成员 公开 有写功能
        /// </summary>
        protected const BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty;

        #region 是否是匹配上的类型
        /// <summary>
        /// 是否是匹配上的类型
        /// </summary>
        /// <param name="paramType">指定的类型</param>
        /// <returns>是的话返回true，否则，返回false</returns>
        public virtual bool IsMatchType(Type paramType)
        {
            //验证当前类型是否为空
            if (paramType == null)
            {
                throw new ArgumentNullException("paramType");
            }

            return true;
        }
        #endregion

        #region 得到相应类型的值
        /// <summary>
        /// 得到相应类型的值
        /// </summary>
        /// <param name="paramName">当前参数的名称</param>
        /// <param name="paramType">当前参数的类型</param>
        /// <param name="currentHttpRequest">当前的请求详情</param>
        /// <returns></returns>
        public virtual object GetValue(string paramName,Type paramType,HttpRequestDescription currentHttpRequest)
        {
            //验证参数名称是否为空
            if (string.IsNullOrEmpty(paramName))
            {
                throw new ArgumentNullException("paramName");
            }

            //验证当前类型是否为空
            if (paramType == null)
            {
                throw new ArgumentNullException("paramType");
            }

            //验证当前的请求详情是否为空
            if(currentHttpRequest==null)
            {
                throw new ArgumentNullException("currentHttpRequest");
            }
            return null;
        }
        #endregion
    }
}
