using System;
using System.Runtime.InteropServices;
/*-----------------------------------------------------------*/
/* Ringnet                                                  */
/*-----------------------------------------------------------*/
namespace GXN
{ 
    public class mc_ringnet
    {
    	public const short RTN_SUCCESS = 0;

        public const short RTN_MALLOC_FAIL = -100;          /* malloc memory fail */
        public const short RTN_FREE_FAIL = -101;            /* free memory or delete the object fail */
        public const short RTN_NULL_POINT = -102;           /* the param point input is null */ 
        public const short RTN_ERR_ORDER = -103;            /* call the function order is wrong, some msg isn't validable */
        public const short RTN_PCI_NULL = -104;             /* the pci address is empty, can't access the pci device*/
        public const short RTN_PARAM_OVERFLOW = -105;       /* the param input is too larget*/
        public const short RTN_LINK_FAIL  = -106;           /* the two ports both link fail*/ 
        public const short RTN_IMPOSSIBLE_ERR = -107;       /* it means the system or same function work wrong*/
        public const short RTN_TOPOLOGY_CONFLICT = -108;    /* the id conflict*/
        public const short RTN_TOPOLOGY_ABNORMAL = -109;    /* scan the net abnormal*/
        public const short RTN_STATION_ALONE = -110;        /* the device no id, it means the device id is 0xF0 */
        public const short RTN_WAIT_OBJECT_OVERTIME = -111; /* multi thread wait for object overtime */
        public const short RTN_ACCESS_OVERFLOW = -112;      /* data[i];  i is larger than the define */
        public const short RTN_NO_STATION = -113;           /* the station accessed not existent */
        public const short RTN_OBJECT_UNCREATED = -114;     /* the object not created yet*/
        public const short RTN_PARAM_ERR = -115;            /* the param input is wrong*/
        public const short RTN_PDU_CFG_ERR = -116;          /*Pdu DMA Cfg Err*/
        public const short RTN_PCI_FPGA_ERR = -117;         /*PCI op err or FPGA op err*/
        public const short RTN_CHECK_RW_ERR = -118;      	/*data write to reg, then rd out, and check err */
        public const short RTN_REMOTE_UNEABLE = -119;       /*the device which will be ctrl by net can't be ctrl by net function*/ 

        public const short RTN_NET_REQ_DATA_NUM_ZERO = -120;     /*mail op or pdu op req data num can't be 0*/
        public const short RTN_WAIT_NET_OBJECT_OVERTIME = -121;  /* net op multi thread wait for object overtime */
        public const short RTN_WAIT_NET_RESP_OVERTIME = -122;    /* Can't wait for resp */
        public const short RTN_WAIT_NET_RESP_ERR = -123;         /*wait mailbox op err*/
        public const short RTN_INITIAL_ERR = -124;               /*initial the device err*/
        public const short RTN_PC_NO_READY = -125;               /*find the station'pc isn't work*/ 
        public const short RTN_STATION_NO_EXIST = -126; 
        public const short RTN_MASTER_FUNCTION = -127;           /* this funciton only used by master */

        public const short RTN_NOT_ALL_RETURN = -128;            /* the GT_RN_PtProcessData funciton fail return */
        public const short RTN_NUM_NOT_EQUAL = -129;             /* the station number of RingNet do not equal  the station number of CFG */

        public const short RTN_CHECK_STATION_ONLINE_NUM_ERR = -130;/*Check no slave*/
        public const short RTN_FILE_ERR_OPEN = -131;               /*open file error*/
        public const short RTN_FILE_ERR_FORMAT = -132;             /*parse file error*/
        public const short RTN_FILE_ERR_MISSMATCH = -133;          /*file info is not match with the actual ones*/
        public const short RTN_DMALIST_ERR_MISSMATCH = -134;       /*can't find the slave*/

        public const short RTN_REQUSET_MAIL_BUS_OVERTIME = -150;   /*Requset Mail Bus Err*/
        public const short RTN_INSTRCTION_ERR = -151;              /*instrctions err*/
        public const short RTN_MAIL_RESP_REQ_ERR = -152;           /*RN_MailRespReq  err*/
        public const short RTN_CTRL_SRC_ERR = -153;                /* the controlled source  is error */
        public const short RTN_PACKET_ERR = -154;                  /*packet is error*/
        public const short RTN_STATION_ID_ERR = -155;              /*the device id is not in the right rang*/
        public const short RTN_WAIT_NET_PDU_RESP_OVERTIME = -156;  /*net pdu op wait overtime*/
        public const short RTN_ETHCAT_ENC_POS_ERR = -157;

