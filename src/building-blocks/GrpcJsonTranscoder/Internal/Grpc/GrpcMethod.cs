using Google.Protobuf;
using Google.Protobuf.Reflection;
using Grpc.Core;
using System;
using System.Collections.Concurrent;

namespace GrpcJsonTranscoder.Internal.Grpc
{
    internal class GrpcMethod<TRequest, KResult> 
        where TRequest : class, IMessage<TRequest>, new()
        where KResult : class, IMessage<KResult>, new()
    {
        private static ConcurrentDictionary<MethodDescriptor, Method<TRequest, KResult>> _methods
            = new ConcurrentDictionary<MethodDescriptor, Method<TRequest, KResult>>();

        public static Method<TRequest, KResult> GetMethod(MethodDescriptor methodDescriptor)
        {
            if (_methods.TryGetValue(methodDescriptor, out Method<TRequest, KResult> method))
                return method;

            int mtype = 0;
            if (methodDescriptor.IsClientStreaming)
                mtype = 1;
            if (methodDescriptor.IsServerStreaming)
                mtype += 2;
            var methodType = (MethodType)Enum.ToObject(typeof(MethodType), mtype);

            var _method = new Method<TRequest, KResult>(
                methodType,
                methodDescriptor.Service.FullName,
                methodDescriptor.Name,
                ArgsParser<TRequest>.Marshaller,
                ArgsParser<KResult>.Marshaller);

            _methods.TryAdd(methodDescriptor, _method);

            return _method;
        }
    }

    internal static class ArgsParser<T> where T : class, IMessage<T>, new()
    {
        public static MessageParser<T> Parser = new MessageParser<T>(() => Factory<T>.CreateInstance());
        public static Marshaller<T> Marshaller = Marshallers.Create((arg) => MessageExtensions.ToByteArray(arg), Parser.ParseFrom);
    }
}
