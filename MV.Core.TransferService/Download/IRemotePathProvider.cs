using System.Threading.Tasks;

namespace Mv.TransferService
{
    public interface IRemotePathProvider /*: IPersistable<IRemotePathProvider>*/
    {
        void Rate(string remotePath, double score);

        Task<string> GetAsync();
    }
}
