using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ToCamel("NAME", "_");

            Regex regex = new Regex(@"(([0-1]?[0-9])|([2][0-4]):[0-5]?[0-9][,]?)");
            
        }

        public string ToCamel(string str, string separator)
        {
            Regex regex = new Regex(string.Format(@"({0}?[A-Za-z0-9]+)",separator));
            str = regex.Replace(str, (m) => {
                string ret="";
                if (m.Value.StartsWith(separator))
                { 
                    //如果是以分隔符开始的
                    ret=m.Value.Substring(1).ToLower();//把分割符截取掉 并将整个字符串转为小写
                    ret=ret.Substring(0,1).ToUpper()+ret.Substring(1);//将首字符转为大写
                }else{
                    ret=m.Value.ToLower();
                }
                return ret;
                
            });
            return str;
        }


    }




}