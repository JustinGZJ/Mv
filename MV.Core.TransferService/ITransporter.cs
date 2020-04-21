using System;
using System.Collections.Generic;

namespace Mv.TransferService
{
    public interface ITransferInfo<out T> : IObservable<TransferNotification>, IDisposable
    {
        Guid Id { get; }

        T Context { get; }

        TransferStatus Status { get; }

        IReadOnlyDictionary<long, BlockTransferContext> BlockContexts { get; }
    }

    public interface ITransferOperations
    {
        void Run();

        void Stop();
    }

    public interface IDownloader : ITransferInfo<DownloadContext>, ITransferOperations
    {
    }
}
