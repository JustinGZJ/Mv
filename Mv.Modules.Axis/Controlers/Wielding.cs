using MotionWrapper;
using Mv.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using System.Linq;
using Mv.Modules.Axis.Controlers;

namespace Mv.Modules.Axis.Controller
{
    public class Wielding : MachineBase
    {
        public Wielding(
            IUnityContainer container,IShareData shareData)
        {
            this.container = container;
            this.shareData = shareData;
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
            await Task.WhenAll(motion.MC_Home(X), motion.MC_Home(Y), motion.MC_Home(Z), motion.MC_Home(SP), motion.MC_Home(B), motion.MC_Home(B), motion.MC_Home(C));

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
        private readonly IShareData shareData;
        private readonly IFactory<string, ICylinder> factory;

        public override void Run()
        {
            while (true)
            {
                X.Prm.MaxVel = 15000;
                X.Prm.MaxAcc = 15000;
                Y.Prm.MaxVel = 15000;
                Y.Prm.MaxAcc = 15000;
                try
                {
                    if (step_auto_temp != Step_auto && (run_mode == EAutoMode.AUTO || run_mode == EAutoMode.STEP))
                    {
                        switch (Step_auto)
                        {
                            case 0:
                                SpinWait.SpinUntil(() => shareData.LoaderHandshake==1);
                                shareData.WeildingHandshake = 1;
                    //            SpinWait.SpinUntil(() => shareData.LoaderHandshake == 1);
                                break;
                            case 1:
                                //   B和Y 同时  →  手指左
                                motion.MC_MoveAbs(B, -12000, 1);
                                motion.MC_MoveAbs(Y, -12000, 1);
                                FingerSideToSide.Set();
                                SpinWait.SpinUntil(() => B.CmdPos == -12000);
                                SpinWait.SpinUntil(() => Y.CmdPos == -12000);
                                break;
                            case 2:
                                //X Y Z 缠pin 上下左右 *10 起线
                                X.Prm.MaxVel = 50000;
                                X.Prm.MaxAcc = 50000;
                                Y.Prm.MaxVel = 50000;
                                Y.Prm.MaxAcc = 50000;
                                var xpos = X.CmdPos;
                                var ypos = Y.CmdPos;
                                for (int i = 0; i < 5; i++)
                                {
                                    motion.MC_MoveAdd(X, 1000);
                                    SpinWait.SpinUntil(() => X.CmdPos == xpos + 1000);
                                    motion.MC_MoveAdd(Y, 1000);
                                    SpinWait.SpinUntil(() => Y.CmdPos == ypos + 1000);
                                    motion.MC_MoveAdd(X, -1000);
                                    SpinWait.SpinUntil(() => X.CmdPos == xpos );
                                    motion.MC_MoveAdd(Y, -1000);
                                    SpinWait.SpinUntil(() => Y.CmdPos == ypos ); 
                                }
                                break;
                            case 3:
                                //手指右   //扯线
                                //手指倒下  //180
                                FingerSideToSide.Reset();
                                FingerFlip.Set();
                                Thread.Sleep(500);
                                break;
                            case 4:
                                //绕线 同时 手指张闭张 * 3
                                Task.Run(() => {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        FingerClosure.Set();
                                        Thread.Sleep(300);
                                        FingerClosure.Reset();
                                        Thread.Sleep(300);
                                    }
                                });
                                var startpos = -12000;
                                motion.MC_MoveAbs(Y, startpos, 1);
                                SpinWait.SpinUntil(() => Y.CmdPos == startpos);
                                motion.MC_MoveAdd(SP, 24000, 1);
                                for (int i = 0; i < 8; i++)
                                {
                                    motion.MC_MoveAbs(Y, -8000, 1);
                                    SpinWait.SpinUntil(() => Y.CmdPos == -8000);
                                    motion.MC_MoveAbs(Y, -12000, 1);
                                    SpinWait.SpinUntil(() => Y.CmdPos == -12000);
                                }
                                SpinWait.SpinUntil(() => !SP.Moving);
                                break;
                            case 5:
                                FingerFlip.Reset();
                                Thread.Sleep(1000);
                                xpos = X.CmdPos;
                                ypos = Y.CmdPos;
                                X.Prm.MaxVel = 50000;
                                X.Prm.MaxAcc = 50000;
                                Y.Prm.MaxVel = 50000;
                                Y.Prm.MaxAcc = 50000;
                                for (int i = 0; i < 5; i++)
                                {
                                    motion.MC_MoveAdd(X, 1000);
                                    SpinWait.SpinUntil(() => X.CmdPos == xpos + 1000);
                                    motion.MC_MoveAdd(Y, 1000);
                                    SpinWait.SpinUntil(() => Y.CmdPos == ypos + 1000);
                                    motion.MC_MoveAdd(X, -1000);
                                    SpinWait.SpinUntil(() => X.CmdPos == xpos);
                                    motion.MC_MoveAdd(Y, -1000);
                                    SpinWait.SpinUntil(() => Y.CmdPos == ypos);
                                }
                                FingerSideToSide.Set();
                                Thread.Sleep(300);
                                for (int i = 0; i < 5; i++)
                                {
                                    motion.MC_MoveAdd(X, 1000);
                                    SpinWait.SpinUntil(() => X.CmdPos == xpos + 1000);
                                    motion.MC_MoveAdd(Y, 1000);
                                    SpinWait.SpinUntil(() => Y.CmdPos == ypos + 1000);
                                    motion.MC_MoveAdd(X, -1000);
                                    SpinWait.SpinUntil(() => X.CmdPos == xpos);
                                    motion.MC_MoveAdd(Y, -1000);
                                    SpinWait.SpinUntil(() => Y.CmdPos == ypos);
                                }
                                FingerSideToSide.Reset();
                                Thread.Sleep(300);
                                break;
                            case 6:
                                motion.MC_MoveAbs(Y, 0, 1);
                                motion.MC_MoveAbs(X, 0, 1);
                                motion.MC_MoveAbs(Z, 0, 1);
                                motion.MC_MoveAbs(B, 0, 1);
                                SpinWait.SpinUntil(() => Y.CmdPos == 0);
                                SpinWait.SpinUntil(() => X.CmdPos == 0);
                                SpinWait.SpinUntil(() => Z.CmdPos == 0);
                                SpinWait.SpinUntil(() => B.CmdPos == 0);
                                shareData.WeildingHandshake = 0;
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
