```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4

Job=ShortRun  Runtime=.NET 10.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method                  | Sample               | Mean            | Error            | StdDev         | Median          | Gen0     | Gen1     | Gen2     | Allocated |
|------------------------ |--------------------- |----------------:|-----------------:|---------------:|----------------:|---------:|---------:|---------:|----------:|
| **ParseStream**             | **Bytes/edge**           |   **767,318.05 ns** |   **749,719.457 ns** |  **41,094.653 ns** |   **749,346.38 ns** | **189.4531** | **175.7813** | **132.8125** | **2116277 B** |
| WriteToStream           | Bytes/edge           |    89,435.35 ns |    29,587.708 ns |   1,621.802 ns |    88,880.29 ns |   0.1221 |        - |        - |    4184 B |
| MergeBytesIntoPopulated | Bytes/edge           |   674,128.66 ns | 1,238,315.437 ns |  67,876.248 ns |   647,514.48 ns |  90.8203 |  90.8203 |  90.8203 | 1049114 B |
| **ParseStream**             | **Bytes/empty**          |       **868.36 ns** |       **242.734 ns** |      **13.305 ns** |       **873.66 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | Bytes/empty          |       781.05 ns |       463.636 ns |      25.413 ns |       793.33 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Bytes/empty          |        40.01 ns |        29.880 ns |       1.638 ns |        39.58 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Bytes/populated**      |     **9,582.70 ns** |     **9,299.034 ns** |     **509.711 ns** |     **9,416.87 ns** |   **1.9989** |   **0.2136** |        **-** |   **37528 B** |
| WriteToStream           | Bytes/populated      |     1,863.46 ns |     3,254.866 ns |     178.410 ns |     1,791.86 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Bytes/populated      |     3,378.25 ns |     1,735.522 ns |      95.130 ns |     3,359.11 ns |   0.8812 |   0.0496 |        - |   16552 B |
| **ParseStream**             | **MapValue/edge**        |       **953.22 ns** |       **409.458 ns** |      **22.444 ns** |       **947.60 ns** |   **0.2327** |   **0.0019** |        **-** |    **4352 B** |
| WriteToStream           | MapValue/edge        |       788.86 ns |       216.163 ns |      11.849 ns |       783.28 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | MapValue/edge        |        79.58 ns |         9.308 ns |       0.510 ns |        79.75 ns |   0.0033 |        - |        - |      64 B |
| **ParseStream**             | **MapValue/empty**       |       **843.03 ns** |       **321.626 ns** |      **17.629 ns** |       **849.72 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | MapValue/empty       |       752.21 ns |       718.910 ns |      39.406 ns |       758.39 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | MapValue/empty       |        34.72 ns |       104.928 ns |       5.751 ns |        31.56 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **MapValue/populated**   |       **945.53 ns** |       **377.963 ns** |      **20.717 ns** |       **938.45 ns** |   **0.2327** |   **0.0019** |        **-** |    **4360 B** |
| WriteToStream           | MapValue/populated   |       792.90 ns |       462.175 ns |      25.333 ns |       807.50 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | MapValue/populated   |        78.00 ns |        34.857 ns |       1.911 ns |        78.94 ns |   0.0038 |        - |        - |      72 B |
| **ParseStream**             | **Maps/edge**            |     **2,150.75 ns** |     **2,398.030 ns** |     **131.444 ns** |     **2,098.69 ns** |   **0.3014** |   **0.0038** |        **-** |    **5680 B** |
| WriteToStream           | Maps/edge            |     1,340.07 ns |       925.896 ns |      50.751 ns |     1,357.74 ns |   0.2327 |   0.0019 |        - |    4352 B |
| MergeBytesIntoPopulated | Maps/edge            |     1,445.65 ns |        47.810 ns |       2.621 ns |     1,446.77 ns |   0.1030 |        - |        - |    1944 B |
| **ParseStream**             | **Maps/empty**           |       **982.07 ns** |       **380.175 ns** |      **20.839 ns** |       **976.49 ns** |   **0.2565** |        **-** |        **-** |    **4792 B** |
| WriteToStream           | Maps/empty           |     1,035.73 ns |     4,758.839 ns |     260.848 ns |       913.55 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Maps/empty           |       338.12 ns |        45.091 ns |       2.472 ns |       337.40 ns |   0.0510 |        - |        - |     960 B |
| **ParseStream**             | **Maps/populated**       |    **20,528.06 ns** |     **8,467.121 ns** |     **464.111 ns** |    **20,498.19 ns** |   **1.5259** |   **0.1221** |        **-** |   **28840 B** |
| WriteToStream           | Maps/populated       |     8,919.00 ns |     2,543.227 ns |     139.403 ns |     8,896.47 ns |   0.2289 |        - |        - |    4352 B |
| MergeBytesIntoPopulated | Maps/populated       |    23,882.22 ns |    11,331.928 ns |     621.141 ns |    23,676.33 ns |   1.3733 |   0.0916 |        - |   25864 B |
| **ParseStream**             | **Node/edge**            |     **1,147.76 ns** |       **931.371 ns** |      **51.052 ns** |     **1,159.55 ns** |   **0.2499** |   **0.0038** |        **-** |    **4672 B** |
| WriteToStream           | Node/edge            |       932.23 ns |     1,280.789 ns |      70.204 ns |       905.80 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Node/edge            |       310.03 ns |        55.210 ns |       3.026 ns |       311.72 ns |   0.0205 |        - |        - |     384 B |
| **ParseStream**             | **Node/empty**           |       **815.43 ns** |       **851.793 ns** |      **46.690 ns** |       **839.84 ns** |   **0.2317** |   **0.0029** |        **-** |    **4336 B** |
| WriteToStream           | Node/empty           |       796.06 ns |       975.919 ns |      53.493 ns |       784.61 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Node/empty           |        34.41 ns |        27.117 ns |       1.486 ns |        34.15 ns |   0.0026 |        - |        - |      48 B |
| **ParseStream**             | **Node/(...)epth1 [21]** |       **890.02 ns** |       **442.681 ns** |      **24.265 ns** |       **900.68 ns** |   **0.2337** |   **0.0029** |        **-** |    **4376 B** |
| WriteToStream           | Node/(...)epth1 [21] |       771.75 ns |       529.203 ns |      29.007 ns |       760.84 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Node/(...)epth1 [21] |        78.09 ns |        32.122 ns |       1.761 ns |        78.95 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Node/(...)epth3 [21]** |     **1,165.00 ns** |     **1,603.455 ns** |      **87.891 ns** |     **1,122.18 ns** |   **0.2422** |   **0.0038** |        **-** |    **4552 B** |
| WriteToStream           | Node/(...)epth3 [21] |       879.76 ns |       383.968 ns |      21.047 ns |       875.95 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Node/(...)epth3 [21] |       318.29 ns |     2,145.669 ns |     117.611 ns |       257.16 ns |   0.0138 |        - |        - |     264 B |
| **ParseStream**             | **Node/(...)epth8 [21]** |     **1,503.33 ns** |     **1,365.884 ns** |      **74.869 ns** |     **1,488.22 ns** |   **0.2670** |   **0.0038** |        **-** |    **4992 B** |
| WriteToStream           | Node/(...)epth8 [21] |     1,446.37 ns |       426.412 ns |      23.373 ns |     1,443.78 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Node/(...)epth8 [21] |       704.85 ns |       490.752 ns |      26.900 ns |       691.43 ns |   0.0372 |        - |        - |     704 B |
| **ParseStream**             | **Oneof(...)int32 [24]** |       **919.81 ns** |     **1,577.764 ns** |      **86.483 ns** |       **936.87 ns** |   **0.2327** |   **0.0019** |        **-** |    **4352 B** |
| WriteToStream           | Oneof(...)int32 [24] |       759.72 ns |        84.169 ns |       4.614 ns |       759.27 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)int32 [24] |        55.88 ns |        28.493 ns |       1.562 ns |        55.00 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Oneof(...)ssage [26]** |       **873.18 ns** |       **134.163 ns** |       **7.354 ns** |       **874.85 ns** |   **0.2337** |   **0.0038** |        **-** |    **4368 B** |
| WriteToStream           | Oneof(...)ssage [26] |     1,123.57 ns |     7,126.356 ns |     390.620 ns |     1,105.59 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)ssage [26] |        82.25 ns |        34.796 ns |       1.907 ns |        81.57 ns |   0.0063 |        - |        - |     120 B |
| **ParseStream**             | **Oneof(...)tring [25]** |       **872.45 ns** |       **112.183 ns** |       **6.149 ns** |       **870.92 ns** |   **0.2308** |   **0.0019** |        **-** |    **4328 B** |
| WriteToStream           | Oneof(...)tring [25] |       767.42 ns |       345.618 ns |      18.944 ns |       775.89 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)tring [25] |        46.44 ns |        23.287 ns |       1.276 ns |        46.35 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Oneof/empty**          |       **870.15 ns** |     **1,364.426 ns** |      **74.789 ns** |       **876.09 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | Oneof/empty          |       794.74 ns |       371.321 ns |      20.353 ns |       783.70 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof/empty          |        38.25 ns |       151.402 ns |       8.299 ns |        36.96 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Oneof/populated-bool** |       **951.70 ns** |       **439.304 ns** |      **24.080 ns** |       **957.16 ns** |   **0.2327** |   **0.0029** |        **-** |    **4352 B** |
| WriteToStream           | Oneof/populated-bool |       781.25 ns |       353.754 ns |      19.390 ns |       786.72 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof/populated-bool |        64.27 ns |        11.784 ns |       0.646 ns |        64.54 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Oneof(...)ouble [22]** |       **884.03 ns** |       **465.225 ns** |      **25.501 ns** |       **871.33 ns** |   **0.2327** |   **0.0029** |        **-** |    **4352 B** |
| WriteToStream           | Oneof(...)ouble [22] |       750.13 ns |       858.294 ns |      47.046 ns |       730.47 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)ouble [22] |        58.04 ns |         8.521 ns |       0.467 ns |        58.26 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Oneof(...)int32 [21]** |       **880.40 ns** |       **363.654 ns** |      **19.933 ns** |       **876.60 ns** |   **0.2327** |   **0.0029** |        **-** |    **4352 B** |
| WriteToStream           | Oneof(...)int32 [21] |       771.86 ns |       562.728 ns |      30.845 ns |       756.33 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)int32 [21] |        73.60 ns |       132.427 ns |       7.259 ns |        69.88 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Oneof(...)int64 [21]** |       **953.07 ns** |     **1,773.871 ns** |      **97.232 ns** |       **939.70 ns** |   **0.2327** |   **0.0019** |        **-** |    **4352 B** |
| WriteToStream           | Oneof(...)int64 [21] |       790.83 ns |       262.204 ns |      14.372 ns |       785.10 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)int64 [21] |        70.07 ns |        44.490 ns |       2.439 ns |        69.23 ns |   0.0046 |        - |        - |      88 B |
| **ParseStream**             | **Oneof(...)ssage [23]** |       **939.19 ns** |       **797.454 ns** |      **43.711 ns** |       **922.89 ns** |   **0.2346** |   **0.0038** |        **-** |    **4408 B** |
| WriteToStream           | Oneof(...)ssage [23] |       876.62 ns |     1,137.117 ns |      62.329 ns |       867.94 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)ssage [23] |       132.28 ns |        25.576 ns |       1.402 ns |       133.03 ns |   0.0083 |        - |        - |     160 B |
| **ParseStream**             | **Oneof(...)tring [22]** |     **1,198.04 ns** |     **4,405.672 ns** |     **241.490 ns** |     **1,158.69 ns** |   **0.2337** |   **0.0038** |        **-** |    **4368 B** |
| WriteToStream           | Oneof(...)tring [22] |       848.11 ns |       727.704 ns |      39.888 ns |       835.56 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)tring [22] |        84.75 ns |        19.705 ns |       1.080 ns |        84.61 ns |   0.0042 |        - |        - |      80 B |
| **ParseStream**             | **OneofPayload/edge**    |     **1,055.98 ns** |       **591.716 ns** |      **32.434 ns** |     **1,067.70 ns** |   **0.2346** |   **0.0038** |        **-** |    **4408 B** |
| WriteToStream           | OneofPayload/edge    |       922.70 ns |     1,313.492 ns |      71.997 ns |       895.61 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | OneofPayload/edge    |       181.65 ns |        30.651 ns |       1.680 ns |       181.96 ns |   0.0062 |        - |        - |     120 B |
| **ParseStream**             | **OneofPayload/empty**   |       **803.45 ns** |       **379.026 ns** |      **20.776 ns** |       **796.26 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | OneofPayload/empty   |       756.64 ns |       257.819 ns |      14.132 ns |       751.30 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | OneofPayload/empty   |        31.88 ns |        15.446 ns |       0.847 ns |        32.15 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Oneof(...)lated [22]** |     **1,036.82 ns** |     **2,830.067 ns** |     **155.126 ns** |       **961.73 ns** |   **0.2327** |   **0.0019** |        **-** |    **4360 B** |
| WriteToStream           | Oneof(...)lated [22] |       835.71 ns |       135.380 ns |       7.421 ns |       832.52 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Oneof(...)lated [22] |        75.59 ns |        26.260 ns |       1.439 ns |        76.29 ns |   0.0038 |        - |        - |      72 B |
| **ParseStream**             | **Optio(...)esent [30]** |       **985.80 ns** |       **231.651 ns** |      **12.698 ns** |       **989.63 ns** |   **0.2346** |   **0.0038** |        **-** |    **4416 B** |
| WriteToStream           | Optio(...)esent [30] |       842.92 ns |       418.321 ns |      22.930 ns |       854.54 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Optio(...)esent [30] |       122.05 ns |        24.771 ns |       1.358 ns |       122.02 ns |   0.0067 |        - |        - |     128 B |
| **ParseStream**             | **Optional/empty**       |       **834.63 ns** |       **863.959 ns** |      **47.357 ns** |       **837.79 ns** |   **0.2327** |   **0.0029** |        **-** |    **4360 B** |
| WriteToStream           | Optional/empty       |       796.83 ns |       469.241 ns |      25.721 ns |       799.21 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Optional/empty       |        40.60 ns |        18.266 ns |       1.001 ns |        40.12 ns |   0.0038 |        - |        - |      72 B |
| **ParseStream**             | **Optional/populated**   |     **1,122.25 ns** |     **1,527.483 ns** |      **83.726 ns** |     **1,104.16 ns** |   **0.2403** |   **0.0038** |        **-** |    **4488 B** |
| WriteToStream           | Optional/populated   |       852.41 ns |       290.596 ns |      15.929 ns |       843.75 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Optional/populated   |       177.39 ns |        99.553 ns |       5.457 ns |       175.22 ns |   0.0105 |        - |        - |     200 B |
| **ParseStream**             | **Repeated/edge**        |     **1,806.23 ns** |       **965.208 ns** |      **52.906 ns** |     **1,824.65 ns** |   **0.2766** |   **0.0038** |        **-** |    **5176 B** |
| WriteToStream           | Repeated/edge        |     1,298.15 ns |       385.508 ns |      21.131 ns |     1,296.49 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Repeated/edge        |     2,373.98 ns |    18,495.591 ns |   1,013.806 ns |     2,057.64 ns |   0.0610 |        - |        - |    1160 B |
| **ParseStream**             | **Repeated/empty**       |     **1,090.09 ns** |     **1,790.845 ns** |      **98.162 ns** |     **1,128.88 ns** |   **0.2432** |   **0.0038** |        **-** |    **4552 B** |
| WriteToStream           | Repeated/empty       |       799.16 ns |       267.669 ns |      14.672 ns |       800.27 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Repeated/empty       |       186.91 ns |        68.983 ns |       3.781 ns |       188.23 ns |   0.0243 |        - |        - |     456 B |
| **ParseStream**             | **Repeated/populated**   |     **7,331.66 ns** |       **628.178 ns** |      **34.433 ns** |     **7,350.58 ns** |   **0.7019** |   **0.0229** |        **-** |   **13144 B** |
| WriteToStream           | Repeated/populated   |     4,280.55 ns |     1,963.430 ns |     107.622 ns |     4,232.78 ns |   0.2213 |        - |        - |    4184 B |
| MergeBytesIntoPopulated | Repeated/populated   |     8,592.62 ns |     7,082.518 ns |     388.217 ns |     8,563.50 ns |   0.7782 |   0.0305 |        - |   14696 B |
| **ParseStream**             | **RepeatedItem/edge**    |     **1,062.11 ns** |     **1,011.393 ns** |      **55.438 ns** |     **1,072.27 ns** |   **0.2346** |   **0.0038** |        **-** |    **4408 B** |
| WriteToStream           | RepeatedItem/edge    |       954.61 ns |     1,033.832 ns |      56.668 ns |       933.34 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | RepeatedItem/edge    |       193.09 ns |       176.349 ns |       9.666 ns |       197.72 ns |   0.0062 |        - |        - |     120 B |
| **ParseStream**             | **RepeatedItem/empty**   |       **828.02 ns** |       **493.031 ns** |      **27.025 ns** |       **813.55 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | RepeatedItem/empty   |       758.26 ns |       362.471 ns |      19.868 ns |       769.33 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | RepeatedItem/empty   |        31.02 ns |        16.824 ns |       0.922 ns |        30.59 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Repea(...)lated [22]** |       **943.72 ns** |        **91.406 ns** |       **5.010 ns** |       **940.90 ns** |   **0.2327** |   **0.0029** |        **-** |    **4360 B** |
| WriteToStream           | Repea(...)lated [22] |       902.39 ns |     2,075.692 ns |     113.776 ns |       842.46 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Repea(...)lated [22] |        77.08 ns |        28.482 ns |       1.561 ns |        76.24 ns |   0.0038 |        - |        - |      72 B |
| **ParseStream**             | **Scalars/edge-max**     |     **1,089.36 ns** |       **785.998 ns** |      **43.083 ns** |     **1,100.64 ns** |   **0.2346** |   **0.0038** |        **-** |    **4392 B** |
| WriteToStream           | Scalars/edge-max     |       962.22 ns |       627.575 ns |      34.400 ns |       962.08 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Scalars/edge-max     |       220.56 ns |       181.843 ns |       9.967 ns |       225.46 ns |   0.0055 |        - |        - |     104 B |
| **ParseStream**             | **Scalars/edge-min**     |       **986.50 ns** |       **365.974 ns** |      **20.060 ns** |       **996.44 ns** |   **0.2346** |   **0.0038** |        **-** |    **4392 B** |
| WriteToStream           | Scalars/edge-min     |       955.69 ns |       613.632 ns |      33.635 ns |       944.68 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Scalars/edge-min     |       171.06 ns |        51.769 ns |       2.838 ns |       171.10 ns |   0.0055 |        - |        - |     104 B |
| **ParseStream**             | **Scalars/empty**        |       **842.35 ns** |       **353.740 ns** |      **19.390 ns** |       **850.62 ns** |   **0.2346** |   **0.0038** |        **-** |    **4392 B** |
| WriteToStream           | Scalars/empty        |       799.81 ns |       223.678 ns |      12.261 ns |       795.16 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Scalars/empty        |        49.29 ns |        29.284 ns |       1.605 ns |        49.83 ns |   0.0055 |        - |        - |     104 B |
| **ParseStream**             | **Scalars/populated**    |     **1,145.82 ns** |       **712.785 ns** |      **39.070 ns** |     **1,153.71 ns** |   **0.2346** |   **0.0038** |        **-** |    **4392 B** |
| WriteToStream           | Scalars/populated    |       913.19 ns |       413.685 ns |      22.675 ns |       901.76 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Scalars/populated    |       177.93 ns |        79.258 ns |       4.344 ns |       176.16 ns |   0.0055 |        - |        - |     104 B |
| **ParseStream**             | **Strings/edge**         | **3,045,310.65 ns** | **2,889,999.460 ns** | **158,410.623 ns** | **3,080,224.04 ns** | **218.7500** | **203.1250** | **164.0625** | **4213962 B** |
| WriteToStream           | Strings/edge         |   868,828.01 ns |   942,551.917 ns |  51,664.451 ns |   844,816.62 ns | 130.8594 | 130.8594 | 130.8594 | 1052833 B |
| MergeBytesIntoPopulated | Strings/edge         | 1,332,907.31 ns |   979,906.952 ns |  53,712.007 ns | 1,317,364.52 ns | 212.8906 | 212.8906 | 212.8906 | 2097905 B |
| **ParseStream**             | **Strings/empty**        |     **1,076.74 ns** |     **2,485.508 ns** |     **136.239 ns** |     **1,025.87 ns** |   **0.2308** |   **0.0019** |        **-** |    **4344 B** |
| WriteToStream           | Strings/empty        |       820.75 ns |       342.405 ns |      18.768 ns |       829.27 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Strings/empty        |        43.42 ns |        22.428 ns |       1.229 ns |        44.05 ns |   0.0030 |        - |        - |      56 B |
| **ParseStream**             | **Strings/populated**    |    **17,954.88 ns** |     **7,897.959 ns** |     **432.914 ns** |    **17,997.93 ns** |   **3.7842** |   **0.6104** |        **-** |   **70880 B** |
| WriteToStream           | Strings/populated    |     7,616.85 ns |     5,036.269 ns |     276.055 ns |     7,602.09 ns |   1.0986 |   0.0153 |        - |   20592 B |
| MergeBytesIntoPopulated | Strings/populated    |     7,604.70 ns |     2,632.581 ns |     144.301 ns |     7,535.95 ns |   1.7853 |   0.1831 |        - |   33496 B |
| **ParseStream**             | **Unkno(...)/edge [22]** |     **1,015.20 ns** |       **212.167 ns** |      **11.630 ns** |     **1,015.99 ns** |   **0.2346** |   **0.0038** |        **-** |    **4408 B** |
| WriteToStream           | Unkno(...)/edge [22] |       909.91 ns |     1,335.988 ns |      73.230 ns |       905.70 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)/edge [22] |       207.54 ns |       205.909 ns |      11.287 ns |       212.00 ns |   0.0062 |        - |        - |     120 B |
| **ParseStream**             | **Unkno(...)empty [23]** |       **877.32 ns** |       **327.982 ns** |      **17.978 ns** |       **869.80 ns** |   **0.2308** |   **0.0029** |        **-** |    **4328 B** |
| WriteToStream           | Unkno(...)empty [23] |       805.01 ns |       197.847 ns |      10.845 ns |       806.39 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)empty [23] |        31.15 ns |        13.243 ns |       0.726 ns |        30.75 ns |   0.0021 |        - |        - |      40 B |
| **ParseStream**             | **Unkno(...)lated [27]** |       **920.96 ns** |       **145.708 ns** |       **7.987 ns** |       **918.44 ns** |   **0.2327** |   **0.0029** |        **-** |    **4360 B** |
| WriteToStream           | Unkno(...)lated [27] |       786.15 ns |       571.856 ns |      31.345 ns |       798.20 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)lated [27] |        79.51 ns |        73.014 ns |       4.002 ns |        78.00 ns |   0.0038 |        - |        - |      72 B |
| **ParseStream**             | **Unkno(...)/edge [22]** |     **1,133.00 ns** |       **338.895 ns** |      **18.576 ns** |     **1,123.47 ns** |   **0.2365** |   **0.0038** |        **-** |    **4432 B** |
| WriteToStream           | Unkno(...)/edge [22] |       946.16 ns |       446.786 ns |      24.490 ns |       936.41 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)/edge [22] |       253.06 ns |       109.591 ns |       6.007 ns |       252.11 ns |   0.0076 |        - |        - |     144 B |
| **ParseStream**             | **Unkno(...)empty [23]** |       **892.11 ns** |       **912.224 ns** |      **50.002 ns** |       **890.49 ns** |   **0.2327** |   **0.0029** |        **-** |    **4352 B** |
| WriteToStream           | Unkno(...)empty [23] |       810.67 ns |       719.016 ns |      39.412 ns |       821.87 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)empty [23] |        38.80 ns |        23.388 ns |       1.282 ns |        39.40 ns |   0.0034 |        - |        - |      64 B |
| **ParseStream**             | **Unkno(...)lated [27]** |     **1,614.49 ns** |     **8,748.492 ns** |     **479.534 ns** |     **1,735.75 ns** |   **0.2365** |   **0.0038** |        **-** |    **4440 B** |
| WriteToStream           | Unkno(...)lated [27] |       906.35 ns |       140.380 ns |       7.695 ns |       906.48 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Unkno(...)lated [27] |       167.07 ns |        25.475 ns |       1.396 ns |       167.64 ns |   0.0081 |        - |        - |     152 B |
| **ParseStream**             | **Wrapp(...)esent [30]** |       **992.53 ns** |       **463.504 ns** |      **25.406 ns** |       **991.36 ns** |   **0.2327** |   **0.0019** |        **-** |    **4376 B** |
| WriteToStream           | Wrapp(...)esent [30] |       844.47 ns |        98.267 ns |       5.386 ns |       846.69 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Wrapp(...)esent [30] |       128.61 ns |        70.861 ns |       3.884 ns |       128.92 ns |   0.0045 |        - |        - |      88 B |
| **ParseStream**             | **Wrappers/empty**       |       **828.04 ns** |       **583.725 ns** |      **31.996 ns** |       **816.01 ns** |   **0.2337** |   **0.0029** |        **-** |    **4376 B** |
| WriteToStream           | Wrappers/empty       |       725.00 ns |       160.539 ns |       8.800 ns |       725.95 ns |   0.2232 |   0.0029 |        - |    4184 B |
| MergeBytesIntoPopulated | Wrappers/empty       |        74.94 ns |       690.867 ns |      37.869 ns |        55.37 ns |   0.0047 |        - |        - |      88 B |
| **ParseStream**             | **Wrappers/populated**   |     **1,154.60 ns** |     **2,037.146 ns** |     **111.663 ns** |     **1,124.66 ns** |   **0.2403** |   **0.0038** |        **-** |    **4504 B** |
| WriteToStream           | Wrappers/populated   |     1,242.62 ns |     2,950.566 ns |     161.730 ns |     1,262.57 ns |   0.2232 |   0.0019 |        - |    4184 B |
| MergeBytesIntoPopulated | Wrappers/populated   |       256.43 ns |       114.486 ns |       6.275 ns |       253.89 ns |   0.0114 |        - |        - |     216 B |
