using System.Collections;
using Google.Protobuf;
using GrpcSpike.Reference;
using ProtoBuf;
using Xunit;

namespace GrpcSpike.Corpus.Tests;

/// <summary>
/// The protobuf-net reference rows (task 4.5) are only comparable if they carry the same data. Each reference object
/// is read from the generated code's bytes. It must hold the same values field by field (a wrong wire format can
/// still round-trip its bytes), and writing it back must give a message equal to the original.
/// </summary>
public class ReferenceEquivalenceTests
{
    public static TheoryData<string> ComparableSampleNames =>
        [.. Samples.All.Where(s => ReferenceShapes.ByShape.ContainsKey(s.Shape)).Select(s => s.ToString())];

    [Theory]
    [MemberData(nameof(ComparableSampleNames))]
    public void ReferenceContractCarriesTheSameData(string name)
    {
        var original = Samples.All.Single(s => s.ToString() == name);
        var type = ReferenceShapes.ByShape[original.Shape];

        var reference = Serializer.NonGeneric.Deserialize(type, new MemoryStream(original.Message.ToByteArray()));
        AssertSameValues(original.Message, reference, original.Shape);

        var written = new MemoryStream();
        Serializer.NonGeneric.Serialize(written, reference);
        Assert.Equal(original.Message, original.Message.Descriptor.Parser.ParseFrom(written.ToArray()));
    }

    [Fact]
    public void EveryReferenceShapeHasSamples() =>
        Assert.All(ReferenceShapes.ByShape.Keys, shape => Assert.Contains(Samples.All, s => s.Shape == shape));

    /// <summary>Matches reference properties to generated ones by name; proto3 omits defaults, so null matches them.</summary>
    private static void AssertSameValues(IMessage generated, object reference, string path)
    {
        foreach (var property in reference.GetType().GetProperties())
        {
            var generatedProperty = generated.GetType().GetProperty(property.Name)
                ?? throw new InvalidOperationException($"{path}.{property.Name} has no generated counterpart.");
            var referenceValue = property.GetValue(reference);
            var hasPresence = generated.GetType().GetProperty("Has" + property.Name);
            if (hasPresence is not null && !(bool)hasPresence.GetValue(generated)!)
            {
                Assert.True(referenceValue is null, $"{path}.{property.Name} is absent but the reference holds a value.");
                continue;
            }

            AssertSameValue(generatedProperty.GetValue(generated), referenceValue, $"{path}.{property.Name}");
        }
    }

    private static void AssertSameValue(object? generated, object? reference, string path)
    {
        switch (generated, reference)
        {
            case (null, null):
            case (string { Length: 0 }, null):
            case (ByteString { IsEmpty: true }, null):
                return;
            case (ByteString bytes, byte[] referenceBytes):
                Assert.True(bytes.Span.SequenceEqual(referenceBytes), $"{path} differs.");
                return;
            case (IMessage message, { } referenceMessage):
                AssertSameValues(message, referenceMessage, path);
                return;
            case (Enum value, Enum referenceValue):
                Assert.True(Convert.ToInt32(value) == Convert.ToInt32(referenceValue), $"{path} differs.");
                return;
            case (IDictionary map, IDictionary referenceMap):
                Assert.True(map.Count == referenceMap.Count, $"{path} has a different count.");
                foreach (DictionaryEntry entry in map)
                {
                    Assert.True(referenceMap.Contains(entry.Key), $"{path}[{entry.Key}] is missing.");
                    AssertSameValue(entry.Value, referenceMap[entry.Key], $"{path}[{entry.Key}]");
                }

                return;
            case (IList list, IList referenceList):
                Assert.True(list.Count == referenceList.Count, $"{path} has a different count.");
                for (var i = 0; i < list.Count; i++)
                {
                    AssertSameValue(list[i], referenceList[i], $"{path}[{i}]");
                }

                return;
            default:
                Assert.True(Equals(generated, reference), $"{path}: generated {generated}, reference {reference}.");
                return;
        }
    }
}
