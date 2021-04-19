using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Mvvm;
using System.Linq;
using Prism.Commands;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Mv.Core.Interfaces;

namespace Mv.Modules.Schneider.ViewModels
{
    public class PdtInfo : BindableBase
    {

        private bool seleceted;
        public bool Seleceted
        {
            get { return seleceted; }
            set { SetProperty(ref seleceted, value); }
        }

        private string code;
        public string Code
        {
            get { return code; }
            set { SetProperty(ref code, value); }
        }

        private string specs;
        public string Specs
        {
            get { return specs; }
            set { SetProperty(ref specs, value); }
        }

        private string model;
        public string Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private string panelSize;
        public string PanelSize
        {
            get { return panelSize; }
            set { SetProperty(ref panelSize, value); }
        }

        private string machineNo;
        public string MachineNo
        {
            get { return machineNo; }
            set { SetProperty(ref machineNo, value); }
        }

        private string date;
        public string Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }
        }


    }

    public class ProductInfoViewModel : BindableBase
    {
        //匹配名
        Regex RegexCode = new Regex(@"(\d+(?:\.\d+)?)+\/([^\(]+)\((\S{1,3})\)\/([\w\d]+)\/(\d{4}\/\d{1,2}\/\d{1,2})", RegexOptions.IgnoreCase);
        Regex RegexST = new Regex(@"ST(\d+)", RegexOptions.IgnoreCase);
        public ObservableCollection<PdtInfo> Infos { get; }
        private string input;
        public string Input
        {
            get { return input; }
            set { SetProperty(ref input, value); }
        }

        private PdtInfo selectedInfo;
        public PdtInfo SelectedInfo
        {
            get { return selectedInfo; }
            set { SetProperty(ref selectedInfo, value); }
        }

        public ProductInfoViewModel(IConfigureFile configureFile)
        {
            Infos = new ObservableCollection<PdtInfo>(Enumerable.Range(0, 8).Select(x => new PdtInfo()));
            var config = configureFile.GetValue<ScheiderConfig>(nameof(ScheiderConfig));
            for (int i = 0; i < config.MaterialCodes.Length; i++)
            {
                var input = config.MaterialCodes[i];
                var info = Infos[i];
                if (RegexCode.IsMatch(input))
                {
                    var match = RegexCode.Match(input);
                    info.Code = match.Groups[0].Value;
                    info.Specs = match.Groups[1].Value;
                    info.Model = match.Groups[2].Value;
                    info.PanelSize = match.Groups[3].Value;
                    info.MachineNo = match.Groups[4].Value;
                    info.Date = match.Groups[5].Value;
                }
            }

            this.configureFile = configureFile;
        }


        private DelegateCommand<KeyEventArgs> cmdKeyIn;
        private readonly IConfigureFile configureFile;

        public DelegateCommand<KeyEventArgs> CmdKeyIn =>
            cmdKeyIn ?? (cmdKeyIn = new DelegateCommand<KeyEventArgs>(ExecuteCmdKeyIn));

        void ExecuteCmdKeyIn(KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
            {
                if (RegexST.IsMatch(Input))
                {
                    var match = RegexST.Match(Input);
                    var ST = int.Parse(match.Groups[1].Value);
                    if (ST > 0 && ST <= Infos.Count)
                    {
                        if (selectedInfo != null)
                            selectedInfo.Seleceted = false;
                        SelectedInfo = Infos[ST - 1];
                        selectedInfo.Seleceted = true;
                    }
                }
                else if (RegexCode.IsMatch(Input) && SelectedInfo != null)
                {
                    var match = RegexCode.Match(Input);        
                    SelectedInfo.Code = Input;
                    SelectedInfo.Specs = match.Groups[1].Value;
                    SelectedInfo.Model = match.Groups[2].Value;
                    SelectedInfo.PanelSize = match.Groups[3].Value;
                    SelectedInfo.MachineNo = match.Groups[4].Value;
                    SelectedInfo.Date = match.Groups[5].Value;
                    SaveConfig();
                }
                Input = "";
            }

            args.Handled = true;
        }

        private void SaveConfig()
        {
            var config = configureFile.GetValue<ScheiderConfig>(nameof(ScheiderConfig));
            config.MaterialCodes[Infos.IndexOf(SelectedInfo)] = Input;
            configureFile.SetValue(nameof(ScheiderConfig), config);
        }
    }
}
