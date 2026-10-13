```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4

Job=ShortRun  Runtime=.NET 10.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method                  | Mean     | Error    | StdDev   | Gen0   | Allocated |
|------------------------ |---------:|---------:|---------:|-------:|----------:|
| ParseV2AsV1             | 731.5 ns | 572.8 ns | 31.40 ns | 0.0734 |    1376 B |
| ReserializeUnknownsAsV1 | 209.4 ns | 122.1 ns |  6.69 ns | 0.0072 |     136 B |
| CloneWithUnknowns       | 326.3 ns | 180.3 ns |  9.88 ns | 0.0319 |     600 B |
