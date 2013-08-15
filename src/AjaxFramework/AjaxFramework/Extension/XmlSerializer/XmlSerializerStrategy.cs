using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AjaxFramework.Extension.XmlSerializer
{
    /// <summary>
    /// Xml序列化策略的抽象类
    /// </summary>
    internal abstract class XmlSerializerStrategy
    {
        /// <summary>
        /// 是否符合类型
        /// </summary>
        /// <returns></returns>
        public virtual bool IsMatchType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return true;
        }

        /// <summary>
        /// 进行序列化操作
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="nodeName">序列化出来节点的名称  如果值为空的话  使用类型作为节点的名称</param>
        /// <param name="nodeValue">需要序列化节点的值</param>
        public virtual void Serialize(TextWriter tw, string nodeName,object nodeValue)
        {
            if (tw == null)
            {
                throw new ArgumentNullException("tw");
            }

            //如果需要序列化的内容为空 则直接返回
            /*if (nodeValue == null)
            {
                return;
            }*/

        }
    }
}
