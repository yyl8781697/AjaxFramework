using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 网络参数的特性 能够进行一些通用性的验证
    /// PriorityLevel = 100
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class WebParameterAttr : ValidateAttr
    {
        #region 属性
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 参数类型 必填 否则不会进行验证 此处仅对Int(Int16,Int32,Int64),String,DateTime有效
        /// 这里需要用typeof(int,string)来赋值
        /// 多精度类型建议用字符串String加正则表达式来进行验证，不过这里也支持float  decmail的类型
        /// 详情去看CheckDataContext类
        /// </summary>
        public Type ParaType { get; set; }

        /// <summary>
        /// 验证失败时的错误信息
        /// </summary>
        public string ErrorMsg { get; set; }

        private bool _nullable = false;
        /// <summary>
        /// 是否允许空值 默认为false 表示不允许
        /// </summary>
        public bool Nullable
        {
            get
            {
                return this._nullable;
            }
            set
            {
                this._nullable = value;
            }
        }

        /// <summary>
        /// 正则的文本规则 默认空 表示不验证
        /// </summary>
        public string RegexText { get; set; }

        private double _minValue = -1;
        /// <summary>
        /// 最小值 仅针对Int类型 默认为-1 表示不验证
        /// </summary>
        public double MinValue
        {
            get
            {
                return this._minValue;
            }
            set
            {
                this._minValue = value;
            }
        }

        private double maxValue = -1;
        /// <summary>
        /// 最大值 仅针对Int类型 默认为-1 表示不验证
        /// </summary>
        public double MaxValue
        {
            get
            {
                return this.maxValue;
            }
            set
            {
                this.maxValue = value;
            }
        }

        private int _maxLength = -1;
        /// <summary>
        /// 最大长度 仅针对String类型 一个汉字算两个字节  默认为-1 表示不验证
        /// </summary>
        public int MaxLength
        {
            get
            {
                return this._maxLength;
            }
            set
            {
                this._maxLength = value;
            }
        }

        private int _minLength = -1;
        /// <summary>
        /// 最小长度 仅针对String类型 一个汉字算两个字节  默认为-1 表示不验证
        /// </summary>
        public int MinLength
        {
            get
            {
                return this._minLength;
            }
            set
            {
                this._minLength = value;
            }
        }


        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">参数的名称</param>
        /// <param name="paraType">参数类型 这里只能是简单的类型 比如int,string,float,datetime之类的，用typeof(xx)来传值</param>
        public WebParameterAttr(string name,Type paraType)
        {
            this.Name = name;
            this.ParaType = paraType;
            this.PriorityLevel = 100;
        }

        #region 验证特征的有效性
        /// <summary>
        /// 验证特征的有效性
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">参数异常</exception>
        public override bool IsValidate()
        {
            base.IsValidate();
            //判断传参的名称和类型是否都存在
            if (string.IsNullOrEmpty(this.Name) || this.ParaType==null)
            {
                throw new ArgumentNullException("parameter name or type is null");
            }
            this.Value = base.CurHttpRequest.WebParameters[this.Name] == null ? "" : base.CurHttpRequest.WebParameters[this.Name];
            CheckDataContext dataContext=new CheckDataContext(this);
            return dataContext.CheckData();
        }
        #endregion

       

    }
}
