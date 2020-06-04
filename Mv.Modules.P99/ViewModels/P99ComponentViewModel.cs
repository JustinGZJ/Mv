using Mv.Modules.P99.Service;
using Mv.Ui.Core;
using Mv.Ui.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity;


namespace Mv.Modules.P99.ViewModels
{
    public class P99ComponentViewModel : ViewModelBase
    {
        public ObservableCollection<BindableWrapper<string>> SupportRingSNs { get; set; } = new ObservableCollection<BindableWrapper<string>>(Enumerable.Repeat(new BindableWrapper<string>() { Value=""} , 4));
        public ObservableCollection<BindableWrapper<string>> MandrelNO { get; set; } = new ObservableCollection<BindableWrapper<string>>(Enumerable.Repeat(new BindableWrapper<string>() { Value=""}, 4));
        public BindableWrapper<bool> IsConnected { get; set; } = new BindableWrapper<bool>();

        public BindableWrapper<bool> Trigger { get; set; } = new BindableWrapper<bool>();
        public P99ComponentViewModel(IUnityContainer container, IDeviceReadWriter device) :base(container)
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    this.Invoke(() =>
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            SupportRingSNs[i] = device.GetString(10 + i * 10, 20);
                            MandrelNO[i] = device.GetString(50 + i * 10, 20);
                        }
                        Trigger = (device.GetWord(0) > 0);
                        IsConnected = device.IsConnected;
                    });
                    Thread.Sleep(1);
                }
            }, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(() =>
            {
                while(true)
                {
                    if(Trigger)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            var dic = new Dictionary<string, string>();
                            dic["Time"] = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
                            dic["Support ring SN"] = SupportRingSNs[i].Value;
                            dic["Spindle NO."] = (i + 1).ToString();
                            dic["Mandrel NO."] = MandrelNO[i].Value;
                            dic["Result"] = ((!SupportRingSNs[i].Value.Contains("NG")) && (!MandrelNO[i].Value.Contains("NG"))) ? "PASS" : "FAIL";
                            Helper.SaveFile("D:\\data\\"+SupportRingSNs[i].Value + ".csv",dic);
                        }
                        device.SetShort(0, 1);
                        SpinWait.SpinUntil(() => (Trigger == false),2000);
                        device.SetShort(0, 0);
                    }
                    Thread.Sleep(1);
                }
            });
        }
    }
}
