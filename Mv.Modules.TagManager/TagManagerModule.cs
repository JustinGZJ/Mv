using Mv.Modules.TagManager.Views;
using Mv.Ui.Core;
using Mv.Ui.Core.Modularity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;

namespace Mv.Modules.TagManager
{
    public class TagManagerModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public TagManagerModule(IUnityContainer container, IRegionManager regionManager) : base(container)
        {
            _regionManager = regionManager;
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
     //       containerregistry.registersingleton<idevicereadwriter, modbusdevicereadwriter>();
            _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(TagEditor));

        }
    }
}