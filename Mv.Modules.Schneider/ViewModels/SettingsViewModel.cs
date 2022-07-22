using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [Category("配置")]
        [DisplayName("服务器IP")]
        public string ServerIP { get; set; } = "192.168.0.201";
        [Category("配置")]
        [DisplayName("站号")]
        public string Station { get; set; } = "ST030";

        [Category("配置")]
        [DisplayName("上传禁用")]
        public bool ServerDisable { get; set; }

        [Category("配置")]
        [DisplayName("采集频率")]
        public int Freq { get; set; } = 400;

        public string[] MaterialCodes { get; set; } = Enumerable.Repeat("", 8).ToArray();

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

        }

        public void OnUnloaded(Settings view)
        {
            configureFile.SetValue(nameof(ScheiderConfig), Config);
        }
    }
}
