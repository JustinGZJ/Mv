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
        [Description("通用输入"), DefaultValue("通用输入")]
        NoamlInput = mc.MC_GPI,
        [Description("通用输出"), DefaultValue("通用输出")]
        NomalOutput = mc.MC_GPO,
        [Description("报警"), DefaultValue("报警")]
        Alarm = mc.MC_ALARM,
        [Description("正限位"), DefaultValue("正限位")]
        LimitP = mc.MC_LIMIT_POSITIVE,
        [Description("负限位"), DefaultValue("负限位")]
        LimitN = mc.MC_LIMIT_NEGATIVE,
        [Description("回零"), DefaultValue("回零")]
        Home = mc.MC_HOME,
        [Description("到位"), DefaultValue("到位")]
        Arrive = mc.MC_ARRIVE,
        [Description("伺服使能"), DefaultValue("伺服使能")]
        ServoOn = mc.MC_ENABLE,
        [Description("清除状态"), DefaultValue("清除状态")]
        clrSts = mc.MC_CLEAR,

    }
    [TypeConverter(typeof(EnumDefaultValueTypeConverter))]
    public enum EHomeMode
    {
        [Description("限位回原点 "), DefaultValue("限位回原点 ")]
        HOME_MODE_LIMIT = 10,
        [Description("限位+Home回原点 "), DefaultValue("限位+Home回原点 ")]
        HOME_MODE_LIMIT_HOME = 11,
        [Description("限位+Index 回原点"), DefaultValue("限位+Index 回原点")]
        HOME_MODE_LIMIT_INDEX = 12,
        [Description("限位+Home+Index 回原点"), DefaultValue("限位+Home+Index 回原点")]
        HOME_MODE_LIMIT_HOME_INDEX = 13,
        [Description("Home 回原点"), DefaultValue("Home 回原点")]
        HOME_MODE_HOME = 20,
        [Description("Home+Index 回原点"), DefaultValue("Home+Index 回原点")]
        HOME_MODE_HOME_INDEX = 22,
        [Description("Index 回原点"), DefaultValue("Index 回原点")]
        HOME_MODE_INDEX = 30,
        [Description("强制 Home 回原点"), DefaultValue("强制 Home 回原点")]
        HOME_MODE_FORCED_HOME = 40,
        [Description("强制 Home+Index 回原点"), DefaultValue("强制 Home+Index 回原点")]
        HOME_MODE_FORCED_HOME_INDEX = 41,
    }
}