using Google.Protobuf;
using Google.Protobuf.Collections;
using Xunit;
using V1 = GrpcSpike.Corpus.Unknown.V1;
using V2 = GrpcSpike.Corpus.Unknown.V2;

namespace GrpcSpike.Corpus.Tests;

/// <summary>
/// Pins the cost of each confirmed finding in <c>docs/analysis/report.md</c> (design D6). The bytes are the ones the
/// report quotes from the committed benchmark results. A version bump that changes a cost fails here, so the report
/// is rechecked. M-18 is CPU only and has no allocation to pin.
/// </summary>
public class AllocationTests
{
    // Tiered compilation promotes a method only after a 100 ms quiet period, then through an instrumented tier. Warming
    // up for longer than that measures the optimised code, as BenchmarkDotNet does. Unoptimised code allocates more:
    // M-27's two enumerators (2 × 56 B) are only removed once optimised.
    private static readonly TimeSpan WarmupTime = TimeSpan.FromMilliseconds(500);
    private static readonly TimeSpan OptimisationDeadline = TimeSpan.FromSeconds(10);
    private const int MeasuredRuns = 20;

    // Keeps results reachable, so the JIT can't treat them as unused and stack-allocate them.
    private static object? sink;

    [Fact]
    public void M01_ParsingASetScalarOneofMemberBoxesIt()
    {
        var populated = ParseSpanBytes(Sample<Oneof>("Oneof", "populated-int32"));
        var empty = ParseSpanBytes(Sample<Oneof>("Oneof", "empty"));

        Assert.Equal(64, populated);
        Assert.Equal(24, populated - empty);
    }

    [Fact]
    public void M02_CloneAndMergeBoxTheScalarAgain()
    {
        var source = Sample<Oneof>("Oneof", "populated-int32");
        var empty = Sample<Oneof>("Oneof", "empty");
        var target = source.Clone();

        Assert.Equal(24, Allocated(() => source.Clone()) - Allocated(() => empty.Clone()));
        Assert.Equal(24, Allocated(() => target.MergeFrom(source)));
    }

    [Fact]
    public void M03_ParsedMapEntriesCarryContainerOverhead() =>
        Assert.Equal(24_552, ParseSpanBytes(Sample<Maps>("Maps", "populated")));

    [Fact]
    public void M04_EveryEmptyMapFieldAllocatesItsContainers()
    {
        Assert.Equal(152, Allocated(() => new MapField<int, long>()));
        Assert.Equal(48 + (3 * 152), Allocated(() => new Maps()));
    }

    [Fact]
    public void M05_WritingANonEmptyMapBoxesItsEnumerator()
    {
        Assert.Equal(3 * 56, WriteToSpanBytes(Sample<Maps>("Maps", "populated")));
        Assert.Equal(0, WriteToSpanBytes(Sample<Maps>("Maps", "empty")));
    }

    [Fact]
    public void M08_ParsingFromAStreamCopiesLongStringsAndBytes()
    {
        var strings = Sample<Strings>("Strings", "populated");
        var bytes = Sample<Bytes>("Bytes", "populated");

        Assert.Equal(4_288 + 33_096, ParseStreamBytes(strings) - ParseSpanBytes(strings));
        Assert.Equal(4_288 + 16_688, ParseStreamBytes(bytes) - ParseSpanBytes(bytes));
    }

    [Fact]
    public void M10_WritingALongStringToSegmentedOutputEncodesItIntoATemporaryArray() =>
        Assert.Equal(4_184 + 16_408, WriteToStreamBytes(Sample<Strings>("Strings", "populated")));

    [Fact]
    public void M12_AnEmptyPresentBytesValueAllocates()
    {
        var present = ParseSpanBytes(Sample<Optional>("Optional", "edge-defaults-present"));
        var absent = ParseSpanBytes(Sample<Optional>("Optional", "empty"));

        Assert.Equal(56, present - absent);
    }

    [Fact]
    public void M14_MergingIntoASetOneofMessageBuildsANewSubMessage()
    {
        var source = Sample<Oneof>("Oneof", "populated-message");
        var payload = source.ToByteArray();
        var target = source.Clone();

        // A new OneofPayload (40 B) plus its name (40 B); merging in place would need only the name.
        Assert.Equal(80, Allocated(() => target.MergeFrom(new ReadOnlySpan<byte>(payload))));
    }

    [Fact]
    public void M16_RepeatedFieldsGrowByDoublingWhileParsing() =>
        Assert.Equal(8_856, ParseSpanBytes(Sample<Repeated>("Repeated", "populated")));

    [Fact]
    public void M17_CloneCopiesSlackCapacity()
    {
        var source = Sample<Repeated>("Repeated", "populated");

        Assert.Equal(4_984, Allocated(() => source.Clone()));
    }

    [Fact]
    public void M20_ParsingAByteArrayAllocatesACodedInputStream()
    {
        var sample = Sample<Scalars>("Scalars", "populated");
        var payload = sample.ToByteArray();

        Assert.Equal(168, Allocated(() => Scalars.Parser.ParseFrom(payload)) - ParseSpanBytes(sample));
    }

    [Fact]
    public void M21_ParsingFromAStreamAllocatesABufferAndACodedInputStream()
    {
        var sample = Sample<Scalars>("Scalars", "populated");

        Assert.Equal(4_288, ParseStreamBytes(sample) - ParseSpanBytes(sample));
    }

