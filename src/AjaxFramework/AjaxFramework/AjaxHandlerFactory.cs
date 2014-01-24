using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AjaxFramework
{
    public class AjaxHandlerFactory : IHttpHandlerFactory
    {
      
    
        public IHttpHandler GetHandler(HttpContext context,
                        string requestType, string virtualPath, string physicalPath)
        {
            return new ResponseHandler(context.Request.AppRelativeCurrentExecutionFilePath);
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}
