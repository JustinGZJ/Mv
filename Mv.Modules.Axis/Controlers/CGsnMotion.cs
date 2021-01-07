using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTN;
namespace MotionWrapper
{
    public class CGsnMotion : CMotionData, IMotionPart1, IMotionPart5, IIoPart1, IInitModel, IFreshModel
    {
        public void Fresh()
        {
           
        }
        /// <summary>
        /// 启动正负限位
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="lmtP"></param>
        /// <param name="lmtN"></param>
        /// <returns></returns>
        public bool setAxisLimit(AxisRef axis,bool lmtP,bool lmtN)
        {
            if (lmtP && lmtN)//两个都打开
            {
                mc.GTN_LmtsOn(axis.prm.cardNum, axis.prm.axisNum, -1);
            }
            else if (!lmtP && !lmtN)//两个都关闭
            {
                mc.GTN_LmtsOff(axis.prm.cardNum, axis.prm.axisNum, -1);
            }
            else 
            {
                if (lmtP)//+限位有效
                {
                    mc.GTN_LmtsOn(axis.prm.cardNum, axis.prm.axisNum, 0);
                }
                else
                {
                    mc.GTN_LmtsOff(axis.prm.cardNum, axis.prm.axisNum, 0);
                }
                if (lmtN)//负限位有效
                {
                    mc.GTN_LmtsOn(axis.prm.cardNum, axis.prm.axisNum, 1);
                }
                else
                {
                    mc.GTN_LmtsOff(axis.prm.cardNum, axis.prm.axisNum, 1);
                }
            }
            return true;
        }
        public long getTime()
        {
            uint clock = 0;
            uint clock2 = 0;
            short rtn = mc.GTN_GetClock(1, out clock,out clock2);
            if (rtn ==0)
            {
                return (long)clock;
            }
            else
            {
                return 0;
            }
        }
        public bool getDi(int startIndex, int lenth, ref bool[] value)
        {
            return true;
        }

        public bool getDi(IoRef input)
        {
            int value = 0;
            if (input.prm.ioType == EIoType.NoamlInput)
            {
                short rtn = mc.GTN_GetDi(input.prm.model, mc.MC_GPI, out value);
                return (value & (1 << (input.prm.index - 1))) == 0;
            }
            else if (input.prm.ioType == EIoType.LimitP)
            {
                short rtn = mc.GTN_GetDi(input.prm.model, mc.MC_LIMIT_POSITIVE, out value);
                return (value & (1 << (input.prm.index - 1))) == 0;
            }
            else if (input.prm.ioType == EIoType.LimitN)
            {
                short rtn = mc.GTN_GetDi(input.prm.model, mc.MC_LIMIT_NEGATIVE, out value);
                return (value & (1 << (input.prm.index - 1))) == 0;
            }
            return false;
        }

        public bool getDiCounter(IoRef input, ref long counter)
        {
            int value = 0;
            uint ct = 0;
            uint sct = 1;
            short rtn = mc.GTN_GetDi(input.prm.model, mc.MC_GPI, out value);
            //rtn = mc.GTN_SetDiReverseCount(input.prm.model, mc.MC_GPI, input.prm.index, ref sct, 1);
            rtn = mc.GTN_GetDiReverseCount(input.prm.model, mc.MC_GPI, input.prm.index, out ct, 1);
            counter = ct;
            return (value & (1 << (input.prm.index - 1))) == 0;
        }

        public bool getDo(int startIndex, int lenth, ref bool[] value)
        {
            return false;
        }

        public bool getDo(IoRef output)
        {
            int value = 0;
            short rtn = mc.GTN_GetDo(output.prm.model, mc.MC_GPO, out value);
            return (value & (1 << output.prm.index-1)) == 0;
        }
        /// <summary>
        /// 使用固定文件名 核心1使用
        /// 启动的时候需要调用
        /// 配置文件有使用相对路径 confige/gtn_core1.cfg和 confige/gtn_core2.cfg
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            short rtn = mc.GTN_Open(0, 0);
            if (rtn == 0)
            {
                rtn += mc.GTN_Reset(1);
                rtn += mc.GTN_Reset(2);
                rtn += mc.GTN_LoadConfig(1, @"confige/gtn_core1.cfg");
                rtn += mc.GTN_LoadConfig(2, @"confige/gtn_core2.cfg");
                rtn += mc.GTN_ClrSts(1, 1, 12);
                rtn += mc.GTN_ClrSts(2, 1, 12);
            }
            return rtn == 0;
        }

