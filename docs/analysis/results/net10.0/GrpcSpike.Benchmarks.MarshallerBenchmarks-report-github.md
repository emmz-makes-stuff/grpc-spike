```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4

Job=ShortRun  Runtime=.NET 10.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method      | Mean     | Error    | StdDev  | Gen0   | Allocated |
|------------ |---------:|---------:|--------:|-------:|----------:|
| Serialize   | 127.3 ns | 28.40 ns | 1.56 ns |      - |         - |
| Deserialize | 186.1 ns | 70.01 ns | 3.84 ns | 0.0055 |     104 B |
