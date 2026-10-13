```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 11.0.0 (11.0.0-rc.1.26425.128, 11.0.26.42628), X64 RyuJIT x86-64-v4

Job=ShortRun  Toolchain=.NET 11.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method                  | Mean     | Error     | StdDev  | Gen0   | Allocated |
|------------------------ |---------:|----------:|--------:|-------:|----------:|
| ParseV2AsV1             | 679.0 ns |  45.87 ns | 2.51 ns | 0.0734 |    1376 B |
| ReserializeUnknownsAsV1 | 176.1 ns |  26.15 ns | 1.43 ns | 0.0072 |     136 B |
| CloneWithUnknowns       | 305.9 ns | 120.25 ns | 6.59 ns | 0.0319 |     600 B |
