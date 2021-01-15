using System;
using System.Collections.Generic;
using System.Threading;
using gts;
using static gts.mc;
using Mv.Core;
using Mv.Modules.Axis.Views;
using System.Windows.Documents;
using System.Linq;
using System.Threading.Tasks;

namespace MotionWrapper
{
    /// <summary>
    /// IO部分
    /// </summary>
    public partial class CGtsMotion
    {
        #region IIoPart1
        public bool getDi(IoRef input)
        {
            int v;
            switch (input.Prm.IoType)
            {
                case EIoType.NoamlInput:
                    if (input.Prm.Model == 0)
                        GT_GetDi(cardNum, (short)input.Prm.IoType, out v);
                    else
                    {
                        GT_GetExtIoValue(cardNum, 0, out var m);
                        v = m;
                    }
                    break;
                case EIoType.Alarm:
                case EIoType.LimitP:
                case EIoType.LimitN:
                case EIoType.Home:
                case EIoType.Arrive:
                    GT_GetDi(cardNum, (short)input.Prm.IoType, out v);
                    break;
                case EIoType.clrSts:
                case EIoType.ServoOn:
                case EIoType.NomalOutput:
                    if (input.Prm.Model > 0)
                        v = extval[input.Prm.Model - 1];
                    else
                        GT_GetDo(cardNum, (short)input.Prm.IoType, out v);
                    break;
                default:
                    v = 0;
                    break;
            }
            return ((v & (1 << input.Prm.Index)) > 0) ^ (input.Prm.NC);
        }
        public ushort[] extval = new ushort[32];
        public void setDO(IoRef output, bool value)
        {
            if (output.Prm.Model > 0 && output.Prm.IoType == EIoType.NomalOutput)
            {
                if (value)
                    extval[output.Prm.Model - 1] |= (ushort)(1 << output.Prm.Index);
                else
                    extval[output.Prm.Model - 1] &= ((ushort)(~(1 << output.Prm.Index)));
                var v = (output.Prm.NC) ^ value;
                GT_SetExtIoBit(cardNum, (short)(output.Prm.Model - 1), output.Prm.Index, (ushort)((v) ? 1 : 0));
            }
            else
            {
                value = (output.Prm.NC) ^ value;
                //从1开始
                GT_SetDoBit(cardNum, (short)output.Prm.IoType, (short)(output.Prm.Index + 1), (short)((value) ? 1 : 0));
            }

        }

        public bool getDo(IoRef output)
        {
            int v;
            switch (output.Prm.IoType)
            {
                case EIoType.NoamlInput:
                    if (output.Prm.Model == 0)
                        GT_GetDi(cardNum, (short)output.Prm.IoType, out v);
                    else
                    {
                        GT_GetExtIoValue(cardNum, 0, out var m);
                        v = m;
                    }
                    break;
                case EIoType.Alarm:
                case EIoType.LimitP:
                case EIoType.LimitN:
                case EIoType.Home:
                case EIoType.Arrive:
                    GT_GetDi(cardNum, (short)output.Prm.IoType, out v);
                    break;
                case EIoType.clrSts:
                case EIoType.ServoOn:
                case EIoType.NomalOutput:
                    if (output.Prm.Model > 0)
                        if (output.Prm.NC)
                            v = ~extval[output.Prm.Model - 1];
                        else
                            v = extval[output.Prm.Model - 1];
                    else
                        GT_GetDo(cardNum, (short)output.Prm.IoType, out v);
                    break;
                default:
                    v = 0;
                    break;
            }

            return ((v & (1 << output.Prm.Index)) > 0) ^ (output.Prm.NC);
        }
        public bool getDi(int startIndex, int lenth, ref bool[] value)
        {
            throw new NotImplementedException();
        }

