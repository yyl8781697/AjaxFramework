using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AjaxFramework
{
    #region 自定义的方法信息
    /// <summary>
    /// 自定义的方法信息 现在用于缓存
    /// </summary>
    public class CustomMethodInfo
    {
        /// <summary>
        /// 自定义的方法的实例
        /// </summary>
        public MethodInfo Method { get; set; }

        /// <summary>
        /// 方法的参数
        /// </summary>
        public ParameterInfo[] ParamterInfos { get; set; }

        /// <summary>
        /// 返回类型
        /// </summary>
        public Type RetureType { get; set; }

        /// <summary>
        /// 该方法的当前实例
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// 方法的所属程序集
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// 最后的更新时间  每获取一次方法都会进行一个更新
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 访问次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 该方法的特性列表
        /// </summary>
        public List<ValidateAttr> AttrList { get; set; }
    }
    #endregion
}
