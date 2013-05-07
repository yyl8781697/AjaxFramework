using AjaxFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBLL
{
    /// <summary>
    /// 这个当为类似为微博QQ之类的开发接口的时候特制的
    /// </summary>
    public class OAuthBase
    {
        /// <summary>
        /// 静态构造函数 在这里添加OAUTH认证事件
        /// </summary>
        static OAuthBase()
        {
            OAuthApiAttr.CheckAuthoration -= OpenApiAttr_CheckAuthoration;
            OAuthApiAttr.CheckAuthoration += OpenApiAttr_CheckAuthoration;
        }

        /// <summary>
        /// 相对于OAuth方法的  认证事件
        /// </summary>
        /// <param name="oAuthParams"></param>
        /// <returns></returns>
        private static bool OpenApiAttr_CheckAuthoration(OAuthParams oAuthParams)
        {
            

            //在这里还可以检测 该token的访问频率 IP之类的 可以作用到限流量 写LOG
           

            oAuthParams.UserName = "admin";
            return true;
        }
    }
}
