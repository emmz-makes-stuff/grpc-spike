using Google.Protobuf;
using Xunit;
using V1 = GrpcSpike.Corpus.Unknown.V1;
using V2 = GrpcSpike.Corpus.Unknown.V2;

namespace GrpcSpike.Corpus.Tests;

public class UnknownFieldTests
{
    [Fact]
    public void UnknownFieldsSurviveAReaderThatDoesNotKnowThem()
    {
        var written = Samples.UnknownFieldsWriter();
        var bytes = written.ToByteArray();

        var reader = V1.Record.Parser.ParseFrom(bytes);
        var reserialized = reader.ToByteArray();

        Assert.Equal(written.Id, reader.Id);
        Assert.Equal(written.Name, reader.Name);
        Assert.Equal(bytes, reserialized);
        Assert.Equal(written, V2.Record.Parser.ParseFrom(reserialized));
    }

    [Fact]
    public void DiscardingUnknownFieldsLosesThem()
    {
        // Negative control: shows the preservation test above would notice lost unknown fields.
        var bytes = Samples.UnknownFieldsWriter().ToByteArray();

        var reader = V1.Record.Parser.WithDiscardUnknownFields(true).ParseFrom(bytes);

        Assert.NotEqual(bytes, reader.ToByteArray());
    }
}
