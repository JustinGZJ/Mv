using PropertyTools.DataAnnotations;
using System.Collections.Generic;

namespace MotionWrapper
{

    public class MotionConfig
    {
        [Category("IO输入|输入")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<CInOutPrm> Inputs { get; private set; } = new List<CInOutPrm>();

        [Category("IO输出|输出")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<CInOutPrm> Outputs { get;private set; } = new List<CInOutPrm>();

        [Category("轴参数|轴参数")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<AxisParameter> AxisParameters { get; private set; } = new List<AxisParameter>();

        [Category("坐标参数|坐标参数")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<CCrdPrm> CrdParameters { get; private set;} = new List<CCrdPrm>();

        [Category("P2P参数|P2P参数")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<P2PPrm> P2PPrameters { get; private set; } = new List<P2PPrm>();
    }

 
}
