using MotionWrapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Mv.Modules.Axis
{
    public class InoutSelecter : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var fe = container as FrameworkElement;
            var obj = item as IoRef;
            DataTemplate dt = null;
            if (obj != null && fe != null)
            {
                if (obj.Prm.IoType==EIoType.NomalOutput|| obj.Prm.IoType == EIoType.ServoOn||obj.Prm.IoType==EIoType.clrSts)
                    dt = fe.FindResource("Output") as DataTemplate;
                else
                    dt = fe.FindResource("Input") as DataTemplate;

            }
            return dt;
        }
    }
}
