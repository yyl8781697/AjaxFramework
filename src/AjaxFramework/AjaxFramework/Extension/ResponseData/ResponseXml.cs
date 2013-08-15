using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using AjaxFramework.Extension.XmlSerializer;

namespace AjaxFramework
{
    /// <summary>
    /// 是输出xml类型的
    /// </summary>
    internal class ResponseXml:ResponseDataStrategy
    {

        private static ResponseXml _instance = null;
        /// <summary>
        /// 得到当前的实例
        /// </summary>
        /// <returns></returns>
        public static ResponseDataStrategy GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ResponseXml();
            }
            return _instance;
        }

        /// <summary>
        /// 得到输出的数据
        /// </summary>
        /// <param name="obj">要输出的值</param>
        /// <param name="type">要输出的类型</param>
        /// <returns>返回Json字符串</returns>
        public override string GetResponse(object obj, Type type)
        {
            base.GetResponse(obj, type);

            if (type == typeof(object))
            {
                //如果类型是object  尝试取他实际的类型
                type = obj.GetType();
            }

            StringBuilder sb = new StringBuilder();//字符流
            TextWriter writer = new StringWriter(sb);//IO写的载体
            XmlSerializerContext serializerContext;//序列化对象
            try
            {
                //声明Xml序列化对象实例serializer
                serializerContext = new XmlSerializerContext(writer, type);
                //执行序列化并将序列化结果输出到writer
                serializerContext.Serialize(obj);
            }
            catch (Exception ex)
            {
                writer.Close();
                sb = new StringBuilder();
                writer = new StringWriter(sb);
                serializerContext = new XmlSerializerContext(writer, type);
                serializerContext.Serialize(ex.Message);
            }
            finally
            {
                //关闭当前写的IO
                writer.Close();
            }

            return sb.ToString();
        }


    }
}
