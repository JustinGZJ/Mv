using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using System.Linq;
using MotionWrapper;
using System.Collections.Generic;
using Mv.Core;
using Unity;

namespace Mv.Modules.Axis.ViewModels
{
    public class P2PDataViewModel:BindableBase
    {
        ICMotionData motionData;
        IMotionPart1 motionPart1;

        public ObservableCollection<P2PPrm> P2PPrms { get => p2PPrms; set => p2PPrms = value; }
        public P2PPrm SelectedP2P { get => selectedP2P; set => SetProperty(ref selectedP2P, value); }
        AxisRef selectedAxisRef;
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
        P2PPrm selectedP2P;
        ObservableCollection<P2PPrm> p2PPrms = new ObservableCollection<P2PPrm>();

        public P2PDataViewModel(IUnityContainer container)
        {
            this.motion = container.Resolve<IGtsMotion>();
            config = container.Resolve<IConfigManager<MotionConfig>>();
            motionPart1 = motion;
            motionData = motion;
        }

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
                    Acceleration = selectedAxisRef.Prm.MaxAcc,
                    AxisName = selectedAxisRef.Name,
                    Position = selectedAxisRef.RelPos,
                    Velocity = selectedAxisRef.Prm.MaxVel
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
            cmdReappear ?? (cmdReappear = new DelegateCommand(ExecuteReappear, () => SelectedAxisRef != null && SelectedP2P != null));

        void ExecuteReappear()
        {
            motionPart1.MC_MoveAbs(selectedAxisRef, selectedP2P.Position);
        }
        #endregion

        #region 保存点位
        private DelegateCommand cmdSavePoint;
        public DelegateCommand CmdSavePoint =>
            cmdSavePoint ?? (cmdSavePoint = new DelegateCommand(ExecuteSavePoint));

        void ExecuteSavePoint()
        {
            config.Set(config.Get());
        }
        #endregion

        #region 删除点位
        private DelegateCommand cmdRemovePoint;
        private IGtsMotion motion;
        private IConfigManager<MotionConfig> config;

        public DelegateCommand CmdRemovePoint =>
            cmdRemovePoint ?? (cmdRemovePoint = new DelegateCommand(ExecuteCmdRemovePoint, () => SelectedAxisRef != null));

        void ExecuteCmdRemovePoint()
        {
            if (selectedP2P != null)
                P2PPrms.Remove(SelectedP2P);
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
    }
}