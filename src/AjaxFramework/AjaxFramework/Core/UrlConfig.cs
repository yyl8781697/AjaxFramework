using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

namespace AjaxFramework
{
    /// <summary>
    /// URL路径的一个配置
    /// </summary>
    internal class UrlConfig
    {

        public static readonly string REGEX_TEXT = @"^[~]?/(?<classname>\w{1,30})[\.|/](?<methodname>\w{1,30})[\.](\w{1,6})$";

        /// <summary>
        /// ajax执行的业务类所在的程序集 默认自己的 有一个测试的
        /// </summary>
        public static readonly string ASSEMBLY = "AjaxFramework";

        static UrlConfig()
        {
            string assembly = ConfigurationManager.AppSettings["AjaxFramewok"];
            if (string.IsNullOrEmpty(assembly))
            {
                throw new ArgumentException("assembly is null");
            }
            ASSEMBLY = assembly;

        }

        public UrlConfig()
        {

        }

        

        /// <summary>
        /// 得到方法的一些基本的路径信息
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static MethodPathInfo GetMethodPathInfo(string virtualPath)
        {
            MethodPathInfo methodPathInfo = null;


            //正则判断
            Match match = Regex.Match(virtualPath, REGEX_TEXT, RegexOptions.IgnoreCase);
            //如果匹配到了
            if (match.Success)
            {
                //取出class和method
                methodPathInfo = new MethodPathInfo();
                methodPathInfo.ClassName = match.Groups["classname"].Value;
                methodPathInfo.MethodName = match.Groups["methodname"].Value;
                methodPathInfo.Assembly = ASSEMBLY;
                
            }
            return methodPathInfo;

        }
    }
}
