using Mv.Modules.Axis.Service;
using Mv.Modules.Axis.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Mv.Modules.Axis
{
    public class AxisModule : IModule
    {
        private readonly IRegionManager regionManager;

        public AxisModule(IRegionManager _regionManager)
        {
            regionManager = _regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var card=containerProvider.Resolve<ICard>();
            card.Init();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICard, CardGoogol>();
            regionManager.RegisterViewWithRegion("APPS", typeof(MainTab));
        }
    }
}