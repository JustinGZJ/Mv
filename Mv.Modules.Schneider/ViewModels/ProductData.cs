using System;
using System.Collections.Generic;
using static Mv.Modules.Schneider.ViewModels.GlobalValues;

namespace Mv.Modules.Schneider.ViewModels
{
    public static class GlobalValues
    {
        public const int AXISCNT= 4;
    }
   
    public class ProductDataCollection
    {

        public ProductDataCollection()
        {
            ProductDatas = new List<ProductData>(AXISCNT);
            for (int i = 0; i < AXISCNT; i++)
            {
                ProductDatas.Add(new ProductData(i));
            }
        }
        public List<ProductData> ProductDatas { get; set; }
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

        public string Type { get; set; }

        public string Size { get; set; }
        public string Customer { get; set; }
        public string GrossWeight { get; set; }
        public string Suttle { get; set; }

        public string SerialNumber { get; set; }

        public DateTime Date { get; set; }

        public DateTime Nose { get; set; }


        public DateTime JobNumber { get; set; }


        public DateTime Online { get; set; }

        public DateTime Offline { get; set; }

        public string BatchNumber { get; set; }
    }

    public class ProductData
    {
        public ProductData(int index)
        {
            TensionGroups = new List<TensionGroup>();
            for (int i = 0; i < 2; i++)
            {
                TensionGroups.Add(new TensionGroup() { Name = $"tension{i*AXISCNT + 1+index}" });
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
        public ICollection<TensionGroup> TensionGroups { get; set; }

        public class TensionGroup
        {
            public TensionGroup()
            {
                Values = new List<Tension>();
            }
            public string Name { get; set; }
            public double UpperLimit { get; set; }
            public double LowerLimit { get; set; }
            public ICollection<Tension> Values { get; set; }
        }
        public class Tension
        {
            public DateTime Time { get; set; } = DateTime.Now;
            public short Value { get; set; }
        }
    }
}
