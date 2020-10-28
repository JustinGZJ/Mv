using System.Collections.Generic;

namespace MotionWrapper
{
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
}
