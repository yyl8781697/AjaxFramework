using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace AjaxFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public delegate object CtorDelegate();


    /// <summary>
    /// 反射的帮助类
    /// </summary>
    internal static class ReflectionHelper
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

        #region 得到方法的最基本的信息
        /// <summary>
        /// 得到方法的最基本的信息
        /// </summary>
        /// <returns></returns>
        public static CustomMethodInfo GetMethodBaseInfo(MethodPathInfo methodPathInfo, BindingFlags bindingAttr)
        {
            CustomMethodInfo customMethodInfo = new CustomMethodInfo();
            try
            {
                //得到程序集
                customMethodInfo.Assembly = Assembly.Load(methodPathInfo.Assembly);
                //得到类的类型
                Type t = customMethodInfo.Assembly.GetType(methodPathInfo.Assembly + "." + methodPathInfo.ClassName, true, true);
                //得到类的类型
                customMethodInfo.ClassType = t;
                //得到方法
                customMethodInfo.Method = t.GetMethod(methodPathInfo.MethodName, bindingAttr);
                if (customMethodInfo.Method == null)
                {
                    //没有得到方法 把整个信息置空
                    customMethodInfo = null;
                }
            }
            catch
            {
                throw;
            }
            return customMethodInfo;
        }
        #endregion

        #region 到属性 里面有缓存处理
        /// <summary>
        /// 缓存 属性
        /// </summary>
        private static IDictionary<Type, PropertyInfo[]> _dictPropertyInfo = new Dictionary<Type, PropertyInfo[]>(1024);

        /// <summary>
        /// 得到属性 里面有缓存处理
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertyInfos(this Type instanceType,BindingFlags bindingFlags)
        {
            if (instanceType == null)
                throw new ArgumentNullException("instanceType");
            PropertyInfo[] propertyInfos;
            if (_dictPropertyInfo.ContainsKey(instanceType))
            {
                propertyInfos = _dictPropertyInfo[instanceType];
            }
            else {
                propertyInfos = instanceType.GetProperties(bindingFlags);
                _dictPropertyInfo.Add(instanceType, propertyInfos);
            }
            return propertyInfos;
        }
        #endregion

        #region 动态创建实例相关
        /// <summary>
        /// 缓存
        /// </summary>
        private static IDictionary<string, CtorDelegate> _dictCtor = new Dictionary<string, CtorDelegate>(1024);

        /// <summary>
        /// 创建一个实例   参考http://www.cnblogs.com/fish-li/ 博客的mymvc框架的设计
        /// </summary>
        /// <param name="instanceType">实例的类型</param>
        /// <returns></returns>
        public static object CreateInstace(this Type instanceType)
        { 
            if( instanceType == null )
				throw new ArgumentNullException("instanceType");

            CtorDelegate ctor;
            if(_dictCtor.Keys.Contains(instanceType.FullName))
            {
                //先尝试从缓存中取数据
                ctor = _dictCtor[instanceType.FullName];
            }else{
				ConstructorInfo ctorInfo = instanceType.GetConstructor(Type.EmptyTypes);
				ctor = CreateConstructor(ctorInfo);
                _dictCtor[instanceType.FullName] = ctor;
			}

			return ctor();
        }
        
        /// <summary>
        /// 创建构造函数 空参数构造函数
        /// </summary>
        /// <param name="constructor"></param>
        /// <returns></returns>
        public static CtorDelegate CreateConstructor(ConstructorInfo constructor)
        {
            if (constructor == null)
                throw new ArgumentNullException("constructor");
            if (constructor.GetParameters().Length > 0)
                throw new NotSupportedException("不支持有参数的构造函数。");

            DynamicMethod dm = new DynamicMethod(
                "ctor",
                constructor.DeclaringType,
                Type.EmptyTypes,
                true);

            ILGenerator il = dm.GetILGenerator();
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);

            return (CtorDelegate)dm.CreateDelegate(typeof(CtorDelegate));
        }

        #endregion

       


    }
}
