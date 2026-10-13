using BenchmarkDotNet.Attributes;
using Google.Protobuf;
using GrpcSpike.Corpus;
using V1 = GrpcSpike.Corpus.Unknown.V1;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// A v1 reader meeting fields only v2 knows (report M-26/M-27). <see cref="MessageBenchmarks"/> parses each record
/// with its own parser, so it never builds an <c>UnknownFieldSet</c>.
/// </summary>
public class UnknownFieldBenchmarks
{
    private readonly byte[] v2Payload = Samples.UnknownFieldsWriter().ToByteArray();
    private V1.Record parsedWithUnknowns = null!;

    [GlobalSetup]
    public void Setup() => parsedWithUnknowns = V1.Record.Parser.ParseFrom(v2Payload);

    [Benchmark]
    public V1.Record ParseV2AsV1() => V1.Record.Parser.ParseFrom(v2Payload);

    [Benchmark]
    public byte[] ReserializeUnknownsAsV1() => parsedWithUnknowns.ToByteArray();

    [Benchmark]
    public V1.Record CloneWithUnknowns() => parsedWithUnknowns.Clone();
}
