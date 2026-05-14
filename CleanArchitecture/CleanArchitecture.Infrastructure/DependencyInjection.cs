using CleanArchitecture.Application.Common.Behaviors;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ArchitectureDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<INewRepository, NewRepository>();

        services.AddTransient( typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));

        return services;
    }
}