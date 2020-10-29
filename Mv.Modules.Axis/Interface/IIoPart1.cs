namespace MotionWrapper
{
    public interface IGtsMotion :ICMotionData, IMotionPart1, IMotionPart5, IIoPart1, IInitable, IFreshable
    {

    }
    /// <summary>
    /// IO控制的通用1
    /// </summary>
    public interface IIoPart1
    {

        bool getDiCounter(IoRef input, ref long counter);
        /// <summary>
        /// 读取一个区域的IO
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="lenth"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool getDi(int startIndex, int lenth, ref bool[] value);
        /// <summary>
        /// 读取区域的输出
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="lenth"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool getDo(int startIndex, int lenth, ref bool[] value);
        bool getDi(IoRef input);
        bool getDo(IoRef output);
        void setDO(IoRef output, bool value);
    }
}
