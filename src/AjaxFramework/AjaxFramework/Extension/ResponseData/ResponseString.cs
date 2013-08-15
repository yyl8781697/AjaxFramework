using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace AjaxFramework
{
    /// <summary>
    /// 直接输出字符串
    /// </summary>
    internal class ResponseString : ResponseDataStrategy
    {
        private static ResponseString _instance = null;
        /// <summary>
        /// 得到当前的实例
        /// </summary>
        /// <returns></returns>
        public static ResponseDataStrategy GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ResponseString();
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
                if (type == typeof(object))
                {
                    //如果类型是object  尝试取他实际的类型
                    type = obj.GetType();
                }

                if (type.IsSampleType())
                {
                    //返回的是简单类型
                    ret = Convert.ToString(obj);
                }
                else if (obj is AjaxResult)
                {
                    ret = (obj as AjaxResult).ToText();
                }
                else if (obj is JsonpResult)
                {
                    ret = (obj as JsonpResult).ToString();
                }
                else
                {
                    ret = JsonMapper.ToJson(obj);
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }

            return ret;
        }
    }
}
