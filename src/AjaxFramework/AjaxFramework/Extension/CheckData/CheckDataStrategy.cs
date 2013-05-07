using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 检查参数的一个策略
    /// </summary>
    abstract class CheckDataStrategy
    {
        /// <summary>
        /// 当前数据的描述
        /// </summary>
        public WebParameterAttr CurrentData { get; set; }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckData()
        {
            if (this.CurrentData == null)
            {
                throw new ArgumentException("no data");
            }
            return true;
        }
    }
}
