using Mv.Core;
using Mv.Modules.Schneider.ViewModels;

namespace Mv.Modules.Schneider.Service
{


    public interface IServerOperations
    {
        int CheckCode(string refId);
        int Upload(UploadDataCollection json);
    }
}