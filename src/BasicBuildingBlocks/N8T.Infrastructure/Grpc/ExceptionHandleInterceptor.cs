using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using N8T.Infrastructure.Validator;
using Serilog;

namespace N8T.Infrastructure.Grpc
{
    public class ExceptionHandleInterceptor : Interceptor
    {
        private const string MessageTemplate =
            "{RequestMethod} responded {StatusCode} with error message {ErrorMessage}";

        [DebuggerStepThrough]
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
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

                throw new RpcException(new global::Grpc.Core.Status(StatusCode.Internal, ex.ValidationResultModel.ToString()));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(MessageTemplate,
                    context.Method,
                    context.Status.StatusCode,
                    ex.Message);

                throw new RpcException(new global::Grpc.Core.Status(StatusCode.Internal, ex.Message));
            }
        }
    }
}
