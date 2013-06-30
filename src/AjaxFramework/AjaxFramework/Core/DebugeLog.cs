using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;


namespace AjaxFramework
{
    /// <summary>
    /// 调试日志   仅在开发环境中开启即可
    /// </summary>
    public class DebugeLog
    {
        /// <summary>
        /// 日志路径 在config中配置 如果没配置将不会写日志
        /// </summary>
        private static readonly string logPath = "";

        /// <summary>
        /// 日志队列
        /// </summary>
        private Queue<string> _logQueue = new Queue<string>();

        /// <summary>
        /// 日志锁
        /// </summary>
        private static object obj = new object();

        static DebugeLog()
        {
            logPath = ConfigurationManager.AppSettings["AjaxFramewok_Log_Path"];
        }

        /// <summary>
        /// 写日志 是一个入队操作
        /// </summary>
        /// <param name="str"></param>
        public void Write(string str)
        {
            if (string.IsNullOrEmpty(logPath))
            {
                return;
            }
            this._logQueue.Enqueue(str);
        }

        /// <summary>
        /// 写异常日志 会包含他的内部日志
        /// </summary>
        /// <param name="ex"></param>
        public void Write(Exception ex)
        {
            this._logQueue.Enqueue(ex.Message);
            this._logQueue.Enqueue(ex.StackTrace);
            if (ex.InnerException != null)
            {
                Write(ex.InnerException);
            }
        }

        /// <summary>
        /// 提交日志
        /// </summary>
        public void Submit()
        {
            if (string.IsNullOrEmpty(logPath))
            {
                return;
            }

            if (!File.Exists(logPath))
            {
                File.Create(logPath);
            }

            //lock (obj)
            //{

                StreamWriter sw = new StreamWriter(logPath, true, Encoding.Default);
                sw.Write(GetLogText());
                sw.Close();

            //}
        }

        /// <summary>
        /// 得到日志文本 一个出队操作
        /// </summary>
        /// <returns></returns>
        private string GetLogText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\r\n**************log start {0}********************", DateTime.Now);
            for (int i = 0, count = this._logQueue.Count; i < count; i++)
            {
                sb.AppendFormat("\r\n" + this._logQueue.Dequeue());
            }
            sb.AppendFormat("\r\n**************log end {0}********************\r\n", DateTime.Now);
            return sb.ToString();
        }
    }
}
