using System;

namespace Mv.Modules.Schneider.Service
{
    public interface IRemoteIOService
    {
        bool[] outputs { get; }

        event Action<bool[]> OnRecieve;
    }
}