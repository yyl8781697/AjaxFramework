using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AjaxFramework
{
    #region ajax请求的结果
    /// <summary>
    /// ajax请求的结果
    /// </summary>
    public class AjaxResult
    {

        /// <summary>
        /// ajax请求结果的标志
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// ajax请求结果中的数据 一般都是存储操作成功的信息
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 返回的错误信息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> _keyValueDict;
        /// <summary>
        /// 额外的一些键值对的数据 一般情况下是不需要了
        /// </summary>
        public IDictionary<string, string> KeyValueDict { get {
            if (this._keyValueDict == null)
            {
                this._keyValueDict = new Dictionary<string, string>();
            }
            return this._keyValueDict;
        }
            set {
                this._keyValueDict = value;
            }
        }

        /// <summary>
        /// 重写该方法 将返回该实例的属性的键值对JSON
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> list = new List<string>();
            //添加必要的标志和数据的键值
            list.Add("\"flag\":\"" + this.Flag + "\"");
            if (this.Data != null)
            {
                //给data赋过值了
                list.Add("\"data\":\"" + ToUnicode(ReplaceJsonChar(this.Data)) + "\"");
            }
            
            if (!string.IsNullOrEmpty(this.ErrorMsg))
            {
                //有错误了 添加上错误信息
                list.Add("\"errorMsg\":\"" + ToUnicode(ReplaceJsonChar(this.ErrorMsg)) + "\"");
            }
            
            if (this.KeyValueDict != null && this.KeyValueDict.Count > 0)
            {
                foreach (string key in this.KeyValueDict.Keys)
                {
                    list.Add("\"" + key + "\":\"" + ToUnicode(this.KeyValueDict[key]) + "\"");
                }
            }
            //合并成json字符串并返回
            return "{" + string.Join(",", list.ToArray()) + "}";
        }

        

        /// <summary>
        /// 输出文本 即Data或ErrorMsg的其中有值的一个
        /// </summary>
        /// <returns></returns>
        public string ToText()
        {
            return string.IsNullOrEmpty(this.Data) ? this.ErrorMsg : this.Data;
        }

        /// <summary>
        /// 替换json的特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ReplaceJsonChar(string str)
        {
            Regex r = new Regex("(['\"\r\n])");
            str = r.Replace(str, "");
            return str;
        }

        /// <summary>
        /// 转为unicode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ToUnicode(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0, count = str.Length; i < count; i++)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(str[i].ToString(), @"[\u4e00-\u9fa5，。！“”；]"))
                {
                    sb.Append(@"\u" + ((int)str[i]).ToString("x"));
                }
                else {
                    sb.Append(str[i]);
                }
            }
            return sb.ToString();
        }
    }
    #endregion
}
