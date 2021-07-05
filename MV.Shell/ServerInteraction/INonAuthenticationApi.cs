using System.Threading.Tasks;

namespace MV.Shell.ServerInteraction
{
    public interface INonAuthenticationApi
    {
        Task<bool> SignUpAsync(SignUpArgs args);
        Task<bool> LoginAsync(LoginArgs args);
    }

}
