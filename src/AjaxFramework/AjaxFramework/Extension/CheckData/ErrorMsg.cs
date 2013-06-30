using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 错误信息
    /// </summary>
    internal sealed class ErrorMsg
    {
        /// <summary>
        /// {0}不是有效的{1}的类型或者不能为空
        /// </summary>
        public const string NOT_INT_OR_NULL = "{0}不是有效的{1}的类型或者不能为空";

        /// <summary>
        /// {0}的值{1}不是有效的数字格式
        /// </summary>
        public const string NOT_REGEX_DEGITAL = "{0}的值{1}不是有效的数字格式";

        /// <summary>
        /// {0}的值{1}不能小于{2}
        /// </summary>
        public const string TOO_SMALL = "{0}的值{1}不能小于{2}";

        /// <summary>
        /// {0}的值{1}不能大于{2}
        /// </summary>
        public const string TOO_BIG = "{0}的值{1}不能大于{2}";

        /// <summary>
        /// {0}的值不能为空
        /// </summary>
        public const string NOT_NULL = "{0}的值不能为空";

        /// <summary>
        /// {0}的值{1}不是有效的字符串格式
        /// </summary>
        public const string NOT_REGEX_STRING = "{0}的值{1}不是有效的字符串格式";

        /// <summary>
        /// {0}的值允许最大的长度为{1}
        /// </summary>
        public const string TOO_LONG = "{0}的值允许最大的长度为{1}";

        /// <summary>
        /// {0}的值允许最小的长度为{1}
        /// </summary>
        public const string TOO_SHORT = "{0}的值允许最小的长度为{1}";

        /// <summary>
        /// {0}的的值{1}不是有效的时间格式
        /// </summary>
        public const string NOT_DATE_TIME = "{0}的的值{1}不是有效的时间格式";

        /// <summary>
        /// {0}的的值不正确
        /// </summary>
        public const string UNKNOW_ERROR = "{0}的的值不正确";
    }
}
