using MotionWrapper;
using Mv.Core.Interfaces;
using System;
using System.Linq;
using Unity;
using System.Threading.Tasks;
using System.Threading;
using Prism.Events;
using System.Collections.Generic;
using Mv.Modules.Axis.Controlers;

namespace Mv.Modules.Axis.Controller
{
    public class Loader : MachineBase
    {
        #region Properties
        public AxisRef Servo { get; set; }
        public ICylinder CyPluginRightLeft { get; set; }
        public ICylinder CyPluginUpDown { get; set; }
        public ICylinder CyPlugInFrontBack { get; set; }
        public ICylinder CyLoadFB { get; set; }
        public ICylinder CyLoadLR { get; set; }
        #endregion
        IGtsMotion motion;
        public Loader(IUnityContainer container, IEventAggregator @event,IShareData shareData)
        {
            var factory = container.Resolve<IFactory<string, ICylinder>>(nameof(CylinderFactory));
            CyPluginRightLeft = factory.Create(CylinderFactory.PlugLeftRight);
            CyPluginUpDown = factory.Create(CylinderFactory.PlugInUpDown);
            CyPlugInFrontBack = factory.Create(CylinderFactory.PlugInForthBack);
            CyLoadFB = factory.Create(CylinderFactory.FeedingForthBack);
            CyLoadLR = factory.Create(CylinderFactory.FeedingRightLeft);
            motion = container.Resolve<IGtsMotion>();
            Servo = motion.AxisRefs.FirstOrDefault(x => x.Name == "C");
            this.@event = @event;
            this.shareData = shareData;
        }

        #region machinebase
        public override void Fresh()
        {

        }

        public override async Task<bool> Home()
        {
            var arr = new List<bool>();
            arr.Add(await CyPluginRightLeft.Reset());
            arr.Add(await CyPluginUpDown.Reset());
            arr.Add(await CyPlugInFrontBack.Reset());
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
        private readonly IShareData shareData;
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
                               // 
                                break;
                            case 6:
                                SpinWait.SpinUntil(() => shareData.WeildingHandshake == 0);
                                CyPluginRightLeft.Reset();
                                PutIntoFixture();
                                CyPluginRightLeft.Set();
                                PutIntoFixture();
                                shareData.LoaderHandshake = 1;
                                SpinWait.SpinUntil(() => shareData.WeildingHandshake == 1);
                                shareData.LoaderHandshake = 0;
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

        private void PutIntoFixture()
        {
            CyPluginUpDown.Reset();
            CyPlugInFrontBack.Set(); //QIANJIN
            Thread.Sleep(1000);
            CyPluginUpDown.Set();
            Thread.Sleep(500);
            CyPlugInFrontBack.Reset();//TUIHUI
            Thread.Sleep(1000);
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
            CyPluginUpDown.Set();
            Thread.Sleep(500);
            CyLoadFB.Reset();
            Thread.Sleep(500);
            CyPluginUpDown.Reset();
            Thread.Sleep(500);
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
