namespace MotionWrapper
{
    /// <summary>
    /// 自定义的一些功能
    /// </summary>
    public interface ISerialNumber
    {
        /// <summary>
        /// 获取机器吗
        /// </summary>
        /// <returns></returns>
        string MC_GetMachSN(int machNum=0);
        /// <summary>
        /// 获取序列号 这个序列号是给用户输入的.包含了可以使用的天数.
        /// </summary>
        /// <returns></returns>
        string MC_GetSn(string machSN,int days= 30,bool type= true);
        /// <summary>
        /// 读取xml中保存的本台机器的序列号，用来验证
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        SerialData MC_ReadSnFile(string xml = @"Confige\SN.xml");
        /// <summary>
        /// 保存一些数据
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        void MC_SaveSnFile(SerialData data,string xml = @"Confige\SN.xml");
        /// <summary>
        /// 检查是否允许运行
        /// </summary>
        /// <returns></returns>
        bool MC_CanRun(string userSn,string machSN,ref int haveDays);
    }
}
