using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 缓存特性  这里的缓存是缓存在客户端 PriorityLevel=9990
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class OutputCacheAttr:ValidateAttr
    {
        /// <summary>
        /// 需要缓存的时间(秒)
        /// </summary>
        public int _cacheSecond;
        /// <summary>
        /// 相应修改标志 头部的键值
        /// </summary>
        private const string CACHE_KEY = "If-Modified-Since";

        /// <summary>
        /// 设置缓存时间 秒
        /// </summary>
        /// <param name="cacheSecond">需要缓存的时间(秒)</param>
        public OutputCacheAttr(int cacheSecond)
        {
            this._cacheSecond = cacheSecond;
            //设置优先级 仅仅低于webattribute特性
            base.PriorityLevel = 9990;
        }

        /// <summary>
        /// 检查是否有缓存 如果有缓存标志 直接输出缓存 并且下面的代码将不再运行
        /// </summary>
        /// <returns></returns>
        public override bool IsValidate()
        {
            base.IsValidate();
            
            //检查当前是否有缓存标志
            if (base.CurHttpRequest.Context.Request.Headers.AllKeys.Contains(CACHE_KEY))
            {
                DateTime dt = Convert.ToDateTime(base.CurHttpRequest.Context.Request.Headers[CACHE_KEY]);
                //判断是否在缓存时间内
                if ((DateTime.Now - dt).TotalSeconds < this._cacheSecond)
                {
                    //在缓存时间里面  这里会输出304的头部 并且停止页面的输出
                    //通知客户端以缓存输出
                    base.CurHttpRequest.Context.Response.StatusCode = 304;
                    base.CurHttpRequest.Context.Response.End();
                    return true;
                }
            }

            // 设置最近修改的响应头Last-Modified，客户端将会发送If-Modified-Since到服务器端
            // 配合上面代码实现缓存
            base.CurHttpRequest.Context.Response.Cache.SetLastModified(DateTime.Now);

            return true;
        }
    }
}
