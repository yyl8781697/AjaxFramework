using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AjaxFramework
{
    /// <summary>
    /// 输出数据的上下文
    /// </summary>
    internal class ResponseDataContext
    {
        /// <summary>
        /// 输出数据的策略
        /// </summary>
        private ResponseDataStrategy _strategy;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">请求的上下文</param>
        public ResponseDataContext(HttpContext context)
        {
            this.InitStrategy(context.Response.ContentType);
            this._strategy.CurrentContext = context;
        }

        /// <summary>
        /// 初始化策略
        /// </summary>
        /// <param name="contentType">输出文档类型</param>
        private void InitStrategy(string contentType)
        {
            //输出Json格式
            if (ContentType.JSON.Equals(contentType, StringComparison.OrdinalIgnoreCase))
            {
                this._strategy = ResponseJson.GetInstance();
                return;
            }

            //输出Xml格式
            if (ContentType.XML.Equals(contentType, StringComparison.OrdinalIgnoreCase))
            {
                this._strategy = ResponseXml.GetInstance();
                return;
            }

            //表示输出图片或者文件格式的
            if (contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) || contentType.IndexOf("octet-stream", StringComparison.OrdinalIgnoreCase) > 0)
            {
                this._strategy = ResponseFile.GetInstance();
                return;
            }

            //其余 只需直接输出文本均可
            this._strategy = ResponseString.GetInstance();
        }

        /// <summary>
        /// 得到输出的数据
        /// </summary>
        /// <param name="obj">要输出的值</param>
        /// <param name="type">要输出的类型</param>
        /// <returns>返回对应类型的字符串</returns>
        public string GetResponse(object obj, Type type)
        {
            return _strategy.GetResponse(obj, type);
        }

        /// <summary>
        /// 得到输出的数据
        /// </summary>
        /// <param name="str">要输出的字符串</param>
        public string GetResponse(string str)
        {
            AjaxResult ajaxResult = new AjaxResult()
            {
                Flag = "0",
                ErrorMsg = str
            };
            return _strategy.GetResponse(ajaxResult, typeof(AjaxResult));
        }
    }
}
