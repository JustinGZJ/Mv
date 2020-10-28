namespace MotionWrapper
{
    /// <summary>
    /// 轴的设置参数
    /// </summary>
    public class CAxisSetPrm
    {
        private bool enableLmtN = false;
        private float softLmtN = 0;//软限位的设置
        private bool enableLmtP;
        private bool enableAlm;
        private float softLmtP;

        public bool EnableLmtP { get => enableLmtP; set => enableLmtP = value; }
        public bool EnableAlm { get => enableAlm; set => enableAlm = value; }
        public float SoftLmtP { get => softLmtP; set => softLmtP = value; }
        public bool EnableLmtN { get => enableLmtN; set => enableLmtN = value; }
        public float SoftLmtN { get => softLmtN; set => softLmtN = value; }
    }
}
