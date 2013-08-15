using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace AjaxFramework.Extension.XmlSerializer
{
    /// <summary>
    /// Xml序列化 的上下文
    /// </summary>
    internal class XmlSerializerContext
    {
        /// <summary>
        /// 实际策略类
        /// </summary>
        private XmlSerializerStrategy _strategy=null;

        /// <summary>
        /// 所有策略缓存类
        /// </summary>
        private static readonly List<XmlSerializerStrategy> _strategyCache;

        /// <summary>
        /// IO输入
        /// </summary>
        private TextWriter _tw;

        /// <summary>
        /// 申明XML的头部
        /// </summary>
        private const string XML_HEADER = "<?xml version='1.0' encoding='UTF-8' ?>";

        /// <summary>
        /// 要序列化的类型
        /// </summary>
        private Type _type;

        /// <summary>
        /// 构造函数 构建策略的上下文
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="type"></param>
        public XmlSerializerContext(TextWriter tw, Type type)
        {
            _tw = tw;
            this._type = type;
            this.InitStrategy(type);
        }

        #region 静态构造函数 初始化策略缓存
        /// <summary>
        /// 初始化策略缓存
        /// </summary>
        static XmlSerializerContext()
        {
            _strategyCache = new List<XmlSerializerStrategy>{
                new SampleDataXmlSerializer(),//最简单的数组格式
                new EnumXmlSerializer(),//枚举的序列化
                new ArrayXmlSerializer(),//数组的序列化
                new DictionaryXmlSerializer(),//字典的序列化
                new HashTableXmlSeralizer(),//HashTable的序列化
                new DataTableXmlSerializer(),//DataTable的序列化
                new JsonDataXmlSerializer(),//JsonData特定格式的序列化(该格式为LitJson的类型)
                new EntityXmlSerializer()//实体的序列化
            };
        }
        #endregion

        #region 初始化策略
        /// <summary>
        /// 初始化策略
        /// </summary>
        /// <param name="type"></param>
        private void InitStrategy(Type type)
        {
            //遍历策略缓存 查找符合类型的处理类
            foreach (XmlSerializerStrategy strategy in _strategyCache)
            {
                if (strategy.IsMatchType(type))
                {
                    this._strategy = strategy;
                    break;
                }
            }
            
        }
        #endregion

        /// <summary>
        /// 对目标对象进行序列化
        /// </summary>
        /// <param name="nodeName">节点的名称</param>
        /// <param name="nodeValue">节点的内容</param>
        public void Serialize(string nodeName,object nodeValue)
        {
            if (this._strategy == null)
            { 
                //如果策略为空  直接返回
                return;
            }

            if (this._type==typeof(object) && this._type != nodeValue.GetType())
            {
                //如果需要序列化的类型 与实际的类型不相等  则取实际类型 进行序列化
                //在object类型下 考虑这种情况
                new XmlSerializerContext(this._tw, nodeValue.GetType()).Serialize(nodeName, nodeValue);
                return;
            }

            this._strategy.Serialize(this._tw, nodeName, nodeValue);
        }

        /// <summary>
        /// 指定目标的值
        /// 这个方法会添加上xml的头部
        /// </summary>
        /// <param name="targetValue"></param>
        public void Serialize(object targetValue)
        {
            if (this._strategy == null)
            {
                //如果策略为空  直接返回
                return;
            }

            if (this._type == typeof(object) && this._type != targetValue.GetType())
            {
                //如果需要序列化的类型 与实际的类型不相等  则取实际类型 进行序列化
                //在object类型下 考虑这种情况
                new XmlSerializerContext(this._tw, targetValue.GetType()).Serialize(targetValue);
                return;
            }

            _tw.WriteLine(XML_HEADER);
            this.Serialize("root", targetValue);
        }

    }
}
