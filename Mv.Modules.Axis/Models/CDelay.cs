using System;

namespace MotionWrapper
{
    /// <summary>
    /// 循环定时器
    /// </summary>
    public class CDelay
    {
        private int starttimeMS = 0;
        private int starttimeSe = 0;
        private int starttimemin = 0;
        private long TargetTime = 0;
        public CDelay()
        {
            starttimeMS = starttimeSe = starttimemin = 0;
            TargetTime = 0;
        }
        /// <summary>
        /// 定时时间
        /// </summary>
        /// <param name="targettime"></param>
        public void Init(long targettime)
        {
            starttimeMS = DateTime.Now.Millisecond;
            starttimeSe = DateTime.Now.Second;
            starttimemin = DateTime.Now.Minute;
            TargetTime = targettime;
        }
        /// <summary>
        /// 循环检测是否时间到了
        /// </summary>
        /// <returns></returns>
        public bool AtTime()
        {
            int minit = 0;
            if (DateTime.Now.Minute >= starttimemin)
            {
                minit = DateTime.Now.Minute - starttimemin;
            }
            else
            {
                minit = 60 - starttimemin + DateTime.Now.Minute;
            }
            if ((minit * 60000 + (DateTime.Now.Second - starttimeSe) * 1000 + (DateTime.Now.Millisecond - starttimeMS)) > TargetTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
