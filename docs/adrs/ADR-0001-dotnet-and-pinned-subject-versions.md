# ADR-0001: .NET 10 primary runtime, .NET 11 comparison, pinned subject versions

- **Status:** Accepted
- **Date:** 2026-10-13

## Context

The spike analyses the C# code that `Grpc.Tools` generates (protoc's C# message generator and
`grpc_csharp_plugin`) and the runtimes it calls into, to decide whether a more allocation-efficient,
drop-in replacement, or an upstream improvement, is worth pursuing. The analysis must point at concrete,
current code, not folklore about older versions. The audience is open source, so the runtime
should match what most consumers run.

## Decision

- **Language/platform:** C# on .NET. Primary target framework `net10.0` (current LTS). Benchmarks also
  run on `net11.0` as a comparison, to show whether runtime/JIT improvements alone shrink the gap.
- **SDK:** the repo pins the .NET 11 SDK (RC1, `allowPrerelease: true`) in `global.json`, since one
  solution cannot target `net11.0` under a .NET 10 SDK. Re-pin to 11.0.100 at GA (November 2026).
- **Subject versions are pinned exactly** via central package management (`Directory.Packages.props`):
  - `Grpc.Tools` 2.84.0
  - `Google.Protobuf` 3.36.2
  - `Grpc.AspNetCore` 2.84.0, `Grpc.Net.Client` 2.84.0
  
  These were the latest stable on NuGet on 2026-10-13. `Google.Protobuf` 4.0.0-rc1/rc2 are unlisted
  placeholders and are ignored; the C# runtime remains on major version 3.
- Changing a pinned subject version is a deliberate act. The committed generated code then shows the
  difference as a diff.

## Alternatives rejected

- **.NET 8:** its LTS support ends in November 2026.
- **.NET 11 as primary:** still RC. It's kept as a comparison only.
- **.NET Framework / netstandard targets:** out of scope for the go/no-go question.
- **Floating versions:** make findings unreproducible and line citations rot.
- **A separate .NET 11 benchmark project on its own `global.json`:** keeps gates on a stable SDK but
  doubles builds. The prerelease-SDK risk is short-lived, because the SDK is backward compatible for `net10.0`.

## Consequences

- Every gate depends on a prerelease SDK until GA. That is a known and accepted risk.
- On Apple Silicon, `Grpc.Tools` runs its x64 protoc under Rosetta 2. That only affects code generation,
  not the measured code.
- BenchmarkDotNet 0.15.8 may lack a built-in .NET 11 runtime moniker. If so, a custom toolchain config is
  needed.
