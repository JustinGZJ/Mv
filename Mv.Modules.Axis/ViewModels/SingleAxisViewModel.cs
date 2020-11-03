using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;
using System.Linq;
using MotionWrapper;
using System.Collections.Generic;
using Mv.Modules.Axis.Views.Dialog;
using MaterialDesignThemes.Wpf;
using Mv.Core;
using Unity;

namespace Mv.Modules.Axis.ViewModels
{
    public class SingleAxisViewModel : BindableBase
    {

        #region Properties

        public AxisRef SelectedAxisRef
        {
            get => selectedAxisRef;
            set
            {
                SetProperty(ref selectedAxisRef, value);
            }
        }
        public List<AxisRef> AxisRefs { get => axisRefs; set => SetProperty(ref axisRefs, value); }
        public double JogDistance { get => jogDistance; set => SetProperty(ref jogDistance, value); }
        public int JogSpeed { get => jogSpeed; set => SetProperty(ref jogSpeed, value); }
        /// <summary>
        /// 连动
        /// </summary>
        public bool Continuous
        {
            get { return continuous; }
            set { SetProperty(ref continuous, value); }
        }
        /// <summary>
        /// 点动
        /// </summary>
        public bool JogMove
        {
            get { return jogMove; }
            set { SetProperty(ref jogMove, value); }
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
        public DelegateCommand CmdMoveForward => cmdMoveForward ??= new DelegateCommand(ExecuteMoveForward, () => SelectedAxisRef != null);
        void ExecuteMoveForward()
        {
            if (jogMove)
            {
                Task.Run(() =>
                {
                    motionPart1.MC_MoveJog(SelectedAxisRef, SelectedAxisRef.Rate);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    motionPart1.MC_MoveAdd(SelectedAxisRef, 10d, SelectedAxisRef.Rate);
                });
            }

        }



        #region 负向移动

        private DelegateCommand cmdMoveBackward;
        public DelegateCommand CmdMoveBackward => cmdMoveBackward ??= new DelegateCommand(ExecuteMoveBackward, () => SelectedAxisRef != null);

        void ExecuteMoveBackward()
        {
            if (jogMove)
            {
                Task.Run(() =>
                {
                    selectedAxisRef.Prm.MaxVel = JogSpeed;
                    motionPart1.MC_MoveJog(SelectedAxisRef, SelectedAxisRef.Rate);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    motionPart1.MC_MoveAdd(SelectedAxisRef, -jogDistance, SelectedAxisRef.Rate);
                });
            }
        }
        #endregion

        #region 停止移动
        private DelegateCommand cmdStopMove;
        public DelegateCommand CmdStopMove =>
            cmdStopMove ?? (cmdStopMove = new DelegateCommand(ExecuteStopMove, () => SelectedAxisRef != null));
        void ExecuteStopMove()
        {
            motionPart1.MC_EStop(SelectedAxisRef);
        }
        #endregion
        #region 设置命令
        private DelegateCommand<AxisRef> cmdSetAxis;
        public DelegateCommand<AxisRef> CmdSetAxis =>
            cmdSetAxis ?? (cmdSetAxis = new DelegateCommand<AxisRef>(ExecuteCmdSetAxis, (axis) =>
             SelectedAxisRef != null));

        async void ExecuteCmdSetAxis(AxisRef axis)
        {
            await DialogHost.Show(new AxisSetting()
            {
                DataContext = new
                {
                    SelectedObject = axis.Prm
                }
            }, "RootDialog"); ;
            config.Set(config.Get());
        }
        #endregion


        #region 回零运动
        private DelegateCommand cmdHome;
        public DelegateCommand CmdHome =>
            cmdHome ?? (cmdHome = new DelegateCommand(ExecuteHome, () => SelectedAxisRef != null));

        void ExecuteHome()
        {
            motionPart1.MC_Home(ref selectedAxisRef);
        }
        #endregion

        #endregion

        #endregion

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
        private double jogDistance;
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

        #region EventHandler
        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < AxisRefs.Count; i++)
            {
                var axis = AxisRefs[i];
                motion.MC_AxisRef(ref axis);
            }
        }
        #endregion
    }
}