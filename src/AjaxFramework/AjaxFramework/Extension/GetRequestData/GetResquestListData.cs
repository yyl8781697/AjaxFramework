using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using LitJson;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 得到简单json数组的值 在后台即表现为List<T> 泛型  尽量少用，这里反射比较好 效率会低
    /// </summary>
    internal class GetResquestListData:GetRequestDataStrategy
    {
        #region 属性
        /// <summary>
        /// 添加方法的缓存
        /// </summary>
        private static Dictionary<Type, MethodInfo> _dictAddMethod = new Dictionary<Type, MethodInfo>();
        /// <summary>
        /// 添加方法的名称
        /// </summary>
        private const string ADD_METHOD_NAME = "Add";
        #endregion

        #region 判断是否是匹配类型
        /// <summary>
        /// 是否是匹配类型 
        /// </summary>
        /// <param name="paramType">所需判断的类型</param>
        /// <returns>如果类型为List<T>格式，则为Json数组格式</returns>
        public override bool IsMatchType(Type paramType)
        {
            base.IsMatchType(paramType);
            return typeof(IList<>).Name.Equals(paramType.Name) || typeof(List<>).Name.Equals(paramType.Name);
        }
        #endregion

        #region  取得批量Josn的值
        /// <summary>
        /// 取得相应的值
        /// </summary>
        /// <param name="paramName">当前参数的名称</param>
        /// <param name="paramType">当前参数的类型</param>
        /// <param name="currentHttpRequest">当前的请求详情</param>
        /// <returns></returns>
        public override object GetValue(string paramName, Type paramType, HttpRequestDescription currentHttpRequest)
        {
            base.GetValue(paramName, paramType, currentHttpRequest);

            string jsonList = currentHttpRequest.WebParameters[paramName];
            Type type = paramType.GetGenericArguments()[0];
            if (string.IsNullOrEmpty(jsonList))
            {
                return null;
            }

            PropertyInfo[] propertys = type.GetPropertyInfos(GetRequestDataStrategy.bindingFlags);//反射该类型的属性

            var list = typeof(List<>).MakeGenericType(type).CreateInstace();//动态创建所指定的泛型类型
            MethodInfo addMethod = GetAddMethodInfo(typeof(List<>).MakeGenericType(type));//得到List<T>的添加方法

            JsonData jsonData = JsonMapper.ToObject(jsonList);//反序列化Json数据得到JsonData
            
            for (int i = 0, count = jsonData.Count; i < count; i++)
            {
                //如果反序列化得到的数据位object 型，在litJson中为Dict型
                if (jsonData[0].IsObject)
                {
                    //将该类型指定为一个新字典
                    IDictionary<string, JsonData> newDict = jsonData[i].Inst_Object;

                    #region 取得实体类的值
                    object t = type.CreateInstace();//动态创建实体的实例
                    for (int j = 0; j < propertys.Length; j++)
                    {
                        if (newDict.Keys.Contains(propertys[j].Name))
                        {
                            //从键值对里面得到值
                            string val = Convert.ToString(newDict[propertys[j].Name]);

                            if (propertys[j].PropertyType.IsSampleType() && propertys[j].CanWrite)
                            {
                                //如果是简单的类型 并且为可写  就将值进行转换
                                object obj = propertys[j].PropertyType.ConvertSampleTypeValue(val);
                                propertys[j].SetValue(t, obj, null);
                            }
                            //不是  还不是简单地类型 就不再进行操作 直接舍弃
                        }
                    }
                    #endregion
                    //执行add的方法  将实体方法泛型类中
                    addMethod.Invoke(list,new object[]{ t});
                }
            }

            return list;
        }
        #endregion

        #region 得到List<T>添加的方法
        /// <summary>
        /// 得到List<T>添添加的方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private MethodInfo GetAddMethodInfo(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            MethodInfo method;
            if (_dictAddMethod.ContainsKey(type))
            {
                //先尝试从缓存中取
                method = _dictAddMethod[type];
            }
            else
            {
                method = type.GetMethod(ADD_METHOD_NAME);
                if (method == null)
                {
                    //如果反射取方法失败 则抛异常
                    throw new AmbiguousMatchException("add method is not exist!");
                }
                //将刚刚反射得到方法添加进缓存
                _dictAddMethod.Add(type, method);
            }
            return method;
        }
        #endregion
    }
}
