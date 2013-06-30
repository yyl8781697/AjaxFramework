using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using LitJson;

namespace Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string json = @"[
            {
                ""name""    : ""www.87cool.com"",
                ""Age""      : 3,
                ""Birthday"" : ""07/17/2007 00:00:00""
            }]";
            IList<Person> thomas = JsonMapper.ToObject<List<Person>>(json);

            
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime Birthday { get; set; }
        }



    }




}