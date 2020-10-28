using PropertyTools.DataAnnotations;
using System.Collections.Generic;

namespace MotionWrapper
{
    public class MotionConfig
    {
        [Category("IO|Inputs")]
        [HeaderPlacement(HeaderPlacement.Above)]
        public List<CInputOutputPrm> i { get; private set; } = new List<CInputOutputPrm>();

        [Category("IO|Outputs")]
        [HeaderPlacement(HeaderPlacement.Above)]
        public List<CInputOutputPrm> o { get;private set; } = new List<CInputOutputPrm>();

        [Category("IO|Para")]
        [HeaderPlacement(HeaderPlacement.Above)]
        public List<AxisParameter> prms { get; private set; } = new List<AxisParameter>();

        [Category("IO|Crd")]
        [HeaderPlacement(HeaderPlacement.Above)]
        public List<CCrdPrm> crdS { get; private set;} = new List<CCrdPrm>();

        [Category("IO|P2P")]
        [HeaderPlacement(HeaderPlacement.Above)]
        public List<P2PPrm> p2Ps { get; private set; } = new List<P2PPrm>();
    }

 
}
