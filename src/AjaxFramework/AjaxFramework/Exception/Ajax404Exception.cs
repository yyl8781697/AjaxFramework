using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 抛404异常
    /// </summary>
    public class Ajax404Exception:AjaxException
    {
        public Ajax404Exception(string errorMsg) : base(errorMsg) { }

        public Ajax404Exception(AjaxResult ajaxResult) : base(ajaxResult) { }
    }
}
