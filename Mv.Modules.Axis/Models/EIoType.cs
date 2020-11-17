using System.ComponentModel;
using GTN;
using Mv.Ui.Core;

namespace MotionWrapper
{
    /// <summary>
    /// IO的类型
    /// </summary>
    ///
     [TypeConverter(typeof(EnumDefaultValueTypeConverter))]
    public enum EIoType
    {
        [Description("通用输入"),DefaultValue("通用输入")]
        NoamlInput = mc.MC_GPI,
        [Description("通用输出"),DefaultValue("通用输出")]
        NomalOutput = mc.MC_GPO,
        [Description("报警"),DefaultValue("报警")]
        Alarm = mc.MC_ALARM,
        [Description("正限位"),DefaultValue("正限位")]
        LimitP = mc.MC_LIMIT_POSITIVE,
        [Description("负限位"),DefaultValue("负限位")]
        LimitN = mc.MC_LIMIT_NEGATIVE,
        [Description("回零"),DefaultValue("回零")]
        Home = mc.MC_HOME,
        [Description("到位"),DefaultValue("到位")]
        Arrive = mc.MC_ARRIVE,
        [Description("伺服使能"),DefaultValue("伺服使能")]
        ServoOn = mc.MC_ENABLE,
        [Description("清除状态"),DefaultValue("清除状态")]
        clrSts = mc.MC_CLEAR,

    }
}