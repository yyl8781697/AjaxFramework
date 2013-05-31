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
    public class ResponseString:ResponseDataStrategy
    {
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
                    ret = Convert.ToString(obj);
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
