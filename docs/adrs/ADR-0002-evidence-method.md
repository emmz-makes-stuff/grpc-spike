# ADR-0002: Evidence method: committed generated code, attributed allocations, allocations first

- **Status:** Accepted
- **Date:** 2026-10-13

## Context

The go/no-go decision rests on code reading to find allocation sites, then measurements. No numeric
threshold is fixed: the decision is made after seeing the numbers. So the numbers, and the code they
describe, must live in the repo and be reproducible.

## Decision

- **Shape corpus:** one `.proto` per message shape, covering:
  - scalars, strings and bytes;
  - repeated (packed and unpacked) and maps;
  - nesting depth;
  - oneofs, wrapper types, `optional`, and unknown fields;
  - unary and streaming services.

  The corpus is compiled by the real, pinned `Grpc.Tools`.
- **Generated code is committed** (Grpc.Tools `OutputDir` points at a tracked folder), so findings
  can cite file:line and version bumps show up as diffs. Generated code is excluded from formatting
  and analyzers.
- **Measurement:** BenchmarkDotNet with its memory diagnostics (allocated bytes and GC counts per
  operation), plus its cross-platform EventPipe profiler (GC-verbose) to attribute allocations to call
  sites.
  - Message work is benchmarked per shape: parse, serialize, size, merge and clone.
  - Stub work is benchmarked at the marshaller level and end-to-end in-process via ASP.NET Core
    TestHost, to see the stub layer's cost next to the whole call.
- **Allocations are the primary evidence, timing secondary.** Allocations are machine-independent, but
  timings are labelled as this machine's: Apple Silicon, macOS.
- **protobuf-net** (3.4.30) is a **reference row only.** It is not drop-in and never counts toward go/no-go.
- **Correctness:** xUnit v3 round-trip tests for every corpus message, plus allocation-count assertions
  for key findings. A benchmark number only counts if its code is shown to be correct.
- **Outputs:** a Markdown analysis report under `docs/analysis/`, BenchmarkDotNet exported results
  committed alongside it, and the final go/no-go recorded as a later ADR. No database.

## Alternatives rejected

- **Reading generated code from `obj/`:** not citable, and it doesn't diff across versions.
- **Stopwatch or ad-hoc timing:** gives no allocation attribution, and it's noisy.
- **Windows-only ETW profilers:** benchmarks run on macOS.
- **Kestrel over loopback for end-to-end:** more realistic HTTP/2, but noisier. It can be added later.
- **A timing-led evaluation:** the stated problem is allocations.
- **MSTest, TUnit:** no meaningful difference for this use.
