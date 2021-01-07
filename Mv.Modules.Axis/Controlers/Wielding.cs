using MotionWrapper;
using Mv.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using System.Linq;

namespace Mv.Modules.Axis.Controller
{
    public class Wielding : MachineBase
    {
        public Wielding(
            IUnityContainer container)
        {
            this.container = container;
            this.factory = container.Resolve<IFactory<string, ICylinder>>(nameof(CylinderFactory));
            SpindleLockingCylinder = factory.Create(CylinderFactory.SpindleLockingCylinder);
            EnableTensioner = factory.Create(CylinderFactory.EnableTensioner);
            FingerClosure = factory.Create(CylinderFactory.FingerClosure);
            FingerFlip = factory.Create(CylinderFactory.FingerFlip);
            FingerSideToSide = factory.Create(CylinderFactory.FingerSideToSide);
            LineNozzleFlip = factory.Create(CylinderFactory.LineNozzleFlip);
            motion = container.Resolve<IGtsMotion>();
            X = motion.AxisRefs.FirstOrDefault(x => x.Name == "X");
            Y = motion.AxisRefs.FirstOrDefault(x => x.Name == "Y");
            Z = motion.AxisRefs.FirstOrDefault(x => x.Name == "Z");
            B = motion.AxisRefs.FirstOrDefault(x => x.Name == "B");
            C = motion.AxisRefs.FirstOrDefault(x => x.Name == "C");
            SP = motion.AxisRefs.FirstOrDefault(x => x.Name == "SP");
            motion.MC_Home(X);
            motion.MC_Home(Y);
            motion.MC_Home(Z);
            motion.MC_Home(SP);
            motion.MC_Home(B);
            motion.MC_Home(C);
        }



        #region machinebase
        public override void Fresh()
        {

        }

        public override async Task<bool> Home()
        {
            List<bool> arr = new List<bool>();
            arr.Add(await SpindleLockingCylinder.Set());
            arr.Add(await EnableTensioner.Reset());
            arr.Add(await FingerClosure.Reset());
            arr.Add(await FingerFlip.Reset());
            arr.Add(await FingerSideToSide.Reset());
            arr.Add(await LineNozzleFlip.Reset());
            return await Task.FromResult(arr.Aggregate((a, b) => a && b));
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

        }

        private int step_auto_temp = -1;
        private readonly IUnityContainer container;
        private readonly IFactory<string, ICylinder> factory;

        public override void Run()
        {
            while (true)
            {
                try
                {
                    if (step_auto_temp != Step_auto && (run_mode == EAutoMode.AUTO || run_mode == EAutoMode.STEP))
                    {
                        switch (Step_auto)
                        {
                            case 0:
                                for (int i = 0; i < 2; i++)
                                {
                                    Task.WaitAll(FingerFlip.Set(), FingerClosure.Set());
                                    Thread.Sleep(1000);
                                    Task.WaitAll(FingerFlip.Reset(), FingerClosure.Reset());
                                    Thread.Sleep(1000);
                                }
                                break;
                            case 1:
                                for (int i = 0; i < 2; i++)
                                {
                                    motion.MC_MoveAbs(Y, -5000, 1);
                                    motion.MC_MoveAdd(SP, 10000, 1);
                                    Thread.Sleep(100);
                                    SpinWait.SpinUntil(() => !Y.Moving);               
                                    motion.MC_MoveAbs(Y, 0, 1);
                                    motion.MC_MoveAdd(SP,5000, 1);                                 
                                    SpinWait.SpinUntil(() => !Y.Moving);
                                    Thread.Sleep(100);
                                }
                                break;
                            case 2:
                                for (int i = 0; i < 2; i++)
                                {
                                    Task.WaitAll(FingerClosure.Set(), FingerSideToSide.Set());
                                    Thread.Sleep(1000);
                                    Task.WaitAll(FingerClosure.Reset(), FingerSideToSide.Reset());
                                    Thread.Sleep(1000);
                                }
                                break;
                            case 3:
                                for (int i = 0; i < 2; i++)
                                {
                                    motion.MC_MoveAdd(SP, 100000, 1);
                                    motion.MC_MoveAbs(Y, -5000, 1);
                                    Thread.Sleep(100);
                                    SpinWait.SpinUntil(() => !Y.Moving);
                                    motion.MC_MoveAbs(X, 5000, 1);                                 
                                    Thread.Sleep(100);
                                    SpinWait.SpinUntil(() => !X.Moving);
                                    motion.MC_MoveAbs(Z, 2000, 1);
                                    Thread.Sleep(100);
                                    SpinWait.SpinUntil(() => !Z.Moving);
                                    motion.MC_MoveAbs(Z, 0, 1);
                                    Thread.Sleep(100);
                                    SpinWait.SpinUntil(() => !Z.Moving);
                                    motion.MC_MoveAbs(Y, 0, 1);
                                    Thread.Sleep(100);
                                    SpinWait.SpinUntil(() => !Y.Moving);
                                    motion.MC_MoveAbs(X, 0, 1);
                                    Thread.Sleep(100);
                                    SpinWait.SpinUntil(() => !X.Moving);
                                }
                                break;
                            case 4:
               
                                break;
                        }
                        step_auto_temp = Step_auto;
                        if (run_mode == EAutoMode.AUTO)
                        {
                            if (Step_auto <100)
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
        ICylinder SpindleLockingCylinder;
        ICylinder EnableTensioner;
        ICylinder FingerClosure;
        ICylinder FingerFlip;
        ICylinder FingerSideToSide;
        ICylinder LineNozzleFlip;
        private IGtsMotion motion;
        AxisRef X;
        AxisRef Y;
        AxisRef Z;
        AxisRef SP;
        AxisRef B;
        AxisRef C;
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
