using Mv.Core.Interfaces;
using Mv.Ui.Mvvm;
using Prism.Commands;
using Unity;

namespace Mv.Modules.P99.ViewModels
{
    public class P99SettingsViewModel : ViewModelBase
    {
        private readonly IConfigureFile configureFile;

        private DelegateCommand _saveConfig;

        public P99SettingsViewModel(IUnityContainer container, IConfigureFile configureFile) : base(container)
        {
            this.configureFile = configureFile;
            Config = configureFile.GetValue<P99Config>(nameof(P99Config)) ?? new P99Config();
        }

        public P99Config Config { get; set; }
        public DelegateCommand SaveCommand =>
            _saveConfig ?? (_saveConfig = new DelegateCommand(Save));

        public void Save()
        {
            configureFile.SetValue<P99Config>(nameof(P99Config), Config);
        }
    }
}