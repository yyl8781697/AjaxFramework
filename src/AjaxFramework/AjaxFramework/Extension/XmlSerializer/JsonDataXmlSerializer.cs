using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using LitJson;
using System.Collections;

namespace AjaxFramework.Extension.XmlSerializer
{
    /// <summary>
    /// JsonData数据的序列化 为LitJson中的类型 如果不使用可以去除
    /// </summary>
    internal class JsonDataXmlSerializer:XmlSerializerStrategy
    {
        /// <summary>
        /// 判断是否属于JsonData
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool IsMatchType(Type type)
        {
            base.IsMatchType(type);
            return typeof(JsonData).Equals(type);
        }

        /// <summary>
        /// 进行序列化操作
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="nodeName">序列化出来节点的名称  如果值为空的话  会使用Table名称作为节点名，如果再为空 则去类型作为节点名</param>
        /// <param name="nodeValue">需要序列化节点的值</param>
        public override void Serialize(TextWriter tw, string nodeName, object nodeValue)
        {
            base.Serialize(tw, nodeName, nodeValue);

            JsonData jsonData = nodeValue as JsonData;
            
            if (string.IsNullOrEmpty(nodeName))
            {
                nodeName = nodeValue.GetType().Name;//取类型作为节点的名称
            }

            tw.WriteLine(string.Format("<{0}>", nodeName));

            if (jsonData != null)
            {
                //在JsonData不为空的情况下才可以
                switch (jsonData.GetJsonType())
                {
                    case JsonType.Object:
                        #region 如果是字典类
                        //如果是字典类
                        IDictionary<string, JsonData> idictJsonData = jsonData.Inst_Object;
                        foreach (string key in idictJsonData.Keys)
                        {
                            //遍历 字典的单个进行序列化
                            XmlSerializerContext dictContext = new XmlSerializerContext(tw, idictJsonData[key].GetValueType());
                            dictContext.Serialize(key, idictJsonData[key]);
                        }
                        #endregion
                        break;
                    case JsonType.Array:
                        #region 如果是数组类型  因为数组里面还是JsonData  所以还需要交还给自己处理
                        tw.WriteLine(string.Format("<{0}>", "array"));
                        for (int i = 0, count = jsonData.Count; i < count; i++)
                        {
                            XmlSerializerContext arrayContext = new XmlSerializerContext(tw, typeof(JsonData));
                            arrayContext.Serialize("item", jsonData[i]);
                        }
                        tw.WriteLine(string.Format("</{0}>", "array"));
                        #endregion
                        break;
                    default:
                        #region 直接是最普通的类型
                        XmlSerializerContext context = new XmlSerializerContext(tw, jsonData.GetValueType());
                        context.Serialize(jsonData.GetValueType().Name, jsonData);
                        #endregion
                        break;

                }
            }
           
            tw.WriteLine(string.Format("</{0}>", nodeName));
        }
    }
}
