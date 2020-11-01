using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mv.Modules.Axis
{
    public class ControlEx
    {
        public static bool GetAttachedState(DependencyObject obj)
        {
            return (bool)obj.GetValue(AttachedStateProperty);
        }

        public static void SetAttachedState(DependencyObject obj, bool value)
        {
            obj.SetValue(AttachedStateProperty, value);
        }

        public static readonly DependencyProperty AttachedStateProperty =
            DependencyProperty.RegisterAttached("AttachedState", typeof(bool), typeof(ControlEx), new PropertyMetadata(false, (s, e) =>
            {
                if (s is Control control)
                {
                    if (!(bool)e.NewValue)
                    {
                        control.Foreground = Brushes.Gray;
                    }              
                    else
                    {
                        control.Foreground = Brushes.Lime;
                    }
                }
            }));


    }
}
