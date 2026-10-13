```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 11.0.0 (11.0.0-rc.1.26425.128, 11.0.26.42628), X64 RyuJIT x86-64-v4

Job=ShortRun  Toolchain=.NET 11.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  Categories=Reference  

```
| Method              | Sample               | Mean           | Error           | StdDev        | Median         | Gen0     | Gen1     | Gen2     | Allocated |
|-------------------- |--------------------- |---------------:|----------------:|--------------:|---------------:|---------:|---------:|---------:|----------:|
| **ParseSpan**           | **Bytes/edge**           |   **768,844.0 ns** |   **591,249.20 ns** |  **32,408.36 ns** |   **785,570.9 ns** |  **54.6875** |  **54.6875** |  **54.6875** | **1048903 B** |
| WriteToBufferWriter | Bytes/edge           |    56,793.1 ns |    19,687.09 ns |   1,079.12 ns |    56,468.3 ns |        - |        - |        - |         - |
| CalculateSize       | Bytes/edge           |       131.4 ns |        20.66 ns |       1.13 ns |       131.6 ns |        - |        - |        - |         - |
| Clone               | Bytes/edge           | 1,596,188.1 ns | 1,282,050.12 ns |  70,273.49 ns | 1,560,507.7 ns | 199.2188 | 199.2188 | 199.2188 | 2097934 B |
| **ParseSpan**           | **Bytes/empty**          |       **179.6 ns** |        **43.93 ns** |       **2.41 ns** |       **178.8 ns** |   **0.0017** |        **-** |        **-** |      **32 B** |
| WriteToBufferWriter | Bytes/empty          |       105.8 ns |        17.04 ns |       0.93 ns |       105.7 ns |        - |        - |        - |         - |
| CalculateSize       | Bytes/empty          |       115.1 ns |         9.62 ns |       0.53 ns |       115.2 ns |        - |        - |        - |         - |
| Clone               | Bytes/empty          |       315.2 ns |       225.07 ns |      12.34 ns |       318.0 ns |   0.0048 |        - |        - |      96 B |
| **ParseSpan**           | **Bytes/populated**      |     **3,812.0 ns** |     **3,002.22 ns** |     **164.56 ns** |     **3,783.7 ns** |   **0.8812** |   **0.0496** |        **-** |   **16480 B** |
| WriteToBufferWriter | Bytes/populated      |       703.1 ns |       129.96 ns |       7.12 ns |       701.3 ns |        - |        - |        - |         - |
| CalculateSize       | Bytes/populated      |       131.4 ns |        53.27 ns |       2.92 ns |       132.8 ns |        - |        - |        - |         - |
| Clone               | Bytes/populated      |     7,113.3 ns |     4,076.59 ns |     223.45 ns |     7,044.7 ns |   1.7776 |   0.0992 |        - |   33256 B |
| **ParseSpan**           | **MapValue/edge**        |       **305.5 ns** |        **57.17 ns** |       **3.13 ns** |       **306.1 ns** |   **0.0029** |        **-** |        **-** |      **56 B** |
| WriteToBufferWriter | MapValue/edge        |       165.5 ns |        23.23 ns |       1.27 ns |       166.2 ns |        - |        - |        - |         - |
| CalculateSize       | MapValue/edge        |       135.0 ns |        37.80 ns |       2.07 ns |       134.2 ns |        - |        - |        - |         - |
| Clone               | MapValue/edge        |       601.2 ns |     2,032.90 ns |     111.43 ns |       549.5 ns |   0.0210 |        - |        - |     400 B |
| **ParseSpan**           | **MapValue/empty**       |       **178.3 ns** |        **33.03 ns** |       **1.81 ns** |       **178.1 ns** |   **0.0017** |        **-** |        **-** |      **32 B** |
| WriteToBufferWriter | MapValue/empty       |       104.2 ns |         3.07 ns |       0.17 ns |       104.3 ns |        - |        - |        - |         - |
| CalculateSize       | MapValue/empty       |       127.2 ns |        15.13 ns |       0.83 ns |       127.6 ns |        - |        - |        - |         - |
| Clone               | MapValue/empty       |       320.3 ns |        65.25 ns |       3.58 ns |       320.1 ns |   0.0048 |        - |        - |      96 B |
| **ParseSpan**           | **MapValue/populated**   |       **284.0 ns** |        **85.22 ns** |       **4.67 ns** |       **281.4 ns** |   **0.0033** |        **-** |        **-** |      **64 B** |
| WriteToBufferWriter | MapValue/populated   |       162.7 ns |        23.17 ns |       1.27 ns |       162.1 ns |        - |        - |        - |         - |
| CalculateSize       | MapValue/populated   |       136.8 ns |        23.83 ns |       1.31 ns |       136.9 ns |        - |        - |        - |         - |
| Clone               | MapValue/populated   |       515.5 ns |       142.59 ns |       7.82 ns |       513.0 ns |   0.0210 |        - |        - |     408 B |
| **ParseSpan**           | **Maps/edge**            |     **1,855.3 ns** |       **703.59 ns** |      **38.57 ns** |     **1,855.0 ns** |   **0.0458** |        **-** |        **-** |     **880 B** |
| WriteToBufferWriter | Maps/edge            |     1,318.7 ns |       623.58 ns |      34.18 ns |     1,299.4 ns |   0.0057 |        - |        - |     120 B |
| CalculateSize       | Maps/edge            |       768.2 ns |       331.19 ns |      18.15 ns |       761.3 ns |   0.0057 |        - |        - |     120 B |
| Clone               | Maps/edge            |     2,856.8 ns |     2,137.58 ns |     117.17 ns |     2,834.7 ns |   0.0610 |        - |        - |    1344 B |
| **ParseSpan**           | **Maps/empty**           |       **262.3 ns** |        **48.06 ns** |       **2.63 ns** |       **263.7 ns** |   **0.0148** |        **-** |        **-** |     **280 B** |
| WriteToBufferWriter | Maps/empty           |       242.7 ns |        75.19 ns |       4.12 ns |       240.9 ns |        - |        - |        - |         - |
| CalculateSize       | Maps/empty           |       258.2 ns |       121.42 ns |       6.66 ns |       254.4 ns |        - |        - |        - |         - |
| Clone               | Maps/empty           |       526.4 ns |        20.32 ns |       1.11 ns |       526.0 ns |   0.0181 |        - |        - |     344 B |
| **ParseSpan**           | **Maps/populated**       |    **23,865.3 ns** |     **8,741.96 ns** |     **479.18 ns** |    **23,763.4 ns** |   **0.9155** |   **0.0305** |        **-** |   **17448 B** |
| WriteToBufferWriter | Maps/populated       |    22,161.7 ns |     2,978.18 ns |     163.24 ns |    22,117.3 ns |   0.1221 |        - |        - |    2632 B |
| CalculateSize       | Maps/populated       |    11,412.9 ns |     3,012.49 ns |     165.12 ns |    11,491.5 ns |   0.1373 |        - |        - |    2632 B |
| Clone               | Maps/populated       |    40,327.6 ns |     9,640.31 ns |     528.42 ns |    40,602.6 ns |   1.0986 |        - |        - |   20696 B |
| **ParseSpan**           | **Node/edge**            |       **934.6 ns** |        **78.23 ns** |       **4.29 ns** |       **933.3 ns** |   **0.0162** |        **-** |        **-** |     **320 B** |
| WriteToBufferWriter | Node/edge            |     1,791.1 ns |        77.33 ns |       4.24 ns |     1,793.2 ns |   0.0248 |        - |        - |     464 B |
| CalculateSize       | Node/edge            |     1,135.7 ns |       497.59 ns |      27.27 ns |     1,150.1 ns |   0.0248 |        - |        - |     464 B |
| Clone               | Node/edge            |     1,693.5 ns |       421.78 ns |      23.12 ns |     1,696.0 ns |   0.0343 |        - |        - |     664 B |
| **ParseSpan**           | **Node/empty**           |       **258.7 ns** |     **1,403.51 ns** |      **76.93 ns** |       **268.7 ns** |   **0.0019** |        **-** |        **-** |      **40 B** |
| WriteToBufferWriter | Node/empty           |       108.0 ns |        10.94 ns |       0.60 ns |       108.3 ns |        - |        - |        - |         - |
| CalculateSize       | Node/empty           |       108.8 ns |        15.54 ns |       0.85 ns |       108.8 ns |        - |        - |        - |         - |
| Clone               | Node/empty           |       306.0 ns |       147.59 ns |       8.09 ns |       301.5 ns |   0.0052 |        - |        - |     104 B |
| **ParseSpan**           | **Node/(...)epth1 [21]** |       **279.1 ns** |        **64.28 ns** |       **3.52 ns** |       **277.2 ns** |   **0.0038** |        **-** |        **-** |      **80 B** |
| WriteToBufferWriter | Node/(...)epth1 [21] |       160.7 ns |        28.43 ns |       1.56 ns |       161.2 ns |        - |        - |        - |         - |
| CalculateSize       | Node/(...)epth1 [21] |       137.5 ns |        27.76 ns |       1.52 ns |       137.4 ns |        - |        - |        - |         - |
| Clone               | Node/(...)epth1 [21] |       522.5 ns |       172.00 ns |       9.43 ns |       527.5 ns |   0.0219 |        - |        - |     424 B |
| **ParseSpan**           | **Node/(...)epth3 [21]** |       **603.4 ns** |       **220.21 ns** |      **12.07 ns** |       **600.5 ns** |   **0.0124** |        **-** |        **-** |     **240 B** |
| WriteToBufferWriter | Node/(...)epth3 [21] |       631.6 ns |       151.78 ns |       8.32 ns |       631.5 ns |        - |        - |        - |         - |
| CalculateSize       | Node/(...)epth3 [21] |       391.9 ns |        62.38 ns |       3.42 ns |       391.3 ns |        - |        - |        - |         - |
| Clone               | Node/(...)epth3 [21] |     1,140.9 ns |       125.35 ns |       6.87 ns |     1,138.8 ns |   0.0305 |        - |        - |     584 B |
| **ParseSpan**           | **Node/(...)epth8 [21]** |     **1,365.9 ns** |       **452.87 ns** |      **24.82 ns** |     **1,353.8 ns** |   **0.0324** |        **-** |        **-** |     **640 B** |
| WriteToBufferWriter | Node/(...)epth8 [21] |     2,312.2 ns |       585.66 ns |      32.10 ns |     2,296.9 ns |   0.0229 |        - |        - |     464 B |
| CalculateSize       | Node/(...)epth8 [21] |     1,277.4 ns |       401.57 ns |      22.01 ns |     1,269.7 ns |   0.0248 |        - |        - |     464 B |
| Clone               | Node/(...)epth8 [21] |     2,633.8 ns |     1,016.34 ns |      55.71 ns |     2,611.2 ns |   0.0496 |        - |        - |     984 B |
| **ParseSpan**           | **Optio(...)esent [30]** |       **647.5 ns** |     **6,589.60 ns** |     **361.20 ns** |       **477.8 ns** |   **0.0043** |        **-** |        **-** |      **88 B** |
| WriteToBufferWriter | Optio(...)esent [30] |       205.5 ns |        16.76 ns |       0.92 ns |       205.3 ns |        - |        - |        - |         - |
| CalculateSize       | Optio(...)esent [30] |       177.2 ns |       102.96 ns |       5.64 ns |       176.8 ns |        - |        - |        - |         - |
| Clone               | Optio(...)esent [30] |       641.7 ns |        42.77 ns |       2.34 ns |       641.0 ns |   0.0229 |        - |        - |     432 B |
| **ParseSpan**           | **Optional/empty**       |       **185.7 ns** |         **3.17 ns** |       **0.17 ns** |       **185.7 ns** |   **0.0045** |        **-** |        **-** |      **88 B** |
| WriteToBufferWriter | Optional/empty       |       110.9 ns |        74.75 ns |       4.10 ns |       108.9 ns |        - |        - |        - |         - |
| CalculateSize       | Optional/empty       |       120.1 ns |       114.08 ns |       6.25 ns |       118.2 ns |        - |        - |        - |         - |
| Clone               | Optional/empty       |       317.6 ns |        64.47 ns |       3.53 ns |       318.9 ns |   0.0081 |        - |        - |     152 B |
| **ParseSpan**           | **Optional/populated**   |       **444.6 ns** |       **149.67 ns** |       **8.20 ns** |       **448.5 ns** |   **0.0095** |        **-** |        **-** |     **184 B** |
| WriteToBufferWriter | Optional/populated   |       243.6 ns |        88.17 ns |       4.83 ns |       244.6 ns |        - |        - |        - |         - |
| CalculateSize       | Optional/populated   |       178.2 ns |        43.74 ns |       2.40 ns |       178.2 ns |        - |        - |        - |         - |
| Clone               | Optional/populated   |       778.1 ns |       192.28 ns |      10.54 ns |       773.8 ns |   0.0277 |        - |        - |     528 B |
| **ParseSpan**           | **Repeated/edge**        |     **1,882.7 ns** |     **2,105.61 ns** |     **115.42 ns** |     **1,851.5 ns** |   **0.0477** |        **-** |        **-** |     **896 B** |
| WriteToBufferWriter | Repeated/edge        |       903.3 ns |        36.52 ns |       2.00 ns |       902.3 ns |        - |        - |        - |         - |
| CalculateSize       | Repeated/edge        |       754.8 ns |     1,439.71 ns |      78.92 ns |       717.5 ns |        - |        - |        - |         - |
| Clone               | Repeated/edge        |     3,021.3 ns |     4,760.01 ns |     260.91 ns |     3,053.8 ns |   0.0610 |        - |        - |    1240 B |
| **ParseSpan**           | **Repeated/empty**       |       **278.4 ns** |        **42.47 ns** |       **2.33 ns** |       **278.9 ns** |   **0.0134** |        **-** |        **-** |     **256 B** |
| WriteToBufferWriter | Repeated/empty       |       283.7 ns |        42.71 ns |       2.34 ns |       284.7 ns |        - |        - |        - |         - |
| CalculateSize       | Repeated/empty       |       274.0 ns |       204.12 ns |      11.19 ns |       268.7 ns |        - |        - |        - |         - |
| Clone               | Repeated/empty       |       776.9 ns |        41.11 ns |       2.25 ns |       778.0 ns |   0.0315 |        - |        - |     600 B |
| **ParseSpan**           | **Repeated/populated**   |    **11,493.7 ns** |     **3,712.70 ns** |     **203.51 ns** |    **11,495.9 ns** |   **0.2899** |        **-** |        **-** |    **5552 B** |
| WriteToBufferWriter | Repeated/populated   |    10,526.7 ns |     1,335.85 ns |      73.22 ns |    10,499.4 ns |   0.1221 |        - |        - |    2512 B |
| CalculateSize       | Repeated/populated   |     8,843.9 ns |    14,558.92 ns |     798.02 ns |     8,411.7 ns |   0.1221 |        - |        - |    2512 B |
| Clone               | Repeated/populated   |    24,043.7 ns |    16,693.92 ns |     915.05 ns |    23,575.4 ns |   0.6104 |        - |        - |   12832 B |
| **ParseSpan**           | **RepeatedItem/edge**    |       **381.5 ns** |        **97.33 ns** |       **5.33 ns** |       **381.7 ns** |   **0.0057** |        **-** |        **-** |     **112 B** |
| WriteToBufferWriter | RepeatedItem/edge    |       239.5 ns |       110.32 ns |       6.05 ns |       242.9 ns |        - |        - |        - |         - |
| CalculateSize       | RepeatedItem/edge    |       146.8 ns |        50.73 ns |       2.78 ns |       146.6 ns |        - |        - |        - |         - |
| Clone               | RepeatedItem/edge    |       741.9 ns |       749.07 ns |      41.06 ns |       761.4 ns |   0.0229 |        - |        - |     456 B |
| **ParseSpan**           | **RepeatedItem/empty**   |       **174.1 ns** |        **52.91 ns** |       **2.90 ns** |       **172.4 ns** |   **0.0017** |        **-** |        **-** |      **32 B** |
| WriteToBufferWriter | RepeatedItem/empty   |       103.9 ns |        22.74 ns |       1.25 ns |       103.9 ns |        - |        - |        - |         - |
| CalculateSize       | RepeatedItem/empty   |       117.5 ns |        98.15 ns |       5.38 ns |       116.5 ns |        - |        - |        - |         - |
| Clone               | RepeatedItem/empty   |       304.7 ns |        40.81 ns |       2.24 ns |       303.6 ns |   0.0048 |        - |        - |      96 B |
| **ParseSpan**           | **Repea(...)lated [22]** |       **285.7 ns** |        **12.53 ns** |       **0.69 ns** |       **285.5 ns** |   **0.0033** |        **-** |        **-** |      **64 B** |
| WriteToBufferWriter | Repea(...)lated [22] |       429.4 ns |     4,038.17 ns |     221.35 ns |       408.3 ns |        - |        - |        - |         - |
| CalculateSize       | Repea(...)lated [22] |       134.2 ns |        18.45 ns |       1.01 ns |       133.7 ns |        - |        - |        - |         - |
| Clone               | Repea(...)lated [22] |       528.1 ns |       202.04 ns |      11.07 ns |       523.9 ns |   0.0210 |        - |        - |     408 B |
| **ParseSpan**           | **Scalars/edge-max**     |       **534.3 ns** |        **75.05 ns** |       **4.11 ns** |       **534.7 ns** |   **0.0048** |        **-** |        **-** |      **96 B** |
| WriteToBufferWriter | Scalars/edge-max     |       322.0 ns |        21.97 ns |       1.20 ns |       321.6 ns |        - |        - |        - |         - |
| CalculateSize       | Scalars/edge-max     |       213.3 ns |        24.90 ns |       1.37 ns |       213.6 ns |        - |        - |        - |         - |
| Clone               | Scalars/edge-max     |       992.4 ns |       318.30 ns |      17.45 ns |       992.0 ns |   0.0229 |        - |        - |     440 B |
| **ParseSpan**           | **Scalars/edge-min**     |       **420.6 ns** |        **44.90 ns** |       **2.46 ns** |       **419.4 ns** |   **0.0048** |        **-** |        **-** |      **96 B** |
| WriteToBufferWriter | Scalars/edge-min     |       272.9 ns |        69.17 ns |       3.79 ns |       271.8 ns |        - |        - |        - |         - |
| CalculateSize       | Scalars/edge-min     |       179.9 ns |        33.01 ns |       1.81 ns |       180.1 ns |        - |        - |        - |         - |
| Clone               | Scalars/edge-min     |       810.3 ns |        98.65 ns |       5.41 ns |       809.9 ns |   0.0229 |        - |        - |     440 B |
| **ParseSpan**           | **Scalars/empty**        |       **185.0 ns** |         **5.90 ns** |       **0.32 ns** |       **185.0 ns** |   **0.0050** |        **-** |        **-** |      **96 B** |
| WriteToBufferWriter | Scalars/empty        |       115.4 ns |         8.09 ns |       0.44 ns |       115.4 ns |        - |        - |        - |         - |
| CalculateSize       | Scalars/empty        |       114.4 ns |         7.83 ns |       0.43 ns |       114.3 ns |        - |        - |        - |         - |
| Clone               | Scalars/empty        |       347.3 ns |        53.21 ns |       2.92 ns |       347.3 ns |   0.0081 |        - |        - |     160 B |
| **ParseSpan**           | **Scalars/populated**    |       **515.7 ns** |         **9.56 ns** |       **0.52 ns** |       **515.9 ns** |   **0.0048** |        **-** |        **-** |      **96 B** |
| WriteToBufferWriter | Scalars/populated    |       313.1 ns |        21.76 ns |       1.19 ns |       313.0 ns |        - |        - |        - |         - |
| CalculateSize       | Scalars/populated    |       214.5 ns |        63.85 ns |       3.50 ns |       214.9 ns |        - |        - |        - |         - |
| Clone               | Scalars/populated    |       937.5 ns |        86.47 ns |       4.74 ns |       938.9 ns |   0.0229 |        - |        - |     440 B |
| **ParseSpan**           | **Strings/edge**         |   **883,682.3 ns** | **2,606,842.19 ns** | **142,889.82 ns** |   **851,998.8 ns** | **218.7500** | **218.7500** | **218.7500** | **2097884 B** |
| WriteToBufferWriter | Strings/edge         |   170,505.3 ns |    35,126.94 ns |   1,925.43 ns |   169,836.8 ns |        - |        - |        - |         - |
| CalculateSize       | Strings/edge         |    73,703.9 ns |    40,584.65 ns |   2,224.58 ns |    74,611.4 ns |        - |        - |        - |         - |
| Clone               | Strings/edge         | 2,860,346.0 ns |   687,034.50 ns |  37,658.68 ns | 2,838,836.3 ns | 406.2500 | 406.2500 | 406.2500 | 5246045 B |
| **ParseSpan**           | **Strings/empty**        |       **178.8 ns** |        **37.74 ns** |       **2.07 ns** |       **177.7 ns** |   **0.0024** |        **-** |        **-** |      **48 B** |
| WriteToBufferWriter | Strings/empty        |       105.3 ns |        25.17 ns |       1.38 ns |       104.7 ns |        - |        - |        - |         - |
| CalculateSize       | Strings/empty        |       116.3 ns |        37.17 ns |       2.04 ns |       115.3 ns |        - |        - |        - |         - |
| Clone               | Strings/empty        |       311.4 ns |        99.96 ns |       5.48 ns |       313.9 ns |   0.0057 |        - |        - |     112 B |
| **ParseSpan**           | **Strings/populated**    |     **8,318.1 ns** |     **7,867.11 ns** |     **431.22 ns** |     **8,557.0 ns** |   **1.7853** |   **0.1831** |        **-** |   **33488 B** |
| WriteToBufferWriter | Strings/populated    |     2,130.7 ns |     1,696.31 ns |      92.98 ns |     2,080.2 ns |        - |        - |        - |         - |
| CalculateSize       | Strings/populated    |     1,162.4 ns |       138.65 ns |       7.60 ns |     1,163.8 ns |        - |        - |        - |         - |
| Clone               | Strings/populated    |    19,680.4 ns |     8,626.32 ns |     472.84 ns |    19,433.7 ns |   4.4556 |   0.4883 |        - |   83864 B |
