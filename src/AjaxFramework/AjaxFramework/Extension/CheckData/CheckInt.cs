using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AjaxFramework
{
    /// <summary>
    /// 检查整型
    /// </summary>
    internal class CheckInt:CheckDataStrategy
    {
        /// <summary>
        /// 检查整型
        /// </summary>
        /// <returns></returns>
        public override bool CheckData()
        {
            base.CheckData();

            try
            {
                #region 检查是否允许为空
                if (!Regex.IsMatch(base.CurrentData.Value, @"^\d+$") || base.CurrentData.Nullable)
                {
                    //不是有效的数字
                    throw new ArgumentException(string.Format(ErrorMsg.NOT_INT_OR_NULL, base.CurrentData.Name,base.CurrentData.ParaType.ToString()));
                }
                #endregion

                #region 验证正则
                if (!string.IsNullOrEmpty(base.CurrentData.RegexText))
                {
                    //验证正则的本文规则
                    if (!Regex.IsMatch(base.CurrentData.Value, base.CurrentData.RegexText))
                    {
                        throw new ArgumentException(string.Format(ErrorMsg.NOT_REGEX_DEGITAL, base.CurrentData.Name, base.CurrentData.Value));
                    }
                }
                #endregion

                #region 验证最大最小值
                Int64 digit = Convert.ToInt64(base.CurrentData.Value);
                if (base.CurrentData.MinValue != -1)
                {
                    if (digit < base.CurrentData.MinValue)
                    {
                        throw new ArgumentException(string.Format(ErrorMsg.TOO_SMALL, base.CurrentData.Name, base.CurrentData.Value, base.CurrentData.MinValue));
                    }
                }

                if (base.CurrentData.MaxValue != -1)
                {
                    if (digit > base.CurrentData.MaxValue)
                    {
                        throw new ArgumentException(string.Format(ErrorMsg.TOO_BIG, base.CurrentData.Name, base.CurrentData.Value, base.CurrentData.MinValue));

                    }
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
                else {
                    //抛自定义的错误信息
                    throw new ArgumentException(base.CurrentData.ErrorMsg);
                }
            }

            return true;
        }
    }
}
