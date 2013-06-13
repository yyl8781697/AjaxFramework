using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace AjaxFramework
{
    /// <summary>
    /// 是输出xml类型的
    /// </summary>
    public class ResponseXml:ResponseDataStrategy
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

            StringBuilder sb = new StringBuilder();//字符流
            TextWriter writer = new StringWriter(sb);//IO写的载体
            XmlSerializer serializer;//序列化对象
            try
            {
                //声明Xml序列化对象实例serializer
                serializer = new XmlSerializer(type);
                //执行序列化并将序列化结果输出到writer
                serializer.Serialize(writer, obj);
            }
            catch (Exception ex)
            {
                //声明针对string的Xml序列化对象实例serializer
                serializer = new XmlSerializer(typeof(string));
                //执行序列化并将序列化结果输出到writer
                serializer.Serialize(writer, ex.Message);
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
