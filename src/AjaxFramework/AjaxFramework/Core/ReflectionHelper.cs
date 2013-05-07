using System;
using System.Collections.Generic;
using System.Reflection;


namespace AjaxFramework
{
    /// <summary>
    /// 反射的帮助类
    /// </summary>
    internal class ReflectionHelper
    {
        #region 获取该方法的特性列表
        /// <summary>
        /// 获取该方法的特性列表 将会转换为指定的泛型列表
        /// </summary>
        /// <param name="methodInfo">方法的类型</param>
        /// <returns></returns>
        public static List<T> GetAttributes<T>(MethodInfo methodInfo) where T : class
        {
            //获取该方法全部的自定义属性
            object[] attrs = GetAttributes(methodInfo);
            List<T> attrList = new List<T>();
            //判断得到的特性是否可用
            if (attrs != null && attrs.Length > 0)
            {
                foreach (object obj in attrs)
                {
                    //转成指定的类型
                    T attr = obj as T;
                    //过滤掉非T的特性
                    if (attr != null)
                    {
                        attrList.Add(attr);
                    }
                }
            }
            //默认进行一个排序
            attrList.Sort();
            return attrList;
        }

        /// <summary>
        /// 获取该方法的特性列表 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static object[] GetAttributes(MethodInfo methodInfo)
        {
            object[] attrs = methodInfo.GetCustomAttributes(false);
            return attrs;
        }

        #endregion

        /// <summary>
        /// 得到类的实例类型
        /// </summary>
        /// <param name="assemlyString">程序集路径/命名空间</param>
        /// <param name="className">实例名称 不需要含程序集</param>
        /// <returns></returns>
        public static object GetClassType(string assemlyString,string className)
        {
            Type t = null;
            //加载指定程序集
            Assembly assembly=Assembly.Load(assemlyString);
            //得到该类的类型
            t = assembly.GetType(className);

            return t;
        }

        /// <summary>
        /// 得到方法实例
        /// </summary>
        /// <param name="assemlyString">程序集路径/命名空间</param>
        /// <param name="className">方法的实例</param>
        /// <param name="methodName">方法的名称</param>
        /// <param name="bindingAttr">该方法的条件</param>
        /// <returns></returns>
        public static MethodInfo GetMethod(string assemlyString, string className,string methodName,BindingFlags bindingAttr)
        {
            MethodInfo methodInfo=null;
            //加载指定程序集
            Assembly assembly = Assembly.Load(assemlyString);
            //得到该类的类型
            Type t = assembly.GetType(className);
            //得到指定的方法
            methodInfo = t.GetMethod(methodName, bindingAttr);

            return methodInfo;
        }

        /// <summary>
        /// 得到方法的实例
        /// </summary>
        /// <param name="t">方法所在类的类型</param>
        /// <param name="methodName">方法的名称</param>
        /// <param name="bindingAttr">获取方法的过滤条件</param>
        /// <returns></returns>
        public static MethodInfo GetMethod(Type t, string methodName, BindingFlags bindingAttr)
        {
            MethodInfo methodInfo = null;
            //得到指定的方法
            methodInfo = t.GetMethod(methodName, bindingAttr);

            return methodInfo;
        }

        




    }
}
