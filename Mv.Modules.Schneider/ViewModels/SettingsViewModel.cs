using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Mv.Core.Interfaces;
using Mv.Modules.Schneider.Views;
using Mv.Ui.Mvvm;
using Prism;
using Unity;

namespace Mv.Modules.Schneider.ViewModels
{
    public class ScheiderConfig
    {
        public string ServerIP { get; set; } = "192.168.0.101";
        public string Station { get; set; } = "ST030";

    }
    public class SettingsViewModel : ViewModelValidateBase,IViewLoadedAndUnloadedAware<Settings>
    {
        private readonly IConfigureFile configureFile;
        ScheiderConfig config=null;

        public SettingsViewModel(IUnityContainer container, IConfigureFile configureFile) : base(container)
        {
            this.configureFile = configureFile;
            config = configureFile.GetValue<ScheiderConfig>(nameof(ScheiderConfig));
        }
        string serverIP;
        [Required]
        [RegularExpression(@"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$", ErrorMessage = "地址格式不符！")]
        public string ServerIP
        {
            get => config.ServerIP; set
            {
                if (SetProperty(ref serverIP, value))
                {
                    config.ServerIP = value;
                    configureFile.SetValue(nameof(ScheiderConfig), config);
                }
            }
        }
        string station;
        [Required]
        public string Station
        {
            get => config.Station; set
            {
                if (SetProperty(ref station, value))
                {
                    config.Station = value;
                    configureFile.SetValue(nameof(ScheiderConfig), config);
                }
            }
        }

        public void OnLoaded(Settings view)
        {
           // throw new NotImplementedException();
        }

        public void OnUnloaded(Settings view)
        {
            //  throw new NotImplementedException();
            configureFile.SetValue(nameof(ScheiderConfig), config);
        }
    }
}
