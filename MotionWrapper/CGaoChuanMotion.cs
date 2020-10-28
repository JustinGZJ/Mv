using System;
using System.Collections.Generic;
using System.Text;
using MotionWrapper.BaseClass;
using MotionWrapper.Interface;
using GC.Frame.Motion.Privt;
using MotionWrapper.PubicData;
using System.Threading;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Xml;

namespace MotionWrapper
{
    public class CGCMotion : CMotionData, IMotionPart1, IMotionPart5, IIoPart1, IInitModel, IFreshModel,ISerialNumber
    {
        public bool initOk = false;
        private System.Threading.Thread runThread = null;       //用来刷新IO的
        byte[] devinfo1 = new byte[4 * 84];
        private const int usedAxisNum = 4;//使用的轴的个数
        private const int usedCrdNum = 1;//使用的坐标系个数
        private UInt16 devHandle = 0;//定义控制器句柄
        private UInt16[] axisHandle = new ushort[maxAxisNum];//轴的句柄
        private ushort[] crdHandle = new ushort[usedCrdNum];

        string regexFloat = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";
        string regexLong = "(\\d+)";
        public CGCMotion()
        {

        }
        void IFreshModel.Fresh()
        {
        }

        bool IIoPart1.getDi(CIoMap input)
        {
            int pos = input.adress;// CIoPrms.i[].dataindex;
            return mdis[pos];
        }

        bool IIoPart1.getDi(int startIndex, int lenth,ref bool[] value)
        {
            if (startIndex >=0 && startIndex < maxDiNum && (lenth+startIndex) < maxDiNum)
            {
                Array.Copy(mdis, startIndex, value, startIndex, lenth);
            }
            return true;
        }

        bool IIoPart1.getDo(CIoMap output)
        {
            int pos = output.adress;// CIoPrms.i[].dataindex;
            return mdos[pos];
        }

        bool IIoPart1.getDo(int startIndex, int lenth,ref bool[] value)
        {
            if (startIndex >= 0 && startIndex < maxDoNum && (lenth + startIndex) < maxDiNum)
            {
                Array.Copy(mdos, startIndex, value, startIndex, lenth);
            }
            return true;
        }

        bool IInitModel.Init()
        {
            short rtn = 0;
            ushort devnum = 0;
            byte[] devinfo = new byte[4 * 84];
            rtn += CNMCLib20.NMC_DevSearch(CNMCLib20.TSearchMode.Ethernet, ref devnum, devinfo);
            if (rtn == 0 && devnum > 0)
            {
                //仅打开第一个控制器
                rtn += CNMCLib20.NMC_DevOpen(0, ref devHandle);
                if (rtn != 0)
                {
                    System.Windows.Forms.MessageBox.Show("打开控制器失败");
                    initOk = false;
                }
                else
                {
                    byte[] path = System.Text.Encoding.Default.GetBytes(@"GCN400.CFG");
                    rtn += CNMCLib20.NMC_DevReset(devHandle);
                    rtn += CNMCLib20.NMC_LoadConfigFromFile(devHandle, path);
                    for (int i = 0; i < usedAxisNum; i++)
                    {
                        rtn += CNMCLib20.NMC_MtOpen(devHandle, (short)i, ref axisHandle[i]);
                        rtn += CNMCLib20.NMC_MtClrError(axisHandle[i]);
                        //rtn += CNMCLib20.NMC_MtSetSvOn(axisHandle[i]);
                    }
                }
                if (rtn == 0)
                {
                    IFreshModel fresh = this as IFreshModel;
                    runThread = new System.Threading.Thread(fresh.Run);
                    runThread.Priority = ThreadPriority.AboveNormal;
                    runThread.IsBackground = true;
                    runThread.Start();
                }
                initOk = rtn == 0;
            }
            else
            {
                initOk = false;
            }
            return initOk;
        }

        public int MC_AxisRef(ref AxisRef axisref)
        {
            int pos = axisref.prm.ID;
            axisref = axisRefs[pos];
            return 0;
        }

