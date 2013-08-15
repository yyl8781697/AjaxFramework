using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Data;

namespace AjaxFramework.Extension.XmlSerializer
{
    /// <summary>
    /// DataTable表的序列化
    /// </summary>
    internal class DataTableXmlSerializer:XmlSerializerStrategy
    {
        /// <summary>
        /// 判断是否属于DataTable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool IsMatchType(Type type)
        {
            base.IsMatchType(type);
            return typeof(DataTable).Equals(type);
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

            DataTable dt=nodeValue as DataTable;

            if (string.IsNullOrEmpty(nodeName))
            {
                if (dt != null && !string.IsNullOrEmpty(dt.TableName))
                {
                    nodeName = dt.TableName;//去表名作为节点名称
                }
                else {
                    nodeName = nodeValue.GetType().Name;//取类型作为节点的名称
                }
                
            }

            tw.WriteLine(string.Format("<{0}>", nodeName));

            if (dt != null && dt.Rows.Count > 0)
            { 
                //DataTable里面有数据
                DataColumnCollection columns = dt.Columns;//取列名
                for (int i = 0, count = dt.Rows.Count; i < count; i++)
                {
                    tw.WriteLine(string.Format("<{0}>", "row"));//添加行标志
                    //遍历行
                    foreach (DataColumn dc in columns)
                    { 
                        //遍历单元格
                        //遍历 进行单个的序列化
                        XmlSerializerContext context = new XmlSerializerContext(tw, columns[dc.ColumnName].DataType);
                        context.Serialize(dc.ColumnName, dt.Rows[i][dc.ColumnName]);
                    }
                    tw.WriteLine(string.Format("</{0}>", "row"));
                }
            }
            
            tw.WriteLine(string.Format("</{0}>", nodeName));
        }
    }
}
