using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MotionWrapper
{
    /// <summary>
    /// 轴的设置参数
    /// </summary>
    public class CAxisSetPrm
    {
        public bool enableAlm =false, enableLmtP=false, enableLmtN=false;
        public float softLmtP = 0, softLmtN = 0;//软限位的设置
    }
    /// <summary>
    /// 捕获的参数
    /// </summary>
    public class CCapturePrm
    {
        //高川
        //#define CAPT_MODE_Z          0   // 编码器Z相捕获 
        //#define CAPT_MODE_IO         1   // IO 捕获 
        //#define CAPT_MODE_Z_AND_IO   2   // IO+Z相 捕获 
        //#define CAPT_MODE_Z_AFT_IO   3   // 先IO触发再Z相触发 捕获 
        //gsn
        //CAPTURE_HOME1
        // CAPTURE_INDEX2
        // CAPTURE_PROBE3
        public short type = 0;//类型
        //高川
        // 编码器硬件捕获IO源选择 
        //#define CAPT_IO_SRC_HOME    0   // 原点输入作为捕获IO 
        //#define CAPT_IO_SRC_LMTN    1   // 负向限位输入作为捕获IO 
        //#define CAPT_IO_SRC_LMTP    2   // 正向限位输入作为捕获IO 
        //#define CAPT_IO_SRC_DI0     3   // 通用数字输入0作为捕获IO 
        //#define CAPT_IO_SRC_DI1     4   // 通用数字输入1作为捕获IO 
        //#define CAPT_IO_SRC_DI2     5   // 通用数字输入2作为捕获IO 
        //#define CAPT_IO_SRC_DI3     6   // 通用数字输入3作为捕获IO 
        //#define CAPT_IO_SRC_DI4     7   // 通用数字输入4作为捕获IO 
        //#define CAPT_IO_SRC_DI5     8   // 通用数字输入5作为捕获IO 
        //#define CAPT_IO_SRC_DI6     9   // 通用数字输入6作为捕获IO 
        //#define CAPT_IO_SRC_DI7     10  // 通用数字输入7作为捕获IO 
        //#define CAPT_IO_SRC_DI8     11  // 通用数字输入8作为捕获IO 
        //#define CAPT_IO_SRC_DI9     12  // 通用数字输入9作为捕获IO 
        //#define CAPT_IO_SRC_DI10    13  // 通用数字输入10作为捕获IO 
        //#define CAPT_IO_SRC_DI11    14  // 通用数字输入11作为捕获IO 
        //#define CAPT_IO_SRC_DI12    15  // 通用数字输入12作为捕获IO 
        public short core = 1;//GSN使用  1/2 代表核号
        public short index = 0;//硬件参数号 GSN = 1-X
        public CCapturePrm(short _type, short _index,short core=1)
        {
            this.core = core;//职有GSN用
            type = _type;
            index = _index;
        }
    }

    /// <summary>
    /// IO控制的通用1
    /// </summary>
    public interface IIoPart1
    {

        bool getDiCounter(IoRef input, ref long counter);
        /// <summary>
        /// 读取一个区域的IO
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="lenth"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool getDi(int startIndex, int lenth, ref bool[] value);
        /// <summary>
        /// 读取区域的输出
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="lenth"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool getDo(int startIndex, int lenth, ref bool[] value);
        bool getDi(IoRef input);
        bool getDo(IoRef output);
        void setDO(IoRef output, bool value);
    }
    /// <summary>
    /// 一些建议组件  都要初始化反初始化和刷新
    /// </summary>
    public interface IInitModel
    {
        bool Init();                     //初始化
        bool UnInit();                   //反初始化
    }
    public interface IReadAndSave
    {
        bool Read(string xml);
        bool Save(string xml);
    }
    /// <summary>
    /// 刷新模块 
    /// </summary>
    public interface IFreshModel
    {
        void Fresh();                    //刷新状态
        void Run();                      //运行中
    }
    /// <summary>
    /// 启停模块
    /// </summary>
    public interface IStartStopModel
    {
        bool Start();
        bool Stop();
        bool Pause();
        bool Reset();
    }
    /// <summary>
    /// 运动控制的1
    /// </summary>
    public interface IMotionPart1
    {
        int MC_PowerOff(AxisRef axis); //关闭伺服
        int MC_Power(AxisRef axis);    //开电
        int MC_Reset(AxisRef axis);    //复位
        int MC_Home(ref AxisRef axis);     //回零
        int MC_HomeStatus(ref AxisRef axis);//回零的状态
        int MC_MoveAbs(AxisRef axis, double tpos, double beilv = 0.5);
        int MC_MoveAdd(AxisRef axis, double dist, double beilv = 0.5);
        int MC_MoveJog(AxisRef axis, double beilv = 0.5);
        int MC_AxisRef(ref AxisRef axisref);
        int MC_AxisRef(int startIndex, int lenth, ref AxisRef[] axisS);
        int MC_EStop(AxisRef axis);
        /// <summary>
        /// 会把编码器和指令规划器都设置成制定位置,单位是MM，内部会根据当量转换成脉冲再设置
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="pos">位置 单位MM</param>
        /// <returns></returns>
        int MC_SetPos(AxisRef axis, double pos);
        /// <summary>
        /// 设置轴的属性
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="prm"></param>
        /// <returns></returns>
        int MC_SetAxis(AxisRef axis, CAxisSetPrm prm);
    } 
    /// <summary>
    /// 运动控制的1
    /// </summary>
    public interface IMotionPart5
    {
        int MC_Compare(AxisRef axis, double posMM, IoRef cmpInput);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crdNum">0-X</param>
        /// <returns></returns>
        int MC_CrdCreate(int crdNum);//创建坐标系
        int MC_CrdSetOrigin(int crdNum, List<double> origionData);//设置坐标系原点
        int MC_CrdData(string cmd, int crdNum = 0);         //坐标系运动
        int MC_CrdData(List<string> gcode, int crdNum = 0);
        int MC_CrdStatus(int crdNum = 0);
        int MC_CrdOverride(int crdNum, float BeiLv);
        int MC_CrdStart(int cardNum = 0);
        int MC_Cam(string camlist);     //凸轮运动
        int MC_CamStatus(AxisRef slaver, ref CCamStatus status);
        /// <summary>
        /// 一个凸轮跟随
        /// </summary>
        /// <param name="master"></param>
        /// <param name="slaver"></param>
        /// <param name="passpos">切记这个是一个相关两,实际使用中要用这个值加上当前编码器值 为了提高速度</param>
        /// <param name="data"></param>
        /// <returns></returns>
        int MC_Cam(AxisRef master, AxisRef slaver, double passpos, List<CCamData> data, bool relPasspos = false);     //凸轮运动
        int preMC_CamModel(AxisRef master, AxisRef slaver);

        //捕获
        int MC_StartCapture(AxisRef enc, CCapturePrm captruePrm);
        /// <summary>
        /// 返回的状态
        /// </summary>
        /// <param name="enc">捕捉的编码器号</param>
        /// <param name="capturePos">这个位置已经转换成MM/度了</param>
        /// <returns>0捕获完成,1=正在捕获</returns>
        int MC_CaptureStatus(AxisRef enc, ref double capturePos);
    }
    public class SerialData
    {
        public string sn = "";
        public string macSN = "";
        public string startTime = "";
    }
    /// <summary>
    /// 自定义的一些功能
    /// </summary>
    public interface ISerialNumber
    {
        /// <summary>
        /// 获取机器吗
        /// </summary>
        /// <returns></returns>
        string MC_GetMachSN(int machNum=0);
        /// <summary>
        /// 获取序列号 这个序列号是给用户输入的.包含了可以使用的天数.
        /// </summary>
        /// <returns></returns>
        string MC_GetSn(string machSN,int days= 30,bool type= true);
        /// <summary>
        /// 读取xml中保存的本台机器的序列号，用来验证
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        SerialData MC_ReadSnFile(string xml = @"Confige\SN.xml");
        /// <summary>
        /// 保存一些数据
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        void MC_SaveSnFile(SerialData data,string xml = @"Confige\SN.xml");
        /// <summary>
        /// 检查是否允许运行
        /// </summary>
        /// <returns></returns>
        bool MC_CanRun(string userSn,string machSN,ref int haveDays);
    }
}
