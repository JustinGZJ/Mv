namespace MotionWrapper
{
    /// <summary>
    /// 启停模块
    /// </summary>
    public interface IStartStopModel
    {
        bool Start();
        bool Stop();
        bool Pause();
        bool Reset();
    }
}
