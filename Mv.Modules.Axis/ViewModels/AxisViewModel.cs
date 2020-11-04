using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
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

    public class AxisViewModel : BindableBase
    {

        #region Properties

        public AxisRef SelectedAxisRef
        {
            get => selectedAxisRef;
            set
            {
                if (SetProperty(ref selectedAxisRef, value))
                {
                    P2PPrms.CollectionChanged -= P2PPrms_CollectionChanged;
                    P2PPrms.Clear();
                    var ms = config.Get().P2PPrameters.Where(x => x.AxisName == selectedAxisRef.Name);
                    foreach (var item in ms)
                    {
                        P2PPrms.Add(item);
                    }
                    P2PPrms.CollectionChanged += P2PPrms_CollectionChanged;
                }
            }
        }
        public List<AxisRef> AxisRefs { get => axisRefs; set => SetProperty(ref axisRefs, value); }
        public double JogDistance { get => jogDistance; set => SetProperty(ref jogDistance, value); }
        public int JogSpeed { get => jogSpeed; set => SetProperty(ref jogSpeed, value); }
        public ObservableCollection<P2PPrm> P2PPrms { get => p2PPrms; set => p2PPrms = value; }
        public P2PPrm SelectedP2P { get => selectedP2P; set => SetProperty(ref selectedP2P, value); }
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
            //  motionPart1.MC_SetPos
        }




        #region 正向移动
        private DelegateCommand cmdMoveForward;
        public DelegateCommand CmdMoveForward => cmdMoveForward ??= new DelegateCommand(ExecuteMoveForward);
       private  void ExecuteMoveForward()
        {
            if (jogMove)
            {
                motionPart1.MC_MoveJog(SelectedAxisRef, SelectedAxisRef.Rate);
            }
            else
            {

                motionPart1.MC_MoveAdd(SelectedAxisRef, JogDistance, SelectedAxisRef.Rate);

            }

        }



        #region 负向移动

        private DelegateCommand cmdMoveBackward;
        public DelegateCommand CmdMoveBackward => cmdMoveBackward ??= new DelegateCommand(ExecuteMoveBackward, () => SelectedAxisRef != null );

      public  void ExecuteMoveBackward()
        {
            if (!jogMove)
            {

                selectedAxisRef.Prm.MaxVel = JogSpeed;
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
            if (Continuous)
            {
                motionPart1.MC_EStop(SelectedAxisRef);
            }
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

        #region 添加位置命令
        private DelegateCommand cmdAdd;
        public DelegateCommand CmdAdd =>
            cmdAdd ?? (cmdAdd = new DelegateCommand(ExecutecmdAdd, () => SelectedAxisRef != null));
        void ExecutecmdAdd()
        {
            if (selectedAxisRef == null)
                return;
            motionPart1.MC_AxisRef(ref selectedAxisRef);
            P2PPrms.Add(new P2PPrm()
            {
                Name = $"{selectedAxisRef.Name}-{p2PPrms.Count()}",
                Acceleration = 100,
                AxisName = selectedAxisRef.Name,
                Position = selectedAxisRef.RelPos,
                Velocity = 100
            });
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

        #region 示教
        private DelegateCommand cmdTeach;
        public DelegateCommand CmdTeach =>
            cmdTeach ?? (cmdTeach = new DelegateCommand(ExecuteTeach, () => SelectedAxisRef != null));

        void ExecuteTeach()
        {
            if (selectedAxisRef == null)
                return;
            if (SelectedP2P == null)
            {
                P2PPrms.Add(new P2PPrm()
                {
                    Name = $"{selectedAxisRef.Name}-{p2PPrms.Count()}",
                    Acceleration = 100,
                    AxisName = selectedAxisRef.Name,
                    Position = selectedAxisRef.RelPos,
                    Velocity = 100
                });
            }
            else
            {
                SelectedP2P.Position = selectedAxisRef.RelPos;
            }

        }
        #endregion

        #region 重现
        private DelegateCommand cmdReappear;
        public DelegateCommand CmdReappear =>
            cmdReappear ?? (cmdReappear = new DelegateCommand(ExecuteReappear, () => SelectedAxisRef != null && selectedP2P != null));

        void ExecuteReappear()
        {
            motionPart1.MC_MoveAbs(selectedAxisRef, selectedP2P.Position);
        }
        #endregion

        #region 保存点位
        private DelegateCommand cmdSavePoint;
        public DelegateCommand CmdSaePoint =>
            cmdSavePoint ?? (cmdSavePoint = new DelegateCommand(ExecuteSaePoint));

        void ExecuteSaePoint()
        {
            config.Set(config.Get());
        }
        #endregion

        #region 删除点位
        private DelegateCommand cmdRemovePoint;
        public DelegateCommand CmdRemovePoint =>
            cmdRemovePoint ?? (cmdRemovePoint = new DelegateCommand(ExecuteCmdRemovePoint, () => SelectedAxisRef != null));

        void ExecuteCmdRemovePoint()
        {
            if (selectedP2P != null)
                P2PPrms.Remove(SelectedP2P);
        }
        #endregion

        #region 前往目标位置
        private DelegateCommand cmdGoTargetPos;
        public DelegateCommand CmdGoTargetPos =>
            cmdGoTargetPos ?? (cmdGoTargetPos = new DelegateCommand(ExecuteGoTargetPos, () => SelectedAxisRef != null));
        void ExecuteGoTargetPos()
        {

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
        IMotionPart5 motionPart5;
        IIoPart1 iopart1;
        AxisRef selectedAxisRef;
        ObservableCollection<P2PPrm> p2PPrms = new ObservableCollection<P2PPrm>();
        P2PPrm selectedP2P;
        bool continuous;
        int jogSpeed = 10;
        bool jogMove = true;
        private double jogDistance;
        #endregion

        #region ctors
        public AxisViewModel(IUnityContainer container)
        {
            this.motion = container.Resolve<IGtsMotion>();
            config = container.Resolve<IConfigManager<MotionConfig>>();
            motionPart1 = motion;
            motionData = motion;
            motionPart5 = motion;
            iopart1 = motion;
            AxisRefs = motionData.AxisRefs.Where(x => x.Name != "").ToList();
            SelectedAxisRef = AxisRefs.FirstOrDefault();

        }

        //public AxisViewModel()
        //{

        //}
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

        private void P2PPrms_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                config.Get().P2PPrameters.AddRange(e.NewItems.Cast<P2PPrm>());
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var paras = config.Get().P2PPrameters;
                foreach (var item in e.OldItems.Cast<P2PPrm>())
                {
                    paras.Remove(item);
                }
            }
        }
        #endregion
    }
}