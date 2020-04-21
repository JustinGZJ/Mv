namespace Mv.Modules.RD402.Service
{
    public class RD402Config
    {
        public string PrinterIpAddress { get; set; } = "192.168.1.240";
        public int PrinterPort { get; set; } = 5000;
        public string PLCIpAddress { get; set; } = "192.168.1.10";
        public int PLCPort { get; set; } = 3600;
        public string WriteAddrStart { get; set; } = "D3700";
        public string ReadAddrStart { get; set; } = "D3600";
        public ushort WriteLens { get; set; } = 50;
        public ushort ReadLens { get; set; } = 50;
        public string LineNo { get; set; } = "01";
        public string MachineCode { get; set; } = "1";
        public string WireVendor { get; set; } = "T";
        public string WireConfig { get; set; } = "E";

        public string SoftwareVER { get; set; } = "1.0.0";

        public string CoilWinding { get; set; } = "1";

        public string Station { get; set; } = "1";

        public string LineNumber { get; set; } = "BU21-B009";

        public string Mo { get; set; } = "H5109-200400087";

        public string FileDir { get; set; } = @"D:\Data";

        public string MachineNumber { get; set; } = "TZ-1";

        public string Factory { get; set; } = "ICT";//ICT 信维

        public string Project { get; set; } = "CE012";
        public string Stage { get; set; } = "EVT";
        public string Model { get; set; } = "D53";
        public string Config { get; set; } = "SW1";
    }
}
