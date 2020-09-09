using Mv.Core.Interfaces;
using Mv.Modules.Schneider.Service;
using Mv.Modules.Schneider.ViewModels;
using Mv.Modules.Schneider.Views;
using Mv.Ui.Core;
using Mv.Ui.Core.Modularity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;

namespace Mv.Modules.Schneider
{
   // [ModuleDependency("TagManager")]
    public class SchneiderModule : ModuleBase
    {
        private readonly IRegionManager regionManager;

        public SchneiderModule(IUnityContainer container, IRegionManager _regionManager) : base(container)
        {
            IConfigureFile configureFile = container.Resolve<IConfigureFile>();
            if (!configureFile.Contains(nameof(ScheiderConfig)))
            {
                configureFile.SetValue(nameof(ScheiderConfig), new ScheiderConfig());
            }
            regionManager = _regionManager;
        }
        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
      
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Container.RegisterType<IServerOperations, ServerOperations>();
            regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(Dashboard));
            regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(Settings));

        }
    }
}