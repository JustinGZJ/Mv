using System.Collections.Generic;

namespace MotionWrapper
{
    //轴的定义
    public class AxisRef
    {
        //状态
        public string name = "";//轴的名称  会到表格中初始化参数
        public bool servoOn = false;
        public bool alarm = false;
        public bool limitP = false;
        public bool limitN = false;
        public bool followErr = false;
        public bool moving = false;
        public bool inPos = false; 
        public bool isHoming = false;//如果在回零过程中 可能会进行回零检测
        public float cmdPos = 0.0f, relPos = 0.0f, cmdVel = 0.0f, relVel = 0.0f;
        public short homed = 0;//0初始化 1 成功 -1失败
        public bool homeSwitch = false;//回零开关是否接通
        public AxisRef(string _name)
        {
            name = _name;
        }
        /// <summary>
        /// 通过轴名字设置参数
        /// </summary>
        /// <param name="prmList"></param>
        /// <returns></returns>
        public bool setAxisPrm(List<AxisParameter> prmList)
        {
            foreach (var item in prmList)
            {
                if (item.Name == this.name)
                {
                    this.prm = item;
                    return true;
                }
            }
            return false;
        }
        //参数
        public AxisParameter prm = new AxisParameter();
    };
}
