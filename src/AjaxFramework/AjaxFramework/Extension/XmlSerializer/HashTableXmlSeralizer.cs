using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.IO;

namespace AjaxFramework.Extension.XmlSerializer
{
    /// <summary>
    /// HashTable的 XML序列化
    /// </summary>
    internal class HashTableXmlSeralizer : XmlSerializerStrategy
    {
        /// <summary>
        /// 判断是否属于HashTable类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool IsMatchType(Type type)
        {
            base.IsMatchType(type);
            return type.Equals(typeof(Hashtable));
        }

        /// <summary>
        /// 进行序列化操作
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="nodeName">序列化出来节点的名称  如果值为空的话  使用类型作为节点的名称</param>
        /// <param name="nodeValue">需要序列化节点的值</param>
        public override void Serialize(TextWriter tw, string nodeName, object nodeValue)
        {
            base.Serialize(tw, nodeName, nodeValue);

            if (string.IsNullOrEmpty(nodeName))
            {
                nodeName = nodeValue.GetType().Name;
            }

            tw.WriteLine(string.Format("<{0}>", nodeName));
            Hashtable hs = nodeValue as Hashtable;
            if (hs != null)
            { 
                //取得了正常转换的Hsh的值

                foreach (DictionaryEntry de in hs)
                {
                    //遍历 进行单个的序列化
                    XmlSerializerContext context = new XmlSerializerContext(tw, de.Value.GetType());
                    context.Serialize(Convert.ToString(de.Key), de.Value);
                }
            }
            tw.WriteLine(string.Format("</{0}>", nodeName));
        }
    }
}
