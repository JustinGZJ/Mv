namespace MotionWrapper
{
    public class CFtriger
    {
        private bool oldvalue = false;
        public bool Result(bool value)
        {
            if (value == true)
            {
                oldvalue = true;
            }
            if (oldvalue == true && value == false)
            {
                oldvalue = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
