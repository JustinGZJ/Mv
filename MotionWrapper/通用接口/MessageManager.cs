using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotionWrapper
{
    /// <summary>
    /// 消息管理类,其他的线程可以不断的读取这个消息 用于显示
    /// </summary>
    public class MessageManager
    {
        public List<string> msgListNew = new List<string>();//保存的最新的50条记录
        private List<string> msgList = new List<string>();//这个回不断的被删除
        private object lockObj = new object();
        private int maxMsg = 50;
        public MessageManager(int maxMsg = 50)
        {
            this.maxMsg = maxMsg;
        }
        public void addMessage(string msg)
        {
            if (msg != "")
            {
                lock (lockObj)
                {
                    if (msgList.Count > maxMsg)
                    {
                        msgList.RemoveAt(0);
                    }
                    if (msgListNew.Count > maxMsg)
                    {
                        msgListNew.RemoveAt(0);
                    }
                    msgListNew.Add(msg);
                    msgList.Add(msg);
                }
            }
        }
        public string getMessage()
        {
            lock (lockObj)
            {
                if (msgList.Count > 0)
                {
                    string msg = msgList[0];
                    msgList.RemoveAt(0);
                    return msg;
                }
                else
                {
                    return "";
                }
            }
        }
        public void clearMessage()
        {
            lock (lockObj)
            {
                msgListNew.Clear();
                msgList.Clear();
            }
        }
    }
}
