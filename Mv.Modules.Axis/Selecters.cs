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
                if (obj.Prm.IoType==EIoType.NomalOutput||obj.Prm.IoType==EIoType.clrSts)
                    dt = fe.FindResource("Output") as DataTemplate;
                else
                    dt = fe.FindResource("Input") as DataTemplate;

            }
            return dt;
        }
    }

    public class CylinderSelecter:DataTemplateSelector
    {

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var fe = container as FrameworkElement;
            DataTemplate dt = null;
            if (item != null && fe != null)
            {
                if (item is ABCylinder)
                {
                    dt = fe.FindResource("abcylinder") as DataTemplate;
                }
                else if(item is Cylinder)
                {
                    dt = fe.FindResource("cylinder") as DataTemplate;
                }
                else if(item is OneSignalCylinder)
                {
                    dt = fe.FindResource("onesignalcylinder") as DataTemplate;
                }

            }
            return dt;
        }
    }
}
