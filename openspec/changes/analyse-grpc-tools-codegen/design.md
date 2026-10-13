# Design

## Context

The repo is greenfield: it holds only `openspec/` and `docs/`. See proposal.md for motivation, and the
specs for the required behaviour. Foundational calls are recorded in
`docs/adrs/ADR-0001-dotnet-and-pinned-subject-versions.md` and `docs/adrs/ADR-0002-evidence-method.md`.
This design binds them to concrete structure.

These facts were checked on 2026-10-13:

- **Latest stable on NuGet:**
  - `Grpc.Tools` 2.84.0;
  - `Google.Protobuf` 3.36.2;
  - `Grpc.AspNetCore` and `Grpc.Net.Client` 2.84.0;
  - BenchmarkDotNet 0.15.8;
  - protobuf-net 3.4.30;
  - xunit.v3 4.0.2;
  - `Microsoft.AspNetCore.TestHost` 10.0.12.
- **Unlisted on NuGet:** `Google.Protobuf` 4.0.0-rc1/rc2 (placeholders, ignored).
- **Grpc.Tools on macOS:** 2.83.0 ships only `macosx_x64` binaries. Its protoc (libprotoc 35.1) runs under
  Rosetta on this Mac.
- **Installed SDKs:** 10.0.400 and 11.0.100-rc.1.

## Goals / Non-Goals

**Goals:**
- Keep one solution and one set of gates, and keep the benchmarks out of the gates.
- Make every number reproducible from committed inputs: corpus, pinned versions and generated code.
- Report allocations exactly; report timing as indicative only.

**Non-Goals:**
- Writing any code generator, prototype or patch.
- Kestrel over a real network, or Linux/x64 timing runs. These can be added later.
- .NET Framework or netstandard consumers.

## Decisions

### D1: Runtime and language
The repo uses C# on .NET, with `net10.0` as the primary target framework. The benchmark project
multi-targets `net10.0;net11.0`.

- **SDK:** `global.json` pins `11.0.100-rc.1.26425.128` with `rollForward: latestFeature` and
  `allowPrerelease: true`. Re-pin at GA.
- **Why:** the subject code is C#. The audience is open source, so LTS-first. .NET 11 shows whether the
  runtime alone shrinks the gap.
- **Rejected:**
  - .NET 8: its support ends in November 2026.
  - .NET 11 as primary: still RC.
  - A separate SDK for the benchmarks: means two builds.

### D2: Pinned subject versions
Central package management is used: `Directory.Packages.props` with
`ManagePackageVersionsCentrally=true`. Exact versions: `Grpc.Tools` 2.84.0, `Google.Protobuf` 3.36.2,
`Grpc.AspNetCore` 2.84.0, `Grpc.Net.Client` 2.84.0.

- **Why:** findings must cite current code and stay reproducible.
- **Rejected:** floating versions; the unlisted 4.0 RCs.

### D3: Measurement
BenchmarkDotNet 0.15.8 with `MemoryDiagnoser`. Allocation attribution uses
`EventPipeProfiler(EventPipeProfile.GcVerbose)`, which works across platforms and writes speedscope/nettrace
output. It's enabled per run with a command-line switch, not on every run.

- **Why:** we need allocated bytes per operation, plus the call sites that allocate them, on macOS.
- **Rejected:** stopwatch harnesses; Windows-only ETW profilers; `dotnet-trace` by hand (kept as a
  fallback only).

### D4: Corpus and committed generated code
Project layout:

```
GrpcSpike.slnx
  src/Corpus/            protos/*.proto (one shape per file) + Generated/ (committed)
  tests/Corpus.Tests/    xUnit v3 round-trip + allocation-count tests
  bench/Benchmarks/      BenchmarkDotNet, net10.0;net11.0
  bench/Reference/       protobuf-net equivalents of comparable shapes
docs/analysis/           report + results/ (committed BDN exports)
```

- `Corpus` uses `<Protobuf Include="protos/**/*.proto" GrpcServices="Both" OutputDir="Generated/%(RelativeDir)" CompileOutputs="true" />`.
  The generated files are written into a tracked folder and compiled from there.
- The build regenerates the code on every run. The spec's "generated code is current" scenario is
  checked by `git diff --exit-code -- src/Corpus/Generated` after a build. This is wrapped as a
  `make` check alongside the gates.
- **Why:** findings must be citable by file and line, and version bumps must show up as diffs.
- **Rejected:** reading the generated code from `obj/`.

