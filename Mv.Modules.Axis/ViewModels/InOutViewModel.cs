using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using MotionWrapper;
using Unity;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Reactive.Linq;
using System.Windows;
using HandyControl.Data;

namespace Mv.Modules.Axis.ViewModels
{
    public class InOutViewModel : BindableBase
    {
        private readonly ICMotionData data;

        IIoPart1 part1;
        /// <summary>
        ///     页码
        /// </summary>
        private int _pageIndex = 1;

        /// <summary>
        ///     页码
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set => SetProperty(ref _pageIndex, value);
        }

        private EIoType ioType;
        public EIoType IoType
        {
            get => ioType;
            set
            {
                if (SetProperty(ref ioType, value))
                {
                    PageIndex = 1;
                    MaxPageCount = (int)Math.Ceiling((totalList.Count / (double)DataCountPerPage));
                    UpdatePage(PageIndex);
                }
            }
        }

        public int MaxPageCount
        {
            get => maxPageCount;
            set => SetProperty(ref maxPageCount, value);
        }

        private int maxPageCount;

        List<IoRef> totalList => data.Dis.Concat(data.Dos).Where(io => io.Name != "").Where(io => io.Prm.IoType == ioType).ToList();
        private DelegateCommand<FunctionEventArgs<int>> pageUpdatedCmd;
        public DelegateCommand<FunctionEventArgs<int>> PageUpdatedCmd =>
pageUpdatedCmd ??= new DelegateCommand<FunctionEventArgs<int>>(PageUpdated);


        public InOutViewModel(IUnityContainer container)
        {
            data = container.Resolve<IGtsMotion>();
            part1 = container.Resolve<IGtsMotion>();
            IoType = EIoType.NoamlInput;
        }
        private int dataCountPerPage = 16;
        public int DataCountPerPage
        {
            get { return dataCountPerPage; }
            set { SetProperty(ref dataCountPerPage, value); }
        }
        //  List<IoRef> totalList
        public ObservableCollection<IoRef> DataList { get; set; } = new ObservableCollection<IoRef>();

        private void PageUpdated(FunctionEventArgs<int> info)
        {
            UpdatePage(info.Info);
        }

        private void UpdatePage(int page)
        {
            DataList = new ObservableCollection<IoRef>(totalList.Skip((page - 1) * DataCountPerPage).Take(DataCountPerPage));
        }

        private DelegateCommand<IoRef> reverseOutput;
        public DelegateCommand<IoRef> CmdReverseOutput =>
reverseOutput ??= new DelegateCommand<IoRef>(ExecuteCmdReverseOutput);

        void ExecuteCmdReverseOutput(IoRef oldvalue)
        {
            part1.setDO(oldvalue, !oldvalue.Value);
        }
    }
}
