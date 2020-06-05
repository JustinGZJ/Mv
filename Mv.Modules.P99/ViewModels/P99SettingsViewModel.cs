using Mv.Modules.P99.Service;
using Mv.Ui.Mvvm;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Mv.Ui.Core;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Unity;
using Mv.Core.Interfaces;
using Prism.Commands;

namespace Mv.Modules.P99.ViewModels
{
    public class P99SettingsViewModel : ViewModelBase
    {
        private readonly IConfigureFile configureFile;

        public P99SettingsViewModel(IUnityContainer container, IConfigureFile configureFile) : base(container)
        {
            this.configureFile = configureFile;
            Config = configureFile.GetValue<P99Config>(nameof(P99Config)) ?? new P99Config();          
        }

        public P99Config Config { get; set; }

        private DelegateCommand _saveConfig;
        public DelegateCommand SaveCommand =>
            _saveConfig ?? (_saveConfig = new DelegateCommand(Save));
        public void Save()
        {
            configureFile.SetValue<P99Config>(nameof(P99Config), Config);
        }
    }
}
