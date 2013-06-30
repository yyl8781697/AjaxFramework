using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AjaxFramework
{
    /// <summary>
    /// 检查字符串
    /// </summary>
    internal class CheckString : CheckDataStrategy
    {
        /// <summary>
        /// 检查字符串
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

                #region 验证正则
                if (!string.IsNullOrEmpty(base.CurrentData.RegexText))
                {
                    //验证正则的本文规则
                    if (!Regex.IsMatch(base.CurrentData.Value, base.CurrentData.RegexText))
                    {
                        throw new ArgumentException(string.Format(ErrorMsg.NOT_REGEX_STRING, base.CurrentData.Name, base.CurrentData.Value));
                    }
                }
                #endregion

                #region 验证最大的长度
                if (base.CurrentData.MaxLength != -1)
                {
                    if (GetLength(base.CurrentData.Value) > base.CurrentData.MaxLength)
                    {
                        throw new ArgumentException(string.Format(ErrorMsg.TOO_LONG, base.CurrentData.Name, base.CurrentData.MaxLength));
                    }
                }
                #endregion

                #region 验证最小的长度  若在允许空情况下  值不为空是才去验证
                if (base.CurrentData.MinLength != -1)
                {
                    if ((!base.CurrentData.Nullable || !string.IsNullOrEmpty(base.CurrentData.Value )) &&GetLength(base.CurrentData.Value) < base.CurrentData.MinLength)
                    {
                        throw new ArgumentException(string.Format(ErrorMsg.TOO_SHORT, base.CurrentData.Name, base.CurrentData.MinLength));
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
                else
                {
                    //抛自定义的错误信息
                    throw new ArgumentException(base.CurrentData.ErrorMsg);
                }
            }

            return true;
        }

        #region 获取字符串长度，一个汉字算两个字节
        /// <summary> 
        /// 获取字符串长度，一个汉字算两个字节 
        /// </summary> 
        /// <param name="str"></param> 
        /// <returns></returns> 
        public static int GetLength(string str)
        {
            if (str.Length == 0) return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0; byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }
        #endregion
    }
}
