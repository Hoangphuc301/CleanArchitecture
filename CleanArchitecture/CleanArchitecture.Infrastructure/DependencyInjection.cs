using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Persistence.Mongo;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection
            AddInfrastructureServices(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            services.AddDbContext<ArchitectureDbContext>( options => options.UseSqlServer( configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<INewRepository, NewRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();

            services.AddScoped<IMenuReadRepository, MenuReadRepository>();

            services.AddSingleton<MongoDbContext>();

            // RabbitMQ
            services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

            // TỰ ĐỘNG CHẠY CONSUMER
            services.AddHostedService<MenuEventConsumer>();

            return services;
        }
    }
}