using Google.Protobuf;
using Xunit;

namespace GrpcSpike.Corpus.Tests;

public class RoundTripTests
{
    public static TheoryData<string> SampleNames => [.. Samples.All.Select(s => s.ToString())];

    [Theory]
    [MemberData(nameof(SampleNames))]
    public void SampleRoundTrips(string name)
    {
        var original = Samples.All.Single(s => s.ToString() == name).Message;

        var bytes = original.ToByteArray();
        var parsed = original.Descriptor.Parser.ParseFrom(bytes);

        Assert.Equal(original, parsed);
        Assert.Equal(bytes, parsed.ToByteArray());
    }

    [Fact]
    public void EveryCorpusMessageHasPopulatedEmptyAndEdgeSamples()
    {
        var messageTypes = typeof(Samples).Assembly.GetTypes()
            .Where(t => typeof(IMessage).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();

        Assert.NotEmpty(messageTypes);
        Assert.All(messageTypes, type =>
        {
            var variants = Samples.All.Where(s => s.Message.GetType() == type).Select(s => s.Variant).ToList();
            Assert.Contains(variants, v => v.StartsWith(Samples.Populated, StringComparison.Ordinal));
            Assert.Contains(Samples.Empty, variants);
            Assert.Contains(variants, v => v.StartsWith(Samples.Edge, StringComparison.Ordinal));
        });
    }

    [Fact]
    public void SampleNamesAreUnique()
    {
        var names = Samples.All.Select(s => s.ToString()).ToList();

        Assert.Equal(names.Count, names.Distinct().Count());
    }
}
