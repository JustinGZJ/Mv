using System;
using System.Runtime.InteropServices;
namespace GXN
{
	public class mc_la
	{
		public const short LA_AXIS_NUM = 8;
		public const short LA_WORK_AXIS_NUM = 6;

        //轴的参数信息（各轴最大速度，各轴最大加速度，各轴最大速度变化量）是否限制速度模式
        public const short AXIS_LIMIT_NONE = 0;     //轴无限制
        public const short AXIS_LIMIT_MAX_VEL = 1;  //轴最大速度限制
        public const short AXIS_LIMIT_MAX_ACC = 2;  //轴最大加速度限制
        public const short AXIS_LIMIT_MAX_DV = 4;   //轴最大速度跳变量限制
		public const short KIN_MSG_BUFFER_SIZE = 32;

        //速度规划模式
        public enum EVelMode
        {
            T_CURVE = 0,
            S_CURVE,
            S_CURVE_NEW,                  //根据加加速度、最大加速度进行S曲线速度前瞻，2015.11.16
            S_CURVE_SMOOTH,

            VEL_MODE_MAX = 0x10000,         //确保长度为4Byte
        };

		//工件坐标系下轨迹是否限制速度模式
		public enum EWorkLimitMode
		{
			WORK_LIMIT_INVALID=0,		//不限制
			WORK_LIMIT_VALID=1,			//限制生效
		};
		
		//设置的速度定义规则
		public enum EVelSettingDef
		{
			NORMAL_DEF_VEL=0,			//输入为轴坐标系所有轴的合成速度
			NUM_DEF_VEL=1,				//以NUM系统的规则定义
			CUT_DEF_VEL=2,				//速度为切削速度
		};
		
		//设置的加速度定义规则
		public enum EAccSettingDef
		{
			NORMAL_DEF_ACC=0,             //输入即输出
			LONG_AXIS_ACC=1,                //长轴最大速度
		};
		
		//机床类型
		public enum EMachineMode
		{
			NORMAL_THREE_AXIS=0,		//标准三轴机床模式
			MULTI_AXES=1,					//多轴联动模式
			FIVE_AXIS=2,					//五轴机床模式，轴坐标系为主，工件坐标系为辅
			FIVE_AXIS_WORK=3,				//五轴机床模式，工件坐标系为主，轴坐标系为辅
			ROBOT=4,
		};
		//前瞻参数结构体
		public struct TLookAheadParameter
		{
			public int lookAheadNum;					//前瞻段数
			public double time;						//时间常数
			public double radiusRatio;					//曲率限制调节参数
			public double vMax1;			//各轴的最大速度
            public double vMax2;
            public double vMax3;
            public double vMax4;
            public double vMax5;
            public double vMax6;
            public double vMax7;
            public double vMax8;
			public double aMax1;			//各轴的最大加速度
            public double aMax2;
            public double aMax3;
            public double aMax4;
            public double aMax5;
            public double aMax6;
            public double aMax7;
            public double aMax8;
			public double DVMax1;			//各轴的最大速度变化量（在时间常数内）
            public double DVMax2;
            public double DVMax3;
            public double DVMax4;
            public double DVMax5;
            public double DVMax6;
            public double DVMax7;
            public double DVMax8;
			public double scale1;			//各轴的脉冲当量
            public double scale2;
            public double scale3;
            public double scale4;
            public double scale5;
            public double scale6;
            public double scale7;
            public double scale8;
			public short axisRelation1;	//输入坐标和内部坐标的对应关系
            public short axisRelation2;
            public short axisRelation3;
            public short axisRelation4;
            public short axisRelation5;
            public short axisRelation6;
            public short axisRelation7;
            public short axisRelation8;
            [MarshalAs(UnmanagedType.ByValTStr,SizeConst=128)]
			public string machineCfgFileName;		//机床配置文件名
		};
		
		//////////////////////////////////////
		public struct RC_KIN_CONFIG
		{
			public short RobotType;
			public short reserved1;
		
