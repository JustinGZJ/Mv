using System;

namespace Mv.TransferService
{
    public interface IManagedTransporterToken : IDisposable
    {
        void Suspend();

        void Ready();

        void AsNext();
    }
}
