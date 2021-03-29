using DbProject.Interfaces;
using DbProject.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DbProject
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbProjectServiceCollection(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
