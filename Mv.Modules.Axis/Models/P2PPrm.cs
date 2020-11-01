using PropertyChanged;

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

 
}
