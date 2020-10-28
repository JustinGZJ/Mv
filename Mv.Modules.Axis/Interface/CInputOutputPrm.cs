namespace MotionWrapper
{
    /// <summary>
    /// 这个是一个IO的参数表,可以从文件中读取到
    /// </summary>
    public class CInputOutputPrm
    {
        //public int dataindex = 0;//这个IO点数据处于控制器类的数据列表中的哪一个
        public string ID = "";//唯一的ID号
        public short model = 0;//0-9是卡  10是别的模块 GSN的时候  1和2
        public short index = 0;//位置 GTS[0,15]  0-15 GSN[1,66]
        public EIoType ioType = EIoType.NoamlInput;
        public bool oftenOpen = false;//常开常闭
        //界面中显示
        public string name = "";//名称    
        public string modelStr = "";//所属的模块
        public string pinStr = "";//所属的针脚
    }
}