        public int MC_AxisRefAtOnce(ref AxisRef axisref)
        {

            int index = axisref.prm.ID;
            short rtn = 0;
            int prfpos = 0;
            int encpos = 0;
            //辅助编码器
            short sts = 0;
            rtn += CNMCLib20.NMC_MtGetSts(axisHandle[index], ref sts);
            rtn += CNMCLib20.NMC_MtGetPrfPos(axisHandle[index], ref prfpos);
            rtn += CNMCLib20.NMC_GetEncPos(axisHandle[index], (short)index, ref encpos);
            if (rtn == 0)
            {
                //解析
                axisref.alarm = (sts & 0x400) != 0;
                axisref.servoOn = (sts & 0x8) != 0;
                axisref.limitN = (sts & 0x80) != 0;
                axisref.limitN = (sts & 0x40) != 0;
                axisref.moving = (sts & 0x1) != 0;

                axisref.cmdPos = (float)axisref.prm.pls2mm(prfpos);
                axisref.relPos = (float)axisref.prm.pls2mm(encpos);
            }
            return 0;
        }

        int IMotionPart5.MC_Cam(string camlist)
        {
            throw new NotImplementedException();
        }

        int IMotionPart5.MC_Cam(AxisRef master, AxisRef slaver, double passpos, List<CCamData> data, bool relPasspos)
        {
            return 0;
        }

        int IMotionPart5.MC_CamStatus(AxisRef slaver, ref CCamStatus status)
        {
            return 0;
        }

