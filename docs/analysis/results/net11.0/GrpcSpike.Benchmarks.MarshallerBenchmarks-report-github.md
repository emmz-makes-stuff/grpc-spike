```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 11.0.0 (11.0.0-rc.1.26425.128, 11.0.26.42628), X64 RyuJIT x86-64-v4

Job=ShortRun  Toolchain=.NET 11.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method      | Mean     | Error    | StdDev  | Gen0   | Allocated |
|------------ |---------:|---------:|--------:|-------:|----------:|
| Serialize   | 132.4 ns | 14.73 ns | 0.81 ns |      - |         - |
| Deserialize | 183.9 ns | 38.56 ns | 2.11 ns | 0.0055 |     104 B |