### D5: Measuring the stub layer
- **Marshaller level:** benchmarks call the generated `Marshaller<T>` contextual serializer and
  deserializer through a minimal test `SerializationContext`/`DeserializationContext`.
- **End-to-end:** ASP.NET Core `TestHost` (10.0.12) hosting the corpus services, called through
  `Grpc.Net.Client` with the test server's `HttpMessageHandler`. Covers unary and all three streaming kinds.
- **Why:** this tests the hypothesis that the stub layer is thin, by showing its share of the
  whole-call cost.
- **Rejected:** Kestrel over loopback for now. It adds network-stack noise not caused by the generated code.

### D6: Correctness tests
xUnit v3 on Microsoft.Testing.Platform. `global.json` has `"test": { "runner": "Microsoft.Testing.Platform" }`.

- **Round-trip tests:** run per shape, using populated, empty and edge-case values, plus unknown-field
  preservation.
- **Allocation-count tests:** for confirmed findings, using `GC.GetAllocatedBytesForCurrentThread()` around
  a warmed operation. They assert the finding's documented cost, so a version bump that changes it fails
  loudly.
- **Rejected:** MSTest and TUnit. Neither makes a meaningful difference here.

### D7: Outputs
- **Report:** `docs/analysis/report.md` holds the findings table, with one row per finding:
  - tier (A/B/C);
  - hot-path or one-off;
  - source citation;
  - benchmark citation;
  - hypothesis status.

  It also holds the prior-art and upstream survey, and the drop-in assessment at both compatibility
  levels.
- **Exports:** BenchmarkDotNet exports (GitHub Markdown + JSON) are written to
  `docs/analysis/results/<runtime>/` with the environment summary.
- **Decision:** recorded in `docs/adrs/ADR-0003-go-no-go.md` after Emmz reviews the numbers.
- **Rejected:** a database or dashboard.

### D8: Evidence order
Allocations are the primary evidence, because they don't depend on the machine. Timings are secondary and
labelled "Apple Silicon (arm64), macOS". protobuf-net rows are labelled `Reference` and never enter go/no-go.

### Finding taxonomy
- **Tier A:** generator only.
- **Tier B:** runtime internals.
- **Tier C:** public API.

The four-way outcome comes from the per-finding route:

```
worth it? --no--> discard
   |yes
tier A/B --> upstream likely to land? --yes--> go upstream
   |                                 --no---> go separate tool
tier C ----> record only
mixed routes across findings ------------------> mix
```

### Gate commands (stack: dotnet, single stack, no prefix)
```
build:  dotnet build GrpcSpike.slnx -c Release
test:   dotnet test --solution GrpcSpike.slnx -c Release
format: dotnet format GrpcSpike.slnx --verify-no-changes
lint:   none (analyzers run in build; TreatWarningsAsErrors=true in Directory.Build.props)
bench:  not a gate; dotnet run -c Release --project bench/Benchmarks -- --filter '*' (wrapped as `make bench`)
```

All gates run non-interactively and exit non-zero on failure.

- `Generated/` is excluded from formatting and analyzers. The `.editorconfig` section marks it
  `generated_code = true`.
- `<Protobuf>` output also carries `// <auto-generated>` headers, which analyzers already skip.

## Risks / Trade-offs

- **[Prerelease SDK for all gates until .NET 11 GA]** → The SDK is pinned to exact RC1. Re-pin to 11.0.100
  at GA (November 2026). If an RC bug blocks a gate, fall back to a 10.0.400 `global.json` and drop
  `net11.0` temporarily.
- **[BenchmarkDotNet 0.15.8 may lack a .NET 11 runtime moniker]** → Configure a custom
  `CsProjCoreToolchain` with `NetCoreAppSettings("net11.0", ...)`, or run per framework with
  `--runtimes`. This gets verified in the benchmark scaffolding task.
- **[Grpc.Tools has no arm64 protoc on macOS]** → Rosetta 2 is a logged dependency. It only affects code
  generation, not the measured code.
- **[Committed generated code can drift]** → The build always regenerates, and the `git diff --exit-code`
  check catches drift.
- **[Timings measured on one Apple Silicon machine]** → Timings are labelled and kept secondary. Allocation
  counts carry the decision.
- **[Attribution traces are large]** → Traces are generated on demand and not committed. Only the
  summarised attribution goes into the report.
- **[Hypotheses based on memory of older code]** → They're treated as questions. The report must confirm
  or reject each one with evidence.
