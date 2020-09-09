using Mv.Modules.Hmi.Service;
using Mv.Modules.Hmi.Views;
using Mv.Ui.Core;
using Mv.Ui.Core.Modularity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;

namespace Mv.Modules.Hmi
{
    public class HmiModule : ModuleBase
    {
        private readonly IRegionManager regionManager;

        public HmiModule(IUnityContainer container,IRegionManager _regionManager) :base(container)
        {
            regionManager = _regionManager;
        }
  
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(Dashboard));
            containerRegistry.RegisterInstance<IDevice>(new TcpDevice("192.168.250.204", 4196), "presure1");
            containerRegistry.RegisterInstance<IDevice>(new TcpDevice("192.168.250.205", 4196), "presure2");
            containerRegistry.RegisterInstance<IDevice>(new TcpDevice("192.168.250.207", 4196), "resistance");
            containerRegistry.RegisterInstance<IDevice>(new TcpDevice("192.168.250.199", 64000), "position");

        }
    }
}