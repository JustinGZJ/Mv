using Mv.Ui.Mvvm;
using Prism.Regions;
using Unity;

namespace Mv.Shell.ViewModels.Dialogs
{
    public class SettingsDialogViewModel : ViewModelBase
    {
        public SettingsDialogViewModel(IUnityContainer container, IRegionManager regionManager) : base(container)
        {
            RegionManager = regionManager;
        }

        public IRegionManager RegionManager { get; }
    }

}
