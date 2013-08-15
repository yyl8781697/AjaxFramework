using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;

namespace AjaxFramework.Extension.XmlSerializer
{
    internal class EntityXmlSerializer : XmlSerializerStrategy
    {
        /// <summary>
        /// 取方法的一些标志  实例成员 公开 有读功能
        /// </summary>
        private const BindingFlags _indingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty;

        /// <summary>
        /// 判断是否属于HashTable类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool IsMatchType(Type type)
        {
            base.IsMatchType(type);
            if (type.FullName.StartsWith("System"))
            {
                //以System开头的  认为不是自己的实体
                return false;
            }

            return true;
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
            //得到该实体类的属性
            PropertyInfo[] propertys = nodeValue.GetType().GetProperties(_indingFlags);
            for (int i = 0; i < propertys.Length; i++)
            {
                //遍历 进行属性进行序列化
                XmlSerializerContext context = new XmlSerializerContext(tw, propertys[i].PropertyType);
                context.Serialize(propertys[i].Name, propertys[i].GetValue(nodeValue, null));
            }
            
            tw.WriteLine(string.Format("</{0}>", nodeName));
        }
    }
}
