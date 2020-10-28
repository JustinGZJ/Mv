using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MotionWrapper
{
    /// <summary>
    /// 轴的类型 现行实际轴  圆弧实际轴 纯编码器  虚拟轴
    /// </summary>
    public enum EAxisType
    {
        Line=1,Circle=2,Enc=3,Vir=4
    }
    /// <summary>
    /// 凸轮数据
    /// </summary>
    public class CCamData
    {
        public long master, slaver;//位移量
        public int moveType;//0=nomal 1 = even 2=stop 3 continue
    }

    /// <summary>
    /// 组件的状态
    /// </summary>
    public enum EMachSts
    {
        Err = -2, Alm = -1, Ok = 0,IDLE=1
    }
}
