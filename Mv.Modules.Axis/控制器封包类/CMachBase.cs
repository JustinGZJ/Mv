using System.Threading;

namespace MotionWrapper
{
    /// <summary>
    /// 机器的基本类
    /// 所有的可运行组件都要继承自这里
    /// 不同的卡 要在这里添加
    /// </summary>
    public abstract class CMachBase
    {
        public Thread runThread = null;//内部使用的线程
        public Thread homeThread = null;//回零用的传感器
        public bool threadself = false, initok = false;//自带线程

        public EAutoMode run_mode = EAutoMode.MANUAL;//当前运行模式
        public CPauseType pauseData = new CPauseType();//暂停的类型,如要暂停的时候执行这个
        public volatile bool alm = true, lmt = true, move = false, allok = false,servoOn = false;//回零OK 有报警 有限位触发 在运动
        //-1为各种异常  0位正常  1为各种运行状态
        //只能表示模组本身的安全状态 不能表示产品流程结果
        public EMachSts statusTotal = EMachSts.Ok;//总体状态
        public float beilv = 0.5f;//运动倍率
        private int step_auto;
        private int step_pause;
        private volatile int step_home = 0;//自动执行得流程  暂停的时候保存的流程号
        private bool homeok;
        public int Step_auto { get => step_auto; set => step_auto = value; }
        public int Step_pause { get => step_pause; set => step_pause = value; }
        public int Step_home { get => step_home; set => step_home = value; }
        public bool Homeok { get => homeok; set => homeok = value; }

        public abstract void Fresh();
        public abstract bool Start();
        public abstract bool Stop();

        /// <summary>
        /// 只有处于手动模式 才能复位某些功能
        /// </summary>
        /// <returns></returns>
        public abstract bool Reset();
        /// <summary>
        /// 获取运动模式
        /// </summary>
        /// <returns></returns>
        public abstract void RunModel(int modelIndex, ref EAutoMode modelNow);
        public abstract bool Pause();
        /// <summary>
        /// 此函数是自动模式下长期运行的函数
        /// 可用于跑长期稳定的流程
        /// 如果不是收启停控制的  不要放这里
        /// 可以通过私有函数 来截取执行其中的某一段来完成某个功能
        /// </summary>
        /// <returns></returns>
        public abstract void Run();
        /// <summary>
        /// 预判当前状态是否正常 
        /// </summary>
        /// <returns></returns>
        public abstract bool preCheckStatus();
        public abstract bool Home();
    }
}
