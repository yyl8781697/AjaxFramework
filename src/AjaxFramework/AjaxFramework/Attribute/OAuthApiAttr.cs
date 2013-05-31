using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace AjaxFramework
{
    /// <summary>
    /// 检测权限
    /// </summary>
    /// <returns></returns>
    public delegate bool CheckAuthorationHandler(OAuthParams oAuthParams);

    /// <summary>
    /// OAuth认证之后 开放api的特性 
    /// PriorityLevel=8000
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class OAuthApiAttr : ValidateAttr
    {
        private const string KEY_ACCESS_TOKEN = "access_token";

        /// <summary>
        /// 想使用该特性 必须添加上此事件 不然无法进行检测
        /// </summary>
        public static event CheckAuthorationHandler CheckAuthoration;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OAuthApiAttr()
        {
            base.PriorityLevel = 8000;
        }

     

        /// <summary>
        /// 验证access_token是否通过 验证项：是否为空 是否存在 是否在有效期呢
        /// 验证method name 即方法名 该方法是否包含在scope调用权限中
        /// </summary>
        /// <returns>验证通过返回true，否则，返回false</returns>
        public override bool IsValidate()
        {
            base.IsValidate();
            if (string.IsNullOrEmpty(base.CurHttpRequest.Context.Request[KEY_ACCESS_TOKEN]))
            {
                throw new AjaxException("准许令牌access_token不能为空");
            }
            
            if (CheckAuthoration == null)
            {
                throw new AjaxException("权限验证方法为空");
            }

            OAuthParams oAuthParams=new OAuthParams(){
                AccessToken = base.CurHttpRequest.WebParameters[KEY_ACCESS_TOKEN],
             MethodName=base.CurHttpRequest.CurrentMethodInfo.Method.Name.ToLower()
            };

            //执行检查参数事件
            if(CheckAuthoration(oAuthParams))
            {
                //验证通过
                NameValueCollection webParameters = new NameValueCollection(base.CurHttpRequest.WebParameters);
                webParameters.Add("username", oAuthParams.UserName);
                base.CurHttpRequest.WebParameters = webParameters;
                return true;
            }else{
                return false;
            }

        }
    }

    public class OAuthParams
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户名  需要反写
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 调用方法
        /// </summary>
        public string MethodName { get; set; }
    }
}
