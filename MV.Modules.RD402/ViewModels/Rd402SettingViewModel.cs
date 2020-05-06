using System;
using System.Collections.Generic;
using System.Text;
using Mv.Core.Interfaces;
using Mv.Modules.RD402.Service;
using Mv.Ui.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Unity;

namespace Mv.Modules.RD402.ViewModels
{


    public class Rd402SettingViewModel : ViewModelBase
    {
        public RD402Config Config { get; }
        private DelegateCommand _cmdSave;
        private DeviceReadWriter _device;

        public DelegateCommand SaveCommand =>
            _cmdSave ??= new DelegateCommand(SaveConfig);

        private IConfigureFile _configure;

        void SaveConfig()
        {
            _configure.SetValue(nameof(RD402Config), Config);
            _device.PlcConnect();
        }

        public Rd402SettingViewModel(IUnityContainer container, IConfigureFile configure, DeviceReadWriter device) :
            base(container)
        {
            _configure = configure;
            Config = configure.GetValue<RD402Config>(nameof(RD402Config)) ?? new RD402Config();
            _device = device;
        }

        public string PlcIpAddress
        {
            get => Config.PLCIpAddress;
            set => Config.PLCIpAddress = value;
        }


        public int PlcPort
        {
            get => Config.PLCPort;
            set => Config.PLCPort = value;
        }

        public string PrinterIpAddress
        {
            get => Config.PrinterIpAddress;
            set => Config.PrinterIpAddress = value;
        }

        public int PrinterPort
        {
            get => Config.PrinterPort;
            set => Config.PrinterPort = value;
        }

        public ushort ReadLens
        {
            get => Config.ReadLens;
            set => Config.ReadLens = value;
        }

        public string ReadAddrStart
        {
            get => Config.ReadAddrStart;
            set => Config.ReadAddrStart = value;
        }

        public ushort WirteLens
        {
            get => Config.WriteLens;
            set => Config.WriteLens = value;
        }

        public string WriteAddrStart
        {
            get => Config.WriteAddrStart;
            set => Config.WriteAddrStart = value;
        }


        public string Mo
        {
            get => Config.Mo;
            set => Config.Mo = value;
        }

        public string LineNumber
        {
            get => Config.LineNumber;
            set => Config.LineNumber = value;
        }

        public string MachineCode
        {
            get => Config.MachineCode;
            set => Config.MachineCode = value;
        }

        public string UploadData
        {
            get => Config.FileDir;
            set => Config.FileDir = value;
        }

        public string MachineNumberFile
        {
            get => Config.MachineNumber;
            set => Config.MachineNumber = value;
        }

        public string Factory
        {
            get => Config.Factory;
            set
            {
                Config.Factory = value;
                RaisePropertyChanged(nameof(Factory));
            }
        }

        public static string[] Factories => new[] { "ICT", "信维", "调试" };

        public string coilWinding
        {
            get => Config.CoilWinding;
            set => Config.CoilWinding = value;
        }
        public string Station
        {
            get => Config.Station;
            set => Config.Station = value;
        }
        public string SoftwareVER
        {
            get => Config.SoftwareVER;
            set => Config.SoftwareVER = value;
        }
    }
}
