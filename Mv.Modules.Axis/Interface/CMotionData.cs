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
    public class CMotionData
    {
        public const int maxDiNum = 1024, maxDoNum = 1024, maxAxisNum = 32;
        //数据区域 尽量做到16个一组 
        public volatile bool[] mdis = new bool[maxDiNum];
        public volatile bool[] mdos = new bool[maxDoNum];
        public volatile AxisRef[] axisRefs = new AxisRef[maxAxisNum];
        public CMotionData()
        {
            for (int i = 0; i < maxAxisNum; i++)
            {
                axisRefs[i] = new AxisRef("");
            }
        }
    }

 
}
