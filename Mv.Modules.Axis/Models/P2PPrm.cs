using Mv.Ui.Core;
using PropertyChanged;
using System.ComponentModel;

namespace MotionWrapper
{
    [AddINotifyPropertyChangedInterface]
    public class P2PPrm
    {
        public string Name { get; set; }
        public string AxisName { get; set; }
        public double Position { get; set; }
        public double Velocity { get; set; }
        public double Acceleration { get; set; }
    }


    [AddINotifyPropertyChangedInterface]
    public class CylinderConfig
    {
        public string Name { get; set; }
        public CylinderType Type { get; set; }
        public string OUT1 { get; set; }
        public string OUT2 { get; set; }
        public string IN1 { get; set; }
        public string IN2 { get; set; }
    }
    [TypeConverter(typeof(EnumDefaultValueTypeConverter))]
    public enum CylinderType
    {
        [Description("AB 双入双出"), DefaultValue("AB 双入双出")]
        O2I2,
        [Description("A 单入单出"), DefaultValue("A 单入单出")]
        O1I1,
        [Description("A 单出双入"), DefaultValue("A 单出双入")]
        O1I2,
        [Description("A 单出"), DefaultValue("A 单出")]
        O1I0
    }

  
}