        public bool getDo(int startIndex, int lenth, ref bool[] value)
        {
            throw new NotImplementedException();
        }
        public bool getDiCounter(IoRef input, ref long counter)
        {
            if (mc.GT_GetDiReverseCount(cardNum, (short)input.Prm.IoType, input.Prm.Index, out var cnt, 1) != 0)
            {
                return false;
            }
            counter = cnt;
            return true;
        }
        #endregion
    }
    public partial class CGtsMotion
    {
        #region IMotionPart1
        public int MC_SetPos(AxisRef axis, double pos)
        {
            mc.GT_SetPrfPos(axis.Prm.CardNum, axis.Prm.AxisNum, (int)pos);
            mc.GT_SetEncPos(axis.Prm.CardNum, axis.Prm.AxisNum, (int)pos);
            mc.GT_SynchAxisPos(axis.Prm.CardNum, 1 << (axis.Prm.AxisNum - 1));
            return 0;
        }
        public async Task<int> MC_Home(AxisRef axis)
        {


            THomePrm prm = new THomePrm();
            axis.Homed = 0;
            axis.IsHoming = false;

            short rtn = GT_GetHomePrm(axis.Prm.CardNum, axis.Prm.AxisNum, out prm);
            rtn = GT_ZeroPos(axis.Prm.CardNum, axis.Prm.AxisNum, 1);
            rtn = GT_ClrSts(axis.Prm.CardNum, axis.Prm.AxisNum, 1);
            prm.mode = 20;
            prm.acc = axis.Prm.getAccPlsPerMs2();
            prm.dec = prm.acc;
            //  prm.pad2_1 = 1;  //检测极限位置和回零位置进行回退
            prm.moveDir = (short)(axis.Prm.HomeSearch >= 0 ? 1 : -1);
            prm.indexDir = 1;
            prm.edge = 0;//1 上升沿 0下降延
            prm.homeOffset = (int)axis.Prm.mm2pls(axis.Prm.Homeoffset);
            prm.velHigh = axis.Prm.mmpers2plsperms(axis.Prm.HomeVelHigh);
            prm.velLow = axis.Prm.mmpers2plsperms(axis.Prm.HomeVelLow);
            prm.searchHomeDistance = Math.Abs((int)axis.Prm.mm2pls(axis.Prm.HomeSearch));
            prm.searchIndexDistance = prm.searchHomeDistance;
            prm.escapeStep = (int)axis.Prm.mm2pls(axis.Prm.HomeLeave);
            if (axis.HomeSwitch)
            {
                rtn += (short)MC_MoveAdd(axis, axis.Prm.HomeLeave * prm.moveDir * (-1), axis.Rate);
                var axisref = axis;
                if (rtn == 0)
                {
                    rtn += await Task.Run<short>(() =>
                      {
                          if (SpinWait.SpinUntil(() => axisref.Moving == true, 1000))
                          {
                              return (short)(SpinWait.SpinUntil(() => axisref.Moving == false, 20 * 1000) ? 0 : -200);
                          }
                          return -100;
                      });
                }
                if (rtn < 0)
                    return rtn;
            }
            rtn += mc.GT_GoHome(axis.Prm.CardNum, axis.Prm.AxisNum, ref prm);
            if (rtn == 0) axis.IsHoming = true;
            else axis.IsHoming = false;
            THomeStatus tHomeSts = new mc.THomeStatus();
            await Task.Run(() =>
             {
                 do
                 {
                     rtn = GT_GetHomeStatus(axis.Prm.CardNum, axis.Prm.AxisNum, out tHomeSts); //获取回原点状态
                 } while (tHomeSts.run != 0); //等待搜索原点停止 
                 Thread.Sleep(500);
             });
            rtn += (short)MC_HomeStatus(ref axis);
            return rtn;
        }


        public int MC_MoveAbs(AxisRef axis, double tpos, double beilv)
        {
            short rtn = 0;
            int pmode;
            int psts;
            uint pclock;
            rtn += mc.GT_GetPrfMode(axis.Prm.CardNum, axis.Prm.AxisNum, out pmode, 1, out pclock);
            rtn += mc.GT_GetSts(axis.Prm.CardNum, axis.Prm.AxisNum, out psts, 1, out pclock);
            if ((psts & 0x400) == 0)
            {
                if (pmode != 0)
                {
                    rtn += mc.GT_PrfTrap(axis.Prm.CardNum, axis.Prm.AxisNum);
                }
                mc.TTrapPrm trapprm = new mc.TTrapPrm();
                trapprm.acc = axis.Prm.getAccPlsPerMs2();
                trapprm.dec = axis.Prm.getAccPlsPerMs2();
                trapprm.smoothTime = 50;
                trapprm.velStart = 0;
                rtn += mc.GT_SetTrapPrm(axis.Prm.CardNum, axis.Prm.AxisNum, ref trapprm);
                rtn += mc.GT_SetVel(axis.Prm.CardNum, axis.Prm.AxisNum, axis.Prm.getVelPlsPerMs() * beilv);
                rtn += mc.GT_SetPos(axis.Prm.CardNum, axis.Prm.AxisNum, (int)(axis.Prm.mm2pls(tpos)));
                //int sts = 0;
                //uint clock = 0;
                //rtn = mc.GT_GetSts(cardnum, relAxis, out sts, 1, out clock);
                rtn += mc.GT_Update(axis.Prm.CardNum, 1 << (axis.Prm.AxisNum - 1));
            }
            else if (pmode == 0)
            {
                rtn += mc.GT_SetVel(axis.Prm.CardNum, axis.Prm.AxisNum, axis.Prm.getVelPlsPerMs() * beilv);
                rtn += mc.GT_SetPos(axis.Prm.CardNum, axis.Prm.AxisNum, (int)(axis.Prm.mm2pls(tpos)));
                rtn += mc.GT_Update(axis.Prm.CardNum, 1 << (axis.Prm.AxisNum - 1));
            }
            return rtn;
        }

