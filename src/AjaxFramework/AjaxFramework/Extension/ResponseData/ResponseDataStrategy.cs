using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 输出数据的一个抽象策略
    /// </summary>
    public abstract class ResponseDataStrategy
    {

        /// <summary>
        /// 得到输出的数据
        /// </summary>
        /// <param name="obj">要输出的值</param>
        /// <param name="type">要输出的类型</param>
        /// <returns></returns>
        public virtual string GetResponse(object obj, Type type)
        {
            if (type == null)
            {
                throw new AjaxException("未指定明确的返回值类型!");
            }

            if (obj == null && type!=typeof(void))
            {
                throw new AjaxException("返回值不能为NULL!");
            }

            return "";
        }
    }
}
