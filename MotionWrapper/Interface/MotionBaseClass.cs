using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using MotionWrapper.Interface;

namespace MotionWrapper.BaseClass
{
    class CInputOutputPrm
    {
        //public int dataindex = 0;//这个IO点数据处于控制器类的数据列表中的哪一个
        public int modelNum = 0;//0-9是卡  10是别的模块
        public int index = 0;//位置 从0开始  0-15
        public bool often_open = false;//常开常闭
        //界面中显示
        public string name = "";//名称    
        public string mode = "";//所属的模块
        public string pin = "";//所属的针脚
    }
    class IoRef
    {
        public bool value = false;
        public CInputOutputPrm prm = new CInputOutputPrm();
    }
    /// <summary>
    /// 轴的参数 
    /// </summary>
    public class AxisParameter
    {
        public AxisParameter()
        {

        }
        /// <summary>
        /// 通过名字和文件来初始化轴参数
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool read(string xml, string name)
        {
            return true;
        }
        public bool save(string xml, string name)
        {
            return true;
        }
        public string name = "轴";
        public string note = "无备注";
        public short ID = -1;//当前选中的栏目-轴号//高川使用
        public short cardNum = 0;//固高控制卡使用
        public short axisNum = 0; //固高控制卡使用
        public short active = 0;//激活
        public short map_rel_io = -1;//映射的实际物理地址
        public short type = 0;//0=直线 1= 旋转
        public float maxAcc = 4900.0f, maxDec = 4900.0f, jerk = 1.0f;                            //最大加速度 默认0.5个G   mm/s2 du/s2
        public float maxVel = 500.0f;                             //最大速度 500ms/s

        public float resolution = 10000;                              //分辨率plus
        public short motorCircle = 1;                                  //电机转一圈
        public short mechanicalCircle = 1;                             //减速机转几圈
        public float pitch = 5;                                     //节距mm/du
        public short direct = 1;                                 //正方向
        public short enableSoftlmt = 0, enableAlm = 1, enableLmtP = 1, enableLmtN = 1;          //限位和报警的使能
        public short enableFollowErr = 0;//启用跟随误差检测
        public float followErrPos = 1.0f, arriveRange = 1.0f;//unit
        public short followeTime = 10, arriveTime = 10;//ms
        public float softlmtp = 0, softlmtn = 0;                       //正负软限位使能 0表示无效 !=0 并且上面限位有效的时候 才有效
        public short enableArrive = 0;
        public short arriveType = 0;

        public short reverseHome = 0, reverseLmtp = 0, reverseLmtn = 0;//开关取反
        public short homeType = 0;
        public float homeSearch = 1000000.0f;//搜索用
        public short homeSequence = 0;//回零的顺序
        public float homeLeave = 100;
        public float homeoffset = 0, homeVel1 = 50.0f, homeVel2 = 25.0f;            //回零相关
        public float backLash = 0;
        public float estopdec = 9800.0f;

