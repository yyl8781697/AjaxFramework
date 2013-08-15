using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace AjaxFramework.Extension.XmlSerializer
{
    /// <summary>
    /// 枚举类型的XML序列化
    /// </summary>
    internal class EnumXmlSerializer : XmlSerializerStrategy
    {
        /// <summary>
        /// 判断是否属于枚举类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool IsMatchType(Type type)
        {
            base.IsMatchType(type);
            return type.IsEnum;
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
                nodeName = nodeValue.GetType().Name;//没有传具体的类型则  直接去类型名 作为节点名称
            }

            tw.WriteLine("<{0}>{1}</{0}>", nodeName, nodeValue.ToString());
        }
    }
}
