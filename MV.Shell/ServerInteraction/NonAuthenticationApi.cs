using System.Threading.Tasks;

namespace MV.Shell.ServerInteraction
{
    public class NonAuthenticationApi : INonAuthenticationApi
    {
        Task<bool> INonAuthenticationApi.SignUpAsync(SignUpArgs args)
        {
            return Task.FromResult(true);
        }
        Task<bool> INonAuthenticationApi.LoginAsync(LoginArgs args)
        {
            return Task.FromResult(true);
        }
    }

}
