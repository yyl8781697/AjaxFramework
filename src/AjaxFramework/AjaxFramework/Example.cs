using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    public class Example:IAjax
    {
        [WebMethodAttr(CurRequestType = RequestType.Get)]
        public int Add(int a, int b)
        {
            return a + b;
        }
        //rpc

        public int Test(BatchJson<int> batch)
        {
            
            return 1;
        }

        public void Dispose()
        { 
            
        }
    }
}
