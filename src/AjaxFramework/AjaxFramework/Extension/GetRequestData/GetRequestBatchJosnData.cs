using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using LitJson;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 得到请求的批量Json数据 
    /// 这里的批量json中  是含有对json记录行标志位的 主要是针对用于miniui的grid删改
    /// 如果仅仅只是获得无标识位的json批量数据 可以使用GetRequestListnData
    /// </summary>
    class GetRequestBatchJosnData : GetRequestDataStrategy
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

        /// <summary>
        /// Json数据中标志位的键值
        /// </summary>
        private const string STATE_KEY = "_state";
        #endregion

        

        #region 判断是否是匹配类型
        /// <summary>
        /// 是否是匹配类型 
        /// </summary>
        /// <param name="paramType">所需判断的类型</param>
        /// <returns>如果类型为BatchJson<T>格式，则标志位json批量数据</returns>
        public override bool IsMatchType(Type paramType)
        {
            base.IsMatchType(paramType);
            return typeof(BatchJson<>).Name.Equals(paramType.Name);
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
            Type type = paramType.GetGenericArguments()[0];//得到泛型的具体类型
            string jsonList = currentHttpRequest.WebParameters[paramName];//得到json的传参
            if (string.IsNullOrEmpty(jsonList))
            {
                return null;
            }


            PropertyInfo[] propertys = type.GetPropertyInfos(GetRequestDataStrategy.bindingFlags);//反射该类型的属性

            var batchJson = typeof(BatchJson<>).MakeGenericType(type).CreateInstace();//动态创建所指定的泛型类型
            MethodInfo addMethod = GetAddMethodInfo(typeof(BatchJson<>).MakeGenericType(type));//得到batch泛型的添加方法

            JsonData jsonData = JsonMapper.ToObject(jsonList);//反序列化Json数组数据得到JsonData
            for (int i = 0, count = jsonData.Count; i < count; i++)
            {
                //如果反序列化得到的数据位object 型，在litJson中为Dict型
                if (jsonData[0].IsObject)
                {
                    //将该类型指定为一个新字典
                    IDictionary<string, JsonData> newDict = jsonData[i].Inst_Object;

                    string state = string.Empty;
                    if (newDict.ContainsKey(STATE_KEY))
                    {
                        //如果有state标志 取得该值
                        state = newDict["_state"].ToString();
                    }

                    #region 取得实体类的值
                    object t = type.CreateInstace();//动态创建实体的实例
                    for (int j = 0; j < propertys.Length; j++)
                    {
                        if (newDict.Keys.Contains(propertys[j].Name))
                        {
                            //从键值对里面得到值
                            string val = newDict[propertys[j].Name].ToString();

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
                    addMethod.Invoke(batchJson, new object[] { state, t });
                }
            }

            return batchJson;
        }
        #endregion

        #region 得到BatchJson添加的方法
        /// <summary>
        /// 得到BatchJson添加的方法
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
            else {
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
