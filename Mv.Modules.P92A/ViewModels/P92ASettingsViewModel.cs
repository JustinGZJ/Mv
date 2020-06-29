using Mv.Core.Interfaces;
using Mv.Modules.P92A.Views;
using Mv.Ui.Mvvm;
using Prism.Commands;
using Unity;

namespace Mv.Modules.P92A.ViewModels
{
    public class P92ASetting
    {
        public string LineNo { get; set; } = "L3002";
        public string Station { get; set; }= "Winding";
        public string MachineCode { get; set; } = "TZ-05";
    }

    public class P92ASettingsViewModel : ViewModelBase
    {
        private readonly IConfigureFile configure;

        public P92ASetting Config { get; set; }

        public P92ASettingsViewModel(IUnityContainer container,IConfigureFile configure) : base(container)
        {
            this.configure = configure;
            Config=configure.GetValue<P92ASetting>(nameof(P92ASetting))??new P92ASetting();
        }
        public void Save()
        {
            configure.SetValue(nameof(P92ASetting), Config);
        }

        private DelegateCommand saveCmd;
        public DelegateCommand SaveCmd =>saveCmd ?? (saveCmd = new DelegateCommand(Save));

    }
}
