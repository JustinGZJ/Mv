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
        public const int maxDiNum = 1024, maxDoNum = 1024, maxAxisNum = 32;
        //数据区域 尽量做到16个一组 
        private volatile bool[] mdis = new bool[maxDiNum];
        private volatile bool[] mdos = new bool[maxDoNum];
        private volatile AxisRef[] axisRefs = new AxisRef[maxAxisNum];
        public CMotionData()
        {
            for (int i = 0; i < maxAxisNum; i++)
            {
                AxisRefs[i] = new AxisRef("");
            }
        }
        public AxisRef[] AxisRefs { get => axisRefs;  }
        public bool[] Mdos { get => mdos; set => mdos = value; }
        public bool[] Mdis { get => mdis; set => mdis = value; }
    }


}
