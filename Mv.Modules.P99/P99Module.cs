using Mv.Modules.P99.Service;
using Mv.Modules.P99.Views;
using Mv.Ui.Core;
using Mv.Ui.Core.Modularity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;

namespace Mv.Modules.P99
{
    public class P99Module : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public P99Module(IUnityContainer container, IRegionManager regionManager) : base(container)
        {
            _regionManager = regionManager;
        
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(P99Component));
            _regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(P99Settings));
            containerRegistry.RegisterSingleton<IDeviceReadWriter, ModbusDeviceReadWriter>();
        }
    }
}