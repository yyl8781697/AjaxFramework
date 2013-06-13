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
        /// 属性详情
        /// </summary>
        private ParameterInfo _parameterInfo;

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
        /// <param name="currentHttpRequest">当前的详细请求</param>
        /// <param name="parameterInfo">当前的详细参数</param>
        public GetRequestDataContext(HttpRequestDescription currentHttpRequest, ParameterInfo parameterInfo)
        {
            if (currentHttpRequest == null)
            { 
                throw new ArgumentNullException("currentHttpRequest");
            }

            if (parameterInfo == null)
            {
                throw new ArgumentNullException("parameterInfo");
            }

            this._currentHttpRequest = currentHttpRequest;
            this._parameterInfo = parameterInfo;
            InitStrategy(_parameterInfo.ParameterType);
        }
        #endregion

        #region 静态构造函数 初始化策略缓存
        /// <summary>
        /// 静态构造函数 初始化策略缓存
        /// </summary>
        static GetRequestDataContext()
        {
            _strategyCache = new List<GetRequestDataStrategy>(){
               new GetRequestSampleTypeData(),
               new GetRequestBatchJosnData(),
               //new GetResquestListData(),//暂时还不支持啊
               new GetRequestFileData(),
               new GetRequestEntityData()
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
                //如果没有得到策略 则给实体类去处理
                _strategy = new GetRequestEntityData();
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
            return this._strategy.GetValue(this._parameterInfo.Name, this._parameterInfo.ParameterType, this._currentHttpRequest);
        }
        #endregion
    }
}
