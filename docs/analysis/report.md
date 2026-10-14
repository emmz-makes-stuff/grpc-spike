# Grpc.Tools generated code: allocation analysis

**Status: complete for review (§5).** Every site from the §3 code reading (the appendices) has been checked against the
committed benchmark results (§4.7, §5.1). Where the byte counts alone don't show which object it is, an attribution
trace was also used. **Findings** lists the confirmed costs. **Sites not counted** lists the rest, each with its
reason. This report doesn't decide which findings are worth acting on: that decision is the ADR's, once the numbers
are reviewed.

## Subject and citations

| Subject | Version | Cited as |
|---|---|---|
| Generated corpus code | Grpc.Tools 2.84.0 (protoc `libprotoc 35.1`, `grpc_csharp_plugin` 1.84.0) | Repo path and line: `src/Corpus/Generated/protos/<File>.cs:<line>` |
| Google.Protobuf runtime | 3.36.2 | `https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/…#L<n>` |
| protoc C# message generator | 35.1 (bundled in Grpc.Tools 2.84.0) | `https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/…#L<n>` |
| `grpc_csharp_plugin` stub generator | grpc 1.84.0 | `https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L<n>` |
| Grpc.Core.Api, Grpc.Net.Client, Grpc.AspNetCore.Server | 2.84.0 | `https://github.com/grpc/grpc-dotnet/blob/v2.84.0/…#L<n>` |

The message generator and the runtime come from different protobuf releases: Grpc.Tools 2.84.0 bundles protoc
35.1, while the pinned runtime is 3.36.2.

A generator citation means the site is emitted into generated code. The findings table gives each finding's main
source location. The site's row in the appendices has the full set of citations.

Link prefixes used below:
- **RT** = `https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/`
- **GEN** = `https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/`

## Measurements

- **Where.** Every number comes from `docs/analysis/results/<runtime>/GrpcSpike.Benchmarks.<Class>-report-full.json`.
  Everything was measured on one AWS EC2 instance: an Intel Xeon Platinum 8259CL with 2 vCPUs, running Ubuntu
  26.04. Each runtime's `environment.md` gives the details.
- **Citing a row.** A benchmark citation such as `MessageBenchmarks.ParseSpan(Sample: Maps/populated)` is that row's
  `FullName` in the class's JSON, without the `GrpcSpike.Benchmarks.` prefix. Use the JSON rather than the
  `-report-github.md` tables, which shorten sample names.
- **Bytes.** Bytes are allocated bytes per operation (`Memory.BytesAllocatedPerOperation`), quoted from `net10.0`.
  They don't depend on the machine, and they are the primary evidence.
  - `net11.0` gives the same bytes on every message-level row, with two exceptions. The 1 MB `*/edge` rows are
    under 50 B apart, which is large-object-heap noise. `MessageBenchmarks.MergeIntoPopulated(Sample: Maps/populated)`
    is 56 B lower on net11.
  - The end-to-end rows do differ (see S-20–S-35).
- **Timings.** Timings (`Statistics.Mean`, ns) are indicative only: BenchmarkDotNet's ShortRun on a shared, burstable
  instance.
- **Traces.** The attribution traces (`make attribute`, Mac, net10) were used only to see *which* types allocate.
  Their shares are sampled estimates, so byte counts always come from the committed results. Traces aren't committed
  (see `README.md`).
- **Arithmetic.** It uses .NET x64 object sizes (for example, an array has a 24 B header, so `int[128]` is 536 B).
  Where a row says "exactly", the sum of the objects equals the measured bytes.

**Tiers:**
- **A:** generated private members or method bodies only.
- **B:** runtime internals, with no public API change.
- **C:** a public API change. Tier C is recorded but excluded from go/no-go.

**Hot path or one-off:**
- **Hot path:** the cost recurs on every message operation or call.
- **One-off:** the cost happens once per type, process or channel. It is never presented as a
  per-operation saving.

## Hypotheses

| # | Hypothesis | Status | Evidence |
|---|---|---|---|
| 1 | Boxing of oneof scalars | **Confirmed** | 24 B per set scalar member, on every parse, Clone and Merge (M-01, M-02). |
| 2 | Per-entry overhead in maps | **Confirmed** | About 70 % of a parsed map's bytes are container overhead (M-03). An empty map field costs 152 B per instance (M-04), and each non-empty map costs 56 B per write (M-05). The CPU site M-06 is not isolated. |
| 3 | String decoding | **Confirmed off the default path only** | Parsing from a span or sequence allocates just the string, which is inherent (M-07). Temporary copies appear when parsing from a `Stream` (M-08) and when writing to segmented output (M-10). |
| 4 | Bytes copying on parse | **Confirmed, mostly inherent** | One copy plus one `ByteString` per field is inherent to the API (M-11, Tier C). An empty but present value costs 56 B, and that part is avoidable (M-12, Tier B). |
| 5 | Nested message instances | **Confirmed as inherent, plus one Tier A case** | 88 B per level, linear in depth (M-13, Tier C). The only extra objects are the 40 B oneof-message merge case (M-14) and each instance's eager collections (M-24, M-25). |
| 6 | Repeated-field growth | **Confirmed** | 38 % of a parsed `Repeated/populated` is discarded growth arrays and slack (M-16). Clone copies the slack (M-17). |
| 7 | Repeated size calculation on deep messages | **Confirmed (CPU)** | Write time grows faster than linearly with depth, while `CalculateSize` grows linearly. About 60 % of a depth-8 write is re-sizing (M-18). |
| 8 | Headroom in the stub layer | **Rejected** | The generated marshaller allocates 0 B to serialize and 104 B to deserialize, which is the message itself. That is about 1.5 % of a 14,240 B unary call. The rest belongs to grpc-dotnet and the HTTP stack (S-20–S-35). |

## Findings

All bytes are net10, per operation. **Avoidable** is what a fix at the stated tier would remove from that row.
`tests/Corpus.Tests/AllocationTests.cs` pins each row's bytes (M-18 is CPU only).

