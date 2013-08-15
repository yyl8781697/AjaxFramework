using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using System.Collections;

namespace AjaxFramework.Extension.XmlSerializer
{
    /// <summary>
    /// 字典类的XML序列化
    /// </summary>
    internal class DictionaryXmlSerializer : XmlSerializerStrategy
    {
        /// <summary>
        /// 判断是否属于字典类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool IsMatchType(Type type)
        {
            base.IsMatchType(type);
            return typeof(IDictionary<,>).Name.Equals(type.Name) || typeof(Dictionary<,>).Name.Equals(type.Name);
        }

        /// <summary>
        /// 方法的缓存
        /// </summary>
        private static Dictionary<Type, MethodInfo> _methodInfoCache = new Dictionary<Type, MethodInfo>();

        /// <summary>
        /// 进行序列化操作
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="nodeName">序列化出来节点的名称  如果值为空的话  使用类型作为节点的名称</param>
        /// <param name="nodeValue">需要序列化节点的值</param>
        public override void Serialize(TextWriter tw, string nodeName, object nodeValue)
        {
            base.Serialize(tw, nodeName, nodeValue);

            Type t = nodeValue.GetType();//获取节点的类型

            Type[] ts = t.GetGenericArguments();//取得泛型类型的参数列表

            if (string.IsNullOrEmpty(nodeName))
            {
                nodeName = nodeValue.GetType().Name;
            }

            tw.WriteLine(string.Format("<{0}>", nodeName));
            if (ts.Length >=2)
            { 
                //在类型2个以上才会进行操作
                if (typeof(string).Equals(ts[0]))
                { //字典键一定要是字符串类型  否则不操作

                    PropertyInfo keys = t.GetProperty("Keys");//取得键的属性
                    MethodInfo mi;//获取值的方法申明

                    #region 获取方法
                    if (_methodInfoCache.ContainsKey(t))
                    {
                        mi = _methodInfoCache[t];
                    }
                    else { 
                        mi = nodeValue.GetType().GetMethod("TryGetValue");//取得获取值的方法
                        _methodInfoCache.Add(t, mi);//把方法添加进缓存
                    }
                    #endregion

                    IEnumerable ieKeys = keys.GetValue(nodeValue, null) as IEnumerable;//取得键的集合

                    foreach (string key in ieKeys)
                    {
                        object[] args = new object[2] { key, null };//参数的申明
                        object result = mi.Invoke(nodeValue, args);
                        if (Convert.ToBoolean(result))
                        { 
                            //方法执行成功
                            //遍历 进行单个的序列化
                            XmlSerializerContext context = new XmlSerializerContext(tw, args[1].GetType());
                            context.Serialize(key, args[1]);
                        }
                        
                    }
                }
            }
            tw.WriteLine(string.Format("</{0}>", nodeName));
        }
    }
}
