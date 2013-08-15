using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    internal class ResponseFile : ResponseDataStrategy
    {
        private static ResponseFile _instance = null;
        /// <summary>
        /// 得到当前的实例
        /// </summary>
        /// <returns></returns>
        public static ResponseDataStrategy GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ResponseFile();
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
                if (typeof(byte[]) == type)
                {
                    //设置取消缓存
                    base.CurrentContext.Response.Buffer = true;
                    base.CurrentContext.Response.ExpiresAbsolute = System.DateTime.Now.AddMilliseconds(0);
                    base.CurrentContext.Response.Expires = 0;
                    base.CurrentContext.Response.CacheControl = "no-cache";
                    base.CurrentContext.Response.AppendHeader("Pragma", "No-Cache");

                    base.CurrentContext.Response.BinaryWrite(obj as byte[]);
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
