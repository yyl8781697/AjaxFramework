using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 自定义的Ajax错误
    /// </summary>
    public class AjaxException:Exception
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        private string _errorMsg;

        /// <summary>
        /// 错误类
        /// </summary>
        private AjaxResult _ajaxResult;

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="errorMsg"></param>
        public AjaxException(string errorMsg)
        {
            this._errorMsg = errorMsg;
        }

        /// <summary>
        /// 直接将ajax结果类传出来
        /// </summary>
        /// <param name="ajaxResult"></param>
        public AjaxException(AjaxResult ajaxResult)
        {
            this._ajaxResult = ajaxResult;
        }

        public override string Message
        {
            get
            {
                if (!string.IsNullOrEmpty(this._errorMsg))
                {
                    return this._errorMsg;
                }
                return _ajaxResult.ToString();
            }
        }

        
    }
}
