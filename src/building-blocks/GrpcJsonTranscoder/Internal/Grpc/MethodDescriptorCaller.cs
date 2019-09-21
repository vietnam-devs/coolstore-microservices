using Google.Protobuf;
using Google.Protobuf.Reflection;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcJsonTranscoder.Internal.Grpc
{
    internal class MethodDescriptorCaller : ClientBase<MethodDescriptorCaller>
    {
        public MethodDescriptorCaller()
        {
        }

        public MethodDescriptorCaller(Channel channel) : base(channel)
        {
        }

        protected MethodDescriptorCaller(ClientBaseConfiguration configuration) : base(configuration)
        {
        }

        protected override MethodDescriptorCaller NewInstance(ClientBaseConfiguration configuration)
        {
            return new MethodDescriptorCaller(configuration);
        }

        public Task<object> InvokeAsync(MethodDescriptor method, IDictionary<string, string> headers, object requestObject)
        {
            object requests;

            if (requestObject != null && typeof(IEnumerable<>).MakeGenericType(method.InputType.ClrType).IsAssignableFrom(requestObject.GetType()))
            {
                requests = requestObject;
            }
            else
            {
                Array ary = Array.CreateInstance(method.InputType.ClrType, 1);
                ary.SetValue(requestObject, 0);
                requests = ary;
            }

            System.Reflection.MethodInfo m = typeof(MethodDescriptorCaller).GetMethod("CallGrpcAsyncCore", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            Task<object> task = (Task<object>)m.MakeGenericMethod(new Type[] { method.InputType.ClrType, method.OutputType.ClrType }).Invoke(this, new object[] { method, headers, requests });

            return task;
        }

        [DebuggerStepThrough]
        private Task<object> CallGrpcAsyncCore<TRequest, TResponse>(MethodDescriptor method, IDictionary<string, string> headers, IEnumerable<TRequest> requests) 
            where TRequest : class, IMessage<TRequest>, new()
            where TResponse : class, IMessage<TResponse>, new()
        {
            CallOptions option = CreateCallOptions(headers);
            var rpc = GrpcMethod<TRequest, TResponse>.GetMethod(method);
            switch (rpc.Type)
            {
                case MethodType.Unary:

                    Task<TResponse> taskUnary = AsyncUnaryCall(CallInvoker, rpc, option, requests.FirstOrDefault());

                    var s = taskUnary.Result;
                    return Task.FromResult<object>(taskUnary.Result);

                case MethodType.ClientStreaming:

                    Task<TResponse> taskClientStreaming = AsyncClientStreamingCall(CallInvoker, rpc, option, requests);

                    return Task.FromResult<object>(taskClientStreaming.Result);

                case MethodType.ServerStreaming:

                    Task<IList<TResponse>> taskServerStreaming = AsyncServerStreamingCall(CallInvoker, rpc, option, requests.FirstOrDefault());

                    return Task.FromResult<object>(taskServerStreaming.Result);

                case MethodType.DuplexStreaming:

                    Task<IList<TResponse>> taskDuplexStreaming = AsyncDuplexStreamingCall(CallInvoker, rpc, option, requests);

                    return Task.FromResult<object>(taskDuplexStreaming.Result);

                default:
                    throw new NotSupportedException(string.Format("MethodType '{0}' is not supported.", rpc.Type));
            }
        }

        private CallOptions CreateCallOptions(IDictionary<string, string> headers)
        {
            Metadata meta = new Metadata();

            foreach (KeyValuePair<string, string> entry in headers)
            {
                meta.Add(entry.Key, entry.Value);
            }

            CallOptions option = new CallOptions(meta);

            return option;
        }

        private Task<TResponse> AsyncUnaryCall<TRequest, TResponse>(CallInvoker invoker, Method<TRequest, TResponse> method, CallOptions option, TRequest request) where TRequest : class where TResponse : class
        {
            return invoker.AsyncUnaryCall(method, null, option, request).ResponseAsync;
        }

        private async Task<TResponse> AsyncClientStreamingCall<TRequest, TResponse>(CallInvoker invoker, Method<TRequest, TResponse> method, CallOptions option, IEnumerable<TRequest> requests) where TRequest : class where TResponse : class
        {
            using (AsyncClientStreamingCall<TRequest, TResponse> call = invoker.AsyncClientStreamingCall(method, null, option))
            {
                if (requests != null)
                {
                    foreach (TRequest request in requests)
                    {
                        await call.RequestStream.WriteAsync(request).ConfigureAwait(false);
                    }
                }

                await call.RequestStream.CompleteAsync().ConfigureAwait(false);

                return call.ResponseAsync.Result;
            }
        }

        private async Task<IList<TResponse>> AsyncServerStreamingCall<TRequest, TResponse>(CallInvoker invoker, Method<TRequest, TResponse> method, CallOptions option, TRequest request) where TRequest : class where TResponse : class
        {
            using (AsyncServerStreamingCall<TResponse> call = invoker.AsyncServerStreamingCall(method, null, option, request))
            {
                List<TResponse> responses = new List<TResponse>();

                while (await call.ResponseStream.MoveNext().ConfigureAwait(false))
                {
                    responses.Add(call.ResponseStream.Current);
                }

                return responses;
            }
        }

        private async Task<IList<TResponse>> AsyncDuplexStreamingCall<TRequest, TResponse>(CallInvoker invoker, Method<TRequest, TResponse> method, CallOptions option, IEnumerable<TRequest> requests) where TRequest : class where TResponse : class
        {
            using (AsyncDuplexStreamingCall<TRequest, TResponse> call = invoker.AsyncDuplexStreamingCall(method, null, option))
            {
                if (requests != null)
                {
                    foreach (TRequest request in requests)
                    {
                        await call.RequestStream.WriteAsync(request).ConfigureAwait(false);
                    }
                }

                await call.RequestStream.CompleteAsync().ConfigureAwait(false);

                List<TResponse> responses = new List<TResponse>();

                while (await call.ResponseStream.MoveNext().ConfigureAwait(false))
                {
                    responses.Add(call.ResponseStream.Current);
                }

                return responses;
            }
        }
    }
}
