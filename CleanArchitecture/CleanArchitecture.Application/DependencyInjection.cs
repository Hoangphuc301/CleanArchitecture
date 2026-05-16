using CleanArchitecture.Application.Common.Behaviors;
using CleanArchitecture.Application.Common.Mapping;
using FluentValidation;
using MediatR;
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

                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); // Đăng ký tất cả các validator trong assembly
                                                                                 // Hoạt động bằng cách cho các class kế thừa AbstractValidator<T>

            return services;
        }
    }
}