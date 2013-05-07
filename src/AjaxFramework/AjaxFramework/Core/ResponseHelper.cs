using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace AjaxFramework
{
    /// <summary>
    /// 输出的帮助类
    /// </summary>
    internal class ResponseHelper
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <param name="returnType">返回类型</param>
        /// <returns>返回字符串</returns>
        public static string GetResponseString(object returnValue,Type returnType)
        {
            string ret = string.Empty;
            try
            {
                if (returnType.IsSampleType())
                {
                    //返回的是简单类型
                    AjaxResult ajaxResult = new AjaxResult()
                    {
                        Flag = "1",
                        Data = Convert.ToString(returnValue)
                    };
                    ret = ajaxResult.ToString();
                }
                else
                {
                    ret = JsonConvert.SerializeObject(returnValue);
                }
            }
            catch (Exception ex)
            {
                //遇到异常
                AjaxResult errResult = new AjaxResult()
                {
                    Flag = "0",
                    ErrorMsg = ex.Message
                };
                ret = errResult.ToString();
            }
           
            return ret;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="errorMessge"></param>
        /// <returns></returns>
        public static string ResponseError(string errorMessge)
        {
            //返回错误
            AjaxResult errResult = new AjaxResult()
            {
                Flag = "0",
                ErrorMsg = errorMessge
            };
            return errResult.ToString();
        }
    }
}
