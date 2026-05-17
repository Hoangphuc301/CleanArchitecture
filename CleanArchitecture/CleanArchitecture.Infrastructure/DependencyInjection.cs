using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Messaging;
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
            AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ArchitectureDbContext>( options => options.UseSqlServer( configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<INewRepository, NewRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();

            services.AddScoped<IMenuReadRepository, MenuReadRepository>();
            services.AddScoped<INewReadRepository, NewReadRepository>();

            services.AddSingleton<MongoDbContext>();

            // RabbitMQ
            services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqMenu"));
            services.Configure<RabbitMqNewSettings>(configuration.GetSection("RabbitMqNew"));

            // TỰ ĐỘNG CHẠY CONSUMER
            services.AddHostedService<MenuEventConsumer>();
            services.AddHostedService<NewEventConsumer>();

            return services;
        }
    }
}