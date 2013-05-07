using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 检查时间
    /// </summary>
    internal class CheckDate:CheckDataStrategy
    {
        /// <summary>
        /// 检查时间
        /// </summary>
        /// <returns></returns>
        public override bool CheckData()
        {
            base.CheckData();

            try
            {
                #region 对空项的验证
                if (!base.CurrentData.Nullable && string.IsNullOrEmpty(base.CurrentData.Value))
                {
                    throw new ArgumentException(string.Format(ErrorMsg.NOT_NULL, base.CurrentData.Name));
                }
                #endregion

                #region 检查是否是时间格式

                DateTime dt;
                if (!DateTime.TryParse(base.CurrentData.Value, out dt))
                {
                    throw new ArgumentException(string.Format(ErrorMsg.NOT_DATE_TIME, base.CurrentData.Name, base.CurrentData.Value));
                }

                #endregion
            }
            catch (ArgumentException argEx)
            {
                //捕获到了抛出的参数异常
                if (string.IsNullOrEmpty(base.CurrentData.ErrorMsg))
                {
                    //没有自定义的错误信息 抛系统设定的信息
                    throw argEx;
                }
                else
                {
                    //抛自定义的错误信息
                    throw new ArgumentException(base.CurrentData.ErrorMsg);
                }
            }

            return true;
        }
    }
}
