using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;

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
            //得到当前参数的类型
            Type paramType = parameterInfo.ParameterType;
            //得到参数的名称
            string paramName=parameterInfo.Name;
            //需要返回的值
            object obj = null;



            if (paramType.IsSampleType())
            {
                //是简单类型  直接转换出来
                //这里感觉都不用转 。。。。好像浪费
                obj = paramType.ConvertSampleTypeValue(this._currentHttpRequest.WebParameters[paramName]);
                return obj;
            }

            if ("HttpPostedFile".Equals(paramType.Name))
            {
                //还有一种可能就是上传文件的类型
                obj = this._currentHttpRequest.Context.Request.Files[paramName];
                return obj;
            }

            //如果都不是 将尝试作为实体类 就是需要再去解析
            obj = this.GetModeValue(paramType);
            
            return obj;

        }

        #region 得到实体类的值
        /// <summary>
        /// 得到实体类的值
        /// </summary>
        /// <param name="t"></param>
        private object GetModeValue(Type type)
        {
            //得到该实体类的属性
            PropertyInfo[] propertys = type.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            //T t = new T();
            object t = type.CreateInstace();
            //type.GetProperty(propertys[i].Name).SetValue(
            for (int i = 0; i < propertys.Length; i++)
            {
                //从键值对里面得到值
                string val = this._currentHttpRequest.WebParameters[propertys[i].Name];
                
                if (propertys[i].PropertyType.IsSampleType() && propertys[i].CanWrite)
                {
                    //如果是简单的类型 并且为可写  就将值进行转换
                    object obj=propertys[i].PropertyType.ConvertSampleTypeValue(val);
                    propertys[i].SetValue(t, obj, null);
                }
                //不是  还不是简单地类型 就不再进行操作 直接舍弃
            }
            return t;
        }
        #endregion

    }
}
