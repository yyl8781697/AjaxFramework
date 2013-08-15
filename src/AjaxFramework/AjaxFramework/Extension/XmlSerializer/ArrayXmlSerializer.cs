using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Collections;

namespace AjaxFramework.Extension.XmlSerializer
{
    /// <summary>
    /// 判断是否是属于数组类  
    /// <para>普通数组,List泛型,ArrayList</para>
    /// </summary>
    internal class ArrayXmlSerializer : XmlSerializerStrategy
    {
        /// <summary>
        /// 判断是否属于Array类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool IsMatchType(Type type)
        {
            base.IsMatchType(type);
            return typeof(IList<>).Name.Equals(type.Name) || typeof(List<>).Name.Equals(type.Name)
                || typeof(Array).Name.Equals(type.Name) || typeof(ArrayList).Name.Equals(type.Name);     
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

            IList list = nodeValue as IList;//先转为接口可以读取的类型
            tw.WriteLine("<{0}>", nodeName);
            if (list != null && list.Count>0)
            {
                //取得了正常转换的泛型的值

                foreach (var obj in list)
                {
                    //遍历 进行单个的序列化
                    XmlSerializerContext context = new XmlSerializerContext(tw, obj.GetType());
                    context.Serialize("item", obj);
                }
            }
            tw.WriteLine("</{0}>", nodeName);
        }
    }
}
