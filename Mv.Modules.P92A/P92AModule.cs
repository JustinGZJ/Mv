using Mv.Modules.P92A.Service;
using Mv.Modules.P92A.Views;
using Mv.Ui.Core;
using Mv.Ui.Core.Modularity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;

namespace Mv.Modules.P92A
{
    public class P92AModule : ModuleBase
    {
        private readonly IRegionManager regionManager;

        public P92AModule(IUnityContainer container, IRegionManager regionManager) : base(container)
        {
            this.regionManager = regionManager;
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDeviceReadWriter, MCDeviceReadWriter>();
            containerRegistry.RegisterSingleton<IAlarmService, AlarmService>();
            containerRegistry.RegisterSingleton<ICE012, CE012>();
            regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(Alarms));

        }
    }
}