using System.Collections.Generic;

namespace MotionWrapper
{
    /// <summary>
    /// IO的一个映射 程序中所有都用这个
    /// </summary>
    public class IoRef
    {
        public bool value = false;
        public string id = "";
        public IoRef(string id)
        {
            this.id = id;
        }
        /// <summary>
        /// 通过ID来指定IO表中的
        /// </summary>
        /// <param name="id"></param>
        public bool setIoPrm(List<CInputOutputPrm> io)
        {
            foreach (var item in io)
            {
                if(id == item.ID)
                {
                    prm = item;
                    return true;
                }
            }
            return false;
        }
        public CInputOutputPrm prm = new CInputOutputPrm();
    }
}
