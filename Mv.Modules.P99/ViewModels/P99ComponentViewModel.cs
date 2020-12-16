using Microsoft.Xaml.Behaviors.Media;
using Mv.Core.Interfaces;
using Mv.Modules.P99.Service;
using Mv.Ui.Core;
using Mv.Ui.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity;


namespace Mv.Modules.P99.ViewModels
{
    public class P99ComponentViewModel : ViewModelBase
    {
        private readonly IPlcScannerComm plcScanner;
        private readonly IConfigureFile configureFile;
        private readonly IScannerComm scannerComm;
        private readonly IEpson2Cognex epson2Cognex;
        private readonly IUpload uploader;

        public ObservableCollection<BindableWrapper<string>> SupportRingSNs { get; set; } = new ObservableCollection<BindableWrapper<string>>(Enumerable.Repeat(new BindableWrapper<string>() { Value = "" }, 4));
        public ObservableCollection<BindableWrapper<string>> MandrelNO { get; set; } = new ObservableCollection<BindableWrapper<string>>(Enumerable.Repeat(new BindableWrapper<string>() { Value = "" }, 4));
        public BindableWrapper<bool> IsConnected { get; set; } = new BindableWrapper<bool>();
        public BindableWrapper<bool> IsConnected2 { get; set; } = new BindableWrapper<bool>();
        public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();
        public BindableWrapper<bool> Trigger { get; set; } = new BindableWrapper<bool>();

        private DelegateCommand saveData;
        public DelegateCommand SaveCmd =>
            saveData ?? (saveData = new DelegateCommand(SaveData));

        private void AddMessage(string Msg)
        {
            this.Invoke(() =>
            {
                Logger.Log(Msg, Prism.Logging.Category.Debug, Prism.Logging.Priority.None);
                Msg = DateTime.Now.ToString() + " " + Msg;
                Messages.Add(Msg);
                if (Messages.Count > 1000)
                {
                    Messages.RemoveAt(0);
                }
            });

        }

        private bool VerifyCode(string code)
        {
            if (string.IsNullOrEmpty(code.Trim('\0')) || code.StartsWith("NG"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        

        public P99ComponentViewModel(IUnityContainer container, IDeviceReadWriter device,IPlcScannerComm plcScanner, IConfigureFile configureFile,IScannerComm scannerComm, IEpson2Cognex epson2Cognex,IUpload uploader) : base(container)
        {
            this.plcScanner = plcScanner;
            this.configureFile = configureFile;
            this.scannerComm = scannerComm;
            this.epson2Cognex = epson2Cognex;
            this.uploader = uploader;
            EventAggregator.GetEvent<MessageEvent>().Subscribe((x) => Invoke(() => {
                AddMessage(x);
            }));
            var m = configureFile.GetValue<P99Config>(nameof(P99Config));
            if (m == null)
            {
                configureFile.SetValue(nameof(P99Config), new P99Config());
            }
          
            Task.Factory.StartNew(() =>
            {
                short itick = 0;
                while (true)
                {
                    this.Invoke(() =>
                    {

                        for (int i = 0; i < 4; i++)
                        {
                            SupportRingSNs[i] = device.GetString(20 + i * 20, 20).Trim('\0');
                            MandrelNO[i] = device.GetString(100 + i * 20, 20).Trim('\0');
                        }
                        Trigger = (device.GetWord(0) > 0);
                        IsConnected.Value = device.IsConnected;
                        IsConnected2.Value = plcScanner.IsConnected;
                        device.SetShort(1, itick++);
                        if (itick >= 10)
                            itick = 0;
                    });
                    Thread.Sleep(1);
                }
            }, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(() =>
            {
              
                while (true)
                {
                    if (Trigger)
                    {
                        AddMessage("收到保存数据信号");
                        device.SetShort(0, 1);
                        AddMessage("置位保存数据输出");
                        try
                        {
                            SaveData();
                        }
                        catch (Exception ex)
                        {
                            AddMessage(ex.Message);
                         //   throw;
                        }
                        AddMessage("数据保存完成，等待触发数据保存信号输入关闭");
                        SpinWait.SpinUntil(() => (Trigger == false), 2000);
                        AddMessage("数据保存信号输入已关闭，关闭数据保存信号输出");
                        device.SetShort(0, 0);
                    }
                    Thread.Sleep(1);
                }
            });

        }

        private void SaveData()
        {
            for (int i = 0; i < 4; i++)
            {
                var dic = new Dictionary<string, string>();
                dic["Time"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dic["Support ring SN"] = SupportRingSNs[i].Value;
                dic["Spindle NO."] = configureFile.GetValue<P99Config>(nameof(P99Config)).MachineNo + "_" + (i + 1).ToString();
                dic["Mandrel NO."] = MandrelNO[i].Value;
                dic["Result"] = ((VerifyCode(SupportRingSNs[i].Value) && VerifyCode(MandrelNO[i].Value))) ? "PASS" : "FAIL";
                var content = string.Join(',', dic.Values).Trim(',');
                string fileName = Path.Combine(configureFile.GetValue<P99Config>(nameof(P99Config)).SaveDir,
                    (VerifyCode(SupportRingSNs[i].Value) ? SupportRingSNs[i].Value : "Empty" + Helper.GetTimeStamp().ToString()) + ".csv");
                AddMessage(content);
                var res = uploader.Upload(SupportRingSNs[i], dic);
                AddMessage(res);
                  Helper.SaveFile(fileName, dic);

            }
        }
    }
}
