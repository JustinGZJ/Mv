using System;
using System.Runtime.InteropServices;

namespace GXN
{
    public class mc_cfg
	{
        /*-----------------------------------------------------------*/
        /* conifg of controller                                      */
        /*-----------------------------------------------------------*/
		public const short RES_LIMIT = 8;
		public const short RES_ALARM = 8;
		public const short RES_HOME = 8;
		public const short RES_GPI = 16;
		public const short RES_ARRIVE = 8;
		public const short RES_MPG = 7;
			
		public const short RES_ENABLE = 8;
		public const short RES_CLEAR = 8;
		public const short RES_GPO = 16;
			
		public const short RES_DAC = 12;
		public const short RES_STEP = 8;
		public const short RES_PULSE = 8;
		public const short RES_ENCODER = 11;
		public const short RES_LASER = 2;  
			
		public const short AXIS_MAX = 8;
		public const short PROFILE_MAX = 8;
		public const short CONTROL_MAX = 8;
			
		public const short PRF_MAP_MAX = 2;
		public const short ENC_MAP_MAX = 2;
			
		public struct TDiConfig
		{
            public short active;
            public short reverse;
            public short filterTime;
		}

		public struct TCountConfig
		{
            public short active;
            public short reverse;
            public short filterType;

            public short captureSource;
            public short captureHomeSense;
            public short captureIndexSense;
		}

		public struct TDoConfig
		{
            public short active;
            public short axis;
            public short axisItem;
            public short reverse;
		}

		public struct TStepConfig
		{
            public short active;
            public short axis;
            public short mode;
            public short parameter;
            public short reverse;
		}

		public struct TDacConfig
		{
            public short active;
            public short control;
            public short reverse;
            public short bias;
            public short limit;
		}

        public struct TAdcConfig
        {
            public short active;
            public short reverse;
            public double a;
            public double b;
            public short filterMode;
        } 

		public struct TControlConfig
		{
            public short active;
            public short axis;
            public short encoder1;
            public short encoder2;
            public Int32 errorLimit;
            public short filterType1;
            public short filterType2;
            public short filterType3;
            public short encoderSmooth;
            public short controlSmooth;
		}

        public struct TControlConfigEx
        {
            public short refType;
            public short refIndex;

            public short feedbackType;
            public short feedbackIndex;

            public Int32 errorLimit;
            public short feedbackSmooth;
            public short controlSmooth;
        } 

		public struct TProfileConfig
		{
            public short active;
            public double decSmoothStop;
            public double decAbruptStop;
		}

		public struct TAxisConfig
		{
            public short active;
            public short alarmType;
            public short alarmIndex;
            public short limitPositiveType;
            public short limitPositiveIndex;
            public short limitNegativeType;
            public short limitNegativeIndex;
            public short smoothStopType;
            public short smoothStopIndex;
            public short abruptStopType;
            public short abruptStopIndex;
            public Int32 prfMap;
            public Int32 encMap;
            public short prfMapAlpha1;
            public short prfMapAlpha2;
            public short prfMapBeta1;
            public short prfMapBeta2;
            public short encMapAlpha1;
            public short encMapAlpha2;
            public short encMapBeta1;
            public short encMapBeta2;
		}

