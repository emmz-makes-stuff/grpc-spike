using System.Reflection;
using Google.Protobuf;
using Xunit;

namespace GrpcSpike.Corpus.Tests;

public class PinnedVersionTests
{
    [Fact]
    public void GoogleProtobufIsThePinnedVersion()
    {
        var version = typeof(IMessage).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;

        Assert.NotNull(version);
        Assert.StartsWith("3.36.2", version);
    }
}