        public int MC_MoveAdd(AxisRef axis, double dist, double beilv)
        {
            short rtn = 0;
            int pmode;
            int psts;
            uint pclock;
            double prfpos;
            rtn += mc.GT_GetPrfMode(axis.Prm.CardNum, axis.Prm.AxisNum, out pmode, 1, out pclock);
            rtn += mc.GT_GetSts(axis.Prm.CardNum, axis.Prm.AxisNum, out psts, 1, out pclock);
            if ((psts & 0x400) == 0)
            {
                if (pmode != 0)
                {
                    rtn += mc.GT_PrfTrap(axis.Prm.CardNum, axis.Prm.AxisNum);
                }
                mc.TTrapPrm trapprm = new mc.TTrapPrm();
                trapprm.acc = axis.Prm.getAccPlsPerMs2();
                trapprm.dec = axis.Prm.getAccPlsPerMs2();
                trapprm.smoothTime = 50;
                trapprm.velStart = 0;
                rtn += mc.GT_SetTrapPrm(axis.Prm.CardNum, axis.Prm.AxisNum, ref trapprm);
                rtn += mc.GT_SetVel(axis.Prm.CardNum, axis.Prm.AxisNum, axis.Prm.getVelPlsPerMs() * beilv);
                rtn += mc.GT_GetPrfPos(axis.Prm.CardNum, axis.Prm.AxisNum, out prfpos, 1, out pclock);
                rtn += mc.GT_SetPos(axis.Prm.CardNum, axis.Prm.AxisNum, (int)(axis.Prm.mm2pls(dist) + prfpos));
                rtn += mc.GT_Update(axis.Prm.CardNum, 1 << (axis.Prm.AxisNum - 1));
            }
            return rtn;
        }

