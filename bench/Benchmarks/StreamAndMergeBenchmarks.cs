using BenchmarkDotNet.Attributes;
using GrpcSpike.Corpus;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// Every corpus sample × the operations off the gRPC path that the code reading flagged (task 5.1): parsing from and
/// writing to a <see cref="Stream"/> (report M-08, M-21, M-23), and parsing bytes into a populated message (M-14).
/// Kept apart from <see cref="MessageBenchmarks"/> so its committed rows stay as they were run.
/// </summary>
public class StreamAndMergeBenchmarks
{
    private MessageOperations operations = null!;

    public static IEnumerable<Sample> AllSamples => Samples.All;

    [ParamsSource(nameof(AllSamples))]
    public Sample Sample { get; set; } = null!;

    [GlobalSetup]
    public void Setup() => operations = MessageOperations.For(Sample);

    [Benchmark]
    public object ParseStream() => operations.ParseStream();

    [Benchmark]
    public void WriteToStream() => operations.WriteToStream();

    [Benchmark]
    public object MergeBytesIntoPopulated() => operations.MergeBytesIntoPopulated();
}
