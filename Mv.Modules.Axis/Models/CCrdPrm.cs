namespace MotionWrapper
{
    /// <summary>
    /// 坐标系参数
    /// </summary>
    public class CCrdPrm
    {
        private int dimension = 3;
        private double synVelMax = 1000;//合成速度   
        private double synAccMax = 1;//重力加速度G
        private double resolution = 10000;//用来转换最大合成速度和最大合成加速度   
        private int cardNum = 0;//卡号
        private int crdNum = 0;//坐标系号 固高需要
        public double[] orgData = new double[9];//坐标系原点--可能需要动态修改
        private double pith;
        private int x=2;
        private int y=3;
        private int z=4;
        private int a=1;
        /// <summary>
        /// 维数
        /// </summary>
        public int Dimension { get => dimension; set => dimension = value; }
        /// <summary>
        /// 最大速度
        /// </summary>
        public double SynVelMax { get => synVelMax; set => synVelMax = value; }
        /// <summary>
        /// 最大加速度 单位是重力加速度
        /// </summary>
        public double SynAccMax { get => synAccMax; set => synAccMax = value; }
       /// <summary>
       /// 每圈移动距离
       /// </summary>
        public double Pith { get => pith; set => pith = value; }
        /// <summary>
        /// 分辨率
        /// </summary>
        public double Resolution { get => resolution; set => resolution = value; }
        /// <summary>
        /// 卡号
        /// </summary>
        public int CardNum { get => cardNum; set => cardNum = value; }
        /// <summary>
        /// 坐标系编号
        /// </summary>
        public int CrdNum { get => crdNum; set => crdNum = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Z { get => z; set => z = value; }
        public int A { get => a; set => a = value; }


        public CCrdPrm()
        {
            for (int i = 0; i < 9; i++)
            {
                orgData[i] = 0;
            }
        }
        public double mmpers2plsperms(double vel)
        {
            return vel / Pith * Resolution / 1000.0;
        }
        public double plsperms2mmpers(double vel)
        {
            return vel * Pith / Resolution * 1000.0;
        }
        public double getCrdMaxVelPlsPerMs()
        {
            return SynVelMax / Pith * Resolution / 1000.0;
        }
        public double getCrdMaxAccPlsPerMs2()
        {
            return SynAccMax / Pith * Resolution / 1000.0;
        }
        public int mm2pls(double mm)
        {
            return (int)(mm / Pith * Resolution);
        }
        public double mm2plsf(double mm)
        {
            return mm / Pith * Resolution;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pls"></param>
        /// <returns></returns>
        public double pls2mm(long pls)
        {
            return Pith * pls / Resolution;
        }

        public double Smoothtime { get; set; } = 0;                     //平花时间
        public double BackLash { get; set; } = 0;                       //反向间隙      
        public double HomeS1 { get; set; } = 10000;                     //搜索距离
        public double HomeVel1 { get; set; } = 100;
        public double HomeVel2 { get; set; } = 10;        //HOME的速度1,HOME速度2
        public double HomeOffset { get; set; } = 0;                     //Home偏移
        public double HomeLeave { get; set; } = 1000;                   //Home撞限位后离开的距离
        public int HomeType { get; set; } = 0;                          //回零方式
        public int ArriveType { get; set; } = 0;                        //到位方式-0=指令到了就算到了  1编码器也必须到宽度范围
        public int ArriveDelay { get; set; } = 100;                     //MS-到位方式为0的时候 延迟这么久以后才能运动 
        public double ArrivePand { get; set; } = 1;                     //MM-到位方式为1的时候   在这个宽度范围内表示到位了-mm
    }
}
