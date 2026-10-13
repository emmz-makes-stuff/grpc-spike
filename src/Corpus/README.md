# Corpus

The shapes the allocation analysis measures. Each `.proto` in `protos/` holds one shape. `Grpc.Tools`
2.84.0 and `Google.Protobuf` 3.36.2 (pinned in `Directory.Packages.props`) compile them into
`Generated/protos/`, which is committed so findings can cite it by file and line. Every build
regenerates that folder, and `make generated` fails if it differs from what is committed.

`Samples.cs` holds the populated, empty and edge-case values for every message. The round-trip tests and
the benchmarks both use it.

## Shape mapping

Each shape in the `codegen-shape-corpus` spec, and the definition that covers it.

| Shape | Definition | Proto | Generated |
|---|---|---|---|
| Scalar fields (every proto3 scalar type, plus an enum) | `Scalars` | `protos/scalars.proto` | `Generated/protos/Scalars.cs` |
| String fields (ASCII, multi-byte UTF-8, long) | `Strings` | `protos/strings.proto` | `Generated/protos/Strings.cs` |
| Bytes fields (small and large) | `Bytes` | `protos/bytes.proto` | `Generated/protos/Bytes.cs` |
| Packed repeated fields | `Repeated.packed_int32`, `Repeated.packed_double` | `protos/repeated.proto` | `Generated/protos/Repeated.cs` |
| Unpacked repeated fields | `Repeated.unpacked_int32`, `Repeated.unpacked_double` (`[packed = false]`) | `protos/repeated.proto` | `Generated/protos/Repeated.cs` |
| Repeated strings and messages | `Repeated.strings`, `Repeated.items` (`RepeatedItem`) | `protos/repeated.proto` | `Generated/protos/Repeated.cs` |
| Map fields (scalar, string and message values) | `Maps` (`MapValue`) | `protos/maps.proto` | `Generated/protos/Maps.cs` |
| Nested messages at depths 1, 3 and 8 | `Node`, a recursive definition; `Samples.Nested(depth)` builds each depth | `protos/nesting.proto` | `Generated/protos/Nesting.cs` |
| Oneof with scalar, string and message members | `Oneof.choice` (`int32`, `int64`, `double`, `bool`, `string`, `OneofPayload`) | `protos/oneof.proto` | `Generated/protos/Oneof.cs` |
| Well-known wrapper types | `Wrappers` (`Int32Value`, `Int64Value`, `DoubleValue`, `BoolValue`, `StringValue`, `BytesValue`) | `protos/wrappers.proto` | `Generated/protos/Wrappers.cs` |
| proto3 `optional` fields | `Optional` | `protos/optional.proto` | `Generated/protos/Optional.cs` |
| Unknown fields: writer | `corpus.unknown.v2.Record` (fields 3–6, one per wire type, unknown to v1) | `protos/unknown_v2.proto` | `Generated/protos/UnknownV2.cs` |
| Unknown fields: reader | `corpus.unknown.v1.Record` (fields 1–2 only) | `protos/unknown_v1.proto` | `Generated/protos/UnknownV1.cs` |
| Unary service method | `CorpusService.Unary` | `protos/service.proto` | `Generated/protos/ServiceGrpc.cs` |
| Server-streaming service method | `CorpusService.ServerStreaming` | `protos/service.proto` | `Generated/protos/ServiceGrpc.cs` |
| Client-streaming service method | `CorpusService.ClientStreaming` | `protos/service.proto` | `Generated/protos/ServiceGrpc.cs` |
| Bidirectional-streaming service method | `CorpusService.Bidi` | `protos/service.proto` | `Generated/protos/ServiceGrpc.cs` |

The unknown-fields payload is `Samples.UnknownFieldsWriter()`: a v2 `Record` serialized and then parsed
by the v1 `Record`.

All four service methods take and return `Scalars`, so the stub benchmarks aren't dominated by string
decoding and the stub layer's own share stays visible.

## Generation notes

- Stubs are generated only for `service.proto` (`GrpcServices="Both"`). Every other proto uses
  `GrpcServices="None"`, because `grpc_csharp_plugin` writes no `*Grpc.cs` for a proto without services,
  yet `Grpc.Tools` still expects one and the build fails.
- A singular proto3 `double` holding `-0.0` does not round-trip: the generated writer only writes the
  field when `!= 0D`, so it parses back as `+0.0`, while equality compares bitwise. Repeated `-0.0` does
  round-trip. This is subject behaviour, not an allocation finding, so the edge samples avoid it.
