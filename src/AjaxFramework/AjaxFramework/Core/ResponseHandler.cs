using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace AjaxFramework
{
    internal class ResponseHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 需要执行方法的一些路径信息
        /// </summary>
        public MethodPathInfo CurrentMethodPathInfo { get; private set; }

        private ResponseDataContext reponseContext;

        /// <summary>
        /// 构造方法  主要是初始化请求方法的一些路径信息 比如 空间 类  方法名
        /// </summary>
        ///<param name="virtualPath">请求的一个虚拟路径</param>
        public ResponseHandler(string virtualPath)
        {
            this.CurrentMethodPathInfo = UrlConfig.GetMethodPathInfo(virtualPath);

        }

        /// <summary>
        /// Handler的入口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            //指定当前的文档内容 默认Json
            string contentType = ContentType.JSON;
            string output = string.Empty;
            //初始化当前输出的上下文 默认Json
            this.reponseContext = new ResponseDataContext(contentType);

            try
            {
                //实例化当前请求的方法帮助类
                MethodHelper methodHelper = new MethodHelper(context, this.CurrentMethodPathInfo);
                //初始化方法
                methodHelper.InitMethod();
                //更新要输出的类型
                contentType = methodHelper.CurCustomMethodInfo.CurWebMethodAttr.CurContentType;
                //再次初始化当前需要输出的上下文
                this.reponseContext = new ResponseDataContext(contentType);

                if (methodHelper.CheckAttribute())
                {
                    //用帮助类执行该方法
                    object ret = methodHelper.ExecMethod();
                    //得到需要输出的字符串
                    output = this.reponseContext.GetResponse(ret, methodHelper.CurCustomMethodInfo.RetureType);
                }
                
            }
            catch (ArgumentException args)
            {
                //捕获参数异常
                output = this.reponseContext.GetResponse(args.Message);
            }
            catch (Ajax404Exception ajax404)
            {
                //捕获404
                context.Response.StatusCode = 404;
                output = this.reponseContext.GetResponse(ajax404.Message);
            }
            catch (AjaxException ajaxErr)
            {
                //捕获自定义异常
                output = this.reponseContext.GetResponse(ajaxErr.Message);
            }
            catch (Exception exError)
            {
                //捕获全部异常
                output = this.reponseContext.GetResponse(exError.Message);
            }

            context.Response.ContentType = contentType;
            context.Response.Write(output);
        }

        public bool IsReusable { get { return false; } }


    }
}