		public struct TMcConfig
		{
            public TProfileConfig profile1;
            public TProfileConfig profile2;
            public TProfileConfig profile3;
            public TProfileConfig profile4;
            public TProfileConfig profile5;
            public TProfileConfig profile6;
            public TProfileConfig profile7;
            public TProfileConfig profile8;
            public TAxisConfig axis1;
            public TAxisConfig axis2;
            public TAxisConfig axis3;
            public TAxisConfig axis4;
            public TAxisConfig axis5;
            public TAxisConfig axis6;
            public TAxisConfig axis7;
            public TAxisConfig axis8;
            public TControlConfig control1;
            public TControlConfig control2;
            public TControlConfig control3;
            public TControlConfig control4;
            public TControlConfig control5;
            public TControlConfig control6;
            public TControlConfig control7;
            public TControlConfig control8;
            public TDacConfig dac1;
            public TDacConfig dac2;
            public TDacConfig dac3;
            public TDacConfig dac4;
            public TDacConfig dac5;
            public TDacConfig dac6;
            public TDacConfig dac7;
            public TDacConfig dac8;
            public TDacConfig dac9;
            public TDacConfig dac10;
            public TDacConfig dac11;
            public TDacConfig dac12;
            public TStepConfig step1;
            public TStepConfig step2;
            public TStepConfig step3;
            public TStepConfig step4;
            public TStepConfig step5;
            public TStepConfig step6;
            public TStepConfig step7;
            public TStepConfig step8;
            public TCountConfig encoder1;
            public TCountConfig encoder2;
            public TCountConfig encoder3;
            public TCountConfig encoder4;
            public TCountConfig encoder5;
            public TCountConfig encoder6;
            public TCountConfig encoder7;
            public TCountConfig encoder8;
            public TCountConfig encoder9;
            public TCountConfig encoder10;
            public TCountConfig encoder11;
            public TCountConfig pulse1;
            public TCountConfig pulse2;
            public TCountConfig pulse3;
            public TCountConfig pulse4;
            public TCountConfig pulse5;
            public TCountConfig pulse6;
            public TCountConfig pulse7;
            public TCountConfig pulse8;
            public TDoConfig enable1;
            public TDoConfig enable2;
            public TDoConfig enable3;
            public TDoConfig enable4;
            public TDoConfig enable5;
            public TDoConfig enable6;
            public TDoConfig enable7;
            public TDoConfig enable8;
            public TDoConfig clear1;
            public TDoConfig clear2;
            public TDoConfig clear3;
            public TDoConfig clear4;
            public TDoConfig clear5;
            public TDoConfig clear6;
            public TDoConfig clear7;
            public TDoConfig clear8;
            public TDoConfig gpo1;
            public TDoConfig gpo2;
            public TDoConfig gpo3;
            public TDoConfig gpo4;
            public TDoConfig gpo5;
            public TDoConfig gpo6;
            public TDoConfig gpo7;
            public TDoConfig gpo8;
            public TDoConfig gpo9;
            public TDoConfig gpo10;
            public TDoConfig gpo11;
            public TDoConfig gpo12;
            public TDoConfig gpo13;
            public TDoConfig gpo14;
            public TDoConfig gpo15;
            public TDoConfig gpo16;
            public TDiConfig limitPositive1;
            public TDiConfig limitPositive2;
            public TDiConfig limitPositive3;
            public TDiConfig limitPositive4;
            public TDiConfig limitPositive5;
            public TDiConfig limitPositive6;
            public TDiConfig limitPositive7;
            public TDiConfig limitPositive8;
            public TDiConfig limitNegative1;
            public TDiConfig limitNegative2;
            public TDiConfig limitNegative3;
            public TDiConfig limitNegative4;
            public TDiConfig limitNegative5;
            public TDiConfig limitNegative6;
            public TDiConfig limitNegative7;
            public TDiConfig limitNegative8;
            public TDiConfig alarm1;
            public TDiConfig alarm2;
            public TDiConfig alarm3;
            public TDiConfig alarm4;
            public TDiConfig alarm5;
            public TDiConfig alarm6;
            public TDiConfig alarm7;
            public TDiConfig alarm8;
            public TDiConfig home1;
            public TDiConfig home2;
            public TDiConfig home3;
            public TDiConfig home4;
            public TDiConfig home5;
            public TDiConfig home6;
            public TDiConfig home7;
            public TDiConfig home8;
            public TDiConfig gpi1;
            public TDiConfig gpi2;
            public TDiConfig gpi3;
            public TDiConfig gpi4;
            public TDiConfig gpi5;
            public TDiConfig gpi6;
            public TDiConfig gpi7;
            public TDiConfig gpi8;
            public TDiConfig arrive1;
            public TDiConfig arrive2;
            public TDiConfig arrive3;
            public TDiConfig arrive4;
            public TDiConfig arrive5;
            public TDiConfig arrive6;
            public TDiConfig arrive7;
            public TDiConfig arrive8;
            public TDiConfig mpg1;
            public TDiConfig mpg2;
            public TDiConfig mpg3;
            public TDiConfig mpg4;
            public TDiConfig mpg5;
            public TDiConfig mpg6;
            public TDiConfig mpg7;
		}
        [DllImport("gts.dll")]
        public static extern short GT_SaveConfig(out string pFile);
		[DllImport("gts.dll")]
		public static extern short GT_SetDiConfig(short diType,short diIndex,ref TDiConfig pDi);
		[DllImport("gts.dll")]
		public static extern short GT_GetDiConfig(short diType,short diIndex,out TDiConfig pDi);
		[DllImport("gts.dll")]
		public static extern short GT_SetDoConfig(short doType,short doIndex,ref TDoConfig pDo);
		[DllImport("gts.dll")]
		public static extern short GT_GetDoConfig(short doType,short doIndex,out TDoConfig pDo);
		[DllImport("gts.dll")]
		public static extern short GT_SetStepConfig(short step,ref TStepConfig pStep);
		[DllImport("gts.dll")]
		public static extern short GT_GetStepConfig(short step,out TStepConfig pStep);
		[DllImport("gts.dll")]
		public static extern short GT_SetDacConfig(short dac,ref TDacConfig pDac);
		[DllImport("gts.dll")]
		public static extern short GT_GetDacConfig(short dac,out TDacConfig pDac);
        [DllImport("gts.dll")]
        public static extern short GT_SetAdcConfig(short adc,ref TAdcConfig pAdc);
        [DllImport("gts.dll")]
        public static extern short GT_GetAdcConfig(short adc,out TAdcConfig pAdc);
		[DllImport("gts.dll")]
		public static extern short GT_SetCountConfig(short countType,short countIndex,ref TCountConfig pCount);
		[DllImport("gts.dll")]
		public static extern short GT_GetCountConfig(short countType,short countIndex,out TCountConfig pCount);
		[DllImport("gts.dll")]
		public static extern short GT_SetControlConfig(short control,ref TControlConfig pControl);
		[DllImport("gts.dll")]
		public static extern short GT_GetControlConfig(short control,out TControlConfig pControl);
        [DllImport("gts.dll")]
        public static extern short GT_SetControlConfigEx(short control, ref TControlConfigEx pControl);
        [DllImport("gts.dll")]
        public static extern short GT_GetControlConfigEx(short control, out TControlConfigEx pControl);
		[DllImport("gts.dll")]
		public static extern short GT_SetProfileConfig(short profile,ref TProfileConfig pProfile);
		[DllImport("gts.dll")]
		public static extern short GT_GetProfileConfig(short profile,out TProfileConfig pProfile);
		[DllImport("gts.dll")]
		public static extern short GT_SetAxisConfig(short axis,ref TAxisConfig pAxis);
		[DllImport("gts.dll")]
		public static extern short GT_GetAxisConfig(short axis,out TAxisConfig pAxis);
        [DllImport("gts.dll")]
		public static extern short GT_ProfileScale(short axis,short alpha,short beta);
        [DllImport("gts.dll")]
		public static extern short GT_EncScale(short axis,short alpha,short beta);
        
        
        [DllImport("gts.dll")]
		public static extern short GT_EncSns(ushort sense);
        [DllImport("gts.dll")]
		public static extern short GT_LmtSns(ushort sense);
        [DllImport("gts.dll")]
		public static extern short GT_GpiSns(ushort sense);
        [DllImport("gts.dll")]
		public static extern short GT_SetAdcFilter(short adc,short filterTime);

