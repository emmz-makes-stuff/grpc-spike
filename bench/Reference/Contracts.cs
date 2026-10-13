using ProtoBuf;

namespace GrpcSpike.Reference;

// protobuf-net contracts that are wire-compatible with the comparable corpus shapes (src/Corpus/protos). Field
// numbers and wire formats match the .proto files, so these read and write the same bytes as the generated code.
// Oneof and wrappers are left out: protobuf-net models them differently, so their rows would not be like-for-like.

public enum Colour
{
    Unspecified = 0,
    Red = 1,
    Green = 2,
    Blue = 3,
}

[ProtoContract]
public sealed class Scalars
{
    [ProtoMember(1)] public int Int32Value { get; set; }
    [ProtoMember(2)] public long Int64Value { get; set; }
    [ProtoMember(3)] public uint Uint32Value { get; set; }
    [ProtoMember(4)] public ulong Uint64Value { get; set; }
    [ProtoMember(5, DataFormat = DataFormat.ZigZag)] public int Sint32Value { get; set; }
    [ProtoMember(6, DataFormat = DataFormat.ZigZag)] public long Sint64Value { get; set; }
    [ProtoMember(7, DataFormat = DataFormat.FixedSize)] public uint Fixed32Value { get; set; }
    [ProtoMember(8, DataFormat = DataFormat.FixedSize)] public ulong Fixed64Value { get; set; }
    [ProtoMember(9, DataFormat = DataFormat.FixedSize)] public int Sfixed32Value { get; set; }
    [ProtoMember(10, DataFormat = DataFormat.FixedSize)] public long Sfixed64Value { get; set; }
    [ProtoMember(11)] public float FloatValue { get; set; }
    [ProtoMember(12)] public double DoubleValue { get; set; }
    [ProtoMember(13)] public bool BoolValue { get; set; }
    [ProtoMember(14)] public Colour EnumValue { get; set; }
}

[ProtoContract]
public sealed class Strings
{
    [ProtoMember(1)] public string? ShortText { get; set; }
    [ProtoMember(2)] public string? MediumText { get; set; }
    [ProtoMember(3)] public string? LongText { get; set; }
    [ProtoMember(4)] public string? UnicodeText { get; set; }
}

[ProtoContract]
public sealed class Bytes
{
    [ProtoMember(1)] public byte[]? SmallBlob { get; set; }
    [ProtoMember(2)] public byte[]? LargeBlob { get; set; }
}

[ProtoContract]
public sealed class Repeated
{
    [ProtoMember(1, IsPacked = true)] public List<int> PackedInt32 { get; } = [];
    [ProtoMember(2, IsPacked = true)] public List<double> PackedDouble { get; } = [];
    [ProtoMember(3)] public List<int> UnpackedInt32 { get; } = [];
    [ProtoMember(4)] public List<double> UnpackedDouble { get; } = [];
    [ProtoMember(5)] public List<string> Strings { get; } = [];
    [ProtoMember(6)] public List<RepeatedItem> Items { get; } = [];
}

[ProtoContract]
public sealed class RepeatedItem
{
    [ProtoMember(1)] public int Id { get; set; }
    [ProtoMember(2)] public string? Name { get; set; }
}

[ProtoContract]
public sealed class Maps
{
    [ProtoMember(1)] public Dictionary<int, long> IntToInt { get; } = [];
    [ProtoMember(2)] public Dictionary<string, string> StringToString { get; } = [];
    [ProtoMember(3)] public Dictionary<string, MapValue> StringToMessage { get; } = [];
}

[ProtoContract]
public sealed class MapValue
{
    [ProtoMember(1)] public int Id { get; set; }
    [ProtoMember(2)] public string? Name { get; set; }
}

[ProtoContract]
public sealed class Node
{
    [ProtoMember(1)] public int Value { get; set; }
    [ProtoMember(2)] public string? Label { get; set; }
    [ProtoMember(3)] public Node? Child { get; set; }
}

[ProtoContract]
public sealed class Optional
{
    [ProtoMember(1)] public int? Int32Value { get; set; }
    [ProtoMember(2)] public long? Int64Value { get; set; }
    [ProtoMember(3)] public double? DoubleValue { get; set; }
    [ProtoMember(4)] public bool? BoolValue { get; set; }
    [ProtoMember(5)] public string? StringValue { get; set; }
    [ProtoMember(6)] public byte[]? BytesValue { get; set; }
    [ProtoMember(7)] public Colour? EnumValue { get; set; }
}

/// <summary>The comparable corpus shapes (by the corpus sample's shape name) and their protobuf-net contract.</summary>
public static class ReferenceShapes
{
    public static IReadOnlyDictionary<string, Type> ByShape { get; } = new Dictionary<string, Type>
    {
        ["Scalars"] = typeof(Scalars),
        ["Strings"] = typeof(Strings),
        ["Bytes"] = typeof(Bytes),
        ["Repeated"] = typeof(Repeated),
        ["RepeatedItem"] = typeof(RepeatedItem),
        ["Maps"] = typeof(Maps),
        ["MapValue"] = typeof(MapValue),
        ["Node"] = typeof(Node),
        ["Optional"] = typeof(Optional),
    };
}
