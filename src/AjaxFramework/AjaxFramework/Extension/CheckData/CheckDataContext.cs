using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxFramework
{
    /// <summary>
    /// 检查数据的上下文
    /// </summary>
    internal class CheckDataContext
    {
        /// <summary>
        /// 检查数据的策略
        /// </summary>
        private CheckDataStrategy _strategy;

        /// <summary>
        /// 策略的字典
        /// </summary>
        private Dictionary<List<Type>, CheckDataStrategy> STRATEGY_DICT = new Dictionary<List<Type>, CheckDataStrategy>()
        {
            {new List<Type>(){typeof(int),typeof(Int16),typeof(Int32),typeof(Int64)},new CheckInt()},
             {new List<Type>(){typeof(decimal),typeof(Decimal),typeof(float),typeof(double),typeof(Single)},new CheckDecimal()},
            {new List<Type>(){typeof(string),typeof(String)},new CheckString()},
            {new List<Type>(){typeof(DateTime)},new CheckDate()}
        };

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public CheckDataContext(WebParameterAttr data)
        {
            //查询相应的检查策略
            foreach (List<Type> typeList in STRATEGY_DICT.Keys)
            {
                if (typeList.Contains(data.ParaType))
                {
                    _strategy = STRATEGY_DICT[typeList];
                    continue;
                }
            }

            if (_strategy == null)
            {
                throw new ArgumentException("CheckDataStrategy is not found");
            }
            //把数据信息给于检查策略
            this._strategy.CurrentData = data;
        }

        

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns></returns>
        public bool CheckData()
        {
            return this._strategy.CheckData();
        }
    }
}
