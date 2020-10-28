using System;
using System.Collections.Generic;
using static Mv.Modules.Schneider.ViewModels.GlobalValues;

namespace Mv.Modules.Schneider.ViewModels
{
    public static class GlobalValues
    {
        public const int AXISCNT= 4;
    }
   
    public class UploadDataCollection
    {

        public UploadDataCollection()
        {
            UploadDatas = new List<UploadData>(AXISCNT);
            for (int i = 0; i < AXISCNT; i++)
            {
                UploadDatas.Add(new UploadData(i));
            }
        }
        public List<UploadData> UploadDatas { get; set; }
    }
    public class ServerData
    {
        public List<string> Codes { get; set; }
        public ServerData()
        {
            Codes = new List<string>();
        }
        public int Status { get; set; }
        public double LoopTime { get; set; }
        public int Quantity { get; set; }
        public int Velocity { get; set; }
        public int Angle { get; set; }
        public int Turns { get; set; }
        public string Program { get; set; } = "";
    }

    public class UploadData
    {
        public UploadData(int index)
        {
            Tensions = new List<Group>();
            for (int i = 0; i < 2; i++)
            {
                Tensions.Add(new Group() { Name = $"tension{i*AXISCNT + 1+index}" });
            }
        }
        public string Code { get; set; } 
        public int Status { get; set; }
        public double LoopTime { get; set; }
        public int Quantity { get; set; }
        public int Velocity { get; set; }
        public int Angle { get; set; }
        public int Turns { get; set; }
        public string Program { get; set; } = "";
        public ICollection<Group> Tensions { get; set; }

        public class Group
        {
            public Group()
            {
                Values = new List<Data>();
            }
            public string Name { get; set; }
            public double UpperLimit { get; set; }
            public double LowerLimit { get; set; }
            public bool Result { get; set; }
            public ICollection<Data> Values { get; set; }
        }
        public class Data
        {
            public DateTime Time { get; set; } = DateTime.Now;
            public short Value { get; set; }
        }
    }
}
