using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using Mv.Core.Interfaces;

namespace MotionWrapper
{
    /// <summary>
    /// 所有控制器都应该具备的数据
    /// </summary>
    public class CMotionData : ICMotionData
    {
        public const int maxDiNum = 100, maxDoNum = 100, maxAxisNum = 32;
        //数据区域 尽量做到16个一组 
        private volatile bool[] mdis = new bool[maxDiNum];
        private volatile bool[] mdos = new bool[maxDoNum];
        private volatile IoRef[] dos = new IoRef[maxDoNum];
        private volatile IoRef[] dis = new IoRef[maxDiNum];
        private volatile AxisRef[] axisRefs = new AxisRef[maxAxisNum];
        public CMotionData()
        {
            for (int i = 0; i < maxAxisNum; i++)
            {
                AxisRefs[i] = new AxisRef("");
            }
            for (int i = 0; i < maxDiNum; i++)
            {
                Dis[i] = new IoRef("");
            }
            for (int i = 0; i < maxDoNum; i++)
            {
                Dos[i] = new IoRef("");
            }
        }
        public AxisRef[] AxisRefs { get => axisRefs;  }
        public bool[] Mdos { get => mdos; set => mdos = value; }
        public bool[] Mdis { get => mdis; set => mdis = value; }
        public IoRef[] Dos { get => dos; set => dos = value; }
        public IoRef[] Dis { get => dis; set => dis = value; }
    }


}
