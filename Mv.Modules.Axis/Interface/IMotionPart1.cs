namespace MotionWrapper
{
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
}