        [DllImport("gts.dll")]
		public static extern short GT_GetConfigTable(short type,out short pCount);
		[DllImport("gts.dll")]
		public static extern short GT_GetConfigTableAll();
		
        
        [DllImport("gts.dll")]
		public static extern short GT_SetMcConfig(ref TMcConfig pMc);
		[DllImport("gts.dll")]
		public static extern short GT_GetMcConfig(out TMcConfig pMc);
		
        
        [DllImport("gts.dll")]
		public static extern short GT_SetMcConfigToFile(ref TMcConfig pMc,ref char pFile);
		[DllImport("gts.dll")]
		public static extern short GT_GetMcConfigFromFile(out TMcConfig pMc,out char pFile);

        [DllImport("gts.dll")]
        public static extern short GTN_SaveConfig(short core, out string pFile);
        [DllImport("gts.dll")]
        public static extern short GTN_SetDiConfig(short core, short diType, short diIndex, ref TDiConfig pDi);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDiConfig(short core, short diType, short diIndex, out TDiConfig pDi);
        [DllImport("gts.dll")]
        public static extern short GTN_SetDoConfig(short core, short doType, short doIndex,ref TDoConfig pDo);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDoConfig(short core, short doType, short doIndex,out TDoConfig pDo);
        [DllImport("gts.dll")]
        public static extern short GTN_SetStepConfig(short core, short step,ref TStepConfig pStep);
        [DllImport("gts.dll")]
        public static extern short GTN_GetStepConfig(short core, short step,out TStepConfig pStep);
        [DllImport("gts.dll")]
        public static extern short GTN_SetDacConfig(short core, short dac,ref TDacConfig pDac);
        [DllImport("gts.dll")]
        public static extern short GTN_GetDacConfig(short core, short dac,out TDacConfig pDac);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAdcConfig(short core, short adc, ref TAdcConfig pAdc);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAdcConfig(short core, short adc, out TAdcConfig pAdc);
        [DllImport("gts.dll")]
        public static extern short GTN_SetCountConfig(short core, short countType, short countIndex,ref TCountConfig pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetCountConfig(short core, short countType, short countIndex,out TCountConfig pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_SetControlConfig(short core, short control,ref TControlConfig pControl);
        [DllImport("gts.dll")]
        public static extern short GTN_GetControlConfig(short core, short control,out TControlConfig pControl);
        [DllImport("gts.dll")]
        public static extern short GTN_SetControlConfigEx(short core, short control,ref TControlConfigEx pControl);
        [DllImport("gts.dll")]
        public static extern short GTN_GetControlConfigEx(short core, short control,out TControlConfigEx pControl);
        [DllImport("gts.dll")]
        public static extern short GTN_SetProfileConfig(short core, short profile,ref TProfileConfig pProfile);
        [DllImport("gts.dll")]
        public static extern short GTN_GetProfileConfig(short core, short profile,out TProfileConfig pProfile);
        [DllImport("gts.dll")]
        public static extern short GTN_SetAxisConfig(short core, short axis,ref TAxisConfig pAxis);
        [DllImport("gts.dll")]
        public static extern short GTN_GetAxisConfig(short core, short axis,out TAxisConfig pAxis);
        [DllImport("gts.dll")]
        public static extern short GTN_ProfileScale(short core, short axis, short alpha, short beta);
        [DllImport("gts.dll")]
        public static extern short GTN_EncScale(short core, short axis, short alpha, short beta);
        /*-----------------------------------------------------------*/
        /* Config of Ext-Module                                      */
        /*-----------------------------------------------------------*/
        public struct TExtModuleStatus
        {
            public short active;
            public short checkError;
            public short linkError;
            public short packageErrorCount;
            public short pad1;
            public short pad2;
            public short pad3;
            public short pad4;
            public short pad5;
            public short pad6;
            public short pad7;
            public short pad8;
        }

        public struct TExtModuleType
        {
            public short type;
            public short input;
            public short output;
        }

        public struct TExtIoMap
        {
            public short station;
            public short module;
            public short index;
        }

        [DllImport("gts.dll")]
        public static extern short GTN_SaveExtModuleConfig(short core, out string pFile);
        [DllImport("gts.dll")]
        public static extern short GTN_SaveRingNetConfig(short core, out string pFile);
        [DllImport("gts.dll")]
        public static extern short GTN_ExtModuleOn(short core, short station);
        [DllImport("gts.dll")]
        public static extern short GTN_ExtModuleOff(short core, short station);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtModuleStatus(short core, short station, out TExtModuleStatus pStatus);
        [DllImport("gts.dll")]
        public static extern short GTN_SetExtModuleId(short core, short station, short count, ref short pId);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtModuleId(short core, short station, short count, out short pId);
        [DllImport("gts.dll")]
        public static extern short GTN_SetExtModuleReverse(short core, short station, short module, short inputCount, ref short pInputReverse, short outputCount, ref short pOutputReverse);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtModuleReverse(short core, short station, short module, short inputCount, out short pInputReverse, short outputCount, out short pOutputReverse);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtModuleCount(short core, short station, out short pCount);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtModuleType(short core, short station, short module, out TExtModuleType pModuleType);
        [DllImport("gts.dll")]
        public static extern short GTN_SetExtIoMap(short core, short type, short index, ref TExtIoMap pMap);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtIoMap(short core, short type, short index, out TExtIoMap pMap);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearExtIoMap(short core, short type);
        [DllImport("gts.dll")]
        public static extern short GTN_SetExtAoRange(short core, short index, double max, double min);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtAoRange(short core, short index, out double pMax, out double pMin);
        [DllImport("gts.dll")]
        public static extern short GTN_SetExtAiRange(short core, short index, double max, double min);
        [DllImport("gts.dll")]
        public static extern short GTN_GetExtAiRange(short core, short index, out double pMax, out double pMin);

        /*-----------------------------------------------------------*/
        /* Config of Laser and Scan                                  */
        /*-----------------------------------------------------------*/
        public struct TScanCommandMotion
        {
            public Int32 segmentNumber;
            public short x;
            public short y;
            public Int32 deltaX;
            public Int32 deltaY;
            public Int32 vel;
            public Int32 acc;
        }
        public struct TScanCommandMotionDelay
        {
            public Int32 delay;
        }

        public struct TScanCommandDo
        {
            public short doType;
            public short doMask;
            public short doValue;
        }

        public struct TScanCommandDoDelay
        {
            public Int32 delay;
        }

        public struct TScanCommandLaser
        {
            public short mask;
            public short value;
        }

        public struct TScanCommandLaserDelay
        {
            public Int32 laserOnDelay;
            public Int32 laserOffDelay;
        }

        public struct TScanCommandLaserPower
        {
            public Int32 power;
        }

        public struct TScanCommandLaserFrequency
        {
            public Int32 frequency;
        }

        public struct TScanCommandLaserPulseWidth
        {
            public Int32 pulseWidth;
        }

        public struct TScanCommandDa
        {
            public short daIndex;
            public short daValue;
        }

        public struct TScanMap
        {
            public short module;
            public short fifo;
        }

        [DllImport("gts.dll")]
        public static extern short GTN_SetScanMap(short core, short index, ref TScanMap pMap);
        [DllImport("gts.dll")]
        public static extern short GTN_GetScanMap(short core, short index, out TScanMap pMap);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearScanMap(short core);
        [DllImport("gts.dll")]
        public static extern short GTN_UpdateScanMap(short core);

        /*-----------------------------------------------------------*/
        /* Config of Position Compare                                */
        /*-----------------------------------------------------------*/
        public struct TPosCompareMap
        {
            public short module;
            public short fifo;
        }

        [DllImport("gts.dll")]
        public static extern short GTN_SetPosCompareMap(short core, short index, ref TPosCompareMap pMap);
        [DllImport("gts.dll")]
        public static extern short GTN_GetPosCompareMap(short core, short index, out TPosCompareMap pMap);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearPosCompareMap(short core);
    }
}