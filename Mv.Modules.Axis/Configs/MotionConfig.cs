
using System.Collections.Generic;
using System.ComponentModel;

namespace MotionWrapper
{

    public class MotionConfig
    {
        [Category("IO输入|输入")]

        public List<CInOutPrm> Inputs { get; private set; } = new List<CInOutPrm>();

        [Category("IO输出|输出")]
        public List<CInOutPrm> Outputs { get;private set; } = new List<CInOutPrm>();

        [Category("轴参数|轴参数")]
        public List<AxisParameter> AxisParameters { get; private set; } = new List<AxisParameter>();

        [Category("坐标参数|坐标参数")]
        public List<CCrdPrm> CrdParameters { get; private set;} = new List<CCrdPrm>();

        [Category("P2P参数|P2P参数")]
        public List<P2PPrm> P2PPrameters { get; private set; } = new List<P2PPrm>();

        [Category("气缸参数")]
        public List<CylinderConfig> CylinderConfigs { get; private set; } = new List<CylinderConfig>();

    }

 
}
