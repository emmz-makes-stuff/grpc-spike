```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4

Job=ShortRun  Runtime=.NET 10.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method          | Messages | Mean     | Error      | StdDev   | Gen0   | Allocated |
|---------------- |--------- |---------:|-----------:|---------:|-------:|----------:|
| **Unary**           | **1**        | **131.6 μs** |   **580.4 μs** | **31.81 μs** | **0.4883** |  **13.91 KB** |
| ServerStreaming | 1        | 149.4 μs |   565.2 μs | 30.98 μs | 0.4883 |  14.39 KB |
| ClientStreaming | 1        | 170.1 μs |   787.0 μs | 43.14 μs | 0.4883 |   15.2 KB |
| Bidi            | 1        | 232.2 μs | 1,286.7 μs | 70.53 μs | 0.4883 |   17.6 KB |
| **Unary**           | **10**       | **143.6 μs** |   **350.0 μs** | **19.19 μs** | **0.4883** |  **14.03 KB** |
| ServerStreaming | 10       | 129.7 μs |   200.1 μs | 10.97 μs | 0.4883 |  17.04 KB |
| ClientStreaming | 10       | 138.4 μs |   548.4 μs | 30.06 μs | 0.4883 |  15.91 KB |
| Bidi            | 10       | 270.6 μs |   204.1 μs | 11.19 μs | 0.9766 |  30.48 KB |
