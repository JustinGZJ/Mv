using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using System.Linq;
using Unity;
using MotionWrapper;

namespace Mv.Modules.Axis.ViewModels
{
    public class FollowAxisViewModel : BindableBase
    {
        private readonly IGtsMotion motion;
        IMotionPart5 part5;
        public SingleAxisViewModel MasterAxis { get; set; }
        public SingleAxisViewModel SlaveAxis { get; set; }

        private readonly MachineBase manager;

        public FollowAxisViewModel(IUnityContainer unityContainer, IGtsMotion motion)
        {

            MasterAxis = new SingleAxisViewModel(unityContainer)
            {
                SelectedAxisRef = motion.AxisRefs.FirstOrDefault(x => x.Name == "SP")
            };
            SlaveAxis = new SingleAxisViewModel(unityContainer)
            {
                SelectedAxisRef = motion.AxisRefs.FirstOrDefault(x => x.Name == "Y")
            };
            manager = unityContainer.Resolve<MachineBase>(nameof(MachineManager));
            this.motion = motion;
            part5 = motion;
        }

        private DelegateCommand cmdStart;
        public DelegateCommand CmdStart =>
            cmdStart ?? (cmdStart = new DelegateCommand(ExecuteCmdStart));

        public ObservableCollection<CCamData> Datas { get; } = new ObservableCollection<CCamData>();

        void ExecuteCmdStart()
        {
            manager.Start();
        }

        private DelegateCommand<CCamData> delegateCommand;
        public DelegateCommand<CCamData> DelegateCommand =>
                delegateCommand ??= new DelegateCommand<CCamData>(ExecuteDelegateCommand);

        void ExecuteDelegateCommand(CCamData obj)
        {
            Datas.Remove(obj);
        }






    }
}