        public const short RTN_IDLINK_PACKET_ERR = -200;           /*ilink master  decode err! packet_length is not match*/
        public const short RTN_IDLINK_PACKET_END_ERR = -201;       /* the ending of ilink packet is not 0xFF*/
        public const short RTN_IDLINK_TYPER_ERR = -202;            /* the type of ilink module is error*/
        public const short RTN_IDLINK_LOST_CNT = -203;             /* the ilink module has lost connection*/
        public const short RTN_IDLINK_CTRL_SRC_ERR = -204;         /* the controlled source of ilink module is error */
        public const short RTN_IDLINK_UPDATA_ERR = -205;           /* the ilink module updata error*/
        public const short RTN_IDLINK_NUM_ERR = -206;              /* the ilink num larger the IDLINK_MAX_NUM(30) */
        public const short RTN_IDLINK_NUM_ZERO = -207;             /* the ilink num is zero */

        public const short RTN_NO_PACKET = 301;                    /* no valid packet */
        public const short RTN_RX_ERR_PDU_PACKET = -302;           /* ERR PDU PACKET */
        public const short RTN_STATE_MECHINE_ERR = -303; 
        public const short RTN_PCI_DSP_UN_FINISH = 304;
        public const short RTN_SEND_ALL_FAIL = -305;
        public const short RTN_STATION_CLOSE = 310;
        public const short RTN_STATION_RESP_FAIL = 311;		

        public const short RTN_UPDATA_MODAL_ERR = -330;            /* update the modal in normal way fail*/

        public const short RTN_NO_MAIL_DATA = 340;                 /*There is no mail data*/
        public const short RTN_NO_PDU_DATA = 341;                  /*There is no pdu data*/


        public const short RTN_FILE_PARAM_NUM_ERR = -500;
        public const short RTN_FILE_PARAM_LEN_ERR = -501;
        public const short RTN_FILE_MALLOC_FAIL = -502;
        public const short RTN_FILE_FREE_FAIL = -503;
        public const short RTN_FILE_PARAM_ERR = -504;
        public const short RTN_FILE_NOT_EXSITS = 505;
        public const short RTN_FILE_CREATE_FAIL = 510;
        public const short RTN_FILE_DELETE_FAIL = 511;
        public const short RTN_FIFE_CRC_CHECK_ERR = -512;
        public const short RTN_FIFE_FUNCTION_ID_RETURN_ERR = -600;

        public const short RTN_DLL_WINCE = -800;
        public const short RTN_DLL_WIN32 = -801;

