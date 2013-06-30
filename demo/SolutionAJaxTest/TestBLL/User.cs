using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBLL
{
    /// <summary>
    /// 用户的实体信息
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 账号密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public System.Decimal Age { get; set; }

        public PeopleSex Sex { get; set; } 

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
    }

    public enum PeopleSex
    { 
        UnKnow=0,
        Female=1,
        Male=2
    }
}
