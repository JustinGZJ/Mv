using System;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;
using System.Linq;
using MotionWrapper;
using System.Collections.Generic;
using Mv.Modules.Axis.Views.Dialog;
using Mv.Core;
using Unity;
using MaterialDesignThemes.Wpf;

namespace Mv.Modules.Axis.ViewModels
{
    public class SingleAxisViewModel : BindableBase
    {

        #region Properties

        public AxisRef SelectedAxisRef
        {
            get => selectedAxisRef;
            set => SetProperty(ref selectedAxisRef, value);
        }
        public List<AxisRef> AxisRefs { get => axisRefs; set => SetProperty(ref axisRefs, value); }
        public double JogDistance { get => jogDistance; set => SetProperty(ref jogDistance, value); }
        public int JogSpeed { get => jogSpeed; set => SetProperty(ref jogSpeed, value); }
        /// <summary>
        /// 连动
        /// </summary>
        public bool ContinueMove
        {
            get { return continuous; }
            set { SetProperty(ref continuous, value); }
        }
        bool _selectable = true;
        public bool Selectable
        {
            get => _selectable;
            set => SetProperty(ref _selectable, value);
        }
        #endregion


        #region commands                                                                

        private DelegateCommand cmdPower;
        public DelegateCommand CmdPower =>
            cmdPower ?? (cmdPower = new DelegateCommand(ExecuteCmdPower, () => SelectedAxisRef != null));

        void ExecuteCmdPower()
        {
            if (!SelectedAxisRef.ServoOn)
                motionPart1.MC_Power(SelectedAxisRef);
            else
                motionPart1.MC_PowerOff(SelectedAxisRef);
        }

        #region 正向移动
        private DelegateCommand cmdMoveForward;
        public DelegateCommand CmdMoveForward => cmdMoveForward ??= new DelegateCommand(ExecuteMoveForward);
        private void ExecuteMoveForward()
        {
            if (ContinueMove)
            {
                selectedAxisRef.Prm.MaxVel = JogSpeed;
                motionPart1.MC_MoveJog(SelectedAxisRef, SelectedAxisRef.Rate);
            }
            else
            {

                motionPart1.MC_MoveAdd(SelectedAxisRef, JogDistance, SelectedAxisRef.Rate);

            }

        }



        #region 负向移动

        private DelegateCommand cmdMoveBackward;
        public DelegateCommand CmdMoveBackward => cmdMoveBackward ??= new DelegateCommand(ExecuteMoveBackward, () => SelectedAxisRef != null);

        public void ExecuteMoveBackward()
        {
            if (ContinueMove)
            {

                selectedAxisRef.Prm.MaxVel = -JogSpeed;
                motionPart1.MC_MoveJog(SelectedAxisRef, SelectedAxisRef.Rate);

            }
            else
            {
                motionPart1.MC_MoveAdd(SelectedAxisRef, -jogDistance, SelectedAxisRef.Rate);
            }
        }
        #endregion

        #region 停止移动
        private DelegateCommand cmdStopMove;
        public DelegateCommand CmdStopMove =>
            cmdStopMove ?? (cmdStopMove = new DelegateCommand(ExecuteStopMove, () => SelectedAxisRef != null));
        public void ExecuteStopMove()
        {
            if (ContinueMove)
            {
                motionPart1.MC_EStop(SelectedAxisRef);
            }
        }
      
        #endregion
        #region 设置命令
        private DelegateCommand cmdSetAxis;
        public DelegateCommand CmdSetAxis =>
            cmdSetAxis ?? (cmdSetAxis = new DelegateCommand(ExecuteCmdSetAxis, () =>
             SelectedAxisRef != null));

        async void ExecuteCmdSetAxis()
        {
            await DialogHost.Show(new AxisSetting()
            {
                DataContext = new
                {
                    SelectedObject = SelectedAxisRef.Prm
                }
            }, "RootDialog"); ;
            config.Set(config.Get());
        }
        #endregion


        #region 回零运动
        private DelegateCommand cmdHome;
        public DelegateCommand CmdHome =>
            cmdHome ?? (cmdHome = new DelegateCommand(async ()=>await ExecuteHomeAsync(), () => SelectedAxisRef != null));

        async System.Threading.Tasks.Task ExecuteHomeAsync()
        {
          await  motionPart1.MC_Home( selectedAxisRef);
        }
        #endregion

        #endregion

        #endregion
        private DelegateCommand  clrAlarm;
        public DelegateCommand ClearAlarms => clrAlarm
             ??  new DelegateCommand(ExecuteClearAlarms);

        void ExecuteClearAlarms()
        {
            motionPart1.MC_Reset(selectedAxisRef);
        }

        #region fields
        DispatcherTimer timer = new DispatcherTimer();
        readonly IGtsMotion motion;
        readonly IConfigManager<MotionConfig> config;
        List<AxisRef> axisRefs;
        ICMotionData motionData;
        IMotionPart1 motionPart1;

        AxisRef selectedAxisRef;
        bool continuous;
        int jogSpeed = 10;
        bool jogMove = true;
        private double jogDistance=10;
        #endregion

        #region ctors
        public SingleAxisViewModel(IUnityContainer container)
        {
            this.motion = container.Resolve<IGtsMotion>();
            config = container.Resolve<IConfigManager<MotionConfig>>();
            motionPart1 = motion;
            motionData = motion;
            AxisRefs = motionData.AxisRefs.Where(x => x.Name != "").ToList();
           SelectedAxisRef = AxisRefs.FirstOrDefault();
        }
        #endregion

    }
}