using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Mv.Core.Interfaces;
using Mv.Modules.RD402.Service;
using Mv.Modules.RD402.Views;
using Mv.Ui.Core;
using Mv.Ui.Mvvm;
using Prism.Commands;
using Prism.Logging;
using Unity;
using ZXing;
using ZXing.Common;
using ZXing.Presentation;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mv.Modules.RD402.ViewModels
{

    public static class RD402Helper
    {
        public static bool SaveFile(string fileName, Dictionary<string, string> hashtable)
        {
            try
            {
                var dir = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                if (!File.Exists(fileName))
                {
                    var header = string.Join(',', hashtable.Keys).Trim(',') + Environment.NewLine;
                    var content = string.Join(',', hashtable.Values).Trim(',') + Environment.NewLine;
                    File.AppendAllText(fileName, header + content);
                }
                else
                {
                    var content = string.Join(',', hashtable.Values).Trim(',');
                    File.AppendAllText(fileName, content + Environment.NewLine);
                }
                return true;
            }
            catch (Exception ex)
            {
                //    AddMsg(ex.Message);
                return false;
            }
        }
    }


    public class Rd402ComponentViewModel : ViewModelBase
    {
        private readonly IConfigureFile _configure;
        private IFactoryInfo factoryInfo;
        private readonly IDeviceReadWriter _device;
        private readonly IInkPrinter inkPrinter;
        private string _2dcode;

        private RD402Config _config;
        private string _dayOfWeek;
        private bool _isConnected;
        private string _linecode;
        private string _machineCode;
        private string _spindle;
        private string _vendor;
        private string _wireConfig;
        private string barcode;
        private string coilWinding;
        private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private string station;

        public ObservableCollection<string> PLCCodes { get;  } = new ObservableCollection<string>(Enumerable.Repeat("",4));

        public Rd402ComponentViewModel(IUnityContainer container, IDeviceReadWriter device, IInkPrinter inkPrinter, IConfigureFile configure
        ) : base(container)
        {
            _device = device;
            this.inkPrinter = inkPrinter;
            _configure = configure;
            _configure.ValueChanged += _configure_ValueChanged;
            _config = configure.GetValue<RD402Config>(nameof(RD402Config)) ?? new RD402Config();

            this.factoryInfo = Container.Resolve<IFactoryInfo>(_config.Factory);

            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        if (Application.Current != null)
                            Application.Current.Dispatcher?.BeginInvoke(() =>
                            {
                                for (var i = 0; i < 16; i++) Obs[i] = device.GetBit(0, i);
                                for (var i = 0; i < 16; i++) Outs[i] = device.GetSetBit(0, i);
                                IsConnected = device.IsConnected;
                                Spindle = factoryInfo.GetSpindle(device.GetWord(1));
                                ReadCodes();
                            });

                        Thread.Sleep(100);
                    }
                },
          cancellationToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            short tick = 0;
            Task.Factory.StartNew(
                async () =>
                {
                    while (true)
                    {
                        if (_device.GetBit(0, 0))
                        {
                            AddMsg("check station...");
                            var tick = Environment.TickCount;
                            var result = await CheckStation().ConfigureAwait(false);
                            AddMsg($"耗时:{Environment.TickCount - tick} ms");
                            if (!result.Item1)
                            {
                                AddMsg("Fail");
                            }
                            else
                            {
                                AddMsg("Success");              
                            }
                            var results = result.Item2.ToArray();
                            for (int i = 0; i < 4; i++)
                            {
                                _device.SetBit(0, 8 + i, results[i]);
                            }
                            AddMsg($"耗时 {Environment.TickCount - tick} ms");
                            _device.SetBit(0, 0, true);
                            AddMsg("station check done!");
                            SpinWait.SpinUntil(() => !_device.GetBit(0, 0), 2000);
                            _device.SetBit(0, 0, false);
                            for (int i = 0; i < 4; i++)
                            {
                                _device.SetBit(0, 8 + i,false);
                            }
                        }
                        if (tick >= 15) tick = 0;
                        _device.SetShort(49, tick++);
                        Thread.Sleep(1);
                        //    AddMsg(tick.ToString());
                    }
                }, cancellationToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public ObservableCollection<string> Msg { get; } = new ObservableCollection<string>();

        public ObservableCollection<BindableWrapper<bool>> Obs { get; } =
            new ObservableCollection<BindableWrapper<bool>>(Enumerable.Repeat(new BindableWrapper<bool>() { Value = false }, 16));

        public ObservableCollection<BindableWrapper<bool>> Outs { get; } =
            new ObservableCollection<BindableWrapper<bool>>(Enumerable.Repeat(new BindableWrapper<bool>() { Value = false }, 16));

        public string LineCode
        {
            get => _linecode;
            set
            {
                SetProperty(ref _linecode, value);
                Barcode = $"{LineCode}{MachineCode}{Spindle}{DayOfWeek}{Vendor}{WireConfig}";
            }
        }

        public string Factory => _config.Factory;

        public Geometry Codeimage { get; private set; }

        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        public string MachineCode
        {
            get => string.IsNullOrEmpty(_machineCode) ? _config.MachineCode : _machineCode;
            set
            {
                if (SetProperty(ref _machineCode, value) && _config.Factory == "ICT")
                {
                    _config.MachineCode = value;
                    Barcode = $"{LineCode}{MachineCode}{Spindle}{DayOfWeek}{Vendor}{WireConfig}";
                }
            }
        }

        public string Vendor
        {
            get => string.IsNullOrEmpty(_vendor) ? _config.WireVendor : _vendor;
            set
            {
                if (SetProperty(ref _vendor, value) && _config.Factory == "ICT")
                    Barcode = $"{LineCode}{MachineCode}{Spindle}{DayOfWeek}{Vendor}{WireConfig}";
            }
        }

        public string MatrixCode
        {
            get => _2dcode;
            set
            {
                if (!SetProperty(ref _2dcode, value)) return;
                Barcode = factoryInfo.GetBarcode(_2dcode, _config, _device.GetWord(1));
                dispatcher.BeginInvoke(() =>
                {
                    var writer = new BarcodeWriterGeometry
                    {
                        Format = BarcodeFormat.DATA_MATRIX,
                        Options = new EncodingOptions
                        {
                            Height = 80,
                            Width = 80,
                            Margin = 0
                        }
                    };
                    var image = writer.Write(_2dcode);
                    Codeimage = image;
                    RaisePropertyChanged(nameof(Codeimage));
                });
            }
        }

        public string WireConfig
        {
            get => string.IsNullOrEmpty(_wireConfig) ? _config.WireConfig : _wireConfig;
            set
            {
                if (SetProperty(ref _wireConfig, value))
                    Barcode = $"{LineCode}{MachineCode}{Spindle}{DayOfWeek}{Vendor}{WireConfig}";
            }
        }

        public string Mo
        {
            get => _config.Mo;
        }

        public string CoilWinding
        {
            get =>  _config.CoilWinding;
        }

        public string Station
        {
            get =>  _config.Station;
        }


        public string Spindle
        {
            get => string.IsNullOrEmpty(_spindle) ? "0" : _spindle;
            set
            {
                SetProperty(ref _spindle, value);
            }
        }

        public string DayOfWeek
        {
            get => _dayOfWeek;
        }

        public string Barcode
        {
            get => barcode;
            set => SetProperty(ref barcode, value);
        }

        public string LineNumber
        {
            get => _config.LineNumber;
        }


        private void AddMsg(string message)
        {
            var msg = $"{DateTime.Now.ToString()} {message}";
            dispatcher.Invoke(() =>
            {
                if (Msg.Count > 100) Msg.RemoveAt(0);
                Msg.Add(msg);
            });
            Logger.Log(msg, Category.Info, Priority.None);
        }



        private void _configure_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.KeyName != nameof(RD402Config)) return;
            var config = _configure.GetValue<RD402Config>(nameof(RD402Config));
            if (_config.Factory != config.Factory)
            {
                factoryInfo = Container.Resolve<IFactoryInfo>(config.Factory);        
            }
            RaisePropertyChanged(nameof(LineNumber));
            RaisePropertyChanged(nameof(Mo));
            RaisePropertyChanged(nameof(MachineCode));
            RaisePropertyChanged(nameof(Factory));
            Barcode = factoryInfo.GetBarcode(MatrixCode, config, _device.GetWord(1));
            _config = config;
            RaisePropertyChanged(nameof(Factory));
        }



        #region 设置条码命令

        private DelegateCommand _setbarcodeCommand;

        public DelegateCommand SetBarcodeCommand =>
            _setbarcodeCommand ??= new DelegateCommand(async () =>
                await SetBarcode().ConfigureAwait(false));

        private async Task SetBarcode()
        {
            if (await inkPrinter.WritePrinterTextAsync(Barcode).ConfigureAwait(false))
            {
                AddMsg($"{MvUser.Username}:设置条码成功.");
            }
            else
            {
                var msg = $"{MvUser.Username}:设置条码成功.";
                AddMsg(msg);
            }
        }

        #endregion

        #region 获取二维码命令

        private DelegateCommand _get2dcodeCommand;

        public DelegateCommand Get2DodeCommand =>
            _get2dcodeCommand ??= new DelegateCommand(ExecuteGet2DodeCommand);

        private async void ExecuteGet2DodeCommand()
        {
            var result = await CheckStation().ConfigureAwait(true);
        }

        private void ReadCodes()
        {
            var code1 = _device.GetString(10, 10).Trim('\0').Trim();
            var code2 = _device.GetString(15, 10).Trim('\0').Trim();
            var code3 = _device.GetString(20, 10).Trim('\0').Trim();
            var code4 = _device.GetString(25, 10).Trim('\0').Trim();
            PLCCodes.Clear();
            PLCCodes.AddRange(new string[] { code1, code2, code3, code4 });
        }


        private async Task<(bool,IEnumerable<bool>)> CheckStation()
        {
            bool[] results = new bool[4];
            var code1= _device.GetString(10, 10).Trim('\0').Trim();
            var code2 = _device.GetString(15,10).Trim('\0').Trim();
            var code3 = _device.GetString(20, 10).Trim('\0').Trim();
            var code4 = _device.GetString(25, 10).Trim('\0').Trim();
            var result = await Task.Run(()=>factoryInfo.CheckStation(new string[] { code1, code2, code3, code4 })).ConfigureAwait(false);
            AddMsg($"{MvUser.Username}:station check {result.Item2}.");
            if ((!result.Item1)&&(!result.Item2.Contains("SFC_OK",StringComparison.CurrentCulture))) return (false,results);
            var partern = @"::\w{4}::(\w{4});(\w{4});(\w{4});(\w{4})";
            Regex regex = new Regex(partern, RegexOptions.IgnoreCase);
           var match =regex.Match(result.Item2);
        
            if(match.Success)
            {
                for (int i = 0; i < 4; i++)
                {
                    results[i]=match.Groups[1 + i].Value.Contains("PASS",StringComparison.CurrentCulture);
                }
            }
            
            return (true,results);
        }

        #endregion


        #region 设置PLC位输出状态切换命令

        private DelegateCommand<string> _setOutputCommand;

        public DelegateCommand<string> SetOutputCommand =>
            _setOutputCommand ??= new DelegateCommand<string>(ExecuteSetOutputCommand,
                m => { return (int)MvUser.Role >= (int)MvRole.Admin; });

        private void ExecuteSetOutputCommand(string index)
        {
            _device.SetBit(0, int.Parse(index), !_device.GetSetBit(0, int.Parse(index)));
        }
        #endregion

        #region 设置二维码到PLC命令

        private DelegateCommand _set2dcodeCommand;

        public DelegateCommand Set2DodeCommand =>
            _set2dcodeCommand ??= new DelegateCommand(async () => await ExecuteSet2DodeCommand().ConfigureAwait(false));

        private async Task ExecuteSet2DodeCommand()
        {
            await WritePrinterCode().ConfigureAwait(false);
        }

        private async Task<bool> WritePrinterCode()
        {
            if (await inkPrinter.WritePrinterCodeAsync(MatrixCode).ConfigureAwait(false))
            {
                AddMsg($"{MvUser.Username}: set matrix code {MatrixCode},ok");
                return true;
            }

            AddMsg($"{MvUser.Username}: set matrix code {MatrixCode},fail");
            return false;
        }

        #endregion

    }


}