using System.Collections.Generic;

namespace Mv.Modules.P92A.Service
{
    public interface IAlarmService
    {
        List<AlarmItem> GetAlarmItems();
        void LoadAlarmInfos(string filePath = "./Alarms/Alarms.csv");
    }
}