			public short KinParUse1;
            public short KinParUse2;
            public short KinParUse3;
            public short KinParUse4;
            public short KinParUse5;
            public short KinParUse6;
            public short KinParUse7;
            public short KinParUse8;
            public short KinParUse9;
            public short KinParUse10;
            public short KinParUse11;
            public short KinParUse12;
            public short KinParUse13;
            public short KinParUse14;
            public short KinParUse15;
            public short KinParUse16;
            public short KinParUse17;
            public short KinParUse18;
			public double KinPar1;
            public double KinPar2;
            public double KinPar3;
            public double KinPar4;
            public double KinPar5;
            public double KinPar6;
            public double KinPar7;
            public double KinPar8;
            public double KinPar9;
            public double KinPar10;
            public double KinPar11;
            public double KinPar12;
            public double KinPar13;
            public double KinPar14;
            public double KinPar15;
            public double KinPar16;
            public double KinPar17;
            public double KinPar18;
			public short KinLimitUse1;
            public short KinLimitUse2;
            public short KinLimitUse3;
            public short KinLimitUse4;
            public short KinLimitUse5;
            public short KinLimitUse6;
            public short KinLimitUse7;
            public short KinLimitUse8;
            public short KinLimitUse9;
            public short KinLimitUse10;
            public short KinLimitUse11;
            public short KinLimitUse12;
			public double KinLimitMin1;
            public double KinLimitMin2;
            public double KinLimitMin3;
            public double KinLimitMin4;
            public double KinLimitMin5;
            public double KinLimitMin6;
            public double KinLimitMin7;
            public double KinLimitMin8;
            public double KinLimitMin9;
            public double KinLimitMin10;
            public double KinLimitMin11;
            public double KinLimitMin12;
			public double KinLimitMax1;
            public double KinLimitMax2;
            public double KinLimitMax3;
            public double KinLimitMax4;
            public double KinLimitMax5;
            public double KinLimitMax6;
            public double KinLimitMax7;
            public double KinLimitMax8;
            public double KinLimitMax9;
            public double KinLimitMax10;
            public double KinLimitMax11;
            public double KinLimitMax12;
			public double KinLimitMinShift1;
            public double KinLimitMinShift2;
            public double KinLimitMinShift3;
            public double KinLimitMinShift4;
            public double KinLimitMinShift5;
            public double KinLimitMinShift6;
            public double KinLimitMinShift7;
            public double KinLimitMinShift8;
            public double KinLimitMinShift9;
            public double KinLimitMinShift11;
            public double KinLimitMinShift12;
			public double KinLimitMaxShift1;
            public double KinLimitMaxShift2;
            public double KinLimitMaxShift3;
            public double KinLimitMaxShift4;
            public double KinLimitMaxShift5;
            public double KinLimitMaxShift6;
            public double KinLimitMaxShift7;
            public double KinLimitMaxShift8;
            public double KinLimitMaxShift9;
            public double KinLimitMaxShift10;
            public double KinLimitMaxShift11;
		    public double KinLimitMaxShift12;

			public short AxisUse1;
            public short AxisUse2;
            public short AxisUse3;
            public short AxisUse4;
            public short AxisUse6;
            public short AxisUse7;
            public short AxisUse8;
			public char AxisPosSignSwitch1;
            public char AxisPosSignSwitch2;
            public char AxisPosSignSwitch3;
            public char AxisPosSignSwitch4;
            public char AxisPosSignSwitch5;
            public char AxisPosSignSwitch6;
            public char AxisPosSignSwitch7;
            public char AxisPosSignSwitch8;
			public double AxisPosOffset1;
            public double AxisPosOffset2;
            public double AxisPosOffset3;
            public double AxisPosOffset4;
            public double AxisPosOffset5;
            public double AxisPosOffset6;
            public double AxisPosOffset7;
            public double AxisPosOffset8;
		
