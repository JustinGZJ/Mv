using BatchCoreService;
using Mv.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mv.Modules.TagManager.Services
{

    public class DriverDataContext : IDriverDataContext
    {

        public const string DRIVERS = "DRIVERS";
        private readonly IConfigureFile configure;
        public DriverDataContext(IConfigureFile configure)
        {
            this.configure = configure;
        }

        public IEnumerable<Driver> GetDrivers()
        {
            var drivers = configure.GetValue<IEnumerable<Driver>>(DRIVERS) ?? new List<Driver>();
            return drivers;
        }

        public void SetDrivers(IEnumerable<Driver> drivers)
        {
            configure.SetValue(DRIVERS, drivers);
        }
    }
}
