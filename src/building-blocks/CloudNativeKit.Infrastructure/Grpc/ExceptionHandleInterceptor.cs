using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.ValidationModel;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Serilog;

namespace CloudNativeKit.Infrastructure.Grpc
{
    public class ExceptionHandleInterceptor : Interceptor
    {
        private const string MessageTemplate = "{RequestMethod} responded {StatusCode} with error message {ErrorMessage}";

        [DebuggerStepThrough]
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                var response = await base.UnaryServerHandler(request, context, continuation);
                return response;
            }
            catch (ValidationException ex)
            {
                Log.Logger.Error(MessageTemplate,
                    context.Method,
                    context.Status.StatusCode,
                    ex.ValidationResultModel.ToString());

                throw new RpcException(new Status(StatusCode.Internal, ex.ValidationResultModel.ToString()));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(MessageTemplate,
                    context.Method,
                    context.Status.StatusCode,
                    ex.Message);

                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }
    }
}
