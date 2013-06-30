using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AjaxFramework
{
    /// <summary>
    /// 特性的抽象基类
    /// </summary>
    public abstract class ValidateAttr:Attribute,IComparable
    {
        /// <summary>
        /// 当前的请求的详细信息
        /// </summary>
        public HttpRequestDescription CurHttpRequest { get; set; }

        /// <summary>
        /// 优先等级 数字越大等级越高
        /// </summary>
        public int PriorityLevel { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ValidateAttr(){
            //初始化优先级为0
            this.PriorityLevel = 0;
        }

        /// <summary>
        /// 验证方法是否通过
        /// </summary>
        /// <returns>验证通过返回true，否则，返回false</returns>
        public virtual bool IsValidate()
        {
            if (this.CurHttpRequest.Context == null)
            {
                throw new AjaxException("没有得到当前的上下文");
            }
            return true;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            ValidateAttr validateAttr = obj as ValidateAttr;
            if (validateAttr != null)
            {
                return validateAttr.PriorityLevel.CompareTo(this.PriorityLevel);
            }
            throw new NotImplementedException("obj is not ValidateAttr");
        }
    }

}