        public float smoothTime = 10;
        //急停中的急停方式减速度
        /// <summary>
        /// 获取脉冲/MS2的单位
        /// </summary>
        /// <returns></returns>
        public double getAccPlsPerMs2()
        {
            return maxAcc / pitch * resolution / 1000.0;
        }
        /// <summary>
        /// 获取P/MS
        /// </summary>
        /// <returns></returns>
        public double getVelPlsPerMs()
        {
            return maxVel / pitch * resolution / 1000.0;
        }
        /// <summary>
        /// MM转换成脉冲
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public double mm2pls(double mm)
        {
            return mm / pitch * resolution;
        }
        public double pls2mm(long pls)
        {
            return pls * pitch / resolution;
        }
        public double mmpers2plsperms(double vel)
        {
            return vel / pitch * resolution / 1000.0;
        }
        public double plsperms2mmpers(double vel)
        {
            return vel * pitch / resolution * 1000.0;
        }
        public double mmpers22plsperms2(double acc)
        {
            return acc / pitch * resolution / 1000.0;
        }
    };
    //轴的定义
    public class AxisRef
    {
        //状态
        public bool servoOn = false;
        public bool alarm = false;
        public bool limitP = false;
        public bool limitN = false;
        public bool followErr = false;
        public bool moving = false;
        public bool inPos = false; 
        public bool isHoming = false;//如果在回零过程中 可能会进行回零检测
        public float cmdPos = 0.0f, relPos = 0.0f, cmdVel = 0.0f, relVel = 0.0f;
        public short homed = 0;//0没进行 1成功 -1 失败
        //参数
        public AxisParameter prm = new AxisParameter();
    };
    /// <summary>
    /// 坐标系参数
    /// </summary>
    public class CCrdPrm
    {
        public int dimension = 3;
        public double synVelMax = 1000;//合成速度   
        public double synAccMax = 1;//重力加速度G
        public double Pith = 10, Resolution = 10000;//用来转换最大合成速度和最大合成加速度   
        public int cardNum = 0;//卡号
        public int crdNum = 0;//坐标系号 固高需要
        public int x = 1, y = 2, z = 3, a = 0;//轴号
        public double[] orgData = new double[9];//坐标系原点--可能需要动态修改
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
            return synVelMax / Pith * Resolution / 1000.0;
        }
        public double getCrdMaxAccPlsPerMs2()
        {
            return synAccMax / Pith * Resolution / 1000.0;
        }
        public int mm2pls(double mm)
        {
            return (int)(mm / Pith * Resolution);
        }
        public double mm2plsf(double mm)
        {
            return mm / Pith * Resolution;
        }
        public double pls2mm(long pls)
        {
            return Pith * pls / Resolution;
        }
    }
    /// <summary>
    /// 单个轴的参数
    /// </summary>
    //public class AxisParameter
    //{
    //    //public int dataindex = 0;                        //0开始  这个轴的数据再控制器类中数据处于哪个位置 
    //    public int cardnum = 0;                         //0开始
    //    public int axisnum = 0;                         //从0开始
    //    public string note = "轴说明";                  //备注
    //    public string AxisName = "轴名称";              //轴名字

    //    public double MaxAcc = 1;                         //最大加速度 单位是重力加速度
    //    public double MaxVel = 500;                       //最大速度mm/s
    //    public double Resolution = 10000;                 //分辨率 10000
    //    public double Pith = 10;                          //节距 5
    //    public double Smoothtime = 0;                     //平花时间
    //    public double BackLash = 0;                       //反向间隙      
    //    public double HomeS1 = 10000;                     //搜索距离
    //    public double HomeVel1 = 100, HomeVel2 = 10;        //HOME的速度1,HOME速度2
    //    public double HomeOffset = 0;                     //Home偏移
    //    public double HomeLeave = 1000;                   //Home撞限位后离开的距离
    //    public int HomeType = 0;                          //回零方式
    //    public int ArriveType = 0;                        //到位方式-0=指令到了就算到了  1编码器也必须到宽度范围
    //    public int ArriveDelay = 100;                     //MS-到位方式为0的时候 延迟这么久以后才能运动 
    //    public double ArrivePand = 1;                     //MM-到位方式为1的时候   在这个宽度范围内表示到位了-mm
    //    /// <summary>
    //    /// 获取脉冲/MS2的单位
    //    /// </summary>
    //    /// <returns></returns>
    //    public double getAccPlsPerMs2()
    //    {
    //        return MaxAcc / Pith * Resolution / 1000.0;
    //    }
    //    /// <summary>
    //    /// 获取P/MS
    //    /// </summary>
    //    /// <returns></returns>
    //    public double getVelPlsPerMs()
    //    {
    //        return MaxVel / Pith * Resolution / 1000.0;
    //    }
    //    /// <summary>
    //    /// MM转换成脉冲
    //    /// </summary>
    //    /// <param name="mm"></param>
    //    /// <returns></returns>
    //    public double mm2pls(double mm)
    //    {
    //        return mm / Pith * Resolution;
    //    }
    //    public double pls2mm(long pls)
    //    {
    //        return pls * Pith / Resolution;
    //    }
    //    public double mmpers2plsperms(double vel)
    //    {
    //        return vel / Pith * Resolution / 1000.0;
    //    }
    //    public double plsperms2mmpers(double vel)
    //    {
    //        return vel * Pith / Resolution * 1000.0;
    //    }
    //    public double mmpers22plsperms2(double acc)
    //    {
    //        return acc / Pith * Resolution / 1000.0;
    //    }
    //}
    /// <summary>
    /// 输入输出的参数
    /// </summary>
    public class CIoPrm
    {
        //public int dataindex = 0;//这个IO点数据处于控制器类的数据列表中的哪一个
        public int modelNum = 0;//0-9是卡  10是别的模块
        public int index = 0;//位置 从0开始  0-15
        public bool often_open = false;//常开常闭
        //界面中显示
        public string name = "";//名称    
        public string mode = "";//所属的模块
        public string pin = "";//所属的针脚
    }
    
    /// <summary>
    /// 机器类,所有的并行组件都在这边
    /// </summary>
    public class CMachManager : CMachBase, IInitModel
    {
        //所有的机器组件都在这个列表中
        List<CMachBase> machlist = new List<CMachBase>();
        public CMachManager()
        {
            machlist.Clear();
        }
        public void addMach(CMachBase inMach)
        {
            machlist.Add(inMach);
        }
        public override bool preCheckStatus()
        {
            bool statusAll = true;
            foreach (CMachBase item in machlist)
            {
                if (!item.preCheckStatus()) statusAll = false;
            }
            return statusAll;
        }

        public override void Fresh()
        {
            throw new NotImplementedException();
        }

        public override bool Home()
        {
            bool statusAll = true;
            foreach (CMachBase item in machlist)
            {
                if (!item.Home())
                {
                    statusAll = false;
                    break;
                }
            }
            if (!statusAll) Stop();
            return statusAll;
        }

        public bool Init()
        {
            initok = true;
            foreach (CMachBase item in machlist)
            {
                if (item is IInitModel)//判断是否继承了接口
                {
                    if (!((IInitModel)item).Init()) initok = false;
                }
            }
            return initok;
        }

        public override bool Pause()
        {
            bool paused = true;
            foreach (CMachBase item in machlist)
            {
                if (!item.Pause()) paused = false;
            }
            if (paused)
            {
                run_mode = EAutoMode.PAUSE;
            }
            return paused;
        }

        public override bool Reset()
        {
            foreach (CMachBase item in machlist)
            {
                item.Reset();
            }
            return true;
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }

        public override bool Start()
        {
            bool checkOk = true;
            foreach (CMachBase item in machlist)
            {
                if (!item.preCheckStatus())
                {
                    checkOk = false;
                    break;
                }
            }
            if (checkOk)
            {
                foreach (CMachBase item in machlist)
                {
                    item.Start();
                }
                run_mode = EAutoMode.AUTO;
                return true;
            }
            else
            {
                run_mode = EAutoMode.MANUAL;
                return false;
            }
        }

        public override bool Stop()
        {
            foreach (CMachBase item in machlist)
            {
                item.Stop();
            }
            run_mode = EAutoMode.MANUAL;
            return true;
        }

        public bool UnInit()
        {
            foreach (CMachBase item in machlist)
            {
                if (item is IInitModel)//判断是否继承了接口
                {
                    ((IInitModel)item).UnInit();
                }
            }
            return true;
        }

        public override void RunModel(int modelIndex,ref EAutoMode modelNow)
        {
            if (modelIndex >=0 && modelIndex < machlist.Count)
            {
                modelNow = machlist[modelIndex].run_mode;
            }
        }
    }

    /// <summary>
    /// 凸轮运行状态
    /// </summary>
    public class CCamStatus
    {
        public int line = 0;//当前执行得段号
        public bool running = false;//当前正在运行
    }
    /// <summary>
    /// 凸轮数据
    /// </summary>
    public class CCamData
    {
        public long master, slaver;//位移量
        public int moveType;//0=nomal 1 = even 2=stop 3 continue
    }

    /// <summary>
    /// 组件的状态
    /// </summary>
    public enum EMachSts
    {
        Err = -2, Alm = -1, Ok = 0
    }
    /// <summary>
    /// 轴状态
    /// 这个整体数据必须和参数列表中的轴一一对应
    /// 这样在指定某个轴的时候,直接从参数的IDEX上指定
    /// </summary>
    //public class AxisRef
    //{
    //    public volatile bool isHoming = false;//回零的时候会不断刷新
    //    public volatile bool On, lmtp = true, lmtn = true, alm = true, arrive, moveing,atPos;
    //    public volatile int homed=2;//0=成功 1正在回 2=没有回零 -1 回零错误
    //    public volatile float encpos, prfpos, encvel,targetPos;
    //    public volatile float axisprfpos;//固高特有
    //}
    /// <summary>
    /// 运行模式
    /// </summary>
    public enum EAutoMode
    {
        MANUAL = 0, AUTO = 1, PAUSE = 2,OHTER=3
    }
    public enum EPauseType
    {
        NONE =0,NEEDSTOP=1
    }
    public class CPauseType
    {
        public EPauseType pauseType = new EPauseType();
        public int restartStep = -1;//重启的段号
    }
    /// <summary>
    /// 机器的基本类
    /// 所有的可运行组件都要继承自这里
    /// 不同的卡 要在这里添加
    /// </summary>
    public abstract class CMachBase
    {
        public Thread runThread = null;//内部使用的线程
        public bool threadself = false, initok = false;//自带线程
        public int step_auto = 0, step_pause = 0;//自动执行得流程  暂停的时候保存的流程号
        public EAutoMode run_mode = EAutoMode.MANUAL;//当前运行模式
        public CPauseType pauseData = new CPauseType();//暂停的类型,如要暂停的时候执行这个
        public volatile bool homeok = false, alm = true, lmt = true, move = false, allok = false,servoOn = false;//回零OK 有报警 有限位触发 在运动
        //-1为各种异常  0位正常  1为各种运行状态
        //只能表示模组本身的安全状态 不能表示产品流程结果
        public EMachSts statusTotal = EMachSts.Ok;//总体状态
        public float beilv = 0.5f;//运动倍率
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
    /// <summary>
    /// 单个IO的映射关系
    /// </summary>
    public class CIoMap
    {
        public int adress = 0;//在参数列表中的位置 0开始
        public CIoMap(int inadress)
        {
            adress = inadress;
        }
    }

    public class CFtriger
    {
        private bool oldvalue = false;
        public bool Result(bool value)
        {
            if (value == true)
            {
                oldvalue = true;
            }
            if (oldvalue == true && value == false)
            {
                oldvalue = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    /// <summary>
    /// 产生一个下降沿
    /// </summary>
    public class CRtriger
    {
        private bool oldvalue = true;
        public bool Result(bool value)
        {
            if (value == false)
            {
                oldvalue = false;
            }
            if (oldvalue == false && value == true)
            {
                oldvalue = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
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