        int IMotionPart5.MC_CaptureStatus(AxisRef enc, ref double capturePos)
        {
            short rtn = 0;
            short sts = 0;
            int tmpCptPos = int.MaxValue;
            rtn = CNMCLib20.NMC_MtGetSts(axisHandle[enc.prm.ID], ref sts);
            if (rtn ==0 && (0x00004000 & sts) != 0)
            {
                rtn = CNMCLib20.NMC_MtGetCaptPos(axisHandle[enc.prm.ID], ref tmpCptPos);
                if (rtn == 0)
                {
                    capturePos = enc.prm.pls2mm(tmpCptPos);
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 1;
            }

        }

        int IMotionPart5.MC_CrdCreate(int crdNum)
        {
            short rtn = 0;
            if (crdNum < 0 || crdNum >= AxisParameters.crdS.Count) return -1;
            CCrdPrm crdPrm = AxisParameters.crdS[crdNum];
            //创建坐标系
            rtn += CNMCLib20.NMC_CrdOpen(devHandle, ref crdHandle[crdPrm.crdNum-1]);
            if (rtn != 0) return -1;
            //参数基本
            CNMCLib20.TCrdConfig crd = new CNMCLib20.TCrdConfig(true);
            //rtn += CNMCLib20.NMC_CrdGetConfig(crdHandle[crdPrm.crdNum - 1], ref crd);
            crd.axCnts = (short)crdPrm.dimension;
            //初始化
            for (int i = 0; i < 4; i++)
            {
                crd.port[i] = 0;
                crd.pAxis[i] = -1;  
            }
            crd.pAxis[0] = (short)crdPrm.x;
            crd.pAxis[1] = (short)crdPrm.y;
            crd.pAxis[2] = (short)crdPrm.z;
            crd.pAxis[3] = (short)crdPrm.a;
            rtn += CNMCLib20.NMC_CrdConfig(crdHandle[crdPrm.crdNum - 1], ref crd);
            //安全
            CNMCLib20.TCrdSafePara safePrm = new CNMCLib20.TCrdSafePara();
            safePrm.maxVel = crdPrm.getCrdMaxVelPlsPerMs();
            safePrm.maxAcc = crdPrm.getCrdMaxAccPlsPerMs2();
            safePrm.estpDec = 1000;
            rtn += CNMCLib20.NMC_CrdSetSafePara(crdHandle[crdPrm.crdNum - 1], ref safePrm);
            //前瞻
            CNMCLib20.TExtCrdPara extPrm = new CNMCLib20.TExtCrdPara();
            rtn += CNMCLib20.NMC_CrdGetExtPara(crdHandle[crdPrm.crdNum - 1], ref extPrm);
            extPrm.startVel = 0;
            extPrm.smoothDec = crdPrm.getCrdMaxAccPlsPerMs2();
            extPrm.eventTime = 5;
            extPrm.abruptDec = 1000;
            extPrm.T = 1;
            rtn += CNMCLib20.NMC_CrdSetExtPara(crdHandle[crdPrm.crdNum - 1], ref extPrm);
            //清空
            rtn += CNMCLib20.NMC_CrdBufClr(crdHandle[crdPrm.crdNum - 1]);
            return rtn;
        }
        /// <summary>
        /// 获取带字母后面的数字
        /// </summary>
        /// <param name="key">XYZ</param>
        /// <param name="cmd">整个字符串</param>
        /// <returns></returns>
        private string getFloatOrLong(string key,string cmd)
        {
            //先测试浮点数
            string tmpStr = Regex.Match(cmd, "("+key + ")" + regexFloat).Value;
            if (tmpStr != "")
            {
                return Regex.Match(tmpStr, regexFloat).Value;
            }
            else
            {
                //测试LONG
                tmpStr = Regex.Match(cmd, "(" + key + ")" + regexLong).Value;
                if (tmpStr != "")
                {
                    return Regex.Match(tmpStr, regexLong).Value;
                }
                else
                {
                    return "";
                }
            }
        }
        public double targetX, targetY;//目标位置
        int IMotionPart5.MC_CrdData(string cmd, int crdNum)
        {
            double synVel = 100, synAcc = 1;
            int[] tpos = new int[3];
            float[] tmpPos = new float[3];
            int seg = 0;
            short rtn = 0;
            if (crdNum < 0 || crdNum >= AxisParameters.crdS.Count) return -1;
            CCrdPrm crdPrm = AxisParameters.crdS[crdNum];

            ushort crd = crdHandle[crdPrm.crdNum - 1];

            string tmpCmd = cmd.ToUpper();
            string tmpStr = "";


            rtn += CNMCLib20.NMC_CrdBufClr(crdHandle[crdPrm.crdNum - 1]);
            if (tmpCmd.IndexOf("G00") >= 0)
            {
                //X
                tmpStr = getFloatOrLong("X", tmpCmd);
                if (tmpStr != "")
                {
                    tmpPos[0] = float.Parse(tmpStr);
                    targetX = tmpPos[0];
                }
                else
                {
                    return -1;
                }
                //Y
                tmpStr = getFloatOrLong("Y", tmpCmd);
                if (tmpStr != "")
                {
                    tmpPos[1] = float.Parse(tmpStr);
                    targetY = tmpPos[1];
                }
                else
                {
                    return -1;
                }
                tmpPos[2] = 0.0f;

                //转换
                tpos[0] = crdPrm.mm2pls(tmpPos[0]);
                tpos[1] = crdPrm.mm2pls(tmpPos[1]); 
                tpos[2] = 0;
                rtn += CNMCLib20.NMC_CrdLineXYZ(crd, seg++, 0x7, tpos, 0, synVel, synAcc);
            }
            else if (tmpCmd.IndexOf("G01") >= 0)
            {
                //X
                tmpStr = getFloatOrLong("X", tmpCmd);
                if (tmpStr != "")
                {
                    tmpPos[0] = float.Parse(tmpStr);
                    targetX = tmpPos[0];
                }
                else
                {
                    return -1;
                }
                //Y
                tmpStr = getFloatOrLong("Y", tmpCmd);
                if (tmpStr != "")
                {
                    tmpPos[1] = float.Parse(tmpStr);
                    targetY = tmpPos[1];
                }
                else
                {
                    return -1;
                }
                tmpPos[2] = 0.0f;
                //转换
                tpos[0] = crdPrm.mm2pls(tmpPos[0]);
                tpos[1] = crdPrm.mm2pls(tmpPos[1]);
                tpos[2] = 0;
                rtn += CNMCLib20.NMC_CrdLineXYZ(crd, seg++, 0x7, tpos, 0, synVel, synAcc);
            }

            rtn = CNMCLib20.NMC_CrdEndMtn(crd);
            return rtn;
        }

        int IMotionPart5.MC_CrdData(List<string> gcode, int crdNum)
        {
            double synVel = 0, synAcc = 0;
            ushort crd = crdHandle[crdNum - 1];
            int[] tpos = new int[3];
            int seg = 0;
            short rtn = 0;
            foreach (var item in gcode)
            {
                string tmpCmd = item.ToUpper();
                if (tmpCmd.IndexOf("GOO") >=0)
                {
                    tpos[0] = 0;
                    tpos[1] = 0;
                    tpos[2] = 0;
                    rtn += CNMCLib20.NMC_CrdLineXYZ(crd,seg++,0x7,tpos,0,synVel,synAcc);
                }
                else if (tmpCmd.IndexOf("GO1") >= 0)
                {
                    tpos[0] = 0;
                    tpos[1] = 0;
                    tpos[2] = 0;
                    rtn += CNMCLib20.NMC_CrdLineXYZ(crd, seg++, 0x7, tpos, 0, synVel, synAcc);
                }
                //CNMCLib20.NMC_CrdLineXYZ
            }
            rtn = CNMCLib20.NMC_CrdEndMtn(crd);
            return rtn;
        }

        int IMotionPart5.MC_CrdStart(int crdNum)
        {
            CCrdPrm crdPrm = AxisParameters.crdS[crdNum];
            ushort crd = crdHandle[crdPrm.crdNum - 1];
            short rtn = CNMCLib20.NMC_CrdStartMtn(crd);
            return rtn;
        }

        int IMotionPart5.MC_CrdStatus(int crdNum)
        {
            ushort crd = crdHandle[crdNum - 1];
            short crdSts = 0;
            short rtn = CNMCLib20.NMC_CrdGetSts(crd, ref crdSts);
            if ((crdSts & CNMCLib20.BIT_CORD_BUSY) == 0)
            {
                return 0; // 运动完成
            }
            else
            {
                return 1;
            }
        }

        int IMotionPart1.MC_EStop(AxisRef axis)
        {
            if (axis.prm.ID >=0 && axis.prm.ID < axisHandle.Length)
            {
                short rtn = CNMCLib20.NMC_MtAbruptStop(axisHandle[axis.prm.ID]);
                return rtn;
            }
            return -1;
        }

        int IMotionPart1.MC_Home(ref AxisRef axis)
        {
            if (!axisNumIsOk(axis)) return -1;
            short rtn = 0;
            CNMCLib20.THomeSetting prm = new CNMCLib20.THomeSetting();
            rtn = CNMCLib20.NMC_MtGetHomePara(axisHandle[axis.prm.ID], ref prm);
            axis.isHoming = false;//初始化
            prm.mode = axis.prm.homeType;
            prm.acc = axis.prm.getAccPlsPerMs2();
            if (axis.prm.homeSearch >= 0)
            {
                prm.dir = 1;
            }
            else
            {
                prm.dir = 0;//注意这里可能说明书有错
            }
            prm.homeEdge = 0;
            prm.lmtEdge = 0;
            prm.zEdge = 0;
            prm.reScanEn = 0;
            prm.offset = (int)axis.prm.mm2pls(axis.prm.homeSearch);
            prm.scan1stVel = axis.prm.mmpers2plsperms(axis.prm.homeVel1);
            prm.scan2ndVel = 0.5 * prm.scan1stVel;
            prm.safeLen = Math.Abs((int)axis.prm.mm2pls(axis.prm.homeSearch));
            prm.iniRetPos = 0;
            prm.usePreSetPtpPara = 0;
            prm.retSwOffset = (int)axis.prm.mm2pls(axis.prm.homeLeave);
            rtn += CNMCLib20.NMC_MtHome(axisHandle[axis.prm.ID]);
            if (rtn == 0)
            {
                axis.isHoming = true;
                axis.homed = 1;//初始化
            }
            else
            {
                axis.isHoming = false;
                axis.homed = 1;//初始化
                CNMCLib20.NMC_MtHomeStop(axisHandle[axis.prm.ID]);
            }
            return rtn;
        }

        public int MC_MoveAbs(AxisRef axis, double tpos, double beilv)
        {
            if (!axisNumIsOk(axis)) return -1;
            short rtn = 0;
            short pMode = 0;
            //step1--设置模式
            rtn += CNMCLib20.NMC_MtGetPrfMode(axisHandle[axis.prm.ID], ref pMode);
            if (pMode != CNMCLib20.MT_PTP_PRF_MODE)
            {
                MC_AxisRef(ref axis);
                if (!axis.moving)
                {
                    rtn += CNMCLib20.NMC_MtSetPrfMode(axisHandle[axis.prm.ID], CNMCLib20.MT_PTP_PRF_MODE);
                }
                else
                {
                    return -1;
                }
            }
            if (rtn == 0)
            {
                MC_AxisRef(ref axis);
                if (!axis.moving)
                {
                    //step2--设置参数
                    CNMCLib20.TPtpPara ptpPara = new CNMCLib20.TPtpPara();
                    ptpPara.acc = axis.prm.getAccPlsPerMs2();
                    ptpPara.dec = ptpPara.acc;
                    ptpPara.smoothCoef = 100;
                    ptpPara.startVel = 0;
                    ptpPara.endVel = 0;
                    ptpPara.dummy1 = 0;
                    ptpPara.dummy2 = 0;
                    ptpPara.dummy3 = 0;
                    rtn += CNMCLib20.NMC_MtSetPtpPara(axisHandle[axis.prm.ID], ref ptpPara);
                    if (rtn != 0) return -3;

                    rtn += CNMCLib20.NMC_MtSetVel(axisHandle[axis.prm.ID], axis.prm.getVelPlsPerMs() * beilv);
                    rtn += CNMCLib20.NMC_MtSetPtpTgtPos(axisHandle[axis.prm.ID], (int)axis.prm.mm2pls(tpos));
                    rtn += CNMCLib20.NMC_MtUpdate(axisHandle[axis.prm.ID]);
                    return rtn;
                }
                else
                {
                    rtn += CNMCLib20.NMC_MtSetVel(axisHandle[axis.prm.ID], axis.prm.getVelPlsPerMs() * beilv);
                    rtn += CNMCLib20.NMC_MtSetPtpTgtPos(axisHandle[axis.prm.ID], (int)axis.prm.mm2pls(tpos));
                    rtn += CNMCLib20.NMC_MtUpdate(axisHandle[axis.prm.ID]);
                    return rtn;
                }
            }
            else
            {
                return -2;
            }
        }

        public int MC_MoveAdd(AxisRef axis, double dist, double beilv)
        {
            if (!axisNumIsOk(axis)) return -1;
            short rtn = 0;
            short pMode = 0;
            //step1--设置模式
            rtn += CNMCLib20.NMC_MtGetPrfMode(axisHandle[axis.prm.ID], ref pMode);
            if (pMode != CNMCLib20.MT_PTP_PRF_MODE)
            {
                MC_AxisRef(ref axis);
                if (!axis.moving)
                {
                    rtn += CNMCLib20.NMC_MtSetPrfMode(axisHandle[axis.prm.ID], CNMCLib20.MT_PTP_PRF_MODE);
                }
                else
                {
                    return -1;
                }
            }
            if (rtn == 0)
            {
                MC_AxisRef(ref axis);
                if (!axis.moving)
                {
                    //step2--设置参数
                    CNMCLib20.TPtpPara ptpPara = new CNMCLib20.TPtpPara();
                    ptpPara.acc = axis.prm.getAccPlsPerMs2();
                    ptpPara.dec = axis.prm.getAccPlsPerMs2();
                    ptpPara.smoothCoef = 100;
                    ptpPara.startVel = 0;
                    ptpPara.endVel = 0;
                    ptpPara.dummy1 = 0;
                    ptpPara.dummy2 = 0;
                    ptpPara.dummy3 = 0;
                    rtn += CNMCLib20.NMC_MtSetPtpPara(axisHandle[axis.prm.ID], ref ptpPara);
                    if (rtn != 0) return -3;

                    double tpos = axis.cmdPos + dist;
                    rtn += CNMCLib20.NMC_MtSetVel(axisHandle[axis.prm.ID], axis.prm.getVelPlsPerMs() * beilv);
                    rtn += CNMCLib20.NMC_MtSetPtpTgtPos(axisHandle[axis.prm.ID], (int)axis.prm.mm2pls(tpos));
                    rtn += CNMCLib20.NMC_MtUpdate(axisHandle[axis.prm.ID]);
                    return rtn;
                }
                else
                {
                    return -3;
                }
            }
            else
            {
                return -2;
            }
        }

        public int MC_MoveJog(AxisRef axis, double beilv)
        {
            if (!axisNumIsOk(axis)) return -1;
            short rtn = 0;
            short pMode = 0;
            //step1--设置模式
            rtn += CNMCLib20.NMC_MtGetPrfMode(axisHandle[axis.prm.ID], ref pMode);
            if (pMode != CNMCLib20.MT_JOG_PRF_MODE)
            {
                MC_AxisRef(ref axis);
                if (!axis.moving)
                {
                    rtn += CNMCLib20.NMC_MtSetPrfMode(axisHandle[axis.prm.ID], CNMCLib20.MT_JOG_PRF_MODE);
                }
                else
                {
                    return -1;
                }
            }
            if (rtn == 0)
            {
                MC_AxisRef(ref axis);
                if (!axis.moving)
                {
                    //step2--设置参数
                    CNMCLib20.TJogPara jogPara = new CNMCLib20.TJogPara();
                    jogPara.acc = axis.prm.getAccPlsPerMs2();
                    jogPara.dec = axis.prm.getAccPlsPerMs2();
                    jogPara.smoothCoef = 100;
                    rtn += CNMCLib20.NMC_MtSetJogPara(axisHandle[axis.prm.ID], ref jogPara);
                    if (rtn != 0) return -3;

                    rtn += CNMCLib20.NMC_MtSetVel(axisHandle[axis.prm.ID], axis.prm.getVelPlsPerMs() * beilv);
                    rtn += CNMCLib20.NMC_MtUpdate(axisHandle[axis.prm.ID]);
                    return rtn;
                }
                else
                {
                    rtn += CNMCLib20.NMC_MtSetVel(axisHandle[axis.prm.ID], axis.prm.getVelPlsPerMs() * beilv);
                    rtn += CNMCLib20.NMC_MtUpdate(axisHandle[axis.prm.ID]);
                    return rtn;
                }
            }
            else
            {
                return -2;
            }
        }

        public int MC_Power(AxisRef axis)
        {
            if (!axisNumIsOk(axis)) return -1;
            short rtn = 0;
            rtn = CNMCLib20.NMC_MtSetSvOn(axisHandle[axis.prm.ID]);
            return rtn;
        }

        int IMotionPart1.MC_PowerOff(AxisRef axis)
        {
            if (!axisNumIsOk(axis)) return -1;
            short rtn = 0;
            rtn = CNMCLib20.NMC_MtSetSvOff(axisHandle[axis.prm.ID]);
            return rtn;
        }
        private bool axisNumIsOk(AxisRef axis)
        {
            return (axis.prm.ID >= 0 && axis.prm.ID < axisHandle.Length);
        }
        int IMotionPart1.MC_Reset(AxisRef axis)
        {
            if(axisNumIsOk(axis))
            {
                short rtn = CNMCLib20.NMC_MtClrError(axisHandle[axis.prm.ID]);
                return rtn;
            }
            return -1;
        }

        int IMotionPart1.MC_SetPos(AxisRef axis, double pos)
        {
            if (axisNumIsOk(axis))
            {
                int posPls = (int)axis.prm.mm2pls(pos);
                short rtn = CNMCLib20.NMC_MtSetAxisPos(axisHandle[axis.prm.ID], posPls);
                rtn += CNMCLib20.NMC_MtSetEncPos(axisHandle[axis.prm.ID], posPls);
                return 0;
            }
            return -1;
            
        }
        //编码器硬件捕获模式---这里只是用IO捕获
        //CAPT_MODE_Z(宏定义 0) 编码器 Z 相捕获
        //CAPT_MODE_IO(宏定义 1)IO 捕获
        //CAPT_MODE_Z_AND_IO(宏定义 2)IO+Z 相捕获
        //CAPT_MODE_Z_AFT_IO(宏定义 3)先 IO 触发再 Z 相触发捕获
        int IMotionPart5.MC_StartCapture(AxisRef enc, CCapturePrm capturePrm)
        {
            if (!axisNumIsOk(enc)) return -1;
            short rtn = 0;
            CNMCLib20.NMC_MtClrCaptSts(axisHandle[enc.prm.ID]);
            rtn += CNMCLib20.NMC_MtSetCaptSns(axisHandle[enc.prm.ID], (short)capturePrm.type, (short)capturePrm.index, 0);
            rtn += CNMCLib20.NMC_MtSetCapt(axisHandle[enc.prm.ID]);
            return rtn;
        }

        int IMotionPart5.preMC_CamModel(AxisRef master, AxisRef slaver)
        {
            return 0;
        }

        void IFreshModel.Run()
        {
            IFreshModel fresh = this as IFreshModel;
            while (true)
            {
                fresh.Fresh();
                Thread.Sleep(1);
            }
        }

        void IIoPart1.setDO(CIoMap output, bool value)
        {
            int index = CIoPrms.o[output.adress].index;
            if (value)
            {
                CNMCLib20.NMC_SetDOBit(devHandle,(short)index,0);
            }
            else
            {
                CNMCLib20.NMC_SetDOBit(devHandle, (short)index, 1);
            }
        }

        bool IInitModel.UnInit()
        {
            try
            {
                //停止刷新
                if (runThread != null && runThread.IsAlive)
                {
                    runThread.Abort();
                    Thread.Sleep(5);
                    runThread = null;
                }
                short rtn = 0;
                if (devHandle != 0)
                {
                    for (int i = 0; i < usedAxisNum; i++)
                    {
                        rtn += CNMCLib20.NMC_MtClose(ref axisHandle[i]);
                    }
                    rtn += CNMCLib20.NMC_DevReset(devHandle);
                    rtn += CNMCLib20.NMC_DevClose(ref devHandle);
                }
                return rtn == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        int IMotionPart1.MC_AxisRef(int startIndex, int lenth, ref AxisRef[] axisS)
        {
            if (startIndex >=0 && (startIndex + lenth) < axisRefs.Length)
            {
                Array.Copy(axisRefs, 0, axisS, 0, lenth);//内存复制
                return 0;
            }
            else
            {
                return -1;
            }
        }

        string ISerialNumber.MC_GetSn(string machSN, int days,bool type)
        {
            if (type)
            {
                string uid = machSN;
                //获取天数
                DateTime dt1 = Convert.ToDateTime("2020-1-1");
                DateTime dt2 = DateTime.Now;
                int dayCount = Convert.ToInt32((dt2 - dt1).TotalDays) - 1 + days;

                uid += dayCount.ToString();

                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(uid)), 4, 8);
                t2 = t2.Replace("-", "");
                return t2.ToUpper();
            }
            else
            {
                string uid = machSN;

                int dayCount = days;

                uid += dayCount.ToString();

                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(uid)), 4, 8);
                t2 = t2.Replace("-", "");
                return t2.ToUpper();
            }
        }
        bool ISerialNumber.MC_CanRun(string userSn,string machSN, ref int haveDays)
        {
            int canUsedDays = -1;
            ISerialNumber app = this as ISerialNumber;
            haveDays = 0;
            for (int i = 0; i < 99999; i++)
            {
                if (app.MC_GetSn(machSN, i,false) == userSn)
                {
                    canUsedDays = i;
                    break;
                }
            }
            if (canUsedDays < 0) return false;
            if (canUsedDays == 0) return true;
            if (canUsedDays > 0)
            {
                //获取天数
                DateTime dt1 = Convert.ToDateTime("2020-1-1");
                DateTime dt2 = DateTime.Now;
                int dayCount = Convert.ToInt32((dt2 - dt1).TotalDays) - 1;
                haveDays = Math.Abs(dayCount - canUsedDays);
                if (dayCount <= canUsedDays) return true;
                else return false;
            }
            return false;
        }

