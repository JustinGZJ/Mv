using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
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

namespace Mv.Modules.RD402.ViewModels
{
    public class Rd402ComponentViewModel : ViewModelBase, IViewLoadedAndUnloadedAware<Rd402Component>
    {
        private readonly IConfigureFile _configure;
        private readonly DeviceReadWriter _device;

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

        public Rd402ComponentViewModel(IUnityContainer container, DeviceReadWriter device, IConfigureFile configure
        ) : base(container)
        {
            _device = device;
            _configure = configure;
            _configure.ValueChanged += _configure_ValueChanged;
            _config = configure.GetValue<RD402Config>(nameof(RD402Config)) ?? new RD402Config();


            Task.Run(() =>
                {
                    while (true)
                    {
                        if (Application.Current != null)
                            Application.Current.Dispatcher?.BeginInvoke(() =>
                            {
                                for (var i = 0; i < 16; i++) Obs[i] = device.GetBit(0, i);

                                for (var i = 0; i < 16; i++) Outs[i] = device.GetSetBit(0, i);

                                IsConnected = device.IsConnected;

                                Spindle = ToStringBase36(device.GetWord(1));
                            });

                        Thread.Sleep(100);
                    }
                }
            );
            short tick = 0;

            Task.Run(
                async () =>
                {
                    while (true)
                    {
                        if (_device.GetBit(0, 0))
                        {
                            AddMsg("获取二维码...");
                            var tick = Environment.TickCount;
                            var result = await GetMatrixCode();

                            AddMsg($"耗时:{Environment.TickCount - tick} ms");
                            if (!result)
                            {
                                AddMsg("Fail");
                                _device.SetBit(0, 2, false);
                            }
                            else
                            {
                                AddMsg("Success");
                                tick = Environment.TickCount;
                                AddMsg($"开始写入二维码: {MatrixCode}");
                                tick = Environment.TickCount;
                                if (!await WritePrinterCode())
                                {
                                    AddMsg("Fail");
                                    _device.SetBit(0, 2, false);
                                }
                                else
                                {
                                    _device.SetString(11, MatrixCode);
                                    AddMsg("Success");
                                    _device.SetBit(0, 2, true);
                                }
                                AddMsg($"耗时 {Environment.TickCount - tick} ms");

                                AddMsg("二维码设置完成...");
                                _device.SetBit(0, 0, true);
                            }
                            SpinWait.SpinUntil(() => !_device.GetBit(0, 0), 2000);
                            _device.SetBit(0, 0, false);
                            _device.SetBit(0, 2, false);
                        }

                        if (_device.GetBit(0, 1))
                        {
                            AddMsg($"start set bar code {Barcode}...");
                            if (Barcode.Length < 7 && _config.Factory == "ICT")
                            {
                                AddMsg("barcode lenth <7.");
                                _device.SetBit(0, 3, false);
                                _device.SetBit(0, 1, true);
                                SpinWait.SpinUntil(() => !_device.GetBit(0, 1), 2000);
                                _device.SetBit(0, 1, false);
                                _device.SetBit(0, 3, false);
                            }
                            else
                            {
                                _device.SetString(23, Barcode);
                                if (await _device.WritePrinterTextAsync(Barcode))
                                {
                                    _device.SetBit(0, 3, true);
                                    AddMsg("Success!");
                                }
                                else
                                {
                                    _device.SetBit(0, 3, false);
                                    AddMsg("Fail!");
                                }

                                AddMsg("条码设置完毕...");
                                _device.SetBit(0, 1, true);
                                SpinWait.SpinUntil(() => !_device.GetBit(0, 1), 2000);
                                _device.SetBit(0, 1, false);
                                _device.SetBit(0, 3, false);
                            }
                        }

                        if (_device.GetBit(0, 4))
                        {
                            AddMsg("开始保存文件...");

                            if (_config.Factory == "ICT")
                            {
                                var machineCode = MachineCode;
                                var axis = Spindle;
                                var station = "STC Winding";
                                var result = _device.GetBit(0, 5) ? "PASS" : "FAIL";
                                var hashtable = new Dictionary<string, string>();
                                hashtable["SN"] = MachineCode;
                                hashtable["Time"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                hashtable["Machine_number"] = _config.MachineNumber;
                                hashtable["Mandrel_number"] = axis;
                                hashtable["Station"] = station;
                                hashtable["Result"] = result;
                                AddMsg(string.Join(',', hashtable.Values));
                                var saveResult = SaveFile(Path.Combine(_config.FileDir, MachineCode + ".csv"), hashtable);
                                _device.SetBit(0, 5, saveResult);
                            }
                            else if (_config.Factory == "信维")
                            {
                                var hashtable = new Dictionary<string, string>();
                                hashtable["MI"] = "Sunway";
                                hashtable["Station"] = _config.Station;
                                hashtable["Project"] = _config.Project;
                                hashtable["Stage"] = _config.Stage;
                                hashtable["Model"] = _config.Model;
                                hashtable["Config"] = _config.Config;
                                hashtable["Test Time"] = DateTime.Now.ToString();
                                hashtable["Fail Item"] = _device.GetBit(0, 5) ? "" : "Barcode NG"; ;
                                hashtable["Fixture"] = "";
                                hashtable["Cavity"] = "";
                                hashtable["Mandrel number"] = _config.MachineNumber;
                                hashtable["Winding spindle"] = Spindle;
                                hashtable["STC SN"] = MatrixCode.Split('+')[0];
                                hashtable["Coil SN"] = "";
                                hashtable["FG SN"] = "";
                                AddMsg(string.Join(',', hashtable.Values));
                                var saveResult = SaveFile(Path.Combine(_config.FileDir, DateTime.Today.ToString("yyyy-MM-dd") + ".csv"), hashtable);
                                _device.SetBit(0, 5, saveResult);
                            }
                            AddMsg("文件保存完毕");
                            _device.SetBit(0, 4, true);
                            SpinWait.SpinUntil(() => !_device.GetBit(0, 4), 2000);
                            _device.SetBit(0, 4, false);
                            _device.SetBit(0, 5, false);
                        }

                        if (tick >= 15) tick = 0;
                        _device.SetShort(10, tick++);
                        Thread.Sleep(1);
                    }
                });
        }

        public ObservableCollection<string> Msg { get; } = new ObservableCollection<string>();

        public ObservableCollection<BindableWrapper<bool>> Obs { get; } =
            new ObservableCollection<BindableWrapper<bool>>
            {
                false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
                false
            };

        public ObservableCollection<BindableWrapper<bool>> Outs { get; } =
            new ObservableCollection<BindableWrapper<bool>>
            {
                false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
                false
            };

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
                if (_config.Factory == "ICT")
                {
                    LineCode = "0" + value.Substring(18, 1);
                    Vendor = value.Substring(19, 1);
                    DayOfWeek = value.Substring(6, 1);
                    WireConfig = value.Substring(21, 1);
                }
                else
                {
                    Barcode = _2dcode.Substring(_2dcode.Length - 4);
                }

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
            set
            {
                _config.Mo = value;
                RaisePropertyChanged(nameof(Mo));
            }
        }

