# Environment: net11.0

Timings are specific to this machine (Intel(R) Xeon(R) Platinum 8259CL CPU @ 2.50GHz, X64, Ubuntu 26.04 LTS) and indicative only.
Allocated bytes do not depend on the machine and are the primary evidence (design D8).
Hardware, OS and runtime details for each run are in the `*-report-github.md` headers and the
`HostEnvironmentInfo` of each `*-report-full.json` (which describes the net10.0 host process, not the runtime measured).

`ReferenceBenchmarks` rows (category `Reference`) are protobuf-net: reference-only,
never part of the go/no-go criteria (ADR-0002, design D8).

- Runtime under test: `net11.0`
- Runtime as measured: .NET 11.0.0 (11.0.0-rc.1.26425.128, 11.0.26.42628), X64 RyuJIT x86-64-v4
- Machine: Intel(R) Xeon(R) Platinum 8259CL CPU @ 2.50GHz, X64, Ubuntu 26.04 LTS

## Pinned packages (Directory.Packages.props)

| Package | Version |
|---|---|
| BenchmarkDotNet | 0.15.8 |
| Google.Protobuf | 3.36.2 |
| Grpc.AspNetCore | 2.84.0 |
| Grpc.Net.Client | 2.84.0 |
| Grpc.Tools | 2.84.0 |
| Microsoft.AspNetCore.TestHost | 10.0.12 |
| protobuf-net | 3.4.30 |
| xunit.v3 | 4.0.2 |

## Subject assemblies as loaded

| Assembly | Informational version |
|---|---|
| Google.Protobuf | 3.36.2+2c74169b34066ceb8ddb6b882fcb3fb32d737a55 |
| Grpc.Core.Api | 2.84.0+4c6997a214601422dd66c5c74c1969db749479e9 |
| Grpc.Net.Client | 2.84.0+4c6997a214601422dd66c5c74c1969db749479e9 |
