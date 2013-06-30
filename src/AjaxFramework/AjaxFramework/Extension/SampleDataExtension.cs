using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AjaxFramework
{
    /// <summary>
    /// 常见的数据类型的转换
    /// </summary>
    internal static class SampleDataExtension
    {
        /// <summary>
        /// 数据类型转换委托
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public delegate object ConvertAction(string val);

        #region 简单的数据类型字典
        /// <summary>
        /// 简单的数据类型字典 key=类型  value=该类型的string=>type的转换方法
        /// </summary>
        private static Dictionary<Type, ConvertAction> _sampleTypeDict = new Dictionary<Type, ConvertAction>(){
            {typeof(int),delegate(string val){ return string.IsNullOrEmpty(val)?0:Convert.ToInt32(val);}},
            {typeof(Int32?),delegate(string val){return string.IsNullOrEmpty(val)?null:val.ConvertTo<Int32?>();}},
            {typeof(Int64),delegate(string val){return string.IsNullOrEmpty(val)?0:Convert.ToInt64(val);}},
            {typeof(Int64?),delegate(string val){return string.IsNullOrEmpty(val)?null:val.ConvertTo<Int64?>();}},
            {typeof(string),delegate(string val){return Convert.ToString(val);}},
            {typeof(decimal),delegate(string val){return Convert.ToDecimal(val);}},
            {typeof(float),delegate(string val){return Convert.ToSingle(val);}},
            {typeof(double),delegate(string val){return Convert.ToDouble(val);}},
            {typeof(DateTime),delegate(string val){
                                        DateTime dt;
                                        DateTime.TryParse(val, out dt);
                                        return  dt;}
            },
            {typeof(DateTime?),delegate(string val){
                                        DateTime dt;
                                        if(DateTime.TryParse(val, out dt))
                                            return  dt;
                                        else
                                            return null;}
            },
            {typeof(bool),new ConvertAction(GetBoolean)},
            {typeof(void),delegate(string val){return null;}},
            {typeof(object),delegate(string val){return val;}}
            
        };

        /// <summary>
        /// 简单的数据类型字典 key=类型  value=该类型的string=>type的转换方法
        /// </summary>
        public static Dictionary<Type, ConvertAction> SampleTypeDict
        {
            get
            {
                return _sampleTypeDict;
            }
        }
        #endregion


        #region 根据字符串得到布尔值
        /// <summary>
        /// 根据字符串得到布尔值 true 正数 均表示为布尔值
        /// </summary>
        /// <param name="val">需要判断的字符串</param>
        /// <returns></returns>
        private static object GetBoolean(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                //如果是空值  直接是非
                return false;
            }
            else
            {
                if ("true".Equals(val, StringComparison.OrdinalIgnoreCase))
                {
                    //表示用true字符串来表示 就直接为 真
                    return true;
                }
                else
                {
                    if (Regex.IsMatch(val, @"\d+"))
                    {
                        //到这一步可以判断为数字类型 大于0 即为true
                        return Convert.ToInt32(val) > 0;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        #endregion



        #region 是否是简单的数据类型 即系统的一些常见的数据类型
        /// <summary>
        /// 是否是简单的数据类型 即系统的一些常见的数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSampleType(this Type type)
        {
            return SampleTypeDict.Keys.Contains(type);
        }
        #endregion

        #region 转换简单的数据类型的值
        /// <summary>
        /// 转换简单的数据类型的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object ConvertSampleTypeValue(this Type type, string val)
        {
            object obj = null;
            if (type.IsSampleType())
            {
                //直接用数据类型字典里面自定义的类型转换方法进行数据类型转换
                obj = SampleTypeDict[type](val);
            }
            return obj;
        }
        #endregion

        public static T ConvertTo<T>(this IConvertible convertibleValue)
        {
            if (null == convertibleValue)
            {
                return default(T);
            }
            if (!typeof(T).IsGenericType)
            {
                return (T)Convert.ChangeType(convertibleValue, typeof(T));
            }
            else
            {
                Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return (T)Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(typeof(T)));
                }
            }
            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, typeof(T).FullName));
        }
    }
}
