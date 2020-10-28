using System;
using System.Collections.Generic;

namespace MotionWrapper
{
    /// <summary>
    /// 机器类,所有的并行组件都在这边
    /// </summary>
    public class CMachManager : CMachBase, IInitModel
    {
        //所有的机器组件都在这个列表中
        List<CMachBase> machlist = new List<CMachBase>();
        public CMachManager()
        {
            machlist.Clear();
        }
        public void addMach(CMachBase inMach)
        {
            machlist.Add(inMach);
        }
        public override bool preCheckStatus()
        {
            bool statusAll = true;
            foreach (CMachBase item in machlist)
            {
                if (!item.preCheckStatus()) statusAll = false;
            }
            return statusAll;
        }

        public override void Fresh()
        {
            throw new NotImplementedException();
        }

        public override bool Home()
        {
            bool statusAll = true;
            foreach (CMachBase item in machlist)
            {
                if (!item.Home())
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
            initok = true;
            foreach (CMachBase item in machlist)
            {
                if (item is IInitModel)//判断是否继承了接口
                {
                    if (!((IInitModel)item).Init()) initok = false;
                }
            }
            return initok;
        }

        public override bool Pause()
        {
            bool paused = true;
            foreach (CMachBase item in machlist)
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
            foreach (CMachBase item in machlist)
            {
                item.Reset();
            }
            return true;
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }

        public override bool Start()
        {
            bool checkOk = true;
            foreach (CMachBase item in machlist)
            {
                if (!item.preCheckStatus())
                {
                    checkOk = false;
                    break;
                }
            }
            if (checkOk)
            {
                foreach (CMachBase item in machlist)
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
            foreach (CMachBase item in machlist)
            {
                item.Stop();
            }
            run_mode = EAutoMode.MANUAL;
            return true;
        }

        public bool UnInit()
        {
            foreach (CMachBase item in machlist)
            {
                if (item is IInitModel)//判断是否继承了接口
                {
                    ((IInitModel)item).UnInit();
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
