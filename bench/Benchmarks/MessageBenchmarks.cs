using BenchmarkDotNet.Attributes;
using GrpcSpike.Corpus;

namespace GrpcSpike.Benchmarks;

/// <summary>Every corpus sample × every message operation (task 4.2; spec "Per-shape message operations are measured").</summary>
public class MessageBenchmarks
{
    private MessageOperations operations = null!;

    public static IEnumerable<Sample> AllSamples => Samples.All;

    [ParamsSource(nameof(AllSamples))]
    public Sample Sample { get; set; } = null!;

    [GlobalSetup]
    public void Setup() => operations = MessageOperations.For(Sample);

    [Benchmark]
    public object New() => operations.New();

    [Benchmark]
    public object ParseByteArray() => operations.ParseByteArray();

    [Benchmark]
    public object ParseSpan() => operations.ParseSpan();

    [Benchmark]
    public byte[] ToByteArray() => operations.ToByteArray();

    [Benchmark]
    public void WriteToSpan() => operations.WriteToSpan();

    [Benchmark]
    public void WriteToSegmentedWriter() => operations.WriteToSegmentedWriter();

    [Benchmark]
    public int CalculateSize() => operations.CalculateSize();

    [Benchmark]
    public object Clone() => operations.Clone();

    [Benchmark]
    public object MergeIntoNew() => operations.MergeIntoNew();

    [Benchmark]
    public object MergeIntoPopulated() => operations.MergeIntoPopulated();
}
