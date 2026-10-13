using System.Buffers;
using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using GrpcSpike.Corpus;
using GrpcSpike.Reference;
using ProtoBuf;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// protobuf-net doing the same work on the same data as <see cref="MessageBenchmarks"/> (task 4.5). Reference only:
/// it is not a drop-in replacement and never enters go/no-go (ADR-0002, design D8). The category column marks the
/// rows. Each method is named after the <see cref="MessageBenchmarks"/> row it compares with.
/// </summary>
[BenchmarkCategory(Category)]
public class ReferenceBenchmarks
{
    public const string Category = "Reference";

    private ReferenceOperations operations = null!;

    /// <summary>The comparable shapes' samples; the reference object is read from the generated code's bytes.</summary>
    public static IEnumerable<Sample> ComparableSamples => Samples.All.Where(s => ReferenceShapes.ByShape.ContainsKey(s.Shape));

    [ParamsSource(nameof(ComparableSamples))]
    public Sample Sample { get; set; } = null!;

    [GlobalSetup]
    public void Setup() => operations = ReferenceOperations.For(Sample);

    [Benchmark]
    public object ParseSpan() => operations.ParseSpan();

    /// <summary>protobuf-net writes to a buffer writer or stream, not a span; compare with <c>WriteToSpan</c>.</summary>
    [Benchmark]
    public void WriteToBufferWriter() => operations.WriteToBufferWriter();

    [Benchmark]
    public long CalculateSize() => operations.CalculateSize();

    [Benchmark]
    public object Clone() => operations.Clone();
}

public abstract class ReferenceOperations
{
    public static ReferenceOperations For(Sample sample)
    {
        var type = ReferenceShapes.ByShape[sample.Shape];
        var payload = sample.Message.ToByteArray();
        var source = Serializer.NonGeneric.Deserialize(type, new MemoryStream(payload));
        return (ReferenceOperations)Activator.CreateInstance(typeof(ReferenceOperations<>).MakeGenericType(type), source, payload)!;
    }

    public abstract object ParseSpan();

    public abstract void WriteToBufferWriter();

    public abstract long CalculateSize();

    public abstract object Clone();
}

public sealed class ReferenceOperations<T>(T source, byte[] payload) : ReferenceOperations
    where T : class
{
    private readonly ArrayBufferWriter<byte> writer = new(payload.Length + 1);

    public override object ParseSpan() => Serializer.Deserialize<T>(new ReadOnlySpan<byte>(payload));

    public override void WriteToBufferWriter()
    {
        writer.ResetWrittenCount();
        Serializer.Serialize(writer, source);
    }

    public override long CalculateSize()
    {
        using var measured = Serializer.Measure(source);
        return measured.Length;
    }

    public override object Clone() => Serializer.DeepClone(source);
}
