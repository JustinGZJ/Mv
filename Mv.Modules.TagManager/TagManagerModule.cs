using BatchCoreService;
using Mv.Modules.TagManager.Services;
using Mv.Modules.TagManager.Views;
using Mv.Modules.TagManager.Views.Dialogs;
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
        private readonly IContainerRegistry containerRegistry;
        private readonly IRegionManager _regionManager;

        public TagManagerModule(IUnityContainer container, IContainerRegistry containerRegistry,IRegionManager regionManager) : base(container)
        {
            this.containerRegistry = containerRegistry;
            _regionManager = regionManager;
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            this.Container.RegisterType<IDriverDataContext, DriverDataContext>();
            containerRegistry.RegisterForNavigation<DriverConfiger>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(TagEditor));

        }
    }
}