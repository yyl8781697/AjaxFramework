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

        int id = 1;

        public Data()
        { 
            
        }

        public Data(int a)
        { 
            
        }

        /// <summary>
        /// 这里有参数的验证 a的最小值为5  b有正则的规定
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [WebMethodAttr(CurRequestType=RequestType.All)]
        [WebParameterAttr("a", typeof(float), MinValue = 5)]
        [WebParameterAttr("b",typeof(float),RegexText=@"^[0-9]{1,3}[\.][0-9]{1,3}$",ErrorMsg="参数b必须是小数")]
        public float Add(float a, float b)
        {
            
            
            //object obj2 = System.Web.HttpContext.Current.Session;
            return a + b;
        }

        [WebMethodAttr(CurRequestType=RequestType.All)]
        public int test(BatchJson<User> batch)
        {
            throw new ArgumentNullException("异常啊'\"\n");
            id++;
            return id;
        }

        /// <summary>
        /// 这个方法只有Post请求才可以
        /// </summary>
        /// <returns></returns>
        [WebMethodAttr(CurRequestType = RequestType.Get,  CurContentType = ContentType.HTML)]
        [OutputCacheAttr(20)]
        public string Get_Pat(HttpPostedFile file)
        {
            
            return "pat STATIC"+DateTime.Now;
        }

        /// <summary>
        /// 返回普通的字符串 会加上一个json的外壳
        /// </summary>
        /// <returns></returns>
        [WebMethodAttr(RequestType.All)]
        [OutputCacheAttr(20)]
        public int Get_Pat2()
        {
            id++;
            return id;
        }

        /// <summary>
        /// 返回DataTable的数据
        /// </summary>
        /// <returns></returns>
        [WebMethodAttr(CurRequestType = RequestType.Get, CurContentType = ContentType.JSON)]
        public DataTable Get_Data()
        {
            id++;
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("USER_ID");
            dt.Columns.Add("USER_NAME_");

            DataRow row = dt.NewRow();
            row["USER_ID"] = 1;
            row["USER_NAME_"] = "tom";
            dt.Rows.Add(row);

            DataRow row2 = dt.NewRow();
            row2["USER_ID"] = 2;
            row2["USER_NAME_"] = "peter";
            dt.Rows.Add(row2);

            return dt;
        }

        /// <summary>
        /// 这个方法是用来测试传实体的
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [WebMethodAttr(CurRequestType = RequestType.Get,CurContentType=ContentType.JSON)]
        public User Insert_User(User user)
        {
            return user;
        }
        
    }
}
