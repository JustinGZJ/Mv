using PropertyChanged;
using System.Collections.Generic;

namespace MotionWrapper
{
    /// <summary>
    /// IO的一个映射 程序中所有都用这个
    /// </summary>
    /// 
    [AddINotifyPropertyChangedInterface]
    public class IoRef
    {
        private bool value = false;
        private string name = "";
        public IoRef(string name)
        {
            this.Name = name;
        }
        /// <summary>
        /// 通过ID来指定IO表中的
        /// </summary>
        /// <param name="id"></param>
        public bool setIoPrm(List<CInOutPrm> io)
        {
            foreach (var item in io)
            {
                if(Name == item.Name)
                {
                    prm = item;
                    return true;
                }
            }
            return false;
        }
        public CInOutPrm prm = new CInOutPrm();

        public bool Value { get => value; set => this.value = value; }
        public string Name { get => name; set => name = value; }
    }
}
