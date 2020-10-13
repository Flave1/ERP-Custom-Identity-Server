using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GODP.APIsContinuation.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
