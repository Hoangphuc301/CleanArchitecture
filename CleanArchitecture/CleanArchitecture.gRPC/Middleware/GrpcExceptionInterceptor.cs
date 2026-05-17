using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.Grpc.Interceptors
{
    public class GrpcExceptionInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );

                var errorJson = JsonSerializer.Serialize(new
                {
                    message = "Xác thực thất bại",
                    errors
                });

                var metadata = new Metadata { { "validation-errors-json", errorJson } };

                throw new RpcException(new Status(StatusCode.InvalidArgument, "Xác thực dữ liệu thất bại"), metadata);
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }
    }
}