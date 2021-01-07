using MotionWrapper;
using Mv.Core.Interfaces;
using System;
using System.Linq;
using Unity;
using System.Threading.Tasks;
using System.Threading;
using Prism.Events;
using System.Collections.Generic;

namespace Mv.Modules.Axis.Controller
{
    public class Loader : MachineBase
    {
        #region Properties
        public AxisRef Servo { get; set; }
        public ICylinder CyRightLeft { get; set; }
        public ICylinder CyUpDown { get; set; }
        public ICylinder CyFrontBack { get; set; }
        public ICylinder CyLoadFB { get; set; }
        public ICylinder CyLoadLR { get; set; }
        #endregion
        IGtsMotion gtsMotion;
        public Loader(IUnityContainer container, IEventAggregator @event)
        {
            var factory = container.Resolve<IFactory<string, ICylinder>>(nameof(CylinderFactory));
            CyRightLeft = factory.Create(CylinderFactory.PlugLeftRight);
            CyUpDown = factory.Create(CylinderFactory.PlugInUpDown);
            CyFrontBack = factory.Create(CylinderFactory.PlugInForthBack);
            CyLoadFB = factory.Create(CylinderFactory.FeedingForthBack);
            CyLoadLR = factory.Create(CylinderFactory.FeedingRightLeft);
            gtsMotion = container.Resolve<IGtsMotion>();
            Servo = gtsMotion.AxisRefs.FirstOrDefault(x => x.Name == "C");
            this.@event = @event;
        }

        #region machinebase
        public override void Fresh()
        {

        }

        public override async Task<bool> Home()
        {
            var arr = new List<bool>();
            arr.Add(await CyRightLeft.Reset());
            arr.Add(await CyUpDown.Reset());
            arr.Add(await CyFrontBack.Reset());
            arr.Add(await CyLoadFB.Reset());
            arr.Add(await CyLoadLR.Reset());
            arr.Add((await gtsMotion.MC_Home(Servo) == 0));
            return arr.Aggregate((a, b) => a && b);

        }

        public override bool Pause()
        {
            run_mode = EAutoMode.PAUSE;
            return true;
        }

        public override bool preCheckStatus()
        {

            return true;
        }

        public override bool Reset()
        {
            return true;
            //throw new NotImplementedException();
        }
        int step_auto_temp = 0;
        private readonly IEventAggregator @event;

        public override void Run()
        {
            while (true)
            {
                try
                {
                    if (run_mode == EAutoMode.AUTO || run_mode == EAutoMode.STEP)
                    {
                        switch (Step_auto)
                        {
                            case 0:

                                break;
                            case 1:

                                break;
                            case 2:

                                break;
                            case 3:

                                break;
                            case 4:

                                break;
                            default:
                                break;
                        }
                        step_auto_temp = Step_auto;
                        if (run_mode == EAutoMode.AUTO)
                        {
                            if (Step_auto < 100)
                                Step_auto++;
                            else
                                Step_auto = 0;
                        }
                    }
                }
                catch (Exception ex)
                {

                    /// throw;
                }
            }
        }

        public override void RunModel(int modelIndex, ref EAutoMode modelNow)
        {
            modelNow = run_mode;
        }

        public override bool Start()
        {

            run_mode = EAutoMode.AUTO;
            if (runThread == null)
            {
                runThread = new Thread(Run)
                {
                    IsBackground = true
                };
                runThread.Start();
            }
            return true;
        }

        public override bool Stop()
        {
            run_mode = EAutoMode.MANUAL;
            Step_auto = 0;
            step_auto_temp = 0;
            return true;
        }
        #endregion
    }
}
