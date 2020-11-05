namespace MotionWrapper
{
    /// <summary>
    /// 这个是一个IO的参数表,可以从文件中读取到
    /// </summary>
    public class CInOutPrm
    {
        //public int dataindex = 0;//这个IO点数据处于控制器类的数据列表中的哪一个
        private string name = "";//唯一的ID号
        private short model = 0;//0-9是卡  10是别的模块 GSN的时候  1和2
        private short index = 0;//位置 GTS[0,15]  0-15 GSN[1,66]
        private EIoType ioType = EIoType.NoamlInput;
        private bool nc = false;//常开常闭
        //界面中显示  
        private string modelStr = "";//所属的模块
        private string pinStr = "";//所属的针脚

        public string Name { get => name; set => name = value; }
        public short Model { get => model; set => model = value; }
        public short Index { get => index; set => index = value; }
        public EIoType IoType { get => ioType; set => ioType = value; }
        public bool NC { get => nc; set => nc = value; }

        public string ModelStr { get => modelStr; set => modelStr = value; }
        public string PinStr { get => pinStr; set => pinStr = value; }
    }
}
