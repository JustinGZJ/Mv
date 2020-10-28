using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotionWrapper.BaseClass;
using MotionWrapper.Interface;
using MotionWrapper.PubicData;

namespace MotionWrapper
{
    public class CGsnMotion : CMotionData, IMotionPart1, IMotionPart5, IIoPart1, IInitModel, IFreshModel
    {
        public void Fresh()
        {
            throw new NotImplementedException();
        }

        public bool getDi(int startIndex, int lenth, ref bool[] value)
        {
            throw new NotImplementedException();
        }

        public bool getDi(CIoMap input)
        {
            throw new NotImplementedException();
        }

        public bool getDo(int startIndex, int lenth, ref bool[] value)
        {
            throw new NotImplementedException();
        }

        public bool getDo(CIoMap output)
        {
            throw new NotImplementedException();
        }

        public bool Init()
        {
            throw new NotImplementedException();
        }

        public int MC_AxisRef(ref AxisRef axisref)
        {
            throw new NotImplementedException();
        }

        public int MC_AxisRef(int startIndex, int lenth, ref AxisRef[] axisS)
        {
            throw new NotImplementedException();
        }

        public int MC_Cam(string camlist)
        {
            throw new NotImplementedException();
        }

        public int MC_Cam(AxisRef master, AxisRef slaver, double passpos, List<CCamData> data, bool relPasspos = false)
        {
            throw new NotImplementedException();
        }

        public int MC_CamStatus(AxisRef slaver, ref CCamStatus status)
        {
            throw new NotImplementedException();
        }

        public int MC_CaptureStatus(AxisRef enc, ref double capturePos)
        {
            throw new NotImplementedException();
        }

        public int MC_CrdCreate(int crdNum)
        {
            throw new NotImplementedException();
        }

        public int MC_CrdData(string cmd, int crdNum = 0)
        {
            throw new NotImplementedException();
        }

        public int MC_CrdData(List<string> gcode, int crdNum = 0)
        {
            throw new NotImplementedException();
        }

        public int MC_CrdOverride(int crdNum, float BeiLv)
        {
            throw new NotImplementedException();
        }

        public int MC_CrdSetOrigin(int crdNum, List<double> origionData)
        {
            throw new NotImplementedException();
        }

        public int MC_CrdStart(int cardNum = 0)
        {
            throw new NotImplementedException();
        }

        public int MC_CrdStatus(int crdNum = 0)
        {
            throw new NotImplementedException();
        }

        public int MC_EStop(AxisRef axis)
        {
            throw new NotImplementedException();
        }

        public int MC_Home(ref AxisRef axis)
        {
            throw new NotImplementedException();
        }

        public int MC_HomeStatus(ref AxisRef axis)
        {
            throw new NotImplementedException();
        }

        public int MC_MoveAbs(AxisRef axis, double tpos, double beilv = 0.5)
        {
            throw new NotImplementedException();
        }

        public int MC_MoveAdd(AxisRef axis, double dist, double beilv = 0.5)
        {
            throw new NotImplementedException();
        }

        public int MC_MoveJog(AxisRef axis, double beilv = 0.5)
        {
            throw new NotImplementedException();
        }

        public int MC_Power(AxisRef axis)
        {
            throw new NotImplementedException();
        }

        public int MC_PowerOff(AxisRef axis)
        {
            throw new NotImplementedException();
        }

        public int MC_Reset(AxisRef axis)
        {
            throw new NotImplementedException();
        }

        public int MC_SetPos(AxisRef axis, double pos)
        {
            throw new NotImplementedException();
        }

        public int MC_StartCapture(AxisRef enc, CCapturePrm captruePrm)
        {
            throw new NotImplementedException();
        }

        public int preMC_CamModel(AxisRef master, AxisRef slaver)
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void setDO(CIoMap output, bool value)
        {
            throw new NotImplementedException();
        }

        public bool UnInit()
        {
            throw new NotImplementedException();
        }
    }
}
