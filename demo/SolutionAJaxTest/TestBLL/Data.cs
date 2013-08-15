using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using AjaxFramework;

using LitJson;

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
        [WebMethodAttr(CurRequestType=RequestType.All,CurContentType=ContentType.XML)]
        [WebParameterAttr("a", typeof(float), MinValue = 5)]
        [WebParameterAttr("b",typeof(float),RegexText=@"^[0-9]{1,3}[\.][0-9]{1,3}$",ErrorMsg="参数b必须是小数")]
        public float Add(float a, float b)
        {
            
            
            //object obj2 = System.Web.HttpContext.Current.Session;
            return a + b;
        }

        [WebMethodAttr(ContentType.HTML)]
        public int test(BatchJson<User> batch)
        {
            //throw new ArgumentNullException("异常啊'\"\n");
            id++;
            return id;
        }

        /// <summary>
        /// 这个方法只有Post请求才可以
        /// </summary>
        /// <returns></returns>
        [WebMethodAttr(CurRequestType = RequestType.Get,  CurContentType = ContentType.IMAGE)]
        [OutputCacheAttr(20)]
        public byte[] Get_Pat(HttpPostedFile file)
        {
            return new byte[1024];
            //return "pat STATIC"+DateTime.Now;
        }

        /// <summary>
        /// 返回普通的字符串 会加上一个json的外壳
        /// </summary>
        /// <returns></returns>
        [WebMethodAttr(RequestType.All,ContentType.XML)]
        public IDictionary<string,object> Get_Pat2()
        {
            IDictionary<string, object> idict = new Dictionary<string, object>();
            idict.Add("flag", "0");
            idict.Add("errorMsg", "账号或者密码错误!");
            return idict;
        }

        /// <summary>
        /// 返回DataTable的数据
        /// </summary>
        /// <returns></returns>
        [WebMethodAttr(CurRequestType = RequestType.Get, CurContentType = ContentType.XML)]
        public JsonData Get_Data(HttpRequestDescription http)
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
            IDictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("name", "tom");
            dict.Add("cls", dt);

            string json = JsonMapper.ToJson(dict);
            //return dt;
            JsonData jd = JsonMapper.ToObject(json);
            return jd;
            /*return new JsonpResult()
            {
                JsonpKey = "1235",
                JsonpData = dt
            };*/
        }

        /// <summary>
        /// 这个方法是用来测试传实体的
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [WebMethodAttr(CurRequestType = RequestType.Get,CurContentType=ContentType.XML)]
        public User Insert_User(User user)
        {
            return user;
        }

        /// <summary>
        /// 这个方法是用来检测传泛型的
        /// </summary>
        /// <param name="list"></param>
         [WebMethodAttr(CurRequestType = RequestType.All, CurContentType = ContentType.JSON)]
        public void SaveUser(List<User> list)
        { 
            
        }

        public void Dispose()
        {

        }
        
    }
}