        string ISerialNumber.MC_GetMachSN()
        {
            CNMCLib20.TDevResourceInfo info = new CNMCLib20.TDevResourceInfo();
            CNMCLib20.NMC_GetCardInfo(devHandle, ref info);
            string uid = System.Text.Encoding.ASCII.GetString(info.uid);

            uid += "TDYDIY";
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(uid)), 4, 8);
            t2 = t2.Replace("-", "");
            string machSN =  t2.ToUpper();
            return machSN;
        }
        void ISerialNumber.MC_SaveSnFile(SerialData data, string xml)
        {
            try
            {
                //文档
                XmlWriterSettings set = new XmlWriterSettings();
                set.Indent = true;
                set.NewLineOnAttributes = false;
                XmlWriter writer = XmlWriter.Create(xml, set);
                //master
                writer.WriteStartDocument();
                writer.WriteStartElement("SN");
                writer.WriteElementString("MachSN", data.macSN);
                writer.WriteElementString("AuthorizeSN", data.sn);
                writer.WriteElementString("StartTime", data.startTime);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            catch (Exception)
            {
                
            }
        }
        SerialData ISerialNumber.MC_ReadSnFile(string xml)
        {
            SerialData tmpdata = new SerialData();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xml);
                //解析XML文件
                if (doc != null)
                {
                    XmlElement root = doc.DocumentElement;
                    if (root != null && root.Name == "SN")
                    {
                        //内参读取
                        XmlNode snNode = root.SelectSingleNode("/SN/AuthorizeSN");
                        if (snNode != null)
                        {
                            tmpdata.sn = snNode.InnerText;
                        }
                        XmlNode snNode2 = root.SelectSingleNode("/SN/MachSN");
                        if (snNode2 != null)
                        {
                            tmpdata.macSN = snNode2.InnerText;
                        }
                        XmlNode snNode3 = root.SelectSingleNode("/SN/StartTime");
                        if (snNode3 != null)
                        {
                            tmpdata.startTime = snNode3.InnerText;
                        }
                    }
                }
                return tmpdata;
            }
            catch (Exception)
            {
                return tmpdata;
            }
        }

        int IMotionPart5.MC_CrdSetOrigin(int crdNum, List<double> origionData)
        {
            if (crdNum < 0 || crdNum >= AxisParameters.crdS.Count) return -1;
            CCrdPrm crdPrm = AxisParameters.crdS[crdNum];
            for (int i = 0; i < origionData.Count; i++)
            {
                crdPrm.orgData[i] = origionData[i];
            }
            return 0;
        }

        public int MC_CrdOverride(int crdNum, float BeiLv)
        {
            short rtn = CNMCLib20.NMC_CrdSetOverRide(crdHandle[crdNum], BeiLv);
            return rtn;
        }

        public int MC_HomeStatus(ref AxisRef axis)
        {
            if (!axisNumIsOk(axis)) return -1;
            if (axis.isHoming)
            {
                short homeSts = 0;
                CNMCLib20.NMC_MtGetHomeSts(axisHandle[axis.prm.ID], ref homeSts);
                if ((homeSts & CNMCLib20.BIT_AXHOME_OK) != 0)//回零完成
                {
                    axis.homed = 0;
                    axis.isHoming = false;
                }
                else if (homeSts == 0)//进行中
                {

                }
                else if (((homeSts & CNMCLib20.BIT_AXHOME_FAIL) != 0) || ((homeSts & CNMCLib20.BIT_AXHOME_ERR_MV) != 0) || ((homeSts & CNMCLib20.BIT_AXHOME_ERR_SWT) != 0))//回零失败
                {
                    axis.homed = -1;
                    axis.isHoming = false;
                }
            }
            return 0;
        }
    }
}
