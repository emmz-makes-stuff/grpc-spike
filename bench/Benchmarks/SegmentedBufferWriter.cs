using System.Buffers;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// A reusable <see cref="IBufferWriter{T}"/> that hands out small fixed-size segments, like a pipe writer does, so a
/// value longer than the space left in the current segment has to be split or staged (report M-10). Segments are
/// allocated during warmup and reused after <see cref="Reset"/>, so steady-state writes allocate nothing here.
/// </summary>
public sealed class SegmentedBufferWriter(int segmentSize) : IBufferWriter<byte>
{
    private readonly List<byte[]> segments = [new byte[segmentSize]];
    private int index;
    private int position;

    public void Reset() => (index, position) = (0, 0);

    public void Advance(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, segments[index].Length - position);
        position += count;
    }

    public Memory<byte> GetMemory(int sizeHint = 0)
    {
        var needed = Math.Max(sizeHint, 1);
        if (segments[index].Length - position < needed)
        {
            (index, position) = (index + 1, 0);
            if (index == segments.Count)
            {
                segments.Add(new byte[Math.Max(segmentSize, needed)]);
            }
            else if (segments[index].Length < needed)
            {
                segments[index] = new byte[needed];
            }
        }

        return segments[index].AsMemory(position);
    }

    public Span<byte> GetSpan(int sizeHint = 0) => GetMemory(sizeHint).Span;
}
