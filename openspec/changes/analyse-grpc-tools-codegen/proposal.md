# Proposal

## Why

The C# code that `Grpc.Tools` generates for Protobuf messages and gRPC services is believed to make more
heap allocations than necessary. Before investing in an alternative generator, we need evidence on several
points:

- where those allocations actually come from;
- how large they are, and on which message shapes;
- whether they can be removed without breaking the public API that existing user code compiles against.

The answer decides whether this idea is worth pursuing at all, and if so, how: as upstream pull requests
or as a separate tool.

## What Changes

- A **shape corpus**: one `.proto` per message or service shape, covering:
  - scalars, strings and bytes;
  - repeated (packed and unpacked) and maps;
  - nesting depth;
  - oneofs, wrapper types, `optional`, and unknown fields;
  - unary and streaming services.

  The corpus is compiled by exact pinned versions of `Grpc.Tools` and `Google.Protobuf`. The generated
  C# is committed so findings can cite it by file and line.
- **Correctness tests** proving every corpus message round-trips, so benchmark numbers describe correct
  code.
- An **allocation and throughput benchmark suite**:
  - per shape: parse, serialize, size calculation, merge and clone;
  - for stubs: at the marshaller level and end-to-end in-process;
  - on .NET 10, with .NET 11 as a comparison, and with allocations attributed to call sites;
  - protobuf-net as a non-counting reference row.
- An **analysis report** covering:
  - each finding classified by what a fix would require: generator-only (Tier A), a runtime change
    (Tier B), or a public API change (Tier C);
  - whether each finding is on the hot path or a one-off;
  - a prior-art survey that includes how upstream receives contributions.
- A **go/no-go decision**, recorded as an ADR, with one of four outcomes:
  - go upstream;
  - go separate tool;
  - a mix of the two;
  - no-go.

Not in this change: building any generator, or opening upstream PRs.

## Capabilities

### New Capabilities

- `codegen-shape-corpus`: the pinned, reproducible set of `.proto` shapes, their committed generated C#,
  and the round-trip correctness guarantees over them.
- `allocation-benchmarks`: repeatable measurement of allocations and time for generated message and stub
  code, attributed to call sites, across the supported runtimes, with exported results.
- `codegen-analysis-report`: the written analysis (tiered findings, hot-path versus one-off, prior-art
  and upstream survey) and the recorded go/no-go decision.

### Modified Capabilities

None. This is a greenfield repo with no existing specs.

## Impact

- **New repo content:**
  - a .NET solution (`GrpcSpike.slnx`) with corpus, test and benchmark projects;
  - `global.json`, `Directory.Build.props` and `Directory.Packages.props`;
  - committed generated code;
  - `docs/analysis/`;
  - a decision ADR.
- **Dependencies:** see `docs/dependencies.md`. These include the .NET 11 RC1 SDK (building net10.0 and
  net11.0) and Rosetta 2 for `Grpc.Tools`' x64-only protoc on Apple Silicon.
- **Binding decisions:** `docs/adrs/ADR-0001-dotnet-and-pinned-subject-versions.md` and
  `docs/adrs/ADR-0002-evidence-method.md`.
- No external systems are changed. Publishing to a public GitHub repo is handled outside this change.
