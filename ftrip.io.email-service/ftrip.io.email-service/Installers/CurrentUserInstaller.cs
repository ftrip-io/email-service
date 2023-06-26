using ftrip.io.framework.Contexts;
using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.email_service.Installers
{
    public class CurrentUserInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public CurrentUserInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddScoped(typeof(CurrentUserContext));
        }
    }
}