        public string CoilWinding
        {
            get => string.IsNullOrEmpty(coilWinding) ? _config.CoilWinding : coilWinding;
            set
            {
                SetProperty(ref coilWinding, value);
                _config.CoilWinding = value;
                _configure.SetValue(nameof(RD402Config), _config);
            }
        }

        public string Station
        {
            get => string.IsNullOrEmpty(station) ? _config.Station : station;
            set
            {
                SetProperty(ref station, value);
                _config.Station = value;
                _configure.SetValue(nameof(RD402Config), _config);
            }
        }


        public string Spindle
        {
            get => string.IsNullOrEmpty(_spindle) ? "0" : _spindle;
            set
            {
                if (SetProperty(ref _spindle, value) && _config.Factory == "ICT")
                    Barcode = $"{LineCode}{MachineCode}{Spindle}{DayOfWeek}{Vendor}{WireConfig}";
            }
        }

        public string DayOfWeek
        {
            get => _dayOfWeek;
            set
            {
                if (SetProperty(ref _dayOfWeek, value) && _config.Factory == "ICT")
                    Barcode = $"{LineCode}{MachineCode}{Spindle}{DayOfWeek}{Vendor}{WireConfig}";
            }
        }

        public string Barcode
        {
            get => barcode;
            set => SetProperty(ref barcode, value);
        }

