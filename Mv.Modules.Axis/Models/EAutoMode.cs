namespace MotionWrapper
{
    /// <summary>
    /// 轴状态
    /// 这个整体数据必须和参数列表中的轴一一对应
    /// 这样在指定某个轴的时候,直接从参数的IDEX上指定
    /// </summary>
    //public class AxisRef
    //{
    //    public volatile bool isHoming = false;//回零的时候会不断刷新
    //    public volatile bool On, lmtp = true, lmtn = true, alm = true, arrive, moveing,atPos;
    //    public volatile int homed=2;//0=成功 1正在回 2=没有回零 -1 回零错误
    //    public volatile float encpos, prfpos, encvel,targetPos;
    //    public volatile float axisprfpos;//固高特有
    //}
    /// <summary>
    /// 运行模式
    /// </summary>
    public enum EAutoMode
    {
        MANUAL = 0, 
        AUTO = 1, 
        PAUSE = 2,
        OHTER=3, 
        STEP = 4
    }
}
