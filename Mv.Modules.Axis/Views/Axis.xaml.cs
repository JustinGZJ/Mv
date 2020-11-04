using Mv.Modules.Axis.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mv.Modules.Axis.Views
{
    /// <summary>
    /// Axis.xaml 的交互逻辑
    /// </summary>
    public partial class Axis : UserControl
    {
        AxisViewModel vm;
        public Axis()
        {
            InitializeComponent();
            
        }

        //private void forward_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    vm =(AxisViewModel)DataContext;
        //    vm.CmdMoveForward.Execute();
        //}

        //private void forward_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    vm = (AxisViewModel)DataContext;
        //    vm.CmdStopMove.Execute();
        //}

        //private void backward_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    vm = (AxisViewModel)DataContext;
        //    vm.CmdMoveBackward.Execute();
        //}

        //private void backward_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    vm = (AxisViewModel)DataContext;
        //    vm.CmdStopMove.Execute();
        //}
    }
}
