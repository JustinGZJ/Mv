using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Category("参数")]
        [DisplayName("型号")]
        public string Type { get; set; }

        [Category("参数")]
        [DisplayName("型号")]
        public string Size { get; set; }
        [Category("参数")]
        [DisplayName("客户")]
        public string Customer { get; set; }

        [Category("参数")]
        [DisplayName("毛重")]
        public string GrossWeight { get; set; }
        [Category("参数")]
        [DisplayName("净重")]
        public string Suttle { get; set; }

        [Category("参数")]
        [DisplayName("编号")]
        public string SerialNumber { get; set; }

        [Category("参数")]
        [DisplayName("日期")]
        public DateTime Date { get; set; }

        [Category("参数")]
        [DisplayName("机头号")]
        public DateTime Nose { get; set; }

        [Category("参数")]
        [DisplayName("工作号")]
        public DateTime JobNumber { get; set; }


        [Category("参数")]
        [DisplayName("上线时间")]
        public DateTime Online { get; set; }

        [Category("参数")]
        [DisplayName("下线时间")]
        public DateTime Offline { get; set; }

        [Category("参数")]
        [DisplayName("批号")]
        public string BatchNumber { get; set; }

        [Category("配置")]
        [DisplayName("服务器IP")]
        public string ServerIP { get; set; } = "192.168.0.201";
        [Category("配置")]
        [DisplayName("站号")]
        public string Station { get; set; } = "ST030";

        [Category("配置")]
        [DisplayName("上传禁用")]
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
