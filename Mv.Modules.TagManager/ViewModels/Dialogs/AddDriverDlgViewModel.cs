﻿using BatchCoreService;
using DataService;
using Mv.Core;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Mv.Modules.TagManager.ViewModels.Dialogs
{

    public class DriverInfo
    {
        public string Assembly { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
    }
    public class AddDriverDlgViewModel : BindableBase
    {
        public Driver Driver { get; private set; } = new Driver();

        public ObservableCollection<DriverInfo> DriverInfos { get; set; } = new ObservableCollection<DriverInfo>();

        private string name;
        public string DriverName
        {
            get { return name; }
            set
            {
                if (SetProperty(ref name, value))
                {
                    Driver.Name = value;
                }
            }
        }

        private DriverInfo selectedValue;
        public DriverInfo SelectedValue
        {
            get { return selectedValue; }
            set
            {
                if (SetProperty(ref selectedValue, value))
                {
                    Driver.ClassName = selectedValue.ClassName;
                    Driver.Assembly = selectedValue.Assembly;
                    Driver.Description = selectedValue.Description;
                }
            }
        }
        public AddDriverDlgViewModel()
        {
            var drivers = Directory.GetFiles(MvFolders.Drivers, "*.dll")
                  .Select(x => GetAssembly(x))
                  .SelectMany(m => m.GetTypes())
                  .Where(t => t.GetInterfaces().Contains(typeof(IDriver)))
                  .Where(t => !t.IsInterface)
                  .Select(t => new DriverInfo
                  {
                      Assembly = t.Assembly.Location,
                      ClassName = t.FullName,
                      Description = (t.GetCustomAttribute(typeof(DescriptionAttribute), true) as DescriptionAttribute)?.Description
                  });
            DriverInfos.AddRange(drivers);
            SelectedValue = DriverInfos.FirstOrDefault();
        }

        private Assembly GetAssembly(string filename)
        {
            try
            {
                return Assembly.LoadFrom(filename);
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
