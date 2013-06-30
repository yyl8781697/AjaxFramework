using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace AjaxFramework
{
    /// <summary>
    /// 批量的json字符串
    /// </summary>
    /// <typeparam name="T">泛型的类型</typeparam>
    public class BatchJson<T>
    {

        private IDictionary<string, IList<T>> _data = new Dictionary<string, IList<T>>();
        /// <summary>
        /// Json批量序列化后的数据 
        /// </summary>
        public IDictionary<string, IList<T>> Data {
            get {
                return _data;
            }
        }

        /// <summary>
        /// 键值的一个添加操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public void Add(string key,T t)
        {
            if (_data.Keys.Contains(key))
            {
                this._data[key].Add(t);
            }
            else {
                this._data.Add(key, new List<T>{t});
            }
        }
    }

    
}