        public int MC_AxisRef(ref AxisRef axisref)
        {
            int sts = 0;
            uint clock = 0;
            double prfpos, encpos, encvel;
            int homeswitch = 0;
            short rtn = mc.GTN_GetSts(axisref.prm.cardNum, axisref.prm.axisNum, out sts, 1, out clock);
            rtn += mc.GTN_GetPrfPos(axisref.prm.cardNum, axisref.prm.axisNum, out prfpos, 1, out clock);
            rtn += mc.GTN_GetEncPos(axisref.prm.cardNum, axisref.prm.axisNum, out encpos, 1, out clock);
            rtn += mc.GTN_GetEncVel(axisref.prm.cardNum, axisref.prm.axisNum, out encvel, 1, out clock);
            rtn += mc.GTN_GetDi(axisref.prm.cardNum, mc.MC_HOME,out homeswitch);
            axisref.homeSwitch = ((1 << (axisref.prm.axisNum - 1)) & homeswitch) == 0;
            axisref.alarm = (sts & 0x2) != 0;
            axisref.servoOn = (sts & 0x200) != 0;
            axisref.limitN = (sts & 0x40) != 0;
            axisref.limitP = (sts & 0x20) != 0;
            axisref.moving = (sts & 0x400) != 0;

            axisref.cmdPos = (float)axisref.prm.pls2mm((long)prfpos);
            axisref.relPos = (float)axisref.prm.pls2mm((long)encpos);
            axisref.relVel = (float)axisref.prm.plsperms2mmpers(encvel);
            return rtn;
        }

        public int MC_AxisRef(int startIndex, int lenth, ref AxisRef[] axisS)
        {
            return 0;
        }

        public int MC_Cam(string camlist)
        {
            return 0;
        }

        public int MC_Cam(AxisRef master, AxisRef slaver, double passpos, List<CCamData> data, bool relPasspos = false)
        {
            short rtn = 0;
            uint clock = 0;
            //判断是否是跟随模式
            foreach (var item in data)
            {
                rtn += mc.GTN_FollowData(slaver.prm.cardNum, slaver.prm.axisNum, (int)item.master, item.slaver, (short)item.moveType, 0);
            }
            if (passpos != 0)
            {
                if (relPasspos)
                {
                    double value = 0;
                    rtn += mc.GTN_GetEncPos(master.prm.cardNum, master.prm.axisNum, out value, 1, out clock);
                    rtn += mc.GTN_SetFollowEvent(slaver.prm.cardNum, slaver.prm.axisNum, mc.FOLLOW_EVENT_PASS, 1, (int)(passpos + value));
                }
                else
                {
                    rtn += mc.GTN_SetFollowEvent(slaver.prm.cardNum, slaver.prm.axisNum, mc.FOLLOW_EVENT_PASS, 1, (int)passpos);
                }
            }
            else
            {
                rtn += mc.GTN_SetFollowEvent(slaver.prm.cardNum, slaver.prm.axisNum, mc.FOLLOW_EVENT_START, 1, (int)passpos);
            }
            rtn += mc.GTN_FollowStart(slaver.prm.cardNum, 1 << (slaver.prm.axisNum - 1), 0);
            return rtn;
        }

        public int MC_CamStatus(AxisRef slaver, ref CCamStatus status)
        {
            return 0;
        }

