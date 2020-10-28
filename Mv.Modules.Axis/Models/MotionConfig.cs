using System.Collections.Generic;

namespace MotionWrapper
{
    public class MotionConfig
    {
        public List<CInputOutputPrm> i { get; private set; } = new List<CInputOutputPrm>();
        public List<CInputOutputPrm> o { get;private set; } = new List<CInputOutputPrm>();

        public List<AxisParameter> prms { get; private set; } = new List<AxisParameter>();
        public List<CCrdPrm> crdS { get; private set;} = new List<CCrdPrm>();

        public List<P2PPrm> p2Ps { get; private set; } = new List<P2PPrm>();
    }

 
}
