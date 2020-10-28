namespace MotionWrapper
{
    /// <summary>
    /// 产生一个下降沿
    /// </summary>
    public class CRtriger
    {
        private bool oldvalue = true;
        public bool Result(bool value)
        {
            {
                oldvalue = false;
            }
            if (oldvalue == false && value == true)
            {
                oldvalue = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
