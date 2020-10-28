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
        private short active = 0;//激活
        private short map_rel_io = -1;//映射的实际物理地址
        private EAxisType type = EAxisType.Line;//0=直线 1= 旋转 3=纯编码器 4=纯虚拟轴
        private float maxAcc = 4900.0f;
        private float maxDec = 4900.0f;
        private float jerk = 1.0f; //最大加速度 单位是重力加速度G
        private float maxVel = 500.0f;                             //最大速度 500ms/s du/s

        private float resolution = 10000;                              //分辨率plus
        private short motorCircle = 1;                                  //电机转一圈
        private short mechanicalCircle = 1;                             //减速机转几圈
        private float pitch = 5;                                     //节距mm/du
        private short direct = 1;                                 //正方向
        private short enableSoftlmt = 0;
        private short enableAlm = 1;
        private short enableLmtP = 1;
        private short enableLmtN = 1;          //限位和报警的使能
        private short enableFollowErr = 0;//启用跟随误差检测
        private float followErrPos = 1.0f;
        private float arrivePand = 1.0f;//unit
        private short followeTime = 10;
        private float arriveDelay = 10;//ms
        private float softlmtn = 0;                       //正负软限位使能 0表示无效 !=0 并且上面限位有效的时候 才有效
        private short enableArriveType = 0;
        private short arriveType = 0;

        public short reverseHome = 0;
        public short reverseLmtp = 0;
        public short reverseLmtn = 0;//开关取反
        public short homeType = 0;//<0表示不需要回零
        public float homeSearch = 1000000.0f;//搜索用
        public short homeSequence = 0;//回零的顺序
        public float homeLeave = 100;
        public float homeTriger = 0;//GSN 1=上升沿 0=下降沿
        public float homeoffset = 0, homeVel1 = 50.0f, homeVel2 = 25.0f;            //回零相关
        public float backLash = 0;
        public float estopdec = 9800.0f; 

        public float smoothTime = 10;
        private float softlmtp;

        public string Name { get => name; set => name = value; }
        public string Note { get => note; set => note = value; }
        public short CardNum { get => cardNum; set => cardNum = value; }
        public short AxisNum { get => axisNum; set => axisNum = value; }
        public short Active { get => active; set => active = value; }
        public short Map_rel_io { get => map_rel_io; set => map_rel_io = value; }
        public EAxisType Type { get => type; set => type = value; }
        public float MaxAcc { get => maxAcc; set => maxAcc = value; }
        public float MaxDec { get => maxDec; set => maxDec = value; }
        public float Jerk { get => jerk; set => jerk = value; }
        public float MaxVel { get => maxVel; set => maxVel = value; }
        public float Resolution { get => resolution; set => resolution = value; }
        public short MotorCircle { get => motorCircle; set => motorCircle = value; }
        public short MechanicalCircle { get => mechanicalCircle; set => mechanicalCircle = value; }
        public float Pitch { get => pitch; set => pitch = value; }
        public short Direct { get => direct; set => direct = value; }
        public short EnableSoftlmt { get => enableSoftlmt; set => enableSoftlmt = value; }
        public short EnableAlm { get => enableAlm; set => enableAlm = value; }
        public short EnableLmtP { get => enableLmtP; set => enableLmtP = value; }
        public short EnableLmtN { get => enableLmtN; set => enableLmtN = value; }
        public short EnableFollowErr { get => enableFollowErr; set => enableFollowErr = value; }
        public float FollowErrPos { get => followErrPos; set => followErrPos = value; }
        public float ArrivePand { get => arrivePand; set => arrivePand = value; }
        public short FolloweTime { get => followeTime; set => followeTime = value; }
        public float ArriveDelay { get => arriveDelay; set => arriveDelay = value; }
        public float Softlmtp { get => softlmtp; set => softlmtp = value; }
        public float Softlmtn { get => softlmtn; set => softlmtn = value; }
        public short EnableArriveType { get => enableArriveType; set => enableArriveType = value; }
        public short ArriveType { get => arriveType; set => arriveType = value; }

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
