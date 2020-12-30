using Mv.Core.Interfaces;
using Unity;
using System.Linq;

namespace MotionWrapper
{
    public class CylinderFactory : IFactory<string, ICylinder>
    {
        private readonly IUnityContainer container;
        private readonly IGtsMotion _motion;

        /// <summary>
        /// 主轴锁
        /// </summary>
        public const string SpindleLockingCylinder = nameof(SpindleLockingCylinder);

        /// <summary>
        /// 手指左右
        /// </summary>
        public const string FingerSideToSide = nameof(FingerSideToSide);

        /// <summary>
        /// 手指翻转
        /// </summary>
        public const string FingerFlip = nameof(FingerFlip);

        /// <summary>
        /// 手指张闭
        /// </summary>
        public const string FingerClosure = nameof(FingerClosure);

        /// <summary>
        /// 线嘴翻转
        /// </summary>
        public const string LineNozzleFlip = nameof(LineNozzleFlip);

        /// <summary>
        /// 插排前后
        /// </summary>
        public const string PlugInForthBack = nameof(PlugInForthBack);

        /// <summary>
        /// 插排上下
        /// </summary>
        public const string PlugInUpDown = nameof(PlugInUpDown);

        /// <summary>
        /// 插排左右
        /// </summary>
        public const string PlugLeftRight = nameof(PlugLeftRight);

        /// <summary>
        /// 张力器
        /// </summary>
        public const string EnableTensioner = nameof(EnableTensioner);

        /// <summary>
        /// 自动上料前后
        /// </summary>
        public const string FeedingForthBack = nameof(FeedingForthBack);

        /// <summary>
        /// 自动上料左右
        /// </summary>
        public const string FeedingRightLeft = nameof(FeedingRightLeft);

        /// <summary>
        /// 直震分料上下
        /// </summary>
        public const string ShockSplitsUpDown = nameof(ShockSplitsUpDown);

        /// <summary>
        /// 直震分料吹气1
        /// </summary>
        public const string ShockSplitsBlow1 = nameof(ShockSplitsBlow1);

        /// <summary>
        /// 直震分料吹气1
        /// </summary>
        public const string ShockSplitsBlow2 = nameof(ShockSplitsBlow2);

        public CylinderFactory(IUnityContainer container, IGtsMotion motion)
        {
            this.container = container;
            _motion = motion;
            //todo:赶工期,待优化
            container.RegisterInstance<ICylinder>(SpindleLockingCylinder, GetAbCylinder(1, 4, 1, 5, 1, 8, 1, 9));
            container.RegisterInstance<ICylinder>(FingerSideToSide, GetCylinder(1, 7, 0, 12, 0, 13));
            container.RegisterInstance<ICylinder>(FingerFlip, GetAbCylinder(1, 8, 1, 9, 1, 0, 1, 1));
            container.RegisterInstance<ICylinder>(FingerClosure,GetNoSignalCylinder(1, 6));
            container.RegisterInstance<ICylinder>(LineNozzleFlip, GetAbCylinder(1, 0, 1, 1, 1, 2, 1, 3));

            container.RegisterInstance<ICylinder>(PlugInForthBack, GetAbCylinder(1, 10, 1, 11, 1, 10, 1, 11));
            container.RegisterInstance<ICylinder>(PlugInUpDown, GetAbCylinder(1, 12, 1, 13, 1, 12, 1, 13));


            container.RegisterInstance<ICylinder>(PlugLeftRight, GetCylinder(1, 14, 1, 14, 1, 15));
            container.RegisterInstance<ICylinder>(EnableTensioner, GetNoSignalCylinder(1, 15));
            container.RegisterInstance<ICylinder>(FeedingForthBack, GetCylinder(1, 2, 1, 4, 1, 5));
            container.RegisterInstance<ICylinder>(FeedingRightLeft, GetCylinder(1, 3, 1, 6, 1, 7));
            //这个的等待信号不知道对不对
            container.RegisterInstance<ICylinder>(ShockSplitsUpDown, GetCylinder(0, 13, 0, 10, 0, 11));
        }

        private NoSignalCylinder GetNoSignalCylinder(int oCh1, int oIndex1)
        {
            return new NoSignalCylinder(
                new[] { GetIoRef(false, oCh1, oIndex1) }, _motion);
        }

        private ABCylinder GetAbCylinder(
            int oCh1, int oIndex1,
            int oCh2, int oIndex2,
            int inCh1, int inIndex1,//原点
            int inCh2, int inIndex2//到位
        )
        {
            return new ABCylinder(

                new[] { GetIoRef(false, oCh1, oIndex1), GetIoRef(false, oCh2, oIndex2) },
                new[] { GetIoRef(true, inCh1, inIndex1), GetIoRef(true, inCh2, inIndex2) }, _motion);
        }

        private Cylinder GetCylinder(
            int oCh1, int oIndex1,
            int inCh1, int inIndex1,//原点
            int inCh2, int inIndex2//到位
        )
        {
            return new Cylinder(

                new[] { GetIoRef(false, oCh1, oIndex1) },
                new[] { GetIoRef(true, inCh1, inIndex1), GetIoRef(true, inCh2, inIndex2) }, _motion);
        }

        private OneSignalCylinder GetOneSignalCylinder(int oCh1, int oIndex1,
            int inCh1, int inIndex1/*到位*/)
        {
            return new OneSignalCylinder(

                new[] { GetIoRef(false, oCh1, oIndex1) },
                new[] { GetIoRef(true, inCh1, inIndex1) }, _motion);
        }

        private IoRef GetIoRef(bool io, int ch, int index)
        {
            var ioRef = io
                ? _motion.Dis.FirstOrDefault(x =>
                    x.Prm.Model == ch && x.Prm.Index == index && x.Prm.IoType == EIoType.NoamlInput)
                : _motion.Dos.FirstOrDefault(x =>
                    x.Prm.Model == ch && x.Prm.Index == index && x.Prm.IoType == EIoType.NomalOutput);
            return ioRef;
        }


        public ICylinder Create(string value)
        {
            return container.Resolve<ICylinder>(value);
        }
    }
}