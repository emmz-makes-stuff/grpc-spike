```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 11.0.0 (11.0.0-rc.1.26425.128, 11.0.26.42628), X64 RyuJIT x86-64-v4

Job=ShortRun  Toolchain=.NET 11.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method                  | Sample               | Mean            | Error            | StdDev         | Median          | Gen0     | Gen1     | Gen2     | Allocated |
|------------------------ |--------------------- |----------------:|-----------------:|---------------:|----------------:|---------:|---------:|---------:|----------:|
| **ParseStream**             | **Bytes/edge**           |   **778,702.24 ns** |   **948,655.782 ns** |  **51,999.025 ns** |   **781,134.37 ns** | **192.3828** | **180.6641** | **135.7422** | **2116257 B** |
| WriteToStream           | Bytes/edge           |    95,019.40 ns |   109,524.528 ns |   6,003.409 ns |    92,587.25 ns |   0.1221 |        - |        - |    4184 B |
| MergeBytesIntoPopulated | Bytes/edge           |   662,591.84 ns |   886,175.189 ns |  48,574.253 ns |   638,892.25 ns |  91.7969 |  91.7969 |  91.7969 | 1049127 B |
| **ParseStream**             | **Bytes/empty**          |       **900.14 ns** |     **1,624.309 ns** |      **89.034 ns** |       **865.61 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | Bytes/empty          |       782.20 ns |       897.336 ns |      49.186 ns |       767.95 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Bytes/empty          |        36.50 ns |        11.136 ns |       0.610 ns |        36.41 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Bytes/populated**      |    **18,462.08 ns** |   **194,766.132 ns** |  **10,675.789 ns** |    **14,506.74 ns** |   **1.9989** |   **0.2136** |        **-** |   **37528 B** |
| WriteToStream           | Bytes/populated      |     1,868.78 ns |       993.067 ns |      54.433 ns |     1,847.79 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Bytes/populated      |     3,714.56 ns |     5,333.714 ns |     292.359 ns |     3,700.63 ns |   0.8774 |   0.0458 |        - |   16552 B |
| **ParseStream**             | **MapValue/edge**        |       **923.04 ns** |       **360.708 ns** |      **19.772 ns** |       **921.88 ns** |   **0.2327** |   **0.0019** |        **-** |    **4352 B** |
| WriteToStream           | MapValue/edge        |       819.27 ns |     1,152.506 ns |      63.173 ns |       787.32 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | MapValue/edge        |        82.99 ns |       101.834 ns |       5.582 ns |        80.52 ns |   0.0033 |        - |        - |      64 B |
| **ParseStream**             | **MapValue/empty**       |       **904.17 ns** |        **54.393 ns** |       **2.981 ns** |       **903.50 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | MapValue/empty       |       759.35 ns |       359.413 ns |      19.701 ns |       757.76 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | MapValue/empty       |        31.93 ns |        12.288 ns |       0.674 ns |        32.25 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **MapValue/populated**   |     **1,001.29 ns** |     **1,118.976 ns** |      **61.335 ns** |       **980.59 ns** |   **0.2327** |   **0.0029** |        **-** |    **4360 B** |
| WriteToStream           | MapValue/populated   |       873.25 ns |     1,485.466 ns |      81.423 ns |       835.36 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | MapValue/populated   |        74.51 ns |         6.920 ns |       0.379 ns |        74.51 ns |   0.0038 |        - |        - |      72 B |
| **ParseStream**             | **Maps/edge**            |     **2,014.29 ns** |       **973.283 ns** |      **53.349 ns** |     **2,016.63 ns** |   **0.3014** |   **0.0038** |        **-** |    **5680 B** |
| WriteToStream           | Maps/edge            |     1,430.72 ns |     2,726.733 ns |     149.461 ns |     1,385.15 ns |   0.2327 |   0.0019 |        - |    4352 B |
| MergeBytesIntoPopulated | Maps/edge            |     1,452.25 ns |     1,009.009 ns |      55.307 ns |     1,447.96 ns |   0.1030 |        - |        - |    1944 B |
| **ParseStream**             | **Maps/empty**           |     **1,614.04 ns** |    **14,359.138 ns** |     **787.073 ns** |     **1,304.83 ns** |   **0.2556** |        **-** |        **-** |    **4792 B** |
| WriteToStream           | Maps/empty           |       908.54 ns |     1,577.127 ns |      86.448 ns |       898.79 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Maps/empty           |       351.85 ns |       394.372 ns |      21.617 ns |       362.76 ns |   0.0510 |        - |        - |     960 B |
| **ParseStream**             | **Maps/populated**       |    **21,463.10 ns** |     **2,843.673 ns** |     **155.871 ns** |    **21,408.14 ns** |   **1.5259** |   **0.1221** |        **-** |   **28840 B** |
| WriteToStream           | Maps/populated       |     9,470.98 ns |     8,354.207 ns |     457.922 ns |     9,447.49 ns |   0.2289 |        - |        - |    4352 B |
| MergeBytesIntoPopulated | Maps/populated       |    25,574.88 ns |    49,735.428 ns |   2,726.167 ns |    24,323.23 ns |   1.2207 |        - |        - |   25864 B |
| **ParseStream**             | **Node/edge**            |     **1,151.57 ns** |       **497.394 ns** |      **27.264 ns** |     **1,165.16 ns** |   **0.2499** |   **0.0038** |        **-** |    **4672 B** |
| WriteToStream           | Node/edge            |       982.53 ns |       667.509 ns |      36.588 ns |     1,000.04 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Node/edge            |       318.02 ns |       176.607 ns |       9.680 ns |       313.09 ns |   0.0205 |        - |        - |     384 B |
| **ParseStream**             | **Node/empty**           |       **819.41 ns** |       **179.599 ns** |       **9.844 ns** |       **819.59 ns** |   **0.2308** |   **0.0019** |        **-** |    **4336 B** |
| WriteToStream           | Node/empty           |       750.11 ns |       239.309 ns |      13.117 ns |       746.44 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Node/empty           |        33.15 ns |         9.405 ns |       0.515 ns |        33.23 ns |   0.0026 |        - |        - |      48 B |
| **ParseStream**             | **Node/(...)epth1 [21]** |     **1,007.63 ns** |       **716.593 ns** |      **39.279 ns** |     **1,008.62 ns** |   **0.2327** |   **0.0019** |        **-** |    **4376 B** |
| WriteToStream           | Node/(...)epth1 [21] |       823.25 ns |       555.944 ns |      30.473 ns |       828.71 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Node/(...)epth1 [21] |        77.63 ns |        12.558 ns |       0.688 ns |        77.45 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Node/(...)epth3 [21]** |     **1,162.60 ns** |       **918.391 ns** |      **50.340 ns** |     **1,178.70 ns** |   **0.2422** |   **0.0038** |        **-** |    **4552 B** |
| WriteToStream           | Node/(...)epth3 [21] |       905.58 ns |     1,193.946 ns |      65.444 ns |       920.08 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Node/(...)epth3 [21] |       247.03 ns |        75.696 ns |       4.149 ns |       246.38 ns |   0.0138 |        - |        - |     264 B |
| **ParseStream**             | **Node/(...)epth8 [21]** |     **1,623.47 ns** |     **3,049.734 ns** |     **167.166 ns** |     **1,590.51 ns** |   **0.2670** |   **0.0038** |        **-** |    **4992 B** |
| WriteToStream           | Node/(...)epth8 [21] |     1,441.55 ns |     1,370.266 ns |      75.109 ns |     1,420.17 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Node/(...)epth8 [21] |       748.16 ns |       323.262 ns |      17.719 ns |       743.23 ns |   0.0372 |        - |        - |     704 B |
| **ParseStream**             | **Oneof(...)int32 [24]** |       **854.83 ns** |       **569.227 ns** |      **31.201 ns** |       **850.96 ns** |   **0.2327** |   **0.0029** |        **-** |    **4352 B** |
| WriteToStream           | Oneof(...)int32 [24] |       800.56 ns |       431.050 ns |      23.627 ns |       814.01 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)int32 [24] |        61.41 ns |        74.935 ns |       4.107 ns |        59.39 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Oneof(...)ssage [26]** |       **937.54 ns** |     **1,424.786 ns** |      **78.097 ns** |       **917.90 ns** |   **0.2327** |   **0.0019** |        **-** |    **4368 B** |
| WriteToStream           | Oneof(...)ssage [26] |       765.48 ns |       476.885 ns |      26.140 ns |       768.96 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)ssage [26] |        94.99 ns |        79.070 ns |       4.334 ns |        94.79 ns |   0.0063 |        - |        - |     120 B |
| **ParseStream**             | **Oneof(...)tring [25]** |       **880.72 ns** |       **302.821 ns** |      **16.599 ns** |       **879.02 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | Oneof(...)tring [25] |       941.75 ns |     1,592.831 ns |      87.308 ns |       893.11 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)tring [25] |        44.77 ns |        41.651 ns |       2.283 ns |        43.96 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Oneof/empty**          |       **901.15 ns** |       **529.962 ns** |      **29.049 ns** |       **910.20 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | Oneof/empty          |       807.61 ns |       657.458 ns |      36.037 ns |       811.02 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof/empty          |        29.83 ns |        20.999 ns |       1.151 ns |        30.16 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Oneof/populated-bool** |       **983.02 ns** |       **617.010 ns** |      **33.820 ns** |       **972.16 ns** |   **0.2327** |   **0.0019** |        **-** |    **4352 B** |
| WriteToStream           | Oneof/populated-bool |       840.19 ns |       832.684 ns |      45.642 ns |       858.67 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof/populated-bool |        64.47 ns |        13.961 ns |       0.765 ns |        64.36 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Oneof(...)ouble [22]** |       **930.96 ns** |       **681.798 ns** |      **37.372 ns** |       **948.37 ns** |   **0.2327** |   **0.0019** |        **-** |    **4352 B** |
| WriteToStream           | Oneof(...)ouble [22] |       767.87 ns |       471.221 ns |      25.829 ns |       760.72 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)ouble [22] |        64.78 ns |        60.937 ns |       3.340 ns |        63.54 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Oneof(...)int32 [21]** |     **1,098.03 ns** |     **1,838.454 ns** |     **100.772 ns** |     **1,118.16 ns** |   **0.2327** |   **0.0019** |        **-** |    **4352 B** |
| WriteToStream           | Oneof(...)int32 [21] |       768.83 ns |       163.418 ns |       8.958 ns |       765.79 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)int32 [21] |        68.98 ns |        24.755 ns |       1.357 ns |        69.47 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Oneof(...)int64 [21]** |       **939.53 ns** |       **866.839 ns** |      **47.514 ns** |       **931.28 ns** |   **0.2327** |   **0.0019** |        **-** |    **4352 B** |
| WriteToStream           | Oneof(...)int64 [21] |       786.57 ns |       444.300 ns |      24.354 ns |       780.32 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)int64 [21] |        74.14 ns |        20.332 ns |       1.114 ns |        73.98 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Oneof(...)ssage [23]** |       **898.04 ns** |       **624.452 ns** |      **34.228 ns** |       **904.35 ns** |   **0.2356** |   **0.0038** |        **-** |    **4408 B** |
| WriteToStream           | Oneof(...)ssage [23] |       782.09 ns |       290.724 ns |      15.936 ns |       777.87 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)ssage [23] |       131.24 ns |        66.823 ns |       3.663 ns |       129.73 ns |   0.0083 |        - |        - |     160 B |
| **ParseStream**             | **Oneof(...)tring [22]** |       **854.67 ns** |       **721.587 ns** |      **39.553 ns** |       **838.09 ns** |   **0.2337** |   **0.0038** |        **-** |    **4368 B** |
| WriteToStream           | Oneof(...)tring [22] |       804.35 ns |     1,539.281 ns |      84.373 ns |       769.72 ns |   0.2213 |        - |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)tring [22] |        80.99 ns |        16.502 ns |       0.905 ns |        80.56 ns |   0.0042 |        - |        - |      80 B |
| **ParseStream**             | **OneofPayload/edge**    |     **1,092.87 ns** |     **1,137.952 ns** |      **62.375 ns** |     **1,059.24 ns** |   **0.2346** |   **0.0038** |        **-** |    **4408 B** |
| WriteToStream           | OneofPayload/edge    |       868.36 ns |       137.840 ns |       7.555 ns |       871.03 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | OneofPayload/edge    |       178.28 ns |        79.728 ns |       4.370 ns |       176.70 ns |   0.0062 |        - |        - |     120 B |
| **ParseStream**             | **OneofPayload/empty**   |       **853.19 ns** |     **1,364.461 ns** |      **74.791 ns** |       **885.48 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | OneofPayload/empty   |       705.49 ns |       414.523 ns |      22.721 ns |       696.76 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | OneofPayload/empty   |        32.71 ns |        23.889 ns |       1.309 ns |        32.05 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Oneof(...)lated [22]** |       **971.46 ns** |     **1,105.903 ns** |      **60.618 ns** |       **950.13 ns** |   **0.2327** |   **0.0019** |        **-** |    **4360 B** |
| WriteToStream           | Oneof(...)lated [22] |       765.68 ns |       746.822 ns |      40.936 ns |       754.48 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)lated [22] |        70.89 ns |         9.079 ns |       0.498 ns |        70.70 ns |   0.0038 |        - |        - |      72 B |
| **ParseStream**             | **Optio(...)esent [30]** |       **965.18 ns** |       **478.125 ns** |      **26.208 ns** |       **971.65 ns** |   **0.2346** |   **0.0038** |        **-** |    **4416 B** |
| WriteToStream           | Optio(...)esent [30] |       778.50 ns |        52.635 ns |       2.885 ns |       779.45 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Optio(...)esent [30] |       120.01 ns |        16.113 ns |       0.883 ns |       120.47 ns |   0.0067 |        - |        - |     128 B |
| **ParseStream**             | **Optional/empty**       |       **745.76 ns** |       **555.904 ns** |      **30.471 ns** |       **736.76 ns** |   **0.2327** |   **0.0029** |        **-** |    **4360 B** |
| WriteToStream           | Optional/empty       |       692.89 ns |       367.204 ns |      20.128 ns |       688.26 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Optional/empty       |        41.01 ns |         9.273 ns |       0.508 ns |        41.19 ns |   0.0038 |        - |        - |      72 B |
| **ParseStream**             | **Optional/populated**   |     **1,167.68 ns** |     **2,175.415 ns** |     **119.242 ns** |     **1,136.83 ns** |   **0.2403** |   **0.0038** |        **-** |    **4488 B** |
| WriteToStream           | Optional/populated   |       839.47 ns |       824.427 ns |      45.190 ns |       858.39 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Optional/populated   |       162.88 ns |        71.346 ns |       3.911 ns |       161.13 ns |   0.0105 |        - |        - |     200 B |
| **ParseStream**             | **Repeated/edge**        |     **1,701.93 ns** |       **740.389 ns** |      **40.583 ns** |     **1,698.77 ns** |   **0.2766** |   **0.0038** |        **-** |    **5176 B** |
| WriteToStream           | Repeated/edge        |     1,200.93 ns |       144.307 ns |       7.910 ns |     1,205.17 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Repeated/edge        |     1,357.07 ns |       984.286 ns |      53.952 ns |     1,338.65 ns |   0.0610 |        - |        - |    1160 B |
| **ParseStream**             | **Repeated/empty**       |       **933.86 ns** |     **2,002.798 ns** |     **109.780 ns** |       **902.02 ns** |   **0.2432** |   **0.0038** |        **-** |    **4552 B** |
| WriteToStream           | Repeated/empty       |       727.71 ns |       184.901 ns |      10.135 ns |       725.59 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Repeated/empty       |       185.47 ns |        37.749 ns |       2.069 ns |       186.23 ns |   0.0243 |        - |        - |     456 B |
| **ParseStream**             | **Repeated/populated**   |     **7,191.60 ns** |     **2,995.014 ns** |     **164.167 ns** |     **7,179.80 ns** |   **0.7019** |   **0.0229** |        **-** |   **13144 B** |
| WriteToStream           | Repeated/populated   |     3,964.18 ns |       833.281 ns |      45.675 ns |     3,961.86 ns |   0.2213 |        - |        - |    4184 B |
| MergeBytesIntoPopulated | Repeated/populated   |     7,736.61 ns |     1,064.890 ns |      58.370 ns |     7,717.84 ns |   0.7782 |   0.0305 |        - |   14696 B |
| **ParseStream**             | **RepeatedItem/edge**    |       **990.09 ns** |       **197.590 ns** |      **10.831 ns** |       **991.89 ns** |   **0.2346** |   **0.0038** |        **-** |    **4408 B** |
| WriteToStream           | RepeatedItem/edge    |       829.01 ns |       566.799 ns |      31.068 ns |       845.28 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | RepeatedItem/edge    |       193.99 ns |       252.989 ns |      13.867 ns |       188.65 ns |   0.0062 |        - |        - |     120 B |
| **ParseStream**             | **RepeatedItem/empty**   |       **920.68 ns** |       **776.184 ns** |      **42.545 ns** |       **932.10 ns** |   **0.2308** |   **0.0019** |        **-** |    **4328 B** |
| WriteToStream           | RepeatedItem/empty   |       798.40 ns |       367.745 ns |      20.157 ns |       804.01 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | RepeatedItem/empty   |        29.91 ns |         5.020 ns |       0.275 ns |        29.84 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Repea(...)lated [22]** |       **849.12 ns** |       **802.548 ns** |      **43.990 ns** |       **832.94 ns** |   **0.2327** |   **0.0029** |        **-** |    **4360 B** |
| WriteToStream           | Repea(...)lated [22] |       755.17 ns |       536.208 ns |      29.391 ns |       744.33 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Repea(...)lated [22] |        73.62 ns |        77.242 ns |       4.234 ns |        71.68 ns |   0.0038 |        - |        - |      72 B |
| **ParseStream**             | **Scalars/edge-max**     |     **1,028.84 ns** |     **1,035.918 ns** |      **56.782 ns** |     **1,021.81 ns** |   **0.2346** |   **0.0038** |        **-** |    **4392 B** |
| WriteToStream           | Scalars/edge-max     |     1,174.31 ns |     7,432.444 ns |     407.397 ns |     1,056.42 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Scalars/edge-max     |       201.20 ns |       280.715 ns |      15.387 ns |       192.77 ns |   0.0055 |        - |        - |     104 B |
| **ParseStream**             | **Scalars/edge-min**     |     **1,058.86 ns** |     **1,578.400 ns** |      **86.517 ns** |     **1,064.46 ns** |   **0.2346** |   **0.0038** |        **-** |    **4392 B** |
| WriteToStream           | Scalars/edge-min     |       876.49 ns |        79.005 ns |       4.331 ns |       878.65 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Scalars/edge-min     |       167.23 ns |       117.692 ns |       6.451 ns |       165.14 ns |   0.0055 |        - |        - |     104 B |
| **ParseStream**             | **Scalars/empty**        |       **806.51 ns** |       **696.628 ns** |      **38.185 ns** |       **817.58 ns** |   **0.2346** |   **0.0038** |        **-** |    **4392 B** |
| WriteToStream           | Scalars/empty        |       745.30 ns |       675.115 ns |      37.005 ns |       730.57 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Scalars/empty        |        44.88 ns |        43.810 ns |       2.401 ns |        43.86 ns |   0.0055 |        - |        - |     104 B |
| **ParseStream**             | **Scalars/populated**    |     **1,047.98 ns** |     **1,389.666 ns** |      **76.172 ns** |     **1,082.65 ns** |   **0.2346** |   **0.0038** |        **-** |    **4392 B** |
| WriteToStream           | Scalars/populated    |       904.22 ns |       499.342 ns |      27.371 ns |       888.84 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Scalars/populated    |       182.42 ns |        50.711 ns |       2.780 ns |       181.87 ns |   0.0055 |        - |        - |     104 B |
| **ParseStream**             | **Strings/edge**         | **2,849,633.61 ns** | **5,972,979.097 ns** | **327,399.141 ns** | **2,728,370.31 ns** | **238.2813** | **226.5625** | **183.5938** | **4213972 B** |
| WriteToStream           | Strings/edge         |   807,248.31 ns |   526,685.233 ns |  28,869.395 ns |   801,455.68 ns | 129.8828 | 129.8828 | 129.8828 | 1052825 B |
| MergeBytesIntoPopulated | Strings/edge         | 1,121,614.70 ns | 1,563,840.915 ns |  85,719.398 ns | 1,155,906.08 ns | 216.7969 | 216.7969 | 216.7969 | 2097892 B |
| **ParseStream**             | **Strings/empty**        |       **784.51 ns** |        **33.309 ns** |       **1.826 ns** |       **783.59 ns** |   **0.2317** |   **0.0029** |        **-** |    **4344 B** |
| WriteToStream           | Strings/empty        |     1,084.53 ns |     8,433.420 ns |     462.264 ns |       896.72 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Strings/empty        |        40.62 ns |         9.048 ns |       0.496 ns |        40.49 ns |   0.0030 |        - |        - |      56 B |
| **ParseStream**             | **Strings/populated**    |    **16,085.86 ns** |     **4,405.313 ns** |     **241.470 ns** |    **16,209.55 ns** |   **3.7842** |   **0.6104** |        **-** |   **70880 B** |
| WriteToStream           | Strings/populated    |     7,827.40 ns |    12,984.398 ns |     711.719 ns |     7,592.11 ns |   1.0986 |   0.0153 |        - |   20592 B |
| MergeBytesIntoPopulated | Strings/populated    |     7,878.47 ns |     4,227.619 ns |     231.730 ns |     7,866.36 ns |   1.7853 |   0.1907 |        - |   33496 B |
| **ParseStream**             | **Unkno(...)/edge [22]** |       **982.87 ns** |       **537.004 ns** |      **29.435 ns** |       **968.64 ns** |   **0.2346** |   **0.0038** |        **-** |    **4408 B** |
| WriteToStream           | Unkno(...)/edge [22] |       881.92 ns |       209.533 ns |      11.485 ns |       875.58 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)/edge [22] |       183.39 ns |       112.270 ns |       6.154 ns |       179.88 ns |   0.0062 |        - |        - |     120 B |
| **ParseStream**             | **Unkno(...)empty [23]** |     **1,218.58 ns** |     **8,850.816 ns** |     **485.143 ns** |     **1,031.38 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | Unkno(...)empty [23] |       784.41 ns |     1,326.289 ns |      72.698 ns |       822.23 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)empty [23] |        31.31 ns |        48.303 ns |       2.648 ns |        29.95 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Unkno(...)lated [27]** |       **914.30 ns** |       **734.865 ns** |      **40.280 ns** |       **928.66 ns** |   **0.2327** |   **0.0019** |        **-** |    **4360 B** |
| WriteToStream           | Unkno(...)lated [27] |       742.37 ns |       430.793 ns |      23.613 ns |       742.72 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)lated [27] |        77.93 ns |        82.845 ns |       4.541 ns |        76.03 ns |   0.0038 |        - |        - |      72 B |
| **ParseStream**             | **Unkno(...)/edge [22]** |     **1,118.07 ns** |       **816.537 ns** |      **44.757 ns** |     **1,100.26 ns** |   **0.2365** |   **0.0038** |        **-** |    **4432 B** |
| WriteToStream           | Unkno(...)/edge [22] |       859.78 ns |       453.785 ns |      24.873 ns |       849.87 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)/edge [22] |       235.20 ns |       196.211 ns |      10.755 ns |       233.60 ns |   0.0076 |        - |        - |     144 B |
| **ParseStream**             | **Unkno(...)empty [23]** |       **900.37 ns** |       **521.717 ns** |      **28.597 ns** |       **895.84 ns** |   **0.2327** |   **0.0029** |        **-** |    **4352 B** |
| WriteToStream           | Unkno(...)empty [23] |       729.33 ns |       548.111 ns |      30.044 ns |       718.23 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)empty [23] |        36.77 ns |        19.016 ns |       1.042 ns |        36.46 ns |   0.0034 |        - |        - |      64 B |
| **ParseStream**             | **Unkno(...)lated [27]** |       **994.45 ns** |       **660.422 ns** |      **36.200 ns** |     **1,012.15 ns** |   **0.2365** |   **0.0038** |        **-** |    **4440 B** |
| WriteToStream           | Unkno(...)lated [27] |       802.00 ns |       325.789 ns |      17.858 ns |       805.70 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)lated [27] |       154.35 ns |        53.590 ns |       2.937 ns |       152.97 ns |   0.0081 |        - |        - |     152 B |
| **ParseStream**             | **Wrapp(...)esent [30]** |     **1,050.46 ns** |     **2,034.978 ns** |     **111.544 ns** |     **1,031.85 ns** |   **0.2327** |   **0.0019** |        **-** |    **4376 B** |
| WriteToStream           | Wrapp(...)esent [30] |       930.13 ns |       294.856 ns |      16.162 ns |       931.67 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Wrapp(...)esent [30] |       122.19 ns |        77.634 ns |       4.255 ns |       120.49 ns |   0.0045 |        - |        - |      88 B |
| **ParseStream**             | **Wrappers/empty**       |       **820.69 ns** |       **391.387 ns** |      **21.453 ns** |       **822.52 ns** |   **0.2337** |   **0.0029** |        **-** |    **4376 B** |
| WriteToStream           | Wrappers/empty       |       701.47 ns |        97.341 ns |       5.336 ns |       701.92 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Wrappers/empty       |        44.71 ns |         7.704 ns |       0.422 ns |        44.94 ns |   0.0047 |        - |        - |      88 B |
| **ParseStream**             | **Wrappers/populated**   |     **1,035.87 ns** |       **426.187 ns** |      **23.361 ns** |     **1,042.71 ns** |   **0.2403** |   **0.0038** |        **-** |    **4504 B** |
| WriteToStream           | Wrappers/populated   |       929.81 ns |       287.110 ns |      15.737 ns |       935.82 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Wrappers/populated   |       234.42 ns |       145.614 ns |       7.982 ns |       230.95 ns |   0.0114 |        - |        - |     216 B |
