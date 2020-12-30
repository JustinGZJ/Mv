
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MotionWrapper
{

    public class MotionConfig
    {
        [Category("IO输入|输入")]

        public ObservableCollection<CInOutPrm> Inputs { get; private set; } = new ObservableCollection<CInOutPrm>();

        [Category("IO输出|输出")]
        public ObservableCollection<CInOutPrm> Outputs { get;private set; } = new ObservableCollection<CInOutPrm>();

        [Category("轴参数|轴参数")]
        public ObservableCollection<AxisParameter> AxisParameters { get; private set; } = new ObservableCollection<AxisParameter>();

        [Category("坐标参数|坐标参数")]
        public ObservableCollection<CCrdPrm> CrdParameters { get; private set;} = new ObservableCollection<CCrdPrm>();

        [Category("P2P参数|P2P参数")]
        public ObservableCollection<P2PPrm> P2PPrameters { get; private set; } = new ObservableCollection<P2PPrm>();

        [Category("气缸参数")]
        public ObservableCollection<CylinderConfig> CylinderConfigs { get; private set; } = new ObservableCollection<CylinderConfig>();

    }

 
}