			public short CartUnitUse1;
            public short CartUnitUse2;
            public short CartUnitUse3;
            public short CartUnitUse4;
            public short CartUnitUse5;
            public short CartUnitUse6;
			public char CartPosKCSSignSwitch1;
            public char CartPosKCSSignSwitch2;
            public char CartPosKCSSignSwitch3;
            public char CartPosKCSSignSwitch4;
            public char CartPosKCSSignSwitch5;
            public char CartPosKCSSignSwitch6;
			public short reserved21;
            public short reserved22;
            public short reserved23;
			public double CartPosKCSOffset1;
            public double CartPosKCSOffset2;
            public double CartPosKCSOffset3;
            public double CartPosKCSOffset4;
            public double CartPosKCSOffset5;
            public double CartPosKCSOffset6;
		};
		
		public struct RC_ERROR_INTERFACE
		{
			public char Error;
			public short ErrorID;
			public string Message;
		};
		
		public struct RC_MSG_BUFFER_ELEMENT
		{
			public short ErrorID;
			public string Message;
			public string LogTime;
			public int InternalID;
		};
		
		public struct RC_MSG_BUFFER 
		{
			public short LastMsgIndex;
			public RC_MSG_BUFFER_ELEMENT MsgElement1;
            public RC_MSG_BUFFER_ELEMENT MsgElement2;
            public RC_MSG_BUFFER_ELEMENT MsgElement3;
            public RC_MSG_BUFFER_ELEMENT MsgElement4;
            public RC_MSG_BUFFER_ELEMENT MsgElement5;
            public RC_MSG_BUFFER_ELEMENT MsgElement6;
            public RC_MSG_BUFFER_ELEMENT MsgElement7;
            public RC_MSG_BUFFER_ELEMENT MsgElement8;
            public RC_MSG_BUFFER_ELEMENT MsgElement9;
            public RC_MSG_BUFFER_ELEMENT MsgElement10;
            public RC_MSG_BUFFER_ELEMENT MsgElement11;
            public RC_MSG_BUFFER_ELEMENT MsgElement12;
            public RC_MSG_BUFFER_ELEMENT MsgElement13;
            public RC_MSG_BUFFER_ELEMENT MsgElement14;
            public RC_MSG_BUFFER_ELEMENT MsgElement15;
            public RC_MSG_BUFFER_ELEMENT MsgElement16;
            public RC_MSG_BUFFER_ELEMENT MsgElement17;
            public RC_MSG_BUFFER_ELEMENT MsgElement18;
            public RC_MSG_BUFFER_ELEMENT MsgElement19;
            public RC_MSG_BUFFER_ELEMENT MsgElement20;
            public RC_MSG_BUFFER_ELEMENT MsgElement21;
            public RC_MSG_BUFFER_ELEMENT MsgElement22;
            public RC_MSG_BUFFER_ELEMENT MsgElement23;
            public RC_MSG_BUFFER_ELEMENT MsgElement24;
            public RC_MSG_BUFFER_ELEMENT MsgElement25;
            public RC_MSG_BUFFER_ELEMENT MsgElement26;
            public RC_MSG_BUFFER_ELEMENT MsgElement27;
            public RC_MSG_BUFFER_ELEMENT MsgElement28;
            public RC_MSG_BUFFER_ELEMENT MsgElement29;
            public RC_MSG_BUFFER_ELEMENT MsgElement30;
            public RC_MSG_BUFFER_ELEMENT MsgElement31;
            public RC_MSG_BUFFER_ELEMENT MsgElement32;
			public int LastMsgID;
		};

        //正逆解方向
        public enum ETransDir
        {
            FORWARD_TRANS = 0,  //正解
            INVERSE_TRANS,    //逆解

            TRANS_DIR_MAX = 0x10000,	// 确保长度为4Byte
        };

		//旋转轴范围设置
		public struct TRotationAxisRange
		{
			public int primaryAxisRangeOn;              //第一旋转轴限定范围是否生效，0：不生效，1：生效
			public int slaveAxisRangeOn;                //第二旋转轴限定范围是否生效，0：不生效，1：生效
			public double maxPrimaryAngle;              //第一旋转轴最大值
			public double minPrimaryAngle;              //第一旋转轴最小值
			public double maxSlaveAngle;                //第二旋转轴最大值
			public double minSlaveAgnle;                //第二旋转轴最小值
		};
		
