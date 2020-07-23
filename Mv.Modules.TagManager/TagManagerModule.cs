using BatchCoreService;
using DataService;
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
        private readonly IRegionManager _regionManager;

        public TagManagerModule(IUnityContainer container,IRegionManager regionManager) : base(container)
        {

            _regionManager = regionManager;
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            this.Container.RegisterType<IDriverDataContext, DriverDataContext>();
            this.Container.RegisterSingleton<IDataServer, DAService>();
            containerRegistry.RegisterForNavigation<DriverConfiger>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(TagEditor));

        }
        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
           
        }
    }
}