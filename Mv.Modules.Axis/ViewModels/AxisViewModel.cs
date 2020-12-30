﻿using System;
using System.Collections.ObjectModel;
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
        public ObservableCollection<P2PPrm> P2PPrms { get; set; } = new ObservableCollection<P2PPrm>();
        public P2PPrm SelectedP2P { get => selectedP2P; set => SetProperty(ref selectedP2P, value); }

        public SingleAxisViewModel SingleAxisViewModel { get; }
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
                    Name = $"{selectedAxisRef.Name}-{P2PPrms.Count()}",
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
cmdReappear ??= new DelegateCommand(ExecuteReappear, () => SelectedAxisRef != null);

        void ExecuteReappear()
        {
            if (SelectedAxisRef == null || SelectedP2P == null)
                return;
            motionPart1.MC_MoveAbs(selectedAxisRef, selectedP2P.Position);
        }
        #endregion

        #region 保存点位
        private DelegateCommand cmdSavePoint;
        public DelegateCommand CmdSavePoint =>
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
                Name = $"{selectedAxisRef.Name}-{P2PPrms.Count()}",
                Acceleration = 100,
                AxisName = selectedAxisRef.Name,
                Position = selectedAxisRef.RelPos,
                Velocity = 100
            });
        }
        #endregion


        #region fields
        readonly IGtsMotion motion;
        readonly IConfigManager<MotionConfig> config;
        private readonly IUnityContainer container;
        IMotionPart1 motionPart1;
        AxisRef selectedAxisRef;
        P2PPrm selectedP2P;
        #endregion

        #region ctors
        public AxisViewModel(IUnityContainer container)
        {
            this.motion = container.Resolve<IGtsMotion>();
            config = container.Resolve<IConfigManager<MotionConfig>>();
            motionPart1 = motion;
            this.container = container;
           selectedAxisRef =motion.AxisRefs.FirstOrDefault();
            SingleAxisViewModel = new SingleAxisViewModel(container) { SelectedAxisRef = selectedAxisRef };
        }

        #endregion


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