		//选解参数
		public enum EGroupSelect
		{
			Continuous=0,
			Group_1,
			Group_2,
		};
	
		public enum OptimizeState
        {
            OPT_OFF=0,
            OPT_ON=1
        };

		public enum OptimizeMethod
        {
            NO_OPT=0,
            OPT_BLENDING, 
            OPT_CIRCLEFITTING,
            OPT_CUBICSPLINE,
            OPT_BSPLINE,
        };
		
		public enum ErrorID
        {
            INIT_ERROR=1,		//没有进行参数初始化
			PASSWORD_ERROR,		//密码错误，请在固高运动控制平台上运行
			INDATA_ERROR,		//输入数据错误（检查圆弧数据是否正确）
			PRE_PROCESS_ERROR,	//
			TOOL_RADIUS_COMPENSATE_ERROR_INOUT,		//刀具半径补偿错误：进入/结束刀补处不能是圆弧
			TOOL_RADIUS_COMPENSATE_ERROR_NOCROSS,	//刀具半径补偿错误：数据不合理，无法计算交点
			USERDATA_ERROR,
		};
		
		//轨迹优化参数结构体
		public struct TOptimizeParamUser 
		{
			public OptimizeState usePathOptimize;	//是否使用路径优化：OPT_OFF:不使用	OPT_ON:使用
		
			public float tolerance;				//公差(suggest: rough:0.1, pre-finish:0.05, finish:0.01)
		
			public OptimizeMethod optimizeMethod;	//选择曲线优化方式
		
			public OptimizeState keepLargeArc;		//是否保留大圆弧：OPT_OFF：不保留， OPT_ON：保留
		
			public float blendingMinError;			//blending的最小设定误差
		
			public float blendingMaxAngle;			//blending的最大角度限制（即当线段向量角度大于该角度时，不做blending，单位：度）
		
		};
		
		public struct TErrorInfo 
		{
			public ErrorID errorID;		//错误号(INIT_ERROR:未初始化参数；PRE_PROCESS_ERROR:预处理模块错误；
									//TOOL_RADIUS_COMPENSATE_ERROR:刀具半径补偿错误；)
			public int errorRowNum;		//错误行号
		};
		
		public struct TPreStartPos 
		{
		    public double Pos1;
            public double Pos2;
            public double Pos3;
            public double Pos4;
            public double Pos5;
            public double Pos6;
            public double Pos7;
            public double Pos8;
		};

