using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace AjaxFramework
{
    /// <summary>
    /// 直接输出Json类型的
    /// </summary>
    public class ResponseJson:ResponseDataStrategy
    {
        private static ResponseJson _instance = null;
        /// <summary>
        /// 得到当前的实例
        /// </summary>
        /// <returns></returns>
        public static ResponseDataStrategy GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ResponseJson();
            }
            return _instance;
        }

        /// <summary>
        /// 得到输出的数据
        /// </summary>
        /// <param name="obj">要输出的值</param>
        /// <param name="type">要输出的类型</param>
        /// <returns>返回Json字符串</returns>
        public override string GetResponse(object obj, Type type)
        {
            base.GetResponse(obj, type);

            string ret = string.Empty;
            try
            {
                if (type.IsSampleType())
                {
                    //返回的是简单类型
                    AjaxResult ajaxResult = new AjaxResult()
                    {
                        Flag = "1",
                        Data = Convert.ToString(obj)
                    };
                    ret = ajaxResult.ToString();
                }
                else if (obj is AjaxResult)
                {
                    ret = (obj as AjaxResult).ToString();
                }
                else
                {
                    ret = JsonMapper.ToJson(obj);
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
    }
}
