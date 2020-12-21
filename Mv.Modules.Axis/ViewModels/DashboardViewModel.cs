using MV.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using PropertyChanged;

namespace Mv.Modules.Axis.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class DashboardViewModel : IMainTab
    {
        public string DisplayName { get; } = "Dashboard";
        public object PackIconKind => MaterialDesignThemes.Wpf.PackIconKind.MonitorDashboard;
        /// <summary>
        /// 利用率
        /// </summary>
        public double Availability { get; set; } =0.88;
        /// <summary>
        /// 稼动率
        /// </summary>
        public double Performance { get; set; } = 0.77;
        /// <summary>
        /// 良率
        /// </summary>
        public double Quality { get; set; } =0.88;
        /// <summary>
        /// 综合指标
        /// </summary>
        public double OEE =>Availability*Performance*Quality;

        public Func<double, string> LabelFormatter => x => (x).ToString("P1");

       
    }
}
