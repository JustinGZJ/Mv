using MV.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mv.Modules.Axis.ViewModels
{
    public class DashboardViewModel : IMainTab
    {
        public string DisplayName { get; } = "Dashboard";
        public object PackIconKind => MaterialDesignThemes.Wpf.PackIconKind.MonitorDashboard;
    }
}
