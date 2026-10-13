using Google.Protobuf;
using GrpcSpike.Corpus;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// The per-shape operations (task 4.2), bound to a sample's concrete message type so every call goes through the
/// generated code's typed paths (<c>MergeFrom(T)</c>, <c>Clone()</c>, <c>MessageParser&lt;T&gt;</c>) as an
/// application's would.
/// </summary>
public abstract class MessageOperations
{
    /// <summary>Segment size for <see cref="WriteToSegmentedWriter"/>: shorter than the corpus's longer strings.</summary>
    public const int SegmentSize = 32;

    public static MessageOperations For(Sample sample) =>
        (MessageOperations)Activator.CreateInstance(
            typeof(MessageOperations<>).MakeGenericType(sample.Message.GetType()), sample.Message)!;

    public abstract object New();

    public abstract object ParseByteArray();

    public abstract object ParseSpan();

    public abstract byte[] ToByteArray();

    public abstract void WriteToSpan();

    public abstract void WriteToSegmentedWriter();

    public abstract int CalculateSize();

    public abstract object Clone();

    public abstract object MergeIntoNew();

    public abstract object MergeIntoPopulated();
}

public sealed class MessageOperations<T>(T source) : MessageOperations
    where T : IMessage<T>, new()
{
    private readonly MessageParser<T> parser = (MessageParser<T>)source.Descriptor.Parser;
    private readonly byte[] payload = source.ToByteArray();
    private readonly byte[] destination = new byte[source.CalculateSize()];
    private readonly SegmentedBufferWriter segmented = new(SegmentSize);

    public override object New() => new T();

    public override object ParseByteArray() => parser.ParseFrom(payload);

    public override object ParseSpan() => parser.ParseFrom(new ReadOnlySpan<byte>(payload));

    public override byte[] ToByteArray() => source.ToByteArray();

    public override void WriteToSpan() => source.WriteTo(destination.AsSpan());

    public override void WriteToSegmentedWriter()
    {
        segmented.Reset();
        source.WriteTo(segmented);
    }

    public override int CalculateSize() => source.CalculateSize();

    public override object Clone() => source.Clone();

    /// <summary>Includes the cost of <see cref="New"/>; subtract that row for the merge alone.</summary>
    public override object MergeIntoNew()
    {
        var target = new T();
        target.MergeFrom(source);
        return target;
    }

    /// <summary>Includes the cost of <see cref="Clone"/>; subtract that row for the merge alone.</summary>
    public override object MergeIntoPopulated()
    {
        var target = source.Clone();
        target.MergeFrom(source);
        return target;
    }
}
