using Mv.Modules.TagManager.Views;
using Mv.Ui.Mvvm;
using Prism.Mvvm;
using Prism.Regions;

namespace Mv.Modules.TagManager.ViewModels
{
    public class TagEditorViewModel : BindableBase, IViewLoadedAndUnloadedAware<TagEditor>
    {
        private readonly IRegionManager regionManager;

        public TagEditorViewModel(IRegionManager _regionManager)
        {
            regionManager = _regionManager;
        }

        public void OnLoaded(TagEditor view)
        {
            regionManager.RequestNavigate("TAG_CONTENT", nameof(DriverMonitor));
            //  throw new NotImplementedException();
        }

        public void OnUnloaded(TagEditor view)
        {
            //  throw new NotImplementedException();
        }
    }
}
