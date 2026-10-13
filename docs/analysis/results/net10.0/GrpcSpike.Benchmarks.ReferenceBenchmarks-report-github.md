```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4

Job=ShortRun  Runtime=.NET 10.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  Categories=Reference  

```
| Method              | Sample               | Mean           | Error           | StdDev        | Median         | Gen0     | Gen1     | Gen2     | Allocated |
|-------------------- |--------------------- |---------------:|----------------:|--------------:|---------------:|---------:|---------:|---------:|----------:|
| **ParseSpan**           | **Bytes/edge**           |   **790,089.7 ns** |   **203,046.97 ns** |  **11,129.69 ns** |   **785,134.3 ns** |  **52.7344** |  **52.7344** |  **52.7344** | **1048889 B** |
| WriteToBufferWriter | Bytes/edge           |    60,147.0 ns |    33,285.91 ns |   1,824.51 ns |    60,199.2 ns |        - |        - |        - |         - |
| CalculateSize       | Bytes/edge           |       127.0 ns |        13.81 ns |       0.76 ns |       126.6 ns |        - |        - |        - |         - |
| Clone               | Bytes/edge           | 1,530,998.9 ns |   889,612.78 ns |  48,762.68 ns | 1,534,454.2 ns | 199.2188 | 199.2188 | 199.2188 | 2098158 B |
| **ParseSpan**           | **Bytes/empty**          |       **182.7 ns** |        **28.47 ns** |       **1.56 ns** |       **182.7 ns** |   **0.0017** |        **-** |        **-** |      **32 B** |
| WriteToBufferWriter | Bytes/empty          |       101.8 ns |        10.69 ns |       0.59 ns |       101.9 ns |        - |        - |        - |         - |
| CalculateSize       | Bytes/empty          |       112.1 ns |        13.38 ns |       0.73 ns |       112.6 ns |        - |        - |        - |         - |
| Clone               | Bytes/empty          |       306.1 ns |       112.04 ns |       6.14 ns |       304.7 ns |   0.0048 |        - |        - |      96 B |
| **ParseSpan**           | **Bytes/populated**      |     **3,544.6 ns** |       **913.57 ns** |      **50.08 ns** |     **3,527.6 ns** |   **0.8812** |   **0.0496** |        **-** |   **16480 B** |
| WriteToBufferWriter | Bytes/populated      |       742.8 ns |       507.29 ns |      27.81 ns |       739.0 ns |        - |        - |        - |         - |
| CalculateSize       | Bytes/populated      |       128.6 ns |        26.77 ns |       1.47 ns |       127.8 ns |        - |        - |        - |         - |
| Clone               | Bytes/populated      |     7,624.8 ns |     4,839.76 ns |     265.28 ns |     7,674.1 ns |   1.7700 |   0.0916 |        - |   33256 B |
| **ParseSpan**           | **MapValue/edge**        |       **312.4 ns** |        **14.19 ns** |       **0.78 ns** |       **312.5 ns** |   **0.0029** |        **-** |        **-** |      **56 B** |
| WriteToBufferWriter | MapValue/edge        |       168.2 ns |        15.04 ns |       0.82 ns |       167.9 ns |        - |        - |        - |         - |
| CalculateSize       | MapValue/edge        |       219.2 ns |     1,893.40 ns |     103.78 ns |       192.7 ns |        - |        - |        - |         - |
| Clone               | MapValue/edge        |       541.5 ns |       118.79 ns |       6.51 ns |       541.4 ns |   0.0210 |        - |        - |     400 B |
| **ParseSpan**           | **MapValue/empty**       |       **198.3 ns** |       **191.45 ns** |      **10.49 ns** |       **196.0 ns** |   **0.0017** |        **-** |        **-** |      **32 B** |
| WriteToBufferWriter | MapValue/empty       |       107.6 ns |        21.37 ns |       1.17 ns |       108.1 ns |        - |        - |        - |         - |
| CalculateSize       | MapValue/empty       |       121.5 ns |        89.62 ns |       4.91 ns |       123.4 ns |        - |        - |        - |         - |
| Clone               | MapValue/empty       |       304.6 ns |        55.92 ns |       3.07 ns |       303.5 ns |   0.0048 |        - |        - |      96 B |
| **ParseSpan**           | **MapValue/populated**   |       **301.6 ns** |       **118.64 ns** |       **6.50 ns** |       **299.8 ns** |   **0.0033** |        **-** |        **-** |      **64 B** |
| WriteToBufferWriter | MapValue/populated   |       166.5 ns |        11.69 ns |       0.64 ns |       166.4 ns |        - |        - |        - |         - |
| CalculateSize       | MapValue/populated   |       140.0 ns |       104.62 ns |       5.73 ns |       140.2 ns |        - |        - |        - |         - |
| Clone               | MapValue/populated   |       517.9 ns |       188.06 ns |      10.31 ns |       517.5 ns |   0.0210 |        - |        - |     408 B |
| **ParseSpan**           | **Maps/edge**            |     **1,946.5 ns** |       **874.56 ns** |      **47.94 ns** |     **1,953.5 ns** |   **0.0458** |        **-** |        **-** |     **880 B** |
| WriteToBufferWriter | Maps/edge            |     1,261.5 ns |       213.03 ns |      11.68 ns |     1,266.1 ns |   0.0057 |        - |        - |     120 B |
| CalculateSize       | Maps/edge            |       731.0 ns |       138.06 ns |       7.57 ns |       730.7 ns |   0.0057 |        - |        - |     120 B |
| Clone               | Maps/edge            |     2,895.6 ns |     1,976.40 ns |     108.33 ns |     2,844.4 ns |   0.0687 |        - |        - |    1344 B |
| **ParseSpan**           | **Maps/empty**           |       **264.2 ns** |        **42.57 ns** |       **2.33 ns** |       **264.8 ns** |   **0.0148** |        **-** |        **-** |     **280 B** |
| WriteToBufferWriter | Maps/empty           |       234.2 ns |         8.77 ns |       0.48 ns |       234.0 ns |        - |        - |        - |         - |
| CalculateSize       | Maps/empty           |       252.6 ns |        44.08 ns |       2.42 ns |       253.5 ns |        - |        - |        - |         - |
| Clone               | Maps/empty           |       525.2 ns |        44.23 ns |       2.42 ns |       526.0 ns |   0.0181 |        - |        - |     344 B |
| **ParseSpan**           | **Maps/populated**       |    **37,367.5 ns** |   **369,646.94 ns** |  **20,261.60 ns** |    **27,401.9 ns** |   **0.9155** |   **0.0305** |        **-** |   **17448 B** |
| WriteToBufferWriter | Maps/populated       |    22,845.8 ns |     5,388.80 ns |     295.38 ns |    22,840.4 ns |   0.1221 |        - |        - |    2632 B |
| CalculateSize       | Maps/populated       |    11,355.8 ns |     3,785.69 ns |     207.51 ns |    11,271.1 ns |   0.1373 |        - |        - |    2632 B |
| Clone               | Maps/populated       |    40,678.1 ns |     8,816.41 ns |     483.26 ns |    40,515.7 ns |   1.0986 |        - |        - |   20696 B |
| **ParseSpan**           | **Node/edge**            |       **934.7 ns** |       **275.39 ns** |      **15.10 ns** |       **926.9 ns** |   **0.0162** |        **-** |        **-** |     **320 B** |
| WriteToBufferWriter | Node/edge            |     1,718.0 ns |       248.93 ns |      13.64 ns |     1,725.4 ns |   0.0248 |        - |        - |     464 B |
| CalculateSize       | Node/edge            |     1,067.5 ns |       268.33 ns |      14.71 ns |     1,066.2 ns |   0.0248 |        - |        - |     464 B |
| Clone               | Node/edge            |     1,664.3 ns |       155.35 ns |       8.52 ns |     1,661.2 ns |   0.0343 |        - |        - |     664 B |
| **ParseSpan**           | **Node/empty**           |       **188.5 ns** |        **69.57 ns** |       **3.81 ns** |       **186.6 ns** |   **0.0019** |        **-** |        **-** |      **40 B** |
| WriteToBufferWriter | Node/empty           |       110.9 ns |        36.19 ns |       1.98 ns |       109.8 ns |        - |        - |        - |         - |
| CalculateSize       | Node/empty           |       108.5 ns |        10.48 ns |       0.57 ns |       108.7 ns |        - |        - |        - |         - |
| Clone               | Node/empty           |       311.4 ns |        84.12 ns |       4.61 ns |       310.6 ns |   0.0052 |        - |        - |     104 B |
| **ParseSpan**           | **Node/(...)epth1 [21]** |       **287.2 ns** |        **73.13 ns** |       **4.01 ns** |       **285.0 ns** |   **0.0038** |        **-** |        **-** |      **80 B** |
| WriteToBufferWriter | Node/(...)epth1 [21] |       165.6 ns |        55.71 ns |       3.05 ns |       165.0 ns |        - |        - |        - |         - |
| CalculateSize       | Node/(...)epth1 [21] |       140.5 ns |        37.06 ns |       2.03 ns |       139.3 ns |        - |        - |        - |         - |
| Clone               | Node/(...)epth1 [21] |       522.4 ns |        66.03 ns |       3.62 ns |       522.3 ns |   0.0219 |        - |        - |     424 B |
| **ParseSpan**           | **Node/(...)epth3 [21]** |       **742.0 ns** |     **3,541.83 ns** |     **194.14 ns** |       **632.2 ns** |   **0.0124** |        **-** |        **-** |     **240 B** |
| WriteToBufferWriter | Node/(...)epth3 [21] |       626.2 ns |        49.91 ns |       2.74 ns |       626.4 ns |        - |        - |        - |         - |
| CalculateSize       | Node/(...)epth3 [21] |       394.5 ns |        55.66 ns |       3.05 ns |       393.8 ns |        - |        - |        - |         - |
| Clone               | Node/(...)epth3 [21] |     1,149.4 ns |       103.81 ns |       5.69 ns |     1,148.7 ns |   0.0305 |        - |        - |     584 B |
| **ParseSpan**           | **Node/(...)epth8 [21]** |     **1,382.7 ns** |       **215.68 ns** |      **11.82 ns** |     **1,386.9 ns** |   **0.0324** |        **-** |        **-** |     **640 B** |
| WriteToBufferWriter | Node/(...)epth8 [21] |     2,307.5 ns |       346.51 ns |      18.99 ns |     2,300.6 ns |   0.0229 |        - |        - |     464 B |
| CalculateSize       | Node/(...)epth8 [21] |     1,259.4 ns |       278.92 ns |      15.29 ns |     1,254.6 ns |   0.0248 |        - |        - |     464 B |
| Clone               | Node/(...)epth8 [21] |     2,755.8 ns |     1,658.48 ns |      90.91 ns |     2,719.9 ns |   0.0496 |        - |        - |     984 B |
| **ParseSpan**           | **Optio(...)esent [30]** |       **669.5 ns** |     **4,651.78 ns** |     **254.98 ns** |       **573.1 ns** |   **0.0043** |        **-** |        **-** |      **88 B** |
| WriteToBufferWriter | Optio(...)esent [30] |       194.2 ns |        47.79 ns |       2.62 ns |       193.1 ns |        - |        - |        - |         - |
| CalculateSize       | Optio(...)esent [30] |       178.3 ns |       241.29 ns |      13.23 ns |       181.2 ns |        - |        - |        - |         - |
| Clone               | Optio(...)esent [30] |       724.4 ns |       513.94 ns |      28.17 ns |       719.3 ns |   0.0229 |        - |        - |     432 B |
| **ParseSpan**           | **Optional/empty**       |       **196.9 ns** |        **12.56 ns** |       **0.69 ns** |       **196.7 ns** |   **0.0045** |        **-** |        **-** |      **88 B** |
| WriteToBufferWriter | Optional/empty       |       113.5 ns |        18.47 ns |       1.01 ns |       114.0 ns |        - |        - |        - |         - |
| CalculateSize       | Optional/empty       |       119.7 ns |        20.39 ns |       1.12 ns |       119.2 ns |        - |        - |        - |         - |
| Clone               | Optional/empty       |       308.0 ns |        10.64 ns |       0.58 ns |       308.3 ns |   0.0081 |        - |        - |     152 B |
| **ParseSpan**           | **Optional/populated**   |       **450.4 ns** |       **100.12 ns** |       **5.49 ns** |       **447.3 ns** |   **0.0095** |        **-** |        **-** |     **184 B** |
| WriteToBufferWriter | Optional/populated   |       245.7 ns |       122.05 ns |       6.69 ns |       243.2 ns |        - |        - |        - |         - |
| CalculateSize       | Optional/populated   |       181.5 ns |        19.86 ns |       1.09 ns |       181.5 ns |        - |        - |        - |         - |
| Clone               | Optional/populated   |       778.3 ns |        92.16 ns |       5.05 ns |       777.1 ns |   0.0277 |        - |        - |     528 B |
| **ParseSpan**           | **Repeated/edge**        |     **1,815.9 ns** |       **294.32 ns** |      **16.13 ns** |     **1,808.4 ns** |   **0.0477** |        **-** |        **-** |     **896 B** |
| WriteToBufferWriter | Repeated/edge        |       916.2 ns |       201.85 ns |      11.06 ns |       912.1 ns |        - |        - |        - |         - |
| CalculateSize       | Repeated/edge        |       682.4 ns |        74.82 ns |       4.10 ns |       681.5 ns |        - |        - |        - |         - |
| Clone               | Repeated/edge        |     2,844.7 ns |       499.01 ns |      27.35 ns |     2,834.3 ns |   0.0648 |        - |        - |    1240 B |
| **ParseSpan**           | **Repeated/empty**       |       **281.3 ns** |       **111.11 ns** |       **6.09 ns** |       **279.6 ns** |   **0.0134** |        **-** |        **-** |     **256 B** |
| WriteToBufferWriter | Repeated/empty       |       296.6 ns |        38.58 ns |       2.11 ns |       297.2 ns |        - |        - |        - |         - |
| CalculateSize       | Repeated/empty       |       261.6 ns |        61.08 ns |       3.35 ns |       260.0 ns |        - |        - |        - |         - |
| Clone               | Repeated/empty       |       778.3 ns |       128.81 ns |       7.06 ns |       779.0 ns |   0.0315 |        - |        - |     600 B |
| **ParseSpan**           | **Repeated/populated**   |    **11,521.9 ns** |     **2,632.71 ns** |     **144.31 ns** |    **11,479.2 ns** |   **0.2899** |        **-** |        **-** |    **5552 B** |
| WriteToBufferWriter | Repeated/populated   |    10,745.2 ns |     2,614.05 ns |     143.29 ns |    10,707.1 ns |   0.1221 |        - |        - |    2512 B |
| CalculateSize       | Repeated/populated   |     7,994.0 ns |     3,736.50 ns |     204.81 ns |     7,896.9 ns |   0.1221 |        - |        - |    2512 B |
| Clone               | Repeated/populated   |    37,210.5 ns |   188,162.76 ns |  10,313.84 ns |    36,060.6 ns |   0.6714 |        - |        - |   12832 B |
| **ParseSpan**           | **RepeatedItem/edge**    |       **404.3 ns** |       **374.88 ns** |      **20.55 ns** |       **394.0 ns** |   **0.0057** |        **-** |        **-** |     **112 B** |
| WriteToBufferWriter | RepeatedItem/edge    |       268.1 ns |       211.74 ns |      11.61 ns |       262.7 ns |        - |        - |        - |         - |
| CalculateSize       | RepeatedItem/edge    |       146.8 ns |        29.59 ns |       1.62 ns |       147.2 ns |        - |        - |        - |         - |
| Clone               | RepeatedItem/edge    |       734.4 ns |       255.48 ns |      14.00 ns |       740.7 ns |   0.0238 |        - |        - |     456 B |
| **ParseSpan**           | **RepeatedItem/empty**   |       **189.4 ns** |        **81.59 ns** |       **4.47 ns** |       **187.5 ns** |   **0.0017** |        **-** |        **-** |      **32 B** |
| WriteToBufferWriter | RepeatedItem/empty   |       110.7 ns |        53.79 ns |       2.95 ns |       109.5 ns |        - |        - |        - |         - |
| CalculateSize       | RepeatedItem/empty   |       118.2 ns |       230.33 ns |      12.63 ns |       111.5 ns |        - |        - |        - |         - |
| Clone               | RepeatedItem/empty   |       306.9 ns |       115.26 ns |       6.32 ns |       309.5 ns |   0.0048 |        - |        - |      96 B |
| **ParseSpan**           | **Repea(...)lated [22]** |       **314.5 ns** |       **103.65 ns** |       **5.68 ns** |       **311.6 ns** |   **0.0033** |        **-** |        **-** |      **64 B** |
| WriteToBufferWriter | Repea(...)lated [22] |       166.2 ns |        45.14 ns |       2.47 ns |       165.8 ns |        - |        - |        - |         - |
| CalculateSize       | Repea(...)lated [22] |       138.7 ns |       104.33 ns |       5.72 ns |       138.9 ns |        - |        - |        - |         - |
| Clone               | Repea(...)lated [22] |       527.2 ns |       183.23 ns |      10.04 ns |       525.3 ns |   0.0210 |        - |        - |     408 B |
| **ParseSpan**           | **Scalars/edge-max**     |       **553.9 ns** |       **381.34 ns** |      **20.90 ns** |       **543.6 ns** |   **0.0048** |        **-** |        **-** |      **96 B** |
| WriteToBufferWriter | Scalars/edge-max     |       330.7 ns |       109.18 ns |       5.98 ns |       327.9 ns |        - |        - |        - |         - |
| CalculateSize       | Scalars/edge-max     |       216.2 ns |        39.33 ns |       2.16 ns |       215.3 ns |        - |        - |        - |         - |
| Clone               | Scalars/edge-max     |     1,070.0 ns |       459.84 ns |      25.21 ns |     1,084.3 ns |   0.0229 |        - |        - |     440 B |
| **ParseSpan**           | **Scalars/edge-min**     |       **429.5 ns** |       **111.03 ns** |       **6.09 ns** |       **427.6 ns** |   **0.0048** |        **-** |        **-** |      **96 B** |
| WriteToBufferWriter | Scalars/edge-min     |       274.4 ns |        90.75 ns |       4.97 ns |       275.0 ns |        - |        - |        - |         - |
| CalculateSize       | Scalars/edge-min     |       184.5 ns |        25.31 ns |       1.39 ns |       183.9 ns |        - |        - |        - |         - |
| Clone               | Scalars/edge-min     |       809.2 ns |       120.52 ns |       6.61 ns |       806.6 ns |   0.0229 |        - |        - |     440 B |
| **ParseSpan**           | **Scalars/empty**        |       **202.0 ns** |        **29.93 ns** |       **1.64 ns** |       **202.3 ns** |   **0.0050** |        **-** |        **-** |      **96 B** |
| WriteToBufferWriter | Scalars/empty        |       115.2 ns |        54.54 ns |       2.99 ns |       114.0 ns |        - |        - |        - |         - |
| CalculateSize       | Scalars/empty        |       161.0 ns |       699.87 ns |      38.36 ns |       165.9 ns |        - |        - |        - |         - |
| Clone               | Scalars/empty        |       354.3 ns |       293.94 ns |      16.11 ns |       361.7 ns |   0.0081 |        - |        - |     160 B |
| **ParseSpan**           | **Scalars/populated**    |       **563.9 ns** |       **188.21 ns** |      **10.32 ns** |       **567.2 ns** |   **0.0048** |        **-** |        **-** |      **96 B** |
| WriteToBufferWriter | Scalars/populated    |       316.0 ns |       101.59 ns |       5.57 ns |       316.1 ns |        - |        - |        - |         - |
| CalculateSize       | Scalars/populated    |       222.2 ns |       144.90 ns |       7.94 ns |       221.4 ns |        - |        - |        - |         - |
| Clone               | Scalars/populated    |       997.6 ns |     1,384.06 ns |      75.87 ns |       959.5 ns |   0.0229 |        - |        - |     440 B |
| **ParseSpan**           | **Strings/edge**         |   **847,216.4 ns** | **1,203,215.08 ns** |  **65,952.28 ns** |   **856,710.0 ns** | **216.7969** | **216.7969** | **216.7969** | **2097898 B** |
| WriteToBufferWriter | Strings/edge         |   176,348.0 ns |   227,461.64 ns |  12,467.94 ns |   178,540.1 ns |        - |        - |        - |         - |
| CalculateSize       | Strings/edge         |    81,402.2 ns |   174,073.57 ns |   9,541.56 ns |    76,531.4 ns |        - |        - |        - |         - |
| Clone               | Strings/edge         | 2,976,212.6 ns | 3,735,204.03 ns | 204,739.14 ns | 2,907,854.9 ns | 406.2500 | 406.2500 | 406.2500 | 5246136 B |
| **ParseSpan**           | **Strings/empty**        |       **193.7 ns** |        **49.01 ns** |       **2.69 ns** |       **194.7 ns** |   **0.0024** |        **-** |        **-** |      **48 B** |
| WriteToBufferWriter | Strings/empty        |       113.7 ns |        32.97 ns |       1.81 ns |       112.8 ns |        - |        - |        - |         - |
| CalculateSize       | Strings/empty        |       109.8 ns |         5.87 ns |       0.32 ns |       109.6 ns |        - |        - |        - |         - |
| Clone               | Strings/empty        |       297.7 ns |        47.61 ns |       2.61 ns |       296.8 ns |   0.0057 |        - |        - |     112 B |
| **ParseSpan**           | **Strings/populated**    |     **7,958.9 ns** |       **298.61 ns** |      **16.37 ns** |     **7,965.0 ns** |   **1.7853** |   **0.1831** |        **-** |   **33488 B** |
| WriteToBufferWriter | Strings/populated    |     2,061.5 ns |       421.82 ns |      23.12 ns |     2,073.9 ns |        - |        - |        - |         - |
| CalculateSize       | Strings/populated    |     1,787.2 ns |    14,259.99 ns |     781.64 ns |     1,384.1 ns |        - |        - |        - |         - |
| Clone               | Strings/populated    |    20,757.6 ns |    17,518.93 ns |     960.27 ns |    20,574.7 ns |   4.4556 |   0.4883 |        - |   83864 B |