        public string LineNumber
        {
            get => _config.LineNumber;
            set
            {
                _config.LineNumber = value;
                RaisePropertyChanged(nameof(LineNumber));
            }
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

        public bool SaveFile(string fileName, Dictionary<string, string> hashtable)
        {
            try
            {
                var dir = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                if (!File.Exists(fileName))
                {
                    var header = string.Join(',', hashtable.Keys).Trim(',');
                    File.AppendAllText(fileName, header + Environment.NewLine);
                }

                var content = string.Join(',', hashtable.Values).Trim(',');
                File.AppendAllText(fileName, content + Environment.NewLine);
                return true;
            }
            catch (Exception ex)
            {
                AddMsg(ex.Message);
                return false;
            }
        }

        private void _configure_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.KeyName == nameof(RD402Config))
            {
                _config = _configure.GetValue<RD402Config>(nameof(RD402Config));
                RaisePropertyChanged(nameof(LineNumber));
                RaisePropertyChanged(nameof(Mo));
                RaisePropertyChanged(nameof(MachineCode));
                RaisePropertyChanged(nameof(Factory));
                if (_config.Factory == "ICT")
                    Barcode = $"{LineCode}{MachineCode}{Spindle}{DayOfWeek}{Vendor}{WireConfig}";
            }
        }

        public static string ToStringBase36(int value)
        {
            if (value >= 0 && value < 10)
                return value.ToString();
            return Chr(Encoding.ASCII.GetBytes("A")[0] + (value - 10));
        }


        #region 设置条码命令

        private DelegateCommand _setbarcodeCommand;

        public DelegateCommand SetBarcodeCommand =>
            _setbarcodeCommand ??= new DelegateCommand(async () =>
                await SetBarcode());

        private async Task SetBarcode()
        {
            if (await _device.WritePrinterTextAsync(Barcode))
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
            var result = await GetMatrixCode();
        }

        public static string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                var asciiEncoding = new ASCIIEncoding();
                byte[] byteArray = { (byte)asciiCode };
                var strCharacter = asciiEncoding.GetString(byteArray);
                return strCharacter;
            }

            throw new Exception("ASCII Code is not valid.");
        }


        private async Task<bool> GetMatrixCode()
        {
            var hashtable = new Hashtable();
            hashtable["lineNumber"] = LineNumber;
            hashtable["station"] = Station;
            hashtable["machineNO"] = _config.MachineNumber;
            hashtable["softwareVER"] = _config.SoftwareVER;
            hashtable["moName"] = Mo;
            hashtable["coilWinding"] = CoilWinding;
            hashtable["axis"] = ToStringBase36(_device.GetWord(1)); ;

            var result = await _device.GetMatrixCodeAsync(hashtable);
            AddMsg($"{MvUser.Username}:获取二维码{result.Item2}.");
            if (!result.Item1) return false;

            dispatcher.Invoke(() => { MatrixCode = result.Item2; });
            AddMsg($"{MvUser.Username}:二维码:{MatrixCode}.");
            return true;
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
            _set2dcodeCommand ??= new DelegateCommand(async () => await ExecuteSet2DodeCommand());

        private async Task ExecuteSet2DodeCommand()
        {
            await WritePrinterCode();
        }

        private async Task<bool> WritePrinterCode()
        {
            if (await _device.WritePrinterCodeAsync(MatrixCode))
            {
                AddMsg($"{MvUser.Username}: set matrix code {MatrixCode},ok");
                return true;
            }

            AddMsg($"{MvUser.Username}: set matrix code {MatrixCode},fail");
            return false;
        }

        #endregion

        private Rd402Component _view;
        public void OnLoaded(Rd402Component view)
        {
            view = _view;
            // throw new NotImplementedException();
        }

        public void OnUnloaded(Rd402Component view)
        {
            //  throw new NotImplementedException();
        }
    }
}