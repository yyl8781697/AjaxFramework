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
            context.Response.ContentType =  ContentType.JSON;

            #region 参数变量的申明
            string output = string.Empty;
            string errorMsg = string.Empty;//错误的信息
            DebugeLog log=new DebugeLog();
            #endregion

            try
            {
                #region 调用Aajax的方法

                //实例化当前请求的方法帮助类
                MethodHelper methodHelper = new MethodHelper(context, this.CurrentMethodPathInfo);
                //初始化方法
                methodHelper.InitMethod();
                //更新要输出的类型
                context.Response.ContentType = methodHelper.CurCustomMethodInfo.CurWebMethodAttr.CurContentType;

                if (methodHelper.CheckAttribute())
                {
                    //用帮助类执行该方法
                    object ret = methodHelper.ExecMethod();
                    //得到需要输出的字符串
                    output = new ResponseDataContext(context).GetResponse(ret, methodHelper.CurCustomMethodInfo.RetureType);
                }
                #endregion

            }
            #region 错误信息的捕获
            catch (ArgumentException args)
            {
                log.Write(args);
                //捕获参数异常
                output = errorMsg = args.Message;
            }
            catch (Ajax404Exception ajax404)
            {
                log.Write(ajax404);
                //捕获404
                context.Response.StatusCode = 404;
                errorMsg = ajax404.Message;
            }
            catch (AjaxException ajaxErr)
            {
                log.Write(ajaxErr);
                //捕获自定义异常
                errorMsg = ajaxErr.Message;
            }
            catch (Exception exError)
            {
                log.Write(exError);
                //捕获全部异常
                errorMsg = exError.Message;
            }
            finally
            {
                //如果存在错误信息 则进行错误信息的获取
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    output = new ResponseDataContext(context).GetResponse(errorMsg);
                }
                log.Submit();
            }
            #endregion

            context.Response.Write(output);
        }

        public string GetOutput(HttpContext context, string output)
        {
            ResponseDataContext reponseContext = new ResponseDataContext(context);
            return reponseContext.GetResponse(output);
        }

        public bool IsReusable { get { return false; } }


    }
}