Weigh each row by whether it is on the gRPC path, not by its size:
- **Off the gRPC path.** The largest rows are `Stream` parsing and writing (M-08, M-21, M-23), and gRPC calls never
  use `Stream`. Their marshallers go through `ReadOnlySequence` and `IBufferWriter` (S-01, S-05). The `byte[]`
  entry points (M-20, M-22) are off the default gRPC path too (S-03 and S-04 aren't taken).
- **Tier A on the gRPC path.** What a generator alone could fix there is small: 24–152 B per operation (M-01, M-02,
  M-04, M-14, M-24, M-25). The exception is M-18's CPU cost.
- **Tier B on the gRPC path.** The larger costs on the gRPC path need runtime changes (M-03, M-05, M-10 on the
  server, M-12, M-16, M-17).

| ID | Finding | Tier | Hot / one-off | Source | Measured (committed) | Avoidable |
|---|---|---|---|---|---|---|
| M-01 | A set scalar oneof member is boxed into `object choice_` on every parse. | A | Hot | `src/Corpus/Generated/protos/Oneof.cs:268`, `:561`; [GEN csharp_primitive_field.cc#L265-L270](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_primitive_field.cc#L265-L270) | `MessageBenchmarks.ParseSpan(Sample: Oneof/populated-int32)` 64 vs `…(Sample: Oneof/empty)` 40. Bool, double and int64 are the same. The trace shows the `System.Int32` box. | 24 B per parse of a set scalar member |
| M-02 | Clone and MergeFrom unbox the value and box it again, instead of sharing the immutable box. | A | Hot (Clone/Merge) | `src/Corpus/Generated/protos/Oneof.cs:86`, `:463`; [GEN csharp_primitive_field.cc#L320-L322](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_primitive_field.cc#L320-L322) | `MessageBenchmarks.Clone(Sample: Oneof/populated-int32)` 64 vs `…(Sample: Oneof/empty)` 40. `MergeIntoPopulated(Sample: Oneof/populated-int32)` 88 = Clone 64 + 24. | 24 B per Clone or Merge |
| M-03 | Each map entry costs a `LinkedListNode` plus `Dictionary` slots, so that insertion order is kept. | B | Hot | [RT Collections/MapField.cs#L54-L56](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L54-L56), [#L415-L421](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L415-L421) | `MessageBenchmarks.ParseSpan(Sample: Maps/populated)` is 24,552 B for 120 entries. Trace: `Dictionary` `Entry[]` ≈ 37 %, `LinkedListNode` ≈ 27 %, bucket `Int32[]` ≈ 8 %, strings ≈ 22 % (sampled). | ≈ 27 % (the nodes) if one ordered structure replaced the two; sampled, not exact |
| M-04 | Every instance creates a `MapField`, a `Dictionary` and a `LinkedList` for each map field, even when the map stays empty. | A (lazy generated field) or B (lazy inside `MapField`) | Hot (per instance) | `src/Corpus/Generated/protos/Maps.cs:102`, `:113`, `:124`; [RT Collections/MapField.cs#L54-L56](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L54-L56) | `MessageBenchmarks.New(Sample: Maps/empty)` 504 = 48 B message + 3 × 152 B, exactly. `ParseSpan(Sample: Maps/empty)` is also 504. | 152 B per empty map field per instance |
| M-05 | Each write boxes a `LinkedList` enumerator for every non-empty map field. | B | Hot | [RT Collections/MapField.cs#L475](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L475), [#L486](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L486) | `MessageBenchmarks.WriteToSpan(Sample: Maps/populated)` 168 = 3 × 56 B. `…(Sample: Maps/empty)` is 0. | 56 B per non-empty map field per write |
| M-08 | Parsing from a `Stream` copies long strings and bytes through temporary arrays. | B | Hot (`Stream` parses only; off the gRPC path) | [RT ParsingPrimitives.cs#L604-L657](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L604-L657), [#L782-L787](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L782-L787) | `StreamAndMergeBenchmarks.ParseStream(Sample: Strings/populated)` 70,880 = `MessageBenchmarks.ParseSpan` 33,496 + 4,288 (M-21) + 33,096. `…(Sample: Bytes/populated)` 37,528 = 16,552 + 4,288 + 16,688. For about 16.6 KB of string or blob content, that is about two temporary copies for strings and one for bytes. The chunk-by-chunk mechanism was not traced. | 33,096 B (`Strings/populated`), 16,688 B (`Bytes/populated`) |
| M-10 | A string that doesn't fit the current output segment is first encoded into a temporary `byte[]`. | B | Hot (segmented output: `Stream` writes, and the server's response `PipeWriter`) | [RT WritingPrimitives.cs#L186-L192](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/WritingPrimitives.cs#L186-L192) | `MessageBenchmarks.WriteToSegmentedWriter(Sample: Strings/populated)` 16,760 vs `WriteToSpan` 0. That benchmark uses 32-byte segments, which is pessimistic. Same sample: `StreamAndMergeBenchmarks.WriteToStream` 20,592 = 4,184 (M-23) + 16,408. The client writes into one exact-size pooled buffer (S-24), where this doesn't trigger. | Up to one copy of each string that crosses a segment boundary (16,408 B on `Strings/populated` through a `Stream`) |
| M-12 | An empty but present bytes value allocates `new byte[0]` and a new `ByteString`, instead of returning `ByteString.Empty`. | B | Hot (small) | [RT ParsingPrimitives.cs#L420-L426](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L420-L426) | `MessageBenchmarks.ParseSpan(Sample: Optional/edge-defaults-present)` 128 vs `…(Sample: Optional/empty)` 72: 24 B + 32 B. Empty strings return `string.Empty`. | 56 B per empty present bytes value |
| M-14 | Parsing a oneof message member that is already set builds a new sub-message and merges the old one into it. | A | Hot (merge into a set member only) | `src/Corpus/Generated/protos/Oneof.cs:581-586`; [GEN csharp_message_field.cc#L231-L245](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_message_field.cc#L231-L245) | `StreamAndMergeBenchmarks.MergeBytesIntoPopulated(Sample: Oneof/populated-message)` 160 = `MessageBenchmarks.Clone(Sample: Oneof/populated-message)` 80 + 80. Merging in place would need only the 40 B string. | 40 B (one `OneofPayload`) per merge |
| M-16 | Packed varint, unpacked, string and message repeats grow by doubling as they parse. Packed fixed-size fields are pre-sized. | B | Hot | [RT Collections/RepeatedField.cs#L146](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/RepeatedField.cs#L146), [#L315-L334](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/RepeatedField.cs#L315-L334); `src/Corpus/Generated/protos/Repeated.cs:339`, `:349`, `:358`, `:362` | `MessageBenchmarks.ParseSpan(Sample: Repeated/populated)` 8,856, exactly. It holds 2,688 B of discarded growth arrays and 640 B of slack in the final arrays. The pre-sized packed double is `double[100]` (824 B). Trace: `Double[]` ≈ 33 %, `Int32[]` ≈ 25 %. | Up to 3,328 B (38 %) with exact pre-sizing; the discarded arrays alone are 2,688 B |
| M-17 | Clone copies each backing array whole, slack included. | B | Hot (Clone) | [RT Collections/RepeatedField.cs#L56-L66](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/RepeatedField.cs#L56-L66) | `MessageBenchmarks.Clone(Sample: Repeated/populated)` 4,984, exactly. The source was built by `Add`, giving capacity 128 for 100 elements and 32 for 20. | 864 B (17 %) here; the amount depends on the source's growth history |
| M-18 | Nested writes re-size every sub-message for its length prefix, which is O(depth²) with no memoisation. | A (size cached in a generated private field) or B | Hot (CPU) | [RT WritingPrimitivesMessages.cs#L26-L30](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/WritingPrimitivesMessages.cs#L26-L30); `src/Corpus/Generated/protos/Nesting.cs:204`, `:223`; [GEN csharp_message_field.cc#L121](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_message_field.cc#L121) | `MessageBenchmarks.CalculateSize(Sample: Node/populated-depth1/3/8)` is 13.6 / 40.9 / 108.6 ns, which is linear. `WriteToSpan` on the same samples is 27.7 / 123.8 / 585.6 ns (net11: 22.9 / 134.0 / 540.3). A linear write at depth 8 would take about 8 × 27.7 ≈ 222 ns. 0 B. | About 60 % of a depth-8 write (CPU; indicative timings) |
| M-20 | `ParseFrom(byte[])` allocates a `CodedInputStream` on every call. | B | Hot | [RT MessageParser.cs#L259-L264](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageParser.cs#L259-L264) | `MessageBenchmarks.ParseByteArray` − `ParseSpan` = 168 B on every sample (e.g. `Sample: Scalars/populated`: 272 vs 104). | 168 B per call |
| M-21 | `ParseFrom(Stream)` allocates a `CodedInputStream` and a fresh 4 KiB buffer on every call. | B | Hot (`Stream` parses only; off the gRPC path) | [RT MessageExtensions.cs#L296-L304](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L296-L304), [RT CodedInputStream.cs#L104-L105](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/CodedInputStream.cs#L104-L105) | `StreamAndMergeBenchmarks.ParseStream` − `MessageBenchmarks.ParseSpan` = 4,288 B (`byte[4096]` 4,120 + 168) on 48 of 52 samples. The other four add M-08. | Up to 4,288 B per call (4,120 B by pooling the buffer) |
| M-22 | `ToByteArray` allocates a `CodedOutputStream` on every call. | B | Hot | [RT MessageExtensions.cs#L94-L102](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L94-L102) | `MessageBenchmarks.ToByteArray(Sample: Scalars/empty)` 88 = `byte[0]` 24 + 64. | 64 B per call |
| M-23 | `WriteTo(Stream)` allocates a `CodedOutputStream` and a fresh 4 KiB buffer on every call. | B | Hot (`Stream` writes only; off the gRPC path) | [RT MessageExtensions.cs#L109-L116](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L109-L116), [RT CodedOutputStream.cs#L120](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/CodedOutputStream.cs#L120) | `StreamAndMergeBenchmarks.WriteToStream` is 4,184 B (4,120 + 64) on 48 of 52 samples. `Maps/*` add M-05 (4,352). `Strings/populated` and `Strings/edge` add M-10. | Up to 4,184 B per call (4,120 B by pooling the buffer) |
| M-24 | Every instance creates a `RepeatedField` for each repeated field, even when it stays empty. | A | Hot (per instance) | `src/Corpus/Generated/protos/Repeated.cs:101`, `:112`, `:123`, `:134`, `:145`, `:156`; [GEN csharp_repeated_primitive_field.cc#L39](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_repeated_primitive_field.cc#L39) | `MessageBenchmarks.New(Sample: Repeated/empty)` 264 = 72 B message + 6 × 32 B, exactly. | 32 B per empty repeated field per instance |
| M-25 | The copy constructor runs the field initialisers, then replaces every collection they created. | A | Hot (Clone) | `src/Corpus/Generated/protos/Repeated.cs:81-88`, `src/Corpus/Generated/protos/Maps.cs:85-89`; [GEN csharp_message.cc#L360](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_message.cc#L360) | `MessageBenchmarks.Clone(Sample: Maps/empty)` 960 = 504 + 456 discarded (3 × 152). `…(Sample: Repeated/empty)` 456 = 264 + 192 discarded (6 × 32). | 32 B per repeated field and 152 B per map field, per Clone |
| M-26 | Unknown fields are decoded into an `UnknownFieldSet` of typed lists. | B | Hot (only when unknown fields are present) | [RT UnknownFieldSet.cs#L139-L158](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/UnknownFieldSet.cs#L139-L158), [RT UnknownField.cs#L254-L261](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/UnknownField.cs#L254-L261) | `UnknownFieldBenchmarks.ParseV2AsV1` 1,376 vs 320 for the same bytes with every field known (`MessageBenchmarks.ParseByteArray(Sample: Unknown.V2.Record/populated)`), so 1,056 B more. `CloneWithUnknowns` 600 vs `Clone(Sample: Unknown.V2.Record/populated)` 64. | Part of 1,056 B per parse and 536 B per Clone; a raw-bytes store would still allocate something |
| S-20–S-35 | grpc-dotnet, ASP.NET Core and `System.Net.Http` allocate per call and per streamed message. | B (in those libraries; out of a generator's reach) | Hot | For example, client [GrpcCall.cs#L70-L75](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCall.cs#L70-L75), [#L960-L972](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCall.cs#L960-L972) and server [HttpContextServerCallContext.cs#L64](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/HttpContextServerCallContext.cs#L64). Each site's citations are in Appendix B. | `EndToEndBenchmarks.Unary(Messages: 1)` is 14,240 B per call, about 208 B of it from the generated marshaller (hypothesis 8). Each extra streamed message, as (`Messages: 10` − `Messages: 1`) / 9: server streaming ≈ 300 B, client streaming ≈ 80 B, bidi ≈ 1,470 B. net11 is 15,621 B per unary call, and 1.1–2.9 KB higher across the end-to-end rows. Noise is a few hundred bytes, because BenchmarkDotNet counts every thread. | Not attributed per site; out of scope for a generator |

## Sites not counted

| Site(s) | Why | Evidence |
|---|---|---|
| M-27 | **Rejected.** No boxed enumerator is measured once the code is optimised: .NET 10's JIT removes both of them (in `CalculateSize` and `WriteTo`). They do exist in unoptimised (tier-0) code. `AllocationTests` measured 248 B (136 + 2 × 56) before tier-up, and the test warms up past tier-up for that reason. | `UnknownFieldBenchmarks.ReserializeUnknownsAsV1` 136 B equals `MessageBenchmarks.ToByteArray(Sample: Unknown.V2.Record/populated)` 136 B, which writes the same bytes with no `UnknownFieldSet`. The trace shows only `Byte[]` and `CodedOutputStream`. |
| S-01–S-10 (hypothesis 8) | **Rejected.** The generated stub and marshaller code takes the runtimes' pooled, no-copy paths. S-01's CPU re-sizing is M-18. S-03/S-04 are fallbacks that aren't taken, S-08 is a struct, and S-10 is the failure path. | `MarshallerBenchmarks.Serialize` 0 B. `MarshallerBenchmarks.Deserialize` 104 B, the same as `MessageBenchmarks.New(Sample: Scalars/empty)` 104 B, i.e. the message (S-05). A unary call deserializes twice: 208 B of 14,240 B ≈ 1.5 %. |
| M-07, M-11, M-13, M-15, S-05 | **Inherent** (Tier C to avoid): one `string` per string field, one copy plus a `ByteString` per bytes field, one object per message. | `MessageBenchmarks.ParseSpan(Sample: Strings/populated)` 33,496 = 56 B message + the four strings. `…(Sample: Bytes/populated)` 16,552 = 40 B message + two copies and two `ByteString`s. `…(Sample: Node/populated-depth1/3/8)` 88 / 264 / 704 = 88 B per level. In M-16's exact sum, each `RepeatedItem` is 40 B plus its name. |
| M-28, M-29 | **No site**, as the code reading found. | The `Wrappers` and `Optional` parse traces show the message plus string and bytes content only. `MessageBenchmarks.ParseSpan(Sample: Wrappers/empty)` and `…(Sample: Optional/empty)` equal `New` (88 and 72 B). |
| M-06, M-09, M-19 | **Not isolated** (CPU, no allocation). No benchmark separates the second size pass from the rest of the write. They stay as sites in the M-18 family and are not counted. | The `CalculateSize` and `WriteToSpan` rows include them but can't separate them. |
| M-30, S-06, S-07, S-09 | **One-off** (per type, process or channel). BenchmarkDotNet's warm-up excludes them. They are never a per-operation saving. | None needed. |
| S-29, S-36 | **Off the measured path:** blocking unary calls, and compression (off in the benchmarks). | None. |

### Other observation (not an allocation site)

A singular proto3 `double` holding `-0.0` does not round-trip in Google.Protobuf 3.36.2:

- The generated writer guards the field with `!= 0D` (`src/Corpus/Generated/protos/Scalars.cs:399`). So `-0.0` is
  never written, and it parses back as `+0.0`.
- Equality, though, compares bitwise (`src/Corpus/Generated/protos/Scalars.cs:334`), so the two values compare
  unequal.
- Repeated `-0.0` does round-trip.

The upstream survey found no existing issue for this (see below), so it is worth filing as a bug.

## Prior art

Surveyed on 2026-10-14. The release versions and dates come from NuGet. The repo status and licences come from
GitHub. The allocation claims are each project's own; none of them was measured here. "Drop-in" has two levels:

- **API:** the generated types keep Google.Protobuf's public API: `IMessage<T>`, `Parser`, descriptors,
  `RepeatedField`/`MapField`, `JsonFormatter`, the well-known types and reflection.
- **Build:** consumers keep their `<Protobuf Include>` items and the Grpc.Tools MSBuild targets.

| Candidate | Wire-compatible | API drop-in | Build drop-in | Allocation approach | Status | Licence |
|---|---|---|---|---|---|---|
| [Google.Protobuf](https://github.com/protocolbuffers/protobuf) + [Grpc.Tools](https://github.com/grpc/grpc) (baseline) | Yes | n/a | n/a | Span-based `ParseContext`/`WriteContext` since 3.15. Messages, collections, strings and `ByteString` stay on the heap. The C# generator has no option that reduces allocations, and arenas exist only in C++. | 3.36.2 (2026-09-17); Grpc.Tools 2.84.0 (2026-09-14) | BSD-3 / Apache-2.0 |
| [protobuf-net](https://github.com/protobuf-net/protobuf-net) (+ protogen, BuildTools, [protobuf-net.Grpc](https://github.com/protobuf-net/protobuf-net.Grpc)) | Yes | No: `[ProtoContract]` plain classes, code-first service interfaces | Partial: a Roslyn generator over `<AdditionalFiles>`, not `<Protobuf>` | Runtime-emitted serializers, pooled buffers. Its compile-time model targets AOT, not allocations. | 3.4.30 (2026-09-16), very active | Apache-2.0 |
| [LightProto](https://github.com/dameng324/LightProto) | Yes | No: protobuf-net-style classes with `List`/`Dictionary` | No: a CLI generator | Source-generated; span, sequence and `IBufferWriter` APIs; optional struct messages | 1.4.0 (2026-08-02), one maintainer | MIT |
| [Protobuf.Generator](https://github.com/ByboyCn/Protobuf.Library) | Claimed | No: its own API | No: its own `<ProtoFile>` item | Not documented | 1.0.3 (2026-06-17), immature | MIT |
| [Lagrange.Proto](https://github.com/LagrangeDev/LagrangeV2) | Yes | No: attribute-based | No | Source-generated, claims "low allocation" | 1.0.10 (2026-07-11), repo archived | GPL-3.0 |
| [MessagePack-CSharp](https://github.com/MessagePack-CSharp/MessagePack-CSharp), [MemoryPack](https://github.com/Cysharp/MemoryPack), [MagicOnion](https://github.com/Cysharp/MagicOnion), [Bond](https://github.com/microsoft/bond) | **No** (other formats) | No | No | Pooled or memory-layout serializers | Active, except Bond (archived, ended 2025-03-31) | MIT |

**No existing tool is drop-in at either level.**

- Every wire-compatible alternative gives up the Google.Protobuf API and replaces `<Protobuf Include>`.
- Only protobuf-net is mature, and it aims at a plain-class API and AOT, not at an allocation-lean version of the
  Google.Protobuf API.
- **The build hook a separate tool could use:** Grpc.Tools' `BUILD-INTEGRATION.md` (grpc/grpc `src/csharp`) documents
  `AdditionalProtocArguments="--plugin=…;--x_out=…"` per `<Protobuf>` item, and the `PROTOBUF_PROTOC` and
  `GRPC_PROTOC_PLUGIN` environment variables. So an extra protoc plugin can run without consumers leaving
  `<Protobuf Include>`. But nothing switches off `--csharp_out`, so the standard C# output is still generated
  alongside it.

## Upstream

Surveyed on 2026-10-14 by searching GitHub issues and PRs, open and closed. protobuf lands PRs through Copybara, so
"landed" means the close event carries a commit.

### History for each finding

| Finding | Related upstream work | Outcome |
|---|---|---|
| M-01, M-02 | [protobuf#329](https://github.com/protocolbuffers/protobuf/issues/329): the single-`object` oneof layout, copied from Java | Closed 2015. **No issue or PR about the boxing.** |
| M-03 | [protobuf#1700](https://github.com/protocolbuffers/protobuf/pull/1700): removed the documented ordering guarantee, explicitly to allow a change later | Merged 2016. The implementation never changed, so the `LinkedList` isn't required by any contract. |
| M-04 | **None found for C#.** Nearest is Java's [protobuf#29660](https://github.com/protocolbuffers/protobuf/issues/29660), where a maintainer was sceptical that caching empty maps pays off. | Java issue closed 2026-09-15 |
| M-05 | [protobuf#13503](https://github.com/protocolbuffers/protobuf/issues/13503) → [protobuf#14284](https://github.com/protocolbuffers/protobuf/pull/14284) (`AsStructEnumerable`, by jskeet). It covers the public enumerator, not `WriteTo`'s internal one. | The issue was closed as inactive in 2024. The PR was never reviewed and closed as inactive on 2025-03-13. |
| M-08 | [protobuf#7644](https://github.com/protocolbuffers/protobuf/pull/7644): strings across segments, span path only | Merged 2020. **The `Stream` case is not covered.** |
| M-10 | [protobuf#7663](https://github.com/protocolbuffers/protobuf/pull/7663) (JamesNK): exactly this case. [#8147](https://github.com/protocolbuffers/protobuf/pull/8147) and [#8149](https://github.com/protocolbuffers/protobuf/pull/8149) cover only the fast path. | #7663 was closed unmerged on 2021-10-14, with no response to the last review. #8147 and #8149 were merged in 2021. |
| M-12 | **None found for C#.** Java's equivalent is [protobuf#24922](https://github.com/protocolbuffers/protobuf/pull/24922). | Java PR merged 2025-12-17 |
| M-14 | **None found.** The generator's `TODO: We may be able to do better than this` is the only trace. | — |
| M-16 | [protobuf#28925](https://github.com/protocolbuffers/protobuf/issues/28925) and [protobuf#29131](https://github.com/protocolbuffers/protobuf/pull/29131): packed varint pre-count plus the second size pass (M-19). Also [#17111](https://github.com/protocolbuffers/protobuf/issues/17111) (no pre-allocation for large repeats) and [#16772](https://github.com/protocolbuffers/protobuf/pull/16772) (`UnsafeCollectionOperations`). | #29131 has been **open since 2026-08-11**: a maintainer invited the PR, and the reviewer couldn't look at it before September. #17111 closed 2024 with no change. #16772 landed 2024-12-03. |
| M-17 | **None found** (Clone copying slack). | — |
| M-18 | **None found for C#.** #29131 mentions size caching as an alternative it considered. The generator's `TODO: Memoize size of frozen messages?` is the only other trace. | — |
| M-20, M-22 | [protobuf#11406](https://github.com/protocolbuffers/protobuf/pull/11406) (span `ParseFrom` for `byte[]`) and [protobuf#8808](https://github.com/protocolbuffers/protobuf/pull/8808) (`ToByteArray` via span) | #11406 closed as inactive in 2024. #8808 was withdrawn by its author in 2021 after a legacy-interop test failed. |
| M-21, M-23 | [protobuf#8238](https://github.com/protocolbuffers/protobuf/issues/8238): buffer pooling is blocked because there is no disposal API. Also [protobuf#8321](https://github.com/protocolbuffers/protobuf/issues/8321) (pooled `ToByteArray`). | #8321 was closed in 2022 as implemented by #7645. **Nothing on the per-call 4 KiB buffer itself.** |
| M-24, M-25 | **None found.** The related designs [protobuf#7722](https://github.com/protocolbuffers/protobuf/pull/7722) (allocations moved for pooling) and [protobuf#9683](https://github.com/protocolbuffers/protobuf/pull/9683) (struct messages) are much broader. | Both were closed unmerged. On #9683, jskeet said it needs "a *lot* of research" (2022). |
| M-26 | [protobuf#9194](https://github.com/protocolbuffers/protobuf/issues/9194): code simplicity, not allocations | Closed as inactive in 2024 |
| S-20–S-35 | Steady grpc-dotnet allocation work: [#489](https://github.com/grpc/grpc-dotnet/pull/489), [#1033](https://github.com/grpc/grpc-dotnet/pull/1033), [#1850](https://github.com/grpc/grpc-dotnet/pull/1850), [#2256](https://github.com/grpc/grpc-dotnet/pull/2256), [#2597](https://github.com/grpc/grpc-dotnet/pull/2597). Still open: [#2078](https://github.com/grpc/grpc-dotnet/issues/2078) and [#2665](https://github.com/grpc/grpc-dotnet/issues/2665). [#970](https://github.com/grpc/grpc-dotnet/issues/970) (a ValueTask service API) was closed on the day it opened. | The five PRs merged between 2019 and 2025 |
| `-0.0` observation | **None found.** | — |

### How each upstream takes contributions

**protocolbuffers/protobuf (C#)**
- **Status:** C# 3.36.x is under "Active Support", not maintenance mode.
- **Rules:** a Google CLA is required. `CONTRIBUTING.md` asks for small, single-concern PRs, and inactive PRs are
  closed by a bot.
- **Reviewers:** jskeet, amanda-tarafa and JasonLunn, all visibly short of time. For example, jskeet wrote "I don't
  have the time to review this at this time" on
  [#26851](https://github.com/protocolbuffers/protobuf/pull/26851), which was closed as inactive on 2026-09-21.
- **Small, clear fixes do land:**
  - [#26455](https://github.com/protocolbuffers/protobuf/pull/26455), a generator over-allocation fix, landed in 1 day
    (2026-03-19).
  - [#29707](https://github.com/protocolbuffers/protobuf/pull/29707) landed in 13 days (2026-09-23).
- **Larger performance work is slow or dies:**
  - [#20392](https://github.com/protocolbuffers/protobuf/pull/20392) took 15 months.
  - #7663, #11406, #14284 and #26851 all died.
- **What jskeet asks for** (#13503): evidence from a real application, not microbenchmarks, and opt-in,
  non-breaking changes.

**grpc/grpc-dotnet**
- **Rules:** a CNCF EasyCLA is required.
- **Reviewers:** in practice one maintainer (JamesNK).
- **Targeted performance PRs that come with BenchmarkDotNet numbers do get merged.** For example,
  [#2697](https://github.com/grpc/grpc-dotnet/pull/2697) merged in 13 days (2026-03-25).
- **Some go unreviewed:** [#2629](https://github.com/grpc/grpc-dotnet/pull/2629) waited 4 months, and its author
  closed it in 2025.

**grpc/grpc `csharp_generator.cc`**
- **Activity:** very low. There have been 4 commits since 2022, and two PRs have been open since 2023.
- **Relevance:** none of the findings belongs here. The message TODOs (M-14, M-18) are in protobuf's
  `src/google/protobuf/compiler/csharp`.

## Appendix A: code-reading sites, message code and Google.Protobuf runtime

Scope: candidate allocation (and notable CPU) sites found by **reading code only** (task 3.1). These rows are the
full source record for the findings above. Their tier and hot-path columns are the code-reading view. The final
status of each row is in **Findings** or **Sites not counted**. The generated code is the committed corpus output (`src/Corpus/Generated/protos/`, protoc 35.1 via Grpc.Tools
2.84.0). Runtime citations are Google.Protobuf 3.36.2 (`protocolbuffers/protobuf@v36.2`); generator
citations are the protoc C# generator at `@v35.1`. A generator citation means the site is emitted into
generated code, so a fix there is Tier A unless it also needs runtime or API changes. Generated-code line
numbers point at the compiled path: `IBufferMessage.InternalMergeFrom(ref ParseContext)` and
`InternalWriteTo(ref WriteContext)`, not the `#else` (`GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE`)
legacy bodies. Hypothesis 8 (stub-layer headroom) is covered by Appendix B. Link prefixes (RT, GEN) are as defined
at the top.

| ID | Hypothesis | Location | Citation(s) | What allocates and when | Tier (prov.) | Hot / one-off | Notes |
|---|---|---|---|---|---|---|---|
| M-01 | 1. Oneof scalar boxing | Generated (emitted by generator) | `src/Corpus/Generated/protos/Oneof.cs:268` (storage), `:131-133` (getter/setter), `:561`, `:565`, `:569`, `:573` (parse); [GEN csharp_message.cc#L215](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_message.cc#L215), [GEN csharp_primitive_field.cc#L265-L270](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_primitive_field.cc#L265-L270) | A boxed `int`/`long`/`double`/`bool` every time a scalar oneof member is set, including each parse of such a member. | A | Hot | `private object choice_;` with `choice_ = value;`. The getter unboxes, which doesn't allocate. String and message members are references, so they don't box. Enum members would box too (none in the corpus). Storage is private, and the public surface is the properties plus `ChoiceCase`, so explicit-layout or per-type private fields would stay Tier A. |
| M-02 | 1. Oneof scalar boxing | Generated (emitted by generator) | `src/Corpus/Generated/protos/Oneof.cs:86` (copy ctor), `:463` (MergeFrom); [GEN csharp_primitive_field.cc#L320-L322](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_primitive_field.cc#L320-L322) | Clone and MergeFrom unbox the source value and box it again: one new box per Clone or Merge of a set scalar member. | A | Hot (Clone/Merge) | `Int32Choice = other.Int32Choice;`. The box is immutable, so `choice_ = other.choice_` would share it. |
| M-03 | 2. Map per-entry overhead | Runtime | [RT Collections/MapField.cs#L54-L56](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L54-L56), [#L184-L192](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L184-L192), [#L415-L421](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L415-L421); `src/Corpus/Generated/protos/Maps.cs:272`, `:276`, `:280` | Per parsed entry: a `LinkedListNode<KeyValuePair<K,V>>` (`list.AddLast(pair)`), plus growth of the `Dictionary<K, LinkedListNode>` buckets and entries. Key strings and value messages come on top (see M-07 and M-15). | B | Hot | The dual Dictionary + LinkedList structure exists to preserve insertion order. A different internal structure keeping the same enumeration order would be Tier B. |
| M-04 | 2. Map per-entry overhead (empty-map baseline) | Runtime + generated | [RT Collections/MapField.cs#L54-L56](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L54-L56); `src/Corpus/Generated/protos/Maps.cs:102`, `:113`, `:124`; [GEN csharp_map_field.cc#L54](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_map_field.cc#L54) | Every message instance (new, parse, Clone) allocates, per map field, a `MapField` + `Dictionary` + `LinkedList` even when the map stays empty. | B (lazy inside MapField) or A (lazy field in generated code) | Hot (per instance) | Upstream TODO on line 54: `// TODO: Don't create the map/list until we have an entry. (Assume many maps will be empty.)` |
| M-05 | 2. Map per-entry overhead | Runtime | [RT Collections/MapField.cs#L475](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L475), [#L486](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L486) | A boxed `LinkedList<T>.Enumerator` per non-empty map field per write: `list` is passed as `IEnumerable<KeyValuePair<K,V>>` and enumerated with `foreach`. | B | Hot | `IEnumerable<KeyValuePair<TKey, TValue>> listToWrite = list;`. `CalculateSize` enumerates the typed `list` (no box). The deterministic path also allocates a sorted `List` copy (`#L478`). |
| M-06 | 2. Map per-entry overhead (CPU) | Runtime | [RT Collections/MapField.cs#L490](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L490), [#L501-L521](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/MapField.cs#L501-L521) | No allocation. Each entry's size is computed in `CalculateSize`, then again in `WriteTo` for its length prefix. With message values, that recomputes the value's `CalculateSize`. | B | Hot (CPU) | Same family as M-18. |
| M-07 | 3. String decoding | Runtime | [RT ParsingPrimitives.cs#L537-L541](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L537-L541), [#L560-L598](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L560-L598); `src/Corpus/Generated/protos/Strings.cs:331`, `:335`, `:339`, `:343` | One `string` per non-empty string field parsed. The fast path decodes straight from the input span (`Utf8Encoding.GetString(sourceBytes, length)`, `#L585`), with no intermediate buffer. | C (to avoid) | Hot | Empty strings return `string.Empty` (`#L565`), so they don't allocate. The string itself is inherent to the `string` property type; avoiding it needs lazy or UTF-8 storage behind a different API. Likely the "inherent cost" baseline, not a finding. |
| M-08 | 3. String decoding (Stream source) | Runtime | [RT ParsingPrimitives.cs#L604-L657](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L604-L657), [#L782-L787](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L782-L787) | When parsing from a `Stream` and a string spans the 4 KiB buffer boundary: a temporary `byte[length]` (`ReadRawBytes`, `#L654`), and then the string. | B | Hot (Stream parses with strings near or over the buffer size) | `IsDataAvailableInSource` is never true for a Stream ("the total length is unknown"), so the pooled/stackalloc branch (`#L610-L645`) is used only for `ReadOnlySequence` sources. |
| M-09 | 3. String encoding (CPU) | Runtime | [RT CodedOutputStream.ComputeSize.cs#L110-L114](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/CodedOutputStream.ComputeSize.cs#L110-L114), [RT WritingPrimitives.cs#L169](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/WritingPrimitives.cs#L169) | No allocation. `Utf8Encoding.GetByteCount` runs once in `CalculateSize`, and again in `WriteString` for strings longer than 42 chars or when the buffer is short (`#L162`). | B | Hot (CPU) | Repeats once per ancestor level in nested messages (M-18). |
| M-10 | 3. String encoding (segmented output) | Runtime | [RT WritingPrimitives.cs#L186-L192](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/WritingPrimitives.cs#L186-L192) | A temporary `byte[]` (`Utf8Encoding.GetBytes(value)`) when the string doesn't fit the current output segment: writing to a Stream (4 KiB buffer) or an `IBufferWriter` segment (the gRPC path). | B | Hot (large strings, Stream/IBufferWriter output) | The code comment there names it an "Opportunity for future optimization". It doesn't trigger for `ToByteArray`, which writes into an exactly sized array. |
| M-11 | 4. Bytes copying on parse | Runtime | [RT ParsingPrimitives.cs#L547-L551](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L547-L551), [#L413-L427](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L413-L427), [RT ByteString.cs#L38-L50](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ByteString.cs#L38-L50); `src/Corpus/Generated/protos/Bytes.cs:264`, `:268` | Per bytes field: a new `byte[size]` copied from the input (`buffer.Slice(...).CopyTo(bytes)`), plus a `ByteString` object wrapping it. Two objects and one full copy. | C (to avoid the copy or the wrapper) | Hot | Aliasing the input instead of copying changes buffer ownership semantics, so it would need an opt-in API (cf. `UnsafeByteOperations.UnsafeWrap`, [RT UnsafeByteOperations.cs#L52](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/UnsafeByteOperations.cs#L52)). `ByteString` is a class, so the wrapper is inherent to the API. |
| M-12 | 4. Bytes (empty but present) | Runtime | [RT ParsingPrimitives.cs#L420-L426](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L420-L426), [#L550](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/ParsingPrimitives.cs#L550) | A zero-length present bytes value (BytesValue, optional, oneof or repeated) allocates `new byte[0]` and a new `ByteString`, instead of returning `ByteString.Empty`. | B | Hot (small) | Strings have the `length == 0` shortcut (M-07); bytes don't. |
| M-13 | 5. Nested message instances | Generated (emitted by generator) | `src/Corpus/Generated/protos/Nesting.cs:311-314`; [GEN csharp_message_field.cc#L85-L94](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_message_field.cc#L85-L94) | One message object per nested level parsed (`Child = new Node()` when null, then `ReadMessage`). Each instance also carries its own eager collections (M-04, M-24). | C (to avoid) | Hot | Inherent to class-per-message. Avoiding it means pooling or arena semantics, which is an API change. Expect the measurement to show cost linear in depth; the question is whether anything beyond one object per level exists (see M-24, M-25). |
| M-14 | 5. Nested message instances (oneof message) | Generated (emitted by generator) | `src/Corpus/Generated/protos/Oneof.cs:581-586`; [GEN csharp_message_field.cc#L231-L245](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_message_field.cc#L231-L245) | A oneof message member always parses into a fresh `subBuilder`. If the member is already set (repeated occurrence, or MergeFrom into a populated message), the existing value is merged into the new one, so there's an extra object plus a copy. | A | Hot (merge case only) | Upstream TODO at `#L233`: `// TODO: We may be able to do better than this`. A single-occurrence parse into a fresh message allocates exactly one, as it should. |
| M-15 | 5. Nested message instances (repeated/map values) | Runtime | [RT FieldCodec.cs#L333-L340](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/FieldCodec.cs#L333-L340) | One message per repeated or map message element (`parser.CreateTemplate()`). | C (to avoid) | Hot | Inherent, listed for completeness. |
| M-16 | 6. Repeated-field growth | Runtime | [RT Collections/RepeatedField.cs#L146](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/RepeatedField.cs#L146), [#L158](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/RepeatedField.cs#L158), [#L315-L334](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/RepeatedField.cs#L315-L334); `src/Corpus/Generated/protos/Repeated.cs:339`, `:349`, `:358`, `:362` | Packed **varint** fields, all unpacked fields, and repeated strings/messages grow by `Add` one element at a time: `MinArraySize` 8, then doubling (`new T[size]` + `Array.Copy`). That leaves discarded intermediate arrays and slack capacity in the final array. | B | Hot | Packed **fixed-size** fields (e.g. `packed_double`) are pre-sized exactly (`EnsureSize(count + length / FixedSize)`, `#L119`), so this hits `packed_int32`, `unpacked_*`, `strings` and `items`. A packed varint payload could be pre-counted (bytes with the MSB clear within the length limit), which is internal (Tier B). Upstream TODO at `#L100`: "Inline some of the Add code". |
| M-17 | 6. Repeated-field growth (Clone) | Runtime | [RT Collections/RepeatedField.cs#L56-L66](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/RepeatedField.cs#L56-L66) | Clone copies the whole backing array including slack capacity (`(T[])array.Clone()`), and for message elements then clones each element. | B | Hot (Clone) | Slack size depends on the M-16 growth history. |
| M-18 | 7. Repeated size calculation on deep messages (CPU) | Runtime + generated | [RT WritingPrimitivesMessages.cs#L26-L30](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/WritingPrimitivesMessages.cs#L26-L30), [RT CodedOutputStream.ComputeSize.cs#L129-L133](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/CodedOutputStream.ComputeSize.cs#L129-L133), [RT MessageExtensions.cs#L97](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L97); `src/Corpus/Generated/protos/Nesting.cs:204`, `:223`; [GEN csharp_message_field.cc#L121](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_message_field.cc#L121), [GEN csharp_message.cc#L578](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_message.cc#L578) | No allocation. `ToByteArray` computes the whole tree's size, then every nested `WriteMessage` calls `value.CalculateSize()` again for its length prefix. A depth-d chain does O(d²) `CalculateSize` work, including repeated `GetByteCount` on each level's strings (M-09). | A (memoised size in a generated private field, cleared by generated setters) or B | Hot (CPU) | No memoisation, and the generator carries `// TODO: Memoize size of frozen messages?`. Other protobuf languages cache the serialized size per message. Mutable public collections (RepeatedField/MapField) complicate invalidation, so a cache may need runtime help (Tier B). |
| M-19 | 7. Repeated size calculation (packed fields, CPU) | Runtime | [RT Collections/RepeatedField.cs#L178](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/RepeatedField.cs#L178), [#L255](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/RepeatedField.cs#L255) | No allocation. A packed **varint** field's data size is computed in `CalculateSize`, then again in `WriteTo`, which is a second pass over every element. | B | Hot (CPU) | Fixed-size packed fields compute the size in O(1). |
| M-20 | Other: parse entry point | Runtime | [RT MessageParser.cs#L259-L264](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageParser.cs#L259-L264), [RT MessageExtensions.cs#L259-L269](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L259-L269) | `ParseFrom(byte[])` allocates a `CodedInputStream` object per call. | B | Hot | The `ReadOnlySpan`/`ReadOnlySequence` overloads go through `ParseContext.Initialize` with no stream object ([#L310-L325](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L310-L325)). The byte[] path could do the same for `IBufferMessage` types. |
| M-21 | Other: parse from Stream | Runtime | [RT MessageExtensions.cs#L296-L304](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L296-L304), [RT CodedInputStream.cs#L104-L105](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/CodedInputStream.cs#L104-L105) | `ParseFrom(Stream)` allocates a `CodedInputStream` plus a fresh 4096-byte buffer per call. | B | Hot | Pooling the buffer would be internal. |
| M-22 | Other: `ToByteArray` | Runtime | [RT MessageExtensions.cs#L94-L102](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L94-L102) | The result `byte[]` (inherent) plus a `CodedOutputStream` object per call. | B | Hot | The `WriteTo(Span<byte>)` path ([#L189-L196](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L189-L196)) writes through a `WriteContext` struct with no stream object, so `ToByteArray` could use it internally. |
| M-23 | Other: `WriteTo(Stream)` | Runtime | [RT MessageExtensions.cs#L109-L116](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L109-L116), [RT CodedOutputStream.cs#L88](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/CodedOutputStream.cs#L88), [#L120](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/CodedOutputStream.cs#L120) | A `CodedOutputStream` plus a fresh 4096-byte buffer per call. | B | Hot | |
| M-24 | Other: eager repeated-field objects | Generated (emitted by generator) | `src/Corpus/Generated/protos/Repeated.cs:101`, `:112`, `:123`, `:134`, `:145`, `:156`; [GEN csharp_repeated_primitive_field.cc#L39](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_repeated_primitive_field.cc#L39), [GEN csharp_repeated_message_field.cc#L54](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_repeated_message_field.cc#L54), [GEN csharp_repeated_enum_field.cc#L39](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_repeated_enum_field.cc#L39) | One `RepeatedField<T>` object per repeated field per message instance, even when it stays empty. The backing array starts as the shared static `EmptyArray` ([RT RepeatedField.cs#L39-L42](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/Collections/RepeatedField.cs#L39-L42)), so it's one object per field. | A | Hot (per instance) | A lazily created field behind the same public getter keeps the API. The generated `Equals`/`CalculateSize`/`WriteTo`/`MergeFrom` would need null-aware bodies, all of which are generated. |
| M-25 | Other: Clone discards initialiser collections | Generated (emitted by generator) | `src/Corpus/Generated/protos/Repeated.cs:81-88`, `src/Corpus/Generated/protos/Maps.cs:85-89`; [GEN csharp_message.cc#L360](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_message.cc#L360), [GEN csharp_repeated_primitive_field.cc#L102](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_repeated_primitive_field.cc#L102), [GEN csharp_map_field.cc#L116](https://github.com/protocolbuffers/protobuf/blob/v35.1/src/google/protobuf/compiler/csharp/csharp_map_field.cc#L116) | The copy constructor chains `: this()`, so the field initialisers allocate every collection (M-04, M-24) and each one is then replaced by `other.x_.Clone()`. Per Clone that throws away 1 object per repeated field and 3 per map field. | A | Hot (Clone) | `public Repeated(Repeated other) : this() { packedInt32_ = other.packedInt32_.Clone(); ... }` |
| M-26 | Other: unknown fields (parse) | Runtime | [RT UnknownFieldSet.cs#L31](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/UnknownFieldSet.cs#L31), [#L139-L158](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/UnknownFieldSet.cs#L139-L158), [#L206-L207](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/UnknownFieldSet.cs#L206-L207), [RT UnknownField.cs#L254-L261](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/UnknownField.cs#L254-L261); `src/Corpus/Generated/protos/UnknownV1.cs:261` | When unknown fields are present: an `UnknownFieldSet` + `Dictionary<int, UnknownField>` per message. Per unknown field number: an `UnknownField` + a `List<T>` + the list's array. Length-delimited unknowns add a `byte[]` + `ByteString` (M-11). | B | Hot only when unknowns are present | Field values are re-decoded into typed lists. Keeping the raw bytes would be an internal change if the public `UnknownFieldSet` surface is preserved. |
| M-27 | Other: unknown fields (write/size) | Runtime | [RT UnknownFieldSet.cs#L31](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/UnknownFieldSet.cs#L31), [#L68-L74](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/UnknownFieldSet.cs#L68-L74), [#L79-L86](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/UnknownFieldSet.cs#L79-L86) | A boxed Dictionary enumerator per `WriteTo` and per `CalculateSize`. `fields` is declared as `IDictionary<int, UnknownField>`, so `foreach` goes through the interface. | B | Hot only when unknowns are present | A private field type change only. |
| M-28 | Other: wrapper types | Generated + runtime | `src/Corpus/Generated/protos/Wrappers.cs:432`, `:439`, `:446`, `:453`, `:460`, `:467`; [RT FieldCodec.cs#L521-L556](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/FieldCodec.cs#L521-L556) | **No allocation site found** for struct wrappers: `int?`/`long?`/`double?`/`bool?` are read in place by the static codec. `StringValue`/`BytesValue` cost the same as M-07/M-11/M-12. | n/a | n/a | CPU only: the wrapper `Write` computes the inner size (`CalculateSizeWithTag`) for the length prefix, after `CalculateSize` already did (`#L546-L556`). That's minor, and part of the M-18 family. |
| M-29 | Other: proto3 `optional` presence | Generated | `src/Corpus/Generated/protos/Optional.cs:57` | **No allocation site found.** Presence is tracked in a private `_hasBits0` int for value types, and by null for string/bytes. | n/a | n/a | Strings and bytes cost as M-07/M-11/M-12. |
| M-30 | Other: static/type initialisation | Generated | `src/Corpus/Generated/protos/Nesting.cs:24-27` (descriptor from base64), `:49` (parser singleton), `src/Corpus/Generated/protos/Maps.cs:101`, `:112`, `:123` (static codecs) | Descriptor building (base64 decode, `FileDescriptor` graph), the `MessageParser<T>` singleton and the field codecs. | n/a | **One-off** (per type/process) | Excluded from per-operation savings. Listed so attribution traces can recognise these and discount them. |

## Appendix B: code-reading sites, service stubs and the gRPC call path

Scope: a code reading (task 3.2) of the stub and marshaller code that `grpc_csharp_plugin` (Grpc.Tools 2.84.0,
grpc v1.84.0) generates for `CorpusService`, and of the Grpc.Core.Api, Grpc.Net.Client and Grpc.AspNetCore.Server
2.84.0 paths it hands messages to, as far as Google.Protobuf 3.36.2's parse/write entry points. These rows come
from reading the code, and the "provisional" columns are that reading's view. The measured outcome is hypothesis 8
(rejected) and the S-20–S-35 row in **Findings**. Tiers: **A** = generated private members or method bodies only (a change to
`csharp_generator.cc`); **B** = runtime-library internals with no public API change; **C** = public API change.
"Per call" means once per RPC; "per message" means once per message sent or received; one-off means once per
type, process or channel. Message-level costs inside `CalculateSize`/`WriteTo`/`MergeFrom` belong to task 3.1
and are only cross-referenced here.

Citations: generated code by repo path; the generator at
`https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc`; grpc-dotnet at
`https://github.com/grpc/grpc-dotnet/blob/v2.84.0/…`; Google.Protobuf at
`https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/…`. (`#L` anchors below.)

### Generated stub code

| ID | Location | Citation(s) | What allocates and when | Provisional tier | Hot-path or one-off | Notes |
|---|---|---|---|---|---|---|
| S-01 | generated → Google.Protobuf | `src/Corpus/Generated/protos/ServiceGrpc.cs:25-27`; generator [csharp_generator.cc#L347-L350](https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L347-L350); [MessageExtensions.cs#L150-L158](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L150-L158) | **CPU, no allocation**: `context.SetPayloadLength(message.CalculateSize())` walks the whole message once, then `WriteTo(message, context.GetBufferWriter())` serializes it. Per message. | A (for the top-level call); nested re-sizing is message-side (3.1) | Hot path | The size call is needed, not waste: a known length is what selects the runtime's pooled/direct buffer (S-24, S-33); without it the runtime falls back to `ArrayBufferWriter`. `WriteTo(IBufferWriter)` itself does not call `CalculateSize` again, but nested sub-messages are re-sized while writing their length prefixes: cross-ref hypothesis 7 (3.1). |
| S-02 | generated | `src/Corpus/Generated/protos/ServiceGrpc.cs:23` | Type check `message is IBufferMessage`. Per message; no allocation. | A | Hot path, negligible | Static type is `IMessage` because one helper serves every message type; a generic helper could make it a JIT-time constant. Expected to be noise; listed for completeness. |
| S-03 | generated → Google.Protobuf → Grpc.Net.Client | `src/Corpus/Generated/protos/ServiceGrpc.cs:31`; generator [#L356-L358](https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L356-L358); [MessageExtensions.cs#L94-L102](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L94-L102); [GrpcCallSerializationContext.cs#L155-L163](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCallSerializationContext.cs#L155-L163) | Fallback serializer: `ToByteArray` allocates a `byte[]` of the message size, which the context then copies into a rented buffer. Per message. | A | Not taken by default | Taken only when `GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION` is defined or the message is not `IBufferMessage`. Every corpus message is `IBufferMessage` (`src/Corpus/Generated/protos/Scalars.cs:64`), so this path is dead for the corpus. Provisionally **not a finding**. |
| S-04 | generated → Grpc.Net.Common/Shared | `src/Corpus/Generated/protos/ServiceGrpc.cs:49`; generator [#L394](https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L394); [DefaultDeserializationContext.cs#L34-L40](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Shared/DefaultDeserializationContext.cs#L34-L40) | Fallback deserializer: `PayloadAsNewBuffer()` copies the payload into a new exact-size `byte[]`. Per message. | A | Not taken by default | Same guard as S-03; the default path is `PayloadAsReadOnlySequence()` (`ServiceGrpc.cs:46`), which does not copy. Provisionally **not a finding**. |
| S-05 | generated → Google.Protobuf | `src/Corpus/Generated/protos/ServiceGrpc.cs:46`; [MessageParser.cs#L310-L315](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageParser.cs#L310-L315); [MessageExtensions.cs#L310-L317](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/MessageExtensions.cs#L310-L317); [SegmentedBufferHelper.cs#L51-L59](https://github.com/protocolbuffers/protobuf/blob/v36.2/csharp/src/Google.Protobuf/SegmentedBufferHelper.cs#L51-L59) | `parser.ParseFrom(ReadOnlySequence)` allocates the result message via `factory()` (`Scalars.cs:67`: `() => new Scalars()`), plus whatever the message's fields allocate (3.1). Per message. `ParseContext`/`SegmentedBufferHelper` are structs: no allocation on single- or multi-segment input. | C (to avoid the message object itself) | Hot path, inherent | The stub API returns a new `T` per message; reusing instances would need a different public API. Field-level costs (strings, `ByteString` copies, collections) are 3.1's. |
| S-06 | generated | `src/Corpus/Generated/protos/ServiceGrpc.cs:35-38`; generator [#L364-L372](https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L364-L372) | `__Helper_MessageCache<T>.IsBufferMessage` evaluates reflection (`GetTypeInfo(...).IsAssignableFrom`) once per `T`. | A | One-off | Per closed generic type; not per call. |
| S-07 | generated → Grpc.Core.Api | `src/Corpus/Generated/protos/ServiceGrpc.cs:53`, `:56-85`; generator [#L404-L405](https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L404-L405); [Marshaller.cs#L121-L123](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Core.Api/Marshaller.cs#L121-L123) | One `Marshaller<T>` per message type, two delegates (method group + `context => …` lambda), one `Method<,>` per RPC method, all in static fields. | A | One-off | Per service type, at type initialisation. |
| S-08 | generated → Grpc.Core.Api | `src/Corpus/Generated/protos/ServiceGrpc.cs:153`, `:163`, `:173`, `:183`, `:193`; generator [#L574](https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L574), [#L622](https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L622); [CallOptions.cs#L29](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Core.Api/CallOptions.cs#L29) | Convenience overloads build `new grpc::CallOptions(headers, deadline, cancellationToken)` and forward. Per call. | — | Hot path, negligible | `CallOptions` is a `struct`: **no heap allocation**, one extra call frame. Provisionally **not a finding**. |
| S-09 | generated → Grpc.AspNetCore.Server | `src/Corpus/Generated/protos/ServiceGrpc.cs:211-218`, `:225-231`; generator [#L709](https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L709), [#L772](https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L772); [BinderServiceModelProvider.cs#L50](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Model/Internal/BinderServiceModelProvider.cs#L50); [ProviderServiceBinder.cs#L83](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Model/Internal/ProviderServiceBinder.cs#L83) | `BindService` builds method delegates. grpc-dotnet calls `BindService(binder, null)` by reflection, so the generated `serviceImpl == null ? null : …` branch allocates nothing, and grpc-dotnet creates its own delegates with `Delegate.CreateDelegate`. | A / B | One-off | Startup only. |
| S-10 | generated | `src/Corpus/Generated/protos/ServiceGrpc.cs:100`, `:106`, `:112`, `:118`; generator [#L492](https://github.com/grpc/grpc/blob/v1.84.0/src/compiler/csharp_generator.cc#L492) | Base virtuals throw `new RpcException(new Status(Unimplemented, ""))`. Per call to an unimplemented method only. | A | Not on the success path | Not a finding for implemented services. |

### Runtime call path (Grpc.Core.Api / Grpc.Net.Client / Grpc.AspNetCore.Server)

Client side (Grpc.Net.Client, `HttpClientCallInvoker` → `GrpcCall`):

| ID | Location | Citation(s) | What allocates and when | Provisional tier | Hot-path or one-off | Notes |
|---|---|---|---|---|---|---|
| S-20 | Grpc.Net.Client / Grpc.Core.Api | [HttpClientCallInvoker.cs#L221](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpClientCallInvoker.cs#L221), [#L125](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpClientCallInvoker.cs#L125); [GrpcCall.cs#L70-L75](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCall.cs#L70-L75), [#L150](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCall.cs#L150); [AsyncUnaryCall.cs#L33](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Core.Api/AsyncUnaryCall.cs#L33) | Per call: `GrpcCall<,>`, a `CancellationTokenSource`, two `TaskCompletionSource`s (HTTP response, call status), a response `TaskCompletionSource` (unary, client streaming), and the public `AsyncUnaryCall`/`Async*Call` wrapper (a sealed class). | B (wrapper: C) | Hot path, per call | Wrapper callbacks are cached statically (`Callbacks<,>`, [#L227-L228](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpClientCallInvoker.cs#L227-L228)). Nothing here is reachable from generated code. |
| S-21 | Grpc.Net.Client → System.Net.Http | [GrpcCall.cs#L960-L972](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCall.cs#L960-L972) | Per call: `HttpRequestMessage` and its header collection (user-agent, TE, accept-encoding added with `TryAddWithoutValidation`), plus everything `HttpClient`/HTTP/2 allocates per request. | B (largely outside gRPC) | Hot path, per call | Expected to dominate the whole-call cost; §4.4 should show it. |
| S-22 | Grpc.Net.Client | [GrpcCall.cs#L122-L134](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCall.cs#L122-L134) | Unary and server-streaming: `PushUnaryContent<,>` plus a delegate for the local function `WriteAsync`, which captures `this`. Per call. | B | Hot path, per call | |
| S-23 | Grpc.Net.Client | [GrpcCall.cs#L456](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCall.cs#L456) | `RunCall` async state machine (boxed once it awaits). Per call. | B | Hot path, per call | |
| S-24 | Grpc.Net.Client | [GrpcCall.NonGeneric.cs#L44-L67](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCall.NonGeneric.cs#L44-L67); [GrpcCallSerializationContext.cs#L38-L46](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCallSerializationContext.cs#L38-L46), [#L205-L226](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCallSerializationContext.cs#L205-L226) | Write side: one `GrpcCallSerializationContext` per call, cached and reused for each streamed message. When the payload length is known and there is no compression (the generated S-01 path), the message is written into an `ArrayPool` buffer with the 5-byte header in place: **no per-message heap allocation**. Otherwise a `new ArrayBufferWriter<byte>` per context. | B | Per call (context); per message pooled | The generated code already selects the allocation-free branch. `SerializationContextLease` is a struct ([SerializationContextLease.cs#L29](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/SerializationContextLease.cs#L29)). |
| S-25 | Grpc.Net.Client | [StreamExtensions.cs#L63](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/StreamExtensions.cs#L63), [#L71](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/StreamExtensions.cs#L71), [#L110-L114](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/StreamExtensions.cs#L110-L114), [#L146-L153](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/StreamExtensions.cs#L146-L153); [GrpcCall.NonGeneric.cs#L72](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCall.NonGeneric.cs#L72) | Read side: a zero-byte read, then an `ArrayPool` rent of ≥4096 bytes (re-rented if the message is larger); the payload is handed to the generated deserializer as a `ReadOnlySequence` over that pooled array (no copy). One `DefaultDeserializationContext` per call. Per message: pooled; async state machine of `ReadMessageAsync` when it awaits. | B | Hot path, per message (pooled) | The only per-message heap allocation the deserializer causes is the message itself (S-05). |
| S-26 | Grpc.Net.Client | [HttpContentClientStreamWriter.cs#L53-L55](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpContentClientStreamWriter.cs#L53-L55), [#L88-L106](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpContentClientStreamWriter.cs#L88-L106), [#L154-L172](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpContentClientStreamWriter.cs#L154-L172); [StreamExtensions.cs#L290-L304](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/StreamExtensions.cs#L290-L304) | Client and duplex streaming writes: per call, the writer, two `TaskCompletionSource`s and a `PushStreamContent`; per message, up to three async state machines (`WriteCoreAsync` → `WriteAsyncCore` → `WriteMessageAsync`) and a `FlushAsync`. | B | Hot path, per message | The static local function `WriteMessageToStream` (#L103) gets a compiler-cached delegate. |
| S-27 | Grpc.Net.Client | [HttpContentClientStreamReader.cs#L54](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpContentClientStreamReader.cs#L54), [#L63](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpContentClientStreamReader.cs#L63), [#L122](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpContentClientStreamReader.cs#L122) | Server and duplex streaming reads: per call, the reader and a `TaskCompletionSource`; per message, `MoveNext` returns `Task<bool>` from the async `MoveNextCore` (boxed state machine when it awaits). | B (`Task<bool>` return is the public `IAsyncStreamReader` API: C) | Hot path, per message | |
| S-28 | Grpc.Net.Client | [HttpClientCallInvoker.cs#L164](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpClientCallInvoker.cs#L164), [#L220](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpClientCallInvoker.cs#L220); [GrpcChannel.cs#L572-L575](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/GrpcChannel.cs#L572-L575) | **CPU only**: method info is looked up in the channel's concurrent cache twice per call (once in `CreateRootGrpcCall`, again in `CreateGrpcCall`). | B | Hot path, negligible | Keyed on the generated `Method<,>` static (S-07), so no allocation after the first call. |
| S-29 | Grpc.Net.Client | `src/Corpus/Generated/protos/ServiceGrpc.cs:158`; [HttpClientCallInvoker.cs#L151-L154](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/HttpClientCallInvoker.cs#L151-L154) | Blocking unary is sync-over-async: same allocations as the async call, plus a blocked thread. Per call. | B | Hot path (blocking callers only) | Benchmarks should use `UnaryAsync`. |

Server side (Grpc.AspNetCore.Server):

| ID | Location | Citation(s) | What allocates and when | Provisional tier | Hot-path or one-off | Notes |
|---|---|---|---|---|---|---|
| S-30 | Grpc.AspNetCore.Server | [ServerCallHandlerBase.cs#L64](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/CallHandlers/ServerCallHandlerBase.cs#L64); [HttpContextServerCallContext.cs#L64](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/HttpContextServerCallContext.cs#L64), [#L68](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/HttpContextServerCallContext.cs#L68); [UnaryServerCallHandler.cs#L42](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/CallHandlers/UnaryServerCallHandler.cs#L42) | Per call: `HttpContextServerCallContext`, its lazily-created serialization and deserialization contexts, and the handler's async state machine. | B | Hot path, per call | Plus everything ASP.NET Core/Kestrel/TestHost allocate per request. |
| S-31 | Grpc.AspNetCore.Server | [UnaryServerMethodInvoker.cs#L100-L104](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Shared/Server/UnaryServerMethodInvoker.cs#L100-L104); [DefaultGrpcServiceActivator.cs#L28-L33](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/DefaultGrpcServiceActivator.cs#L28-L33) | Per call: the service instance, from DI or `ActivatorUtilities` (transient by default). | B / app configuration | Hot path, per call | The §4.4 benchmark must state how the service is registered; a singleton removes this. |
| S-32 | Grpc.AspNetCore.Server | [PipeExtensions.cs#L196](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/PipeExtensions.cs#L196), [#L231-L233](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/PipeExtensions.cs#L231-L233), [#L295](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/PipeExtensions.cs#L295), [#L324-L326](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/PipeExtensions.cs#L324-L326) | Read side: the request is a slice of the `PipeReader` buffer passed straight to the generated deserializer. **No payload copy**; the `ValueTask` state machine is boxed only if the read awaits. Per message. | B | Hot path, per message | As on the client, the deserializer's only per-message allocation is the message (S-05). |
| S-33 | Grpc.AspNetCore.Server | [HttpContextSerializationContext.cs#L42-L54](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/HttpContextSerializationContext.cs#L42-L54), [#L124-L137](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/HttpContextSerializationContext.cs#L124-L137), [#L165-L176](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/HttpContextSerializationContext.cs#L165-L176); [PipeExtensions.cs#L46-L64](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/PipeExtensions.cs#L46-L64) | Write side: with a known length and no compression (again the S-01 path), the header and message are written straight into the response `PipeWriter`: **no intermediate buffer**. Otherwise `new ArrayBufferWriter<byte>`. | B | Hot path, per message | |
| S-34 | Grpc.AspNetCore.Server | [HttpContextStreamWriter.cs#L71](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/HttpContextStreamWriter.cs#L71), [#L102](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/HttpContextStreamWriter.cs#L102); [PipeExtensions.cs#L80](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/PipeExtensions.cs#L80), [#L105](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/PipeExtensions.cs#L105); [HttpContextStreamReader.cs#L55-L84](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/HttpContextStreamReader.cs#L55-L84) | Streaming: per message, the `WriteCoreAsync` and `WriteStreamedMessageAsync` state machines and a `FlushAsync`; reads return cached `TrueTask`/`FalseTask` when synchronous, else an async local function's `Task<bool>`. | B (`Task<bool>`: C) | Hot path, per message | |
| S-35 | Grpc.AspNetCore.Server | [ServerStreamingServerCallHandler.cs#L51](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/CallHandlers/ServerStreamingServerCallHandler.cs#L51); [ClientStreamingServerCallHandler.cs#L52](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/CallHandlers/ClientStreamingServerCallHandler.cs#L52); [DuplexStreamingServerCallHandler.cs#L49-L50](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/CallHandlers/DuplexStreamingServerCallHandler.cs#L49-L50) | Per streaming call: `HttpContextStreamReader<T>` and/or `HttpContextStreamWriter<T>`. | B | Per call | |
| S-36 | Grpc.Net.Client / Grpc.AspNetCore.Server | [StreamExtensions.cs#L260](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/StreamExtensions.cs#L260); [GrpcCallSerializationContext.cs#L292-L301](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.Net.Client/Internal/GrpcCallSerializationContext.cs#L292-L301); [PipeExtensions.cs#L448](https://github.com/grpc/grpc-dotnet/blob/v2.84.0/src/Grpc.AspNetCore.Server/Internal/PipeExtensions.cs#L448) | Compression: `MemoryStream`s and array copies per message. | B | Per message, only with compression | Compression is off in the planned benchmarks; out of scope, recorded only. |

### Hypothesis 8: headroom in the stub layer (the code-reading argument)

**Rejected; the measurement is in Hypotheses above.** The code reading below predicted that result. What generated
code alone controls is small and already allocation-optimal on the default path:

- Per message, the generated marshaller does one `CalculateSize` plus `WriteTo(IBufferWriter)` (S-01), and
  `ParseFrom(ReadOnlySequence)` (S-05). Its `SetPayloadLength` call is what lets both runtimes write into a pooled
  buffer or straight into the response pipe (S-24, S-33), and both runtimes hand the deserializer a no-copy
  sequence (S-25, S-32). The copying fallbacks (S-03, S-04) are compiled out of the path for generated messages.
- Per call, the generated client adds only a `CallOptions` struct and a forwarding call (S-08); everything else
  generated is one-off (S-06, S-07, S-09).
- The per-message allocations left on a stub path are the parsed message itself (S-05, inherent unless the
  public API changes: Tier C) and whatever the message's own field code allocates (task 3.1, hypotheses 1–7).
- The per-call and per-message overhead that is not the message (S-20–S-23, S-26, S-27, S-30, S-31, S-34, S-35) lives
  entirely in Grpc.Net.Client, Grpc.AspNetCore.Server and System.Net.Http/ASP.NET Core: Tier B at best, and out of a
  generator's reach. A separate generator would inherit all of it.

The one generated-code CPU question is S-01's double walk on nested messages, which is hypothesis 7 (M-18).
`MarshallerBenchmarks` against `EndToEndBenchmarks` confirmed the rest: the marshaller allocates only the message
(104 B for `Scalars`), about 1.5 % of a unary call.

## Separate-tool assessment

A separate tool generates the message code in place of protoc's C# generator, and keeps the Google.Protobuf runtime.
It is assessed at the two compatibility levels defined under Prior art. The effort figures are rough judgements, not
estimates from a plan.

**What a separate tool can reach.**
- **Tier A: yes.** It owns the generated code, so it can make every Tier A change.
- **Tier B: no.** Those changes live inside the runtime's internals, and generated code reaches the runtime only
  through its public API.
- **Shipping its own runtime instead breaks the API.** `RepeatedField`, `MapField`, `ByteString` and `IMessage<T>`
  are Google.Protobuf types, so replacing them is Tier C.
- **The stub generator offers nothing to change** (hypothesis 8 rejected).
- **Two Tier B costs are partly reachable through public API:**
  - **M-18:** `WriteContext.WriteLength` is public, so generated code can write a cached size and then the
    sub-message's own body.
  - **M-16, packed fields only:** `RepeatedField<T>.Capacity` is public, so generated code can size a packed field
    from its byte length (an upper bound) before reading it. This is untested here. Unpacked fields give no length
    up front.

### Level 1: API only

Consumers keep the generated public API, but change how the code is generated. For example, they run the tool, or
reference its own MSBuild package with its own item type.

- **Savings available:** the Tier A rows plus the partial M-16 above, all on the gRPC path.
  - **M-01, M-02:** 24 B per parse, Clone or Merge of a set scalar oneof member.
  - **M-04, M-24 (lazy collections):** up to 152 B per unused map field and 32 B per unused repeated field, per
    instance. This is up to 456 B of `New(Sample: Maps/empty)`'s 504 B, and 192 B of `New(Sample: Repeated/empty)`'s
    264 B.
  - **M-25:** the discarded initialisers per Clone, 456 B of `Clone(Sample: Maps/empty)`'s 960 B. With lazy
    collections, an empty `Maps` would clone as its 48 B object alone.
  - **M-14:** 40 B per merge into a set oneof message.
  - **M-18:** about 60 % of a depth-8 write's CPU.
  - **Scale:** these are tens to hundreds of bytes per message. Next to a 14,240 B unary call, they are per-message
    costs inside the 1.5 % generated share, plus whatever a message's eager collections add.
- **Rough effort: weeks for a first version, then ongoing.**
  - Fork protoc's C# generator (C++, `src/google/protobuf/compiler/csharp`) and change the emitters behind these
    rows.
  - Prove the API is unchanged: descriptors, reflection, JSON, well-known types, proto2/editions features and
    extensions.
  - Pass the protobuf conformance suite.
  - The ongoing cost is tracking every protoc release, because the runtime and the generator move together.
  - Writing a generator from scratch would take months.

### Level 2: API and build integration

Consumers keep `<Protobuf Include>` and the Grpc.Tools targets, and add one package reference.

- **Savings available:** the same as Level 1. Build integration removes adoption friction; it saves no bytes.
- **Mechanism:**
  - Grpc.Tools reads the protoc path from the MSBuild property `Protobuf_ProtocFullPath` (or the `PROTOBUF_PROTOC`
    environment variable; see `build/_protobuf/Google.Protobuf.Tools.targets` in Grpc.Tools 2.84.0). A package could
    set it to its own protoc build, or to a wrapper that sends `--csharp_out` to the tool's generator.
  - `AdditionalProtocArguments` alone is not enough: it adds a plugin, but `--csharp_out` still runs, so every type
    would be generated twice.
- **Rough effort: Level 1 plus a few weeks.**
  - Native protoc or wrapper binaries for every platform Grpc.Tools ships (Windows, Linux, macOS; x64 and arm64).
  - A `.props` file that sets the property.
  - Tests across Grpc.Tools versions, since the property is a Grpc.Tools internal and could change.

### Compared with upstream

The Tier A changes could instead be PRs to protoc's C# generator, and the Tier B changes can only land upstream. The
upstream section above describes how each project takes such PRs. Choosing between the routes is the decision ADR's
job, not this report's.

## Summary grid

**For review.** Allocated bytes per operation, net10 (net11 is the same). The rows are representative samples; the
committed exports hold every sample. Current bytes come from `MessageBenchmarks`: Parse is `ParseSpan`, which is
the parse the gRPC marshaller does. Write is `WriteToSpan`. Clone is `Clone`. The savings are the Findings'
**Avoidable** bytes for that sample, with the finding IDs. "—" means none.

| Shape (sample) | Parse | Write | Clone | Tier A saving | Tier B saving |
|---|---|---|---|---|---|
| `Scalars/populated` | 104 | 0 | 104 | — | — |
| `Strings/populated` | 33,496 | 0 | 56 | — | — (M-07 is inherent. Segmented output adds up to 16,408 B: M-10.) |
| `Bytes/populated` | 16,552 | 0 | 40 | — | — (M-11 is inherent) |
| `Repeated/populated` | 8,856 | 0 | 4,984 | Clone 192 (M-25) | Parse 3,328 (M-16); Clone 864 (M-17) |
| `Repeated/empty` | 264 | 0 | 456 | Parse 192, Clone 384 (M-24, M-25) | — |
| `Maps/populated` | 24,552 | 168 | 19,624 | Clone 456 (M-25) | Parse ≈ 6.6 KB, sampled (M-03); Write 168 (M-05) |
| `Maps/empty` | 504 | 0 | 960 | Parse 456, Clone 912 (M-04, M-25) | — |
| `Node/populated-depth3` | 264 | 0 | 144 | Write CPU (M-18; about 60 % at depth 8) | — |
| `Oneof/populated-int32` | 64 | 0 | 64 | Parse 24, Clone 24, Merge 24 (M-01, M-02) | — |
| `Oneof/populated-message` | 120 | 0 | 80 | 40 per merge into the set member (M-14) | — |
| `Optional/edge-defaults-present` | 128 | 0 | 72 | — | Parse 56 (M-12) |
| `Wrappers/populated` | 216 | 0 | 88 | — | — |

The same ideas in plain terms:
- **Off the gRPC path, any shape** (Tier B):
  - `ParseFrom(Stream)` +4,288 B (M-21);
  - `WriteTo(Stream)` +4,184 B (M-23);
  - `ParseFrom(byte[])` +168 B (M-20);
  - `ToByteArray` +64 B (M-22);
  - long strings and bytes from a `Stream` (M-08).
- **Unknown fields** (Tier B): 1,056 B per parse and 536 B per Clone, only when they are present (M-26).
- **A whole call:** `EndToEndBenchmarks.Unary(Messages: 1)` is 14,240 B. About 208 B of it is the generated
  marshaller (two `Scalars` messages), and the rest is grpc-dotnet and the HTTP stack (Tier B, out of a generator's
  reach).
- **Checking the grid:** its Parse, Write and Clone columns were generated from
  `results/net10.0/GrpcSpike.Benchmarks.MessageBenchmarks-report-full.json` with:

  ```sh
  jq -r '.Benchmarks[] | select(.FullName | test("\\.(ParseSpan|WriteToSpan|Clone)\\(Sample: (Scalars/populated|Strings/populated|Bytes/populated|Repeated/(populated|empty)|Maps/(populated|empty)|Node/populated-depth3|Oneof/populated-(int32|message)|Optional/edge-defaults-present|Wrappers/populated)\\)")) | "\(.FullName) \(.Memory.BytesAllocatedPerOperation)"' docs/analysis/results/net10.0/GrpcSpike.Benchmarks.MessageBenchmarks-report-full.json
  ```

  `tests/Corpus.Tests/AllocationTests.cs` pins the savings' bytes.
