using Google.Protobuf;

namespace GrpcSpike.Corpus;

/// <summary>A named instance of a corpus message. Variants start with populated, empty or edge.</summary>
public sealed record Sample(string Shape, string Variant, IMessage Message)
{
    public override string ToString() => $"{Shape}/{Variant}";
}

/// <summary>
/// Deterministic sample values for every corpus message, shared by the round-trip tests and the benchmarks
/// so both describe the same data.
/// </summary>
public static class Samples
{
    public const string Populated = "populated";
    public const string Empty = "empty";
    public const string Edge = "edge";

    private const string Unicode = "héllo wörld · こんにちは · 👋🏽";

    public static IReadOnlyList<Sample> All { get; } = Build();

    public static Node Nested(int depth)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(depth, 1);
        var node = new Node { Value = depth, Label = $"level-{depth}" };
        return depth == 1 ? node : new Node { Value = depth, Label = $"level-{depth}", Child = Nested(depth - 1) };
    }

    private static List<Sample> Build() =>
    [
        new("Scalars", Populated, new Scalars
        {
            Int32Value = 150,
            Int64Value = 1_234_567_890_123,
            Uint32Value = 3_000_000_000,
            Uint64Value = 9_000_000_000_000_000_000,
            Sint32Value = -150,
            Sint64Value = -1_234_567_890_123,
            Fixed32Value = 123_456,
            Fixed64Value = 123_456_789_012,
            Sfixed32Value = -123_456,
            Sfixed64Value = -123_456_789_012,
            FloatValue = 3.14f,
            DoubleValue = 2.718281828459045,
            BoolValue = true,
            EnumValue = Colour.Green,
        }),
        new("Scalars", Empty, new Scalars()),
        new("Scalars", $"{Edge}-min", new Scalars
        {
            Int32Value = int.MinValue,
            Int64Value = long.MinValue,
            Uint32Value = uint.MinValue,
            Uint64Value = ulong.MinValue,
            Sint32Value = int.MinValue,
            Sint64Value = long.MinValue,
            Sfixed32Value = int.MinValue,
            Sfixed64Value = long.MinValue,
            FloatValue = float.NegativeInfinity,
            // Not -0.0: a singular proto3 double is written only when != 0D, so -0.0 parses back as +0.0
            // while equality compares bitwise. Repeated -0.0 does round-trip (see the Repeated edge sample).
            DoubleValue = double.Epsilon,
            EnumValue = (Colour)(-1),
        }),
        new("Scalars", $"{Edge}-max", new Scalars
        {
            Int32Value = int.MaxValue,
            Int64Value = long.MaxValue,
            Uint32Value = uint.MaxValue,
            Uint64Value = ulong.MaxValue,
            Sint32Value = int.MaxValue,
            Sint64Value = long.MaxValue,
            Fixed32Value = uint.MaxValue,
            Fixed64Value = ulong.MaxValue,
            Sfixed32Value = int.MaxValue,
            Sfixed64Value = long.MaxValue,
            FloatValue = float.NaN,
            DoubleValue = double.PositiveInfinity,
            BoolValue = true,
            EnumValue = (Colour)99,
        }),

        new("Strings", Populated, new Strings
        {
            ShortText = "hello",
            MediumText = new string('m', 256),
            LongText = new string('l', 16 * 1024),
            UnicodeText = Unicode,
        }),
        new("Strings", Empty, new Strings()),
        new("Strings", Edge, new Strings
        {
            ShortText = "\0",
            MediumText = string.Concat(Enumerable.Repeat("👋🏽", 64)),
            LongText = new string('l', 1024 * 1024),
            UnicodeText = "﻿​",
        }),

        new("Bytes", Populated, new Bytes
        {
            SmallBlob = ByteString.CopyFrom(Pattern(16)),
            LargeBlob = ByteString.CopyFrom(Pattern(16 * 1024)),
        }),
        new("Bytes", Empty, new Bytes()),
        new("Bytes", Edge, new Bytes
        {
            SmallBlob = ByteString.CopyFrom(0),
            LargeBlob = ByteString.CopyFrom(Pattern(1024 * 1024)),
        }),

        new("Repeated", Populated, new Repeated
        {
            PackedInt32 = { Enumerable.Range(0, 100) },
            PackedDouble = { Enumerable.Range(0, 100).Select(i => i * 0.5) },
            UnpackedInt32 = { Enumerable.Range(0, 100) },
            UnpackedDouble = { Enumerable.Range(0, 100).Select(i => i * 0.5) },
            Strings = { Enumerable.Range(0, 20).Select(i => $"item-{i}") },
            Items = { Enumerable.Range(0, 20).Select(i => new RepeatedItem { Id = i, Name = $"item-{i}" }) },
        }),
        new("Repeated", Empty, new Repeated()),
        new("Repeated", Edge, new Repeated
        {
            PackedInt32 = { int.MinValue, 0, -1, int.MaxValue },
            PackedDouble = { double.NaN, -0.0, double.NegativeInfinity },
            UnpackedInt32 = { int.MinValue, 0, -1, int.MaxValue },
            UnpackedDouble = { double.NaN, -0.0, double.PositiveInfinity },
            Strings = { "", Unicode, "" },
            Items = { new RepeatedItem(), new RepeatedItem() },
        }),
        new("RepeatedItem", Populated, new RepeatedItem { Id = 7, Name = "seven" }),
        new("RepeatedItem", Empty, new RepeatedItem()),
        new("RepeatedItem", Edge, new RepeatedItem { Id = int.MinValue, Name = Unicode }),

        new("Maps", Populated, new Maps
        {
            IntToInt = { Enumerable.Range(0, 50).ToDictionary(i => i, i => i * 1_000_000L) },
            StringToString = { Enumerable.Range(0, 50).ToDictionary(i => $"key-{i}", i => $"value-{i}") },
            StringToMessage = { Enumerable.Range(0, 20).ToDictionary(i => $"key-{i}", i => new MapValue { Id = i, Name = $"value-{i}" }) },
        }),
        new("Maps", Empty, new Maps()),
        new("Maps", Edge, new Maps
        {
            IntToInt = { [0] = 0, [int.MinValue] = long.MinValue },
            StringToString = { [""] = "", [Unicode] = Unicode },
            StringToMessage = { [""] = new MapValue() },
        }),
        new("MapValue", Populated, new MapValue { Id = 3, Name = "three" }),
        new("MapValue", Empty, new MapValue()),
        new("MapValue", Edge, new MapValue { Id = int.MaxValue, Name = "\0" }),

        new("Node", $"{Populated}-depth1", Nested(1)),
        new("Node", $"{Populated}-depth3", Nested(3)),
        new("Node", $"{Populated}-depth8", Nested(8)),
        new("Node", Empty, new Node()),
        new("Node", Edge, EmptyChain(8)),

        new("Oneof", $"{Populated}-int32", new Oneof { Id = 1, Int32Choice = 42 }),
        new("Oneof", $"{Populated}-int64", new Oneof { Id = 1, Int64Choice = 42_000_000_000 }),
        new("Oneof", $"{Populated}-double", new Oneof { Id = 1, DoubleChoice = 4.2 }),
        new("Oneof", $"{Populated}-bool", new Oneof { Id = 1, BoolChoice = true }),
        new("Oneof", $"{Populated}-string", new Oneof { Id = 1, StringChoice = "forty-two" }),
        new("Oneof", $"{Populated}-message", new Oneof { Id = 1, MessageChoice = new OneofPayload { Id = 42, Name = "forty-two" } }),
        new("Oneof", Empty, new Oneof()),
        new("Oneof", $"{Edge}-default-int32", new Oneof { Int32Choice = 0 }),
        new("Oneof", $"{Edge}-default-string", new Oneof { StringChoice = "" }),
        new("Oneof", $"{Edge}-default-message", new Oneof { MessageChoice = new OneofPayload() }),
        new("OneofPayload", Populated, new OneofPayload { Id = 5, Name = "five" }),
        new("OneofPayload", Empty, new OneofPayload()),
        new("OneofPayload", Edge, new OneofPayload { Id = int.MinValue, Name = Unicode }),

        new("Wrappers", Populated, new Wrappers
        {
            Int32Value = 150,
            Int64Value = 1_234_567_890_123,
            DoubleValue = 2.5,
            BoolValue = true,
            StringValue = "wrapped",
            BytesValue = ByteString.CopyFrom(Pattern(32)),
        }),
        new("Wrappers", Empty, new Wrappers()),
        new("Wrappers", $"{Edge}-defaults-present", new Wrappers
        {
            Int32Value = 0,
            Int64Value = 0,
            DoubleValue = 0,
            BoolValue = false,
            StringValue = "",
            BytesValue = ByteString.Empty,
        }),

        new("Optional", Populated, new Optional
        {
            Int32Value = 150,
            Int64Value = 1_234_567_890_123,
            DoubleValue = 2.5,
            BoolValue = true,
            StringValue = "optional",
            BytesValue = ByteString.CopyFrom(Pattern(32)),
            EnumValue = Colour.Blue,
        }),
        new("Optional", Empty, new Optional()),
        new("Optional", $"{Edge}-defaults-present", new Optional
        {
            Int32Value = 0,
            Int64Value = 0,
            DoubleValue = 0,
            BoolValue = false,
            StringValue = "",
            BytesValue = ByteString.Empty,
            EnumValue = Colour.Unspecified,
        }),

        new("Unknown.V1.Record", Populated, new Unknown.V1.Record { Id = 1, Name = "one" }),
        new("Unknown.V1.Record", Empty, new Unknown.V1.Record()),
        new("Unknown.V1.Record", Edge, new Unknown.V1.Record { Id = int.MinValue, Name = Unicode }),
        new("Unknown.V2.Record", Populated, UnknownFieldsWriter()),
        new("Unknown.V2.Record", Empty, new Unknown.V2.Record()),
        new("Unknown.V2.Record", Edge, new Unknown.V2.Record
        {
            Id = int.MinValue,
            Name = Unicode,
            AddedVarint = long.MinValue,
            AddedFixed64 = ulong.MaxValue,
            AddedString = "",
            AddedFixed32 = uint.MaxValue,
        }),
    ];

    /// <summary>A v2 record whose fields 3-6 are unknown to the v1 reader: one per wire type.</summary>
    public static Unknown.V2.Record UnknownFieldsWriter() => new()
    {
        Id = 1,
        Name = "one",
        AddedVarint = 1_234_567_890_123,
        AddedFixed64 = 0xDEAD_BEEF_CAFE_F00D,
        AddedString = "not known to v1",
        AddedFixed32 = 0xCAFE_F00D,
    };

    private static Node EmptyChain(int depth) => depth == 1 ? new Node() : new Node { Child = EmptyChain(depth - 1) };

    private static byte[] Pattern(int length)
    {
        var bytes = new byte[length];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(i * 31);
        }

        return bytes;
    }
}