        [DllImport("gts.dll")]
        public static extern short GT_SetupLookAheadCrd(short crd, EMachineMode machineMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetVelDefineModeLa(short crd, EVelSettingDef velDefMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetAccDefineModeLa(short crd, EAccSettingDef accDefMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisLimitModeLa(short crd, ref int pAxisLimitMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetWorkLimitModeLa(short crd, EWorkLimitMode workLimitMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisFollowModeLa(short crd, ref int pFollowMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisVelValidModeLa(short crd, int velValidAxis);

        [DllImport("gts.dll")]
        public static extern short GT_SetArcAllowErrorLa(short crd, double error);
        [DllImport("gts.dll")]
        public static extern short GT_SetMinEvenVelTime(short crd, double evenTime);
        [DllImport("gts.dll")]
        public static extern short GT_SetMinDccAngle(short crd, double dccAngle);
        [DllImport("gts.dll")]
        public static extern short GT_SetProfilePeriod(short crd, double profilePeriod);
        [DllImport("gts.dll")]
        public static extern short GT_SetFilterTime(short crd, Int32 filtNum);
        [DllImport("gts.dll")]
        public static extern short GT_SetPrecisionControl(short crd, short mode, double error);
        [DllImport("gts.dll")]
        public static extern short GT_SetVelSmoothModeLa(short crd, short smoothMode);
       
        [DllImport("gts.dll")]
        public static extern short GT_SetVelModeLa(short crd, EVelMode velMode);
        [DllImport("gts.dll")]
        public static extern short GT_InitLookAheadEx(short crd, ref TLookAheadParameter pLookAheadPara, short fifo, short motionMode, ref TPreStartPos pPreStartPos);
        [DllImport("gts.dll")]
        public static extern short GT_PrintLACmdLa(short crd, int printFlag, int clearFile);
        
        [DllImport("gts.dll")]
        public static extern short GT_GetLookAheadSegCountEx(short crd, out int pSegCount, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetMotionTimeEx(short crd, out double pTime, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_SetUserSegNumEx(short crd, int segNum, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_CrdDataEx(short crd, System.IntPtr pCrdData, short fifo);  //调用时传入 IntPtr.Zero GTN_CrdDataEx(1, System.IntPtr.Zero, 0);

        [DllImport("gts.dll")]
        public static extern short GT_LnXYEx(short crd, double x, double y, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYG0Ex(short crd, double x, double y, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZEx(short crd, double x, double y, double z, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZG0Ex(short crd, double x, double y, double z, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAEx(short crd, double x, double y, double z, double a, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAG0Ex(short crd, double x, double y, double z, double a, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZACEx(short crd, ref double pPos, short posMask, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZACG0Ex(short crd, ref double pPos, short posMask, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZACUVWEx(short crd, ref double pPos, short posMask, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZACUVWG0Ex(short crd, ref double pPos, short posMask, double synVel, double synAcc, int segNum, short override2, short fifo);


        [DllImport("gts.dll")]
        public static extern short GT_ArcXYREx(short crd, double x, double y, double radius, short circleDir, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZREx(short crd, double y, double z, double radius, short circleDir, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXREx(short crd, double z, double x, double radius, short circleDir, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYCEx(short crd, double x, double y, double xCenter, double yCenter, short circleDir, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZCEx(short crd, double y, double z, double yCenter, double zCenter, short circleDir, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXCEx(short crd, double z, double x, double zCenter, double xCenter, short circleDir, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYZEx(short crd, double x, double y, double z, double interX, double interY, double interZ, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYRZEx(short crd, double x, double y, double z, double radius, short circleDir, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYCZEx(short crd, double x, double y, double z, double xCenter, double yCenter, short circleDir, double synVel, double synAcc, int segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZRXEx(short crd, double x, double y, double z, double radius, short circleDir, double synVel, double synAcc, Int32 segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZCXEx(short crd, double x, double y, double z, double yCenter, double zCenter, short circleDir, double synVel, double synAcc, Int32 segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXRYEx(short crd, double x, double y, double z, double radius, short circleDir, double synVel, double synAcc, Int32 segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXCYEx(short crd, double x, double y, double z, double zCenter, double xCenter, short circleDir, double synVel, double synAcc, Int32 segNum, short override2, short fifo);

        [DllImport("gts.dll")]
        public static extern short GT_HelixXYRMultiZEx(short crd, ref double pPos, double radius, short circleDir, double synVel, double synAcc, Int32 segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYCMultiZEx(short crd, ref double pPos, double xCenter, double yCenter, short circleDir, double synVel, double synAcc, Int32 segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZRMultiXEx(short crd, ref double pPos, double radius, short circleDir, double synVel, double synAcc, Int32 segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZCMultiXEx(short crd, ref double pPos, double yCenter, double zCenter, short circleDir, double synVel, double synAcc, Int32 segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXRMultiYEx(short crd, ref double pPos, double radius, short circleDir, double synVel, double synAcc, Int32 segNum, short override2, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXCMultiYEx(short crd, ref double pPos, double zCenter, double xCenter, short circleDir, double synVel, double synAcc, Int32 segNum, short override2, short fifo);


        [DllImport("gts.dll")]
        public static extern short GT_BufDelayEx(short crd, ushort delayTime, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufIOEx(short crd, ushort doType, ushort doMask, ushort doValue, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufDAEx(short crd, short chn, short daValue, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufEnableDoBitPulseEx(short crd, short doType, short doIndex, ushort highLevelTime, ushort lowLevelTime, int pulseNum, short firstLevel, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufDisableDoBitPulseEx(short crd, short doType, short doIndex, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufGearEx(short crd, short gearAxis, double deltaPos, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufMoveEx(short crd, short moveAxis, double pos, double vel, double acc, short modal, short fifo);
        
       
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowMasterEx(short crd, ref GTN.mc.TBufFollowMaster pBufFollowMaster, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowEventCrossEx(short crd, ref GTN.mc.TBufFollowEventCross pEventCross, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowEventTriggerEx(short crd, ref GTN.mc.TBufFollowEventTrigger pEventTrigger, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowStartEx(short crd, Int32 masterSegment, Int32 slaveSegment, Int32 masterFrameWidth, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowNextEx(short crd, Int32 width, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowReturnEx(short crd, double vel, double acc, short smoothPercent, short fifo);

        [DllImport("gts.dll")]
        public static extern short GT_BufSmartCutterOnEx(short crd, short index, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufSmartCutterOffEx(short crd, short index, short fifo);


        [DllImport("gts.dll")]
        public static extern short GT_BufLaserOnEx(short crd, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserOffEx(short crd, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowModeEx(short crd, short source, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowTableEx(short crd, short tableId, double minPower, double maxPower, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowOffEx(short crd, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserPrfCmdEx(short crd, double laserPower, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowRatioEx(short crd, double ratio, double minPower, double maxPower, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_SetupLookAheadCrd(short core,short crd,EMachineMode machineMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAxisFollowModeLa(short core, short crd,ref int pFollowMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetVelDefineModeLa(short core,short crd,EVelSettingDef velDefMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAccDefineModeLa(short core, short crd, EAccSettingDef accDefMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAxisLimitModeLa(short core,short crd,ref int pAxisLimitMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetWorkLimitModeLa(short core,short crd,EWorkLimitMode workLimitMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAxisVelValidModeLa(short core,short crd,int velValidAxis);
        [DllImport("gts.dll")]
        public static extern short GTN_SetVelModeLa(short core, short crd, EVelMode velMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetVelSmoothModeLa(short core, short crd, short smoothMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetVelSmoothMode(short core,int crd,int smoothMode);
        [DllImport("gts.dll")]
        public static extern short GTN_PrintLACmdLa(short core, short crd, int printFlag, int clearFile);
        [DllImport("gts.dll")]
        public static extern short GTN_InitLookAheadEx(short core, short crd, ref TLookAheadParameter pLookAheadPara, short fifo, short motionMode, ref TPreStartPos pPreStartPos);

        [DllImport("gts.dll")]
        public static extern short GTN_GetLookAheadSegCountEx(short core, short crd, out int pSegCount, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetMotionTimeEx(short core, short crd, out double pTime, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_SetUserSegNumEx(short core, short crd, int segNum, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_CrdDataEx(short core, short crd, System.IntPtr pCrdData, short fifo);  //调用时传入 IntPtr.Zero GTN_CrdDataEx(1, System.IntPtr.Zero, 0);
        
        
        [DllImport("gts.dll")]
        public static extern short  GTN_LnXYEx(short core,short crd,double x,double y,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_LnXYG0Ex(short core,short crd,double x,double y,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_LnXYZEx(short core,short crd,double x,double y,double z,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_LnXYZG0Ex(short core,short crd,double x,double y,double z,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_LnXYZAEx(short core,short crd,double x,double y,double z,double a,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_LnXYZAG0Ex(short core,short crd,double x,double y,double z,double a,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_LnXYZACEx(short core,short crd,ref double pPos,short posMask,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_LnXYZACG0Ex(short core,short crd,ref double pPos,short posMask,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_LnXYZACUVWEx(short core,short crd,ref double pPos,short posMask,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_LnXYZACUVWG0Ex(short core,short crd,ref double pPos,short posMask,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_ArcXYREx(short core,short crd,double x,double y,double radius,short circleDir,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_ArcYZREx(short core,short crd,double y,double z,double radius,short circleDir,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_ArcZXREx(short core,short crd,double z,double x,double radius,short circleDir,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_ArcXYCEx(short core,short crd,double x,double y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_ArcYZCEx(short core,short crd,double y,double z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_ArcZXCEx(short core,short crd,double z,double x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_ArcXYZEx(short core,short crd,double x,double y,double z,double interX,double interY,double interZ,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_HelixXYRZEx(short core,short crd,double x,double y,double z,double radius,short circleDir,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_HelixXYCZEx(short core,short crd,double x,double y,double z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,int segNum,short override2,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_BufDelayEx(short core,short crd,ushort delayTime,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_BufGearEx(short core,short crd,short gearAxis,double deltaPos,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_BufMoveEx(short core,short crd,short moveAxis,double pos,double vel,double acc,short modal,short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_BufIOEx(short core,short crd,ushort doType,ushort doMask,ushort doValue,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufEnableDoBitPulseEx(short core, short crd, short doType, short doIndex, ushort highLevelTime, ushort lowLevelTime, int pulseNum, short firstLevel, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufDisableDoBitPulseEx(short core, short crd, short doType, short doIndex, short fifo);
        [DllImport("gts.dll")]
        public static extern short  GTN_BufDAEx(short core,short crd,short chn,short daValue,short fifo);


        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowMasterEx(short core, short crd, ref GTN.mc.TBufFollowMaster pBufFollowMaster, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowEventCrossEx(short core, short crd, ref GTN.mc.TBufFollowEventCross pEventCross, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowEventTriggerEx(short core, short crd, ref GTN.mc.TBufFollowEventTrigger pEventTrigger, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowStartEx(short core, short crd, Int32 masterSegment, Int32 slaveSegment, Int32 masterFrameWidth, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowNextEx(short core, short crd, Int32 width, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowReturnEx(short core, short crd, double vel, double acc, short smoothPercent, short fifo);

        [DllImport("gts.dll")]
        public static extern short GTN_BufSmartCutterOnEx(short core, short crd, short index, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufSmartCutterOffEx(short core, short crd, short index, short fifo);


        [DllImport("gts.dll")]
        public static extern short  GTN_BufLaserOnEx(short core,short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short  GTN_BufLaserOffEx(short core,short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserFollowModeEx(short core, short crd, short source, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserFollowTableEx(short core, short crd, short tableId, double minPower, double maxPower, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserFollowOffEx(short core, short crd, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short  GTN_BufLaserPrfCmdEx(short core,short crd,double laserPower,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short  GTN_BufLaserFollowRatioEx(short core,short crd,double ratio,double minPower,double maxPower,short fifo,short channel);
        
        
        [DllImport("gts.dll")]
        public static extern short GT_BufWaitDiEx(short crd, short diType, UInt16 diIndex, UInt16 level, short continueTime, Int32 overTime, short flagMode, Int32 segNum, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufWaitLongVarEx(short crd, short index, Int32 value, Int32 overTime, short flagMode, Int32 segNum, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufWaitDiEx(short core, short crd, short diType, UInt16 diIndex, UInt16 level, short continueTime, Int32 overTime, short flagMode, Int32 segNum, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufWaitLongVarEx(short core, short crd, short index, Int32 value, Int32 overTime, short flagMode, Int32 segNum, short fifo);

        [DllImport("gts.dll")]
        public static extern short GT_BufDoBitEx(short crd,UInt16 doType,UInt16 index,short value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufDoBitEx(short core,short crd, Int32 doType,UInt16 index,short value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufDoBitDelayEx(short core,short crd,UInt16  doType,UInt16 index,short value,Int32 delayTime,short fifo);

        [DllImport("gts.dll")]
        public static extern short GT_BufPosCompareStartEx(short core, short crd, short fifo, short index);
        [DllImport("gts.dll")]
        public static extern short GT_BufPosCompareStopEx(short core, short crd, short fifo, short index);
        [DllImport("gts.dll")]
        public static extern short GTN_BufPosComparePsoPrmEx(short core, short crd, short index, ref GTN.mc.TPosComparePsoPrm pPrm, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufPosCompareStartEx(short core, short crd, short fifo, short index);
        [DllImport("gts.dll")]
        public static extern short GTN_BufPosCompareStopEx(short core, short crd, short fifo, short index);

      }
}
