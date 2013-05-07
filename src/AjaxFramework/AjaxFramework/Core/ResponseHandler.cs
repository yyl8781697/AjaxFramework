using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace AjaxFramework
{
    internal class ResponseHandler : IHttpHandler,IRequiresSessionState
    {
        /// <summary>
        /// 需要执行方法的一些路径信息
        /// </summary>
        public MethodPathInfo CurrentMethodPathInfo { get; private set; }

        /// <summary>
        /// 构造方法  主要是初始化请求方法的一些路径信息 比如 空间 类  方法名
        /// </summary>
        ///<param name="virtualPath">请求的一个虚拟路径</param>
        public ResponseHandler(string virtualPath)
        {
            //
            this.CurrentMethodPathInfo = UrlConfig.GetMethodPathInfo(virtualPath);  
        }

        /// <summary>
        /// Handler的入口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            //暂时都是输出json格式
            context.Response.ContentType = "application/json";
            
            string output = "";
            try
            {
                //实例化当前请求的方法帮助类
                MethodHelper methodHelper = new MethodHelper(context, this.CurrentMethodPathInfo);
                //得到自定义方法的一些详细信息
                CustomMethodInfo customMethodInfo = methodHelper.GetMethod();

                if (customMethodInfo != null)
                {
                    //用帮助类执行该方法
                    object ret = methodHelper.ExecMethod(customMethodInfo);
                    //得到需要输出的字符串
                    output = ResponseHelper.GetResponseString(ret, customMethodInfo.RetureType);
                }
                else
                {
                    output = ResponseHelper.ResponseError("");
                }
            }
            catch (ArgumentException args)
            {
                output = ResponseHelper.ResponseError(args.Message);
            }
            catch (AjaxException ajaxErr)
            {
                output = ResponseHelper.ResponseError(ajaxErr.Message);
            }
            context.Response.Write(output);
        }

        public bool IsReusable { get { return false; } }
    }
}
