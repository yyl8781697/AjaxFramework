using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    public class Example:IAjax
    {
        [WebMethodAttr(RequestType.Get)]
        public int Add(int a, int b)
        {
            return a + b;
        }
        //rpc
    }
}
