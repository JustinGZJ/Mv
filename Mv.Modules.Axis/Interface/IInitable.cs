namespace MotionWrapper
{
    /// <summary>
    /// 一些建议组件  都要初始化反初始化和刷新
    /// </summary>
    public interface IInitable
    {
        bool Init();                     //初始化
        bool UnInit();                   //反初始化
    }
}