    [Fact]
    public void M22_ToByteArrayAllocatesACodedOutputStream()
    {
        var sample = Sample<Scalars>("Scalars", "empty");

        Assert.Equal(24 + 64, Allocated(() => sample.ToByteArray()));
    }

    [Fact]
    public void M23_WritingToAStreamAllocatesABufferAndACodedOutputStream() =>
        Assert.Equal(4_184, WriteToStreamBytes(Sample<Scalars>("Scalars", "populated")));

    [Fact]
    public void M24_EveryEmptyRepeatedFieldIsAnObject()
    {
        Assert.Equal(32, Allocated(() => new RepeatedField<int>()));
        Assert.Equal(72 + (6 * 32), Allocated(() => new Repeated()));
    }

    [Fact]
    public void M25_CloneDiscardsTheCollectionsItsInitialisersCreated()
    {
        var maps = Sample<Maps>("Maps", "empty");
        var repeated = Sample<Repeated>("Repeated", "empty");

        Assert.Equal(504 + (3 * 152), AllocatedOnceOptimised(504 + (3 * 152), () => maps.Clone()));
        Assert.Equal(264 + (6 * 32), Allocated(() => repeated.Clone()));
    }

    [Fact]
    public void M26_UnknownFieldsAreDecodedIntoAnUnknownFieldSet()
    {
        var payload = Samples.UnknownFieldsWriter().ToByteArray();
        var withUnknowns = V1.Record.Parser.ParseFrom(payload);
        var known = V2.Record.Parser.ParseFrom(payload);

        Assert.Equal(1_056, Allocated(() => V1.Record.Parser.ParseFrom(payload)) - Allocated(() => V2.Record.Parser.ParseFrom(payload)));
        Assert.Equal(536, Allocated(() => withUnknowns.Clone()) - Allocated(() => known.Clone()));
    }

    [Fact]
    public void M27_Rejected_WritingUnknownFieldsAllocatesNoEnumerator()
    {
        var payload = Samples.UnknownFieldsWriter().ToByteArray();
        var withUnknowns = V1.Record.Parser.ParseFrom(payload);
        var known = V2.Record.Parser.ParseFrom(payload);

        Assert.Equal(136, AllocatedOnceOptimised(136, () => known.ToByteArray()));
        Assert.Equal(136, AllocatedOnceOptimised(136, () => withUnknowns.ToByteArray()));
    }

    private static T Sample<T>(string shape, string variant)
        where T : IMessage<T> =>
        (T)Samples.All.Single(s => s.Shape == shape && s.Variant == variant).Message;

    private static long ParseSpanBytes<T>(T sample)
        where T : IMessage<T>
    {
        var payload = sample.ToByteArray();
        var parser = (MessageParser<T>)sample.Descriptor.Parser;
        return Allocated(() => parser.ParseFrom(new ReadOnlySpan<byte>(payload)));
    }

    private static long ParseStreamBytes<T>(T sample)
        where T : IMessage<T>
    {
        var input = new MemoryStream(sample.ToByteArray(), writable: false);
        var parser = (MessageParser<T>)sample.Descriptor.Parser;
        return Allocated(() =>
        {
            input.Position = 0;
            return parser.ParseFrom(input);
        });
    }

    private static long WriteToSpanBytes(IMessage sample)
    {
        var destination = new byte[sample.CalculateSize()];
        return Allocated(() =>
        {
            sample.WriteTo(destination.AsSpan());
            return destination;
        });
    }

    private static long WriteToStreamBytes(IMessage sample)
    {
        var output = new MemoryStream(sample.CalculateSize());
        return Allocated(() =>
        {
            output.Position = 0;
            sample.WriteTo(output);
            return output;
        });
    }

    /// <summary>
    /// For costs that only the fully optimised code reaches (M-25's map clone, M-27): the JIT removes their boxed
    /// enumerators only after tiered PGO, so on a busy machine one warm-up may not be enough. This keeps measuring
    /// until it sees <paramref name="expected"/> or the deadline passes. A real change still fails, because its bytes
    /// never reach the expected value.
    /// </summary>
    private static long AllocatedOnceOptimised(long expected, Func<object?> operation)
    {
        var deadline = System.Diagnostics.Stopwatch.StartNew();
        long measured;
        do
        {
            measured = Allocated(operation);
        }
        while (measured != expected && deadline.Elapsed < OptimisationDeadline);

        return measured;
    }

    private static long Allocated(Action operation) =>
        Allocated(() =>
        {
            operation();
            return null;
        });

    /// <summary>
    /// The fewest bytes one call allocated on this thread, after warm-up. Tiered compilation can change what is
    /// allocated (for example by stack-allocating an object), so the minimum is the cost once fully optimised.
    /// </summary>
    private static long Allocated(Func<object?> operation)
    {
        var warmup = System.Diagnostics.Stopwatch.StartNew();
        while (warmup.Elapsed < WarmupTime)
        {
            sink = operation();
        }

        var fewest = long.MaxValue;
        for (var i = 0; i < MeasuredRuns; i++)
        {
            var before = GC.GetAllocatedBytesForCurrentThread();
            sink = operation();
            fewest = Math.Min(fewest, GC.GetAllocatedBytesForCurrentThread() - before);
        }

        return fewest;
    }
}
