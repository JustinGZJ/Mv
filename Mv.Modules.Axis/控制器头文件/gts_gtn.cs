using System;
using System.Runtime.InteropServices;

namespace GTN
{ 
    public class mc
    {
        public const short CHANNEL_HOST = 0;
        public const short CHANNEL_UART = 1;
        public const short CHANNEL_SIM = 2;
        public const short CHANNEL_ETHER = 3;
        public const short CHANNEL_RS232 = 4;
        public const short CHANNEL_PCIE = 5;
        public const short CHANNEL_RINGNET = 6;
/*-----------------------------------------------------------*/
/* Error Code                                                */
/*-----------------------------------------------------------*/

        public const short CMD_SUCCESS = 0;

        public const short CMD_ERROR_READ_LEN = -2;        /* read data length error */
        public const short CMD_ERROR_READ_CHECKSUM = -3;   /* checksum error */

        public const short CMD_ERROR_WRITE_BLOCK = -4;    /* write pci fail */
        public const short CMD_ERROR_READ_BLOCK = -5;     /* read pci fail */

        public const short CMD_ERROR_OPEN = -6;           /* error to open device */
        public const short CMD_ERROR_CLOSE = -6;          /* error to close device */
        public const short CMD_ERROR_DSP_BUSY = -7;       /* DSP busy */

        public const short CMD_LOCK_ERROR = -8;           /*multi-thread lock error*/
        public const short CMD_DMA_ERROR = -9;            /*dma transmission error*/    
        public const short CMD_COMM_ERROR = -10;           /*pcie comm error*/
        public const short CMD_LOAD_RINGNET_ERROR = -11;  /*load ringnet dll error*/
        public const short CMD_RINGNET_STIME_ERROR = -12; /*load ringnet dll error*/

        public const short CMD_RINGNET_ENC_ERROR = -13;   /*encoder initial error*/

        public const short CMD_RNVERSION_MATCH_ERROR = -14;   /*ringnet version error*/
        public const short CMD_MCVERSION_MATCH_ERROR = -15;   /*dll error,need to update dll*/
        public const short CMD_MCVERSION_MATCH_WARNING = 15;  /*dll error,without some functions*/
        public const short CMD_DSPVERSION_MATCH_WARNING = 16; /*dsp is too old*/

        public const short CMD_ERROR_EXECUTE = 1;
        public const short CMD_ERROR_VERSION_NOT_MATCH = 3;
        public const short CMD_ERROR_PARAMETER= 7;
        public const short CMD_ERROR_UNKNOWN = 8;   /* unspported command */


        public const short MC_NONE  = -1;

        public const short MC_LIMIT_POSITIVE = 0;
        public const short MC_LIMIT_NEGATIVE = 1;
        public const short MC_ALARM  = 2;
        public const short MC_HOME  = 3;
        public const short MC_GPI = 4;
        public const short MC_ARRIVE = 5;
        public const short MC_EGPI0 = 6;
//      public const short MC_EGPI1 = 7;
//      public const short MC_EGPI2 = 8;
        public const short MC_MPG = 9;

        public const short MC_ENABLE = 10;
        public const short MC_CLEAR = 11;
        public const short MC_GPO  = 12;
//      public const short MC_EGPO0 = 13;
//      public const short MC_EGPO1 = 14;

        public const short MC_AU_ADC = 17;
        public const short MC_HSO = 18;
        public const short MC_AU_DAC = 19;

        public const short MC_DAC = 20;
        public const short MC_STEP  = 21;
        public const short MC_PULSE = 22;
        public const short MC_ENCODER = 23;
        public const short MC_ADC = 24;

        public const short MC_AU_ENCODER = 26;

        public const short MC_ABS_ENCODER = 29;

        public const short MC_AXIS = 30;
        public const short MC_PROFILE = 31;
        public const short MC_CONTROL = 32;

        public const short MC_TRIGGER = 40;

        public const short MC_AU_TRIGGER = 44;

        public const short MC_TERMINAL = 50;

        public const short MC_EXT_MODULE = 60;
        public const short MC_EXT_DI = 61;
        public const short MC_EXT_DO = 62;
        public const short MC_EXT_AI = 63;
        public const short MC_EXT_AO = 64;

        public const short MC_SCAN_CRD  = 70;
        public const short MC_LASER	= 71;
        public const short MC_LASER_AO = 72;

        public const short MC_POS_COMPARE = 80;

        public struct TVersion
        {
            public short year;
            public short month;
            public short day;
            public short version;
            public short user;
            public short reserve1;
            public short reserve2;
            public short chip;
         } 
        public const short CORE_MODE_TIMER = 0;
        public const short CORE_MODE_SYNCH = 1;
        public const short CORE_MODE_EXTERNAL =	2;

        public const short CORE_TASK_DEFAULT = 0;
        public const short CORE_TASK_DLM = 1;

        public const short SKIP_MODULE_SCAN	= 0x001;
        public const short SKIP_MODULE_POS_COMPARE = 0x002;
        public const short SKIP_MODULE_CRD = 0x004;

        public const short SKIP_MODULE_PLC = 0x010;
        public const short SKIP_MODULE_DLM = 0x020;

        public const short SKIP_MODULE_AXIS_CALCULATE = 0x100;

        public const short SKIP_MODULE_WATCH = 0x800;

        public enum ETimeElapse
        {
	        TIME_ELAPSE_PROFILE = 1000,

	        TIME_ELAPSE_HOST_COMMAND_EXECUTE = 1220,
	        TIME_ELAPSE_ETHER_COMMAND_EXECUTE,

	        TIME_ELAPSE_PROFILE_CALCULATE = 6000,

	        TIME_ELAPSE_SCAN = 18000,

	        TIME_ELAPSE_AXIS_CHECK = 20000,
	        TIME_ELAPSE_AXIS_CALCULATE,

	        TIME_ELAPSE_ENCODER = 30000,

	        TIME_ELAPSE_DI = 31000,

	        TIME_ELAPSE_DO = 32000,

	        TIME_ELAPSE_AI = 33000,

	        TIME_ELAPSE_AO = 34000,

	        TIME_ELAPSE_TRIGGER = 38000,

	        TIME_ELAPSE_CONTROL = 40000,

	        TIME_ELAPSE_WATCH = 52000,

	        TIME_ELAPSE_TERMINAL = 53000,
            TIME_ELAPSE_TERMINALDET = 54000,
       } 
       
