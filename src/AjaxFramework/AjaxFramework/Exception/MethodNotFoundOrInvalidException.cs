using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 方法没有找到或者无效
    /// </summary>
    public class MethodNotFoundOrInvalidException:AjaxException
    {
        public MethodNotFoundOrInvalidException(string errorMsg) : base(errorMsg) { }

        public MethodNotFoundOrInvalidException(AjaxResult ajaxResult) : base(ajaxResult) { }
    }
}
