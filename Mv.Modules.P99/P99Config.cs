using System;
using System.Collections.Generic;
using System.Text;

namespace Mv.Modules.P99
{
    public class P99Config
    {
        public string MachineNo { get; set; } = "1";
        public string SaveDir { get; set; } = @"D:\DATA";
        public string UvLightIp { get; set; } = "192.168.1.16";
        public int UvLightPort { get; set; } = 8000;
        public string Station { get; set; } = "T0479";
    }
}
