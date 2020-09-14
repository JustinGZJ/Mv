using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Mv.Core.Interfaces;
using Mv.Modules.Schneider.Views;
using Mv.Ui.Mvvm;
using Prism;
using Unity;
using PropertyTools.DataAnnotations;

namespace Mv.Modules.Schneider.ViewModels
{
    public class ScheiderConfig
    {
        [Category("配置")]
        public string ServerIP { get; set; } = "192.168.0.201";
        [Category("配置")]
        public string Station { get; set; } = "ST030";

        [Category("配置")]
        public bool ServerDisable { get; set; } 

    }
    public class SettingsViewModel : ViewModelValidateBase,IViewLoadedAndUnloadedAware<Settings>
    {
        private readonly IConfigureFile configureFile;
        ScheiderConfig config=null;

        public SettingsViewModel(IUnityContainer container, IConfigureFile configureFile) : base(container)
        {
            this.configureFile = configureFile;
            Config = configureFile.GetValue<ScheiderConfig>(nameof(ScheiderConfig));
        }

        public ScheiderConfig Config { get => config; set => config = value; }

        public void OnLoaded(Settings view)
        {
           // throw new NotImplementedException();
        }

        public void OnUnloaded(Settings view)
        {
            //  throw new NotImplementedException();
            configureFile.SetValue(nameof(ScheiderConfig), Config);
        }
    }
}
