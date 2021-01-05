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

    public enum EMoveType
    {
        Normal,Even,Stop,Continue
    }
    /// <summary>
    /// 凸轮数据
    /// </summary>
    public class CCamData
    {
        private long slaver;//位移量
        private EMoveType moveType;//0=nomal 1 = even 2=stop 3 continue
        private long master;

        public long Master { get => master; set => master = value; }
        public EMoveType MoveType { get => moveType; set => moveType = value; }
        public long Slaver { get => slaver; set => slaver = value; }
    }

    /// <summary>
    /// 组件的状态
    /// </summary>
    public enum EMachSts
    {
        Err = -2, Alm = -1, Ok = 0,IDLE=1
    }
}
