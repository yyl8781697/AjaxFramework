using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using AjaxFramework;

namespace TestBLL
{
    /// <summary>
    /// 一个测试的数据类
    /// </summary>
    public class Data : OAuthBase,IAjax
    {

        /// <summary>
        /// 这里有参数的验证 a的最小值为5  b有正则的规定
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [WebMethodAttr(RequestType.Get)]
        [WebParameterAttr("a", typeof(float), MinValue = 5)]
        [WebParameterAttr("b",typeof(float),RegexText=@"^[0-9]{1,3}[\.][0-9]{1,3}$",ErrorMsg="参数b必须是小数")]
        public float Add(float a, float b)
        {
            //object obj2 = System.Web.HttpContext.Current.Session;
            return a + b;
        }

        /// <summary>
        /// 这个方法只有Post请求才可以
        /// </summary>
        /// <returns></returns>
        [WebMethodAttr(RequestType.Post)]
        public string Get_Pat()
        {
            
            return "pat";
        }

        /// <summary>
        /// 返回普通的字符串 会加上一个json的外壳
        /// </summary>
        /// <returns></returns>
        [WebMethodAttr(RequestType.Get)]
        [OutputCacheAttr(20)]
        public string Get_Pat2()
        {
            return "pat" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// 返回DataTable的数据
        /// </summary>
        /// <returns></returns>
        [WebMethodAttr(RequestType.Get)]
        public DataTable Get_Data()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("id");
            dt.Columns.Add("name");

            DataRow row = dt.NewRow();
            row["id"] = 1;
            row["name"] = "tom";
            dt.Rows.Add(row);

            DataRow row2 = dt.NewRow();
            row2["id"] = 2;
            row2["name"] = "peter";
            dt.Rows.Add(row2);

            return dt;
        }

        /// <summary>
        /// 这个方法是用来测试传实体的
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [WebMethodAttr(RequestType.Get)]
        public User Insert_User(User user)
        {

            return user;
        }
        
    }
}
