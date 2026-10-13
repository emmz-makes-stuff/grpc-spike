```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 11.0.0 (11.0.0-rc.1.26425.128, 11.0.26.42628), X64 RyuJIT x86-64-v4

Job=ShortRun  Toolchain=.NET 11.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method          | Messages | Mean     | Error      | StdDev    | Median   | Gen0   | Allocated |
|---------------- |--------- |---------:|-----------:|----------:|---------:|-------:|----------:|
| **Unary**           | **1**        | **189.6 μs** |   **895.4 μs** |  **49.08 μs** | **181.5 μs** | **0.4883** |  **15.25 KB** |
| ServerStreaming | 1        | 358.1 μs | 6,143.9 μs | 336.77 μs | 189.3 μs | 0.4883 |  15.67 KB |
| ClientStreaming | 1        | 246.4 μs | 1,360.5 μs |  74.57 μs | 215.9 μs | 0.4883 |  17.53 KB |
| Bidi            | 1        | 234.2 μs | 1,136.2 μs |  62.28 μs | 227.5 μs | 0.9766 |  18.74 KB |
| **Unary**           | **10**       | **176.2 μs** |   **513.0 μs** |  **28.12 μs** | **171.2 μs** | **0.4883** |   **15.2 KB** |
| ServerStreaming | 10       | 154.2 μs |   571.4 μs |  31.32 μs | 152.5 μs | 0.9766 |  18.13 KB |
| ClientStreaming | 10       | 179.5 μs |   935.5 μs |  51.28 μs | 161.2 μs | 0.9766 |  18.73 KB |
| Bidi            | 10       | 339.4 μs | 1,098.6 μs |  60.22 μs | 344.1 μs | 0.9766 |  31.72 KB |
