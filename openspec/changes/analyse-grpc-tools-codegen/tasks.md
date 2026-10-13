# Tasks

## 1. Solution scaffolding

- [x] 1.1 Add `global.json` pinning SDK `11.0.100-rc.1.26425.128` (`rollForward: latestFeature`, `allowPrerelease: true`, test runner Microsoft.Testing.Platform). Verify: `dotnet --version` in the repo prints the pinned SDK.
- [x] 1.2 Add `Directory.Build.props` (`TreatWarningsAsErrors`, nullable, `LangVersion` latest) and `Directory.Packages.props` with exact pins: Grpc.Tools 2.84.0, Google.Protobuf 3.36.2, Grpc.AspNetCore 2.84.0, Grpc.Net.Client 2.84.0, BenchmarkDotNet 0.15.8, protobuf-net 3.4.30, xunit.v3 4.0.2, Microsoft.AspNetCore.TestHost 10.0.12. Verify: no project file carries a `Version=` attribute.
- [x] 1.3 Create `GrpcSpike.slnx` with empty `src/Corpus`, `tests/Corpus.Tests`, `bench/Benchmarks` (net10.0;net11.0) and `bench/Reference` projects, plus `.editorconfig` marking `src/Corpus/Generated/**` as `generated_code = true`. Verify: build, test and format gates all exit 0.

## 2. Shape corpus and committed generated code

- [x] 2.1 Write the scalar, string and bytes shape protos, and wire `<Protobuf ... OutputDir="Generated/%(RelativeDir)" GrpcServices="Both">`. Verify: build exits 0 and generated `.cs` appears under `src/Corpus/Generated/`.
- [x] 2.2 Add protos for repeated (packed and unpacked), maps, nesting depths (1, 3, 8), oneofs (with scalar, string and message members), wrapper types, proto3 `optional`, and an "unknown fields" pair (v2 writer, v1 reader). Verify: build exits 0.
- [x] 2.3 Add a service proto with unary, server-streaming, client-streaming and bidi methods over corpus messages. Verify: generated `*Grpc.cs` contains client and server base types for all four.
- [x] 2.4 Document the shape-to-definition mapping in `src/Corpus/README.md`. Verify: every shape in the `codegen-shape-corpus` spec has a row.
- [x] 2.5 Add round-trip tests (populated, empty and edge values) for every corpus message, and an unknown-field preservation test. Verify: test gate exits 0, and deliberately corrupting one equality check makes it exit non-zero.
- [x] 2.6 Add the generated-code drift check (`git diff --exit-code -- src/Corpus/Generated` after a build). Verify: it exits 0 on a clean build, and non-zero after hand-editing a generated file.

## 3. Code reading: allocation sites

- [x] 3.1 Read the generated message code and the Google.Protobuf 3.36.2 runtime paths it calls (parse/merge, WriteTo, CalculateSize, Clone, collections, ByteString, strings, UnknownFieldSet). Record candidate allocation sites in `docs/analysis/report.md` with file:line citations and a provisional tier (A/B/C) and hot-path or one-off label. Verify: every hypothesis in the `codegen-analysis-report` spec has at least one cited entry, or a note saying no site was found.
- [x] 3.2 Read the generated stub and marshaller code, and the Grpc.Net.Client / Grpc.AspNetCore call paths it hands off to. Record candidate sites in the same way. Verify: the report separates sites in generated code from sites in the runtime.

## 4. Benchmarks

- [ ] 4.1 Set up BenchmarkDotNet with `MemoryDiagnoser`, both runtimes (custom `net11.0` toolchain if 0.15.8 lacks a moniker), and exporters (GitHub Markdown + JSON) writing to `docs/analysis/results/<runtime>/`. Verify: a trivial benchmark runs on both runtimes and the export records hardware, OS, runtime and subject versions.
- [ ] 4.2 Add per-shape message benchmarks for parse, serialize, CalculateSize, MergeFrom and Clone. Verify: a full run produces allocated bytes, GC counts and mean time for every shape and operation pair.
- [ ] 4.3 Add marshaller-level stub benchmarks using minimal test serialization/deserialization contexts. Verify: results report allocations for marshalling only.
- [ ] 4.4 Add TestHost end-to-end benchmarks (unary and the three streaming kinds) using the same messages as 4.3. Verify: results show whole-call allocations next to the marshaller-only figures.
- [ ] 4.5 Add protobuf-net reference equivalents for comparable shapes, labelled `Reference`. Verify: exports show the reference rows marked as reference-only.
- [ ] 4.6 Add the on-demand attribution switch (`EventPipeProfiler` GcVerbose), and document how to run it and summarise a trace in `docs/analysis/README.md`. Verify: an attributed run for one benchmark names the allocated types and the methods allocating them.
- [ ] 4.7 Run the full suite on both runtimes and commit the exports. Verify: `docs/analysis/results/net10.0/` and `.../net11.0/` contain results for every benchmark.

## 5. Analysis report

- [ ] 5.1 Confirm or reject each hypothesis with attribution traces and benchmark numbers, and finalise each finding's tier, hot-path or one-off label, and both citations. Verify: every finding row cites both a source location and a committed result.
- [ ] 5.2 Add allocation-count tests asserting the measured cost of each confirmed finding. Verify: test gate exits 0.
- [ ] 5.3 Write the prior-art survey (alternative generators and serializers, with drop-in fit) and the upstream survey (contribution receptiveness, plus linked issues and PRs on each finding). Verify: each confirmed finding states any related upstream history, or "none found".
- [ ] 5.4 Write the separate-tool assessment at both compatibility levels: API only, and API plus build integration. Verify: both levels state the available savings and the rough effort.
- [ ] 5.5 Write the summary grid (shape × operation, current allocations, and the Tier A/B savings) for Emmz's review. Verify: the grid is generated from or checked against the committed exports.

## 6. Decision

- [ ] 6.1 Emmz reviews the report and chooses one outcome: go upstream / go separate tool / mix / no-go. Record it in `docs/adrs/ADR-0003-go-no-go.md` with reasons citing findings. Verify: the ADR names exactly one outcome and its citations resolve to report rows.

## Workflow follow-up

- Archive the change after review.
- If the outcome is a go, propose follow-up change(s) for the upstream PRs or the separate tool.