        public int MC_CaptureStatus(AxisRef enc, ref double capturePos)
        {
            mc.TTriggerStatusEx trigerSts = new mc.TTriggerStatusEx();
            short rtn = mc.GTN_GetTriggerStatusEx(enc.prm.cardNum, enc.prm.axisNum, out trigerSts, 1);
            if (rtn == 0 && trigerSts.done == 1)
            {
                capturePos = trigerSts.position;
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public int MC_Compare(AxisRef axis, double posMM, IoRef cmpInput)
        {
            mc.TPosCompareMode mode = new mc.TPosCompareMode();
            short rtn = mc.GTN_GetPosCompareMode(1, 1, out mode); //读取位置比较输出模式
            mode.dimension = 1; //一维
            mode.mode = 0; //FIFO模式
            mode.outputMode = 0; // 0-脉冲，1-电平，2-电平自动翻转
            mode.outputPulseWidth = 10; //设置脉冲宽度
            mode.sourceMode = 1; //0-编码器，1-冲计数器
            mode.sourceX = axis.prm.axisNum; //x轴对应的实际轴为轴1
            rtn += mc.GTN_SetPosCompareMode(axis.prm.cardNum,1,ref mode); //设置位置比较输出模式
            throw new NotImplementedException();
        }

        public int MC_CrdCreate(int crdNum)
        {
            return 0;
        }

        public int MC_CrdData(string cmd, int crdNum = 0)
        {
            return 0;
        }

        public int MC_CrdData(List<string> gcode, int crdNum = 0)
        {
            return 0;
        }

        public int MC_CrdOverride(int crdNum, float BeiLv)
        {
            return 0;
        }

        public int MC_CrdSetOrigin(int crdNum, List<double> origionData)
        {
            return 0;
        }

        public int MC_CrdStart(int cardNum = 0)
        {
            return 0;
        }

        public int MC_CrdStatus(int crdNum = 0)
        {
            return 0;
        }

        public int MC_EStop(AxisRef axis)
        {
            axis.isHoming = false;
            mc.GTN_Stop(axis.prm.cardNum, 1 << (axis.prm.axisNum - 1), 1 << (axis.prm.axisNum - 1));
            return 0;
        }

        public int MC_Home(ref AxisRef axis)
        {
            mc.THomePrm prm = new mc.THomePrm();
            axis.homed = 0;
            axis.isHoming = false;
            short rtn = mc.GTN_GetHomePrm(axis.prm.cardNum, axis.prm.axisNum, out prm);
            rtn = mc.GTN_ZeroPos(axis.prm.cardNum, axis.prm.axisNum, 1);
            rtn = mc.GTN_ClrSts(axis.prm.cardNum, axis.prm.axisNum, 1);
            prm.mode = axis.prm.homeType;
            prm.acc = axis.prm.getAccPlsPerMs2();
            prm.dec = prm.acc;
            if (axis.prm.homeSearch >= 0)
            {
                prm.moveDir = 1;
            }
            else
            {
                prm.moveDir = -1;
            }
            prm.indexDir = 1;
            prm.edge = (short)axis.prm.homeTriger;
            prm.homeOffset = (int)axis.prm.mm2pls(axis.prm.homeoffset);
            prm.velHigh = axis.prm.mmpers2plsperms(axis.prm.homeVel1);
            prm.velLow = axis.prm.mmpers2plsperms(axis.prm.homeVel2);
            prm.searchHomeDistance = Math.Abs((int)axis.prm.mm2pls(axis.prm.homeSearch));
            prm.searchIndexDistance = prm.searchHomeDistance;
            prm.escapeStep = (int)axis.prm.mm2pls(axis.prm.homeLeave);
            rtn += mc.GTN_GoHome(axis.prm.cardNum, axis.prm.axisNum, ref prm);
            //int sts1;
            //uint clock;
            //mc.GTN_GetSts(axis.prm.cardNum, axis.prm.axisNum, out sts1, 1,out clock);
            if (rtn == 0) axis.isHoming = true;
            else axis.isHoming = false;
            return rtn;
        }

        public int MC_HomeStatus(ref AxisRef axis)
        {
            short rtn = 0;
            mc.THomeStatus tmpHomeSts = new mc.THomeStatus();
            rtn = mc.GTN_GetHomeStatus(axis.prm.cardNum, axis.prm.axisNum, out tmpHomeSts);
            if (tmpHomeSts.run == 0 && tmpHomeSts.error == mc.HOME_ERROR_NONE)//回零完成
            {
                axis.homed = 1;
                mc.GTN_ZeroPos(axis.prm.cardNum, axis.prm.axisNum, 1);
                axis.isHoming = false;
            }
            if (tmpHomeSts.run == 0 && tmpHomeSts.error != mc.HOME_ERROR_NONE)//回零完成
            {
                axis.homed = -1;
                axis.isHoming = false;
            }
            return rtn;
        }

        public int MC_MoveAbs(AxisRef axis, double tpos, double beilv = 0.5)
        {
            short rtn = 0;
            int pmode;
            int psts;
            uint pclock;
            rtn += mc.GTN_GetPrfMode(axis.prm.cardNum, axis.prm.axisNum, out pmode, 1, out pclock);
            rtn += mc.GTN_GetSts(axis.prm.cardNum, axis.prm.axisNum, out psts, 1, out pclock);
            if ((psts & 0x400) == 0)
            {
                if (pmode != 0)
                {
                    rtn += mc.GTN_PrfTrap(axis.prm.cardNum, axis.prm.axisNum);
                }
                mc.TTrapPrm trapprm = new mc.TTrapPrm();
                trapprm.acc = axis.prm.getAccPlsPerMs2();
                trapprm.dec = axis.prm.getAccPlsPerMs2();
                trapprm.smoothTime = 50;
                trapprm.velStart = 0;
                rtn += mc.GTN_SetTrapPrm(axis.prm.cardNum, axis.prm.axisNum, ref trapprm);
                rtn += mc.GTN_SetVel(axis.prm.cardNum, axis.prm.axisNum, axis.prm.getVelPlsPerMs() * beilv);
                rtn += mc.GTN_SetPos(axis.prm.cardNum, axis.prm.axisNum, (int)(axis.prm.mm2pls(tpos)));
                //int sts = 0;
                //uint clock = 0;
                //rtn = mc.GT_GetSts(cardnum, relAxis, out sts, 1, out clock);
                rtn += mc.GTN_Update(axis.prm.cardNum, 1 << (axis.prm.axisNum - 1));
            }
            else if (pmode == 0)
            {
                rtn += mc.GTN_SetVel(axis.prm.cardNum, axis.prm.axisNum, axis.prm.getVelPlsPerMs() * beilv);
                rtn += mc.GTN_SetPos(axis.prm.cardNum, axis.prm.axisNum, (int)(axis.prm.mm2pls(tpos)));
                rtn += mc.GTN_Update(axis.prm.cardNum, 1 << (axis.prm.axisNum - 1));
            }
            return rtn;
        }

        public int MC_MoveAdd(AxisRef axis, double dist, double beilv = 0.5)
        {
            short rtn = 0;
            int pmode,psts;
            uint pclock;
            double prfpos;
            rtn += mc.GTN_GetSts(axis.prm.cardNum, axis.prm.axisNum, out psts, 1, out pclock);
            if ((psts & 0x400) == 0)
            {
                rtn += mc.GTN_GetPrfMode(axis.prm.cardNum, axis.prm.axisNum, out pmode, 1, out pclock);
                if (pmode != 0)
                {
                    rtn += mc.GTN_PrfTrap(axis.prm.cardNum, axis.prm.axisNum);
                }
                mc.TTrapPrm trapprm = new mc.TTrapPrm();
                trapprm.acc = axis.prm.getAccPlsPerMs2();
                trapprm.dec = axis.prm.getAccPlsPerMs2();
                trapprm.smoothTime = 50;
                trapprm.velStart = 0;
                rtn += mc.GTN_SetTrapPrm(axis.prm.cardNum, axis.prm.axisNum, ref trapprm);
                rtn += mc.GTN_SetVel(axis.prm.cardNum, axis.prm.axisNum, axis.prm.getVelPlsPerMs() * beilv);
                rtn += mc.GTN_GetPrfPos(axis.prm.cardNum, axis.prm.axisNum, out prfpos, 1, out pclock);
                rtn += mc.GTN_SetPos(axis.prm.cardNum, axis.prm.axisNum, (int)(axis.prm.mm2pls(dist) + prfpos));
                rtn += mc.GTN_Update(axis.prm.cardNum, 1 << (axis.prm.axisNum - 1));
            }
            else
            {
                rtn += mc.GTN_GetPrfMode(axis.prm.cardNum, axis.prm.axisNum, out pmode, 1, out pclock);
                if (pmode == 0)
                {
                    rtn += mc.GTN_GetPrfPos(axis.prm.cardNum, axis.prm.axisNum, out prfpos, 1, out pclock);
                    rtn += mc.GTN_SetPos(axis.prm.cardNum, axis.prm.axisNum, (int)(axis.prm.mm2pls(dist) + prfpos));
                    rtn += mc.GTN_Update(axis.prm.cardNum, 1 << (axis.prm.axisNum - 1));
                }
            }
            return rtn;
        }

        public int MC_MoveJog(AxisRef axis, double beilv = 0.5)
        {
            try
            {
                short rtn = 0;
                int pmode;
                int psts;
                uint pclock;
                rtn += mc.GTN_GetPrfMode(axis.prm.cardNum, axis.prm.axisNum, out pmode, 1, out pclock);
                rtn += mc.GTN_GetSts(axis.prm.cardNum, axis.prm.axisNum, out psts, 1, out pclock);
                if ((psts & 0x400) == 0)
                {
                    if (pmode != 1)
                    {
                        rtn = mc.GTN_PrfJog(axis.prm.cardNum, axis.prm.axisNum);
                    }
                    mc.TJogPrm jogprm = new mc.TJogPrm();
                    jogprm.acc = axis.prm.getAccPlsPerMs2();
                    jogprm.dec = axis.prm.getAccPlsPerMs2();
                    jogprm.smooth = 0.5;
                    rtn = mc.GTN_SetJogPrm(axis.prm.cardNum, axis.prm.axisNum, ref jogprm);
                    rtn = mc.GTN_SetVel(axis.prm.cardNum, axis.prm.axisNum, axis.prm.getVelPlsPerMs() * beilv);
                    rtn = mc.GTN_ClrSts(axis.prm.cardNum, axis.prm.axisNum, 1);
                    rtn = mc.GTN_Update(axis.prm.cardNum, 1 << (axis.prm.axisNum - 1));
                }
                else if (pmode == 1)
                {
                    rtn = mc.GTN_SetVel(axis.prm.cardNum, axis.prm.axisNum, axis.prm.getVelPlsPerMs() * beilv);
                    rtn = mc.GTN_ClrSts(axis.prm.cardNum, axis.prm.axisNum, 1);
                    rtn = mc.GTN_Update(axis.prm.cardNum, 1 << (axis.prm.axisNum - 1));
                }
                return rtn;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int MC_Power(AxisRef axis)
        {
            short rtn = 0;
            rtn += mc.GTN_AxisOn(axis.prm.cardNum, axis.prm.axisNum);
            return rtn;
        }

        public int MC_PowerOff(AxisRef axis)
        {
            short rtn = 0;
            rtn += mc.GTN_AxisOff(axis.prm.cardNum, axis.prm.axisNum);
            return rtn;
        }

        public int MC_Reset(AxisRef axis)
        {
            mc.GTN_ClrSts(axis.prm.cardNum, axis.prm.axisNum, 1);
            MC_AxisRef(ref axis);
            if (axis.alarm)
            {
                mc.GTN_SetDoBitReverse(axis.prm.cardNum, axis.prm.axisNum, mc.MC_CLEAR, axis.prm.axisNum, 1000);
            }
            return 0;
        }

        public int MC_SetAxis(AxisRef axis, CAxisSetPrm prm)
        {
            short rtn = 0;
            if (prm.enableAlm)
            {
                rtn += mc.GTN_AlarmOn(axis.prm.cardNum, axis.prm.axisNum);
            }
            else
            {
                rtn += mc.GTN_AlarmOff(axis.prm.cardNum, axis.prm.axisNum);
            }
            if (prm.enableLmtN)
            {
                rtn += mc.GTN_LmtsOn(axis.prm.cardNum, axis.prm.axisNum, mc.MC_LIMIT_NEGATIVE);
            }
            else
            {
                rtn += mc.GTN_LmtsOff(axis.prm.cardNum, axis.prm.axisNum, mc.MC_LIMIT_NEGATIVE);
            }
            if (prm.enableLmtP)
            {
                rtn += mc.GTN_LmtsOn(axis.prm.cardNum, axis.prm.axisNum, mc.MC_LIMIT_POSITIVE);
            }
            else
            {
                rtn += mc.GTN_LmtsOff(axis.prm.cardNum, axis.prm.axisNum, mc.MC_LIMIT_POSITIVE);
            }
            return rtn;
        }

        public int MC_SetPos(AxisRef axis, double pos)
        {
            short rtn = 0;
            rtn += mc.GTN_SetPrfPos(axis.prm.cardNum, axis.prm.axisNum, (int)pos);
            rtn += mc.GTN_SetEncPos(axis.prm.cardNum, axis.prm.axisNum, (int)pos);
            rtn += mc.GTN_SynchAxisPos(axis.prm.cardNum, 1 << (axis.prm.axisNum-1));
            return rtn;
        }

        public int MC_StartCapture(AxisRef enc, CCapturePrm captruePrm)
        {
            mc.GTN_ClearCaptureStatus(enc.prm.cardNum, enc.prm.axisNum);
            mc.TTriggerEx triger = new mc.TTriggerEx();
            short rtn = mc.GTN_GetTriggerEx(enc.prm.cardNum, enc.prm.axisNum, out triger);
            triger.latchType = 23;
            triger.latchIndex = enc.prm.axisNum;
            triger.probeType = captruePrm.type;
            triger.probeIndex = captruePrm.index;
            triger.sense = 0;
            triger.loop = 1;
            triger.windowOnly = 0;
            rtn += mc.GTN_SetTriggerEx(enc.prm.cardNum, enc.prm.axisNum, ref triger);
            return rtn;
        }

        public int preMC_CamModel(AxisRef master, AxisRef slaver)
        {
            int model = 0;
            uint clock = 0;
            //判断是否是跟随模式
            short rtn = mc.GTN_GetPrfMode(slaver.prm.cardNum, slaver.prm.axisNum, out model, 1, out clock);
            if (model != 4)
            {
                int sts = 0;
                mc.GTN_GetSts(slaver.prm.cardNum, slaver.prm.axisNum, out sts, 1, out clock);
                if ((sts & (1 << 10)) == 0)
                {
                    rtn += mc.GTN_PrfFollow(slaver.prm.cardNum, slaver.prm.axisNum, 0);
                }
                else
                {
                    return -1;//模式不对
                }
            }
            rtn += mc.GTN_SetFollowMaster(slaver.prm.cardNum, slaver.prm.axisNum, master.prm.axisNum, mc.FOLLOW_MASTER_ENCODER, 0);
            rtn += mc.GTN_FollowClear(slaver.prm.cardNum, slaver.prm.axisNum, 0);
            rtn += mc.GTN_FollowClear(slaver.prm.cardNum, slaver.prm.axisNum, 1);
            return rtn;
        }

        public void Run()
        {
            
        }

        public void setDO(IoRef output, bool value)
        {
            short rtn = 0;
            if (value)
            {
                rtn += mc.GTN_SetDoBit(output.prm.model, mc.MC_GPO, output.prm.index, 0);
            }
            else
            {
                rtn += mc.GTN_SetDoBit(output.prm.model, mc.MC_GPO, output.prm.index, 1);
            }
        }

        public bool UnInit()
        {
            short rtn = mc.GTN_Reset(1);
            rtn += mc.GTN_Reset(2);
            rtn += mc.GTN_Close();
            return rtn == 0;
        }
    }
}
