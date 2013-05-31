using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 方法的一些路径信息
    /// </summary>
    internal class MethodPathInfo
    {
        /// <summary>
        /// 程序集
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 这个路径信息是否是有效的 表面判断 如果有空值 则无效
        /// </summary>
        public bool IsValidate
        {
            get {
                return !string.IsNullOrEmpty(Assembly) && !string.IsNullOrEmpty(ClassName) && !string.IsNullOrEmpty(MethodName);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", this.Assembly, this.ClassName, this.MethodName);
        }

       
    }
}
