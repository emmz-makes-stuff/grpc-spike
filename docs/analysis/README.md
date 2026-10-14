# Analysis: how the numbers are made

`report.md` holds the findings. This page says how to reproduce the numbers behind them.

## Benchmark suite (`bench/Benchmarks`)

| Class | What it measures |
|---|---|
| `MessageBenchmarks` | Every corpus sample (`Samples.All`) × `New`, `ParseByteArray`, `ParseSpan`, `ToByteArray`, `WriteToSpan`, `WriteToSegmentedWriter` (32-byte segments, report M-10), `CalculateSize`, `Clone`, `MergeIntoNew`, `MergeIntoPopulated`. The merge rows include a `New` or `Clone`; subtract that row for the merge alone. |
| `StreamAndMergeBenchmarks` | Every corpus sample × `ParseStream`, `WriteToStream` (both through a reused `MemoryStream`) and `MergeBytesIntoPopulated` (parse the sample's bytes into a clone of itself). These are the code-reading sites off the gRPC path (report M-08, M-14, M-21, M-23). The merge row includes a `Clone`; subtract `MessageBenchmarks`' `Clone` row for the merge alone. |
| `UnknownFieldBenchmarks` | The v1 record reading v2 bytes, then re-serialising and cloning what it kept (M-26/M-27). |
| `MarshallerBenchmarks` | The generated `Marshaller<Scalars>` alone, through minimal reusable serialization contexts. |
| `EndToEndBenchmarks` | Whole in-process calls through ASP.NET Core TestHost: unary and the three streaming kinds, 1 and 10 messages per stream (`Unary` ignores `Messages`). Same message as `MarshallerBenchmarks`. Singleton service, `UnaryAsync`, no compression. |
| `ReferenceBenchmarks` | protobuf-net on the same data, for comparable shapes. Category `Reference`: **reference-only, never part of go/no-go** (ADR-0002, design D8). |

Every job is BenchmarkDotNet's ShortRun with `MemoryDiagnoser`. Allocated bytes per operation are exact. Timings
are indicative and specific to the machine they ran on; each `environment.md` names it.

## Running

```sh
make bench                                   # everything, net10.0 then net11.0 (hours: needs a quiet machine)
make bench FILTER='*MessageBenchmarks*'      # one class; FILTER takes BenchmarkDotNet globs
make bench BENCH_TARGETS=net11.0             # one runtime
make bench ARGS='--iterationCount 1'         # other BenchmarkDotNet arguments
```

Each run ends with `BENCH_EXIT:<n>`. It is non-zero if no benchmark ran or any failed. The host process is always
net10.0. BenchmarkDotNet 0.15.8 has no .NET 11 moniker, so net11.0 runs through a custom csproj toolchain.

## Results (`results/<runtime>/`)

- `*-report-github.md`: the human-readable table. The header gives BenchmarkDotNet, OS, CPU, SDK and the runtime
  that was measured (the `Job` line).
- `*-report-full.json`: machine-readable, with every measurement. Its `HostEnvironmentInfo` describes the net10.0
  **host** process, even in `net11.0/`.
- `environment.md`: the runtime that was measured, the host OS, every pin from `Directory.Packages.props`, and the
  subject assemblies' versions as loaded.

A run copies only the classes it ran, so a filtered run refreshes those files and leaves the rest. It always rewrites
`environment.md`, though. So commit a filtered run's results only if it ran on the same machine as the rest.

## Allocation attribution

To find which types a benchmark allocates, and where:

```sh
make attribute FILTER='*UnknownFieldBenchmarks.ParseV2AsV1*'          # net10.0
make attribute FILTER='*MessageBenchmarks.Clone*' BENCH_TARGET=net11.0
```

This adds BenchmarkDotNet's `EventPipeProfiler` with the GcVerbose profile, which records one `.nettrace` (plus
`.speedscope.json`) per benchmark under `BenchmarkDotNet.Artifacts/<runtime>/`. Then, for each trace, it writes
`<trace>.summary.md` beside it and prints it. The summary has two tables:

- **Top allocated types**, with sampled bytes and their share.
- **Top allocation sites**: type, allocating method (the innermost managed frame), and caller (the first frame
  outside `System.*`, `Microsoft.*` and the harness), with sampled bytes and their share.

`make attribute` refuses to run without a `FILTER`, since the suite would produce hundreds of traces. A filter
matches benchmark methods, not parameters, so `*MessageBenchmarks.Clone*` traces `Clone` for every sample.

Reading a summary:

- The shares come from sampled `GCAllocationTick` events, about one per 100 KB. They are estimates; use the
  benchmark's `Allocated` column for exact bytes per operation.
- The trace covers the whole benchmark process, harness included. Workload types dominate, and harness types show
  up near 0 %.
- JIT inlining folds frames. An allocation inside an inlined method, such as `CodedInputStream` in `ParseFrom`, is
  credited to its caller. To see the frames, add `ARGS='--envVars DOTNET_JitNoInline:1'`. But .NET 10 can
  stack-allocate objects that don't escape, and turning inlining off can change what is allocated. Check the
  `Allocated` column against a normal run before relying on such a trace.
- Attributed runs carry profiler overhead, so their exports are **not** copied to `results/`. Traces are not
  committed (`.gitignore`). Only the summarised attribution goes into `report.md`.
