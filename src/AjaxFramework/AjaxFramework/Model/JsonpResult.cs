using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LitJson;

namespace AjaxFramework
{
    /// <summary>
    /// 返回的是Jsonp的结果
    /// </summary>
    public class JsonpResult
    {
        /// <summary>
        /// Jsonp的键值
        /// </summary>
        public string JsonpKey { get; set; }

        /// <summary>
        /// Jsonp的数据
        /// </summary>
        public object JsonpData { get; set; }

        /// <summary>
        /// 格式化成jsonp格式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}({1})", this.JsonpKey, JsonMapper.ToJson(JsonpData));
        }
    }
}
