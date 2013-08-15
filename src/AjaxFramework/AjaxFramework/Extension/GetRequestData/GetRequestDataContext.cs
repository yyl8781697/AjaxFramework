using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AjaxFramework.Extension.GetRequestData
{
    /// <summary>
    /// 得到请求数据的上下文
    /// </summary>
    public class GetRequestDataContext
    {
        #region 成员变量
        /// <summary>
        /// 当前的请求的详细信息
        /// </summary>
        private HttpRequestDescription _currentHttpRequest;


        /// <summary>
        /// 参数的名字
        /// </summary>
        private string _paramName;

        /// <summary>
        /// 参数的类型
        /// </summary>
        private Type _paramType;

        /// <summary>
        /// 取值策略变量
        /// </summary>
        private GetRequestDataStrategy _strategy;

        /// <summary>
        /// 策略缓存 在静态方法中初始化
        /// </summary>
        private static readonly List<GetRequestDataStrategy> _strategyCache;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="paramName">当前参数的名称</param>
        /// <param name="paramType">参数类型</param>
        /// <param name="currentHttpRequest">当前的详细请求</param>
        public GetRequestDataContext( string paramName, Type paramType,HttpRequestDescription currentHttpRequest)
        {
            if (currentHttpRequest == null)
            { 
                throw new ArgumentNullException("当前的请求信息不能为空");
            }

            if (paramType == null)
            {
                throw new ArgumentNullException("参数类型不能为空!");
            }

            this._paramName = paramName;
            this._paramType = paramType;
            this._currentHttpRequest = currentHttpRequest;
            InitStrategy(paramType);
        }
        #endregion

        #region 静态构造函数 初始化策略缓存
        /// <summary>
        /// 静态构造函数 初始化策略缓存
        /// </summary>
        static GetRequestDataContext()
        {
            _strategyCache = new List<GetRequestDataStrategy>(){
               new GetRequestSampleTypeData(),//最简单的数据类型
               new GetRequestEnumData(),//枚举类型
               new GetResquestListData(),//尽量少用 建议用其他方法来实现
               new GetRequestBatchJosnData(),//批量的Json类型
               new GetRequestFileData(),//文件类型
               new GetRequestHttpContext(),//获取当前请求上下文
               new GetRequestDescription(),//获取请求的详情
               new GetRequestEntityData()//实体类型
            };
        }
        #endregion

        #region 根据类型初始化取值策略
        /// <summary>
        /// 根据类型初始化取值策略
        /// </summary>
        /// <param name="type"></param>
        private void InitStrategy(Type type)
        { 
            //遍历策略缓存 查找符合类型的处理类
            foreach(GetRequestDataStrategy strategy in _strategyCache)
            {
                if (strategy.IsMatchType(type))
                {
                    this._strategy = strategy;
                    break;
                }
            }

             if (_strategy == null)
            {
                //如果没有得到策略 则报错
                throw new Exception("处理请求参数的策略没有找到!");
            }
        }
        #endregion

        #region 取值
        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            return this._strategy.GetValue(this._paramName, this._paramType, this._currentHttpRequest);
        }
        #endregion
    }
}
