using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Grpc.Services;
using CleanArchitecture.Infrastructure.Messaging;
using CleanArchitecture.Infrastructure.Persistence.Mongo;
using CleanArchitecture.Grpc.Interceptors;
using FluentValidation;
using CleanArchitecture.Application.Common.Behaviors;
using MediatR;
var builder = WebApplication.CreateBuilder(args);

// Add configuration for MongoDB and RabbitMQ
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));

// Add gRPC exception
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcExceptionInterceptor>();
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddValidatorsFromAssembly(System.Reflection.Assembly.Load("CleanArchitecture.Application"));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Add gRPC reflection
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<NewGrpcService>();
app.MapGrpcService<MenuGrpcService>();

// Add gRPC reflection endpoint
app.MapGrpcReflectionService();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
