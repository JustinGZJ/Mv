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
        private CInOutPrm prm = new CInOutPrm();
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
                    Prm = item;
                    return true;
                }
            }
            return false;
        }


        public bool Value { get => value; set => this.value = value; }
        public string Name { get => name; set => name = value; }
        public CInOutPrm Prm { get => prm; set => prm = value; }
    }

    public static class IoRefEx
    {
        public static bool GetValue(this IoRef io, IIoPart1 ioPart)
        {
            return true;
        }
    }
}