        public int MC_MoveJog(AxisRef axis, double beilv)
        {
            try
            {
                short rtn = 0;
                int pmode;
                int psts;
                uint pclock;
                rtn += mc.GT_GetPrfMode(axis.Prm.CardNum, axis.Prm.AxisNum, out pmode, 1, out pclock);
                rtn += mc.GT_GetSts(axis.Prm.CardNum, axis.Prm.AxisNum, out psts, 1, out pclock);
                if ((psts & 0x400) == 0)
                {
                    if (pmode != 1)
                    {
                        rtn = mc.GT_PrfJog(axis.Prm.CardNum, axis.Prm.AxisNum);
                    }
                    mc.TJogPrm jogprm = new mc.TJogPrm();
                    jogprm.acc = axis.Prm.getAccPlsPerMs2();
                    jogprm.dec = axis.Prm.getAccPlsPerMs2();
                    jogprm.smooth = 0.5;
                    rtn = mc.GT_SetJogPrm(axis.Prm.CardNum, axis.Prm.AxisNum, ref jogprm);
                    rtn = mc.GT_SetVel(axis.Prm.CardNum, axis.Prm.AxisNum, axis.Prm.getVelPlsPerMs() * beilv);
                    rtn = mc.GT_ClrSts(axis.Prm.CardNum, axis.Prm.AxisNum, 1);
                    rtn = mc.GT_Update(axis.Prm.CardNum, 1 << (axis.Prm.AxisNum - 1));
                }
                else if (pmode == 1)
                {
                    rtn = mc.GT_SetVel(axis.Prm.CardNum, axis.Prm.AxisNum, axis.Prm.getVelPlsPerMs() * beilv);
                    rtn = mc.GT_ClrSts(axis.Prm.CardNum, axis.Prm.AxisNum, 1);
                    rtn = mc.GT_Update(axis.Prm.CardNum, 1 << (axis.Prm.AxisNum - 1));
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
            GT_ClrSts(axis.Prm.CardNum, axis.Prm.AxisNum, 1);
            return mc.GT_AxisOn(axis.Prm.CardNum, axis.Prm.AxisNum);
        }

        public int MC_Reset(AxisRef axis)
        {
            mc.GT_ClrSts(axis.Prm.CardNum, axis.Prm.AxisNum, 1);
            if (axis.Alarm)
            {
                mc.GT_SetDoBitReverse(axis.Prm.CardNum, mc.MC_CLEAR, axis.Prm.AxisNum, 0, 500);
            }
            return 0;
        }

        public int MC_AxisRef(ref AxisRef axisref)
        {
            short rtn = 0;
            uint clock = 0;
            double[] prfpos = new double[1];
            double[] encpos = new double[1];
            double[] encvel = new double[1];
            int[] sts = new int[1];
            rtn += mc.GT_GetPrfPos(0, 1, out prfpos[0], 1, out clock);
            rtn += mc.GT_GetEncPos(0, 1, out encpos[0], 1, out clock);
            rtn += mc.GT_GetEncVel(0, 1, out encvel[0], 1, out clock);
            rtn += mc.GT_GetSts(0, 1, out sts[0], 1, out clock);
            rtn += mc.GT_GetDi(cardNum, (short)EIoType.Home, out var home);
            //解析
            axisref.Alarm = (sts[0] & 0x2) != 0;
            axisref.ServoOn = (sts[0] & 0x200) != 0;
            axisref.LimitN = (sts[0] & 0x40) != 0;
            axisref.LimitP = (sts[0] & 0x20) != 0;
            axisref.Moving = (sts[0] & 0x400) != 0;
            axisref.HomeSwitch = (home & (1 << (axisref.Prm.AxisNum - 1))) > 0;

            axisref.CmdPos = (float)axisref.Prm.pls2mm((long)prfpos[0]);
            axisref.RelPos = (float)axisref.Prm.pls2mm((long)encpos[0]);
            axisref.RelVel = (float)axisref.Prm.plsperms2mmpers(encvel[0]);

            return rtn;
        }

        public int MC_PowerOff(AxisRef axis)
        {
            return mc.GT_AxisOff(axis.Prm.CardNum, axis.Prm.AxisNum);
        }

        public int MC_EStop(AxisRef axis)
        {
            axis.IsHoming = false;
            mc.GT_Stop(axis.Prm.CardNum, 1 << (axis.Prm.AxisNum - 1), 1 << (axis.Prm.AxisNum - 1));
            return 0;
        }

        public int MC_AxisRef(int startIndex, int lenth, ref AxisRef[] axisS)
        {
            throw new NotImplementedException();
        }

        public int MC_SetAxis(AxisRef axis, CAxisSetPrm prm)
        {
            short rtn = 0;
            rtn += mc.GT_SetSoftLimit(cardNum, axis.Prm.AxisNum, prm.EnableLmtP ? 1 : 0, prm.EnableLmtN ? 1 : 0);
            rtn += prm.EnableAlm
                ? mc.GT_AlarmOn(cardNum, axis.Prm.AxisNum)
                : mc.GT_AlarmOff(cardNum, axis.Prm.AxisNum);
            rtn += mc.GT_SetSoftLimit(cardNum, axis.Prm.AxisNum, (int)axis.Prm.mm2pls(prm.SoftLmtP),
                (int)axis.Prm.mm2pls(prm.SoftLmtN));
            return rtn;
        }

        public int MC_HomeStatus(ref AxisRef axis)
        {
            short rtn = 0;
            mc.THomeStatus tmpHomeSts = new mc.THomeStatus();
            rtn = mc.GT_GetHomeStatus(axis.Prm.CardNum, axis.Prm.AxisNum, out tmpHomeSts);

            if (tmpHomeSts.run == 0 && tmpHomeSts.error == mc.HOME_ERROR_NONE)//回零完成
            {
                if (axis.Homed == 0)
                {
                    mc.GT_ZeroPos(axis.Prm.CardNum, axis.Prm.AxisNum, 1);
                }
                axis.Homed = 1;

                axis.IsHoming = false;
            }
            if (tmpHomeSts.run == 0 && tmpHomeSts.error != mc.HOME_ERROR_NONE)//回零完成
            {
                axis.Homed = -1;
                axis.IsHoming = false;
            }
            return rtn;
        }
        #endregion
    }

    /// <summary>
    /// 固高卡的封装  都在这里
    /// 数据区域都在这个类里面
    /// 每次项目  这个类需要重写的
    /// </summary>
    public partial class CGtsMotion : CMotionData, IGtsMotion
    {
        #region Fields
        public const int usedInput = 16, usedOutput = 16, usedAxis = 5;//必须外部指定
        private readonly IConfigManager<MotionConfig> configManager;
        public bool initOk = false;
        private Thread runThread = null;       //用来刷新IO的
        //私有变量
        private short cardNum = 0;
        #endregion
        //局部变量
        #region Ctors
        public CGtsMotion(IConfigManager<MotionConfig> configManager)
        {
            MotionConfig motionConfig = configManager.Get();
            List<AxisParameter> axisParameters = motionConfig.AxisParameters.ToList();
            var inputs = motionConfig.Inputs;
            var outputs = motionConfig.Outputs;
            for (int i = 0; i < axisParameters.Count; i++)
            {
                AxisRefs[i] = new AxisRef(axisParameters[i].Name)
                {
                    Prm = axisParameters[i]
                };
            }
            for (int i = 0; i < inputs.Count; i++)
            {
                Dis[i] = new IoRef(inputs[i].Name);
                Dis[i].Prm = inputs[i];
            }
            for (int i = 0; i < outputs.Count; i++)
            {
                Dos[i] = new IoRef(outputs[i].Name);
                Dos[i].Prm = outputs[i];
            }

            this.configManager = configManager;
        }
        #endregion
        //局部变量





        /// <summary>
        /// 启动坐标系
        /// </summary>
        /// <param name="crdNum"></param>
        /// <returns></returns>

        #region IMotionPart5
        public int MC_CrdStart(int crdNum)
        {
            CCrdPrm cCrdPrm = configManager.Get().CrdParameters[crdNum];
            short rtn = mc.GT_CrdStart((short)cCrdPrm.CardNum, (short)cCrdPrm.CrdNum, 0);
            return rtn;
        }

        public int MC_CamStatus(AxisRef slaver, ref CCamStatus status)
        {
            return 0;
        }

        public int MC_CrdSetOrigin(int crdNum, List<double> origionData)
        {
            throw new NotImplementedException();
        }

        public int MC_CrdOverride(int crdNum, float BeiLv)
        {
            throw new NotImplementedException();
        }

        public int MC_CrdStatus(int crdNum)
        {
            CCrdPrm prm = configManager.Get().CrdParameters[crdNum];
            short crdRun = 0;
            int segment = 0;
            short rtn = mc.GT_CrdStatus((short)prm.CardNum, (short)prm.CrdNum, out crdRun, out segment, 0);
            if (crdRun <= 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public int MC_CrdData(string cmd, int crdNum)
        {
            int[] tpos = new int[3];
            float[] tmpPos = new float[3];
            short rtn = 0;
            if (crdNum < 0 || crdNum >= configManager.Get().CrdParameters.Count) return -1;
            CCrdPrm crdPrm = configManager.Get().CrdParameters[crdNum];


            string tmpCmd = cmd.ToUpper();
            string tmpStr = "";

            CompileUserCode compiler = new CompileUserCode();
            //     rtn += CNMCLib20.NMC_CrdBufClr(crdHandle[crdPrm.CrdNum - 1]);
            if (tmpCmd.IndexOf("G00") >= 0)
            {
                //X
                tmpStr = compiler.getFloatOrLong("X", tmpCmd);
                if (tmpStr != "")
                {
                    tmpPos[0] = float.Parse(tmpStr);
                    float targetX = tmpPos[0];
                }
                else
                {
                    return -1;
                }
                //Y
                tmpStr = compiler.getFloatOrLong("Y", tmpCmd);
                if (tmpStr != "")
                {
                    tmpPos[1] = float.Parse(tmpStr);
                    float targetY = tmpPos[1];
                }
                else
                {
                    return -1;
                }
                tmpPos[2] = 0.0f;
                tmpStr = compiler.getFloatOrLong("Z", tmpCmd);
                if (tmpStr != "")
                {
                    tmpPos[2] = float.Parse(tmpStr);
                    float targetY = tmpPos[2];
                }
                else
                {
                    return -2;
                }
                //转换
                tpos[0] = crdPrm.mm2pls(tmpPos[0]);
                tpos[1] = crdPrm.mm2pls(tmpPos[1]);
                tpos[2] = crdPrm.mm2pls(tmpPos[2]);
                rtn += GT_LnXYZ(cardNum, (short)crdNum, tpos[0], tpos[1], tpos[2], crdPrm.SynAccMax, crdPrm.SynVelMax, 0, 0);
            }
            else
            {
                return -1;
            }
            return rtn;
        }
        public int MC_CrdData(List<string> gcode, int crdNum)
        {
            throw new NotImplementedException();
        }

        public int MC_Cam(string camlist)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 提前进入Follow模式  防止浪费时间
        /// </summary>
        /// <param name="master"></param>
        /// <param name="slaver"></param>
        /// <returns></returns>
        public int MC_FollowMode(AxisRef master, AxisRef slaver)
        {
            int model = 0;
            uint clock = 0;
            //判断是否是跟随模式
            short rtn = mc.GT_GetPrfMode(slaver.Prm.CardNum, slaver.Prm.AxisNum, out model, 1, out clock);
            if (model != 4)
            {
                int sts = 0;
                mc.GT_GetSts(slaver.Prm.CardNum, slaver.Prm.AxisNum, out sts, 1, out clock);
                if ((sts & (1 << 10)) == 0)
                {
                    rtn += mc.GT_PrfFollow(slaver.Prm.CardNum, slaver.Prm.AxisNum, 0);
                }
                else
                {
                    return -1;//模式不对
                }
            }
            rtn += mc.GT_SetFollowMaster(slaver.Prm.CardNum, slaver.Prm.AxisNum, master.Prm.AxisNum, mc.FOLLOW_MASTER_PROFILE, 0);
            rtn += mc.GT_FollowClear(slaver.Prm.CardNum, slaver.Prm.AxisNum, 0);
            rtn += mc.GT_FollowClear(slaver.Prm.CardNum, slaver.Prm.AxisNum, 1);
            return rtn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="master"></param>
        /// <param name="slaver"></param>
        /// <param name="passpos">relPasspos为true的时候 这个是增量值,relPasspos为false的时候,这个为绝对值</param>
        /// <param name="data"></param>
        /// <param name="relPasspos">是否使用增量穿越点</param>
        /// <returns></returns>
        public int MC_Follow(AxisRef master, AxisRef slaver, double passpos, List<CCamData> data, bool relPasspos)
        {
            short rtn = 0;
            uint clock = 0;

            //判断是否是跟随模式
            foreach (var item in data)
            {
                rtn += GT_FollowSpace(slaver.Prm.CardNum, slaver.Prm.AxisNum, out short space, 0);
                rtn += mc.GT_FollowData(slaver.Prm.CardNum, slaver.Prm.AxisNum, (int)item.Master, item.Slaver, (short)item.MoveType, 0);
            }


            if (passpos != 0)
            {
                if (relPasspos)
                {
                    double value = 0;
                    rtn += mc.GT_GetAxisPrfPos(master.Prm.CardNum, master.Prm.AxisNum, out value, 1, out clock);
                    rtn += mc.GT_SetFollowEvent(slaver.Prm.CardNum, slaver.Prm.AxisNum, mc.FOLLOW_EVENT_PASS, 1, (int)(passpos + value));
                }
                else
                {
                    rtn += mc.GT_SetFollowEvent(slaver.Prm.CardNum, slaver.Prm.AxisNum, mc.FOLLOW_EVENT_PASS, 1, (int)passpos);
                }
            }
            else
            {
                rtn += mc.GT_SetFollowEvent(slaver.Prm.CardNum, slaver.Prm.AxisNum, mc.FOLLOW_EVENT_START, 1, (int)passpos);
            }
            rtn += mc.GT_FollowStart(slaver.Prm.CardNum, 1 << (slaver.Prm.AxisNum - 1), 0);
            return rtn;
        }


        public int MC_StartCapture(AxisRef enc, CCapturePrm capturePrm)
        {
            //CAPTURE_HOME(该宏定义为 1) Home 捕获
            //CAPTURE_INDEX(该宏定义为 2) Index 捕获
            //CAPTURE_PROBE(该宏定义为 3) 探针捕获
            //CAPTURE_HSIO0(该宏定义为 6) HSIO0 口捕获
            //CAPTURE_HSIO1(该宏定义为 7) HSIO1 口捕获
            //short rtn = mc.GT_ClrSts((short)prm.cardnum, (short)prm.axisnum, 1);
            short rtn = mc.GT_SetCaptureMode(enc.Prm.CardNum, enc.Prm.AxisNum, (short)capturePrm.type);
            return rtn;
        }

        public int MC_CaptureStatus(AxisRef enc, ref double capturePos)
        {
            short cptSts = 0;
            int value = 0;
            uint clock = 0;
            short rtn = mc.GT_GetCaptureStatus(enc.Prm.CardNum, enc.Prm.AxisNum, out cptSts, out value, 1, out clock);
            if (rtn == 0 && cptSts == 1)
            {
                capturePos = enc.Prm.pls2mm(value);
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public int MC_CrdCreate(int crdNum)
        {
            if (cardNum < 0 || cardNum >= configManager.Get().CrdParameters.Count) return -1;
            short rtn = 0;
            int pmode;
            int psts;
            uint pclock;
            CCrdPrm crdPrm = configManager.Get().CrdParameters[crdNum];
            //随便找一个轴来查看下是否是插补坐标系
            rtn += mc.GT_GetPrfMode((short)crdPrm.CardNum, (short)crdPrm.X, out pmode, 1, out pclock);
            rtn += mc.GT_GetSts((short)crdPrm.CardNum, (short)crdPrm.X, out psts, 1, out pclock);
            if ((psts & 0x400) == 0)
            {
                TCrdPrm prm = new TCrdPrm();
                prm.dimension = (short)crdPrm.Dimension;
                prm.evenTime = 5;
                prm.setOriginFlag = 1;
                prm.synVelMax = crdPrm.getCrdMaxVelPlsPerMs();
                prm.synAccMax = crdPrm.getCrdMaxAccPlsPerMs2();
                prm.profile1 = (short)crdPrm.X;
                prm.profile2 = (short)crdPrm.Y;
                prm.profile3 = (short)crdPrm.Z;
                prm.profile4 = (short)crdPrm.A;
                rtn += GT_SetCrdPrm((short)crdPrm.CardNum, (short)crdPrm.CrdNum, ref prm);
                rtn += GT_CrdClear((short)crdPrm.CardNum, (short)crdPrm.CrdNum, 0);
                rtn += GT_CrdClear((short)crdPrm.CardNum, (short)crdPrm.CrdNum, 1);
                return rtn;
            }
            return -1;
        }

        public int MC_Compare(AxisRef axis, double posMM, IoRef cmpInput)
        {
            throw new NotImplementedException();
        }
        #endregion



        #region IFreshable
        public void Fresh()
        {
            subStatusData();
        }

        public void Run()
        {
            IFreshable fresh = this as IFreshable;
            while (true)
            {
                fresh.Fresh();
                Thread.Sleep(1);
            }
        }
        #endregion

        #region IInitable
        public bool Init()
        {
            try
            {
                short rtn = 0;
                rtn += mc.GT_Open(cardNum, 0, 0);
                rtn += mc.GT_Reset(cardNum);
                rtn += mc.GT_LoadConfig(cardNum, @"GTS800.cfg");
                rtn += mc.GT_OpenExtMdl(cardNum, null);
                rtn += mc.GT_LoadExtConfig(cardNum, @"EXTIO.cfg");
                rtn += mc.GT_ClrSts(cardNum, 1, 8);
                rtn += mc.GT_ZeroPos(cardNum, 1, 8);
                rtn += (short)AxisRefs.Where(x => x.Prm.Active).Select(axis => this.MC_Power(axis)).Sum();
                if (rtn == 0)
                {
                    runThread = new Thread(Run);
                    runThread.IsBackground = true;
                    runThread.Start();
                    return initOk = true;
                }
                return initOk = false;
            }
            catch (Exception)
            {
                return initOk = false;
            }
        }

        public bool UnInit()
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
                rtn += mc.GT_CloseExtMdl(cardNum);
                rtn += mc.GT_Reset(cardNum);
                rtn += mc.GT_Close(cardNum);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion


        private void subStatusData()
        {
            short rtn = 0;
            int inValue = 0, outValue = 0;
            double[] prfpos = new double[8];
            double[] encpos = new double[8];
            double[] encvel = new double[8];
            //辅助编码器
            //double[] fzencpos = new double[2];
            //double[] fzencvel = new double[2];
            int[] sts = new int[8];
            rtn += GT_GetDi(0, mc.MC_GPI, out inValue);
            rtn += mc.GT_GetDo(0, mc.MC_GPO, out outValue);
            rtn += GT_GetDi(cardNum, MC_HOME, out var homeVal); ;
            rtn = mc.GT_GetPrfPos(0, 1, out prfpos[0], 8, out _);
            rtn = mc.GT_GetEncPos(0, 1, out encpos[0], 8, out _);
            rtn = mc.GT_GetEncVel(0, 1, out encvel[0], 8, out _);
            rtn = mc.GT_GetSts(0, 1, out sts[0], 8, out _);
            ////辅助编码器
            //rtn = mc.GT_GetEncPos(0, 9, out fzencpos[0], 2, out _);
            //rtn = mc.GT_GetEncVel(0, 9, out fzencvel[0], 2, out _);
            List<AxisParameter> axisParameters = configManager.Get().AxisParameters.ToList();


            //解析
            for (int i = 0; i < 16; i++)
            {
                Mdis[i] = ((1 << i) & inValue) == 0;
                Mdos[i] = ((1 << i) & outValue) == 0;

                if (i < axisParameters.Count)//轴状态分解
                {
                    AxisRef axisRef = AxisRefs[i];
                    int index = axisRef.Prm.AxisNum - 1;
                    int status = sts[index];
                    axisRef.Alarm = (status & 0x2) != 0;
                    axisRef.ServoOn = (status & 0x200) != 0;
                    axisRef.LimitN = (status & 0x40) != 0;
                    axisRef.LimitP = (status & 0x20) != 0;
                    axisRef.Moving = (status & 0x400) != 0;
                    axisRef.HomeSwitch = (homeVal & (1 << index)) > 0;
                    axisRef.CmdPos = (float)axisRef.Prm.pls2mm((long)prfpos[index]);
                    axisRef.RelPos = (float)axisRef.Prm.pls2mm((long)encpos[index]);
                    axisRef.RelVel = (float)axisRef.Prm.plsperms2mmpers(encvel[index]);
                    //  MC_HomeStatus(ref AxisRefs[i]);
                }

            }

            var dis = Dis.Where(x => x.Name != "");
            foreach (var item in dis)
            {
                item.Value = getDi(item);
            }
            var dos = Dos.Where(x => x.Name != "");
            foreach (var item in dos)
            {
                item.Value = getDo(item);
            }
        }

        /// <summary>
        /// 获取指定范围的数组
        /// </summary>
        /// <param name="start"></param>
        /// <param name="lenth"></param>
        /// <returns></returns>
        public List<bool> getAllInput()
        {
            List<bool> tmp = new List<bool>();
            for (int i = 0; i < usedInput; i++)
            {
                tmp.Add(Mdis[i]);
            }
            return tmp;
        }
        /// <summary>
        /// 获取指定范围的数组
        /// </summary>
        /// <param name="start"></param>
        /// <param name="lenth"></param>
        /// <returns></returns>
        public List<bool> getAllOutput()
        {
            List<bool> tmp = new List<bool>();
            for (int i = 0; i < usedOutput; i++)
            {
                tmp.Add(Mdos[i]);
            }
            return tmp;
        }
        /// <summary>
        /// 获取指定范围的数组
        /// </summary>
        /// <param name="start"></param>
        /// <param name="lenth"></param>
        /// <returns></returns>
        public List<AxisRef> getAllAxis()
        {
            List<AxisRef> tmp = new List<AxisRef>();
            for (int i = 0; i < usedAxis; i++)
            {
                tmp.Add(AxisRefs[i]);
            }
            return tmp;
        }




    }
}
