using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using AjaxFramework.Extension.GetRequestData;
using LitJson;

namespace AjaxFramework
{
    /// <summary>
    /// 参数的帮助类
    /// </summary>
    internal class ParameterHelper
    {
        /// <summary>
        /// 当前的请求的详细信息
        /// </summary>
        private HttpRequestDescription _currentHttpRequest;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentHttpRequest">当前Http请求</param>
        public ParameterHelper(HttpRequestDescription currentHttpRequest)
        {
            if (currentHttpRequest == null)
            {
                throw new ArgumentNullException("currentHttpRequest");
            }

            this._currentHttpRequest = currentHttpRequest;

        }

        /// <summary>
        /// 得到参数的值
        /// </summary>
        /// <param name="parameterInfos">方法参数的详细信息</param>
        /// <returns></returns>
        public object[] GetParameterValues(ParameterInfo[] parameterInfos)
        {
            Array a = Array.CreateInstance(typeof(object), parameterInfos.Length);

            object[] args = new object[parameterInfos.Length];//新建参数数组

            //得到参数值的数组
            if (args != null & args.Length > 0)
            {
                for (int i = 0, count = parameterInfos.Length; i < count; i++)
                {
                    args[i] = GetValue(parameterInfos[i]);
                }
            }
            return args;
        }

        /// <summary>
        /// 得到参数的值
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        private object GetValue(ParameterInfo parameterInfo)
        {
            GetRequestDataContext context = new GetRequestDataContext(parameterInfo.Name, parameterInfo.ParameterType,this._currentHttpRequest);
            return context.GetValue();
        }

        

    }
}
