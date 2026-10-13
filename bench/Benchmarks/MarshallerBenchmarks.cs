using System.Buffers;
using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using Grpc.Core;
using GrpcSpike.Corpus;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// The generated marshaller alone (task 4.3, design D5), for the message <see cref="EndToEndBenchmarks"/> sends. The
/// contexts are minimal and reused, so steady state they allocate nothing themselves.
/// </summary>
public class MarshallerBenchmarks
{
    private readonly Scalars message = StubSamples.Request;
    private readonly Marshaller<Scalars> marshaller = StubSamples.Marshaller;
    private readonly ReusableSerializationContext serialization = new();
    private ReusableDeserializationContext deserialization = null!;

    [GlobalSetup]
    public void Setup() => deserialization = new ReusableDeserializationContext(message.ToByteArray());

    [Benchmark]
    public void Serialize()
    {
        serialization.Reset();
        marshaller.ContextualSerializer(message, serialization);
    }

    [Benchmark]
    public Scalars Deserialize() => marshaller.ContextualDeserializer(deserialization);

    /// <summary>Gives the serializer a buffer writer, as grpc-dotnet does, rather than taking a <c>byte[]</c>.</summary>
    private sealed class ReusableSerializationContext : SerializationContext
    {
        private readonly ArrayBufferWriter<byte> writer = new();

        public void Reset() => writer.ResetWrittenCount();

        public override void SetPayloadLength(int payloadLength)
        {
        }

        public override IBufferWriter<byte> GetBufferWriter() => writer;

        public override void Complete()
        {
        }
    }

    /// <summary>Exposes the payload as a <see cref="ReadOnlySequence{T}"/>, as grpc-dotnet does.</summary>
    private sealed class ReusableDeserializationContext(byte[] payload) : DeserializationContext
    {
        private readonly ReadOnlySequence<byte> sequence = new(payload);

        public override int PayloadLength => payload.Length;

        public override byte[] PayloadAsNewBuffer() => payload.ToArray();

        public override ReadOnlySequence<byte> PayloadAsReadOnlySequence() => sequence;
    }
}
