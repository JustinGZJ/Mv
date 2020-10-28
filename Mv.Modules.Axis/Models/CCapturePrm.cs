namespace MotionWrapper
{
    /// <summary>
    /// 捕获的参数
    /// </summary>
    public class CCapturePrm
    {
        //高川
        //#define CAPT_MODE_Z          0   // 编码器Z相捕获 
        //#define CAPT_MODE_IO         1   // IO 捕获 
        //#define CAPT_MODE_Z_AND_IO   2   // IO+Z相 捕获 
        //#define CAPT_MODE_Z_AFT_IO   3   // 先IO触发再Z相触发 捕获 
        //gsn
        //CAPTURE_HOME1
        // CAPTURE_INDEX2
        // CAPTURE_PROBE3
        public short type = 0;//类型
        //高川
        // 编码器硬件捕获IO源选择 
        //#define CAPT_IO_SRC_HOME    0   // 原点输入作为捕获IO 
        //#define CAPT_IO_SRC_LMTN    1   // 负向限位输入作为捕获IO 
        //#define CAPT_IO_SRC_LMTP    2   // 正向限位输入作为捕获IO 
        //#define CAPT_IO_SRC_DI0     3   // 通用数字输入0作为捕获IO 
        //#define CAPT_IO_SRC_DI1     4   // 通用数字输入1作为捕获IO 
        //#define CAPT_IO_SRC_DI2     5   // 通用数字输入2作为捕获IO 
        //#define CAPT_IO_SRC_DI3     6   // 通用数字输入3作为捕获IO 
        //#define CAPT_IO_SRC_DI4     7   // 通用数字输入4作为捕获IO 
        //#define CAPT_IO_SRC_DI5     8   // 通用数字输入5作为捕获IO 
        //#define CAPT_IO_SRC_DI6     9   // 通用数字输入6作为捕获IO 
        //#define CAPT_IO_SRC_DI7     10  // 通用数字输入7作为捕获IO 
        //#define CAPT_IO_SRC_DI8     11  // 通用数字输入8作为捕获IO 
        //#define CAPT_IO_SRC_DI9     12  // 通用数字输入9作为捕获IO 
        //#define CAPT_IO_SRC_DI10    13  // 通用数字输入10作为捕获IO 
        //#define CAPT_IO_SRC_DI11    14  // 通用数字输入11作为捕获IO 
        //#define CAPT_IO_SRC_DI12    15  // 通用数字输入12作为捕获IO 
        public short core = 1;//GSN使用  1/2 代表核号
        public short index = 0;//硬件参数号 GSN = 1-X
        public CCapturePrm(short _type, short _index,short core=1)
        {
            this.core = core;//职有GSN用
            type = _type;
            index = _index;
        }
    }
}
