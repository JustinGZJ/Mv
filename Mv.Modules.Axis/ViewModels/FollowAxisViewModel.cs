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

        private DelegateCommand<CCamData> deletecommand;
        public DelegateCommand<CCamData> DeleteCommand =>
                deletecommand ??= new DelegateCommand<CCamData>(ExecuteDelegateCommand);

        void ExecuteDelegateCommand(CCamData obj)
        {
            Datas.Remove(obj);
        }

        /// <summary>
        /// 添加指令
        /// </summary>
        private DelegateCommand<EMoveType?> addcommand;
        public DelegateCommand<EMoveType?> Addcommand =>
            addcommand ?? (addcommand = new DelegateCommand<EMoveType?>(ExecuteAddcommand));

        void ExecuteAddcommand(EMoveType? value)
        {
            CCamData camData = new CCamData();
            camData.MoveType = value.Value;
            camData.Master = (long)MasterAxis.SelectedAxisRef.Prm.mm2pls(masterdistance);
            camData.Slaver = (long)SlaveAxis.SelectedAxisRef.Prm.mm2pls(slavedistance);
            Datas.Add(camData);
        }


        private DelegateCommand movecommand;
        public DelegateCommand Movecommand =>
            movecommand ?? (movecommand = new DelegateCommand(ExecuteMovecommand));

        void ExecuteMovecommand()
        {
            AxisRef master = MasterAxis.SelectedAxisRef;
            AxisRef slave = SlaveAxis.SelectedAxisRef;
            motion.MC_MoveJog(master);
            motion.MC_FollowMode(master, slave);
            motion.MC_Follow(master, slave, 0, Datas.ToList());
            
          // motion.MC_MoveJog()
        }


        private double masterdistance;
        public double MasterDistance
        {
            get { return masterdistance; }
            set { SetProperty(ref masterdistance, value); }
        }

        private double slavedistance;
        public double SlaveDistance
        {
            get { return slavedistance; }
            set { SetProperty(ref slavedistance, value); }
        }

        private int repeattime;
        public int Repeattime
        {
            get { return repeattime; }
            set { SetProperty(ref repeattime, value); }
        }









    }
}
