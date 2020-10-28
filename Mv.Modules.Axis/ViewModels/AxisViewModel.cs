using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Mv.Modules.Axis.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Linq;
namespace Mv.Modules.Axis.ViewModels
{
    public class AxisViewModel : BindableBase
    {


        #region Properties
        /// <summary>
        /// 轴状态
        /// </summary>
   ///     public ObservableCollection<AxisStatus> AxisStatuses { get; } = new ObservableCollection<AxisStatus>();


        /// <summary>
        /// 伺服速度
        /// </summary>
        private double speed;
        public double Speed
        {
            get { return speed; }
            set { SetProperty(ref speed, value); }
        }

        /// <summary>
        /// 伺服速度
        /// </summary>
        private double jogSpeed;
        public double JogSpeed
        {
            get { return jogSpeed; }
            set { SetProperty(ref jogSpeed, value); }
        }

        public string axisName;
        public string AxisName
        {
            get { return axisName; }
            set { SetProperty(ref axisName, value); }
        }

        /// <summary>
        /// 目标位置
        /// </summary>
        private double tagrgetPos;
        public double TargetPos
        {
            get { return tagrgetPos; }
            set { SetProperty(ref tagrgetPos, value); }
        }


        /// <summary>
        /// 伺服使能信号
        /// </summary>
        private bool sevoON;
        public bool SevoON
        {
            get { return sevoON; }
            set { SetProperty(ref sevoON, value); }
        }
        /// <summary>
        /// 负向限位
        /// </summary>
        private bool negativeLimit;
        public bool NegativeLimit
        {

            get { return negativeLimit; }
            set { SetProperty(ref negativeLimit, value); }
        }
        /// <summary>
        /// 正向限位
        /// </summary>
        /// 
        private bool positiveLimit;
        public bool PositiveLimit
        {
            get { return positiveLimit; }
            set { SetProperty(ref positiveLimit, value); }
        }

        /// <summary>
        /// 报警
        /// </summary>
        private bool alarmSignal;
        public bool AlarmSignal
        {
            get { return alarmSignal; }
            set { SetProperty(ref alarmSignal, value); }
        }
        /// <summary>
        /// 原点信号
        /// </summary>
        private bool originSignal;
        public bool OriginSignal
        {
            get { return originSignal; }
            set { SetProperty(ref originSignal, value); }
        }

        /// <summary>
        /// 停止信号
        /// </summary>
        /// 
        private bool stop;
        public bool StopSignal
        {
            get { return stop; }
            set { SetProperty(ref stop, value); }
        }

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
        private bool jogMove=true;
        public bool JogMove
        {
            get { return jogMove; }
            set { SetProperty(ref jogMove, value); }
        }

        private string[] axisNames;
        public string[] AxisNames
        {
            get { return axisNames; }
            set { SetProperty(ref axisNames, value); }
        }


        private double jogDistance;
        public double JogDistance
        {
            get { return jogDistance; }
            set { SetProperty(ref jogDistance, value); }
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
            if(jogMove)
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

        void ExecuteCmdSetAxis(string para)
        {

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
        public AxisViewModel()
        {
;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }
    }
}