        public struct TPid
        {
	            public double kp;
	            public double ki;
	            public double kd;
	            public double kvff;
	            public double kaff;
	            public Int32   IntegralLimit;
	            public Int32   derivativeLimit;
	            public short  limit;
         }
        [DllImport("gts.dll")]
        public static extern short GT_SetCardNo(short index);
        [DllImport("gts.dll")]
        public static extern short GT_GetCardNo(out short index);
        [DllImport("gts.dll")]
        public static extern short GT_Open(short channel,short param);
        [DllImport("gts.dll")]
        public static extern short GT_Close();
        [DllImport("gts.dll")]
        public static extern short GT_SetCore(short core);
        [DllImport("gts.dll")]
        public static extern short GT_GetCore(out short core);
        [DllImport("gts.dll")]
        public static extern short GT_GetVersion(out string pVersion);
        [DllImport("gts.dll")]
        public static extern short GT_GetVersionEx(short type,out TVersion pVersion);
        [DllImport("gts.dll")]
        public static extern short GT_Reset();
        [DllImport("gts.dll")]
        public static extern short GT_GetClock(out UInt32 pClock, out UInt32 pLoop);
        [DllImport("gts.dll")]
        public static extern short GT_GetClockHighPrecision(out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_ClearTime(ETimeElapse item);
        [DllImport("gts.dll")]
        public static extern short GT_GetTime(ETimeElapse item, out UInt32 pTime, out UInt32 pTimeMax, out UInt32 pValue);
        [DllImport("gts.dll")]
        public static extern short GT_GetSts(short axis, out Int32 pSts, short count, out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_ClrSts(short axis,short count);
        [DllImport("gts.dll")]
        public static extern short GT_AxisOn(short axis);
        [DllImport("gts.dll")]
        public static extern short GT_AxisOff(short axis);
        [DllImport("gts.dll")]
        public static extern short GT_MultiAxisOn(UInt32 mask);
        [DllImport("gts.dll")]
        public static extern short GT_MultiAxisOff(UInt32 mask);
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisOnDelayTime(ushort ms);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisOnDelayTime(out ushort pMs);
        [DllImport("gts.dll")]
        public static extern short GT_Stop(Int32 mask, Int32 option);
        [DllImport("gts.dll")]
        public static extern short GT_SetPrfPos(short profile, Int32 prfPos);
        [DllImport("gts.dll")]
        public static extern short GT_SynchAxisPos(Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GT_ZeroPos(short axis,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetLimitStatus(short axis,out short pLimitPositive,out short pLimitNegative);
        [DllImport("gts.dll")]
        public static extern short GT_SetSoftLimitMode(short axis,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_GetSoftLimitMode(short axis,out short pMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetSoftLimit(short axis, Int32 positive, Int32 negative);
        [DllImport("gts.dll")]
        public static extern short GT_GetSoftLimit(short axis, out Int32 pPositive, out Int32 pNegative);

        [DllImport("gts.dll")]
        public static extern short GT_SetAxisBand(short axis, Int32 band, Int32 time);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisBand(short axis, out Int32 pBand, out Int32 pTime);
        [DllImport("gts.dll")]
        public static extern short GT_GetPrfPos(short profile, out double Value, short count, out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetPrfVel(short profile,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetPrfAcc(short profile,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetPrfMode(short profile, out Int32 pValue, short count, out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisPrfPos(short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisPrfPosCompensate(short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisPrfVel(short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisPrfAcc(short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisEncPos(short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisEncVel(short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisEncAcc(short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisError(short axis,out double pValue, short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_SetControlFilter(short control,short index);
        [DllImport("gts.dll")]
        public static extern short GT_GetControlFilter(short control,out short pIndex);
        [DllImport("gts.dll")]
        public static extern short GT_SetControlSuperimposed(short control,short superimposedType,short superimposedIndex);
        [DllImport("gts.dll")]
        public static extern short GT_GetControlSuperimposed(short control,out short pSuperimposedType,out short pSuperimposedIndex);
        [DllImport("gts.dll")]
        public static extern short GT_SetPid(short control,short index,ref TPid pPid);
        [DllImport("gts.dll")]
        public static extern short GT_GetPid(short control,short index,out TPid pPid);
        [DllImport("gts.dll")]
        public static extern short GT_SetKvffFilter(short control,short index,short kvffFilterExp,double accMax);
        [DllImport("gts.dll")]
        public static extern short GT_GetKvffFilter(short control,short index,out short pKvffFilterExp,out double pAccMax);
        [DllImport("gts.dll")]
        public static extern short GT_Delay(ushort ms);
        [DllImport("gts.dll")]
        public static extern short GT_DelayHighPrecision(ushort profile);
        
        
        [DllImport("gts.dll")]
        public static extern short GTN_Open(short channel,short param);
        [DllImport("gts.dll")]
        public static extern short GTN_OpenRingNet(short channel,short param,ref string pFile,short index,Int32 count);
        [DllImport("gts.dll")]
        public static extern short GTN_Close();
        [DllImport("gts.dll")]
        public static extern short GTN_GetChannel(out short pChannel);
        [DllImport("gts.dll")]
        public static extern short GTN_GetVersion(short core,out string pVersion);
        [DllImport("gts.dll")]
        public static extern short GTN_GetVersionEx(short core,short type,out TVersion pVersion);
        [DllImport("gts.dll")]
        public static extern short GTN_SetVersion(short core,short type,ref TVersion pVersion);
        [DllImport("gts.dll")]
        public static extern short GTN_Reset(short core);
        [DllImport("gts.dll")]
        public static extern short GTN_GetClock(short core, out UInt32 pClock, out UInt32 pLoop);
        [DllImport("gts.dll")]
        public static extern short GTN_GetClockHighPrecision(short core,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearTime(short core,ETimeElapse item);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTime(short core, ETimeElapse item, out UInt32 pTime, out UInt32 pTimeMax, out UInt32 pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCoreMode(short core,short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCoreMode(short core,out short pMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCoreShare(short core,short type,short index,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCoreShare(short core,short type,out short pIndex,out short pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCoreTask(short core,short task);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCoreTask(short core,out short pTask);
        [DllImport("gts.dll")]
        public static extern short GTN_GetResMax(short core,short type,out short pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_SetResCount(short core,short type,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetResCount(short core,short type,out short pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetSts(short core,short axis,out Int32 pSts,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_ClrSts(short core,short axis,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_AxisOn(short core,short axis);
        [DllImport("gts.dll")]
        public static extern short GTN_AxisOff(short core,short axis);
        [DllImport("gts.dll")]
        public static extern short GTN_MultiAxisOn(short core,UInt32 mask);
        [DllImport("gts.dll")]
        public static extern short GTN_MultiAxisOff(short core, UInt32 mask);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAxisOnDelayTime(short core,ushort delayTime);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisOnDelayTime(short core,out ushort pDelayTime);
        [DllImport("gts.dll")]
        public static extern short GTN_Stop(short core, Int32 mask, Int32 option);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPrfPos(short core, short profile, Int32 prfPos);
        [DllImport("gts.dll")]
        public static extern short GTN_SynchAxisPos(short core, Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GTN_ZeroPos(short core,short axis,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetLimitStatus(short core,short axis,out short pLimitPositive,out short pLimitNegative);
        [DllImport("gts.dll")]
        public static extern short GTN_SetSoftLimitMode(short core,short axis,short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetSoftLimitMode(short core,short axis,out short pMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetSoftLimit(short core, short axis, Int32 positive, Int32 negative);
        [DllImport("gts.dll")]
        public static extern short GTN_GetSoftLimit(short core, short axis, out Int32 pPositive, out Int32 pNegative);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAxisBand(short core, short axis, Int32 band, Int32 time);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisBand(short core, short axis, out Int32 pBand, out Int32 pTime);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPrfPos(short core,short profile,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPrfVel(short core,short profile,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPrfAcc(short core,short profile,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPrfMode(short core,short profile,out Int32 pValue, short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisPrfPos(short core,short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisPrfPosCompensate(short core,short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisPrfVel(short core,short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisPrfAcc(short core,short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisEncPos(short core,short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisEncVel(short core,short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisEncAcc(short core,short axis,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisError(short core,short axis,out double pValue, short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_SetControlFilter(short core,short control,short index);
        [DllImport("gts.dll")]
        public static extern short GTN_GetControlFilter(short core,short control,out short pIndex);
        [DllImport("gts.dll")]
        public static extern short GTN_SetControlSuperimposed(short core,short control,short superimposedType,short superimposedIndex);
        [DllImport("gts.dll")]
        public static extern short GTN_GetControlSuperimposed(short core,short control,out short pSuperimposedType,out short pSuperimposedIndex);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPid(short core,short control,short index,ref TPid pPid);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPid(short core,short control,short index,out TPid pPid);
        [DllImport("gts.dll")]
        public static extern short GTN_SetKvffFilter(short core,short control,short index,short kvffFilterExp,double accMax);
        [DllImport("gts.dll")]
        public static extern short GTN_GetKvffFilter(short core,short control,short index,out short pKvffFilterExp,out double pAccMax);
        [DllImport("gts.dll")]
        public static extern short GTN_Delay(short core,ushort ms);
        [DllImport("gts.dll")]
        public static extern short GTN_DelayHighPrecision(short core,ushort profile);

        public const short STEP_DIR  = 0;
        public const short STEP_PULSE = 1;
        public const short STEP_ORTHOGONAL = 2;

        [DllImport("gts.dll")]
        public static extern short GT_LoadConfig(ref string pFile);
        [DllImport("gts.dll")]
        public static extern short GT_AlarmOff(short axis);
        [DllImport("gts.dll")]
        public static extern short GT_AlarmOn(short axis);
        [DllImport("gts.dll")]
        public static extern short GT_LmtsOn(short axis,short limitType);
        [DllImport("gts.dll")]
        public static extern short GT_LmtsOff(short axis,short limitType);
        [DllImport("gts.dll")]
        public static extern short GT_StepDir(short step);
        [DllImport("gts.dll")]
        public static extern short GT_StepPulse(short step);
        [DllImport("gts.dll")]
        public static extern short GT_StepOrthogonal(short step);
        [DllImport("gts.dll")]
        public static extern short GT_SetMtrBias(short dac,short bias);
        [DllImport("gts.dll")]
        public static extern short GT_GetMtrBias(short dac,out short pBias);
        [DllImport("gts.dll")]
        public static extern short GT_SetMtrLmt(short dac,short limit);
        [DllImport("gts.dll")]
        public static extern short GT_GetMtrLmt(short dac,out short pLimit);
        [DllImport("gts.dll")]
        public static extern short GT_EncOn(short encoder);
        [DllImport("gts.dll")]
        public static extern short GT_EncOff(short encoder);
        [DllImport("gts.dll")]
        public static extern short GT_SetPosErr(short control,Int32 error);
        [DllImport("gts.dll")]
        public static extern short GT_GetPosErr(short control, out Int32 pError);
        [DllImport("gts.dll")]
        public static extern short GT_SetStopDec(short profile,double decSmoothStop,double decAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GT_GetStopDec(short profile,out double pDecSmoothStop,out double pDecAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GT_CtrlMode(short axis,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_SetStopIo(short axis,short stopType,short inputType,short inputIndex);
        [DllImport("gts.dll")]
        public static extern short GT_SetAdcFilterPrm(short adc,double k);
        [DllImport("gts.dll")]
        public static extern short GT_GetAdcFilterPrm(short adc,out double pk);
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisPrfVelFilter(short axis,short filterNumExp);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisPrfVelFilter(short axis,out short pFilterNumExp);
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisEncVelFilter(short axis,short filterNumExp);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisEncVelFilter(short axis,out short pFilterNumExp);
        [DllImport("gts.dll")]
        public static extern short GT_SetProfileScale(short i, Int32 alpha, Int32 beta);
        [DllImport("gts.dll")]
        public static extern short GT_GetProfileScale(short i, out Int32 pAlhpa, out Int32 pBeta);
        [DllImport("gts.dll")]
        public static extern short GT_SetEncoderScale(short i, Int32 alpha, Int32 beta);
        [DllImport("gts.dll")]
        public static extern short GT_GetEncoderScale(short i, out Int32 pAlhpa, out Int32 pBeta);
        
        
        [DllImport("gts.dll")]
        public static extern short GTN_LoadConfig(short core,string pFile);
       [DllImport("gts.dll")]
        public static extern short GTN_AlarmOn(short core,short axis);
        [DllImport("gts.dll")]
        public static extern short GTN_AlarmOff(short core,short axis);
        [DllImport("gts.dll")]
        public static extern short GTN_LmtsOn(short core,short axis,short limitType);
        [DllImport("gts.dll")]
        public static extern short GTN_LmtsOff(short core,short axis,short limitType);
        [DllImport("gts.dll")]
        public static extern short GTN_StepDir(short core,short step);
        [DllImport("gts.dll")]
        public static extern short GTN_StepPulse(short core,short step);
        [DllImport("gts.dll")]
        public static extern short GTN_StepOrthogonal(short core,short step);
        [DllImport("gts.dll")]
        public static extern short GTN_SetMtrBias(short core,short dac,short bias);
        [DllImport("gts.dll")]
        public static extern short GTN_GetMtrBias(short core,short dac,out short pBias);
        [DllImport("gts.dll")]
        public static extern short GTN_SetMtrLmt(short core,short dac,short limit);
        [DllImport("gts.dll")]
        public static extern short GTN_GetMtrLmt(short core,short dac,out short pLimit);
        [DllImport("gts.dll")]
        public static extern short GTN_SetSense(short core,short dataType,short dataIndex,short value);
        [DllImport("gts.dll")]
        public static extern short GTN_GetSense(short core,short dataType,short dataIndex,out short pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_EncOn(short core,short encoder);
        [DllImport("gts.dll")]
        public static extern short GTN_EncOff(short core,short encoder);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPosErr(short core, short control, Int32 error);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPosErr(short core, short control, out Int32 pError);
        [DllImport("gts.dll")]
        public static extern short GTN_SetStopDec(short core,short profile,double decSmoothStop,double decAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GTN_GetStopDec(short core,short profile,out double pDecSmoothStop,out double pDecAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GTN_CtrlMode(short core,short axis,short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetStopIo(short core,short axis,short stopType,short inputType,short inputIndex);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAdcFilterPrm(short core,short adc,double k);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAdcFilterPrm(short core,short adc,out double pk);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAxisPrfVelFilter(short core,short axis,short filterNumExp);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisPrfVelFilter(short core,short axis,out short pFilterNumExp);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAxisEncVelFilter(short core,short axis,short filterNumExp);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisEncVelFilter(short core,short axis,out short pFilterNumExp);
        [DllImport("gts.dll")]
        public static extern short GTN_SetProfileScale(short core, short i, Int32 alpha, Int32 beta);
        [DllImport("gts.dll")]
        public static extern short GTN_GetProfileScale(short core, short i, out Int32 pAlhpa, out Int32 pBeta);
        [DllImport("gts.dll")]
        public static extern short GTN_SetEncoderScale(short core, short i, Int32 alpha, Int32 beta);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEncoderScale(short core, short i, out Int32 pAlhpa, out Int32 pBeta);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAuEncoderScale(short core, short i, Int32 alpha, Int32 beta);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAuEncoderScale(short core, short i, out Int32 pAlhpa, out Int32 pBeta);
        
        /*-----------------------------------------------------------*/
        /* Capture and Triggr                                        */
        /*-----------------------------------------------------------*/
        public const short CAPTURE_HOME = 1;
        public const short CAPTURE_INDEX = 2;
        public const short CAPTURE_PROBE = 3;
        public const short CAPTURE_HSIO0 = 6;
        public const short CAPTURE_HSIO1 = 7;

        public struct TTrigger
        {
            public short encoder;
            public short probeType;
            public short probeIndex;
            public short sense;
            public Int32 offset;
            public UInt32 loop;
            public short windowOnly;
            public Int32 firstPosition;
            public Int32 lastPosition;
        }
        public struct TTriggerEx
        {
            public short latchType;
            public short latchIndex;
            public short probeType;
            public short probeIndex;
            public short sense;
            public Int32 offset;
            public UInt32 loop;
            public short windowOnly;
            public Int32 firstPosition;
            public Int32 lastPosition;
        }
        public struct TTriggerAlign
        {
            public short encoder;
            public short probeType;
            public short probeIndex;
            public short sense;
            public Int32 offset;
            public UInt32 loop;
            public short windowOnly;
            public short pad2;
            public Int32 firstPosition;
            public Int32 lastPosition;
        }

        public struct TTriggerStatus
        {
            public short execute;
            public short done;
            public Int32 position;
        }

        public struct TTriggerStatusEx
        {
            public short execute;
            public short done;
            public Int32 position;
            public UInt32 clock;
            public UInt32 loopCount;
        }
        [DllImport("gts.dll")]
        public static extern short GT_SetTrigger(short i,ref TTrigger pTrigger);
        [DllImport("gts.dll")]
        public static extern short GT_GetTrigger(short i,out TTrigger pTrigger);
        [DllImport("gts.dll")]
        public static extern short GT_GetTriggerStatus(short i,out TTriggerStatus pTriggerStatus,short count);
        [DllImport("gts.dll")]
        public static extern short GT_ClearTriggerStatus(short i);
        [DllImport("gts.dll")]
        public static extern short GT_SetCaptureMode(short encoder,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureMode(short encoder,out short pMode,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureStatus(short encoder, out short pStatus, out Int32 pValue, short count, out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_SetCaptureSense(short encoder,short mode,short sense);
        [DllImport("gts.dll")]
        public static extern short GT_ClearCaptureStatus(short encoder);
        [DllImport("gts.dll")]
        public static extern short GT_SetCaptureRepeat(short encoder,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureRepeatStatus(short encoder,out short pCount);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureRepeatPos(short encoder, out Int32 pValue, short startNum, short count);
        [DllImport("gts.dll")]
        public static extern short GT_SetCaptureRepeatFifoMode(short encoder, short mode);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureRepeatFifoMode(short encoder, out short pMode);

        [DllImport("gts.dll")]
        public static extern short GTN_SetAuTrigger(short core, short i, ref TTriggerEx pTrigger);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAuTrigger(short core, short i, out TTriggerEx pTrigger);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearAuTriggerStatus(short core, short i);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAuTriggerStatus(short core, short i, out TTriggerStatusEx pTriggerStatusEx, short count);



        [DllImport("gts.dll")]
        public static extern short GTN_SetTrigger(short core,short i,ref TTrigger pTrigger);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTrigger(short core,short i,out TTrigger pTrigger);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTriggerEx(short core, short i, ref TTriggerEx pTrigger);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTriggerEx(short core, short i,out TTriggerEx pTrigger);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTriggerStatus(short core,short i,out TTriggerStatus pTriggerStatus,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTriggerStatusEx(short core,short i,out TTriggerStatusEx pTriggerStatusEx,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearTriggerStatus(short core,short i);
        [DllImport("gts.dll")]
        public static extern short GTN_DisableTrigger(short core, short i);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCaptureMode(short core,short encoder,short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCaptureMode(short core,short encoder,out short pMode,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCaptureStatus(short core,short encoder,out short pStatus,out Int32 pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCaptureSense(short core,short encoder,short mode,short sense);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearCaptureStatus(short core,short encoder);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCaptureRepeat(short core,short encoder,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCaptureRepeatStatus(short core,short encoder,out short pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCaptureRepeatPos(short core, short encoder, out Int32 pValue, short startNum, short count);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCaptureRepeatFifoMode(short core, short encoder, short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCaptureRepeatFifoMode(short core, short encoder, out short pMode);

        public const short TRIGGER_DELTA_MODE_DEFAULT = 0;
        public const short TRIGGER_DELTA_CHECKPOINT_MODE_AUTO = 0;
        public const short TRIGGER_DELTA_CHECKPOINT_MODE_MANUAL = 1;
        public struct TCheckpoint
        {
	        public short mode;
            public Int32 offset;
	        public short fifoIndex;
            public UInt32 crossCount;
	        public short fifoDataCount;
	        public short dataReady;
            public Int32 data;
	        public short dataIndex;
        } 
        public struct TTriggerDeltaPrm
        {
	        public short mode;
	        public short dir;
	        public short triggerIndex0;
            public short triggerIndex1;
        } 

        public struct TTriggerDeltaInfo
        {
	        public short enable;
	        public short checkpointCount;
	        public short fifoDataCount;
	        public short lostCount;
        }
        [DllImport("gts.dll")]
        public static extern short GT_ClearTriggerDelta(short index,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_AddTriggerDeltaCheckpoint(short index, short mode, Int32 offset, short fifo, ref short pIndex);
        [DllImport("gts.dll")]
        public static extern short GT_ReadTriggerDeltaCheckpointData(short index, short checkpointIndex, out Int32 pBuf, short count, out short pReadCount);
        [DllImport("gts.dll")]
        public static extern short GT_WriteTriggerDeltaCheckpointData(short index, short checkpointIndex, ref Int32 pBuf, short count, ref short pWriteCount);
        [DllImport("gts.dll")]
        public static extern short GT_SetTriggerDeltaPrm(short index,ref TTriggerDeltaPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetTriggerDeltaPrm(short index,out TTriggerDeltaPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetTriggerDeltaCheckpoint(short index,short checkpointIndex,out TCheckpoint pCheckpoint);
        [DllImport("gts.dll")]
        public static extern short GT_GetTriggerDeltaInfo(short index,out TTriggerDeltaInfo pTriggerDelta);
        [DllImport("gts.dll")]
        public static extern short GT_TriggerDeltaOn(short index);
        [DllImport("gts.dll")]
        public static extern short GT_TriggerDeltaOff(short index);
        [DllImport("gts.dll")]

        public static extern short GTN_ClearTriggerDelta(short core,short index,short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_AddTriggerDeltaCheckpoint(short core, short index, short mode, Int32 offset, short fifo, ref short pIndex);
        [DllImport("gts.dll")]
        public static extern short GTN_ReadTriggerDeltaCheckpointData(short core, short index, short checkpointIndex, out Int32 pBuf, short count, out short pReadCount);
        [DllImport("gts.dll")]
        public static extern short GTN_WriteTriggerDeltaCheckpointData(short core, short index, short checkpointIndex, ref Int32 pBuf, short count, ref short pWriteCount);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTriggerDeltaPrm(short core,short index,ref TTriggerDeltaPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTriggerDeltaPrm(short core,short index,out TTriggerDeltaPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTriggerDeltaCheckpoint(short core,short index,short checkpointIndex,out TCheckpoint pCheckpoint);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTriggerDeltaInfo(short core,short index,out TTriggerDeltaInfo pTriggerDelta);
        [DllImport("gts.dll")]
        public static extern short GTN_TriggerDeltaOn(short core,short index);
        [DllImport("gts.dll")]
        public static extern short GTN_TriggerDeltaOff(short core,short index);

        [DllImport("gts.dll")]
        public static extern short GT_LinkCaptureOffset(short encoder, short source);
        [DllImport("gts.dll")]
        public static extern short GT_SetCaptureOffset(short encoder, ref long pOffset, short count, long loop);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureOffset(short encoder, out long pOffset, out short pCount, out long pLoop);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureOffsetStatus(short encoder, out short pCount, out long pLoop, out long pCapturePos);

        [DllImport("gts.dll")]
        public static extern short GT_SetCaptureEncoder(short trigger, short encoder);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureWidth(short trigger, out short pWidth, short count);

        [DllImport("gts.dll")]
        public static extern short GTN_LinkCaptureOffset(short core, short encoder, short source);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCaptureOffset(short core, short encoder, ref long pOffset, short count, long loop);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCaptureOffset(short core, short encoder, out long pOffset, out short pCount, out long pLoop);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCaptureOffsetStatus(short core, short encoder, out short pCount, out long pLoop, out long pCapturePos);

        /*-----------------------------------------------------------*/
        /* Basic Motion                                              */
        /*-----------------------------------------------------------*/

        public struct TTrapPrm
        {
	        public double acc;
	        public double dec;
	        public double velStart;
	        public short  smoothTime;
        } 
        [DllImport("gts.dll")]
        public static extern short GT_Update(Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GT_SetPos(short profile, Int32 pos);
        [DllImport("gts.dll")]
        public static extern short GT_GetPos(short profile, out Int32 pPos);
        [DllImport("gts.dll")]
        public static extern short GT_SetVel(short profile,double vel);
        [DllImport("gts.dll")]
        public static extern short GT_GetVel(short profile,out double pVel);
        [DllImport("gts.dll")]

        public static extern short GT_PrfTrap(short profile);
        [DllImport("gts.dll")]
        public static extern short GT_SetTrapPrm(short profile,ref TTrapPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetTrapPrm(short profile,out TTrapPrm pPrm);
        [DllImport("gts.dll")]

        public static extern short GTN_Update(short core, Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPos(short core, short profile, Int32 pos);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPos(short core, short profile, out Int32 pPos);
        [DllImport("gts.dll")]
        public static extern short GTN_SetVel(short core,short profile,double vel);
        [DllImport("gts.dll")]
        public static extern short GTN_GetVel(short core,short profile,out double pVel);
        [DllImport("gts.dll")]

        public static extern short GTN_PrfTrap(short core,short profile);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTrapPrm(short core,short profile,ref TTrapPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTrapPrm(short core,short profile,out TTrapPrm pPrm);
        

        public struct TJogPrm
        {
	        public double acc;
	        public double dec;
	        public double smooth;
        }
        [DllImport("gts.dll")]
        public static extern short GT_PrfJog(short profile);
        [DllImport("gts.dll")]
        public static extern short GT_SetJogPrm(short profile,ref TJogPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetJogPrm(short profile,out TJogPrm pPrm);
        [DllImport("gts.dll")]

        public static extern short GTN_PrfJog(short core,short profile);
        [DllImport("gts.dll")]
        public static extern short GTN_SetJogPrm(short core,short profile,ref TJogPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetJogPrm(short core,short profile,out TJogPrm pPrm);

        public const short PT_MODE_STATIC = 0;
        public const short PT_MODE_DYNAMIC = 1;

        public const short PT_SEGMENT_NORMAL = 0;
        public const short PT_SEGMENT_EVEN = 1;
        public const short PT_SEGMENT_STOP = 2;

        public struct TPtInfo 
        {
	        public double prfPos;
            public Int32 loop;
	        public short mode;
	        public short fifoUse;
	        public short fifoPlace;
	        public short segmentNumber;
            public UInt32 segmentReceive1;
            public UInt32 segmentReceive2;
            public UInt32 segmentExecute1;
            public UInt32 segmentExecute2;
            public UInt32 bufferReceive1;
            public UInt32 bufferReceive2;
            public UInt32 bufferExecute1;
            public UInt32 bufferExecute2;
        } 
        [DllImport("gts.dll")]
        public static extern short GT_PrfPt(short profile,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_SetPtLoop(short profile, Int32 loop);
        [DllImport("gts.dll")]
        public static extern short GT_GetPtLoop(short profile, out Int32 pLoop);
        [DllImport("gts.dll")]
        public static extern short GT_PtSpace(short profile,out short pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_PtSpaceEx(short profile,out short pSpace,out short pListSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_PtData(short profile, double pos, Int32 time, short type);
        [DllImport("gts.dll")]
        public static extern short GT_PtClear(short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_PtStart(Int32 mask, Int32 option);
        [DllImport("gts.dll")]
        public static extern short GT_SetPtMemory(short profile,short memory);
        [DllImport("gts.dll")]
        public static extern short GT_GetPtMemory(short profile,out short pMemory);
        [DllImport("gts.dll")]
        public static extern short GT_SetPtPrecisionMode(short profile,short precisionMode);
        [DllImport("gts.dll")]
        public static extern short GT_GetPtPrecisionMode(short profile,out short pPrecisionMode);
        [DllImport("gts.dll")]
        public static extern short GT_GetPtInfo(short profile,out TPtInfo pPtInfo);
        [DllImport("gts.dll")]
        public static extern short GT_SetPtLink(short profile,short fifo,short list);
        [DllImport("gts.dll")]
        public static extern short GT_GetPtLink(short profile,short fifo,out short pList);
        [DllImport("gts.dll")]
        public static extern short GT_PtDoBit(short profile,short doType,short index,short value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_PtAo(short profile,short aoType,short index,double value,short fifo);
        //[DllImport("gts.dll")]
        //public static extern short GTN_PosCurrFeedForward(short core,short profile,double pos,long time,short torque,short type,short fifo);
       
        [DllImport("gts.dll")]
        public static extern short GTN_PrfPt(short core,short profile,short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPtLoop(short core, short profile, Int32 loop);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPtLoop(short core, short profile, out Int32 pLoop);
        [DllImport("gts.dll")]
        public static extern short GTN_PtSpace(short core,short profile,out short pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_PtSpaceEx(short core,short profile,out short pSpace,out short pListSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_PtData(short core,short profile,double pos,Int32 time,short type,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_PtClear(short core,short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_PtStart(short core, Int32 mask, Int32 option);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPtMemory(short core,short profile,short memory);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPtMemory(short core,short profile,out short pMemory);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPtPrecisionMode(short core,short profile,short precisionMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPtPrecisionMode(short core,short profile,out short pPrecisionMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPtInfo(short core,short profile,out TPtInfo pPtInfo);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPtLink(short core,short profile,short fifo,short list);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPtLink(short core,short profile,short fifo,out short pList);
        [DllImport("gts.dll")]
        public static extern short GTN_PtDoBit(short core,short profile,short doType,short index,short value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_PtAo(short core,short profile,short aoType,short index,double value,short fifo);



        public struct TPvtTableMovePrm
        {	
	        public short tableId;
            public Int32 distance;					
	        public double vm;						
	        public double am;						
	        public double jm;						
	        public double time;					
        } 
        [DllImport("gts.dll")]
        public static extern short GT_PrfPvt(short profile);
        [DllImport("gts.dll")]
        public static extern short GT_SetPvtLoop(short profile, Int32 loop);
        [DllImport("gts.dll")]
        public static extern short GT_GetPvtLoop(short profile, out Int32 pLoopCount, out Int32 pLoop);
        [DllImport("gts.dll")]
        public static extern short GT_PvtStatus(short profile,out short pTableId,out double pTime,short count);
        [DllImport("gts.dll")]
        public static extern short GT_PvtStart(Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableSelect(short profile,short tableId);
        
        
        [DllImport("gts.dll")]
        public static extern short GT_PvtTable(short tableId, Int32 count, ref double pTime, ref double pPos, ref double pVel);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableEx(short tableId, Int32 count, ref double pTime, ref double pPos, ref double pVelBegin, ref double pVelEnd);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableComplete(short tableId, Int32 count, ref double pTime, ref double pPos, ref double pA, ref double pB, ref double pC, double velBegin, double velEnd);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTablePercent(short tableId, Int32 count, ref double pTime, ref double pPos, ref double pPercent, double velBegin);
        [DllImport("gts.dll")]
        public static extern short GT_PvtPercentCalculate(Int32 n, ref double pTime, ref double pPos, ref double pPercent, double velBegin, ref double pVel);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableContinuousEx(short tableId,Int32 n,ref double pPos,ref double pVel,ref double pAccPercent,ref double pDecPercent,ref double pVelMax,ref double pAcc,ref double pDec,double timeBegin);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableContinuous(short tableId, Int32 count, ref double pPos, ref double pVel, ref double pPercent, ref double pVelMax, ref double pAcc, ref double pDec, double timeBegin);
        [DllImport("gts.dll")]
        public static extern short GT_PvtContinuousCalculate(Int32 n, ref double pPos, ref double pVel, ref double pPercent, ref double pVelMax, ref double pAcc, ref double pDec, ref double pTime);


        [DllImport("gts.dll")]
        public static extern short GT_PvtTableMove(short tableId, Int32 distance, double vm, double am, double jm, ref double pTime);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableMove2(short tableId,  Int32 distance,double vm,double am,double jm,ref double pTime);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableMovePercent(short tableId, Int32 distance, double vm, double acc, double pa1, double pa2, double dec, double pd1, double pd2, ref double pVel, ref double pAcc, ref double pDec, ref double pTime);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableMovePercentEx(short tableId, Int32 distance, double vm,
	    double acc,double pa1,double pa2,double ma,
	    double dec,double pd1,double pd2,double md,
	    ref double pVel,ref double pAcc,ref double pDec,ref double pTime);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableMoveTogether(short tableCount,ref TPvtTableMovePrm pPvtTableMovePrm);
        [DllImport("gts.dll")]


        public static extern short GTN_PrfPvt(short core,short profile);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPvtLoop(short core, short profile, Int32 loop);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPvtLoop(short core, short profile, out Int32 pLoopCount, out Int32 pLoop);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtStatus(short core,short profile,out short pTableId,out double pTime,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtStart(short core, Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtTableSelect(short core,short profile,short tableId);
        [DllImport("gts.dll")]

        public static extern short GTN_PvtTable(short core, short tableId, Int32 count, ref double pTime, ref double pPos, ref double pVel);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtTableEx(short core, short tableId, Int32 count, ref double pTime, ref double pPos, ref double pVelBegin, ref double pVelEnd);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtTableComplete(short core, short tableId, Int32 count, ref double pTime, ref double pPos, ref double pA, ref double pB, ref double pC, double velBegin, double velEnd);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtTablePercent(short core, short tableId, Int32 count, ref double pTime, ref double pPos, ref double pPercent, double velBegin);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtPercentCalculate(short core, Int32 n, ref double pTime, ref double pPos, ref double pPercent, double velBegin, ref double pVel);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtTableContinuous(short core, short tableId, Int32 count, ref double pPos, ref double pVel, ref double pPercent, ref double pVelMax, ref double pAcc, ref double pDec, double timeBegin);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtContinuousCalculate(short core, Int32 n, ref double pPos, ref double pVel, ref double pPercent, ref double pVelMax, ref double pAcc, ref double pDec, ref double pTime);
       
        [DllImport("gts.dll")]
        public static extern short GTN_PvtTableMove(short core, short tableId, Int32 distance, double vm, double am, double jm, ref double pTime);
       [DllImport("gts.dll")]
        public static extern short GTN_PvtTableMove2(short core, short tableId, long distance, double vm, double am, double jm, ref double pTime);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtTableMovePercent(short core, short tableId, Int32 distance, double vm,
	    double acc,double pa1,double pa2,
	    double dec,double pd1,double pd2,
	    ref double pVel,ref double pAcc,ref double pDec,ref double pTime);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtTableMovePercentEx(short core, short tableId, Int32 distance, double vm,
	    double acc,double pa1,double pa2,double ma,
	    double dec,double pd1,double pd2,double md,
	    ref double pVel,ref double pAcc,ref double pDec,ref double pTime);
        [DllImport("gts.dll")]
        public static extern short GTN_PvtTableMoveTogether(short core,short tableCount,ref TPvtTableMovePrm pPvtTableMovePrm);
        public const short GEAR_MASTER_ENCODER = 1;
        public const short GEAR_MASTER_PROFILE = 2;
        public const short GEAR_MASTER_AXIS = 3;
        public const short GEAR_MASTER_AU_ENCODER  = 4;

        public const short GEAR_MASTER_ENCODER_OTHER = 101;
        public const short GEAR_MASTER_AXIS_OTHER = 103;

        public const short GEAR_EVENT_START = 1;
        public const short GEAR_EVENT_PASS = 2;
        public const short GEAR_EVENT_AREA = 5;
        [DllImport("gts.dll")]
        public static extern short GT_PrfGear(short profile,short dir);
        [DllImport("gts.dll")]
        public static extern short GT_SetGearMaster(short profile,short masterIndex,short masterType,short masterItem);
        [DllImport("gts.dll")]
        public static extern short GT_GetGearMaster(short profile,out short  pMasterIndex,out short  pMasterType,out short  pMasterItem);
        [DllImport("gts.dll")]
        public static extern short GT_SetGearRatio(short profile, Int32 masterEven, Int32 slaveEven, Int32 masterSlope);
        [DllImport("gts.dll")]
        public static extern short GT_GetGearRatio(short profile, out Int32 pMasterEven, out Int32 pSlaveEven, out Int32 pMasterSlope);
        [DllImport("gts.dll")]
        public static extern short GT_GearStart(Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GT_SetGearEvent(short profile, short gearevent, Int32 startPara0, Int32 startPara1);
        [DllImport("gts.dll")]
        public static extern short GT_GetGearEvent(short profile, out short pEvent, out Int32 pStartPara0, out Int32 pStartPara1);
        [DllImport("gts.dll")]


        public static extern short GTN_PrfGear(short core,short profile,short dir);
        [DllImport("gts.dll")]
        public static extern short GTN_SetGearMaster(short core,short profile,short masterIndex,short masterType,short masterItem);
        [DllImport("gts.dll")]
        public static extern short GTN_GetGearMaster(short core,short profile,out short  pMasterIndex,out short  pMasterType,out short  pMasterItem);
        [DllImport("gts.dll")]
        public static extern short GTN_SetGearRatio(short core, short profile, Int32 masterEven, Int32 slaveEven, Int32 masterSlope);
        [DllImport("gts.dll")]
        public static extern short GTN_GetGearRatio(short core, short profile, out Int32 pMasterEven, out Int32 pSlaveEven, out Int32 pMasterSlope);
        [DllImport("gts.dll")]
        public static extern short GTN_GearStart(short core, Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GTN_SetGearEvent(short core, short profile, short gearevent, Int32 startPara0, Int32 startPara1);
        [DllImport("gts.dll")]
        public static extern short GTN_GetGearEvent(short core, short profile, out short pEvent, out Int32 pStartPara0, out Int32 pStartPara1);
        
        public const short FOLLOW_SWITCH_SEGMENT = 1;
        public const short FOLLOW_SWITCH_TABLE = 2;

        public const short FOLLOW_MASTER_ENCODER = 1;
        public const short FOLLOW_MASTER_PROFILE = 2;
        public const short FOLLOW_MASTER_AXIS = 3;
        public const short FOLLOW_MASTER_AU_ENCODER = 4;

        public const short FOLLOW_MASTER_ENCODER_OTHER = 101;
        public const short FOLLOW_MASTER_AXIS_OTHER = 103;

        public const short FOLLOW_EVENT_START = 1;
        public const short FOLLOW_EVENT_PASS = 2;

        public const short FOLLOW_SEGMENT_NORMAL = 0;
        public const short FOLLOW_SEGMENT_EVEN = 1;
        public const short FOLLOW_SEGMENT_STOP = 2;
        public const short FOLLOW_SEGMENT_CONTINUE = 3;
        [DllImport("gts.dll")]
        public static extern short GT_PrfFollow(short profile,short dir);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowMaster(short profile,short masterIndex,short masterType,short masterItem);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowMaster(short profile,out short  pMasterIndex,out short  pMasterType,out short  pMasterItem);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowLoop(short profile, Int32 loop);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowLoop(short profile, out Int32 pLoop);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowEvent(short profile, short followevent, short masterDir, Int32 pos);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowEvent(short profile, out short pEvent, out short pMasterDir, out Int32 pPos);
        [DllImport("gts.dll")]
        public static extern short GT_FollowSpace(short profile,out short  pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowData(short profile, Int32 masterSegment, double slaveSegment, short type, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowClear(short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowStart(Int32 mask, Int32 option);
        [DllImport("gts.dll")]
        public static extern short GT_FollowSwitch(Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowMemory(short profile,short memory);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowMemory(short profile,out short  pMemory);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowStatus(short profile,out short  pFifoNum,out short  pSwitchStatus);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowPhasing(short profile,short profilePhasing);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowPhasing(short profile,out short  pProfilePhasing);
        [DllImport("gts.dll")]


        public static extern short GT_PrfFollowEx(short profile,short dir);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowMasterEx(short profile,short masterIndex,short masterType,short masterItem);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowMasterEx(short profile,out short  pMasterIndex,out short  pMasterType,out short  pMasterItem);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowLoopEx(short profile, Int32 loop);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowLoopEx(short profile, out Int32 pLoop);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowEventEx(short profile, short followevent, short masterDir, Int32 pos);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowEventEx(short profile, out short pEvent, out short pMasterDir, out Int32 pPos);
        [DllImport("gts.dll")]
        public static extern short GT_FollowSpaceEx(short profile,out short  pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowDataPercentEx(short profile,double masterSegment,double slaveSegment,short type,short percent,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowClearEx(short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowStartEx(Int32 mask, Int32 option);
        [DllImport("gts.dll")]
        public static extern short GT_FollowSwitchEx(Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowMemoryEx(short profile,short memory);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowMemoryEx(short profile,out short  pMemory);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowStatusEx(short profile,out short  pFifoNum,out short  pSwitchStatus);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowPhasingEx(short profile,short profilePhasing);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowPhasingEx(short profile,out short  pProfilePhasing);
        [DllImport("gts.dll")]
        public static extern short GT_FollowSwitchNowEx(short profile,short method,short buffer,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowDataPercent2Ex(short profile,double masterSegment,double slaveSegment,double velBeginRatio,double velEndRatio,short percent,out short  pPercent1,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowDataPercent2Ex(double masterPos,double v1,double v2,double p,double p1,out double pSlavePos);
        [DllImport("gts.dll")]
        public static extern short GT_FollowDoBitEx(short profile,short doType,short index,short value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowDelayEx(short profile, UInt32 delayTime, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowDiBitEx(short profile, short diType, short index, short value, UInt32 time, short fifo);
        
        
        [DllImport("gts.dll")]
        public static extern short GTN_PrfFollow(short core,short profile,short dir);
        [DllImport("gts.dll")]
        public static extern short GTN_SetFollowMaster(short core,short profile,short masterIndex,short masterType,short masterItem);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowMaster(short core,short profile,out short  pMasterIndex,out short  pMasterType,out short  pMasterItem);
        [DllImport("gts.dll")]
        public static extern short GTN_SetFollowLoop(short core, short profile, Int32 loop);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowLoop(short core, short profile, out Int32 pLoop);
        [DllImport("gts.dll")]
        public static extern short GTN_SetFollowEvent(short core, short profile, short followevent, short masterDir, Int32 pos);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowEvent(short core, short profile, out short pEvent, out short pMasterDir, out Int32 pPos);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowSpace(short core,short profile,out short  pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowData(short core, short profile, Int32 masterSegment, double slaveSegment, short type, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowClear(short core,short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowStart(short core, Int32 mask, Int32 option);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowSwitch(short core,Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GTN_SetFollowMemory(short core,short profile,short memory);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowMemory(short core,short profile,out short  pMemory);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowStatus(short core,short profile,out short  pFifoNum,out short  pSwitchStatus);
        [DllImport("gts.dll")]
        public static extern short GTN_SetFollowPhasing(short core,short profile,short profilePhasing);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowPhasing(short core,short profile,out short  pProfilePhasing);
        [DllImport("gts.dll")]

        public static extern short GTN_PrfFollowEx(short core,short profile,short dir);
        [DllImport("gts.dll")]
        public static extern short GTN_SetFollowMasterEx(short core,short profile,short masterIndex,short masterType,short masterItem);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowMasterEx(short core,short profile,out short  pMasterIndex,out short  pMasterType,out short  pMasterItem);
        [DllImport("gts.dll")]
        public static extern short GTN_SetFollowLoopEx(short core, short profile, Int32 loop);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowLoopEx(short core, short profile, out Int32 pLoop);
        [DllImport("gts.dll")]
        public static extern short GTN_SetFollowEventEx(short core, short profile, short followevent, short masterDir, Int32 pos);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowEventEx(short core, short profile, out short pEvent, out short pMasterDir, out Int32 pPos);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowSpaceEx(short core,short profile,out short  pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowDataPercentEx(short core,short profile,double masterSegment,double slaveSegment,short type,short percent,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowClearEx(short core,short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowStartEx(short core, Int32 mask, Int32 option);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowSwitchEx(short core, Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GTN_SetFollowMemoryEx(short core,short profile,short memory);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowMemoryEx(short core,short profile,out short  pMemory);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowStatusEx(short core,short profile,out short  pFifoNum,out short  pSwitchStatus);
        [DllImport("gts.dll")]
        public static extern short GTN_SetFollowPhasingEx(short core,short profile,short profilePhasing);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowPhasingEx(short core,short profile,out short  pProfilePhasing);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowSwitchNowEx(short core,short profile,short method,short buffer,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowDataPercent2Ex(short core,short profile,double masterSegment,double slaveSegment,double velBeginRatio,double velEndRatio,short percent,out short  pPercent1,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFollowDataPercent2Ex(short core,double masterPos,double v1,double v2,double p,double p1,out double pSlavePos);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowDoBitEx(short core,short profile,short doType,short index,short value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowDelayEx(short core,short profile,UInt32 delayTime,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_FollowDiBitEx(short core,short profile,short diType,short index,short value,UInt32 time,short fifo);

        [DllImport("gts.dll")]
        public static extern short GT_SetFollowVirtualSeg(short profile, short segment, short axis, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowVirtualSeg(short profile, out short pSegment, out short pAxis, short fifo);

        [DllImport("gts.dll")]
        public static extern short GT_GetFollowVirtualErr(short profile, out double pVirtualErr);
        [DllImport("gts.dll")]
        public static extern short GT_ClearFollowVirtualErr(short profile);


        public struct TMoveAbsolutePrm
        {
	        public Int32 pos;
	        public double vel;
	        public double acc;
	        public double dec;
	        public short percent;
        } 
        [DllImport("gts.dll")]
        public static extern short GT_MoveAbsolute(short profile,ref TMoveAbsolutePrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetMoveAbsolute(short profile,out TMoveAbsolutePrm pPrm);
        [DllImport("gts.dll")]

        public static extern short GTN_MoveAbsolute(short core,short profile,ref TMoveAbsolutePrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetMoveAbsolute(short core,short profile,out TMoveAbsolutePrm pPrm);


        public struct  TTransformOrthogonal
        {
	        public short source;
	        public short enable;
	        public short x;
	        public short y;
	        public double theta;		// degree
        } 
        [DllImport("gts.dll")]
        public static extern short GT_SetTransformOrthogonal(short index,ref TTransformOrthogonal pOrthogonal);
        [DllImport("gts.dll")]
        public static extern short GT_GetTransformOrthogonal(short index,out TTransformOrthogonal pOrthogonal);
        [DllImport("gts.dll")]
        public static extern short GT_GetTransformOrthogonalPosition(short index,out double pPositionX,out double pPositionY);
        
        [DllImport("gts.dll")]
        public static extern short GTN_SetTransformOrthogonal(short core,short index,ref TTransformOrthogonal pOrthogonal);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTransformOrthogonal(short core,short index,out TTransformOrthogonal pOrthogonal);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTransformOrthogonalPosition(short core,short index,out double pPositionX,out double pPositionY);
        
        /*-----------------------------------------------------------*/
        /* Home                                                      */
        /*-----------------------------------------------------------*/
        public const short HOME_STAGE_IDLE = 0;
        public const short HOME_STAGE_START	= 1;

        public const short HOME_STAGE_SEARCH_LIMIT = 10;
        public const short HOME_STAGE_SEARCH_LIMIT_STOP	= 11;

        public const short HOME_STAGE_SEARCH_LIMIT_ESCAPE = 13;

        public const short HOME_STAGE_SEARCH_LIMIT_RETURN = 15;
        public const short HOME_STAGE_SEARCH_LIMIT_RETURN_STOP = 16;

        public const short HOME_STAGE_SEARCH_HOME = 20;

        public const short HOME_STAGE_SEARCH_HOME_RETURN = 25;

        public const short HOME_STAGE_SEARCH_INDEX = 30;

        public const short HOME_STAGE_SEARCH_GPI = 40;

        public const short HOME_STAGE_SEARCH_GPI_RETURN = 45;

        public const short HOME_STAGE_GO_HOME = 80;

        public const short HOME_STAGE_END = 100;

        public const short HOME_ERROR_NONE = 0;
        public const short HOME_ERROR_NOT_TRAP_MODE = 1;
        public const short HOME_ERROR_DISABLE = 2;
        public const short HOME_ERROR_ALARM = 3;
        public const short HOME_ERROR_STOP = 4;
        public const short HOME_ERROR_STAGE = 5;
        public const short HOME_ERROR_HOME_MODE	= 6;
        public const short HOME_ERROR_SET_CAPTURE_HOME = 7;
        public const short HOME_ERROR_NO_HOME = 8;
        public const short HOME_ERROR_SET_CAPTURE_INDEX	= 9;
        public const short HOME_ERROR_NO_INDEX = 10;

        public const short HOME_MODE_LIMIT = 10;
        public const short HOME_MODE_LIMIT_HOME	= 11;
        public const short HOME_MODE_LIMIT_INDEX = 12;
        public const short HOME_MODE_LIMIT_HOME_INDEX = 13;

        public const short HOME_MODE_HOME = 20;

        public const short HOME_MODE_HOME_INDEX	= 22;

        public const short HOME_MODE_INDEX = 30;

        public struct THomePrm
        {
	        public short mode;						// 回零模式
	        public short moveDir;					// 设置启动搜索时的运动方向
	        public short indexDir;					// 设置Index搜索方向
	        public short edge;						// 设置捕获沿
	        public short triggerIndex;				// 用于设置触发器
	        public short pad10;					// 保留1
            public short pad11;					// 保留1
            public short pad12;					// 保留1
	        public double velHigh;					// Home搜索速度
	        public double velLow;					// Index搜索速度
	        public double acc;
	        public double dec;
	        public short smoothTime;
	        public short pad20;					// 保留2
            public short pad21;					// 保留2
            public short pad22;					// 保留2
	        public Int32 homeOffset;				// 原点偏移
	        public Int32 searchHomeDistance;		// Home最大搜索距离，为0表示不限制
	        public Int32 searchIndexDistance;		// Index最大搜索距离，为0表示不限制
	        public Int32 escapeStep;				// 脱离步长
	        public Int32 pad31;					// 保留3
            public Int32 pad32;					// 保留3
        } 

        public struct THomeStatus
        {
	        public short run;
	        public short stage;
	        public short error;
	        public short pad1;
	        public Int32 capturePos;
	        public Int32 targetPos;
        } 
        [DllImport("gts.dll")]
        public static extern short GT_GoHome(short axis,ref THomePrm pHomePrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetHomePrm(short profile,out THomePrm pHomePrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetHomeStatus(short profile,out THomeStatus pHomeStatus);
        [DllImport("gts.dll")]

        public static extern short GTN_GoHome(short core,short axis,ref THomePrm pHomePrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetHomePrm(short core,short axis,out THomePrm pHomePrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetHomeStatus(short core,short axis,out THomeStatus pHomeStatus);

        [DllImport("gts.dll")]
        public static extern short GT_HandwheelInit(short mode);
        [DllImport("gts.dll")]
        public static extern short GT_SetHandwheelStopDec(short slave, double decSmoothStop, double decAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GT_StartHandwheel(short slave, short master, short masterEven, short slaveEven, short intervalTime, double acc, double dec, double vel, short stopWaitTime);
        [DllImport("gts.dll")]
        public static extern short GT_EndHandwheel(short slave);

        [DllImport("gts.dll")]
        public static extern short GTN_HandwheelInit(short core, short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetHandwheelStopDec(short core, short slave, double decSmoothStop, double decAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GTN_StartHandwheel(short core, short slave, short master, short masterEven, short slaveEven, short intervalTime, double acc, double dec, double vel, short stopWaitTime);
        [DllImport("gts.dll")]
        public static extern short GTN_EndHandwheel(short core, short slave);

        /*-----------------------------------------------------------*/
        /* PLC                                                       */
        /*-----------------------------------------------------------*/
        public const short PLC_THREAD_MAX =	32;
        public const short PLC_PAGE_MAX	= 32;
        public const short PLC_LOCAL_VAR_MAX = 1024;
        public const short PLC_ACCESS_VAR_COUNT_MAX = 8;

        public const short PLC_TIMER_TT	= 0;
        public const short PLC_TIMER_TF	= 1;
        public const short PLC_TIMER_TTF = 2;

        public const short PLC_COUNTER_EQ = 0;
        public const short PLC_COUNTER_LE = 1;
        public const short PLC_COUNTER_GE = 2;

        public const short PLC_COUNTER_EDGE_UP = 0;
        public const short PLC_COUNTER_EDGE_DOWN = 1;
        public const short PLC_COUNTER_EDGE_UP_DOWN = 2;

        public const short PLC_FLANK_UP	= 0;
        public const short PLC_FLANK_DOWN = 1;
        public const short PLC_FLANK_UP_DOWN = 2;

        public enum EPlcBind
        {
	        PLC_BIND_NONE,
	        PLC_BIND_DI,
	        PLC_BIND_DO,
	        PLC_BIND_TIMER,
	        PLC_BIND_COUNTER,
	        PLC_BIND_FLANK,
	        PLC_BIND_SRFF,
        } 

        public struct TVarInfo
        {
	        public short id;
	        public short dataType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String name;
        } 

        public struct TBindDi
        {
	        public short diType;
	        public short index;
	        public short reverse;
        } 

        public struct TBindDo
        {
	        public short doType;
	        public short index;
	        public short reverse;
        } 

        public struct TBindTimer
        {
	        public short timerType;
	        public Int32 delay;
	        public short inputVarId;
        } 

        public struct TBindCounter
        {
	        public short counterType;
	        public short edge;
	        public Int32 init;
	        public Int32 target;
	        public Int32 begin;
	        public Int32 end;
	        public short dir;
	        public Int32 unit;
	        public short inputVarId;
	        public short resetVarId;
        } 

        public struct TBindFlank
        {
	        public short flankType;
	        public short inputVarId;
        } 

        public struct TBindSrff
        {
	        public short setVarId;
	        public short resetVarId;
        } 

        public struct TCompileInfo
        {
	       public string pFileName;
	       public short  pLineNo;
	       public string pMessage;
        } 

        public struct TThreadSts
        {
	        public short  run;
	        public short  error;
	        public double result;
	        public short  line;
        } 

        [DllImport("gts.dll")]
        public static extern short GT_Compile(string pFileName,out TCompileInfo pWrongInfo);
        [DllImport("gts.dll")]
        public static extern short GT_Download(string pFileName);
        [DllImport("gts.dll")]
        public static extern short GT_GetFunId(string pFunName,out short  pFunId);
        [DllImport("gts.dll")]
        public static extern short GT_GetVarId(string pFunName,string pVarName,out TVarInfo pVarInfo);
        [DllImport("gts.dll")]
        public static extern short GT_Bind(short thread,short funId,short page);
        [DllImport("gts.dll")]
        public static extern short GT_RunThread(short thread);
        [DllImport("gts.dll")]
        public static extern short GT_RunThreadPeriod(short thread,short ms,short priority);
        
        [DllImport("gts.dll")]
        public static extern short GT_StopThread(short thread);
        [DllImport("gts.dll")]
        public static extern short GT_PauseThread(short thread);
        [DllImport("gts.dll")]
        public static extern short GT_GetThreadSts(short thread,out TThreadSts pThreadSts);
        [DllImport("gts.dll")]
        public static extern short GT_GetThreadTime(short thread,out short  pPeriod,out double pExecuteTime,out double pExecuteTimeMax);
        [DllImport("gts.dll")]
        public static extern short GT_SetVarValue(short page,ref TVarInfo pVarInfo,out double pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetVarValue(short page,ref TVarInfo pVarInfo,out double pValue,short count);
        
        [DllImport("gts.dll")]
        public static extern short GT_UnbindVar(short thread);
        [DllImport("gts.dll")]
        public static extern short GT_BindDi(short thread,ref TVarInfo pVarInfo,ref TBindDi pBindDi);
        [DllImport("gts.dll")]
        public static extern short GT_BindDo(short thread,ref TVarInfo pVarInfo,ref TBindDo pBindDo);
        [DllImport("gts.dll")]
        public static extern short GT_BindTimer(short thread,ref TVarInfo pVarInfo,ref TBindTimer pBindTimer);
        [DllImport("gts.dll")]
        public static extern short GT_BindCounter(short thread,ref TVarInfo pVarInfo,ref TBindCounter pBindCounter);
        [DllImport("gts.dll")]
        public static extern short GT_BindFlank(short thread,ref TVarInfo pVarInfo,ref TBindFlank pBindFlank);
        [DllImport("gts.dll")]
        public static extern short GT_BindSrff(short thread,ref TVarInfo pVarInfo,ref TBindSrff pBindSrff);
        
        [DllImport("gts.dll")]
        public static extern short GT_GetBindDi(out TVarInfo pVarInfo,out TBindDi pBindDi);
        [DllImport("gts.dll")]
        public static extern short GT_GetBindDo(out TVarInfo pVarInfo,out TBindDo pBindDo);
        [DllImport("gts.dll")]
        public static extern short GT_GetBindTimer(out TVarInfo pVarInfo,out TBindTimer pBindTimer,out Int32  pCount);
        [DllImport("gts.dll")]
        public static extern short GT_GetBindCounter(out TVarInfo pVarInfo,out TBindCounter pBindCounter,out Int32  pUnitCount,out Int32  pCount);
        [DllImport("gts.dll")]
        public static extern short GT_GetBindFlank(out TVarInfo pVarInfo,out TBindFlank pBindFlank);
        [DllImport("gts.dll")]
        public static extern short GT_GetBindSrff(out TVarInfo pVarInfo,out TBindSrff pBindSrff);

        [DllImport("gts.dll")]
        public static extern short GTN_Compile(string pFileName,ref TCompileInfo pWrongInfo);
        [DllImport("gts.dll")]
        public static extern short GTN_Download(short core,string pFileName);
        [DllImport("gts.dll")]
        public static extern short GTN_GetFunId(string pFunName,out short  pFunId);
        [DllImport("gts.dll")]
        public static extern short GTN_GetVarId(string pFunName,string pVarName,out TVarInfo pVarInfo);
        [DllImport("gts.dll")]
        public static extern short GTN_Bind(short core,short thread,short funId,short page);
        [DllImport("gts.dll")]
        public static extern short GTN_RunThread(short core,short thread);
        [DllImport("gts.dll")]
        public static extern short GTN_RunThreadPeriod(short core,short thread,short ms,short priority);
        [DllImport("gts.dll")]
        public static extern short GTN_StopThread(short core,short thread);
        [DllImport("gts.dll")]
        public static extern short GTN_PauseThread(short core,short thread);
        [DllImport("gts.dll")]
        public static extern short GTN_GetThreadSts(short core,short thread,out TThreadSts pThreadSts);
        [DllImport("gts.dll")]
        public static extern short GTN_GetThreadTime(short core,short thread,out short  pPeriod,out double pExecuteTime,out double pExecuteTimeMax);
        [DllImport("gts.dll")]
        public static extern short GTN_SetVarValue(short core,short page,ref TVarInfo pVarInfo,ref double pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetVarValue(short core, short page, ref TVarInfo pVarInfo, out double pValue, short count);
        [DllImport("gts.dll")]

        public static extern short GTN_UnbindVar(short core,short thread);
        [DllImport("gts.dll")]
        public static extern short GTN_BindDi(short core,short thread,ref TVarInfo pVarInfo,ref TBindDi pBindDi);
        [DllImport("gts.dll")]
        public static extern short GTN_BindDo(short core,short thread,ref TVarInfo pVarInfo,ref TBindDo pBindDo);
        [DllImport("gts.dll")]
        public static extern short GTN_BindTimer(short core,short thread,ref TVarInfo pVarInfo,ref TBindTimer pBindTimer);
        [DllImport("gts.dll")]
        public static extern short GTN_BindCounter(short core,short thread,ref TVarInfo pVarInfo,ref TBindCounter pBindCounter);
        [DllImport("gts.dll")]
        public static extern short GTN_BindFlank(short core,short thread,ref TVarInfo pVarInfo,ref TBindFlank pBindFlank);
        [DllImport("gts.dll")]
        public static extern short GTN_BindSrff(short core,short thread,ref TVarInfo pVarInfo,ref TBindSrff pBindSrff);
        [DllImport("gts.dll")]

        public static extern short GTN_GetBindDi(short core,out TVarInfo pVarInfo,out TBindDi pBindDi);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindDo(short core,out TVarInfo pVarInfo,out TBindDo pBindDo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindTimer(short core,out TVarInfo pVarInfo,out TBindTimer pBindTimer,out Int32  pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindCounter(short core,out TVarInfo pVarInfo,out TBindCounter pBindCounter,out Int32  pUnitCount,out Int32  pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindFlank(short core,out TVarInfo pVarInfo,out TBindFlank pBindFlank);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindSrff(short core,out TVarInfo pVarInfo,out TBindSrff pBindSrff);
        [DllImport("gts.dll")]

        public static extern short GTN_GetBindDiCount(short core,short thread,out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindDoCount(short core,short thread,out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindTimerCount(short core,short thread,out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindCounterCount(short core,short thread,out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindFlankCount(short core,short thread,out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindSrffCount(short core,short thread,out short  pCount);
        [DllImport("gts.dll")]

        public static extern short GTN_GetBindDiInfo(short core,short thread,short index,out short  pVar,out TBindDi pBindDi);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindDoInfo(short core,short thread,short index,out short  pVar,out TBindDo pBindDo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindTimerInfo(short core,short thread,short index,out short  pVar,out TBindTimer pBindTimer);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindCounterInfo(short core,short thread,short index,out short  pVar,out TBindCounter pBindCounter);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindFlankInfo(short core,short thread,short index,out short  pVar,out TBindFlank pBindFlank);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBindSrffInfo(short core,short thread,short index,out short  pVar,out TBindSrff pBindSrff);

        public struct TThreadStatus
        {
            public short link;
            public UInt32 address;
            public short size;
            public UInt32 page;
            public short delay;
            public short priority;
            public short ptr;
            public short status;
            public short error;
            public short result1;
            public short result2;
            public short result3;
            public short result4;
            public short resultType;
            public short breakpoint;
            public short period;
            public short count;
            public short function;
        } 

        [DllImport("gts.dll")]
        public static extern short GT_ClearPlc();
        [DllImport("gts.dll")]
        public static extern short GT_LoadPlc(short id,short returnType);
        [DllImport("gts.dll")]
        public static extern short GT_LoadPlcCommand(short id,short count,ref short pData);
        [DllImport("gts.dll")]
        public static extern short GT_StepThread(short thread);
        [DllImport("gts.dll")]
        public static extern short GT_RunThreadToBreakpoint(short thread,short line);
        [DllImport("gts.dll")]
        public static extern short GT_GetThread(short thread,out TThreadStatus pThread);

        [DllImport("gts.dll")]
        public static extern short GTN_StepThread(short core,short thread);
        /*-----------------------------------------------------------*/
        /* Interpolation                                             */
        /*-----------------------------------------------------------*/

        public const short INTERPOLATION_AXIS_MAX = 8;

        public const short CRD_OPERATION_DATA_EXT_MAX = 2;


        public const short CRD_BUFFER_MODE_DYNAMIC_DEFAULT = 0;
        public const short CRD_BUFFER_MODE_DYNAMIC_KEEP = 1;
        public const short CRD_BUFFER_MODE_STATIC_INPUT = 11;
        public const short CRD_BUFFER_MODE_STATIC_READY = 12;
        public const short CRD_BUFFER_MODE_STATIC_START  = 1;

        public const short INTERPOLATION_CIRCLE_PLAT_XY = 0;
        public const short INTERPOLATION_CIRCLE_PLAT_YZ = 1;
        public const short INTERPOLATION_CIRCLE_PLAT_ZX = 2;

        public const short INTERPOLATION_HELIX_CIRCLE_XY_LINE_Z = 0;
        public const short INTERPOLATION_HELIX_CIRCLE_YZ_LINE_X = 1;
        public const short INTERPOLATION_HELIX_CIRCLE_ZX_LINE_Y = 2;
        public const short INTERPOLATION_CIRCLE_DIR_CW = 0;
        public const short INTERPOLATION_CIRCLE_DIR_CCW = 1;

        public struct TCrdPrm
        {
	        public short dimension;                              // 坐标系维数
	        public short profile1;                             // 关联profile和坐标轴
            public short profile2;                             // 关联profile和坐标轴
            public short profile3;                             // 关联profile和坐标轴
            public short profile4;                             // 关联profile和坐标轴
            public short profile5;                             // 关联profile和坐标轴
            public short profile6;                             // 关联profile和坐标轴
            public short profile7;                             // 关联profile和坐标轴
            public short profile8;                             // 关联profile和坐标轴
	        public double synVelMax;                             // 最大合成速度
	        public double synAccMax;                             // 最大合成加速度
	        public short evenTime;                               // 最小匀速时间
	        public short setOriginFlag;                          // 设置原点坐标值标志,0:默认当前规划位置为原点位置;1:用户指定原点位置
	        public Int32 originPos1;                            // 用户指定的原点位置
            public Int32 originPos2;                            // 用户指定的原点位置
            public Int32 originPos3;                            // 用户指定的原点位置
            public Int32 originPos4;                            // 用户指定的原点位置
            public Int32 originPos5;                            // 用户指定的原点位置
            public Int32 originPos6;                            // 用户指定的原点位置
            public Int32 originPos7;                            // 用户指定的原点位置
            public Int32 originPos8;                            // 用户指定的原点位置
        }

        public struct TCrdBufOperation
        {
	        public short flag;                                   // 标志该插补段是否含有IO和延时
	        public ushort delay;                         // 延时时间
	        public short doType;                                 // 缓存区IO的类型,0:不输出IO
            public ushort doMask;                        // 缓存区IO的输出控制掩码
            public ushort doValue;                       // 缓存区IO的输出值
            public ushort dataExt1;     // 辅助操作扩展数据
            public ushort dataExt2;     // 辅助操作扩展数据
        }
         
        public struct TCrdData
        {
	        public short motionType;                             // 运动类型,0:直线插补,1:圆弧插补
	        public short circlePlat;                             // 圆弧插补的平面
	        public Int32 pos1;             // 当前段各轴终点位置
            public Int32 pos2;             // 当前段各轴终点位置
            public Int32 pos3;             // 当前段各轴终点位置
            public Int32 pos4;             // 当前段各轴终点位置
            public Int32 pos5;             // 当前段各轴终点位置
            public Int32 pos6;             // 当前段各轴终点位置
            public Int32 pos7;             // 当前段各轴终点位置
            public Int32 pos8;             // 当前段各轴终点位置
	        public double radius;                                  // 圆弧插补的半径
	        public short circleDir;                              // 圆弧旋转方向,0:顺时针;1:逆时针
	        public double center1;                               // 圆弧插补的圆心坐标
            public double center2;                               // 圆弧插补的圆心坐标
            public double center3;                               // 圆弧插补的圆心坐标
            public double vel;                                   // 当前段合成目标速度
	        public double acc;                                   // 当前段合成加速度
	        public short velEndZero;                             // 标志当前段的终点速度是否强制为0,0:不强制为0;1:强制为0
	        public TCrdBufOperation operation;                   // 缓存区延时和IO结构体

	        public double cos1;           // 当前段各轴对应的余弦值
            public double cos2;           // 当前段各轴对应的余弦值
            public double cos3;           // 当前段各轴对应的余弦值
            public double cos4;           // 当前段各轴对应的余弦值
            public double cos5;           // 当前段各轴对应的余弦值
            public double cos6;           // 当前段各轴对应的余弦值
            public double cos7;           // 当前段各轴对应的余弦值
            public double cos8;           // 当前段各轴对应的余弦值
	        public double velEnd;                                // 当前段合成终点速度
	        public double velEndAdjust;                          // 调整终点速度时用到的变量(前瞻模块)
	        public double r;                                     // 当前段合成位移量
        }

        public struct  TCrdTime
        {
	        public double time;
	        public Int32 segmentUsed;
	        public Int32 segmentHead;
	        public Int32 segmentTail;
        }

        public struct TBufFollowMaster
        {
            public short crdAxis;
            public short masterIndex;
            public short masterType;
        }

        public struct TBufFollowEventCross
        {
            public Int32 masterPos;
            public Int32 pad;
        }

        public struct TBufFollowEventTrigger
        {
            public short triggerIndex;
            public Int32 triggerOffset;
            public Int32 pad;
        }

        public struct TCrdFollowPrm 
        {
            public double velRatioMax;
            public double accRatioMax;
            public Int32 masterLead;
            public Int32 masterEven;
            public Int32 slaveEven;
            public short dir;
            public short smoothPercent;
            public short synchAlign;
        }

        public struct TCrdFollowStatus
        {
            public short stage;
            public double slavePos;
            public double slaveVel;
            public Int32 masterFrameWidth;
            public Int32 masterFrameIndex;
            public UInt32 loopCount;
        }
        [DllImport("gts.dll")]
        public static extern short GT_SetCrdPrm(short crd,ref TCrdPrm pCrdPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdPrm(short crd,out TCrdPrm pCrdPrm);
        [DllImport("gts.dll")]
        public static extern short GT_CrdSpace(short crd,out Int32  pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_CrdData(short crd,IntPtr pCrdData, short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_LnXY(short crd,Int32 x,Int32 y,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYOverride2(short crd,Int32 x,Int32 y,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYWN(short crd,Int32 x,Int32 y,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYOverride2WN(short crd,Int32 x,Int32 y,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_LnXYG0(short crd,Int32 x,Int32 y,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYG0Override2(short crd,Int32 x,Int32 y,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYG0WN(short crd,Int32 x,Int32 y,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYG0Override2WN(short crd,Int32 x,Int32 y,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_LnXYZ(short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZOverride2(short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZWN(short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZOverride2WN(short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_LnXYZG0(short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZG0Override2(short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZG0WN(short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZG0Override2WN(short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_LnXYZA(short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAOverride2(short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAWN(short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAOverride2WN(short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_LnXYZAG0(short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAG0Override2(short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAG0WN(short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAG0Override2WN(short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_LnXYZACUVW(short crd,ref Int32  pPos,short posMask,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZACUVWWN(short crd,ref Int32  pPos,short posMask,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZACUVWOverride2(short crd,ref Int32  pPos,short posMask,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZACUVWOverride2WN(short crd,ref Int32  pPos,short posMask,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_ArcXYR(short crd,Int32 x,Int32 y,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYROverride2(short crd,Int32 x,Int32 y,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYRWN(short crd,Int32 x,Int32 y,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYROverride2WN(short crd,Int32 x,Int32 y,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_ArcXYC(short crd,Int32 x,Int32 y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYCOverride2(short crd,Int32 x,Int32 y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYCWN(short crd,Int32 x,Int32 y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYCOverride2WN(short crd,Int32 x,Int32 y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_ArcYZR(short crd,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZROverride2(short crd,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZRWN(short crd,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZROverride2WN(short crd,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_ArcYZC(short crd,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZCOverride2(short crd,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZCWN(short crd,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZCOverride2WN(short crd,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_ArcZXR(short crd,Int32 z,Int32 x,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXROverride2(short crd,Int32 z,Int32 x,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXRWN(short crd,Int32 z,Int32 x,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXROverride2WN(short crd,Int32 z,Int32 x,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_ArcZXC(short crd,Int32 z,Int32 x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXCOverride2(short crd,Int32 z,Int32 x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXCWN(short crd,Int32 z,Int32 x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXCOverride2WN(short crd,Int32 z,Int32 x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_HelixXYRZ(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYRZOverride2(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYRZWN(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYRZOverride2WN(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_HelixXYCZ(short crd,Int32 x,Int32 y,Int32 z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYCZOverride2(short crd,Int32 x,Int32 y,Int32 z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYCZWN(short crd,Int32 x,Int32 y,Int32 z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYCZOverride2WN(short crd,Int32 x,Int32 y,Int32 z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_HelixYZRX(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZRXOverride2(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZRXWN(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZRXOverride2WN(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_HelixYZCX(short crd,Int32 x,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZCXOverride2(short crd,Int32 x,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZCXWN(short crd,Int32 x,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZCXOverride2WN(short crd,Int32 x,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_HelixZXRY(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXRYOverride2(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXRYWN(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXRYOverride2WN(short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_HelixZXCY(short crd,Int32 x,Int32 y,Int32 z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXCYOverride2(short crd,Int32 x,Int32 y,Int32 z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXCYWN(short crd,Int32 x,Int32 y,Int32 z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXCYOverride2WN(short crd,Int32 x,Int32 y,Int32 z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GT_BufIO(short crd,UInt32 doType,UInt32 doMask,UInt32 doValue,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufDelay(short crd,UInt32 delayTime,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufDA(short crd,short chn,short daValue,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufIOWN(short crd, Int16 doType, Int16 doMask, Int16 doValue, Int32 segNum, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufDelayWN(short crd,Int16 delayTime,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufDAWN(short crd, short chn, short daValue, Int32 segNum, short fifo);  
        [DllImport("gts.dll")]
        public static extern short GT_BufLmtsOn(short crd,short axis,short limitType,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufLmtsOff(short crd,short axis,short limitType,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufSetStopIo(short crd,short axis,short stopType,short inputType,short inputIndex,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufMove(short crd,short moveAxis,Int32 pos,double vel,double acc,short modal,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufGear(short crd,short gearAxis,Int32 pos,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufGearPercent(short crd,short gearAxis,Int32 pos,short accPercent,short decPercent,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufStopMotion(short crd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufStopMotionWN(short crd, Int32 segNum, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufSetVarValue(short crd,short pageId,ref TVarInfo pVarInfo,double value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufJumpNextSeg(short crd,short axis,short limitType,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufSynchPrfPos(short crd,short encoder,short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufVirtualToActual(short crd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_CrdStart(short mask,short option);
        [DllImport("gts.dll")]
        public static extern short GT_CrdStartStep(short mask,short option);
        [DllImport("gts.dll")]
        public static extern short GT_CrdStepMode(short mask,short option);
        [DllImport("gts.dll")]
        public static extern short GT_SetOverride(short crd,double synVelRatio);
        [DllImport("gts.dll")]
        public static extern short GT_SetOverride2(short crd,double synVelRatio);
        [DllImport("gts.dll")]
        public static extern short GT_InitLookAhead(short crd,short fifo,double T,double accMax,short n,ref TCrdData pLookAheadBuf);
        [DllImport("gts.dll")]
        public static extern short GT_GetLookAheadSpace(short crd,out Int32  pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetLookAheadSegCount(short crd,out Int32  pSegCount,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_CrdClear(short crd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_CrdStatus(short crd,out short  pRun,out Int32  pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_SetUserSegNum(short crd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetUserSegNum(short crd,out Int32  pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetUserSegNumWN(short crd,out Int32  pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetRemainderSegNum(short crd,out Int32  pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_SetCrdStopDec(short crd,double decSmoothStop,double decAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdStopDec(short crd,out double pDecSmoothStop,out double pDecAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GT_SetCrdLmtStopMode(short crd,short lmtStopMode);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdLmtStopMode(short crd,out short  pLmtStopMode);
        [DllImport("gts.dll")]
        public static extern short GT_GetUserTargetVel(short crd,out double pTargetVel);
        [DllImport("gts.dll")]
        public static extern short GT_GetSegTargetPos(short crd,out Int32  pTargetPos);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdPos(short crd,out double pPos);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdVel(short crd,out double pSynVel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserOn(short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserOff(short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserPrfCmd(short crd,double laserPower,short fifo,short channel);
        [DllImport("gts.dll")]

        public static extern short GT_SetG0Mode(short crd,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_GetG0Mode(short crd,out short  pMode);
        [DllImport("gts.dll")]

        public static extern short GT_SetCrdMapBase(short crd,short Mapbase);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdMapBase(short crd,out short  pBase);
        [DllImport("gts.dll")]
        public static extern short GT_SetCrdBufferMode(short crd,  short bufferMode,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdBufferMode(short crd,out short  pBufferMode,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdSegmentTime(short crd,Int32 segmentIndex,out double pSegmentTime,out Int32  pSegmentNumber,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdTime(short crd,out TCrdTime pTime,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowMaster(short crd, ref TBufFollowMaster pBufFollowMaster, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowEventCross(short crd, ref TBufFollowEventCross pEventCross, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowEventTrigger(short crd, ref TBufFollowEventTrigger pEventTrigger, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowStart(short crd, Int32 masterSegment, Int32 slaveSegment, Int32 masterFrameWidth, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowNext(short crd, Int32 width, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufFollowReturn(short crd, double vel, double acc, short smoothPercent, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowMode(short crd, short source, short fifo, short channel, double startPower);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowRatio(short crd, double ratio, double minPower, double maxPower, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowOff(short crd, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowSpline(short crd, short tableId, double minPower, double maxPower, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowTable(short crd, short tableId, double minPower, double maxPower, short fifo, short channel);


        [DllImport("gts.dll")]
        public static extern short GTN_SetCrdPrm(short core, short crd, ref TCrdPrm pCrdPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdPrm(short core,short crd,out TCrdPrm pCrdPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_CrdSpace(short core,short crd,out Int32  pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_CrdData(short core, short crd, IntPtr pCrdData, short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_LnXY(short core,short crd,Int32 x,Int32 y,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYOverride2(short core,short crd,Int32 x,Int32 y,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYWN(short core,short crd,Int32 x,Int32 y,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYOverride2WN(short core,short crd,Int32 x,Int32 y,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_LnXYG0(short core,short crd,Int32 x,Int32 y,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYG0Override2(short core,short crd,Int32 x,Int32 y,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYG0WN(short core,short crd,Int32 x,Int32 y,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYG0Override2WN(short core,short crd,Int32 x,Int32 y,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_LnXYZ(short core,short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZOverride2(short core,short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZWN(short core,short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZOverride2WN(short core,short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_LnXYZG0(short core,short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZG0Override2(short core,short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZG0WN(short core,short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZG0Override2WN(short core,short crd,Int32 x,Int32 y,Int32 z,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_LnXYZA(short core,short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZAOverride2(short core,short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZAWN(short core,short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZAOverride2WN(short core,short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_LnXYZAG0(short core,short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZAG0Override2(short core,short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZAG0WN(short core,short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZAG0Override2WN(short core,short crd,Int32 x,Int32 y,Int32 z,Int32 a,double synVel,double synAcc,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_LnXYZACUVW(short core,short crd,ref Int32  pPos,short posMask,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZACUVWOverride2(short core,short crd,ref Int32  pPos,short posMask,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZACUVWWN(short core,short crd,ref Int32  pPos,short posMask,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_LnXYZACUVWOverride2WN(short core,short crd,ref Int32  pPos,short posMask,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_ArcXYR(short core,short crd,Int32 x,Int32 y,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcXYROverride2(short core,short crd,Int32 x,Int32 y,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcXYRWN(short core,short crd,Int32 x,Int32 y,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcXYROverride2WN(short core,short crd,Int32 x,Int32 y,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_ArcXYC(short core,short crd,Int32 x,Int32 y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcXYCOverride2(short core,short crd,Int32 x,Int32 y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcXYCWN(short core,short crd,Int32 x,Int32 y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcXYCOverride2WN(short core,short crd,Int32 x,Int32 y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_ArcYZR(short core,short crd,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcYZROverride2(short core,short crd,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcYZRWN(short core,short crd,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcYZROverride2WN(short core,short crd,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_ArcYZC(short core,short crd,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcYZCOverride2(short core,short crd,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcYZCWN(short core,short crd,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcYZCOverride2WN(short core,short crd,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_ArcZXR(short core,short crd,Int32 z,Int32 x,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcZXROverride2(short core,short crd,Int32 z,Int32 x,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcZXRWN(short core,short crd,Int32 z,Int32 x,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcZXROverride2WN(short core,short crd,Int32 z,Int32 x,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_ArcZXC(short core,short crd,Int32 z,Int32 x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcZXCOverride2(short core,short crd,Int32 z,Int32 x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcZXCWN(short core,short crd,Int32 z,Int32 x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ArcZXCOverride2WN(short core,short crd,Int32 z,Int32 x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_HelixXYRZ(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixXYRZOverride2(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixXYRZWN(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixXYRZOverride2WN(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_HelixXYCZ(short core,short crd,Int32 x,Int32 y,Int32 z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixXYCZOverride2(short core,short crd,Int32 x,Int32 y,Int32 z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixXYCZWN(short core,short crd,Int32 x,Int32 y,Int32 z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixXYCZOverride2WN(short core,short crd,Int32 x,Int32 y,Int32 z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_HelixYZRX(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixYZRXOverride2(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixYZRXWN(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixYZRXOverride2WN(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_HelixYZCX(short core,short crd,Int32 x,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixYZCXOverride2(short core,short crd,Int32 x,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixYZCXWN(short core,short crd,Int32 x,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixYZCXOverride2WN(short core,short crd,Int32 x,Int32 y,Int32 z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_HelixZXRY(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixZXRYOverride2(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixZXRYWN(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixZXRYOverride2WN(short core,short crd,Int32 x,Int32 y,Int32 z,double radius,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_HelixZXCY(short core,short crd,Int32 x,Int32 y,Int32 z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixZXCYOverride2(short core,short crd,Int32 x,Int32 y,Int32 z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixZXCYWN(short core,short crd,Int32 x,Int32 y,Int32 z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_HelixZXCYOverride2WN(short core,short crd,Int32 x,Int32 y,Int32 z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]

        public static extern short GTN_BufIO(short core,short crd,short doType,ushort doMask,ushort doValue,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufDelay(short core,short crd,ushort delayTime,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufDA(short core,short crd,short chn,short daValue,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufIOWN(short core, short crd, Int16 doType, Int16 doMask, Int16 doValue, Int32 segNum, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufDelayWN(short core,short crd,Int16 delayTime,Int32 segNum,short fifo=0);
        [DllImport("gts.dll")]
        public static extern short GTN_BufDAWN(short core,short crd,short chn,short daValue,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLmtsOn(short core, short crd, short axis, short limitType, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLmtsOff(short core,short crd,short axis,short limitType,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufSetStopIo(short core,short crd,short axis,short stopType,short inputType,short inputIndex,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufMove(short core,short crd,short moveAxis,Int32 pos,double vel,double acc,short modal,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufGear(short core,short crd,short gearAxis,Int32 pos,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufGearPercent(short core,short crd,short gearAxis,Int32 pos,short accPercent,short decPercent,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufStopMotion(short core,short crd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufSetVarValue(short core,short crd,short pageId,ref TVarInfo pVarInfo,double value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufJumpNextSeg(short core,short crd,short axis,short limitType,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufSynchPrfPos(short core,short crd,short encoder,short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufVirtualToActual(short core,short crd,short fifo);
        
        
        [DllImport("gts.dll")]
        public static extern short GTN_CrdStart(short core,short mask,short option);
        [DllImport("gts.dll")]
        public static extern short GTN_CrdStartStep(short core,short mask,short option);
        [DllImport("gts.dll")]
        public static extern short GTN_CrdStepMode(short core,short mask,short option);
        [DllImport("gts.dll")]
        public static extern short GTN_SetOverride(short core,short crd,double synVelRatio);
        [DllImport("gts.dll")]
        public static extern short GTN_SetOverride2(short core,short crd,double synVelRatio);
        [DllImport("gts.dll")]
        public static extern short GTN_InitLookAhead(short core,short crd,short fifo,double T,double accMax,short n,ref TCrdData pLookAheadBuf);
        [DllImport("gts.dll")]
        public static extern short GTN_GetLookAheadSpace(short core,short crd,out Int32  pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetLookAheadSegCount(short core,short crd,out Int32  pSegCount,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_CrdClear(short core,short crd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_CrdStatus(short core,short crd,out short  pRun,out Int32  pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_SetUserSegNum(short core,short crd,Int32 segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetUserSegNum(short core,short crd,out Int32  pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetUserSegNumWN(short core,short crd,out Int32  pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetRemainderSegNum(short core,short crd,out Int32  pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCrdStopDec(short core,short crd,double decSmoothStop,double decAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdStopDec(short core,short crd,out double pDecSmoothStop,out double pDecAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCrdLmtStopMode(short core,short crd,short lmtStopMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdLmtStopMode(short core,short crd,out short  pLmtStopMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetUserTargetVel(short core,short crd,out double pTargetVel);
        [DllImport("gts.dll")]
        public static extern short GTN_GetSegTargetPos(short core,short crd,out Int32  pTargetPos);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdPos(short core,short crd,out double pPos);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdVel(short core,short crd,out double pSynVel);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserOn(short core,short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserOff(short core,short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserPrfCmd(short core,short crd,double laserPower,short fifo,short channel);
        
        
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserFollowMode(short core, short crd, short source, short fifo, short channel, double startPower);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserFollowRatio(short core, short crd, double ratio, double minPower, double maxPower, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserFollowOff(short core, short crd, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserFollowSpline(short core, short crd, short tableId, double minPower, double maxPower, short fifo, short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_BufLaserFollowTable(short core, short crd, short tableId, double minPower, double maxPower, short fifo, short channel);

        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowMaster(short core, short crd, ref TBufFollowMaster pBufFollowMaster, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowEventCross(short core, short crd, ref TBufFollowEventCross pEventCross, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowEventTrigger(short core, short crd, ref TBufFollowEventTrigger pEventTrigger, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowStart(short core, short crd, Int32 masterSegment, Int32 slaveSegment, Int32 masterFrameWidth, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowNext(short core, short crd, Int32 width, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufFollowReturn(short core, short crd, double vel, double acc, short smoothPercent, short fifo);


        [DllImport("gts.dll")]
        public static extern short GTN_SetG0Mode(short core, short crd, short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetG0Mode(short core,short crd,out short  pMode);
        [DllImport("gts.dll")]

        public static extern short GTN_SetCrdMapBase(short core,short crd,short Mapbase);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdMapBase(short core,short crd,out short  pBase);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCrdBufferMode(short core,short crd,  short bufferMode,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdBufferMode(short core,short crd,out short  pBufferMode,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdSegmentTime(short core,short crd,Int32 segmentIndex,out double pSegmentTime,out Int32  pSegmentNumber,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdTime(short core,short crd,out TCrdTime pTime,short fifo);
       
        [DllImport("gts.dll")]
        public static extern short GT_GetCmdCount(short crd,out short  pResult,short fifo);

        /*-----------------------------------------------------------*/
        /* Compensate                                                */
        /*-----------------------------------------------------------*/

        [DllImport("gts.dll")]
        public static extern short GT_SetBacklash(short axis,Int32 value,double changeValue,Int32 dir);
        [DllImport("gts.dll")]
        public static extern short GT_GetBacklash(short axis,out Int32  pValue,out double pChangeValue,out Int32  pDir);
        [DllImport("gts.dll")]
        public static extern short GT_SetLeadScrewComp(short axis,short n,Int32 startPos,Int32 lenPos,ref Int32  pPositive,ref Int32  pNegative);
        [DllImport("gts.dll")]
        public static extern short GT_SetLeadScrewCompEx(short axis,short n,Int32 startPos,Int32 lenPos,ref Int32 pPositive,ref Int32 pNegative,double compChangeValue);
        [DllImport("gts.dll")]
        public static extern short GT_EnableLeadScrewComp(short axis,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_SetLeadScrewCrossComp(short axis,short n,Int32 startPos,Int32 lenPos,ref Int32  pPositive,ref Int32  pNegative,short link);
        [DllImport("gts.dll")]
        public static extern short GT_EnableLeadScrewCrossComp(short axis,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_GetCompensate(short axis,out double pPitchError,out double pCrossError,out double pBacklashError,out double pEncPos,out double pPrfPos);
        
        
        [DllImport("gts.dll")]
        public static extern short GTN_SetBacklash(short core,short axis,Int32 value,double changeValue,Int32 dir);
        [DllImport("gts.dll")]
        public static extern short GTN_GetBacklash(short core,short axis,out Int32  pValue,out double pChangeValue,out Int32  pDir);
        [DllImport("gts.dll")]
        public static extern short GTN_SetLeadScrewComp(short core,short axis,short n,Int32 startPos,Int32 lenPos,ref Int32  pPositive,out Int32  pNegative);
        [DllImport("gts.dll")]
        public static extern short GTN_EnableLeadScrewComp(short core,short axis,short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetLeadScrewCrossComp(short core,short axis,short n,Int32 startPos,Int32 lenPos,ref Int32  pPositive,out Int32  pNegative,short link);
        [DllImport("gts.dll")]
        public static extern short GTN_EnableLeadScrewCrossComp(short core,short axis,short mode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCompensate(short core,short axis,out double pPitchError,out double pCrossError,out double pBacklashError,ref double pEncPos,ref double pPrfPos);
        
        
        public struct TLeadScrewPrm
        {
	        public short n;
	        public Int32 startPos;
	        public Int32 lenPos;
	        public Int32  pCompPos;
	        public Int32  pCompNeg;
        } 
       
        
        [DllImport("gts.dll")]
        public static extern short GT_SetLeadScrewTable(short axis,ref TLeadScrewPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_EnableLeadScrewTable(short axis,Int32 error);
        [DllImport("gts.dll")]
        public static extern short GT_DisableLeadScrewTable(short axis);
        [DllImport("gts.dll")]
        public static extern short GT_GetLeadScrewTablePrfPosCount(Int32 encPos,out TLeadScrewPrm pPrm,out short  pCountPositive,out short  pCountNegative);
        [DllImport("gts.dll")]
        public static extern short GT_GetLeadScrewTablePrfPosPositive(Int32 encPos,out TLeadScrewPrm pPrm,short index,out Int32  pPrfPosPositive);
        [DllImport("gts.dll")]
        public static extern short GT_GetLeadScrewTablePrfPosNegative(Int32 encPos,out TLeadScrewPrm pPrm,short index,out Int32  pPrfPosNegative);
        [DllImport("gts.dll")]

        public static extern short GTN_SetLeadScrewTable(short core,short axis,ref TLeadScrewPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_EnableLeadScrewTable(short core,short axis,Int32 error);
        [DllImport("gts.dll")]
        public static extern short GTN_DisableLeadScrewTable(short core,short axis);
        [DllImport("gts.dll")]
        public static extern short GTN_GetLeadScrewTablePrfPosCount(short core,Int32 encPos,out TLeadScrewPrm pPrm,out short  pCountPositive,out short  pCountNegative);
        [DllImport("gts.dll")]
        public static extern short GTN_GetLeadScrewTablePrfPosPositive(short core,Int32 encPos,out TLeadScrewPrm pPrm,short index,out Int32  pPrfPosPositive);
        [DllImport("gts.dll")]
        public static extern short GTN_GetLeadScrewTablePrfPosNegative(short core,Int32 encPos,out TLeadScrewPrm pPrm,short index,out Int32  pPrfPosNegative);


        public struct TCompensate2DTable
        {
	        public short count1;                               // 行数和列数，最小值为2
            public short count2;                               // 行数和列数，最小值为2
	        public Int32 posBegin1;                             // 起点位置
            public Int32 posBegin2;                             // 起点位置
	        public Int32 step1;                                 // 步长
            public Int32 step2;                                 // 步长
        } 

        public struct TCompensate2D
        {
	        public short enable;                                  // 2D补偿使能标志
	        public short tableIndex;                              // 所使用的2D补偿表
	        public short axisType1;                             // 查表轴类型
            public short axisType2;                             // 查表轴类型
	        public short axisIndex1;                            // 查表轴索引
            public short axisIndex2;                            // 查表轴索引
        } 
        [DllImport("gts.dll")]
        public static extern short GT_SetCompensate2DTable(short tableIndex,ref TCompensate2DTable pTable,ref Int32  pData,short extend);
        [DllImport("gts.dll")]
        public static extern short GT_GetCompensate2DTable(short tableIndex,out TCompensate2DTable pTable,out short  pExtend);
        [DllImport("gts.dll")]
        public static extern short GT_SetCompensate2D(short axis,ref TCompensate2D pComp2d);
        [DllImport("gts.dll")]
        public static extern short GT_GetCompensate2D(short axis,out TCompensate2D pComp2d);
        [DllImport("gts.dll")]
        public static extern short GT_GetCompensate2DValue(short axis,out double pValue);
       
        [DllImport("gts.dll")]
        public static extern short GTN_SetCompensate2DTable(short core,short tableIndex,ref TCompensate2DTable pTable,ref Int32  pData,short extend);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCompensate2DTable(short core,short tableIndex,out TCompensate2DTable pTable,out short  pExtend);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCompensate2D(short core,short axis,ref TCompensate2D pComp2d);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCompensate2D(short core,short axis,out TCompensate2D pComp2d);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCompensate2DValue(short core,short axis,out double pValue);

        /*-----------------------------------------------------------*/
        /* IO and Encoder                                            */
        /*-----------------------------------------------------------*/

        [DllImport("gts.dll")]
        public static extern short GT_SetDo(short doType,Int32 value);
        [DllImport("gts.dll")]
        public static extern short GT_SetDoBit(short doType,short doIndex,short value);
        [DllImport("gts.dll")]
        public static extern short GT_GetDo(short doType,out Int32  pValue);
        [DllImport("gts.dll")]
        public static extern short GT_SetDoBitReverse(short doType,short doIndex,short value,short reverseTime);
        [DllImport("gts.dll")]
        public static extern short GT_GetDi(short diType,out Int32  pValue);
        [DllImport("gts.dll")]
        public static extern short GT_GetDiReverseCount(short diType,short diIndex,out UInt32 pReverseCount,short count);
        [DllImport("gts.dll")]
        public static extern short GT_SetDiReverseCount(short diType,short diIndex,ref UInt32 pReverseCount,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetDiRaw(short diType,out Int32  pValue);
        [DllImport("gts.dll")]
        public static extern short GT_GetDiEx(short diType,out Int32  pValue,short count);
        
        
        [DllImport("gts.dll")]
        public static extern short GT_SetDac(short dac,ref short  pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetDac(short dac,out short  pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAdc(short adc,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAdcValue(short adc,out short  pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAuAdc(short adc,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAuAdcValue(short adc,out short pValue,short count,out UInt32 pClock);

        
        [DllImport("gts.dll")]
        public static extern short GT_SetEncPos(short encoder,Int32 encPos);
        [DllImport("gts.dll")]
        public static extern short GT_GetEncPos(short encoder,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetEncPosPre(short encoder,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetEncVel(short encoder,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetEncVelEx(short encoder, out double pValue, short count, out UInt32 pClock);
        
        
        [DllImport("gts.dll")]
        public static extern short GT_SetPlsPos(short encoder,Int32 encPos);
        [DllImport("gts.dll")]
        public static extern short GT_GetPlsPos(short pulse,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetPlsVel(short pulse,out double pValue,short count,out UInt32 pClock);
       
        
        [DllImport("gts.dll")]
        public static extern short GT_SetAuEncPos(short encoder,Int32 encPos);
        [DllImport("gts.dll")]
        public static extern short GT_GetAuEncPos(short encoder,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAuEncVel(short encoder,out double pValue,short count,out UInt32 pClock);
        
        
        [DllImport("gts.dll")]
        public static extern short GTN_SetDo(short core,short doType,Int32 value);
        [DllImport("gts.dll")]
        public static extern short GTN_SetDoEx(short core, short doType, ref Int32 pValue, short count);
        [DllImport("gts.dll")]
        public static extern short GTN_SetDoBit(short core,short doType,short doIndex,short value);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDo(short core,short doType,out Int32  pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_SetDoBitReverse(short core,short doType,short doIndex,short value,short reverseTime);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDi(short core,short diType,out Int32  pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDiReverseCount(short core,short diType,short diIndex,out UInt32 pReverseCount,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_SetDiReverseCount(short core,short diType,short diIndex,ref UInt32 pReverseCount,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDiRaw(short core,short diType,out Int32  pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDiEx(short core,short diType,out Int32  pValue,short count);
       
        
        [DllImport("gts.dll")]
        public static extern short GTN_WriteAo(short core,Int32 aoType,short aoIndex,ref double pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_SetDac(short core,short dac,ref short  pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDac(short core,short dac,out short  pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAuDac(short core,short dac,ref short pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAuDac(short core, short dac, out short pValue, short count, out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAdc(short core,short adc,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAdcValue(short core,short adc,out short  pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAuAdc(short core,short adc,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAuAdcValue(short core,short adc,out short pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEncLineNum(short core,short encoder,out Int32 pLineNum,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEncType(short core, short encoder, out short pType, short count, out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_SetEncPos(short core,short encoder,Int32 encPos);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEncPos(short core,short encoder,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEncPosPre(short core,short encoder,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEncVel(short core,short encoder,out double pValue,short count,out UInt32 pClock);
      
        
        [DllImport("gts.dll")]
        public static extern short GTN_SetPlsPos(short core,short encoder,Int32 encPos);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPlsPos(short core,short pulse,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPlsVel(short core,short pulse,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]

        public static extern short GTN_SetAuEncPos(short core,short encoder,Int32 encPos);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAuEncPos(short core,short encoder,out double pValue,short count,out UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAuEncVel(short core,short encoder,out double pValue,short count,out UInt32 pClock);
      
        
        [DllImport("gts.dll")]
        public static extern short GTN_GetAbsEncPos(short core,short encoder,out Int32  pValue,short mode,short param);
        
        /*-----------------------------------------------------------*/
        /* ExtModule                                                 */
        /*-----------------------------------------------------------*/
        [DllImport("gts.dll")]
        public static extern short GTN_ExtModuleInit(short core,short method);
        [DllImport("gts.dll")]
        public static extern short GTN_SetILinkManuMode(short core, short station);
        [DllImport("gts.dll")]
        public static extern short GTN_GetILinkManuId(short core, short station, short autoId, out short manuId);
        [DllImport("gts.dll")]
        public static extern short GTN_GetILinkAutoId(short core, short station, short manuId, out short autoId);
        [DllImport("gts.dll")]
        public static extern short GTN_SetExtDoBit(short core,short doIndex,short value);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtDoBit(short core, short doIndex, out short pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_SetExtDo(short core,short doIndex,Int32 value,Int32 mask);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtDo(short core,short doIndex,out Int32  pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtDiBit(short core,short diIndex,out short pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtDi(short core,short diIndex,out Int32 pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_SetExtAoValue(short core,short index,out short pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtAiValue(short core,short index,out short pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_SetExtAo(short core,short index,ref double pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtAo(short core,short index,out double pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtAi(short core,short index,out double pValue,short count);

        [DllImport("gts.dll")]
        public static extern short GTN_SetILinkDo(short core,short station,short module,ushort data,ushort mask);
        [DllImport("gts.dll")]
        public static extern short GTN_GetILinkDi(short core,short station,short module,ushort data);
        [DllImport("gts.dll")]
        public static extern short GTN_SetILinkAo(short core,short station,short module,short channel,short data);
        [DllImport("gts.dll")]
        public static extern short GTN_GetILinkAi(short core,short station,short module,short channel,out short data);

        /*-----------------------------------------------------------*/
        /* Pos Compare                                               */
        /*-----------------------------------------------------------*/
        public const short POS_COMPARE_MODE_FIFO = 0;
        public const short POS_COMPARE_MODE_LINEAR = 1;
        public const short POS_COMPARE_MODE_EQUIDISTANT = 2;

        public const short POS_COMPARE_OUTPUT_PULSE = 0;
        public const short POS_COMPARE_OUTPUT_LEVEL = 1;

        public const short POS_COMPARE_SOURCE_ENCODER = 0;
        public const short POS_COMPARE_SOURCE_PULSE = 1;


        public struct TPosCompareMode
        {
	        public short mode;                       
	        public short dimension;                  
	        public short sourceMode;                 
	        public short sourceX;                    
	        public short sourceY;                    
	        public short outputMode;                
	        public short outputCounter;              
	        public ushort outputPulseWidth;  
            public ushort errorBand;         
        } 

        public struct TPosCompareLinear
        {
	        public UInt32 count;
            public ushort hso;
            public ushort gpo;

	        public Int32 startPos;
	        public Int32 interval;
        } 

        public struct TPosCompareLinear2D
        {
	        public UInt32 count;
            public ushort hso;
            public ushort gpo;

	        public Int32 startPosX;
	        public Int32 startPosY;
	        public Int32 intervalX;
	        public Int32 intervalY;
        } 


        public struct TPosCompareData
        {
	        public Int32 pos;
            public ushort hso;
            public ushort gpo;
	        public UInt32 segmentNumber;
        } 

        public struct TPosCompareData2D
        {
	        public Int32 posX;
	        public Int32 posY;
            public ushort hso;
            public ushort gpo;
	        public UInt32 segmentNumber;
        } 

        public struct TPosCompareStatus
        {
            public ushort mode;      
            public ushort run;
            public ushort space;
	        public UInt32 pulseCount;
            public ushort hso;
            public ushort gpo;
	        public UInt32 segmentNumber;
        } 

        public struct TPosCompareInfo
        {
            public ushort config;
            public ushort fifoEmpty;
            public ushort head;
            public ushort tail;
	        public UInt32 commandReceive;
	        public UInt32 commandSend;
	        public Int32 posX;
	        public Int32 posY;
        } 
        
        public struct TPosComparePsoPrm
        {
            public UInt32 count;
            public Int16 hso;
            public Int16 gpo;
            public Int32 startPosX;
            public Int32 startPosY;
            public Int32 syncPos;
            public Int32 time;
            public short reserve1;
            public short reserve2;
            public short reserve3;
            public short reserve4;
            public short reserve5;
            public short reserve6;
            public short reserve7;
            public short reserve8;
            public short reserve9;
            public short reserve10;
            public short reserve11;
            public short reserve12;
            public short reserve13;
            public short reserve14;
            public short reserve15;
            public short reserve16;
            public short reserve17;
            public short reserve18;
            public short reserve19;
            public short reserve20;
        }


        public struct TPosCompareContinueMode
        {
            public short mode;
            public Int16 count;
            public short highLevel;
            public short lowLevel;
            public short resver1;
            public short resver2;
            public short resver3;
            public short resver4;
            public short resver5;
            public short resver6;
            public short resver7;
            public short resver8;
            public short resver9;
            public short resver10;
            public short resver11;
            public short resver12;
            public short resver13;
            public short resver14;
            public short resver15;
            public short resver16;
            public short resver17;
            public short resver18;
            public short resver19;
            public short resver20;
        }

        [DllImport("gts.dll")]
        public static extern short GT_PosCompareStart(short core,short index);
        [DllImport("gts.dll")]
        public static extern short GT_PosCompareStop(short core,short index);
        [DllImport("gts.dll")]
        public static extern short GT_PosCompareClear(short core,short index);
        [DllImport("gts.dll")]
        public static extern short GT_PosCompareStatus(short core,short index,out TPosCompareStatus pStatus);
        [DllImport("gts.dll")]
        public static extern short GT_PosCompareData(short core,short index,ref TPosCompareData pData);
        [DllImport("gts.dll")]
        public static extern short GT_PosCompareData2D(short core,short index,ref TPosCompareData2D pData);
        [DllImport("gts.dll")]
        public static extern short GT_SetPosCompareMode(short core,short index,ref TPosCompareMode pMode);
        [DllImport("gts.dll")]
        public static extern short GT_GetPosCompareMode(short core,short index,out TPosCompareMode pMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetPosCompareLinear(short core,short index,ref TPosCompareLinear pLinear);
        [DllImport("gts.dll")]
        public static extern short GT_GetPosCompareLinear(short core,short index,out TPosCompareLinear pLinear);
        [DllImport("gts.dll")]
        public static extern short GT_SetPosCompareLinear2D(short core,short index,ref TPosCompareLinear2D pLinear);
        [DllImport("gts.dll")]
        public static extern short GT_GetPosCompareLinear2D(short core,short index,out TPosCompareLinear2D pLinear);
        [DllImport("gts.dll")]
        public static extern short GT_PosCompareInfo(short core,short index,out TPosCompareInfo pInfo);
        [DllImport("gts.dll")]
        public static extern short GT_SetPosComparePsoPrm(short core, short index, ref TPosComparePsoPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetPosComparePsoPrm(short core, short index, out TPosComparePsoPrm pPrm);

        [DllImport("gts.dll")]
        public static extern short GTN_PosCompareStart(short core,short index);
        [DllImport("gts.dll")]
        public static extern short GTN_PosCompareStop(short core,short index);
        [DllImport("gts.dll")]
        public static extern short GTN_PosCompareClear(short core,short index);
        [DllImport("gts.dll")]
        public static extern short GTN_PosCompareStatus(short core,short index,out TPosCompareStatus pStatus);
        [DllImport("gts.dll")]
        public static extern short GTN_PosCompareData(short core,short index,System.IntPtr pData);
        [DllImport("gts.dll")]
        public static extern short GTN_PosCompareData2D(short core,short index,ref TPosCompareData2D pData);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPosCompareMode(short core,short index,ref TPosCompareMode pMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPosCompareMode(short core,short index,out TPosCompareMode pMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPosCompareContinueMode(short core,short index,ref TPosCompareContinueMode pMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPosCompareContinueMode(short core,short index,out TPosCompareContinueMode pMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPosCompareLinear(short core,short index,ref TPosCompareLinear pLinear);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPosCompareLinear(short core,short index,out TPosCompareLinear pLinear);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPosCompareLinear2D(short core,short index,ref TPosCompareLinear2D pLinear);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPosCompareLinear2D(short core,short index,out TPosCompareLinear2D pLinear);
        [DllImport("gts.dll")]
        public static extern short GTN_PosCompareInfo(short core,short index,out TPosCompareInfo pInfo);
        [DllImport("gts.dll")]
        public static extern short GTN_PosCompareHsOn(short core,short index,short link,Int16 threshold);
        [DllImport("gts.dll")]
        public static extern short GTN_PosCompareHsOff(short core,short index);
        [DllImport("gts.dll")]
        public static extern short GTN_PosCompareSpace(short core,short index,out Int16 pSpace);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPosComparePsoPrm(short core,short index,ref TPosComparePsoPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPosComparePsoPrm(short core,short index,out TPosComparePsoPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPosCompareStartLevel(short core,short index,short type,short startLevel);

        public const short COMPARE_SEND_DATA_MAX = 30;
        public const short COMPARE_DATA_MAX = 4096;
        public const Int32 COMPARE_STEP_MAX = 0x1fff;
        public const Int32 COMPARE_MAX_NUM = 0x3fffffff;

        [DllImport("gts.dll")]
        public static extern short GT_ComparePulse(short level, short outputType, short time);
        [DllImport("gts.dll")]
        public static extern short GT_CompareStop();
        [DllImport("gts.dll")]
        public static extern short GT_CompareStatus(out short pStatus, out long pCount);
        [DllImport("gts.dll")]
        public static extern short GT_CompareData(short encoder, short source, short pulseType, short startLevel, short time,
            ref Int32 pBuf1, short count1, ref Int32 pBuf2, short count2);
        [DllImport("gts.dll")]
        public static extern short GT_CompareLinear(short encoder, short channel, Int32 startPos, Int32 repeatTimes, Int32 interval, short time, short source);


        /*-----------------------------------------------------------*/
        /* Laser and Scan                                            */
        /*-----------------------------------------------------------*/
        public const short FIFO_MODE_STATIC = 0;
        public const short FIFO_MODE_DYNAMIC = 1;

        public const short SCAN_STATUS_WAIT = 0;
        public const short SCAN_STATUS_RUN = 1;
        public const short SCAN_STATUS_DONE = 2;

        public struct TScanInit
        {
	        public int lookAheadNum;             //前瞻段数
	        public double time;                  //时间常数
	        public double radiusRatio;           //曲率限制调节参数
        }

        public struct TScanInfo
        {
	        public UInt32 segmentNumber;
            public ushort commandNumber;
            public ushort prfVel;
            public ushort fifoEmpty;
            public ushort head;
            public ushort tail;
	        public UInt32 commandReceive;
	        public UInt32 commandSend;
	        public UInt32 reserve1;
            public UInt32 reserve2;
            public UInt32 reserve3;
            public UInt32 reserve4;
            public UInt32 reserve5;
            public UInt32 reserve6;
        }



        public struct TScanPosSuperposeParamter
        {
            public short enable;
            public short superposeSrc;
            public short superposeAxisX;
            public short superposeAxisY;
            public double xCoefficient;
            public double yCoefficient;
        }

        public struct TLaserInfo
        {
            public ushort hso;
            public ushort powerMode;
            public ushort power;
            public ushort powerMax;
            public ushort powerMin;
            public ushort frequency;
            public ushort pulseWidth;
        } 
        public struct TLaserPowerPrm
        {
            public short n;
            public double startVel;
            public double power;
        }

        public struct TLaserPowerTable
        {
            public short n;
            public double startVel;
            public double velStep;
            public double power;
        }
        //typedef struct
        //{
        //    short corrX[65][65];
        //    short corrY[65][65];
        //}TScanCorrectionTableData;

        [DllImport("gts.dll")]
        public static extern short GT_LaserAo(short value,Int16 laserChannel);
        [DllImport("gts.dll")]
        public static extern short GT_SetHSIOOpt(Int16 value,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_GetHSIOOpt(ref Int16 pValue,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_SetLaserMode(Int16 laserMode);
        [DllImport("gts.dll")]
        public static extern short GT_LaserPowerMode(short laserPowerMode,double maxValue,double minValue,short laserChannel,short delayMode);
        [DllImport("gts.dll")]
        public static extern short GT_LaserPrfCmd(double power,Int16 laserChannel);
        [DllImport("gts.dll")]
        public static extern short GT_LaserOutFrq(double outFrq,Int16 laserChannel);
        [DllImport("gts.dll")]
        public static extern short GT_SetPulseWidth(Int16 width1,Int16 laserChannel);
        [DllImport("gts.dll")]
        public static extern short GT_SetLevelDelay(Int16 highLevelDelay, Int16 lowLevelDelay, Int16 laserChannel);
        [DllImport("gts.dll")]
        public static extern short GT_LaserInfo(out TLaserInfo pLaserInfo,short crd);

        [DllImport("gts.dll")]
        public static extern short GTN_ScanInit(short core, ref TScanInit pScanInit, double jumpAcc, double markAcc, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanCrdDataEnd(short core, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_SetScanLaserLink(short core, short link, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_GetScanLaserLink(short core, out short pLink, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_SetScanMode(short core, short mode, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_GetScanMode(short core, out short pMode, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearScanStatus(short core, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanGetCrdPos(short core, out short pPos, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanJump(short core, short x, short y, double vel, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanJumpPoint(short core, short x, short y, double vel, Int32 motionDelayTime, Int32 laserDelayTime, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanTimeJump(short core, short x, short y, ushort time, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanTimeJumpPoint(short core, short x, short y, ushort time, Int32 motionDelayTime, Int32 laserDelayTime, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanMark(short core, short x, short y, double vel, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanTimeMark(short core, short x, short y, ushort time, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufLaserPrfCmd(short core, double laserPower, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufIO(short core, ushort doType, ushort doMask, ushort doValue, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufDelay(short core, Int32 time, short crd);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufDA(short core, ushort chn, short value, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufAO(short core,Int16 chn,double voltage,short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufLaserDelay(short core, short laserOnDelay, short laserOffDelay, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufLaserOutFrq(short core, double outFrq, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufSetPulseWidth(short core, ushort width, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufLaserOn(short core, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufLaserOff(short core, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanBufStop(short core, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanLaserIntervalOnList(short core, Int32 time, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_SetScanDelayTime(short core, ushort maxJumpDelay, ushort markDelay, ushort multiMarkDelayConst, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_SetScanDelayMode(short core, short multiMarkDelayMode, ushort multiMarkLaserOffDelay, ushort minJumpDelay, ushort jumpDelayLengthLimit, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanStop(short core, short stopType, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanCrdSpace(short core, out short pSpace, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanCrdStart(short core, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanCrdClear(short core, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanCrdStatus(short core, out short pRun, out short pStatus, short scan);
        
        
        [DllImport("gts.dll")]
        public static extern short GTN_SetScanPosSuperposeParameter(short core,short crd,TScanPosSuperposeParamter param);
        //[DllImport("gts.dll")]
        //public static extern short GTN_ScanSetCorrectionTable(short core,short crd,ref TScanCorrectionTableData pParam);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanCorrectionOn(short core,short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanCorrectionOff(short core,short scan);
        //[DllImport("gts.dll")]
        //public static extern short GTN_ScanGenerateCorrectionTable(short core,double paraX,double paraY,short rangeX,short rangeY,ref TScanCorrectionTableData pParam);

        [DllImport("gts.dll")]
        public static extern short GTN_LaserOn(short core,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_LaserOff(short core,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_LaserOnStatus(short core,out ushort pValue,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_LaserPowerMode(short core,short laserPowerMode,double maxValue,double minValue,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_LaserPrfCmd(short core,double power,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_LaserOutFrq(short core,double outFrq,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPulseWidth(short core,ushort width,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_SetLevelDelay(short core,UInt32 highLevelDelay,UInt32 lowLevelDelay,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_LaserInfo(short core,out TLaserInfo pLaserInfo,short channel);

        [DllImport("gts.dll")]
        public static extern short GTN_ScanInfo(short core,ref TScanInfo pScanInfo,short crd);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanHsOn(short core,short crd,short link,Int16 threshold);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanHsOff(short core,short crd);
        

        [DllImport("gts.dll")]
        public static extern short GTN_ScanLaserOn(short core,short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanLaserOff(short core,short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanLaserOnStatus(short core,out Int16 pValue,short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanSetLaserMode(short core,Int16 laserMode, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanLaserPowerMode(short core,short laserPowerMode,double maxValue,double minValue, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanLaserPrfCmd(short core,double power, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanLaserOutFrq(short core,double outFrq, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanSetPulseWidth(short core,Int16 width, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanSetLevelDelay(short core, Int16 highLevelDelay, Int16 lowLevelDelay, short scan);
        [DllImport("gts.dll")]
        public static extern short GTN_ScanLaserInfo(short core,ref TLaserInfo pLaserInfo, short scan);


       /*-----------------------------------------------------------*/
        /* DLM                                                       */
        /*-----------------------------------------------------------*/
        public const short DLM_FUNCTION_EVENT = 0;
        public const short DLM_FUNCTION_TIMER = 1;
        public const short DLM_FUNCTION_BACKGROUND = 2;
        public const short DLM_FUNCTION_COMMAND	= 3;

        public const short DLM_FUNCTION_PROCEDURE = 7;

        public const short DLM_FUNCTION_PROFILE_EVENT = 8;
        public const short DLM_FUNCTION_PROFILE	= 9;
        public const short DLM_FUNCTION_PROFILE_SUPERIMPOSED = 10;
        public const short DLM_FUNCTION_PROFILE_FILTER = 11;

        public const short DLM_FUNCTION_SERVO_EVENT = 16;
        public const short DLM_FUNCTION_SERVO = 17;
        public const short DLM_FUNCTION_SERVO_SUPERIMPOSED = 18;
        public const short DLM_FUNCTION_SERVO_FILTER = 19;

        public const short DLM_LOAD_MODE_NONE = 0;
        public const short DLM_LOAD_MODE_COMMAND = 1;
        public const short DLM_LOAD_MODE_BOOT = 2;
        public const short DLM_LOAD_MODE_RUN = 3;

        public struct TDlmStatus
        {
	        public Int32 version;
	        public Int32 date;
	        public short enable;
	        public Int32 function;
        } 

        public struct TDlmFunction
        {
	        public short function;
	        public short enable;
	        public Int32 value;
        } 
        [DllImport("gts.dll")]
        public static extern short GT_LoadDlm(Int32 vender,Int32 module,string fileName,out short  pId);
        [DllImport("gts.dll")]
        public static extern short GT_ProgramDlm(short id,short loadMode);
        [DllImport("gts.dll")]
        public static extern short GT_GetDlmLoadMode(short id,out short  pLoadMode);
        [DllImport("gts.dll")]
        public static extern short GT_RunDlm(short id);
        [DllImport("gts.dll")]
        public static extern short GT_StopDlm(short id);
        [DllImport("gts.dll")]
        public static extern short GT_GetDlmStatus(short id,out TDlmStatus pStatus);
        [DllImport("gts.dll")]
        public static extern short GT_SetDlmFunction(short id,ref TDlmFunction pFunction);
        [DllImport("gts.dll")]
        public static extern short GT_GetDlmFunction(short id,out TDlmFunction pFunction);
        
        
        [DllImport("gts.dll")]
        public static extern short GT_DlmCommandInit(short code,Int32 index);
        [DllImport("gts.dll")]
        public static extern short GT_DlmCommandAdd16(short value);
        [DllImport("gts.dll")]
        public static extern short GT_DlmCommandAdd32(Int32 value);
        [DllImport("gts.dll")]
        public static extern short GT_DlmCommandAddFloat(float value);
        [DllImport("gts.dll")]
        public static extern short GT_DlmCommandAddDouble(double value);
        [DllImport("gts.dll")]
        public static extern short GT_SendDlmCommand(short id,out short  pReturnValue);
        [DllImport("gts.dll")]
        public static extern short GT_DlmCommandGet16(out short  pValue);
        [DllImport("gts.dll")]
        public static extern short GT_DlmCommandGet32(out Int32  pValue);
        [DllImport("gts.dll")]
        public static extern short GT_DlmCommandGetFloat(out float pValue);
        [DllImport("gts.dll")]
        public static extern short GT_DlmCommandGetDouble(out double pValue);
        [DllImport("gts.dll")]


        public static extern short GTN_LoadDlm(short core,Int32 vender,Int32 module,string fileName,out short  pId);
        [DllImport("gts.dll")]
        public static extern short GTN_ProgramDlm(short core,short id,short loadMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDlmLoadMode(short core,short id,out short  pLoadMode);
        [DllImport("gts.dll")]
        public static extern short GTN_RunDlm(short core,short id);
        [DllImport("gts.dll")]
        public static extern short GTN_StopDlm(short core,short id);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDlmStatus(short core,short id,out TDlmStatus pStatus);
        [DllImport("gts.dll")]
        public static extern short GTN_SetDlmFunction(short core,short id,ref TDlmFunction pFunction);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDlmFunction(short core,short id,out TDlmFunction pFunction);
        [DllImport("gts.dll")]


        public static extern short GTN_DlmCommandInit(short core,short code,Int32 index);
        [DllImport("gts.dll")]
        public static extern short GTN_DlmCommandAdd16(short core,short value);
        [DllImport("gts.dll")]
        public static extern short GTN_DlmCommandAdd32(short core,Int32 value);
        [DllImport("gts.dll")]
        public static extern short GTN_DlmCommandAddFloat(short core,float value);
        [DllImport("gts.dll")]
        public static extern short GTN_DlmCommandAddDouble(short core,double value);
        [DllImport("gts.dll")]
        public static extern short GTN_SendDlmCommand(short core,short id,out short  pReturnValue);
        [DllImport("gts.dll")]
        public static extern short GTN_DlmCommandGet16(short core,out short  pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_DlmCommandGet32(short core,out Int32  pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_DlmCommandGetFloat(short core,ref float pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_DlmCommandGetDouble(short core,out double pValue);

        /*-----------------------------------------------------------*/
        /* Event-Task                                                */
        /*-----------------------------------------------------------*/
        public const short TASK_SET_DO_BIT = 0x1101;
        public const short TASK_SET_DAC	= 0x1120;

        public const short TASK_STOP = 0x1303;

        public const short TASK_UPDATE_POS = 0x2002;
        public const short TASK_UPDATE_VEL = 0x2004;

        public const short TASK_PT_START = 0x2306;
        public const short TASK_PVT_START = 0x2346;
        public const short TASK_MOVE_ABSOLUTE = 0x2500;

        public const short TASK_GEAR_START = 0x3005;

        public const short TASK_FOLLOW_START = 0x310A;
        public const short TASK_FOLLOW_SWITCH = 0x310B;

        public const short TASK_CRD_START = 0x4004;
        public const short TASK_SCAN_START = 0x4102;

        public const short TASK_SET_DO_BIT_MODE_NONE = 0;
        public const short TASK_SET_DO_BIT_MODE_TIME = 10;
        public const short TASK_SET_DO_BIT_MODE_DISTANCE = 20;

        public struct TWatchVar
        {
            public Int16 type;
            public Int16 index;
            public Int16 id;
        }

        public struct   TTaskSetDoBit
        {
	        public short doType;
	        public short doIndex;
	        public short doValue;
	        public short mode;
	        public Int32 parameter1;
            public Int32 parameter2;
            public Int32 parameter3;
            public Int32 parameter4;
            public Int32 parameter5;
            public Int32 parameter6;
            public Int32 parameter7;
            public Int32 parameter8;
        } 

        public struct  TTaskSetDac
        {
	        public short dac;
	        public short value;
        } 

        public struct  TTaskStop
        {
	        public Int32 mask;
	        public Int32 option;
        } 

        public struct  TTaskFifoOperation
        {
	        public short type;
	        public short index;
	        public short operation;
	        public short data1;
            public short data2;
            public short data3;
            public short data4;
            public short data5;
            public short data6;
            public short data7;
            public short data8;
            public short data9;
            public short data10;
            public short data11;
            public short data12;
            public short data13;
            public short data14;
            public short data15;
            public short data16;
            public short data17;
            public short data18;
            public short data19;
            public short data20;
        } 

        public struct  TTaskUpdatePos
        {
	        public short profile;
	        public Int32 pos;
        }

        public struct TTaskUpdateDistance
        {
            public short profile;
            public short triggerIndex;
            public Int32 distance;
        } 
        public struct  TTaskUpdateVel
        {
	        public short profile;
	        public double vel;
        } 

        public struct  TTaskPtStart
        {
	        public Int32 mask;
	        public Int32 option;
        } 

        public struct  TTaskPvtStart
        {
	        public Int32 mask;
        } 

        public struct  TTaskGearStart
        {
	        public Int32 mask;
        } 

        public struct TTaskFollowStart
        {
	        public Int32 mask;
	        public Int32 option;
        } 

        public struct TTaskFollowSwitch
        {
	      public Int32 mask;
        } 

        public struct  TTaskMoveAbsolute
        {
	        public short profile;
	        public Int32 pos;
	        public double vel;
	        public double acc;
	        public double dec;
	        public short percent;
        } 

        public struct  TTaskCrdStart
        {
	        public short mask;
	        public short option;
        } 

        public struct  TTaskScanStart
        {
	        public short core;
	        public short index;
	        public short count;
        } 

        public struct  TEvent
        {
	        public UInt32 loop;
	        public TWatchVar var;
            public ushort condition;
	        public double value;
        } 
        [DllImport("gts.dll")]
        public static extern short GT_ClearEvent();
        [DllImport("gts.dll")]
        public static extern short GT_ClearTask();
        [DllImport("gts.dll")]
        public static extern short GT_ClearEventTaskLink();
        [DllImport("gts.dll")]
        public static extern short GT_AddTask(short taskType, IntPtr pTaskData, ref short pTaskIndex);
        [DllImport("gts.dll")]
        public static extern short GT_AddEvent(ref TEvent pEvent,ref short  pEventIndex);
        [DllImport("gts.dll")]
        public static extern short GT_AddEventTaskLink(short eventIndex,short taskIndex,ref short  pLinkIndex);
        [DllImport("gts.dll")]
        public static extern short GT_GetEventCount(out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GT_GetEvent(short eventIndex,out TEvent pEvent);
        [DllImport("gts.dll")]
        public static extern short GT_GetEventLoop(short eventIndex,out UInt32 pCount);
        [DllImport("gts.dll")]
        public static extern short GT_GetTaskCount(out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GT_GetEventTaskLinkCount(out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GT_GetEventTaskLink(short linkIndex,out short  pEventIndex,out short  pTaskIndex);
        [DllImport("gts.dll")]
        public static extern short GT_EventOn(short eventIndex,short count);
        [DllImport("gts.dll")]
        public static extern short GT_EventOff(short eventIndex,short count);
       
        [DllImport("gts.dll")]
        public static extern short GTN_ClearEvent(short core);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearTask(short core);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearEventTaskLink(short core);
        [DllImport("gts.dll")]
        public static extern short GTN_AddTask(short core,short taskType, IntPtr pTaskData, ref short pTaskIndex);
        [DllImport("gts.dll")]
        public static extern short GTN_AddEvent(short core,ref TEvent pEvent,ref short  pEventIndex);
        [DllImport("gts.dll")]
        public static extern short GTN_AddEventTaskLink(short core,short eventIndex,short taskIndex,ref short  pLinkIndex);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEventCount(short core,out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEvent(short core,short eventIndex,out TEvent pEvent);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEventLoop(short core,short eventIndex,out UInt32 pEventLoop);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTaskCount(short core,out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEventTaskLinkCount(short core,out short  pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetEventTaskLink(short core,short linkIndex,out short  pEventIndex,out short  pTaskIndex);
        [DllImport("gts.dll")]
        public static extern short GTN_EventOn(short core,short eventIndex,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_EventOff(short core,short eventIndex,short count);
       
        /*--------- -------------------------------------------------*/
        /* Gantry                                                    */
        /*-----------------------------------------------------------*/
        public const short GANTRY_MODE_NONE = -1;
        public const short GANTRY_MODE_OPEN_LOOP_GANTRY = 1;
        public const short GANTRY_MODE_DECOUPLE_POSITION_LOOP = 2;

        [DllImport("gts.dll")]
        public static extern short GT_SetGantryMode(short group, short master, short slave, short mode, Int32 syncErrorLimit);
        [DllImport("gts.dll")]
        public static extern short GT_GetGantryMode(short group, out short pMaster, out short pSlave, out short pMode, out Int32 pSyncErrorLimit);
        [DllImport("gts.dll")]
        public static extern short GT_SetGantryPid(short group, ref TPid pGantryPid, ref TPid pYawPid);
        [DllImport("gts.dll")]
        public static extern short GT_GetGantryPid(short group, out TPid pGantryPid, out TPid pYawPid);
        [DllImport("gts.dll")]
        public static extern short GT_GantryAxisOn(short group);
        [DllImport("gts.dll")]
        public static extern short GT_GantryAxisOff(short group);

        [DllImport("gts.dll")]
        public static extern short GTN_SetGantryMode(short core, short group, short master, short slave, short mode, Int32 syncErrorLimit);
        [DllImport("gts.dll")]
        public static extern short GTN_GetGantryMode(short core, short group,out short pMaster,out short pSlave,out short pMode, out Int32 pSyncErrorLimit);
        [DllImport("gts.dll")]
        public static extern short GTN_SetGantryPid(short core, short group, ref TPid pGantryPid, ref TPid pYawPid);
        [DllImport("gts.dll")]
        public static extern short GTN_GetGantryPid(short core, short group, out TPid pGantryPid,out TPid pYawPid);
        [DllImport("gts.dll")]
        public static extern short GTN_GantryAxisOn(short core, short group);
        [DllImport("gts.dll")]
        public static extern short GTN_GantryAxisOff(short core, short group);


        public struct TSecondOrderFilterPara
        {
            public short active;

            public double a0;
            public double a1;
            public double a2;

            public double b1;
            public double b2;
        }

        public const short TYPE_CONTROL_OUTPUT_FILTER = 1;
        public const short TYPE_YAW_CONTROL_OUTPUT_FILTER = 2;
        public const short FILTER_COUNT_MAX = 2;


        [DllImport("gts.dll")]
        public static extern short GT_SetSecondOrderFilterPara(short control, short type, short index, ref TSecondOrderFilterPara pFilterPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetSecondOrderFilterPara(short control, short type, short index, out TSecondOrderFilterPara pFilterPrm);

        [DllImport("gts.dll")]
        public static extern short GTN_SetSecondOrderFilterPara(short core, short control, short type, short index, ref TSecondOrderFilterPara pFilterPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetSecondOrderFilterPara(short core, short control, short type, short index, out TSecondOrderFilterPara pFilterPrm);

        public struct OCA_CTRL_GEN
        {
            public short s16Gain;
            public short s16Offset;
            public double dFrq;
            public Int32 u32RunPeriod;
            public short u16SigType;
        }
        
        public struct TExcitationPara
        {
            public short gain;
            public short offset;
            public short frq;		// hz
            public double runTime;	// s
        }

        public struct TExcitationTrpPara
        {
            public double vel;
            public double acc;
            public double dec;
            public double runTime;	// s
        }


        public const short NO_EXCIT = 0xff;
        public const short OPEN_LOOP_EXCIT = 1;
        public const short SIGNAL_TYPE_STEP = 3;
        public const short SIGNAL_TYPE_SIN = 0x10;
        public const short SIGNAL_TYPE_TRAP = 0x11;
        public const short SIGNAL_TYPE_NOTHING = 0xFF;
        
        [DllImport("gts.dll")]
        public static extern short GT_SetGenerator(ref OCA_CTRL_GEN pGenStr);
        [DllImport("gts.dll")]
        public static extern short GT_GetGenerator(out OCA_CTRL_GEN pGenStr);
        [DllImport("gts.dll")]
        public static extern short GT_StepResponse(short control,short gain, double time);
        [DllImport("gts.dll")]
        public static extern short GT_SetExctLoopMode(short control,short exciteLoopMode);
        [DllImport("gts.dll")]
        public static extern short GT_GetExctLoopMode(short control,out short pExciteLoopMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetExcitation(short axis,short objectValue,short type,short pParameter);

        [DllImport("gts.dll")]
        public static extern short GTN_SetGenerator(short core,ref OCA_CTRL_GEN pGenStr);
       [DllImport("gts.dll")]
        public static extern short GTN_StepResponse(short core,short control,short gain,double time);
        [DllImport("gts.dll")]
        public static extern short GTN_SetExctLoopMode(short core,short control,short exciteLoopMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExctLoopMode(short core,short control,out short pExciteLoopMode);

        [DllImport("gts.dll")]
        public static extern short GTN_SetExcitation(short core,short axis,short objectValue,short type,short pParameter);

        /*-----------------------------------------------------------*/
        /* DMA                                                       */
        /*-----------------------------------------------------------*/
        [DllImport("gts.dll")]
        public static extern short GT_CrdHsOn(short crd,short fifo,short link,Int16 threshold,short lookAheadInMc);
        [DllImport("gts.dll")]
        public static extern short GT_CrdHsOff(short crd,short fifo);

        [DllImport("gts.dll")]
        public static extern short GTN_CrdHsOn(short core, short crd, short fifo, short link, Int16 threshold, short lookAheadInMc);
        [DllImport("gts.dll")]
        public static extern short GTN_CrdHsOff(short core,short crd,short fifo);

        /*-----------------------------------------------------------*/
        /* Others                                                    */
        /*-----------------------------------------------------------*/
        [DllImport("gts.dll")]
        public static extern short GTN_SetRetainValue(short core, UInt32 address, short count, ref short pData);
        [DllImport("gts.dll")]
        public static extern short GTN_GetRetainValue(short core, UInt32 address, short count, out short pData);


        [DllImport("gts.dll")]
        public static extern short GTN_SetGPIOConfig(short core, short effectiveLevel, short direction);
        [DllImport("gts.dll")]
        public static extern short GTN_SetPrfTorque(short core, short axis, short prfTorque);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAtlTorque(short core, short axis, out short atlTorque);
        [DllImport("gts.dll")]
        public static extern short GTN_PosCurrFeedForward(short core, short profile, double pos, long gtime, short torque, short gtype, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_SetMotionMode(short core, short axis, short motionMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetMotionMode(short core, short axis, out short motionMode);

        [DllImport("gts.dll")]
        public static extern short GTN_SetProfileTime(short core, Int32 profileTime, Int32 delay, Int32 stepCoef);
        [DllImport("gts.dll")]
        public static extern short GTN_GetProfileTime(short core, out Int32 pProfileTime, out Int32 pDelay, out Int32 pStepCoef);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCoreTime(short core, Int32 profileTime, Int32 delay, Int32 stepCoef);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCoreTime(short core, out Int32 pProfileTime, out Int32 pDelay, out Int32 pStepCoef);

        public struct TControlInfo
        {
            public double refPos;
            public double refPosFilter;
            public double refPosFilter2;
            public double cntPos;
            public double cntPosFilter;

            public double error;
            public double refVel;
            public double refAcc;

            public short value;
            public short valueFilter;

            public short offset;
        }
        
        public struct TCommandCount
        {
	        public UInt32 notify;
	        public UInt32 receive;
	        public UInt32 execute;
	        public UInt32 retry;
	        public UInt32 receiveError;
            public UInt32 echo;
        }

        
        [DllImport("gts.dll")]
        public static extern short GT_ClearCommandCount();
        [DllImport("gts.dll")]
        public static extern short GT_GetCommandCount(out TCommandCount pCommandCount);
        [DllImport("gts.dll")]
        public static extern short GT_SetServoTime(Int32 servoTime, Int32 delay, Int32 stepCoef);
        [DllImport("gts.dll")]
        public static extern short GT_GetServoTime(out Int32 pServoTime, out Int32 pDelay, out Int32 pStepCoef);
        [DllImport("gts.dll")]
        public static extern short GT_GetControlInfo(short control, out TControlInfo pControlInfo);

        [DllImport("gts.dll")]
        public static extern short GT_ClearCommandCount(short core);
        [DllImport("gts.dll")]
        public static extern short GT_GetCommandCount(short core,out TCommandCount pCommandCount);
        [DllImport("gts.dll")]
        public static extern short GT_SetServoTime(short core, Int32 servoTime, Int32 delay, Int32 stepCoef);
        [DllImport("gts.dll")]
        public static extern short GT_GetServoTime(short core, out Int32 pServoTime, out Int32 pDelay, out Int32 pStepCoef);
        [DllImport("gts.dll")]
        public static extern short GT_GetControlInfo(short core, short control, out TControlInfo pControlInfo);

        [DllImport("gts.dll")]
        public static extern short GT_SetLongVar(short index, Int32 value);
        [DllImport("gts.dll")]
        public static extern short GT_GetLongVar(short index, out Int32 pValue);
        [DllImport("gts.dll")]
        public static extern short GT_SetDoubleVar(short index, double value);
        [DllImport("gts.dll")]
        public static extern short GT_GetDoubleVar(short index, out double pValue);

        [DllImport("gts.dll")]
        public static extern short GT_GetBufWaitDiStatus(short crd, out short pDiType, out Int16 pDiIndex, out Int16 pLevel, out short pContinueTime, out Int32 pOverTime, out short pFlagMode, out Int32 pSegNum, out short pEnable, out Int32 pCount, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetBufWaitLongVarStatus(short crd, out short pIndex, out Int32 pValue, out short pFlagMode, out Int32 pSegNum, out short pEnable, out short pStatus, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ClearBufWaitStatus(short crd, short fifo);
        
        [DllImport("gts.dll")]
        public static extern short GT_BufWaitDi(short crd, short diType, Int16 diIndex, Int16 level, short continueTime, Int32 overTime, short flagMode, Int32 segNum, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufWaitLongVar(short crd, short index, Int32 value, Int32 overTime, short flagMode, Int32 segNum, short fifo);


        [DllImport("gts.dll")]
        public static extern short GTN_SetLongVar(short core, short index, Int32 value);
        [DllImport("gts.dll")]
        public static extern short GTN_GetLongVar(short core, short index, out Int32 pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_SetDoubleVar(short core, short index, double value);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDoubleVar(short core, short index, out double pValue);

        
        [DllImport("gts.dll")]
        public static extern short GTN_GetBufWaitDiStatus(short core,short crd,out short pDiType,out Int16 pDiIndex,out Int16 pLevel, out Int16 pContinueTime,out Int32 pOverTime,out short pFlagMode,out Int32 pSegNum,out short pEnable,out Int32 pCount,short fifo);
        [DllImport("gts.dll")]
         public static extern short GTN_GetBufWaitLongVarStatus(short core,short crd,out short pIndex,out Int32 pValue,out short pFlagMode,out Int32 pSegNum,out short pEnable,out short pStatus,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearBufWaitStatus(short core,short crd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufWaitDi(short core, short crd, short diType, Int16 diIndex, Int16 level, short continueTime, Int32 overTime, short flagMode, Int32 segNum, short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufWaitLongVar(short core, short crd, short index, Int32 value, Int32 overTime, short flagMode, Int32 segNum, short fifo);

        
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisMotionSmooth(short axis,double time,double k);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisMotionSmooth(short axis,out double pTime,out double pK);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAxisMotionSmooth(short core,short axis,double time,double k);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisMotionSmooth(short core,short axis,out double pTime,out double pK);

        [DllImport("gts.dll")]
        public static extern short GT_BufDoBit(short crd,Int16 doType,Int16 index,short value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufDoBit(short core,short crd,Int16 doType,Int16 index,short value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GTN_BufDoBitDelay(short core, short crd, Int16 doType, Int16 index, short value, Int32 delayTime, short fifo);

        [DllImport("gts.dll")]
        public static extern short GT_SetCrdJerk(short crd,double jerkMax);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdJerk(short crd,out double pJerkMax);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCrdJerk(short core,short crd,double jerkMax);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdJerk(short core,short crd,out double pJerkMax);
        [DllImport("gts.dll")]
        public static extern short GT_SetCrdJerkTime(short crd,double jerkTime,double coef);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdJerkTime(short crd,out double pJerkTime,out double pCoef);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCrdJerkTime(short core,short crd,double jerkTime,double coef);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdJerkTime(short core,short crd,out double pJerkTime,out double pCoef);

        public struct TCrdSmooth
        {
            public short percent;
            public short accStartPercent;
            public short decEndPercent;
            public double reserve;
        }

        [DllImport("gts.dll")]
        public static extern short GT_SetCrdSmooth(short crd, ref TCrdSmooth pCrdSmooth);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdSmooth(short crd, out TCrdSmooth pCrdSmooth);
        [DllImport("gts.dll")]
        public static extern short GT_SetCrdSmoothTime(short crd, short smoothType, ref double pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdSmoothTime(short crd, out short pSmoothType, ref double pPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCrdSmooth(short core, short crd, ref TCrdSmooth pCrdSmooth);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdSmooth(short core, short crd, out TCrdSmooth pCrdSmooth);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCrdSmoothTime(short core, short crd, short smoothType, ref double pPrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCrdSmoothTime(short core, short crd, out short pSmoothType, out double pPrm);

        //////////////////////////////////////////////////////////////////////////
        //Standard Home
        //////////////////////////////////////////////////////////////////////////
        
        public const short STANDARD_HOME_STAGE_IDLE = 0; //未启动回原点
        public const short STANDARD_HOME_STAGE_START = 1; //启动回原点
        public const short STANDARD_HOME_STAGE_SEARCH_HOME = 20; //正在搜索Home
        public const short STANDARD_HOME_STAGE_SEARCH_INDEX   = 30; //正在搜索Index
        public const short STANDARD_HOME_STAGE_GO_HOME = 80; //正在运动到原点
        public const short STANDARD_HOME_STAGE_END = 100; //回原点结束
        public const short STANDARD_HOME_STAGE_START_CHECK = -1; //启动回原点前自检
        public const short STANDARD_HOME_STAGE_CHECKING = -2; //自检中

        public const short STANDARD_HOME_ERROR_NONE = 0; //未发生错误
        public const short STANDARD_HOME_ERROR_DISABLE = 10; //执行回原点的轴未使能
        public const short STANDARD_HOME_ERROR_ALARM = 20; //执行回原点的轴报警
        public const short STANDARD_HOME_ERROR_STOP = 30; //未完成回原点，被停止运动
        public const short STANDARD_HOME_ERROR_ON_LIMIT = 40; //触发了限位无法继续
        public const short STANDARD_HOME_ERROR_NO_HOME = 50; //未找到Home
        public const short STANDARD_HOME_ERROR_NO_INDEX = 60; //未找到Index
        public const short STANDARD_HOME_ERROR_NO_LIMIT = 70; //未找到限位
        public const short STANDARD_HOME_ERROR_ENCODER_DIR_SCALE = -1; //规划器与编码器方向方向相反或者当量不一致
        
        public struct TStandardHomePrm
        {
            public short mode;		 // 回原点模式取值范围1~36
            public double highSpeed; // 搜索Home的速度，单位pulse/ms
            public double lowSpeed;	 // 搜索Index的速度，单位pulse/ms
            public double acc;		 // 回零加速度，单位pulse/ms^2
            public Int32 offset;       // 回零偏移量，单位pulse
            public short check; // 是否启用自检功能，1-启用，其它值-不启用
            public short autoZeroPos; // 回零完毕是否自动清零，1-自动清零，其它值-不清零
            public Int32 motorStopDelay; //电机到位延时，单位：控制周期
            public short pad10;	 // 保留（不需要设置）
            public short pad11;	 // 保留（不需要设置）
            public short pad12;	 // 保留（不需要设置）
        }
        
        public struct TStandardHomeStatus
        {
	        public short run;     // 是正在进行回原点，0—已停止运动，1-正在回原点
	        public short stage;   // 回原点运动的阶段
	        public short error;    // 回原点过程的发生的错误
	        public short pad10;       // 保留（无具体含义）
            public short pad11;       // 保留（无具体含义）
            public short pad12;       // 保留（无具体含义）
            public Int32 capturePos;  // 捕获到Home或Index时刻的编码器位置
	        public Int32 targetPos;    // 需要运动到的目标位置（原点位置或者原点位置+偏移量），在搜索Limit时或者搜索Home或Index时，设置的搜索距离为0，那么该值显示为805306368
        }

        [DllImport("gts.dll")]
        public static extern short GT_ExecuteStandardHome(short axis, ref TStandardHomePrm pHomePrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetStandardHomePrm(short axis, out TStandardHomePrm pHomePrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetStandardHomeStatus(short axis, out TStandardHomeStatus pHomeStatus);

        [DllImport("gts.dll")]
        public static extern short GTN_ExecuteStandardHome(short core, short axis, ref TStandardHomePrm pHomePrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetStandardHomePrm(short core, short axis, out TStandardHomePrm pHomePrm);
        [DllImport("gts.dll")]
        public static extern short GTN_GetStandardHomeStatus(short core, short axis, out TStandardHomeStatus pHomeStatus);

        public struct TLaserOnOffCount
        {
            public UInt32 onCount;
            public UInt32 offCount;
            public UInt32 onCountInFpga;
            public UInt32 offCountInFpga;
            public UInt32 pad1;
            public UInt32 pad2;
            public UInt32 pad3;
            public UInt32 pad4;
        }

        [DllImport("gts.dll")]
        public static extern short GTN_GetLaserOnOffCount(short core, short channel, out TLaserOnOffCount pLaserCount);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearLaserOnOffCount(short core, short channel);

        [DllImport("gts.dll")]
        public static extern short GTN_BufPosComparePsoPrm(short core, short crd, short index, ref TPosComparePsoPrm pPrm, short fifo);

        [DllImport("gts.dll")]
        public static extern short GT_SetAxisInputShaping(short axis,short enable,short count,double k);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAxisInputShaping(short core,short axis,short enable,short count,double k);

        
        [DllImport("gts.dll")]
        public static extern short GT_SetGantrySynchErrorCompensate2DTable(short tableIndex,ref TCompensate2DTable pTable,ref long pData,short extend);
        [DllImport("gts.dll")]
        public static extern short GT_GetGantrySynchErrorCompensate2DTable(short tableIndex,out TCompensate2DTable pTable,out short pExtend);
        [DllImport("gts.dll")]
        public static extern short GT_SetGantrySynchErrorCompensate2D(short group,ref TCompensate2D pComp2d);
        [DllImport("gts.dll")]
        public static extern short GT_GetGantrySynchErrorCompensate2D(short group,out TCompensate2D pComp2d);
        [DllImport("gts.dll")]
        public static extern short GT_GetGantrySynchErrorCompensate2DValue(short group,out double pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_SetGantrySynchErrorCompensate2DTable(short core,short tableIndex,ref TCompensate2DTable pTable,ref long pData,short extend);
        [DllImport("gts.dll")]
        public static extern short GTN_SetGantrySynchErrorCompensate2D(short core,short group,ref TCompensate2D pComp2d);

        [DllImport("gts.dll")]
        public static extern short GTN_SetLaserFollowTable(short core,short tableId,long n,ref double pVel,ref double pPower,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_LaserFollowOff(short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_LaserFollowOff(short core,short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GTN_SetLaserOnOffSmooth(short core,short crd,short fifo,short channel,short mask);

        public struct TBufWaitDiStatusEx
        {
	        short type;
	        short enable;
	        short flagMode;
	        short diType;
	        short diIndex;
	        short diValue;
	        ushort continueTime;
	        ushort trigDelay;
	        UInt32 overTime;
	        UInt32 counter;
	        Int32 longVar;
	        Int32 segNum;
	        short stop;
	        short overTimeStop;
	        short pad11;
	        short pad12;
        };
        [DllImport("gts.dll")]
        public static extern short GTN_GetBufWaitDiStatusEx(short core,short crd,short fifo,out TBufWaitDiStatusEx pStatus);

        public struct TLatchValueInfo
        {
	        short fifoFull;
	        short pad11;
	        short pad12;
	        short pad13;
	        double pad21;
	        double pad22;
        };

        [DllImport("gts.dll")]
        public static extern short GTN_SetPosCompareFifoMode(short core, short index, short mode, short enable);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPosCompareFifoMode(short core,short index,out short pMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPosCompareLatchValue(short core,short index,Int32 count,out Int32 pDataX,out Int32 pDataY,out Int32 pCount,out TLatchValueInfo pInfo);
        
        public struct TTaskMoveEscape
        {
	        UInt32 profileMask;
	        Int32 offset;
	        double vel;
	        double acc;
        };

        public struct TEventStatus
        {
	        short eventHit;
	        short pad11;
	        short pad12;
	        short pad13;
	        Int32 pad21;
	        Int32 pad22;
	        double pad31;
	        double pad32;
        };

        public struct TaskStatus
        {
	        short start;
	        short execute;
	        short pad11;
	        short pad12;
	        Int32 pad21;
	        Int32 pad22;
	        double pad31;
	        double pad32;
        };
                
        [DllImport("gts.dll")]
        public static extern short GTN_GetEventStatus(short core,short index,out TEventStatus pStatus);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTaskStatus(short core,short index,out TaskStatus pStatus);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTriggerMoveEscape(short core,short trigger,ref TTaskMoveEscape pPrm);
                
       }
}
