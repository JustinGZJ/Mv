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

namespace Mv.Modules.Axis.ViewModels
{
    public class AxisViewModel : BindableBase
    {


        #region Properties

        public AxisRef SelectedAxisRef { get; set; }

        /// <summary>
        /// 连动
        /// </summary>
        private bool continuous;
        public bool Continuous
        {
            get { return continuous; }
            set { SetProperty(ref continuous, value); }
        }
        /// <summary>
        /// 点动
        /// </summary>
        private bool jogMove = true;
        public bool JogMove
        {
            get { return jogMove; }
            set { SetProperty(ref jogMove, value); }
        }

        #endregion

        #region commands    

        #region 正向移动
        void ExecuteMoveForward()
        {
            if (jogMove)
            {
                Task.Run(() =>
                {
                    //             card.MoveRelative(axisName, (int)jogDistance , (short)JogSpeed);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    //                card.Jog(axisName, (int)JogSpeed);
                });
            }

        }
        private DelegateCommand cmdMoveForward;
        public DelegateCommand CmdMoveForward =>
            cmdMoveForward ?? (cmdMoveForward = new DelegateCommand(ExecuteMoveForward));
        #endregion

        #region 负向移动
        private DelegateCommand cmdMoveBackward;
        public DelegateCommand CmdMoveBackward =>
            cmdMoveBackward ?? (cmdMoveBackward = new DelegateCommand(ExecuteMoveBackward));

        void ExecuteMoveBackward()
        {
            if (jogMove)
            {
                Task.Run(() =>
                {
                    //             card.MoveRelative(axisName, (int)jogDistance * -1, (short)JogSpeed);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    //              card.Jog(axisName, -(int)JogSpeed);
                });
            }



        }
        #endregion

        #region 停止移动
        private DelegateCommand cmdStopMove;
        public DelegateCommand CmdStopMove =>
            cmdStopMove ?? (cmdStopMove = new DelegateCommand(ExecuteStopMove));

        void ExecuteStopMove()
        {
            //         card.Stop(AxisName, true);
        }
        #endregion
        #region 设置命令
        private DelegateCommand<string> cmdSetAxis;
        public DelegateCommand<string> CmdSetAxis =>
            cmdSetAxis ?? (cmdSetAxis = new DelegateCommand<string>(ExecuteCmdSetAxis));

        async void ExecuteCmdSetAxis(string para)
        {
            await DialogHost.Show(new AxisSetting()
            {
                DataContext = new
                {
                    SelectedObject = AxisRefs.FirstOrDefault(axis => axis.Name == para).Prm
                }
            }, "RootDialog"); ;
           config.Set(config.Get());
        }
        #endregion

        #region 回零运动
        private DelegateCommand cmdHome;
        public DelegateCommand CmdHome =>
            cmdHome ?? (cmdHome = new DelegateCommand(ExecuteHome));

        void ExecuteHome()
        {
            //     card.Home(AxisName, 10000, 1000, 1000, HomeDir.N负方向, 1000, 1000);
        }
        #endregion

        #region 示教
        private DelegateCommand cmdTeach;
        public DelegateCommand CmdTeach =>
            cmdTeach ?? (cmdTeach = new DelegateCommand(ExecuteTeach));

        void ExecuteTeach()
        {

        }
        #endregion

        #region 重现
        private DelegateCommand cmdReappear;
        public DelegateCommand CmdReappear =>
            cmdReappear ?? (cmdReappear = new DelegateCommand(ExecuteReappear));

        void ExecuteReappear()
        {

        }
        #endregion

        #region 保存点位
        private DelegateCommand cmdSavePoint;
        public DelegateCommand CmdSaePoint =>
            cmdSavePoint ?? (cmdSavePoint = new DelegateCommand(ExecuteSaePoint));

        void ExecuteSaePoint()
        {

        }
        #endregion

        #region 删除点位
        private DelegateCommand cmdRemovePoint;
        public DelegateCommand CmdRemovePoint =>
            cmdRemovePoint ?? (cmdRemovePoint = new DelegateCommand(ExecuteCmdRemovePoint));

        void ExecuteCmdRemovePoint()
        {

        }
        #endregion

        #region 前往目标位置
        private DelegateCommand cmdGoTargetPos;
        public DelegateCommand CmdGoTargetPos =>
            cmdGoTargetPos ?? (cmdGoTargetPos = new DelegateCommand(ExecuteGoTargetPos));

        public List<AxisRef> AxisRefs { get => axisRefs; set =>SetProperty(ref axisRefs , value); }

        void ExecuteGoTargetPos()
        {
            //   card.MoveAbs(AxisName, 10000, 1000);
        }
        #endregion

        #endregion

        void UpdateStatus()
        {

        }
        DispatcherTimer timer = new DispatcherTimer();
        private readonly IGtsMotion motion;
        private readonly IConfigManager<MotionConfig> config;
        List<AxisRef> axisRefs;
        ICMotionData motionData;
        IMotionPart1 motionPart1;
        IMotionPart5 motionPart5;
        IIoPart1 iopart1;
        public AxisViewModel(IGtsMotion motion, IConfigManager<MotionConfig> config)
        {
            this.motion = motion;
            this.config = config;
            motionPart1 = motion;
            motionData = motion;
            motionPart5 = motion;
            iopart1 = motion;
            AxisRefs = motionData.AxisRefs.Where(x => x.Name != "").ToList();

        }
        public AxisViewModel()
        {


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < AxisRefs.Count; i++)
            {
                var axis = AxisRefs[i];
                motion.MC_AxisRef(ref axis);
            }
        }
    }
}