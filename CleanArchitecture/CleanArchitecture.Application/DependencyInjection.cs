using CleanArchitecture.Application.Common.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace CleanArchitecture.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(typeof(ApplicationMappingProfile));
            });

            return services;
        }
    }
}