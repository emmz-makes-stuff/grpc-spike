```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4

Job=ShortRun  Runtime=.NET 10.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method                 | Sample               | Mean             | Error             | StdDev          | Median           | Gen0     | Gen1     | Gen2     | Allocated |
|----------------------- |--------------------- |-----------------:|------------------:|----------------:|-----------------:|---------:|---------:|---------:|----------:|
| **New**                    | **Bytes/edge**           |        **27.631 ns** |         **6.1368 ns** |       **0.3364 ns** |        **27.664 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Bytes/edge           |   773,573.368 ns |    99,398.7397 ns |   5,448.3803 ns |   773,028.506 ns |  74.2188 |  74.2188 |  74.2188 | 1049257 B |
| ParseSpan              | Bytes/edge           |   764,904.998 ns |   222,878.6428 ns |  12,216.7305 ns |   763,631.625 ns |  72.2656 |  72.2656 |  72.2656 | 1049029 B |
| ToByteArray            | Bytes/edge           |   783,703.924 ns |   390,320.1470 ns |  21,394.7644 ns |   781,647.756 ns | 126.9531 | 126.9531 | 126.9531 | 1049707 B |
| WriteToSpan            | Bytes/edge           |    56,052.363 ns |    19,696.5100 ns |   1,079.6322 ns |    55,527.824 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Bytes/edge           |   630,971.528 ns |    82,333.2057 ns |   4,512.9608 ns |   632,667.879 ns |        - |        - |        - |         - |
| CalculateSize          | Bytes/edge           |         3.776 ns |         1.0886 ns |       0.0597 ns |         3.764 ns |        - |        - |        - |         - |
| Clone                  | Bytes/edge           |        24.931 ns |         7.6125 ns |       0.4173 ns |        24.844 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Bytes/edge           |        44.938 ns |         9.9100 ns |       0.5432 ns |        44.909 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Bytes/edge           |        55.489 ns |       398.1077 ns |      21.8216 ns |        43.244 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Bytes/empty**          |        **26.820 ns** |        **17.0991 ns** |       **0.9373 ns** |        **26.568 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Bytes/empty          |        87.206 ns |        12.2671 ns |       0.6724 ns |        87.031 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | Bytes/empty          |        24.048 ns |        11.1571 ns |       0.6116 ns |        23.774 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | Bytes/empty          |        52.260 ns |        27.8378 ns |       1.5259 ns |        51.639 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Bytes/empty          |         6.397 ns |         1.1416 ns |       0.0626 ns |         6.430 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Bytes/empty          |         8.029 ns |         0.4567 ns |       0.0250 ns |         8.018 ns |        - |        - |        - |         - |
| CalculateSize          | Bytes/empty          |         2.876 ns |         0.3629 ns |       0.0199 ns |         2.869 ns |        - |        - |        - |         - |
| Clone                  | Bytes/empty          |        25.717 ns |        21.4332 ns |       1.1748 ns |        25.320 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Bytes/empty          |        35.566 ns |        12.7597 ns |       0.6994 ns |        35.794 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Bytes/empty          |        31.868 ns |         4.8479 ns |       0.2657 ns |        31.790 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Bytes/populated**      |        **29.012 ns** |         **8.4488 ns** |       **0.4631 ns** |        **29.243 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Bytes/populated      |     3,289.048 ns |     1,524.6883 ns |      83.5733 ns |     3,267.980 ns |   0.8926 |   0.0496 |        - |   16720 B |
| ParseSpan              | Bytes/populated      |     3,909.517 ns |    19,052.9822 ns |   1,044.3582 ns |     3,337.306 ns |   0.8812 |   0.0496 |        - |   16552 B |
| ToByteArray            | Bytes/populated      |     3,241.573 ns |     2,411.2573 ns |     132.1691 ns |     3,248.845 ns |   0.8774 |        - |        - |   16496 B |
| WriteToSpan            | Bytes/populated      |       295.664 ns |       138.0183 ns |       7.5652 ns |       292.315 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Bytes/populated      |    10,263.152 ns |     2,144.4831 ns |     117.5464 ns |    10,326.173 ns |        - |        - |        - |         - |
| CalculateSize          | Bytes/populated      |         4.002 ns |         3.9493 ns |       0.2165 ns |         3.883 ns |        - |        - |        - |         - |
| Clone                  | Bytes/populated      |        25.050 ns |        13.2844 ns |       0.7282 ns |        24.715 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Bytes/populated      |        44.811 ns |        68.0700 ns |       3.7311 ns |        42.867 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Bytes/populated      |        37.130 ns |         5.3539 ns |       0.2935 ns |        37.273 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **MapValue/edge**        |        **24.093 ns** |        **11.4102 ns** |       **0.6254 ns** |        **23.980 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | MapValue/edge        |       137.403 ns |        30.7335 ns |       1.6846 ns |       137.334 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | MapValue/edge        |        67.818 ns |         3.8639 ns |       0.2118 ns |        67.926 ns |   0.0033 |        - |        - |      64 B |
| ToByteArray            | MapValue/edge        |        84.321 ns |        76.2030 ns |       4.1769 ns |        82.249 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | MapValue/edge        |        26.210 ns |        12.0315 ns |       0.6595 ns |        25.841 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | MapValue/edge        |        37.018 ns |         6.9688 ns |       0.3820 ns |        36.809 ns |        - |        - |        - |         - |
| CalculateSize          | MapValue/edge        |        10.268 ns |         2.5365 ns |       0.1390 ns |        10.307 ns |        - |        - |        - |         - |
| Clone                  | MapValue/edge        |        20.261 ns |        15.7077 ns |       0.8610 ns |        19.801 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | MapValue/edge        |        33.813 ns |         5.9323 ns |       0.3252 ns |        33.701 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | MapValue/edge        |        26.628 ns |        11.1111 ns |       0.6090 ns |        26.718 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **MapValue/empty**       |        **23.782 ns** |         **9.2016 ns** |       **0.5044 ns** |        **23.496 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | MapValue/empty       |        89.124 ns |        69.7791 ns |       3.8248 ns |        87.468 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | MapValue/empty       |        39.719 ns |         8.2363 ns |       0.4515 ns |        39.634 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | MapValue/empty       |        50.793 ns |         6.9803 ns |       0.3826 ns |        50.732 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | MapValue/empty       |         5.758 ns |         0.0922 ns |       0.0051 ns |         5.756 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | MapValue/empty       |         8.751 ns |         5.3792 ns |       0.2949 ns |         8.610 ns |        - |        - |        - |         - |
| CalculateSize          | MapValue/empty       |         2.080 ns |         0.3667 ns |       0.0201 ns |         2.087 ns |        - |        - |        - |         - |
| Clone                  | MapValue/empty       |        20.036 ns |        14.8368 ns |       0.8133 ns |        19.709 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | MapValue/empty       |        32.017 ns |         5.8917 ns |       0.3229 ns |        32.038 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | MapValue/empty       |        28.229 ns |         1.1178 ns |       0.0613 ns |        28.247 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **MapValue/populated**   |        **26.061 ns** |        **11.8647 ns** |       **0.6503 ns** |        **25.843 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | MapValue/populated   |       127.627 ns |         5.2880 ns |       0.2899 ns |       127.644 ns |   0.0126 |        - |        - |     240 B |
| ParseSpan              | MapValue/populated   |        66.337 ns |        52.9488 ns |       2.9023 ns |        65.095 ns |   0.0038 |        - |        - |      72 B |
| ToByteArray            | MapValue/populated   |        81.160 ns |        26.8316 ns |       1.4707 ns |        80.719 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | MapValue/populated   |        29.621 ns |        24.9740 ns |       1.3689 ns |        29.701 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | MapValue/populated   |        34.544 ns |         4.5482 ns |       0.2493 ns |        34.518 ns |        - |        - |        - |         - |
| CalculateSize          | MapValue/populated   |        12.261 ns |         3.7568 ns |       0.2059 ns |        12.225 ns |        - |        - |        - |         - |
| Clone                  | MapValue/populated   |        19.619 ns |        12.5544 ns |       0.6881 ns |        19.816 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | MapValue/populated   |        36.157 ns |        13.2205 ns |       0.7247 ns |        36.568 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | MapValue/populated   |        26.570 ns |        15.1218 ns |       0.8289 ns |        26.451 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Maps/edge**            |       **227.116 ns** |       **951.9079 ns** |      **52.1773 ns** |       **222.057 ns** |   **0.0269** |        **-** |        **-** |     **504 B** |
| ParseByteArray         | Maps/edge            |     1,191.097 ns |       729.0208 ns |      39.9601 ns |     1,190.054 ns |   0.0820 |        - |        - |    1560 B |
| ParseSpan              | Maps/edge            |     1,142.916 ns |       824.6772 ns |      45.2033 ns |     1,168.736 ns |   0.0744 |        - |        - |    1392 B |
| ToByteArray            | Maps/edge            |       712.150 ns |       328.1680 ns |      17.9880 ns |       712.730 ns |   0.0200 |        - |        - |     384 B |
| WriteToSpan            | Maps/edge            |       562.397 ns |       315.9518 ns |      17.3184 ns |       569.070 ns |   0.0086 |        - |        - |     168 B |
| WriteToSegmentedWriter | Maps/edge            |       714.713 ns |       522.8489 ns |      28.6591 ns |       700.100 ns |   0.0162 |        - |        - |     312 B |
| CalculateSize          | Maps/edge            |        86.117 ns |        14.6163 ns |       0.8012 ns |        86.096 ns |        - |        - |        - |         - |
| Clone                  | Maps/edge            |       819.644 ns |       157.8724 ns |       8.6535 ns |       816.480 ns |   0.0925 |        - |        - |    1744 B |
| MergeIntoNew           | Maps/edge            |       640.728 ns |       539.1832 ns |      29.5545 ns |       630.102 ns |   0.0668 |        - |        - |    1248 B |
| MergeIntoPopulated     | Maps/edge            |     1,025.893 ns |       987.8813 ns |      54.1491 ns |     1,002.478 ns |   0.0954 |        - |        - |    1800 B |
| **New**                    | **Maps/empty**           |       **182.672 ns** |       **212.9612 ns** |      **11.6731 ns** |       **177.890 ns** |   **0.0269** |        **-** |        **-** |     **504 B** |
| ParseByteArray         | Maps/empty           |       254.933 ns |        83.9535 ns |       4.6018 ns |       252.482 ns |   0.0358 |        - |        - |     672 B |
| ParseSpan              | Maps/empty           |       166.262 ns |        62.4257 ns |       3.4218 ns |       164.920 ns |   0.0269 |        - |        - |     504 B |
| ToByteArray            | Maps/empty           |        79.189 ns |        17.4553 ns |       0.9568 ns |        79.306 ns |   0.0046 |        - |        - |      88 B |
| WriteToSpan            | Maps/empty           |        32.002 ns |         2.9240 ns |       0.1603 ns |        32.061 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Maps/empty           |        36.940 ns |        58.2131 ns |       3.1909 ns |        35.744 ns |        - |        - |        - |         - |
| CalculateSize          | Maps/empty           |         4.911 ns |         3.0335 ns |       0.1663 ns |         4.905 ns |        - |        - |        - |         - |
| Clone                  | Maps/empty           |       310.309 ns |       150.0577 ns |       8.2252 ns |       311.891 ns |   0.0510 |        - |        - |     960 B |
| MergeIntoNew           | Maps/empty           |       192.144 ns |        83.8464 ns |       4.5959 ns |       191.623 ns |   0.0269 |        - |        - |     504 B |
| MergeIntoPopulated     | Maps/empty           |       321.869 ns |        55.6148 ns |       3.0484 ns |       320.277 ns |   0.0510 |        - |        - |     960 B |
| **New**                    | **Maps/populated**       |       **172.688 ns** |       **166.8917 ns** |       **9.1479 ns** |       **168.670 ns** |   **0.0269** |        **-** |        **-** |     **504 B** |
| ParseByteArray         | Maps/populated       |    19,177.413 ns |     2,878.1767 ns |     157.7626 ns |    19,176.210 ns |   1.3123 |   0.0916 |        - |   24720 B |
| ParseSpan              | Maps/populated       |    19,186.635 ns |       594.2432 ns |      32.5725 ns |    19,190.394 ns |   1.3123 |   0.0916 |        - |   24552 B |
| ToByteArray            | Maps/populated       |    11,032.209 ns |     1,213.4260 ns |      66.5120 ns |    11,032.251 ns |   0.1068 |        - |        - |    2136 B |
| WriteToSpan            | Maps/populated       |     8,413.394 ns |     2,329.7200 ns |     127.6998 ns |     8,358.169 ns |        - |        - |        - |     168 B |
| WriteToSegmentedWriter | Maps/populated       |    11,173.571 ns |     3,651.1658 ns |     200.1327 ns |    11,176.478 ns |   0.0458 |        - |        - |    1096 B |
| CalculateSize          | Maps/populated       |     2,609.827 ns |       428.5793 ns |      23.4919 ns |     2,604.659 ns |        - |        - |        - |         - |
| Clone                  | Maps/populated       |    11,834.599 ns |       601.1820 ns |      32.9528 ns |    11,843.088 ns |   1.0376 |   0.0458 |        - |   19624 B |
| MergeIntoNew           | Maps/populated       |    10,388.690 ns |     1,460.5522 ns |      80.0578 ns |    10,351.210 ns |   0.9766 |   0.0458 |        - |   18424 B |
| MergeIntoPopulated     | Maps/populated       |    14,559.512 ns |     2,558.1782 ns |     140.2224 ns |    14,530.195 ns |   1.0529 |   0.0458 |        - |   19736 B |
| **New**                    | **Node/edge**            |        **27.018 ns** |         **1.6065 ns** |       **0.0881 ns** |        **27.011 ns** |   **0.0026** |        **-** |        **-** |      **48 B** |
| ParseByteArray         | Node/edge            |       347.453 ns |       444.5362 ns |      24.3665 ns |       339.999 ns |   0.0291 |        - |        - |     552 B |
| ParseSpan              | Node/edge            |       260.207 ns |        76.2840 ns |       4.1814 ns |       259.821 ns |   0.0205 |        - |        - |     384 B |
| ToByteArray            | Node/edge            |       209.901 ns |        35.3806 ns |       1.9393 ns |       209.764 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Node/edge            |       140.170 ns |        37.4857 ns |       2.0547 ns |       139.164 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Node/edge            |       156.446 ns |        66.5231 ns |       3.6464 ns |       155.900 ns |        - |        - |        - |         - |
| CalculateSize          | Node/edge            |        27.411 ns |         9.5265 ns |       0.5222 ns |        27.247 ns |        - |        - |        - |         - |
| Clone                  | Node/edge            |       168.087 ns |        75.4350 ns |       4.1348 ns |       169.504 ns |   0.0205 |        - |        - |     384 B |
| MergeIntoNew           | Node/edge            |       160.610 ns |        64.6367 ns |       3.5430 ns |       158.665 ns |   0.0205 |        - |        - |     384 B |
| MergeIntoPopulated     | Node/edge            |       325.986 ns |     3,540.6761 ns |     194.0764 ns |       217.812 ns |   0.0205 |        - |        - |     384 B |
| **New**                    | **Node/empty**           |        **27.520 ns** |         **9.9817 ns** |       **0.5471 ns** |        **27.388 ns** |   **0.0026** |        **-** |        **-** |      **48 B** |
| ParseByteArray         | Node/empty           |        90.660 ns |        32.8871 ns |       1.8027 ns |        89.690 ns |   0.0114 |        - |        - |     216 B |
| ParseSpan              | Node/empty           |        24.341 ns |        28.4460 ns |       1.5592 ns |        23.445 ns |   0.0026 |        - |        - |      48 B |
| ToByteArray            | Node/empty           |        51.966 ns |         5.8143 ns |       0.3187 ns |        51.943 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Node/empty           |         6.425 ns |         7.3526 ns |       0.4030 ns |         6.213 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Node/empty           |         9.119 ns |         1.6494 ns |       0.0904 ns |         9.125 ns |        - |        - |        - |         - |
| CalculateSize          | Node/empty           |         2.626 ns |         2.0810 ns |       0.1141 ns |         2.562 ns |        - |        - |        - |         - |
| Clone                  | Node/empty           |        23.238 ns |         9.6223 ns |       0.5274 ns |        23.138 ns |   0.0026 |        - |        - |      48 B |
| MergeIntoNew           | Node/empty           |        34.502 ns |        34.8309 ns |       1.9092 ns |        33.454 ns |   0.0026 |        - |        - |      48 B |
| MergeIntoPopulated     | Node/empty           |        30.035 ns |         9.4065 ns |       0.5156 ns |        29.888 ns |   0.0026 |        - |        - |      48 B |
| **New**                    | **Node/(...)epth1 [21]** |        **24.788 ns** |         **7.5896 ns** |       **0.4160 ns** |        **24.669 ns** |   **0.0026** |        **-** |        **-** |      **48 B** |
| ParseByteArray         | Node/(...)epth1 [21] |       131.858 ns |        17.5483 ns |       0.9619 ns |       131.545 ns |   0.0136 |        - |        - |     256 B |
| ParseSpan              | Node/(...)epth1 [21] |        72.088 ns |        56.2111 ns |       3.0811 ns |        70.357 ns |   0.0046 |        - |        - |      88 B |
| ToByteArray            | Node/(...)epth1 [21] |        91.811 ns |       171.6794 ns |       9.4103 ns |        87.130 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Node/(...)epth1 [21] |        27.711 ns |         2.1439 ns |       0.1175 ns |        27.656 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Node/(...)epth1 [21] |        36.691 ns |        11.2109 ns |       0.6145 ns |        36.587 ns |        - |        - |        - |         - |
| CalculateSize          | Node/(...)epth1 [21] |        13.624 ns |         1.9582 ns |       0.1073 ns |        13.665 ns |        - |        - |        - |         - |
| Clone                  | Node/(...)epth1 [21] |        22.934 ns |         8.7981 ns |       0.4823 ns |        22.832 ns |   0.0026 |        - |        - |      48 B |
| MergeIntoNew           | Node/(...)epth1 [21] |        51.283 ns |       352.1878 ns |      19.3046 ns |        42.268 ns |   0.0026 |        - |        - |      48 B |
| MergeIntoPopulated     | Node/(...)epth1 [21] |        32.682 ns |        10.1778 ns |       0.5579 ns |        32.916 ns |   0.0026 |        - |        - |      48 B |
| **New**                    | **Node/(...)epth3 [21]** |        **25.673 ns** |        **18.2953 ns** |       **1.0028 ns** |        **26.082 ns** |   **0.0026** |        **-** |        **-** |      **48 B** |
| ParseByteArray         | Node/(...)epth3 [21] |       309.979 ns |       102.1460 ns |       5.5990 ns |       313.171 ns |   0.0229 |        - |        - |     432 B |
| ParseSpan              | Node/(...)epth3 [21] |       226.194 ns |        40.1984 ns |       2.2034 ns |       226.398 ns |   0.0141 |        - |        - |     264 B |
| ToByteArray            | Node/(...)epth3 [21] |       208.780 ns |        52.9769 ns |       2.9038 ns |       209.121 ns |   0.0067 |        - |        - |     128 B |
| WriteToSpan            | Node/(...)epth3 [21] |       123.847 ns |         8.9215 ns |       0.4890 ns |       124.121 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Node/(...)epth3 [21] |       194.768 ns |        72.9068 ns |       3.9963 ns |       193.212 ns |   0.0017 |        - |        - |      32 B |
| CalculateSize          | Node/(...)epth3 [21] |        40.875 ns |         6.8257 ns |       0.3741 ns |        40.938 ns |        - |        - |        - |         - |
| Clone                  | Node/(...)epth3 [21] |        69.553 ns |       105.1418 ns |       5.7632 ns |        67.553 ns |   0.0076 |        - |        - |     144 B |
| MergeIntoNew           | Node/(...)epth3 [21] |        82.485 ns |        13.0009 ns |       0.7126 ns |        82.674 ns |   0.0076 |        - |        - |     144 B |
| MergeIntoPopulated     | Node/(...)epth3 [21] |        88.702 ns |        45.7248 ns |       2.5063 ns |        87.707 ns |   0.0076 |        - |        - |     144 B |
| **New**                    | **Node/(...)epth8 [21]** |        **26.402 ns** |        **12.5787 ns** |       **0.6895 ns** |        **26.100 ns** |   **0.0026** |        **-** |        **-** |      **48 B** |
| ParseByteArray         | Node/(...)epth8 [21] |       731.724 ns |       610.2168 ns |      33.4480 ns |       717.805 ns |   0.0458 |        - |        - |     872 B |
| ParseSpan              | Node/(...)epth8 [21] |       637.856 ns |        37.5184 ns |       2.0565 ns |       637.802 ns |   0.0372 |        - |        - |     704 B |
| ToByteArray            | Node/(...)epth8 [21] |       767.598 ns |        90.1895 ns |       4.9436 ns |       765.249 ns |   0.0095 |        - |        - |     192 B |
| WriteToSpan            | Node/(...)epth8 [21] |       585.617 ns |       168.2601 ns |       9.2229 ns |       585.821 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Node/(...)epth8 [21] |       748.833 ns |        69.5395 ns |       3.8117 ns |       747.425 ns |   0.0029 |        - |        - |      64 B |
| CalculateSize          | Node/(...)epth8 [21] |       108.632 ns |        19.1275 ns |       1.0484 ns |       108.245 ns |        - |        - |        - |         - |
| Clone                  | Node/(...)epth8 [21] |       157.840 ns |        11.3735 ns |       0.6234 ns |       157.547 ns |   0.0205 |        - |        - |     384 B |
| MergeIntoNew           | Node/(...)epth8 [21] |       181.101 ns |        55.6775 ns |       3.0519 ns |       181.501 ns |   0.0205 |        - |        - |     384 B |
| MergeIntoPopulated     | Node/(...)epth8 [21] |       207.786 ns |        31.2792 ns |       1.7145 ns |       206.933 ns |   0.0205 |        - |        - |     384 B |
| **New**                    | **Oneof(...)int32 [24]** |        **24.569 ns** |        **36.2507 ns** |       **1.9870 ns** |        **23.609 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)int32 [24] |       107.637 ns |        89.7264 ns |       4.9182 ns |       105.360 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Oneof(...)int32 [24] |        36.902 ns |         3.3793 ns |       0.1852 ns |        37.004 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Oneof(...)int32 [24] |        61.216 ns |        16.5384 ns |       0.9065 ns |        61.549 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof(...)int32 [24] |        12.694 ns |         3.5207 ns |       0.1930 ns |        12.594 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)int32 [24] |        27.081 ns |        31.4522 ns |       1.7240 ns |        26.143 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)int32 [24] |         5.706 ns |         4.0360 ns |       0.2212 ns |         5.581 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)int32 [24] |        28.832 ns |        10.4065 ns |       0.5704 ns |        28.846 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Oneof(...)int32 [24] |        48.036 ns |        22.5068 ns |       1.2337 ns |        48.738 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Oneof(...)int32 [24] |        45.829 ns |        29.1195 ns |       1.5961 ns |        46.194 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Oneof(...)ssage [26]** |        **22.833 ns** |         **3.5393 ns** |       **0.1940 ns** |        **22.870 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)ssage [26] |       120.867 ns |        46.2027 ns |       2.5325 ns |       121.467 ns |   0.0131 |        - |        - |     248 B |
| ParseSpan              | Oneof(...)ssage [26] |        96.934 ns |       897.5740 ns |      49.1991 ns |        75.987 ns |   0.0042 |        - |        - |      80 B |
| ToByteArray            | Oneof(...)ssage [26] |        68.741 ns |        51.6624 ns |       2.8318 ns |        67.813 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof(...)ssage [26] |        16.605 ns |         0.3658 ns |       0.0201 ns |        16.605 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)ssage [26] |        32.527 ns |         8.0748 ns |       0.4426 ns |        32.491 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)ssage [26] |         7.523 ns |         1.5079 ns |       0.0827 ns |         7.562 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)ssage [26] |        35.641 ns |         6.4034 ns |       0.3510 ns |        35.839 ns |   0.0042 |        - |        - |      80 B |
| MergeIntoNew           | Oneof(...)ssage [26] |        52.767 ns |        15.6659 ns |       0.8587 ns |        53.236 ns |   0.0042 |        - |        - |      80 B |
| MergeIntoPopulated     | Oneof(...)ssage [26] |        53.444 ns |        44.3171 ns |       2.4292 ns |        54.146 ns |   0.0042 |        - |        - |      80 B |
| **New**                    | **Oneof(...)tring [25]** |        **23.717 ns** |         **4.7136 ns** |       **0.2584 ns** |        **23.781 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)tring [25] |        95.772 ns |        34.7589 ns |       1.9053 ns |        95.497 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | Oneof(...)tring [25] |        34.031 ns |        11.6707 ns |       0.6397 ns |        34.306 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | Oneof(...)tring [25] |        78.358 ns |        19.4917 ns |       1.0684 ns |        78.500 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof(...)tring [25] |        22.019 ns |         5.4257 ns |       0.2974 ns |        21.858 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)tring [25] |        39.364 ns |        97.4656 ns |       5.3424 ns |        36.408 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)tring [25] |        12.595 ns |         5.2870 ns |       0.2898 ns |        12.508 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)tring [25] |        20.998 ns |         9.8351 ns |       0.5391 ns |        20.741 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Oneof(...)tring [25] |        34.824 ns |         3.1867 ns |       0.1747 ns |        34.857 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Oneof(...)tring [25] |        31.554 ns |         2.5349 ns |       0.1389 ns |        31.596 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Oneof/empty**          |        **24.514 ns** |        **32.6635 ns** |       **1.7904 ns** |        **23.547 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof/empty          |        88.190 ns |       106.1729 ns |       5.8197 ns |        85.226 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | Oneof/empty          |        23.267 ns |         6.8366 ns |       0.3747 ns |        23.395 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | Oneof/empty          |        56.249 ns |        17.9166 ns |       0.9821 ns |        56.782 ns |   0.0046 |        - |        - |      88 B |
| WriteToSpan            | Oneof/empty          |         9.569 ns |         5.2946 ns |       0.2902 ns |         9.464 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof/empty          |        13.515 ns |        11.7957 ns |       0.6466 ns |        13.371 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof/empty          |         4.167 ns |         0.2166 ns |       0.0119 ns |         4.169 ns |        - |        - |        - |         - |
| Clone                  | Oneof/empty          |        18.680 ns |         3.9968 ns |       0.2191 ns |        18.747 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Oneof/empty          |        32.082 ns |         3.5075 ns |       0.1923 ns |        32.011 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Oneof/empty          |        25.970 ns |         3.0227 ns |       0.1657 ns |        25.977 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Oneof/populated-bool** |        **22.804 ns** |        **10.2589 ns** |       **0.5623 ns** |        **22.621 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof/populated-bool |       110.761 ns |         5.0429 ns |       0.2764 ns |       110.622 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Oneof/populated-bool |        46.972 ns |        11.6209 ns |       0.6370 ns |        46.795 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Oneof/populated-bool |        87.284 ns |       372.2234 ns |      20.4028 ns |        93.304 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof/populated-bool |        15.008 ns |         6.6607 ns |       0.3651 ns |        15.045 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof/populated-bool |        30.277 ns |        10.7527 ns |       0.5894 ns |        30.495 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof/populated-bool |         5.923 ns |         2.7244 ns |       0.1493 ns |         5.880 ns |        - |        - |        - |         - |
| Clone                  | Oneof/populated-bool |        28.549 ns |         7.0773 ns |       0.3879 ns |        28.344 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Oneof/populated-bool |        43.252 ns |         8.7765 ns |       0.4811 ns |        43.297 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Oneof/populated-bool |        45.839 ns |        11.4982 ns |       0.6303 ns |        46.172 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Oneof(...)ouble [22]** |        **24.320 ns** |        **10.7246 ns** |       **0.5879 ns** |        **24.428 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)ouble [22] |       108.062 ns |        31.7232 ns |       1.7389 ns |       108.469 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Oneof(...)ouble [22] |        40.924 ns |         3.4036 ns |       0.1866 ns |        40.882 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Oneof(...)ouble [22] |        67.137 ns |        15.2959 ns |       0.8384 ns |        66.832 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Oneof(...)ouble [22] |        16.809 ns |         9.0286 ns |       0.4949 ns |        16.726 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)ouble [22] |        30.306 ns |         5.1686 ns |       0.2833 ns |        30.278 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)ouble [22] |         6.007 ns |         2.8746 ns |       0.1576 ns |         6.052 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)ouble [22] |        29.274 ns |        16.5299 ns |       0.9061 ns |        29.590 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Oneof(...)ouble [22] |        42.725 ns |        10.2479 ns |       0.5617 ns |        42.489 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Oneof(...)ouble [22] |        44.780 ns |        10.6449 ns |       0.5835 ns |        44.839 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Oneof(...)int32 [21]** |        **22.829 ns** |         **1.6112 ns** |       **0.0883 ns** |        **22.870 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)int32 [21] |       110.708 ns |        51.2253 ns |       2.8078 ns |       109.389 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Oneof(...)int32 [21] |        52.707 ns |        16.8280 ns |       0.9224 ns |        52.683 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Oneof(...)int32 [21] |        64.775 ns |        22.2911 ns |       1.2218 ns |        64.682 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof(...)int32 [21] |        15.549 ns |         1.8924 ns |       0.1037 ns |        15.496 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)int32 [21] |        28.010 ns |         9.4346 ns |       0.5171 ns |        28.205 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)int32 [21] |         6.444 ns |         7.7575 ns |       0.4252 ns |         6.266 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)int32 [21] |        28.710 ns |         8.8103 ns |       0.4829 ns |        28.675 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Oneof(...)int32 [21] |        43.332 ns |         8.2419 ns |       0.4518 ns |        43.521 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Oneof(...)int32 [21] |        45.104 ns |         8.4372 ns |       0.4625 ns |        44.893 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Oneof(...)int64 [21]** |        **24.988 ns** |        **14.9351 ns** |       **0.8186 ns** |        **25.162 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)int64 [21] |       117.656 ns |        29.0467 ns |       1.5921 ns |       118.340 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Oneof(...)int64 [21] |        54.529 ns |         8.6281 ns |       0.4729 ns |        54.533 ns |   0.0033 |        - |        - |      64 B |
| ToByteArray            | Oneof(...)int64 [21] |        77.774 ns |        11.6695 ns |       0.6396 ns |        77.641 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Oneof(...)int64 [21] |        23.244 ns |         3.3651 ns |       0.1845 ns |        23.295 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)int64 [21] |        38.396 ns |        13.9961 ns |       0.7672 ns |        38.083 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)int64 [21] |         8.083 ns |         1.0817 ns |       0.0593 ns |         8.055 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)int64 [21] |        27.779 ns |         7.2354 ns |       0.3966 ns |        27.886 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Oneof(...)int64 [21] |        45.355 ns |        35.3684 ns |       1.9387 ns |        44.959 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Oneof(...)int64 [21] |        47.067 ns |        34.4108 ns |       1.8862 ns |        46.032 ns |   0.0046 |        - |        - |      88 B |
| **New**                    | **Oneof(...)ssage [23]** |        **22.708 ns** |         **2.8974 ns** |       **0.1588 ns** |        **22.638 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)ssage [23] |       169.285 ns |        60.5999 ns |       3.3217 ns |       170.326 ns |   0.0153 |        - |        - |     288 B |
| ParseSpan              | Oneof(...)ssage [23] |       102.812 ns |        20.3906 ns |       1.1177 ns |       102.902 ns |   0.0063 |        - |        - |     120 B |
| ToByteArray            | Oneof(...)ssage [23] |       116.776 ns |        30.2025 ns |       1.6555 ns |       116.854 ns |   0.0060 |        - |        - |     112 B |
| WriteToSpan            | Oneof(...)ssage [23] |        56.051 ns |         2.2137 ns |       0.1213 ns |        56.089 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)ssage [23] |        68.942 ns |        15.2413 ns |       0.8354 ns |        68.723 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)ssage [23] |        18.699 ns |         3.3913 ns |       0.1859 ns |        18.604 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)ssage [23] |        36.034 ns |        15.6017 ns |       0.8552 ns |        35.774 ns |   0.0042 |        - |        - |      80 B |
| MergeIntoNew           | Oneof(...)ssage [23] |        53.758 ns |        12.6715 ns |       0.6946 ns |        53.889 ns |   0.0042 |        - |        - |      80 B |
| MergeIntoPopulated     | Oneof(...)ssage [23] |        48.606 ns |         9.0176 ns |       0.4943 ns |        48.402 ns |   0.0042 |        - |        - |      80 B |
| **New**                    | **Oneof(...)tring [22]** |        **24.676 ns** |        **57.9421 ns** |       **3.1760 ns** |        **22.989 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)tring [22] |       134.668 ns |        98.9869 ns |       5.4258 ns |       132.297 ns |   0.0131 |        - |        - |     248 B |
| ParseSpan              | Oneof(...)tring [22] |        69.487 ns |        25.5039 ns |       1.3980 ns |        68.852 ns |   0.0042 |        - |        - |      80 B |
| ToByteArray            | Oneof(...)tring [22] |        88.746 ns |        25.2321 ns |       1.3831 ns |        88.580 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Oneof(...)tring [22] |        29.998 ns |         8.8240 ns |       0.4837 ns |        29.936 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)tring [22] |        41.192 ns |         7.1753 ns |       0.3933 ns |        41.107 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)tring [22] |        16.012 ns |         4.5090 ns |       0.2472 ns |        15.895 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)tring [22] |        28.193 ns |       182.2792 ns |       9.9913 ns |        24.579 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Oneof(...)tring [22] |        35.384 ns |        30.4058 ns |       1.6666 ns |        34.551 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Oneof(...)tring [22] |        31.459 ns |         8.8209 ns |       0.4835 ns |        31.349 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **OneofPayload/edge**    |        **23.861 ns** |         **8.3497 ns** |       **0.4577 ns** |        **23.911 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | OneofPayload/edge    |       247.417 ns |        50.5460 ns |       2.7706 ns |       248.478 ns |   0.0153 |        - |        - |     288 B |
| ParseSpan              | OneofPayload/edge    |       172.598 ns |        23.8868 ns |       1.3093 ns |       172.211 ns |   0.0062 |        - |        - |     120 B |
| ToByteArray            | OneofPayload/edge    |       183.324 ns |         5.4216 ns |       0.2972 ns |       183.290 ns |   0.0081 |        - |        - |     152 B |
| WriteToSpan            | OneofPayload/edge    |       162.360 ns |       982.3929 ns |      53.8483 ns |       136.314 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | OneofPayload/edge    |       164.425 ns |       162.1439 ns |       8.8877 ns |       159.365 ns |   0.0038 |        - |        - |      72 B |
| CalculateSize          | OneofPayload/edge    |        21.683 ns |         9.4159 ns |       0.5161 ns |        21.608 ns |        - |        - |        - |         - |
| Clone                  | OneofPayload/edge    |        20.090 ns |         6.6131 ns |       0.3625 ns |        20.014 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | OneofPayload/edge    |        33.829 ns |        13.7883 ns |       0.7558 ns |        33.754 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | OneofPayload/edge    |        27.237 ns |        12.0532 ns |       0.6607 ns |        26.909 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **OneofPayload/empty**   |        **23.351 ns** |         **7.8110 ns** |       **0.4281 ns** |        **23.567 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | OneofPayload/empty   |        83.946 ns |        23.1942 ns |       1.2714 ns |        84.450 ns |   0.0110 |        - |        - |     208 B |
| ParseSpan              | OneofPayload/empty   |        22.054 ns |         1.3510 ns |       0.0741 ns |        22.031 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | OneofPayload/empty   |        50.443 ns |         6.3610 ns |       0.3487 ns |        50.411 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | OneofPayload/empty   |         5.741 ns |         1.8296 ns |       0.1003 ns |         5.697 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | OneofPayload/empty   |         8.945 ns |         8.4880 ns |       0.4653 ns |         8.782 ns |        - |        - |        - |         - |
| CalculateSize          | OneofPayload/empty   |         2.313 ns |         2.4417 ns |       0.1338 ns |         2.309 ns |        - |        - |        - |         - |
| Clone                  | OneofPayload/empty   |        19.413 ns |         7.6295 ns |       0.4182 ns |        19.634 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | OneofPayload/empty   |        34.235 ns |        21.9012 ns |       1.2005 ns |        34.499 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | OneofPayload/empty   |        27.198 ns |        31.6690 ns |       1.7359 ns |        26.575 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Oneof(...)lated [22]** |        **23.786 ns** |        **13.3007 ns** |       **0.7291 ns** |        **23.575 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)lated [22] |       128.217 ns |        15.8338 ns |       0.8679 ns |       128.380 ns |   0.0126 |        - |        - |     240 B |
| ParseSpan              | Oneof(...)lated [22] |        63.422 ns |        10.6287 ns |       0.5826 ns |        63.392 ns |   0.0038 |        - |        - |      72 B |
| ToByteArray            | Oneof(...)lated [22] |        77.914 ns |         8.6845 ns |       0.4760 ns |        78.142 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof(...)lated [22] |        22.364 ns |         6.9606 ns |       0.3815 ns |        22.458 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)lated [22] |        33.345 ns |        15.4108 ns |       0.8447 ns |        33.057 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)lated [22] |        12.238 ns |         2.4413 ns |       0.1338 ns |        12.165 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)lated [22] |        21.325 ns |        16.2087 ns |       0.8885 ns |        21.080 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Oneof(...)lated [22] |        35.587 ns |        34.4629 ns |       1.8890 ns |        34.859 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Oneof(...)lated [22] |        26.488 ns |        13.3233 ns |       0.7303 ns |        26.149 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Optio(...)esent [30]** |        **29.576 ns** |         **6.4924 ns** |       **0.3559 ns** |        **29.377 ns** |   **0.0038** |        **-** |        **-** |      **72 B** |
| ParseByteArray         | Optio(...)esent [30] |       204.108 ns |       159.8639 ns |       8.7627 ns |       199.393 ns |   0.0157 |        - |        - |     296 B |
| ParseSpan              | Optio(...)esent [30] |       136.010 ns |       100.4812 ns |       5.5077 ns |       136.041 ns |   0.0067 |        - |        - |     128 B |
| ToByteArray            | Optio(...)esent [30] |       113.763 ns |        11.0005 ns |       0.6030 ns |       113.995 ns |   0.0060 |        - |        - |     112 B |
| WriteToSpan            | Optio(...)esent [30] |        51.715 ns |        13.5572 ns |       0.7431 ns |        51.908 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Optio(...)esent [30] |        62.586 ns |         9.1626 ns |       0.5022 ns |        62.602 ns |        - |        - |        - |         - |
| CalculateSize          | Optio(...)esent [30] |        13.510 ns |        10.7709 ns |       0.5904 ns |        13.279 ns |        - |        - |        - |         - |
| Clone                  | Optio(...)esent [30] |        29.326 ns |        38.6652 ns |       2.1194 ns |        29.473 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoNew           | Optio(...)esent [30] |        44.964 ns |         3.6366 ns |       0.1993 ns |        45.012 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoPopulated     | Optio(...)esent [30] |        44.960 ns |        58.1769 ns |       3.1889 ns |        46.210 ns |   0.0038 |        - |        - |      72 B |
| **New**                    | **Optional/empty**       |        **31.057 ns** |        **17.8770 ns** |       **0.9799 ns** |        **30.699 ns** |   **0.0038** |        **-** |        **-** |      **72 B** |
| ParseByteArray         | Optional/empty       |        93.833 ns |        65.8025 ns |       3.6069 ns |        94.576 ns |   0.0128 |        - |        - |     240 B |
| ParseSpan              | Optional/empty       |        29.478 ns |        14.5727 ns |       0.7988 ns |        29.118 ns |   0.0038 |        - |        - |      72 B |
| ToByteArray            | Optional/empty       |        56.622 ns |        21.2107 ns |       1.1626 ns |        57.036 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Optional/empty       |         7.990 ns |         4.0439 ns |       0.2217 ns |         7.958 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Optional/empty       |        13.743 ns |        54.7609 ns |       3.0016 ns |        13.060 ns |        - |        - |        - |         - |
| CalculateSize          | Optional/empty       |         4.837 ns |         3.5415 ns |       0.1941 ns |         4.791 ns |        - |        - |        - |         - |
| Clone                  | Optional/empty       |        27.920 ns |         9.0103 ns |       0.4939 ns |        27.710 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoNew           | Optional/empty       |        39.381 ns |         3.2005 ns |       0.1754 ns |        39.444 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoPopulated     | Optional/empty       |        36.836 ns |        20.3002 ns |       1.1127 ns |        36.844 ns |   0.0038 |        - |        - |      72 B |
| **New**                    | **Optional/populated**   |        **30.634 ns** |        **30.3689 ns** |       **1.6646 ns** |        **30.194 ns** |   **0.0038** |        **-** |        **-** |      **72 B** |
| ParseByteArray         | Optional/populated   |       249.043 ns |       243.6477 ns |      13.3552 ns |       249.747 ns |   0.0196 |        - |        - |     368 B |
| ParseSpan              | Optional/populated   |       182.703 ns |       178.3065 ns |       9.7736 ns |       177.411 ns |   0.0105 |        - |        - |     200 B |
| ToByteArray            | Optional/populated   |       149.428 ns |       110.4211 ns |       6.0526 ns |       149.003 ns |   0.0083 |        - |        - |     160 B |
| WriteToSpan            | Optional/populated   |        64.159 ns |        19.0010 ns |       1.0415 ns |        63.957 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Optional/populated   |       118.890 ns |        59.8547 ns |       3.2808 ns |       119.832 ns |        - |        - |        - |         - |
| CalculateSize          | Optional/populated   |        20.619 ns |        15.0407 ns |       0.8244 ns |        21.065 ns |        - |        - |        - |         - |
| Clone                  | Optional/populated   |        27.859 ns |         9.4195 ns |       0.5163 ns |        27.732 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoNew           | Optional/populated   |        47.733 ns |        16.4387 ns |       0.9011 ns |        47.699 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoPopulated     | Optional/populated   |        40.537 ns |        48.8409 ns |       2.6771 ns |        39.143 ns |   0.0038 |        - |        - |      72 B |
| **New**                    | **Repeated/edge**        |       **106.646 ns** |        **40.1257 ns** |       **2.1994 ns** |       **107.264 ns** |   **0.0141** |        **-** |        **-** |     **264 B** |
| ParseByteArray         | Repeated/edge        |     1,003.094 ns |     1,485.2838 ns |      81.4134 ns |       966.488 ns |   0.0553 |        - |        - |    1056 B |
| ParseSpan              | Repeated/edge        |       889.086 ns |       370.3652 ns |      20.3010 ns |       878.937 ns |   0.0467 |        - |        - |     888 B |
| ToByteArray            | Repeated/edge        |       626.372 ns |        47.4632 ns |       2.6016 ns |       625.180 ns |   0.0134 |        - |        - |     256 B |
| WriteToSpan            | Repeated/edge        |       433.125 ns |       149.9145 ns |       8.2173 ns |       432.127 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Repeated/edge        |       576.311 ns |        85.7914 ns |       4.7025 ns |       574.566 ns |   0.0038 |        - |        - |      72 B |
| CalculateSize          | Repeated/edge        |       113.625 ns |       181.7041 ns |       9.9598 ns |       116.872 ns |        - |        - |        - |         - |
| Clone                  | Repeated/edge        |       839.960 ns |       704.3436 ns |      38.6074 ns |       821.789 ns |   0.0534 |        - |        - |    1000 B |
| MergeIntoNew           | Repeated/edge        |       368.915 ns |       115.7885 ns |       6.3468 ns |       369.453 ns |   0.0386 |        - |        - |     728 B |
| MergeIntoPopulated     | Repeated/edge        |       892.807 ns |       389.9889 ns |      21.3766 ns |       885.442 ns |   0.0534 |        - |        - |    1000 B |
| **New**                    | **Repeated/empty**       |       **110.682 ns** |       **115.9144 ns** |       **6.3537 ns** |       **109.490 ns** |   **0.0141** |        **-** |        **-** |     **264 B** |
| ParseByteArray         | Repeated/empty       |       189.921 ns |        77.6326 ns |       4.2553 ns |       189.947 ns |   0.0229 |        - |        - |     432 B |
| ParseSpan              | Repeated/empty       |       114.342 ns |        11.3647 ns |       0.6229 ns |       114.112 ns |   0.0141 |        - |        - |     264 B |
| ToByteArray            | Repeated/empty       |        60.697 ns |        55.4553 ns |       3.0397 ns |        60.073 ns |   0.0046 |        - |        - |      88 B |
| WriteToSpan            | Repeated/empty       |        22.231 ns |         2.6032 ns |       0.1427 ns |        22.212 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Repeated/empty       |        26.072 ns |        15.4257 ns |       0.8455 ns |        25.710 ns |        - |        - |        - |         - |
| CalculateSize          | Repeated/empty       |         6.924 ns |         6.1389 ns |       0.3365 ns |         6.771 ns |        - |        - |        - |         - |
| Clone                  | Repeated/empty       |       171.987 ns |        36.6139 ns |       2.0069 ns |       172.040 ns |   0.0243 |        - |        - |     456 B |
| MergeIntoNew           | Repeated/empty       |       176.342 ns |       236.1240 ns |      12.9428 ns |       169.477 ns |   0.0141 |        - |        - |     264 B |
| MergeIntoPopulated     | Repeated/empty       |       237.845 ns |       203.4157 ns |      11.1499 ns |       232.118 ns |   0.0243 |        - |        - |     456 B |
| **New**                    | **Repeated/populated**   |       **123.917 ns** |       **406.7537 ns** |      **22.2955 ns** |       **111.146 ns** |   **0.0141** |        **-** |        **-** |     **264 B** |
| ParseByteArray         | Repeated/populated   |     6,333.376 ns |     1,675.9094 ns |      91.8623 ns |     6,294.387 ns |   0.4730 |        - |        - |    9024 B |
| ParseSpan              | Repeated/populated   |     6,339.100 ns |     1,093.2185 ns |      59.9230 ns |     6,331.850 ns |   0.4730 |   0.0076 |        - |    8856 B |
| ToByteArray            | Repeated/populated   |     5,141.185 ns |       975.8611 ns |      53.4902 ns |     5,132.772 ns |   0.1297 |        - |        - |    2512 B |
| WriteToSpan            | Repeated/populated   |     3,161.182 ns |       364.8529 ns |      19.9988 ns |     3,171.793 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Repeated/populated   |     5,071.781 ns |       789.0813 ns |      43.2522 ns |     5,052.740 ns |   0.0076 |        - |        - |     256 B |
| CalculateSize          | Repeated/populated   |     1,440.653 ns |       216.7575 ns |      11.8812 ns |     1,447.337 ns |        - |        - |        - |         - |
| Clone                  | Repeated/populated   |     1,896.083 ns |       773.7858 ns |      42.4138 ns |     1,884.465 ns |   0.2632 |   0.0038 |        - |    4984 B |
| MergeIntoNew           | Repeated/populated   |       862.164 ns |       297.6317 ns |      16.3142 ns |       865.993 ns |   0.1669 |   0.0019 |        - |    3128 B |
| MergeIntoPopulated     | Repeated/populated   |     3,736.920 ns |     1,236.1902 ns |      67.7598 ns |     3,768.704 ns |   0.6561 |   0.0153 |        - |   12296 B |
| **New**                    | **RepeatedItem/edge**    |        **23.319 ns** |        **10.4603 ns** |       **0.5734 ns** |        **23.645 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | RepeatedItem/edge    |       240.915 ns |        40.9542 ns |       2.2448 ns |       241.715 ns |   0.0153 |        - |        - |     288 B |
| ParseSpan              | RepeatedItem/edge    |       171.978 ns |        26.0831 ns |       1.4297 ns |       171.433 ns |   0.0062 |        - |        - |     120 B |
| ToByteArray            | RepeatedItem/edge    |       185.207 ns |        43.4363 ns |       2.3809 ns |       186.003 ns |   0.0081 |        - |        - |     152 B |
| WriteToSpan            | RepeatedItem/edge    |       112.488 ns |        65.8798 ns |       3.6111 ns |       111.843 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | RepeatedItem/edge    |       171.761 ns |       240.1449 ns |      13.1632 ns |       166.443 ns |   0.0038 |        - |        - |      72 B |
| CalculateSize          | RepeatedItem/edge    |        21.685 ns |        11.3877 ns |       0.6242 ns |        21.781 ns |        - |        - |        - |         - |
| Clone                  | RepeatedItem/edge    |        20.269 ns |         4.9248 ns |       0.2699 ns |        20.219 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | RepeatedItem/edge    |        34.926 ns |         4.4410 ns |       0.2434 ns |        34.968 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | RepeatedItem/edge    |        26.625 ns |         2.3141 ns |       0.1268 ns |        26.571 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **RepeatedItem/empty**   |        **24.654 ns** |         **7.2916 ns** |       **0.3997 ns** |        **24.555 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | RepeatedItem/empty   |        93.637 ns |        73.8159 ns |       4.0461 ns |        94.874 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | RepeatedItem/empty   |        39.087 ns |       483.0804 ns |      26.4793 ns |        24.016 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | RepeatedItem/empty   |        52.241 ns |        27.9597 ns |       1.5326 ns |        51.643 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | RepeatedItem/empty   |         6.048 ns |         3.0803 ns |       0.1688 ns |         5.983 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | RepeatedItem/empty   |         8.546 ns |         3.4883 ns |       0.1912 ns |         8.449 ns |        - |        - |        - |         - |
| CalculateSize          | RepeatedItem/empty   |         2.191 ns |         0.9459 ns |       0.0518 ns |         2.211 ns |        - |        - |        - |         - |
| Clone                  | RepeatedItem/empty   |        21.137 ns |        12.0193 ns |       0.6588 ns |        21.127 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | RepeatedItem/empty   |        32.976 ns |        34.0495 ns |       1.8664 ns |        32.331 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | RepeatedItem/empty   |        27.551 ns |        15.6619 ns |       0.8585 ns |        27.809 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Repea(...)lated [22]** |        **24.861 ns** |        **12.1969 ns** |       **0.6686 ns** |        **25.165 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Repea(...)lated [22] |       131.018 ns |        42.9411 ns |       2.3537 ns |       129.850 ns |   0.0126 |        - |        - |     240 B |
| ParseSpan              | Repea(...)lated [22] |        68.930 ns |        69.4580 ns |       3.8072 ns |        66.783 ns |   0.0038 |        - |        - |      72 B |
| ToByteArray            | Repea(...)lated [22] |        81.501 ns |        22.6672 ns |       1.2425 ns |        81.118 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Repea(...)lated [22] |        23.241 ns |         8.1264 ns |       0.4454 ns |        23.393 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Repea(...)lated [22] |        42.752 ns |       153.9681 ns |       8.4395 ns |        41.155 ns |        - |        - |        - |         - |
| CalculateSize          | Repea(...)lated [22] |        12.030 ns |         5.8322 ns |       0.3197 ns |        11.910 ns |        - |        - |        - |         - |
| Clone                  | Repea(...)lated [22] |        19.977 ns |        11.0417 ns |       0.6052 ns |        19.813 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Repea(...)lated [22] |        34.031 ns |         7.7833 ns |       0.4266 ns |        34.113 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Repea(...)lated [22] |        26.320 ns |         5.9499 ns |       0.3261 ns |        26.245 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Scalars/edge-max**     |        **35.832 ns** |        **10.4350 ns** |       **0.5720 ns** |        **36.049 ns** |   **0.0055** |        **-** |        **-** |     **104 B** |
| ParseByteArray         | Scalars/edge-max     |       272.242 ns |        62.9359 ns |       3.4497 ns |       271.089 ns |   0.0143 |        - |        - |     272 B |
| ParseSpan              | Scalars/edge-max     |       197.258 ns |        22.8759 ns |       1.2539 ns |       197.634 ns |   0.0052 |        - |        - |     104 B |
| ToByteArray            | Scalars/edge-max     |       223.286 ns |        61.5576 ns |       3.3742 ns |       223.294 ns |   0.0098 |        - |        - |     184 B |
| WriteToSpan            | Scalars/edge-max     |       145.513 ns |        23.6057 ns |       1.2939 ns |       144.996 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Scalars/edge-max     |       163.467 ns |        12.3233 ns |       0.6755 ns |       163.551 ns |        - |        - |        - |         - |
| CalculateSize          | Scalars/edge-max     |        22.546 ns |         0.9884 ns |       0.0542 ns |        22.528 ns |        - |        - |        - |         - |
| Clone                  | Scalars/edge-max     |        31.358 ns |         6.9874 ns |       0.3830 ns |        31.174 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoNew           | Scalars/edge-max     |        48.137 ns |         7.3543 ns |       0.4031 ns |        47.944 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoPopulated     | Scalars/edge-max     |        46.177 ns |        58.8559 ns |       3.2261 ns |        45.601 ns |   0.0055 |        - |        - |     104 B |
| **New**                    | **Scalars/edge-min**     |        **35.010 ns** |         **6.1179 ns** |       **0.3353 ns** |        **34.912 ns** |   **0.0055** |        **-** |        **-** |     **104 B** |
| ParseByteArray         | Scalars/edge-min     |       227.376 ns |        62.4432 ns |       3.4227 ns |       226.780 ns |   0.0145 |        - |        - |     272 B |
| ParseSpan              | Scalars/edge-min     |       170.521 ns |        68.6094 ns |       3.7607 ns |       169.576 ns |   0.0055 |        - |        - |     104 B |
| ToByteArray            | Scalars/edge-min     |       199.826 ns |        43.6678 ns |       2.3936 ns |       200.475 ns |   0.0088 |        - |        - |     168 B |
| WriteToSpan            | Scalars/edge-min     |       123.993 ns |        40.7429 ns |       2.2333 ns |       123.035 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Scalars/edge-min     |       159.749 ns |         7.5396 ns |       0.4133 ns |       159.967 ns |        - |        - |        - |         - |
| CalculateSize          | Scalars/edge-min     |        21.768 ns |       104.1507 ns |       5.7088 ns |        20.360 ns |        - |        - |        - |         - |
| Clone                  | Scalars/edge-min     |        32.322 ns |         7.4780 ns |       0.4099 ns |        32.099 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoNew           | Scalars/edge-min     |        50.328 ns |        44.7438 ns |       2.4526 ns |        49.179 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoPopulated     | Scalars/edge-min     |        44.092 ns |         1.1707 ns |       0.0642 ns |        44.087 ns |   0.0055 |        - |        - |     104 B |
| **New**                    | **Scalars/empty**        |        **34.562 ns** |        **23.1239 ns** |       **1.2675 ns** |        **34.136 ns** |   **0.0055** |        **-** |        **-** |     **104 B** |
| ParseByteArray         | Scalars/empty        |       103.065 ns |        42.4990 ns |       2.3295 ns |       102.605 ns |   0.0145 |        - |        - |     272 B |
| ParseSpan              | Scalars/empty        |        36.376 ns |         5.4202 ns |       0.2971 ns |        36.486 ns |   0.0055 |        - |        - |     104 B |
| ToByteArray            | Scalars/empty        |        60.362 ns |        55.4900 ns |       3.0416 ns |        58.730 ns |   0.0046 |        - |        - |      88 B |
| WriteToSpan            | Scalars/empty        |        12.182 ns |         3.7169 ns |       0.2037 ns |        12.116 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Scalars/empty        |        13.411 ns |         8.0504 ns |       0.4413 ns |        13.410 ns |        - |        - |        - |         - |
| CalculateSize          | Scalars/empty        |         7.197 ns |         1.3538 ns |       0.0742 ns |         7.185 ns |        - |        - |        - |         - |
| Clone                  | Scalars/empty        |        31.733 ns |         6.2335 ns |       0.3417 ns |        31.576 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoNew           | Scalars/empty        |        47.130 ns |        12.9724 ns |       0.7111 ns |        46.745 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoPopulated     | Scalars/empty        |        57.939 ns |       368.8689 ns |      20.2189 ns |        47.104 ns |   0.0055 |        - |        - |     104 B |
| **New**                    | **Scalars/populated**    |        **35.394 ns** |        **21.0275 ns** |       **1.1526 ns** |        **35.122 ns** |   **0.0055** |        **-** |        **-** |     **104 B** |
| ParseByteArray         | Scalars/populated    |       243.696 ns |        61.5666 ns |       3.3747 ns |       242.816 ns |   0.0143 |        - |        - |     272 B |
| ParseSpan              | Scalars/populated    |       168.128 ns |        21.5621 ns |       1.1819 ns |       168.289 ns |   0.0055 |        - |        - |     104 B |
| ToByteArray            | Scalars/populated    |       189.664 ns |        36.5948 ns |       2.0059 ns |       188.985 ns |   0.0093 |        - |        - |     176 B |
| WriteToSpan            | Scalars/populated    |       111.472 ns |        31.4774 ns |       1.7254 ns |       110.989 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Scalars/populated    |       136.774 ns |        29.3026 ns |       1.6062 ns |       136.095 ns |        - |        - |        - |         - |
| CalculateSize          | Scalars/populated    |        19.960 ns |         0.9658 ns |       0.0529 ns |        19.948 ns |        - |        - |        - |         - |
| Clone                  | Scalars/populated    |        31.410 ns |         9.0338 ns |       0.4952 ns |        31.589 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoNew           | Scalars/populated    |        48.082 ns |        22.3095 ns |       1.2229 ns |        47.583 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoPopulated     | Scalars/populated    |        45.509 ns |         7.9920 ns |       0.4381 ns |        45.739 ns |   0.0055 |        - |        - |     104 B |
| **New**                    | **Strings/edge**         |        **27.679 ns** |         **2.1481 ns** |       **0.1177 ns** |        **27.675 ns** |   **0.0030** |        **-** |        **-** |      **56 B** |
| ParseByteArray         | Strings/edge         |   606,048.948 ns |   327,524.1852 ns |  17,952.7058 ns |   605,066.895 ns | 248.0469 | 248.0469 | 248.0469 | 2098069 B |
| ParseSpan              | Strings/edge         |   662,397.107 ns | 1,241,367.6392 ns |  68,043.5494 ns |   679,372.915 ns | 248.0469 | 248.0469 | 248.0469 | 2097901 B |
| ToByteArray            | Strings/edge         |   699,160.386 ns |   426,995.7417 ns |  23,405.0775 ns |   688,177.312 ns | 178.7109 | 178.7109 | 178.7109 | 1050695 B |
| WriteToSpan            | Strings/edge         |   455,228.873 ns | 2,301,639.7470 ns | 126,160.6416 ns |   491,341.479 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Strings/edge         | 1,365,086.409 ns | 1,427,157.1555 ns |  78,227.2997 ns | 1,338,328.287 ns | 160.1563 | 160.1563 | 160.1563 | 1049186 B |
| CalculateSize          | Strings/edge         |    78,072.655 ns |    35,595.7418 ns |   1,951.1227 ns |    77,933.731 ns |        - |        - |        - |         - |
| Clone                  | Strings/edge         |        38.275 ns |       177.3000 ns |       9.7184 ns |        34.778 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoNew           | Strings/edge         |        46.388 ns |         8.2438 ns |       0.4519 ns |        46.531 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoPopulated     | Strings/edge         |        47.959 ns |         5.6867 ns |       0.3117 ns |        48.103 ns |   0.0030 |        - |        - |      56 B |
| **New**                    | **Strings/empty**        |        **25.966 ns** |        **11.4540 ns** |       **0.6278 ns** |        **25.684 ns** |   **0.0030** |        **-** |        **-** |      **56 B** |
| ParseByteArray         | Strings/empty        |        95.721 ns |       184.6912 ns |      10.1235 ns |        89.991 ns |   0.0119 |        - |        - |     224 B |
| ParseSpan              | Strings/empty        |        25.289 ns |         8.3457 ns |       0.4575 ns |        25.260 ns |   0.0030 |        - |        - |      56 B |
| ToByteArray            | Strings/empty        |        50.497 ns |         8.5472 ns |       0.4685 ns |        50.468 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Strings/empty        |         6.511 ns |         0.9189 ns |       0.0504 ns |         6.537 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Strings/empty        |         9.277 ns |         0.7863 ns |       0.0431 ns |         9.289 ns |        - |        - |        - |         - |
| CalculateSize          | Strings/empty        |         3.154 ns |         2.2727 ns |       0.1246 ns |         3.120 ns |        - |        - |        - |         - |
| Clone                  | Strings/empty        |        30.073 ns |        10.1462 ns |       0.5561 ns |        30.091 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoNew           | Strings/empty        |        36.721 ns |         5.3366 ns |       0.2925 ns |        36.779 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoPopulated     | Strings/empty        |        37.062 ns |        14.3559 ns |       0.7869 ns |        36.615 ns |   0.0030 |        - |        - |      56 B |
| **New**                    | **Strings/populated**    |        **27.521 ns** |        **14.0626 ns** |       **0.7708 ns** |        **27.795 ns** |   **0.0030** |        **-** |        **-** |      **56 B** |
| ParseByteArray         | Strings/populated    |     7,539.786 ns |     2,419.5560 ns |     132.6240 ns |     7,578.919 ns |   1.7853 |   0.1831 |        - |   33664 B |
| ParseSpan              | Strings/populated    |     7,528.437 ns |     6,979.3469 ns |     382.5616 ns |     7,393.555 ns |   1.7853 |   0.1907 |        - |   33496 B |
| ToByteArray            | Strings/populated    |     8,241.441 ns |     4,220.7627 ns |     231.3542 ns |     8,116.715 ns |   0.8850 |        - |        - |   16792 B |
| WriteToSpan            | Strings/populated    |     4,257.437 ns |     3,057.4400 ns |     167.5886 ns |     4,205.551 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Strings/populated    |    31,606.158 ns |   282,338.7021 ns |  15,475.9370 ns |    28,190.795 ns |   0.8850 |        - |        - |   16760 B |
| CalculateSize          | Strings/populated    |       904.407 ns |       138.2474 ns |       7.5778 ns |       902.490 ns |        - |        - |        - |         - |
| Clone                  | Strings/populated    |        30.300 ns |         3.9613 ns |       0.2171 ns |        30.235 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoNew           | Strings/populated    |        48.447 ns |        67.0191 ns |       3.6735 ns |        48.645 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoPopulated     | Strings/populated    |        42.618 ns |        14.6289 ns |       0.8019 ns |        42.647 ns |   0.0030 |        - |        - |      56 B |
| **New**                    | **Unkno(...)/edge [22]** |        **23.073 ns** |         **9.0085 ns** |       **0.4938 ns** |        **22.884 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Unkno(...)/edge [22] |       250.008 ns |        62.9592 ns |       3.4510 ns |       250.691 ns |   0.0153 |        - |        - |     288 B |
| ParseSpan              | Unkno(...)/edge [22] |       178.794 ns |        77.9760 ns |       4.2741 ns |       179.821 ns |   0.0062 |        - |        - |     120 B |
| ToByteArray            | Unkno(...)/edge [22] |       182.128 ns |        62.1294 ns |       3.4055 ns |       181.504 ns |   0.0081 |        - |        - |     152 B |
| WriteToSpan            | Unkno(...)/edge [22] |       108.147 ns |         2.7171 ns |       0.1489 ns |       108.174 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)/edge [22] |       163.143 ns |        56.3594 ns |       3.0892 ns |       163.515 ns |   0.0038 |        - |        - |      72 B |
| CalculateSize          | Unkno(...)/edge [22] |        21.826 ns |         5.8837 ns |       0.3225 ns |        21.779 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)/edge [22] |        18.909 ns |         1.7814 ns |       0.0976 ns |        18.888 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Unkno(...)/edge [22] |        53.434 ns |       505.2632 ns |      27.6952 ns |        39.951 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Unkno(...)/edge [22] |        27.595 ns |         8.5433 ns |       0.4683 ns |        27.754 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Unkno(...)empty [23]** |        **23.114 ns** |        **10.5990 ns** |       **0.5810 ns** |        **22.871 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Unkno(...)empty [23] |        89.849 ns |        36.8579 ns |       2.0203 ns |        88.999 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | Unkno(...)empty [23] |        22.614 ns |        13.9257 ns |       0.7633 ns |        22.627 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | Unkno(...)empty [23] |        51.605 ns |         3.7280 ns |       0.2043 ns |        51.512 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Unkno(...)empty [23] |         5.809 ns |         0.9208 ns |       0.0505 ns |         5.830 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)empty [23] |         8.826 ns |         6.1412 ns |       0.3366 ns |         8.726 ns |        - |        - |        - |         - |
| CalculateSize          | Unkno(...)empty [23] |         2.002 ns |         0.3937 ns |       0.0216 ns |         1.996 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)empty [23] |        20.091 ns |         2.5894 ns |       0.1419 ns |        20.169 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Unkno(...)empty [23] |        33.436 ns |         9.1713 ns |       0.5027 ns |        33.223 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Unkno(...)empty [23] |        26.156 ns |         1.9683 ns |       0.1079 ns |        26.128 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Unkno(...)lated [27]** |        **24.696 ns** |        **18.1843 ns** |       **0.9967 ns** |        **24.135 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Unkno(...)lated [27] |       139.822 ns |       156.7244 ns |       8.5906 ns |       136.565 ns |   0.0126 |        - |        - |     240 B |
| ParseSpan              | Unkno(...)lated [27] |        69.910 ns |        58.2487 ns |       3.1928 ns |        68.594 ns |   0.0038 |        - |        - |      72 B |
| ToByteArray            | Unkno(...)lated [27] |        80.893 ns |        15.6381 ns |       0.8572 ns |        80.484 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Unkno(...)lated [27] |        23.793 ns |         4.4338 ns |       0.2430 ns |        23.772 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)lated [27] |        33.283 ns |         2.4013 ns |       0.1316 ns |        33.316 ns |        - |        - |        - |         - |
| CalculateSize          | Unkno(...)lated [27] |        12.248 ns |         4.3450 ns |       0.2382 ns |        12.376 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)lated [27] |        19.738 ns |         6.2059 ns |       0.3402 ns |        19.651 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Unkno(...)lated [27] |        35.204 ns |        15.0546 ns |       0.8252 ns |        35.094 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Unkno(...)lated [27] |        28.011 ns |         5.2587 ns |       0.2882 ns |        27.892 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Unkno(...)/edge [22]** |        **28.107 ns** |         **5.5625 ns** |       **0.3049 ns** |        **27.939 ns** |   **0.0034** |        **-** |        **-** |      **64 B** |
| ParseByteArray         | Unkno(...)/edge [22] |       306.246 ns |       112.7469 ns |       6.1800 ns |       305.625 ns |   0.0167 |        - |        - |     312 B |
| ParseSpan              | Unkno(...)/edge [22] |       234.681 ns |        99.7451 ns |       5.4674 ns |       232.827 ns |   0.0076 |        - |        - |     144 B |
| ToByteArray            | Unkno(...)/edge [22] |       282.914 ns |       231.7140 ns |      12.7010 ns |       288.908 ns |   0.0091 |        - |        - |     176 B |
| WriteToSpan            | Unkno(...)/edge [22] |       135.917 ns |        52.9629 ns |       2.9031 ns |       134.932 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)/edge [22] |       214.206 ns |        60.5072 ns |       3.3166 ns |       212.461 ns |   0.0038 |        - |        - |      72 B |
| CalculateSize          | Unkno(...)/edge [22] |        27.278 ns |         3.6416 ns |       0.1996 ns |        27.248 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)/edge [22] |        27.542 ns |        17.1907 ns |       0.9423 ns |        27.537 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Unkno(...)/edge [22] |        43.040 ns |        16.9610 ns |       0.9297 ns |        43.133 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Unkno(...)/edge [22] |        36.529 ns |         7.9431 ns |       0.4354 ns |        36.324 ns |   0.0034 |        - |        - |      64 B |
| **New**                    | **Unkno(...)empty [23]** |        **27.648 ns** |        **14.0501 ns** |       **0.7701 ns** |        **27.951 ns** |   **0.0033** |        **-** |        **-** |      **64 B** |
| ParseByteArray         | Unkno(...)empty [23] |        94.613 ns |        58.9042 ns |       3.2287 ns |        93.902 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Unkno(...)empty [23] |        27.826 ns |        15.9749 ns |       0.8756 ns |        27.793 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Unkno(...)empty [23] |        51.705 ns |        15.2225 ns |       0.8344 ns |        52.019 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Unkno(...)empty [23] |         7.344 ns |         0.4253 ns |       0.0233 ns |         7.348 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)empty [23] |         9.386 ns |         0.8203 ns |       0.0450 ns |         9.388 ns |        - |        - |        - |         - |
| CalculateSize          | Unkno(...)empty [23] |         3.299 ns |         0.6693 ns |       0.0367 ns |         3.282 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)empty [23] |        26.616 ns |        20.4051 ns |       1.1185 ns |        26.494 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Unkno(...)empty [23] |        40.310 ns |        47.8275 ns |       2.6216 ns |        39.378 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Unkno(...)empty [23] |        32.081 ns |        20.3202 ns |       1.1138 ns |        31.609 ns |   0.0034 |        - |        - |      64 B |
| **New**                    | **Unkno(...)lated [27]** |        **28.678 ns** |         **8.2978 ns** |       **0.4548 ns** |        **28.486 ns** |   **0.0034** |        **-** |        **-** |      **64 B** |
| ParseByteArray         | Unkno(...)lated [27] |       231.820 ns |       103.4640 ns |       5.6712 ns |       228.588 ns |   0.0167 |        - |        - |     320 B |
| ParseSpan              | Unkno(...)lated [27] |       161.449 ns |       122.0450 ns |       6.6897 ns |       158.164 ns |   0.0081 |        - |        - |     152 B |
| ToByteArray            | Unkno(...)lated [27] |       163.232 ns |       251.0762 ns |      13.7623 ns |       157.449 ns |   0.0072 |        - |        - |     136 B |
| WriteToSpan            | Unkno(...)lated [27] |        72.818 ns |        19.6236 ns |       1.0756 ns |        73.070 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)lated [27] |       133.945 ns |        36.9123 ns |       2.0233 ns |       134.748 ns |   0.0019 |        - |        - |      40 B |
| CalculateSize          | Unkno(...)lated [27] |        25.974 ns |         0.7286 ns |       0.0399 ns |        25.960 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)lated [27] |        26.612 ns |         9.4698 ns |       0.5191 ns |        26.800 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Unkno(...)lated [27] |        40.904 ns |        14.5958 ns |       0.8000 ns |        41.171 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Unkno(...)lated [27] |        37.704 ns |         3.6476 ns |       0.1999 ns |        37.610 ns |   0.0034 |        - |        - |      64 B |
| **New**                    | **Wrapp(...)esent [30]** |        **33.802 ns** |         **4.9546 ns** |       **0.2716 ns** |        **33.654 ns** |   **0.0047** |        **-** |        **-** |      **88 B** |
| ParseByteArray         | Wrapp(...)esent [30] |       184.893 ns |       111.1353 ns |       6.0917 ns |       182.090 ns |   0.0136 |        - |        - |     256 B |
| ParseSpan              | Wrapp(...)esent [30] |       119.297 ns |        65.6979 ns |       3.6011 ns |       119.011 ns |   0.0045 |        - |        - |      88 B |
| ToByteArray            | Wrapp(...)esent [30] |       139.356 ns |        33.0588 ns |       1.8121 ns |       139.881 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Wrapp(...)esent [30] |        63.700 ns |        15.7317 ns |       0.8623 ns |        64.154 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Wrapp(...)esent [30] |        81.357 ns |        18.7948 ns |       1.0302 ns |        80.849 ns |        - |        - |        - |         - |
| CalculateSize          | Wrapp(...)esent [30] |        26.102 ns |         6.9266 ns |       0.3797 ns |        26.295 ns |        - |        - |        - |         - |
| Clone                  | Wrapp(...)esent [30] |        35.263 ns |        14.6266 ns |       0.8017 ns |        35.142 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoNew           | Wrapp(...)esent [30] |        46.971 ns |        14.2541 ns |       0.7813 ns |        46.963 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoPopulated     | Wrapp(...)esent [30] |        47.820 ns |        39.8977 ns |       2.1869 ns |        48.014 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Wrappers/empty**       |        **33.225 ns** |        **12.3788 ns** |       **0.6785 ns** |        **33.286 ns** |   **0.0047** |        **-** |        **-** |      **88 B** |
| ParseByteArray         | Wrappers/empty       |        99.047 ns |        50.7163 ns |       2.7799 ns |        99.530 ns |   0.0136 |        - |        - |     256 B |
| ParseSpan              | Wrappers/empty       |        34.996 ns |        21.9428 ns |       1.2028 ns |        34.848 ns |   0.0047 |        - |        - |      88 B |
| ToByteArray            | Wrappers/empty       |        58.005 ns |        65.3419 ns |       3.5816 ns |        57.050 ns |   0.0046 |        - |        - |      88 B |
| WriteToSpan            | Wrappers/empty       |         8.268 ns |         6.2352 ns |       0.3418 ns |         8.138 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Wrappers/empty       |         9.091 ns |         9.9705 ns |       0.5465 ns |         8.880 ns |        - |        - |        - |         - |
| CalculateSize          | Wrappers/empty       |         2.995 ns |         0.3721 ns |       0.0204 ns |         3.006 ns |        - |        - |        - |         - |
| Clone                  | Wrappers/empty       |        30.987 ns |        12.6656 ns |       0.6942 ns |        30.710 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoNew           | Wrappers/empty       |        41.954 ns |        14.9140 ns |       0.8175 ns |        41.773 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoPopulated     | Wrappers/empty       |        40.686 ns |         7.7806 ns |       0.4265 ns |        40.924 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Wrappers/populated**   |        **32.626 ns** |         **2.5537 ns** |       **0.1400 ns** |        **32.576 ns** |   **0.0047** |        **-** |        **-** |      **88 B** |
| ParseByteArray         | Wrappers/populated   |       296.698 ns |       326.5599 ns |      17.8999 ns |       287.711 ns |   0.0200 |        - |        - |     384 B |
| ParseSpan              | Wrappers/populated   |       210.168 ns |        21.3328 ns |       1.1693 ns |       210.010 ns |   0.0114 |        - |        - |     216 B |
| ToByteArray            | Wrappers/populated   |       295.707 ns |       181.9823 ns |       9.9751 ns |       291.688 ns |   0.0086 |        - |        - |     168 B |
| WriteToSpan            | Wrappers/populated   |       157.146 ns |        38.0356 ns |       2.0849 ns |       156.355 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Wrappers/populated   |       214.748 ns |        21.4631 ns |       1.1765 ns |       214.378 ns |        - |        - |        - |         - |
| CalculateSize          | Wrappers/populated   |        56.665 ns |        18.1798 ns |       0.9965 ns |        56.260 ns |        - |        - |        - |         - |
| Clone                  | Wrappers/populated   |        32.092 ns |        13.7331 ns |       0.7528 ns |        32.256 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoNew           | Wrappers/populated   |        49.534 ns |        46.2455 ns |       2.5349 ns |        50.854 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoPopulated     | Wrappers/populated   |        53.700 ns |         1.8894 ns |       0.1036 ns |        53.723 ns |   0.0047 |        - |        - |      88 B |