        public const short RTN_XML_STATION_ERR = -900;                  //dma config file confilit with slave type

        
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetEncPos(short encoder,out double pValue, short count, ref UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetAxisError(short axis, out double pValue, short count, ref UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetPrfMode(short axis, out Int32 pValue, short count, ref UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetAuEncPos(short encoder, out double pValue, short count, ref UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetCaptureStatus(short encoder, out short pStatus, out Int32 pValue, short count, ref UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetSts(short axis, out Int32 pSts, short count, ref UInt32 pClock);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetPowerSts(out Int32 pValue);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetEcatAxisACTArray(short axis, ref short pCur, out short pTorque, short count);
        [DllImport("gts.dll")]
        public static extern short GT_RN_PtSpaceArray(short profile, out short pSpace, short fifo, short count);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetDoEx(short doType, out Int32 pValue);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetDiEx(short diType, out Int32 pValue);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetDo(short doType, out Int32 pValue);
        [DllImport("gts.dll")]
        public static extern short GT_RN_GetDi(short diType, out Int32 pValue);
        
        [DllImport("gts.dll")]
        public static extern short GTN_LoadRingNetConfig(short core,string pFile);
        [DllImport("gts.dll")]
        public static extern short GTN_SaveRingNetConfig(short core,string pFile);

        public const short TERMINAL_LOAD_MODE_NONE = 0;
        public const short TERMINAL_LOAD_MODE_BOOT = 2;

        
        public struct TTerminalStatus
        {
	        public UInt16 type;
	        public short id;
	        public Int32 status;
	        public UInt32 synchCount;
	        public UInt32 ringNetType;
	        public UInt32 portStatus;
	        public UInt32 sportDropCount;
	        public UInt32 reserve1;
            public UInt32 reserve2;
            public UInt32 reserve3;
            public UInt32 reserve4;
            public UInt32 reserve5;
            public UInt32 reserve6;
            public UInt32 reserve7;
        }

        [DllImport("gts.dll")]
        public static extern short GTN_TerminalInit(short core,short detect);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalVersion(short core,short index,out GTN.mc.TVersion pTerminalVersion);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTerminalPermit(short core,short index,short dataType,UInt16 permit);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTerminalPermitEx(short core,short station,short dataType,ref short permit,short index,short count);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalPermitEx(short core,short station,short dataType,out short pPermit,short index,short count);

        [DllImport("gts.dll")]
        public static extern short GTN_FindStation(short core,short station,UInt32 time);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalPermit(short core,short index,short dataType,out UInt16 pPermit);
        [DllImport("gts.dll")]
        public static extern short GTN_ProgramTerminalConfig(short core,short loadMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalConfigLoadMode(short core,out short pLoadMode);

        [DllImport("gts.dll")]
        public static extern short GTN_ReadPhysicalMap();
        [DllImport("gts.dll")]
        public static extern short ConvertPhysical(short core,short dataType,short terminal,short index);

        [DllImport("gts.dll")]
        public static extern short GTN_SetTerminalSafeMode(short core,short index,short safeMode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalSafeMode(short core,short index,ref short pSafeMode);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearTerminalSafeMode(short core,short index);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalStatus(short core,short index,out TTerminalStatus pTerminalStatus);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalType(short core,short count,out UInt16 pType,ref short pTypeConnect);

        /*-----------------------------------------------------------*/
        /* Config of module                                          */
        /*-----------------------------------------------------------*/
        public const short TERMINAL_OPERATION_NONE = 0;
        public const short TERMINAL_OPERATION_SKIP = 1;
        public const short TERMINAL_OPERATION_CLEAR = 2;
        public const short TERMINAL_OPERATION_RESET_MODULE = 3;

        public const short TERMINAL_OPERATION_PROGRAM = 11;
        
        public struct TRingNetCrcStatus
        {
            public UInt32 portACrcOkCnt;
            public UInt16 portACrcErrorCnt;
            public UInt32 portBCrcOkCnt;
            public UInt16 portBCrcErrorCnt;
            public UInt32 reserve;//Ŀǰ���ڶ�ȡFLASH�����ݳ���
        }

        public struct TTerminalError
        {
            public UInt16 errorCountReceive;
            public UInt16 errorCountPackageDown;
            public UInt16 errorCountPackageUp;
            /*unsigned long portACrcOkCnt;
	        unsigned short portACrcErrorCnt;
	        unsigned long portBCrcOkCnt;
	        unsigned short portBCrcErrorCnt;*/
            public UInt16 reserve1;
            public UInt16 reserve2;
            public UInt16 reserve3;
            public UInt16 reserve4;
            public UInt16 reserve5;
            public UInt16 reserve6;
            public UInt16 reserve7;
            public UInt16 reserve8;
            public UInt16 reserve9;
            public UInt16 reserve10;
            public UInt16 reserve11;
            public UInt16 reserve12;
            public UInt16 reserve13;
        }
        public struct TTerminalMap
        {
            public short moduleDataType;
            public short moduleDataIndex;
            public short dataIndex;
            public short dataCount;
        }

        [DllImport("gts.dll")]
        public static extern short GT_SetMailbox(short core, short station, UInt16 byteAddress, ref UInt16 pData, UInt16 wordCount, UInt16 dataMode, UInt16 desId, UInt16 type);
        [DllImport("gts.dll")]
        public static extern short GT_GetMailbox(short core, short station, UInt16 byteAddress, out UInt16 pData, UInt16 wordCount, UInt16 dataMode, UInt16 desId, UInt16 type);

        [DllImport("gts.dll")]
        public static extern short GTN_LoadTerminalConfig(short core, string pFile);
        [DllImport("gts.dll")]
        public static extern short GTN_SaveTerminalConfig(short core, string pFile);
        [DllImport("gts.dll")]
        public static extern short GTN_TerminalOn(short core, short index);
        [DllImport("gts.dll")]
        public static extern short GTN_TerminalSynch(short core, short index);
        [DllImport("gts.dll")]
        public static extern short GTN_GetRingNetCrcStatus(short core, short index, out TRingNetCrcStatus pRingNetCrcStatus);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalError(short core, short index, out TTerminalError pTerminalError);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTerminalType(short core, short count, ref short pType);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalLinkStatus(short core, short count, out short ringNetType, out short pLinkStatus);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTerminalMap(short core, short dataType, short moduleIndex, ref TTerminalMap pMap);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalMap(short core, short dataType, short moduleIndex, out TTerminalMap pMap);
        [DllImport("gts.dll")]
        public static extern short GTN_ClearTerminalMap(short core, short dataType);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTerminalMode(short core, short station, UInt16 mode);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalMode(short core, short station, out UInt16 pMode);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTerminalTest(short core, short station, short index, UInt16 value);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalTest(short core, short station, short index, out UInt16 pValue);
        [DllImport("gts.dll")]
        public static extern short GTN_SetTerminalOperation(short core, short operation);
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalOperation(short core, out short pOperation);
        [DllImport("gts.dll")]
        public static extern short GTN_SetMailbox(short core, short station, UInt16 byteAddress, ref UInt16 pData, UInt16 wordCount, UInt16 dataMode, UInt16 desId, UInt16 type);
        [DllImport("gts.dll")]
        public static extern short GTN_GetMailbox(short core, short station, UInt16 byteAddress, out UInt16 pData, UInt16 wordCount, UInt16 dataMode, UInt16 desId, UInt16 type);
        [DllImport("gts.dll")]
        public static extern short GTN_GetUuid(short core, string pCode, short count);
        
        [DllImport("gts.dll")]
        public static extern short GTN_GetTerminalPhyId(short core, short count, out short pPhyId);
        
    }
}
