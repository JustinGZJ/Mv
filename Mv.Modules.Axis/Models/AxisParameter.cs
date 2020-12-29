
using System.ComponentModel;
using System.Windows.Documents;

namespace MotionWrapper
{
    /// <summary>
    /// 轴的参数 
    /// </summary>
    public class AxisParameter
    {
        public AxisParameter()
        {

        }
        private string name = "轴";
        private string note = "无备注";
        private short cardNum = 0;//固高控制卡使用 gtn的core1-2 高川0-x
        private short axisNum = 0; //固高控制卡使用 GSN从1-X 高川0-x
        private bool active = false;//激活
        private EAxisType type = EAxisType.Line;//0=直线 1= 旋转 3=纯编码器 4=纯虚拟轴
        private float maxAcc = 4900.0f;
        private float maxDec = 4900.0f;
        private float jerk = 1.0f; //最大加速度 单位是重力加速度G
        private float maxVel = 500.0f;                             //最大速度 500ms/s du/s

        private float resolution = 10000;                              //分辨率plus
        private float pitch = 5;                                     //节距mm/du
        private short enableSoftlmt = 0;
        private bool enableAlm = true;
        private bool enableLmtP = true;
        private bool enableLmtN = true;          //限位和报警的使能
        //private bool enableFollowErr = false;//启用跟随误差检测
        //private float followErrPos = 1.0f;
   //     private float arrivePand = 1.0f;//unit
      //  private short followeTime = 10;
     //   private float arriveDelay = 10;//ms
        private float softlmtn = 0;                       //正负软限位使能 0表示无效 !=0 并且上面限位有效的时候 才有效

        //public short reverseHome = 0;
        //public short reverseLmtp = 0;
        //public short reverseLmtn = 0;//开关取反
        public short homeType = gts.mc.HOME_MODE_HOME;//<0表示不需要回零
        public float homeSearch = 1000000.0f;//回零搜索距离
        private float homeLeave = 100;
        //public float homeTriger = 0;//GSN 1=上升沿 0=下降沿
        private float homeVel2 = 25.0f;            //回零相关
        public float estopdec = 9800.0f;

        public float smoothTime = 10;
        private float softlmtp;
        private float homeoffset;
        private float homeVel1;

        [DisplayName("名称")]
        public string Name { get => name; set => name = value; }
        [DisplayName("描述")]
        public string Note { get => note; set => note = value; }
        [DisplayName("板卡号")]
        public short CardNum { get => cardNum; set => cardNum = value; }
        [DisplayName("轴号")]
        public short AxisNum { get => axisNum; set => axisNum = value; }
        [DisplayName("活动")]
        public bool Active { get => active; set => active = value; }
        [DisplayName("类型")]
        public EAxisType Type { get => type; set => type = value; }
        [DisplayName("最大加速度")]
        public float MaxAcc { get => maxAcc; set => maxAcc = value; }
        [DisplayName("最大减速度")]
        public float MaxDec { get => maxDec; set => maxDec = value; }
        public float Jerk { get => jerk; set => jerk = value; }
        [DisplayName("最大速度")]
        public float MaxVel { get => maxVel; set => maxVel = value; }
        [DisplayName("解析度")]
        public float Resolution { get => resolution; set => resolution = value; }
        [DisplayName("节距")]
        public float Pitch { get => pitch; set => pitch = value; }

        [DisplayName("软限位使能")]
        public short EnableSoftlmt { get => enableSoftlmt; set => enableSoftlmt = value; }
        [DisplayName("报警使能")]
        public bool EnableAlm { get => enableAlm; set => enableAlm = value; }
        [DisplayName("正限位使能")]
        public bool EnableLmtP { get => enableLmtP; set => enableLmtP = value; }
        [DisplayName("负限位使能")]
        public bool EnableLmtN { get => enableLmtN; set => enableLmtN = value; }
        //[DisplayName("启用跟随误差检测")]
        //public bool EnableFollowErr { get => enableFollowErr; set => enableFollowErr = value; }
        //[DisplayName("跟随误差值")]
        //public float FollowErrPos { get => followErrPos; set => followErrPos = value; }
        //[DisplayName("跟随时间")]
        //public short FolloweTime { get => followeTime; set => followeTime = value; }
        [DisplayName("正软限位")]
        public float SoftLimitPositive { get => softlmtp; set => softlmtp = value; }
        [DisplayName("负软限位")]
        public float SoftLimitNegative { get => softlmtn; set => softlmtn = value; }
        [DisplayName("回零偏移量")]
        public float Homeoffset { get => homeoffset; set => homeoffset = value; }
        [DisplayName("回零速度上限")]
        public float HomeVelHigh { get => homeVel1; set => homeVel1 = value; }
        [DisplayName("回零速度下限")]
        public float HomeVelLow { get => homeVel2; set => homeVel2 = value; }
        [DisplayName("极限离开(极限回原点)")]
        public float HomeLeave { get => homeLeave; set => homeLeave = value; }

        //急停中的急停方式减速度
        /// <summary>
        /// 获取脉冲/MS2的单位
        /// </summary>
        /// <returns></returns>
        public double getAccPlsPerMs2()
        {
            return MaxAcc * 9.8 / Pitch * Resolution / 1000.0;
        }
        /// <summary>
        /// 获取P/MS
        /// </summary>
        /// <returns></returns>
        public double getVelPlsPerMs()
        {
            return MaxVel / Pitch * Resolution / 1000.0;
        }
        /// <summary>
        /// MM转换成脉冲
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public double mm2pls(double mm)
        {
            return mm / Pitch * Resolution;
        }
        public double pls2mm(long pls)
        {
            return pls * Pitch / Resolution;
        }
        /// <summary>
        /// ms/s to pulse/ms
        /// </summary>
        /// <param name="vel">速度</param>
        /// <returns></returns>
        public double mmpers2plsperms(double vel)
        {
            return vel / Pitch * Resolution / 1000.0;
        }
        /// <summary>
        /// pulse/ms to mm/s
        /// </summary>
        /// <param name="vel">脉冲</param>
        /// <returns></returns>
        public double plsperms2mmpers(double vel)
        {
            return vel * Pitch / Resolution * 1000.0;
        }
        /// <summary>
        /// mm/(s*s) 2 pulse/(ms*ms)
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public double mmpers22plsperms2(double acc)
        {
            return acc / Pitch * Resolution / 1000.0;
        }
    };
}
