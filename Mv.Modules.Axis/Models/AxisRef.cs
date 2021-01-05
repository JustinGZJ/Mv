using PropertyChanged;
using System.Collections.Generic;
using System.Linq;

namespace MotionWrapper
{
    public static class AxisRefExt
    {
        public static List<P2PPrm> GetP2PPrms(this AxisRef axisRef, List<P2PPrm> p2pList)
        {
            return p2pList.Where(x => x.Name == axisRef.Name).ToList();
        }
    }

    [AddINotifyPropertyChangedInterface]
    //轴的定义
    public class AxisRef
    {
        #region feilds
        //状态
        private string name = "";//轴的名称  会到表格中初始化参数
        private bool servoOn = false;
        private bool alarm = false;
        private bool limitP = false;
        private bool limitN = false;
        private bool followErr = false;
        private bool moving = false;
        private bool inPos = false;
        private bool isHoming = false;//如果在回零过程中 可能会进行回零检测
        private float relVel = 0.0f;
        private short homed = 0;//0初始化 1 成功 -1失败
        private bool homeSwitch = false;//回零开关是否接通
        private float cmdPos;
        private float relPos;
        private float cmdVel;

        private float belif = 1f;
        //参数
        private AxisParameter prm = new AxisParameter();
        #endregion
        public AxisRef(string _name)
        {
            Name = _name;
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
                if (item.Name == Name)
                {
                    this.Prm = item;
                    return true;
                }
            }
            return false;
        }


        public bool ServoOn { get => servoOn; set => servoOn = value; }
        public bool Alarm { get => alarm; set => alarm = value; }
        public bool LimitP { get => limitP; set => limitP = value; }
        public bool LimitN { get => limitN; set => limitN = value; }
        public bool FollowErr { get => followErr; set => followErr = value; }
        public bool Moving { get => moving; set => moving = value; }
        public bool InPos { get => inPos; set => inPos = value; }
        public bool IsHoming { get => isHoming; set => isHoming = value; }
        public float CmdPos { get => cmdPos; set => cmdPos = value; }
        public float RelPos { get => relPos; set => relPos = value; }
        public float CmdVel { get => cmdVel; set => cmdVel = value; }
        public float RelVel { get => relVel; set => relVel = value; }
        public short Homed { get => homed; set => homed = value; }
        public bool HomeSwitch { get => homeSwitch; set => homeSwitch = value; }

        public bool AtHome { get; set; }
        public string Name { get => name; set => name = value; }
        public AxisParameter Prm { get => prm; set => prm = value; }
        public float Rate { get => belif; set => belif = value; }
    };
}
