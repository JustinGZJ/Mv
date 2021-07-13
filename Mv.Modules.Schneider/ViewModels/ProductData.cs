﻿using System;
using System.Collections.Generic;
using System.Linq;
using static Mv.Modules.Schneider.ViewModels.GlobalValues;

namespace Mv.Modules.Schneider.ViewModels
{
    public static class GlobalValues
    {
        public const int AXISCNT = 1;
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
            TensionResut = new int[1];
        }
        public int[] TensionResut { get; }
        public uint TensionOutput { get; set; }
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
        public int HVC { get; set; }
        public uint TensionOutput { get; set; }
        public int[] TensionResults { get; set; }

        public string[] MaterialCodes { get; set; }

    }

    public class ProductData
    {
        public ProductData(int index)
        {
            TensionGroups = new List<TensionGroup>();
            for (int i = 0; i < 1; i++)
            {
                TensionGroups.Add(new TensionGroup() { Name = $"tension{i * AXISCNT + 1 + index}" });
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
        public int HVC { get; set; }
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
            public bool Result => Values.Count == 0 || Values.All(z => z.Value <= UpperLimit && z.Value >= LowerLimit);
            public int iResult
            {
                get
                {
                    if (Values.Count == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return Values.All(z => z.Value <= UpperLimit && z.Value >= LowerLimit) ? -1 : 1;
                    }
                }
            }
            public ICollection<Tension> Values { get; set; }
        }
        public class Tension
        {
            public DateTime Time { get; set; } = DateTime.Now;
            public ushort Value { get; set; }
        }
    }
}
