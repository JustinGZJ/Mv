using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotionWrapper
{
    /// <summary>
    /// 机器类,所有的并行组件都在这边
    /// </summary>
    public class MachineManager : MachineBase, IInitable
    {
        //所有的机器组件都在这个列表中
        List<MachineBase> machlist = new List<MachineBase>();
        private readonly IGtsMotion motion;

        public MachineManager(IGtsMotion motion)
        {
            this.motion = motion;
          //  motion.Dis.get
            // machlist.Clear();
        }
        public void AddMachine(MachineBase inMach)
        {
            machlist.Add(inMach);
        }
        public override bool preCheckStatus()
        {
            bool statusAll = true;
            foreach (MachineBase item in machlist)
            {
                if (!item.preCheckStatus()) statusAll = false;
            }
            return statusAll;
        }

        public override void Fresh()
        {
           
           // throw new NotImplementedException();
        }

        public override async Task<bool> Home()
        {
            bool statusAll = true;
            foreach (MachineBase item in machlist)
            {
                if (!await item.Home())
                {
                    statusAll = false;
                    break;
                }
            }
            if (!statusAll) Stop();
            return statusAll;
        }

        public bool Init()
        {
            Initok = true;
            foreach (MachineBase item in machlist)
            {
                if (item is IInitable)//判断是否继承了接口
                {
                    if (!((IInitable)item).Init()) Initok = false;
                }
            }
            return Initok;
        }

        public override bool Pause()
        {
            bool paused = true;
            foreach (MachineBase item in machlist)
            {
                if (!item.Pause()) paused = false;
            }
            if (paused)
            {
                run_mode = EAutoMode.PAUSE;
            }
            return paused;
        }

        public override bool Reset()
        {
            foreach (MachineBase item in machlist)
            {
                item.Reset();
            }
            return true;
        }

        public override void Run()
        {
           while(true)
            {
                Fresh();
            }
        }

        public override bool Start()
        {
            bool checkOk = true;
            foreach (MachineBase item in machlist)
            {
                if (!item.preCheckStatus())
                {
                    checkOk = false;
                    break;
                }
            }
            if (checkOk)
            {
                foreach (MachineBase item in machlist)
                {
                    item.Start();
                }
                run_mode = EAutoMode.AUTO;
                return true;
            }
            else
            {
                run_mode = EAutoMode.MANUAL;
                return false;
            }
        }

        public override bool Stop()
        {
            foreach (MachineBase item in machlist)
            {
                item.Stop();
            }
            run_mode = EAutoMode.MANUAL;
            return true;
        }

        public bool UnInit()
        {
            foreach (MachineBase item in machlist)
            {
                if (item is IInitable)//判断是否继承了接口
                {
                    ((IInitable)item).UnInit();
                }
            }
            return true;
        }

        public override void RunModel(int modelIndex,ref EAutoMode modelNow)
        {
            if (modelIndex >=0 && modelIndex < machlist.Count)
            {
                modelNow = machlist[modelIndex].run_mode;
            }
        }
    }
}
