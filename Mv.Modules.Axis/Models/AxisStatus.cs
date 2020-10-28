using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mv.Modules.Axis.Models
{
    public class AxisStatus:BindableBase
    {
        private int index;
        public int Index
        {
            get { return index; }
            set { SetProperty(ref index, value); }
        }

        private string axisName;
        public string AxisName
        {
            get { return axisName; }
            set { SetProperty(ref axisName, value); }
        }

        private double crrentEncoder;
        public double CrrentEncoder
        {
            get { return crrentEncoder; }
            set { SetProperty(ref crrentEncoder, value); }
        }

        private double targetPulse;
        public double TargetPulse
        {
            get { return targetPulse; }
            set { SetProperty(ref targetPulse, value); }
        }

    }
}
