namespace MotionWrapper
{
    public interface ICMotionData
    {
        AxisRef[] AxisRefs { get; }
        bool[] Mdis { get; set; }
        bool[] Mdos { get; set; }

        public IoRef[] Dos { get; }
        public IoRef[] Dis { get; }
    }
}