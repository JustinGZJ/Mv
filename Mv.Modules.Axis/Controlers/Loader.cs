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
        IGtsMotion motion;
        public Loader(IUnityContainer container, IEventAggregator @event)
        {
            var factory = container.Resolve<IFactory<string, ICylinder>>(nameof(CylinderFactory));
            CyRightLeft = factory.Create(CylinderFactory.PlugLeftRight);
            CyUpDown = factory.Create(CylinderFactory.PlugInUpDown);
            CyFrontBack = factory.Create(CylinderFactory.PlugInForthBack);
            CyLoadFB = factory.Create(CylinderFactory.FeedingForthBack);
            CyLoadLR = factory.Create(CylinderFactory.FeedingRightLeft);
            motion = container.Resolve<IGtsMotion>();
            Servo = motion.AxisRefs.FirstOrDefault(x => x.Name == "C");
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
            arr.Add((await motion.MC_Home(Servo) == 0));
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
        AxisRef c;
        public override void Run()
        {
            c = Servo;
            c.Prm.MaxVel = 40000;
            c.Prm.MaxAcc = 40000;
            while (true)
            {
                try
                {
                    if (run_mode == EAutoMode.AUTO || run_mode == EAutoMode.STEP)
                    {
                        switch (Step_auto)
                        {
                            case 0:
                                GetProduct();
                                break;
                            case 1:
                                PutProduct(30000);
                                break;
                            case 2:
                                GetProduct();
                                break;
                            case 3:
                                PutProduct(15000);
                                break;
                            case 4:
                                GetProduct();
                                break;
                            case 5:
                                PutProduct(0);
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
        private void GetProduct()
        {
            CyLoadFB.Reset();
            CyLoadLR.Reset();
            motion.MC_MoveAbs(c, 50000);
            SpinWait.SpinUntil(() => c.CmdPos == 50000);
            CyLoadFB.Set();
            Thread.Sleep(1000);
            motion.MC_MoveAbs(c, 45000);
            SpinWait.SpinUntil(() => c.CmdPos == 45000);
        }

        private void PutProduct(double location)
        {
            CyLoadLR.Set();
            motion.MC_MoveAbs(c, location);
            SpinWait.SpinUntil(() => c.CmdPos == location);
            CyUpDown.Set();
            Thread.Sleep(1000);
            CyLoadFB.Reset();
            Thread.Sleep(1000);
            CyUpDown.Reset();
            Thread.Sleep(1000);
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
