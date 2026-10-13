using Grpc.Core;
using GrpcSpike.Corpus;

namespace GrpcSpike.Benchmarks;

/// <summary>What the stub benchmarks share: the request message, and the generated marshaller for it.</summary>
public static class StubSamples
{
    /// <summary>The populated <see cref="Scalars"/> sample: the service's only message type.</summary>
    public static Scalars Request { get; } =
        (Scalars)Samples.All.Single(s => s.Shape == "Scalars" && s.Variant == Samples.Populated).Message;

    /// <summary>
    /// The generated marshaller, captured through <c>BindService</c> the way grpc-dotnet discovers it; the generated
    /// field itself is private.
    /// </summary>
    public static Marshaller<Scalars> Marshaller { get; } = CaptureMarshaller();

    private static Marshaller<Scalars> CaptureMarshaller()
    {
        var binder = new MarshallerCapture();
        CorpusService.BindService(binder, null!);
        return binder.Marshaller ?? throw new InvalidOperationException("CorpusService bound no methods.");
    }

    private sealed class MarshallerCapture : ServiceBinderBase
    {
        public Marshaller<Scalars>? Marshaller { get; private set; }

        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, UnaryServerMethod<TRequest, TResponse>? handler) =>
            Capture(method);

        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, ServerStreamingServerMethod<TRequest, TResponse>? handler) =>
            Capture(method);

        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, ClientStreamingServerMethod<TRequest, TResponse>? handler) =>
            Capture(method);

        public override void AddMethod<TRequest, TResponse>(Method<TRequest, TResponse> method, DuplexStreamingServerMethod<TRequest, TResponse>? handler) =>
            Capture(method);

        private void Capture<TRequest, TResponse>(Method<TRequest, TResponse> method)
            where TRequest : class
            where TResponse : class =>
            Marshaller ??= method.RequestMarshaller as Marshaller<Scalars>;
    }
}
