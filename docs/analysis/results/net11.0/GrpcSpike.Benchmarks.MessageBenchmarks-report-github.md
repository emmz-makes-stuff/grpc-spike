```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04 LTS (Resolute Raccoon)
Intel Xeon Platinum 8259CL CPU 2.50GHz, 1 CPU, 2 logical cores and 1 physical core
.NET SDK 11.0.100-rc.1.26425.128
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  ShortRun : .NET 11.0.0 (11.0.0-rc.1.26425.128, 11.0.26.42628), X64 RyuJIT x86-64-v4

Job=ShortRun  Toolchain=.NET 11.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method                 | Sample               | Mean             | Error             | StdDev         | Median           | Gen0     | Gen1     | Gen2     | Allocated |
|----------------------- |--------------------- |-----------------:|------------------:|---------------:|-----------------:|---------:|---------:|---------:|----------:|
| **New**                    | **Bytes/edge**           |        **26.308 ns** |         **5.1936 ns** |      **0.2847 ns** |        **26.247 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Bytes/edge           |   806,449.464 ns |   250,305.3079 ns | 13,720.0786 ns |   805,305.799 ns |  80.0781 |  80.0781 |  80.0781 | 1049299 B |
| ParseSpan              | Bytes/edge           |   800,178.281 ns |   124,772.8278 ns |  6,839.2197 ns |   802,976.313 ns |  72.2656 |  72.2656 |  72.2656 | 1049005 B |
| ToByteArray            | Bytes/edge           |   663,915.747 ns |   348,280.6365 ns | 19,090.4370 ns |   660,859.502 ns | 112.3047 | 112.3047 | 112.3047 | 1049677 B |
| WriteToSpan            | Bytes/edge           |    55,240.925 ns |    24,133.2372 ns |  1,322.8242 ns |    55,186.328 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Bytes/edge           |   609,698.016 ns |   185,693.8315 ns | 10,178.5055 ns |   603,851.833 ns |        - |        - |        - |         - |
| CalculateSize          | Bytes/edge           |         5.146 ns |         6.2398 ns |      0.3420 ns |         5.064 ns |        - |        - |        - |         - |
| Clone                  | Bytes/edge           |        25.257 ns |         9.3356 ns |      0.5117 ns |        25.039 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Bytes/edge           |        41.604 ns |        22.8838 ns |      1.2543 ns |        42.305 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Bytes/edge           |        36.300 ns |         2.4686 ns |      0.1353 ns |        36.226 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Bytes/empty**          |        **27.441 ns** |        **30.7355 ns** |      **1.6847 ns** |        **26.798 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Bytes/empty          |        88.561 ns |        31.9733 ns |      1.7526 ns |        87.789 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | Bytes/empty          |        23.880 ns |         9.1563 ns |      0.5019 ns |        24.147 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | Bytes/empty          |        46.300 ns |        21.9312 ns |      1.2021 ns |        45.625 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Bytes/empty          |        19.424 ns |        26.9335 ns |      1.4763 ns |        18.659 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Bytes/empty          |         8.863 ns |        16.0707 ns |      0.8809 ns |         8.630 ns |        - |        - |        - |         - |
| CalculateSize          | Bytes/empty          |         2.388 ns |         1.0340 ns |      0.0567 ns |         2.408 ns |        - |        - |        - |         - |
| Clone                  | Bytes/empty          |        25.506 ns |        16.0026 ns |      0.8772 ns |        25.184 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Bytes/empty          |        38.271 ns |         2.4523 ns |      0.1344 ns |        38.272 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Bytes/empty          |        31.585 ns |        13.8247 ns |      0.7578 ns |        31.294 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Bytes/populated**      |        **26.317 ns** |        **13.7206 ns** |      **0.7521 ns** |        **26.437 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Bytes/populated      |     3,301.125 ns |     1,848.8935 ns |    101.3441 ns |     3,323.589 ns |   0.8926 |   0.0496 |        - |   16720 B |
| ParseSpan              | Bytes/populated      |     4,161.631 ns |    15,950.8195 ns |    874.3182 ns |     4,231.235 ns |   0.8812 |   0.0496 |        - |   16552 B |
| ToByteArray            | Bytes/populated      |     3,339.954 ns |     1,710.0184 ns |     93.7319 ns |     3,308.211 ns |   0.8774 |        - |        - |   16496 B |
| WriteToSpan            | Bytes/populated      |       302.553 ns |        42.5392 ns |      2.3317 ns |       302.101 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Bytes/populated      |     9,228.958 ns |     1,100.1965 ns |     60.3055 ns |     9,202.877 ns |        - |        - |        - |         - |
| CalculateSize          | Bytes/populated      |         5.049 ns |         1.4338 ns |      0.0786 ns |         5.028 ns |        - |        - |        - |         - |
| Clone                  | Bytes/populated      |        26.080 ns |        12.9813 ns |      0.7115 ns |        25.947 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Bytes/populated      |        40.619 ns |         7.0694 ns |      0.3875 ns |        40.720 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Bytes/populated      |        35.629 ns |        36.1516 ns |      1.9816 ns |        34.626 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **MapValue/edge**        |        **23.274 ns** |         **4.4735 ns** |      **0.2452 ns** |        **23.292 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | MapValue/edge        |       144.771 ns |        22.3933 ns |      1.2275 ns |       145.275 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | MapValue/edge        |        70.315 ns |        32.4676 ns |      1.7797 ns |        70.103 ns |   0.0033 |        - |        - |      64 B |
| ToByteArray            | MapValue/edge        |        77.962 ns |        60.1664 ns |      3.2979 ns |        76.385 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | MapValue/edge        |        26.602 ns |        34.4739 ns |      1.8896 ns |        25.798 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | MapValue/edge        |        39.377 ns |        27.5235 ns |      1.5087 ns |        38.552 ns |        - |        - |        - |         - |
| CalculateSize          | MapValue/edge        |        10.344 ns |         2.1854 ns |      0.1198 ns |        10.356 ns |        - |        - |        - |         - |
| Clone                  | MapValue/edge        |        20.164 ns |         5.1758 ns |      0.2837 ns |        20.049 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | MapValue/edge        |        35.864 ns |        37.4306 ns |      2.0517 ns |        36.618 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | MapValue/edge        |        26.105 ns |        20.8001 ns |      1.1401 ns |        25.790 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **MapValue/empty**       |        **23.558 ns** |         **8.5922 ns** |      **0.4710 ns** |        **23.519 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | MapValue/empty       |        81.217 ns |        24.0862 ns |      1.3202 ns |        80.802 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | MapValue/empty       |        22.853 ns |         8.4471 ns |      0.4630 ns |        22.652 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | MapValue/empty       |        46.225 ns |        24.9442 ns |      1.3673 ns |        46.804 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | MapValue/empty       |         5.165 ns |         0.9730 ns |      0.0533 ns |         5.176 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | MapValue/empty       |         8.325 ns |         3.0883 ns |      0.1693 ns |         8.312 ns |        - |        - |        - |         - |
| CalculateSize          | MapValue/empty       |         2.652 ns |         8.3818 ns |      0.4594 ns |         2.564 ns |        - |        - |        - |         - |
| Clone                  | MapValue/empty       |        20.205 ns |        34.2504 ns |      1.8774 ns |        19.204 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | MapValue/empty       |        33.724 ns |         5.5024 ns |      0.3016 ns |        33.760 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | MapValue/empty       |        27.271 ns |        11.3595 ns |      0.6227 ns |        27.167 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **MapValue/populated**   |        **22.940 ns** |         **2.0529 ns** |      **0.1125 ns** |        **22.897 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | MapValue/populated   |       123.673 ns |        35.3612 ns |      1.9383 ns |       122.998 ns |   0.0126 |        - |        - |     240 B |
| ParseSpan              | MapValue/populated   |        66.705 ns |        16.9391 ns |      0.9285 ns |        66.208 ns |   0.0038 |        - |        - |      72 B |
| ToByteArray            | MapValue/populated   |        75.895 ns |        80.9104 ns |      4.4350 ns |        74.608 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | MapValue/populated   |        21.302 ns |         1.0255 ns |      0.0562 ns |        21.326 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | MapValue/populated   |        33.387 ns |         2.6557 ns |      0.1456 ns |        33.383 ns |        - |        - |        - |         - |
| CalculateSize          | MapValue/populated   |         9.408 ns |         8.2216 ns |      0.4507 ns |         9.382 ns |        - |        - |        - |         - |
| Clone                  | MapValue/populated   |        19.494 ns |         9.6804 ns |      0.5306 ns |        19.492 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | MapValue/populated   |        33.432 ns |         9.8242 ns |      0.5385 ns |        33.390 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | MapValue/populated   |        30.333 ns |        24.1218 ns |      1.3222 ns |        29.677 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Maps/edge**            |       **175.580 ns** |       **107.4144 ns** |      **5.8877 ns** |       **178.722 ns** |   **0.0269** |        **-** |        **-** |     **504 B** |
| ParseByteArray         | Maps/edge            |     1,093.833 ns |       528.6932 ns |     28.9795 ns |     1,080.470 ns |   0.0820 |        - |        - |    1560 B |
| ParseSpan              | Maps/edge            |     1,331.218 ns |     8,616.2062 ns |    472.2833 ns |     1,079.402 ns |   0.0744 |        - |        - |    1392 B |
| ToByteArray            | Maps/edge            |       660.885 ns |       123.3562 ns |      6.7616 ns |       659.669 ns |   0.0200 |        - |        - |     384 B |
| WriteToSpan            | Maps/edge            |       513.149 ns |       125.6905 ns |      6.8895 ns |       511.094 ns |   0.0086 |        - |        - |     168 B |
| WriteToSegmentedWriter | Maps/edge            |       672.441 ns |        46.5566 ns |      2.5519 ns |       671.262 ns |   0.0162 |        - |        - |     312 B |
| CalculateSize          | Maps/edge            |        81.638 ns |        39.1189 ns |      2.1442 ns |        81.743 ns |        - |        - |        - |         - |
| Clone                  | Maps/edge            |       823.052 ns |       178.9540 ns |      9.8091 ns |       821.499 ns |   0.0925 |        - |        - |    1744 B |
| MergeIntoNew           | Maps/edge            |       602.511 ns |       207.0497 ns |     11.3491 ns |       596.636 ns |   0.0668 |        - |        - |    1248 B |
| MergeIntoPopulated     | Maps/edge            |       959.332 ns |       361.9061 ns |     19.8373 ns |       955.001 ns |   0.0954 |        - |        - |    1800 B |
| **New**                    | **Maps/empty**           |       **184.195 ns** |       **319.6867 ns** |     **17.5231 ns** |       **175.055 ns** |   **0.0269** |        **-** |        **-** |     **504 B** |
| ParseByteArray         | Maps/empty           |       247.534 ns |       101.9504 ns |      5.5882 ns |       247.973 ns |   0.0358 |        - |        - |     672 B |
| ParseSpan              | Maps/empty           |       169.582 ns |        64.2807 ns |      3.5234 ns |       170.331 ns |   0.0269 |        - |        - |     504 B |
| ToByteArray            | Maps/empty           |        73.669 ns |         7.9039 ns |      0.4332 ns |        73.887 ns |   0.0046 |        - |        - |      88 B |
| WriteToSpan            | Maps/empty           |        35.066 ns |        29.1232 ns |      1.5963 ns |        35.580 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Maps/empty           |        33.208 ns |        10.9615 ns |      0.6008 ns |        33.363 ns |        - |        - |        - |         - |
| CalculateSize          | Maps/empty           |         5.321 ns |         3.8731 ns |      0.2123 ns |         5.204 ns |        - |        - |        - |         - |
| Clone                  | Maps/empty           |       451.043 ns |     2,743.3000 ns |    150.3695 ns |       381.982 ns |   0.0510 |        - |        - |     960 B |
| MergeIntoNew           | Maps/empty           |       187.892 ns |        38.0754 ns |      2.0870 ns |       187.202 ns |   0.0269 |        - |        - |     504 B |
| MergeIntoPopulated     | Maps/empty           |       320.458 ns |       330.4793 ns |     18.1147 ns |       314.883 ns |   0.0510 |        - |        - |     960 B |
| **New**                    | **Maps/populated**       |       **183.082 ns** |       **133.8382 ns** |      **7.3361 ns** |       **186.257 ns** |   **0.0269** |        **-** |        **-** |     **504 B** |
| ParseByteArray         | Maps/populated       |    18,603.764 ns |     1,709.9818 ns |     93.7299 ns |    18,599.400 ns |   1.3123 |   0.0916 |        - |   24720 B |
| ParseSpan              | Maps/populated       |    19,324.073 ns |     6,544.6601 ns |    358.7349 ns |    19,272.966 ns |   1.3123 |   0.0916 |        - |   24552 B |
| ToByteArray            | Maps/populated       |    10,901.852 ns |     3,740.2017 ns |    205.0131 ns |    10,824.395 ns |   0.1068 |        - |        - |    2136 B |
| WriteToSpan            | Maps/populated       |     7,758.338 ns |     1,254.5010 ns |     68.7634 ns |     7,722.620 ns |        - |        - |        - |     168 B |
| WriteToSegmentedWriter | Maps/populated       |    11,114.651 ns |    18,374.0361 ns |  1,007.1429 ns |    10,545.507 ns |   0.0458 |        - |        - |    1096 B |
| CalculateSize          | Maps/populated       |     2,637.369 ns |       656.2222 ns |     35.9698 ns |     2,624.942 ns |        - |        - |        - |         - |
| Clone                  | Maps/populated       |    12,364.850 ns |     9,631.4056 ns |    527.9298 ns |    12,169.077 ns |   1.0376 |   0.0458 |        - |   19624 B |
| MergeIntoNew           | Maps/populated       |    10,129.718 ns |     2,840.1720 ns |    155.6794 ns |    10,210.392 ns |   0.9766 |   0.0458 |        - |   18424 B |
| MergeIntoPopulated     | Maps/populated       |    13,850.319 ns |     7,037.9943 ns |    385.7762 ns |    13,877.644 ns |   1.0529 |   0.0458 |        - |   19680 B |
| **New**                    | **Node/edge**            |        **27.229 ns** |        **14.7542 ns** |      **0.8087 ns** |        **27.087 ns** |   **0.0026** |        **-** |        **-** |      **48 B** |
| ParseByteArray         | Node/edge            |       317.947 ns |        66.6267 ns |      3.6520 ns |       317.758 ns |   0.0291 |        - |        - |     552 B |
| ParseSpan              | Node/edge            |       254.327 ns |        80.0081 ns |      4.3855 ns |       251.972 ns |   0.0205 |        - |        - |     384 B |
| ToByteArray            | Node/edge            |       244.466 ns |     1,174.7794 ns |     64.3936 ns |       213.154 ns |   0.0052 |        - |        - |     104 B |
| WriteToSpan            | Node/edge            |       139.408 ns |        40.9427 ns |      2.2442 ns |       138.882 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Node/edge            |       151.190 ns |        37.6055 ns |      2.0613 ns |       150.014 ns |        - |        - |        - |         - |
| CalculateSize          | Node/edge            |        30.675 ns |         5.7336 ns |      0.3143 ns |        30.566 ns |        - |        - |        - |         - |
| Clone                  | Node/edge            |       170.534 ns |        61.9116 ns |      3.3936 ns |       170.739 ns |   0.0205 |        - |        - |     384 B |
| MergeIntoNew           | Node/edge            |       181.822 ns |        57.6473 ns |      3.1598 ns |       180.807 ns |   0.0205 |        - |        - |     384 B |
| MergeIntoPopulated     | Node/edge            |       202.405 ns |        98.9371 ns |      5.4231 ns |       199.846 ns |   0.0205 |        - |        - |     384 B |
| **New**                    | **Node/empty**           |        **25.876 ns** |        **60.9550 ns** |      **3.3411 ns** |        **23.987 ns** |   **0.0026** |        **-** |        **-** |      **48 B** |
| ParseByteArray         | Node/empty           |        92.000 ns |       157.6746 ns |      8.6427 ns |        90.265 ns |   0.0114 |        - |        - |     216 B |
| ParseSpan              | Node/empty           |        24.084 ns |        10.6259 ns |      0.5824 ns |        23.757 ns |   0.0026 |        - |        - |      48 B |
| ToByteArray            | Node/empty           |        45.801 ns |        13.5096 ns |      0.7405 ns |        45.887 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Node/empty           |         5.607 ns |         6.2728 ns |      0.3438 ns |         5.412 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Node/empty           |         8.551 ns |         4.5136 ns |      0.2474 ns |         8.472 ns |        - |        - |        - |         - |
| CalculateSize          | Node/empty           |         4.674 ns |         4.2935 ns |      0.2353 ns |         4.609 ns |        - |        - |        - |         - |
| Clone                  | Node/empty           |        23.385 ns |        19.2281 ns |      1.0540 ns |        23.476 ns |   0.0026 |        - |        - |      48 B |
| MergeIntoNew           | Node/empty           |        34.415 ns |         7.3694 ns |      0.4039 ns |        34.318 ns |   0.0026 |        - |        - |      48 B |
| MergeIntoPopulated     | Node/empty           |        27.646 ns |        12.9931 ns |      0.7122 ns |        28.031 ns |   0.0026 |        - |        - |      48 B |
| **New**                    | **Node/(...)epth1 [21]** |        **24.391 ns** |         **8.9046 ns** |      **0.4881 ns** |        **24.212 ns** |   **0.0026** |        **-** |        **-** |      **48 B** |
| ParseByteArray         | Node/(...)epth1 [21] |       125.248 ns |        43.6963 ns |      2.3951 ns |       124.612 ns |   0.0136 |        - |        - |     256 B |
| ParseSpan              | Node/(...)epth1 [21] |        75.069 ns |       150.2010 ns |      8.2330 ns |        70.891 ns |   0.0046 |        - |        - |      88 B |
| ToByteArray            | Node/(...)epth1 [21] |        77.631 ns |        39.1134 ns |      2.1439 ns |        76.970 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Node/(...)epth1 [21] |        22.859 ns |        14.3136 ns |      0.7846 ns |        22.643 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Node/(...)epth1 [21] |        34.781 ns |         6.0477 ns |      0.3315 ns |        34.726 ns |        - |        - |        - |         - |
| CalculateSize          | Node/(...)epth1 [21] |        11.472 ns |         2.3088 ns |      0.1266 ns |        11.409 ns |        - |        - |        - |         - |
| Clone                  | Node/(...)epth1 [21] |        24.249 ns |        35.5482 ns |      1.9485 ns |        23.907 ns |   0.0026 |        - |        - |      48 B |
| MergeIntoNew           | Node/(...)epth1 [21] |        48.187 ns |       372.0531 ns |     20.3935 ns |        36.507 ns |   0.0026 |        - |        - |      48 B |
| MergeIntoPopulated     | Node/(...)epth1 [21] |        32.453 ns |        18.2401 ns |      0.9998 ns |        31.955 ns |   0.0026 |        - |        - |      48 B |
| **New**                    | **Node/(...)epth3 [21]** |        **23.869 ns** |         **8.0428 ns** |      **0.4409 ns** |        **24.076 ns** |   **0.0026** |        **-** |        **-** |      **48 B** |
| ParseByteArray         | Node/(...)epth3 [21] |       291.304 ns |       204.3483 ns |     11.2010 ns |       289.486 ns |   0.0229 |        - |        - |     432 B |
| ParseSpan              | Node/(...)epth3 [21] |       224.785 ns |        19.5191 ns |      1.0699 ns |       224.897 ns |   0.0141 |        - |        - |     264 B |
| ToByteArray            | Node/(...)epth3 [21] |       198.564 ns |        73.2945 ns |      4.0175 ns |       196.819 ns |   0.0067 |        - |        - |     128 B |
| WriteToSpan            | Node/(...)epth3 [21] |       134.024 ns |       376.9203 ns |     20.6603 ns |       129.352 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Node/(...)epth3 [21] |       177.913 ns |        44.6216 ns |      2.4459 ns |       176.943 ns |   0.0017 |        - |        - |      32 B |
| CalculateSize          | Node/(...)epth3 [21] |        30.999 ns |        10.2505 ns |      0.5619 ns |        31.310 ns |        - |        - |        - |         - |
| Clone                  | Node/(...)epth3 [21] |        65.382 ns |        20.5188 ns |      1.1247 ns |        65.608 ns |   0.0076 |        - |        - |     144 B |
| MergeIntoNew           | Node/(...)epth3 [21] |        76.575 ns |        22.9790 ns |      1.2596 ns |        76.688 ns |   0.0076 |        - |        - |     144 B |
| MergeIntoPopulated     | Node/(...)epth3 [21] |        82.824 ns |        30.2145 ns |      1.6562 ns |        81.954 ns |   0.0076 |        - |        - |     144 B |
| **New**                    | **Node/(...)epth8 [21]** |        **24.615 ns** |        **12.7730 ns** |      **0.7001 ns** |        **24.565 ns** |   **0.0026** |        **-** |        **-** |      **48 B** |
| ParseByteArray         | Node/(...)epth8 [21] |       690.204 ns |       110.7294 ns |      6.0695 ns |       690.435 ns |   0.0458 |        - |        - |     872 B |
| ParseSpan              | Node/(...)epth8 [21] |       667.221 ns |     1,276.0001 ns |     69.9419 ns |       628.735 ns |   0.0343 |        - |        - |     704 B |
| ToByteArray            | Node/(...)epth8 [21] |       694.785 ns |       206.6870 ns |     11.3292 ns |       697.825 ns |   0.0095 |        - |        - |     192 B |
| WriteToSpan            | Node/(...)epth8 [21] |       540.279 ns |       263.2373 ns |     14.4289 ns |       532.987 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Node/(...)epth8 [21] |       658.903 ns |       169.9754 ns |      9.3169 ns |       664.214 ns |   0.0029 |        - |        - |      64 B |
| CalculateSize          | Node/(...)epth8 [21] |        84.099 ns |        15.6521 ns |      0.8579 ns |        83.661 ns |        - |        - |        - |         - |
| Clone                  | Node/(...)epth8 [21] |       160.934 ns |        61.8601 ns |      3.3908 ns |       162.631 ns |   0.0205 |        - |        - |     384 B |
| MergeIntoNew           | Node/(...)epth8 [21] |       183.374 ns |        93.2289 ns |      5.1102 ns |       186.064 ns |   0.0205 |        - |        - |     384 B |
| MergeIntoPopulated     | Node/(...)epth8 [21] |       205.776 ns |        32.1703 ns |      1.7634 ns |       205.113 ns |   0.0205 |        - |        - |     384 B |
| **New**                    | **Oneof(...)int32 [24]** |        **23.519 ns** |        **16.5637 ns** |      **0.9079 ns** |        **23.071 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)int32 [24] |       102.301 ns |        29.4060 ns |      1.6118 ns |       102.811 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Oneof(...)int32 [24] |        36.787 ns |        22.1702 ns |      1.2152 ns |        36.775 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Oneof(...)int32 [24] |        54.770 ns |        20.1625 ns |      1.1052 ns |        54.553 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof(...)int32 [24] |        11.711 ns |         6.6520 ns |      0.3646 ns |        11.824 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)int32 [24] |        25.408 ns |         3.5711 ns |      0.1957 ns |        25.491 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)int32 [24] |         5.770 ns |         0.7000 ns |      0.0384 ns |         5.774 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)int32 [24] |        29.620 ns |        21.6051 ns |      1.1842 ns |        29.115 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Oneof(...)int32 [24] |        41.870 ns |        10.1845 ns |      0.5582 ns |        41.915 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Oneof(...)int32 [24] |        43.699 ns |         2.3795 ns |      0.1304 ns |        43.755 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Oneof(...)ssage [26]** |        **23.260 ns** |         **1.5525 ns** |      **0.0851 ns** |        **23.212 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)ssage [26] |       114.683 ns |        40.9855 ns |      2.2466 ns |       113.909 ns |   0.0131 |        - |        - |     248 B |
| ParseSpan              | Oneof(...)ssage [26] |        84.533 ns |       685.7149 ns |     37.5863 ns |        64.581 ns |   0.0042 |        - |        - |      80 B |
| ToByteArray            | Oneof(...)ssage [26] |        60.381 ns |         7.9352 ns |      0.4350 ns |        60.334 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof(...)ssage [26] |        21.588 ns |        15.6959 ns |      0.8603 ns |        21.212 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)ssage [26] |        30.135 ns |         3.3888 ns |      0.1858 ns |        30.165 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)ssage [26] |         7.724 ns |         2.8618 ns |      0.1569 ns |         7.784 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)ssage [26] |        35.284 ns |         4.3917 ns |      0.2407 ns |        35.260 ns |   0.0042 |        - |        - |      80 B |
| MergeIntoNew           | Oneof(...)ssage [26] |        51.187 ns |         2.2521 ns |      0.1234 ns |        51.238 ns |   0.0042 |        - |        - |      80 B |
| MergeIntoPopulated     | Oneof(...)ssage [26] |        49.405 ns |        26.1511 ns |      1.4334 ns |        49.015 ns |   0.0042 |        - |        - |      80 B |
| **New**                    | **Oneof(...)tring [25]** |        **24.350 ns** |        **13.4766 ns** |      **0.7387 ns** |        **24.507 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)tring [25] |        91.460 ns |        44.0978 ns |      2.4171 ns |        91.474 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | Oneof(...)tring [25] |        40.868 ns |        21.3303 ns |      1.1692 ns |        41.382 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | Oneof(...)tring [25] |        72.855 ns |        28.5212 ns |      1.5633 ns |        73.563 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof(...)tring [25] |        20.297 ns |         2.1099 ns |      0.1156 ns |        20.348 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)tring [25] |        46.285 ns |       344.6873 ns |     18.8935 ns |        39.043 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)tring [25] |        10.866 ns |         1.2702 ns |      0.0696 ns |        10.860 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)tring [25] |        21.994 ns |         1.3510 ns |      0.0741 ns |        21.965 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Oneof(...)tring [25] |        34.268 ns |         0.6805 ns |      0.0373 ns |        34.272 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Oneof(...)tring [25] |        32.318 ns |        14.1777 ns |      0.7771 ns |        32.112 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Oneof/empty**          |        **22.835 ns** |         **0.6648 ns** |      **0.0364 ns** |        **22.831 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof/empty          |       111.440 ns |       201.7372 ns |     11.0579 ns |       110.767 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | Oneof/empty          |        23.317 ns |         4.1677 ns |      0.2284 ns |        23.301 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | Oneof/empty          |        50.907 ns |        22.4433 ns |      1.2302 ns |        51.221 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Oneof/empty          |         9.240 ns |        10.7832 ns |      0.5911 ns |         8.969 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof/empty          |        11.743 ns |         4.9876 ns |      0.2734 ns |        11.617 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof/empty          |         6.349 ns |        19.4408 ns |      1.0656 ns |         5.795 ns |        - |        - |        - |         - |
| Clone                  | Oneof/empty          |        19.532 ns |         6.7768 ns |      0.3715 ns |        19.602 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Oneof/empty          |        35.695 ns |        16.6134 ns |      0.9106 ns |        35.952 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Oneof/empty          |        23.960 ns |         7.6084 ns |      0.4170 ns |        24.074 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Oneof/populated-bool** |        **22.814 ns** |         **1.4663 ns** |      **0.0804 ns** |        **22.818 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof/populated-bool |       118.494 ns |       148.2583 ns |      8.1265 ns |       114.999 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Oneof/populated-bool |        50.171 ns |         6.0196 ns |      0.3300 ns |        50.048 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Oneof/populated-bool |        60.468 ns |        32.5421 ns |      1.7837 ns |        60.752 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof/populated-bool |        14.310 ns |         5.6904 ns |      0.3119 ns |        14.134 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof/populated-bool |        29.061 ns |         2.2688 ns |      0.1244 ns |        29.029 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof/populated-bool |         5.369 ns |         6.0732 ns |      0.3329 ns |         5.210 ns |        - |        - |        - |         - |
| Clone                  | Oneof/populated-bool |        28.449 ns |         3.7784 ns |      0.2071 ns |        28.343 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Oneof/populated-bool |        43.114 ns |        13.4973 ns |      0.7398 ns |        43.308 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Oneof/populated-bool |        47.311 ns |        19.6276 ns |      1.0759 ns |        47.606 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Oneof(...)ouble [22]** |        **22.952 ns** |         **9.4557 ns** |      **0.5183 ns** |        **22.892 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)ouble [22] |       103.214 ns |        46.4340 ns |      2.5452 ns |       102.643 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Oneof(...)ouble [22] |        40.759 ns |         9.3082 ns |      0.5102 ns |        40.470 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Oneof(...)ouble [22] |        63.587 ns |        33.4803 ns |      1.8352 ns |        63.163 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Oneof(...)ouble [22] |        15.625 ns |         4.1756 ns |      0.2289 ns |        15.509 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)ouble [22] |        53.403 ns |       542.3318 ns |     29.7270 ns |        36.306 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)ouble [22] |         5.440 ns |         6.3139 ns |      0.3461 ns |         5.360 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)ouble [22] |        30.717 ns |         6.8536 ns |      0.3757 ns |        30.736 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Oneof(...)ouble [22] |        47.207 ns |        10.0299 ns |      0.5498 ns |        47.276 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Oneof(...)ouble [22] |        46.750 ns |        25.1228 ns |      1.3771 ns |        46.896 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Oneof(...)int32 [21]** |        **23.542 ns** |         **5.2858 ns** |      **0.2897 ns** |        **23.509 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)int32 [21] |       109.157 ns |        24.8613 ns |      1.3627 ns |       109.819 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Oneof(...)int32 [21] |        51.096 ns |        17.6673 ns |      0.9684 ns |        50.866 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Oneof(...)int32 [21] |        60.108 ns |        36.2581 ns |      1.9874 ns |        60.254 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof(...)int32 [21] |        14.403 ns |         7.5153 ns |      0.4119 ns |        14.198 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)int32 [21] |        29.262 ns |         6.9600 ns |      0.3815 ns |        29.044 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)int32 [21] |         6.953 ns |         4.1585 ns |      0.2279 ns |         6.860 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)int32 [21] |        29.436 ns |        15.6081 ns |      0.8555 ns |        29.776 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Oneof(...)int32 [21] |        42.053 ns |        11.0213 ns |      0.6041 ns |        42.322 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Oneof(...)int32 [21] |        44.688 ns |        17.4505 ns |      0.9565 ns |        44.419 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Oneof(...)int64 [21]** |        **22.780 ns** |        **13.0455 ns** |      **0.7151 ns** |        **22.395 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)int64 [21] |       111.185 ns |        35.5828 ns |      1.9504 ns |       110.393 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Oneof(...)int64 [21] |        47.628 ns |        14.0472 ns |      0.7700 ns |        47.276 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Oneof(...)int64 [21] |        71.208 ns |        29.1046 ns |      1.5953 ns |        70.326 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Oneof(...)int64 [21] |        23.117 ns |         8.8910 ns |      0.4873 ns |        23.329 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)int64 [21] |        38.193 ns |        21.5463 ns |      1.1810 ns |        37.779 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)int64 [21] |         7.331 ns |         5.1311 ns |      0.2813 ns |         7.182 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)int64 [21] |        30.650 ns |        16.3065 ns |      0.8938 ns |        30.797 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Oneof(...)int64 [21] |        44.669 ns |         1.3813 ns |      0.0757 ns |        44.698 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Oneof(...)int64 [21] |        45.945 ns |        21.3321 ns |      1.1693 ns |        45.286 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Oneof(...)ssage [23]** |        **29.631 ns** |       **138.3598 ns** |      **7.5840 ns** |        **25.621 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)ssage [23] |       161.368 ns |        37.7579 ns |      2.0696 ns |       161.816 ns |   0.0153 |        - |        - |     288 B |
| ParseSpan              | Oneof(...)ssage [23] |       109.676 ns |        32.6942 ns |      1.7921 ns |       109.335 ns |   0.0063 |        - |        - |     120 B |
| ToByteArray            | Oneof(...)ssage [23] |       109.602 ns |        19.3973 ns |      1.0632 ns |       109.810 ns |   0.0060 |        - |        - |     112 B |
| WriteToSpan            | Oneof(...)ssage [23] |        51.747 ns |        11.6501 ns |      0.6386 ns |        51.399 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)ssage [23] |        61.329 ns |        18.8890 ns |      1.0354 ns |        60.912 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)ssage [23] |        18.674 ns |         4.2489 ns |      0.2329 ns |        18.580 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)ssage [23] |        47.030 ns |        85.2481 ns |      4.6727 ns |        47.384 ns |   0.0042 |        - |        - |      80 B |
| MergeIntoNew           | Oneof(...)ssage [23] |        57.385 ns |        38.6655 ns |      2.1194 ns |        56.498 ns |   0.0042 |        - |        - |      80 B |
| MergeIntoPopulated     | Oneof(...)ssage [23] |        53.732 ns |        28.4365 ns |      1.5587 ns |        54.602 ns |   0.0042 |        - |        - |      80 B |
| **New**                    | **Oneof(...)tring [22]** |        **23.813 ns** |         **5.5776 ns** |      **0.3057 ns** |        **23.691 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)tring [22] |       130.264 ns |        10.9225 ns |      0.5987 ns |       130.234 ns |   0.0131 |        - |        - |     248 B |
| ParseSpan              | Oneof(...)tring [22] |        70.848 ns |        37.3039 ns |      2.0448 ns |        70.570 ns |   0.0042 |        - |        - |      80 B |
| ToByteArray            | Oneof(...)tring [22] |        87.864 ns |        55.9079 ns |      3.0645 ns |        87.860 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Oneof(...)tring [22] |        30.351 ns |         4.0657 ns |      0.2229 ns |        30.324 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)tring [22] |        40.922 ns |        13.1434 ns |      0.7204 ns |        41.010 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)tring [22] |        15.120 ns |         9.2616 ns |      0.5077 ns |        14.868 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)tring [22] |        22.425 ns |        17.5171 ns |      0.9602 ns |        22.278 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Oneof(...)tring [22] |        38.528 ns |        26.5195 ns |      1.4536 ns |        38.497 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Oneof(...)tring [22] |        31.290 ns |        15.3409 ns |      0.8409 ns |        30.828 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **OneofPayload/edge**    |        **23.140 ns** |        **17.1202 ns** |      **0.9384 ns** |        **22.630 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | OneofPayload/edge    |       245.276 ns |        62.9416 ns |      3.4500 ns |       244.240 ns |   0.0153 |        - |        - |     288 B |
| ParseSpan              | OneofPayload/edge    |       164.932 ns |         9.5869 ns |      0.5255 ns |       164.969 ns |   0.0062 |        - |        - |     120 B |
| ToByteArray            | OneofPayload/edge    |       178.715 ns |        46.2835 ns |      2.5370 ns |       178.773 ns |   0.0081 |        - |        - |     152 B |
| WriteToSpan            | OneofPayload/edge    |        99.695 ns |        80.4721 ns |      4.4109 ns |        98.860 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | OneofPayload/edge    |       167.543 ns |        64.7550 ns |      3.5494 ns |       165.815 ns |   0.0038 |        - |        - |      72 B |
| CalculateSize          | OneofPayload/edge    |        20.805 ns |        12.9802 ns |      0.7115 ns |        20.510 ns |        - |        - |        - |         - |
| Clone                  | OneofPayload/edge    |        20.186 ns |        21.9831 ns |      1.2050 ns |        20.537 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | OneofPayload/edge    |        33.861 ns |        15.3302 ns |      0.8403 ns |        33.549 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | OneofPayload/edge    |        27.561 ns |        16.9969 ns |      0.9317 ns |        27.967 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **OneofPayload/empty**   |        **22.579 ns** |         **3.3911 ns** |      **0.1859 ns** |        **22.629 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | OneofPayload/empty   |        83.103 ns |        49.2120 ns |      2.6975 ns |        82.120 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | OneofPayload/empty   |        21.799 ns |         6.3216 ns |      0.3465 ns |        21.715 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | OneofPayload/empty   |        45.264 ns |         4.2771 ns |      0.2344 ns |        45.261 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | OneofPayload/empty   |         5.279 ns |         3.6969 ns |      0.2026 ns |         5.247 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | OneofPayload/empty   |         8.893 ns |         4.9101 ns |      0.2691 ns |         8.998 ns |        - |        - |        - |         - |
| CalculateSize          | OneofPayload/empty   |         2.141 ns |         2.6764 ns |      0.1467 ns |         2.066 ns |        - |        - |        - |         - |
| Clone                  | OneofPayload/empty   |        19.388 ns |        11.2323 ns |      0.6157 ns |        19.376 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | OneofPayload/empty   |        31.504 ns |         2.7310 ns |      0.1497 ns |        31.561 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | OneofPayload/empty   |        27.416 ns |         5.8111 ns |      0.3185 ns |        27.317 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Oneof(...)lated [22]** |        **22.128 ns** |         **6.4042 ns** |      **0.3510 ns** |        **22.069 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Oneof(...)lated [22] |       124.283 ns |        28.5582 ns |      1.5654 ns |       124.702 ns |   0.0126 |        - |        - |     240 B |
| ParseSpan              | Oneof(...)lated [22] |        65.012 ns |        35.8107 ns |      1.9629 ns |        64.747 ns |   0.0038 |        - |        - |      72 B |
| ToByteArray            | Oneof(...)lated [22] |        72.422 ns |        37.6333 ns |      2.0628 ns |        71.278 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Oneof(...)lated [22] |        20.431 ns |         1.4207 ns |      0.0779 ns |        20.454 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Oneof(...)lated [22] |        32.969 ns |         4.1906 ns |      0.2297 ns |        32.914 ns |        - |        - |        - |         - |
| CalculateSize          | Oneof(...)lated [22] |        11.134 ns |         2.6018 ns |      0.1426 ns |        11.058 ns |        - |        - |        - |         - |
| Clone                  | Oneof(...)lated [22] |        18.630 ns |         9.5236 ns |      0.5220 ns |        18.676 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Oneof(...)lated [22] |        35.261 ns |        33.5082 ns |      1.8367 ns |        35.211 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Oneof(...)lated [22] |        27.019 ns |        12.8714 ns |      0.7055 ns |        27.260 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Optio(...)esent [30]** |        **29.212 ns** |        **12.9357 ns** |      **0.7090 ns** |        **29.025 ns** |   **0.0038** |        **-** |        **-** |      **72 B** |
| ParseByteArray         | Optio(...)esent [30] |       198.657 ns |        53.1875 ns |      2.9154 ns |       199.675 ns |   0.0157 |        - |        - |     296 B |
| ParseSpan              | Optio(...)esent [30] |       122.645 ns |        44.5136 ns |      2.4399 ns |       121.665 ns |   0.0067 |        - |        - |     128 B |
| ToByteArray            | Optio(...)esent [30] |       106.428 ns |        12.4962 ns |      0.6850 ns |       106.266 ns |   0.0060 |        - |        - |     112 B |
| WriteToSpan            | Optio(...)esent [30] |        47.192 ns |        25.0653 ns |      1.3739 ns |        46.409 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Optio(...)esent [30] |        62.201 ns |        38.7254 ns |      2.1227 ns |        61.325 ns |        - |        - |        - |         - |
| CalculateSize          | Optio(...)esent [30] |        13.600 ns |        16.4147 ns |      0.8997 ns |        13.255 ns |        - |        - |        - |         - |
| Clone                  | Optio(...)esent [30] |        28.449 ns |        15.5559 ns |      0.8527 ns |        28.262 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoNew           | Optio(...)esent [30] |        45.841 ns |         8.7858 ns |      0.4816 ns |        46.004 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoPopulated     | Optio(...)esent [30] |        42.560 ns |         6.6362 ns |      0.3638 ns |        42.382 ns |   0.0038 |        - |        - |      72 B |
| **New**                    | **Optional/empty**       |        **28.695 ns** |        **13.9950 ns** |      **0.7671 ns** |        **28.709 ns** |   **0.0038** |        **-** |        **-** |      **72 B** |
| ParseByteArray         | Optional/empty       |        97.844 ns |        19.5483 ns |      1.0715 ns |        97.567 ns |   0.0128 |        - |        - |     240 B |
| ParseSpan              | Optional/empty       |        29.778 ns |         9.4872 ns |      0.5200 ns |        29.762 ns |   0.0038 |        - |        - |      72 B |
| ToByteArray            | Optional/empty       |        50.545 ns |        21.0038 ns |      1.1513 ns |        50.753 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Optional/empty       |         6.849 ns |         0.6342 ns |      0.0348 ns |         6.829 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Optional/empty       |        10.080 ns |         2.0968 ns |      0.1149 ns |        10.074 ns |        - |        - |        - |         - |
| CalculateSize          | Optional/empty       |         3.341 ns |         2.1490 ns |      0.1178 ns |         3.287 ns |        - |        - |        - |         - |
| Clone                  | Optional/empty       |        39.531 ns |       150.6855 ns |      8.2596 ns |        41.571 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoNew           | Optional/empty       |        39.025 ns |        10.7799 ns |      0.5909 ns |        39.012 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoPopulated     | Optional/empty       |        35.640 ns |         4.4081 ns |      0.2416 ns |        35.645 ns |   0.0038 |        - |        - |      72 B |
| **New**                    | **Optional/populated**   |        **30.627 ns** |        **10.5322 ns** |      **0.5773 ns** |        **30.925 ns** |   **0.0038** |        **-** |        **-** |      **72 B** |
| ParseByteArray         | Optional/populated   |       229.471 ns |        22.5996 ns |      1.2388 ns |       229.200 ns |   0.0196 |        - |        - |     368 B |
| ParseSpan              | Optional/populated   |       161.982 ns |        63.6454 ns |      3.4886 ns |       160.553 ns |   0.0105 |        - |        - |     200 B |
| ToByteArray            | Optional/populated   |       131.857 ns |         8.1808 ns |      0.4484 ns |       131.719 ns |   0.0083 |        - |        - |     160 B |
| WriteToSpan            | Optional/populated   |        62.573 ns |         3.4095 ns |      0.1869 ns |        62.550 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Optional/populated   |       210.366 ns |     1,292.5209 ns |     70.8474 ns |       178.481 ns |        - |        - |        - |         - |
| CalculateSize          | Optional/populated   |        17.726 ns |        13.7031 ns |      0.7511 ns |        17.305 ns |        - |        - |        - |         - |
| Clone                  | Optional/populated   |        30.294 ns |        31.8582 ns |      1.7463 ns |        29.536 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoNew           | Optional/populated   |        45.929 ns |        10.8888 ns |      0.5969 ns |        45.623 ns |   0.0038 |        - |        - |      72 B |
| MergeIntoPopulated     | Optional/populated   |        41.273 ns |        22.1276 ns |      1.2129 ns |        40.686 ns |   0.0038 |        - |        - |      72 B |
| **New**                    | **Repeated/edge**        |       **105.983 ns** |        **33.9490 ns** |      **1.8609 ns** |       **106.823 ns** |   **0.0141** |        **-** |        **-** |     **264 B** |
| ParseByteArray         | Repeated/edge        |       957.424 ns |     1,277.5866 ns |     70.0288 ns |       935.506 ns |   0.0563 |        - |        - |    1056 B |
| ParseSpan              | Repeated/edge        |       848.199 ns |       200.2157 ns |     10.9745 ns |       848.743 ns |   0.0467 |        - |        - |     888 B |
| ToByteArray            | Repeated/edge        |       629.550 ns |       322.1079 ns |     17.6558 ns |       631.996 ns |   0.0134 |        - |        - |     256 B |
| WriteToSpan            | Repeated/edge        |       415.392 ns |        81.3363 ns |      4.4583 ns |       413.124 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Repeated/edge        |       561.116 ns |       318.8172 ns |     17.4754 ns |       554.248 ns |   0.0038 |        - |        - |      72 B |
| CalculateSize          | Repeated/edge        |       104.228 ns |        17.9671 ns |      0.9848 ns |       103.707 ns |        - |        - |        - |         - |
| Clone                  | Repeated/edge        |       725.751 ns |        75.3026 ns |      4.1276 ns |       726.830 ns |   0.0534 |        - |        - |    1000 B |
| MergeIntoNew           | Repeated/edge        |       356.524 ns |        84.2662 ns |      4.6189 ns |       357.724 ns |   0.0386 |        - |        - |     728 B |
| MergeIntoPopulated     | Repeated/edge        |       828.464 ns |       259.0370 ns |     14.1987 ns |       830.433 ns |   0.0534 |        - |        - |    1000 B |
| **New**                    | **Repeated/empty**       |       **104.631 ns** |        **29.0857 ns** |      **1.5943 ns** |       **104.797 ns** |   **0.0141** |        **-** |        **-** |     **264 B** |
| ParseByteArray         | Repeated/empty       |       169.962 ns |        20.6343 ns |      1.1310 ns |       169.801 ns |   0.0229 |        - |        - |     432 B |
| ParseSpan              | Repeated/empty       |       102.040 ns |        47.9944 ns |      2.6307 ns |       102.120 ns |   0.0141 |        - |        - |     264 B |
| ToByteArray            | Repeated/empty       |        54.882 ns |        21.2066 ns |      1.1624 ns |        55.525 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Repeated/empty       |        21.524 ns |         9.6051 ns |      0.5265 ns |        21.654 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Repeated/empty       |        27.902 ns |        98.8200 ns |      5.4167 ns |        25.004 ns |        - |        - |        - |         - |
| CalculateSize          | Repeated/empty       |         6.738 ns |         1.3561 ns |      0.0743 ns |         6.746 ns |        - |        - |        - |         - |
| Clone                  | Repeated/empty       |       170.976 ns |        85.2441 ns |      4.6725 ns |       168.901 ns |   0.0243 |        - |        - |     456 B |
| MergeIntoNew           | Repeated/empty       |       166.770 ns |        93.9895 ns |      5.1519 ns |       163.803 ns |   0.0141 |        - |        - |     264 B |
| MergeIntoPopulated     | Repeated/empty       |       222.359 ns |        38.7221 ns |      2.1225 ns |       221.236 ns |   0.0243 |        - |        - |     456 B |
| **New**                    | **Repeated/populated**   |       **111.774 ns** |        **50.1042 ns** |      **2.7464 ns** |       **112.044 ns** |   **0.0141** |        **-** |        **-** |     **264 B** |
| ParseByteArray         | Repeated/populated   |     6,073.321 ns |     1,493.2904 ns |     81.8523 ns |     6,045.405 ns |   0.4807 |   0.0076 |        - |    9024 B |
| ParseSpan              | Repeated/populated   |     6,713.890 ns |    10,346.3757 ns |    567.1198 ns |     6,404.584 ns |   0.4578 |        - |        - |    8856 B |
| ToByteArray            | Repeated/populated   |     5,141.849 ns |     2,118.7171 ns |    116.1340 ns |     5,081.488 ns |   0.1297 |        - |        - |    2512 B |
| WriteToSpan            | Repeated/populated   |     2,963.337 ns |       710.0091 ns |     38.9180 ns |     2,942.290 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Repeated/populated   |     4,919.704 ns |       617.6290 ns |     33.8543 ns |     4,906.865 ns |   0.0076 |        - |        - |     256 B |
| CalculateSize          | Repeated/populated   |     1,388.188 ns |       712.9856 ns |     39.0811 ns |     1,374.475 ns |        - |        - |        - |         - |
| Clone                  | Repeated/populated   |     1,806.461 ns |       643.1055 ns |     35.2508 ns |     1,786.504 ns |   0.2651 |   0.0038 |        - |    4984 B |
| MergeIntoNew           | Repeated/populated   |       863.443 ns |       540.3553 ns |     29.6187 ns |       850.658 ns |   0.1669 |   0.0019 |        - |    3128 B |
| MergeIntoPopulated     | Repeated/populated   |     3,574.953 ns |     2,056.2793 ns |    112.7116 ns |     3,621.643 ns |   0.6561 |   0.0229 |        - |   12296 B |
| **New**                    | **RepeatedItem/edge**    |        **25.798 ns** |        **35.1049 ns** |      **1.9242 ns** |        **24.942 ns** |   **0.0020** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | RepeatedItem/edge    |       245.896 ns |       164.9844 ns |      9.0433 ns |       244.657 ns |   0.0153 |        - |        - |     288 B |
| ParseSpan              | RepeatedItem/edge    |       167.991 ns |        24.1576 ns |      1.3242 ns |       168.158 ns |   0.0062 |        - |        - |     120 B |
| ToByteArray            | RepeatedItem/edge    |       178.122 ns |        80.1184 ns |      4.3916 ns |       179.712 ns |   0.0081 |        - |        - |     152 B |
| WriteToSpan            | RepeatedItem/edge    |       101.437 ns |        62.5551 ns |      3.4289 ns |       100.860 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | RepeatedItem/edge    |       165.627 ns |        39.1323 ns |      2.1450 ns |       164.834 ns |   0.0038 |        - |        - |      72 B |
| CalculateSize          | RepeatedItem/edge    |        20.759 ns |         7.2236 ns |      0.3960 ns |        20.643 ns |        - |        - |        - |         - |
| Clone                  | RepeatedItem/edge    |        23.615 ns |       105.6329 ns |      5.7901 ns |        21.140 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | RepeatedItem/edge    |        33.688 ns |         9.7782 ns |      0.5360 ns |        33.886 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | RepeatedItem/edge    |        27.889 ns |        27.5290 ns |      1.5090 ns |        27.889 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **RepeatedItem/empty**   |        **24.263 ns** |        **18.1249 ns** |      **0.9935 ns** |        **24.014 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | RepeatedItem/empty   |        82.363 ns |        40.1188 ns |      2.1990 ns |        81.108 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | RepeatedItem/empty   |        23.092 ns |         3.7149 ns |      0.2036 ns |        23.202 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | RepeatedItem/empty   |        57.265 ns |       151.6902 ns |      8.3147 ns |        56.422 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | RepeatedItem/empty   |         5.132 ns |         1.8562 ns |      0.1017 ns |         5.163 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | RepeatedItem/empty   |         8.690 ns |         5.1418 ns |      0.2818 ns |         8.603 ns |        - |        - |        - |         - |
| CalculateSize          | RepeatedItem/empty   |         2.056 ns |         0.0771 ns |      0.0042 ns |         2.056 ns |        - |        - |        - |         - |
| Clone                  | RepeatedItem/empty   |        19.764 ns |        10.4353 ns |      0.5720 ns |        19.574 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | RepeatedItem/empty   |        62.910 ns |       664.5671 ns |     36.4272 ns |        43.380 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | RepeatedItem/empty   |        26.834 ns |         7.3185 ns |      0.4012 ns |        26.999 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Repea(...)lated [22]** |        **23.235 ns** |        **10.5630 ns** |      **0.5790 ns** |        **23.019 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Repea(...)lated [22] |       122.197 ns |        45.7727 ns |      2.5090 ns |       120.837 ns |   0.0126 |        - |        - |     240 B |
| ParseSpan              | Repea(...)lated [22] |        64.293 ns |        29.0832 ns |      1.5941 ns |        63.955 ns |   0.0038 |        - |        - |      72 B |
| ToByteArray            | Repea(...)lated [22] |        73.474 ns |        13.9187 ns |      0.7629 ns |        73.468 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Repea(...)lated [22] |        21.383 ns |         1.4168 ns |      0.0777 ns |        21.384 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Repea(...)lated [22] |        34.680 ns |         7.8129 ns |      0.4283 ns |        34.717 ns |        - |        - |        - |         - |
| CalculateSize          | Repea(...)lated [22] |         9.177 ns |         2.4807 ns |      0.1360 ns |         9.129 ns |        - |        - |        - |         - |
| Clone                  | Repea(...)lated [22] |        20.088 ns |        11.8003 ns |      0.6468 ns |        19.839 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Repea(...)lated [22] |        33.788 ns |        14.3758 ns |      0.7880 ns |        33.487 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Repea(...)lated [22] |        24.391 ns |        10.7732 ns |      0.5905 ns |        24.055 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Scalars/edge-max**     |        **35.946 ns** |         **7.6875 ns** |      **0.4214 ns** |        **35.891 ns** |   **0.0055** |        **-** |        **-** |     **104 B** |
| ParseByteArray         | Scalars/edge-max     |       254.920 ns |        94.2731 ns |      5.1674 ns |       254.575 ns |   0.0143 |        - |        - |     272 B |
| ParseSpan              | Scalars/edge-max     |       186.838 ns |        25.7232 ns |      1.4100 ns |       186.459 ns |   0.0055 |        - |        - |     104 B |
| ToByteArray            | Scalars/edge-max     |       217.443 ns |        18.5978 ns |      1.0194 ns |       217.420 ns |   0.0095 |        - |        - |     184 B |
| WriteToSpan            | Scalars/edge-max     |       149.978 ns |       100.6809 ns |      5.5187 ns |       152.555 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Scalars/edge-max     |       170.396 ns |       156.1215 ns |      8.5575 ns |       166.257 ns |        - |        - |        - |         - |
| CalculateSize          | Scalars/edge-max     |        23.039 ns |         6.4038 ns |      0.3510 ns |        23.142 ns |        - |        - |        - |         - |
| Clone                  | Scalars/edge-max     |        31.382 ns |        13.2270 ns |      0.7250 ns |        31.636 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoNew           | Scalars/edge-max     |        48.374 ns |        20.1041 ns |      1.1020 ns |        48.229 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoPopulated     | Scalars/edge-max     |        43.430 ns |        22.5492 ns |      1.2360 ns |        43.537 ns |   0.0055 |        - |        - |     104 B |
| **New**                    | **Scalars/edge-min**     |        **35.315 ns** |        **51.5901 ns** |      **2.8278 ns** |        **34.718 ns** |   **0.0055** |        **-** |        **-** |     **104 B** |
| ParseByteArray         | Scalars/edge-min     |       226.281 ns |        53.5747 ns |      2.9366 ns |       224.592 ns |   0.0145 |        - |        - |     272 B |
| ParseSpan              | Scalars/edge-min     |       159.340 ns |        30.1901 ns |      1.6548 ns |       159.917 ns |   0.0055 |        - |        - |     104 B |
| ToByteArray            | Scalars/edge-min     |       193.117 ns |       108.7768 ns |      5.9624 ns |       191.872 ns |   0.0088 |        - |        - |     168 B |
| WriteToSpan            | Scalars/edge-min     |       122.125 ns |        23.3227 ns |      1.2784 ns |       121.472 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Scalars/edge-min     |       164.382 ns |       128.4229 ns |      7.0393 ns |       161.781 ns |        - |        - |        - |         - |
| CalculateSize          | Scalars/edge-min     |        16.717 ns |         4.1441 ns |      0.2272 ns |        16.711 ns |        - |        - |        - |         - |
| Clone                  | Scalars/edge-min     |        51.866 ns |       104.7233 ns |      5.7402 ns |        51.602 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoNew           | Scalars/edge-min     |        49.903 ns |        21.5397 ns |      1.1807 ns |        50.334 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoPopulated     | Scalars/edge-min     |        45.226 ns |        26.4619 ns |      1.4505 ns |        45.156 ns |   0.0055 |        - |        - |     104 B |
| **New**                    | **Scalars/empty**        |        **35.983 ns** |        **50.1388 ns** |      **2.7483 ns** |        **35.215 ns** |   **0.0055** |        **-** |        **-** |     **104 B** |
| ParseByteArray         | Scalars/empty        |       100.989 ns |        87.9197 ns |      4.8192 ns |        99.570 ns |   0.0145 |        - |        - |     272 B |
| ParseSpan              | Scalars/empty        |        36.847 ns |        29.7460 ns |      1.6305 ns |        36.368 ns |   0.0055 |        - |        - |     104 B |
| ToByteArray            | Scalars/empty        |        56.497 ns |        75.4664 ns |      4.1366 ns |        55.096 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Scalars/empty        |        11.276 ns |         2.0256 ns |      0.1110 ns |        11.233 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Scalars/empty        |        13.955 ns |         2.4094 ns |      0.1321 ns |        13.920 ns |        - |        - |        - |         - |
| CalculateSize          | Scalars/empty        |         6.356 ns |         1.1868 ns |      0.0651 ns |         6.359 ns |        - |        - |        - |         - |
| Clone                  | Scalars/empty        |        31.464 ns |        19.2541 ns |      1.0554 ns |        31.504 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoNew           | Scalars/empty        |        46.909 ns |        12.2686 ns |      0.6725 ns |        46.975 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoPopulated     | Scalars/empty        |        41.596 ns |        10.4611 ns |      0.5734 ns |        41.576 ns |   0.0055 |        - |        - |     104 B |
| **New**                    | **Scalars/populated**    |        **35.131 ns** |        **18.1212 ns** |      **0.9933 ns** |        **34.723 ns** |   **0.0055** |        **-** |        **-** |     **104 B** |
| ParseByteArray         | Scalars/populated    |       250.535 ns |       160.4312 ns |      8.7938 ns |       246.288 ns |   0.0143 |        - |        - |     272 B |
| ParseSpan              | Scalars/populated    |       176.949 ns |        11.0690 ns |      0.6067 ns |       177.057 ns |   0.0055 |        - |        - |     104 B |
| ToByteArray            | Scalars/populated    |       187.642 ns |        50.0380 ns |      2.7428 ns |       188.287 ns |   0.0093 |        - |        - |     176 B |
| WriteToSpan            | Scalars/populated    |       111.551 ns |        28.4246 ns |      1.5581 ns |       110.881 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Scalars/populated    |       140.135 ns |        20.3778 ns |      1.1170 ns |       140.104 ns |        - |        - |        - |         - |
| CalculateSize          | Scalars/populated    |        20.451 ns |         8.4884 ns |      0.4653 ns |        20.193 ns |        - |        - |        - |         - |
| Clone                  | Scalars/populated    |        31.286 ns |         6.9220 ns |      0.3794 ns |        31.247 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoNew           | Scalars/populated    |        49.241 ns |        23.0407 ns |      1.2629 ns |        49.746 ns |   0.0055 |        - |        - |     104 B |
| MergeIntoPopulated     | Scalars/populated    |        44.827 ns |        16.0236 ns |      0.8783 ns |        44.414 ns |   0.0055 |        - |        - |     104 B |
| **New**                    | **Strings/edge**         |        **26.648 ns** |        **11.6493 ns** |      **0.6385 ns** |        **26.692 ns** |   **0.0030** |        **-** |        **-** |      **56 B** |
| ParseByteArray         | Strings/edge         |   581,829.042 ns |    30,328.7709 ns |  1,662.4223 ns |   582,033.256 ns | 248.0469 | 248.0469 | 248.0469 | 2098069 B |
| ParseSpan              | Strings/edge         |   692,163.209 ns |   634,519.7882 ns | 34,780.1708 ns |   677,401.040 ns | 246.0938 | 246.0938 | 246.0938 | 2097914 B |
| ToByteArray            | Strings/edge         |   674,915.734 ns |    31,163.8463 ns |  1,708.1956 ns |   675,337.112 ns | 178.7109 | 178.7109 | 178.7109 | 1050695 B |
| WriteToSpan            | Strings/edge         |   275,623.488 ns |    84,735.3445 ns |  4,644.6302 ns |   277,374.952 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Strings/edge         | 1,408,708.212 ns | 1,479,740.2204 ns | 81,109.5550 ns | 1,418,579.416 ns | 160.1563 | 160.1563 | 160.1563 | 1049186 B |
| CalculateSize          | Strings/edge         |    72,777.796 ns |    57,085.1303 ns |  3,129.0286 ns |    72,413.147 ns |        - |        - |        - |         - |
| Clone                  | Strings/edge         |        30.112 ns |         6.7660 ns |      0.3709 ns |        29.909 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoNew           | Strings/edge         |        46.520 ns |        22.6322 ns |      1.2405 ns |        46.682 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoPopulated     | Strings/edge         |        43.034 ns |         3.8809 ns |      0.2127 ns |        43.136 ns |   0.0030 |        - |        - |      56 B |
| **New**                    | **Strings/empty**        |        **26.210 ns** |         **7.5039 ns** |      **0.4113 ns** |        **26.031 ns** |   **0.0030** |        **-** |        **-** |      **56 B** |
| ParseByteArray         | Strings/empty        |        86.168 ns |        14.8077 ns |      0.8117 ns |        85.938 ns |   0.0119 |        - |        - |     224 B |
| ParseSpan              | Strings/empty        |        28.078 ns |        24.3921 ns |      1.3370 ns |        28.708 ns |   0.0030 |        - |        - |      56 B |
| ToByteArray            | Strings/empty        |        47.739 ns |        14.3015 ns |      0.7839 ns |        47.370 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Strings/empty        |         8.036 ns |         3.8100 ns |      0.2088 ns |         7.923 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Strings/empty        |         9.979 ns |         3.5770 ns |      0.1961 ns |         9.913 ns |        - |        - |        - |         - |
| CalculateSize          | Strings/empty        |         3.157 ns |         2.6138 ns |      0.1433 ns |         3.086 ns |        - |        - |        - |         - |
| Clone                  | Strings/empty        |        29.156 ns |         9.7563 ns |      0.5348 ns |        29.381 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoNew           | Strings/empty        |        36.568 ns |        30.5766 ns |      1.6760 ns |        35.737 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoPopulated     | Strings/empty        |        34.645 ns |         6.3354 ns |      0.3473 ns |        34.520 ns |   0.0030 |        - |        - |      56 B |
| **New**                    | **Strings/populated**    |        **28.827 ns** |        **19.3098 ns** |      **1.0584 ns** |        **29.312 ns** |   **0.0030** |        **-** |        **-** |      **56 B** |
| ParseByteArray         | Strings/populated    |     7,303.401 ns |       610.9204 ns |     33.4866 ns |     7,299.330 ns |   1.7929 |   0.1984 |        - |   33664 B |
| ParseSpan              | Strings/populated    |     7,292.476 ns |     5,076.1246 ns |    278.2395 ns |     7,287.537 ns |   1.7853 |   0.1907 |        - |   33496 B |
| ToByteArray            | Strings/populated    |     8,017.635 ns |     1,548.9003 ns |     84.9004 ns |     7,974.299 ns |   0.8850 |        - |        - |   16792 B |
| WriteToSpan            | Strings/populated    |     4,453.082 ns |     5,282.0879 ns |    289.5291 ns |     4,511.209 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Strings/populated    |    15,519.641 ns |     5,750.1297 ns |    315.1840 ns |    15,550.584 ns |   0.8850 |        - |        - |   16760 B |
| CalculateSize          | Strings/populated    |       863.332 ns |       148.8774 ns |      8.1605 ns |       860.729 ns |        - |        - |        - |         - |
| Clone                  | Strings/populated    |        32.008 ns |         8.4005 ns |      0.4605 ns |        31.824 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoNew           | Strings/populated    |        46.434 ns |        25.6557 ns |      1.4063 ns |        46.284 ns |   0.0030 |        - |        - |      56 B |
| MergeIntoPopulated     | Strings/populated    |        45.532 ns |        14.4582 ns |      0.7925 ns |        45.346 ns |   0.0030 |        - |        - |      56 B |
| **New**                    | **Unkno(...)/edge [22]** |        **22.682 ns** |         **3.8896 ns** |      **0.2132 ns** |        **22.763 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Unkno(...)/edge [22] |       252.902 ns |       308.5910 ns |     16.9149 ns |       244.037 ns |   0.0153 |        - |        - |     288 B |
| ParseSpan              | Unkno(...)/edge [22] |       164.363 ns |        14.0333 ns |      0.7692 ns |       163.968 ns |   0.0062 |        - |        - |     120 B |
| ToByteArray            | Unkno(...)/edge [22] |       355.332 ns |     2,333.1246 ns |    127.8864 ns |       421.841 ns |   0.0081 |        - |        - |     152 B |
| WriteToSpan            | Unkno(...)/edge [22] |        95.328 ns |         3.3291 ns |      0.1825 ns |        95.404 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)/edge [22] |       166.909 ns |       131.5154 ns |      7.2088 ns |       164.795 ns |   0.0038 |        - |        - |      72 B |
| CalculateSize          | Unkno(...)/edge [22] |        21.789 ns |         8.2623 ns |      0.4529 ns |        21.529 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)/edge [22] |        19.882 ns |        13.3916 ns |      0.7340 ns |        19.676 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Unkno(...)/edge [22] |        34.793 ns |        10.3999 ns |      0.5701 ns |        34.488 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Unkno(...)/edge [22] |        28.507 ns |         5.5053 ns |      0.3018 ns |        28.370 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Unkno(...)empty [23]** |        **22.900 ns** |        **17.4012 ns** |      **0.9538 ns** |        **22.768 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Unkno(...)empty [23] |        81.044 ns |        27.3608 ns |      1.4997 ns |        81.844 ns |   0.0111 |        - |        - |     208 B |
| ParseSpan              | Unkno(...)empty [23] |        22.792 ns |         5.4390 ns |      0.2981 ns |        22.930 ns |   0.0021 |        - |        - |      40 B |
| ToByteArray            | Unkno(...)empty [23] |        45.560 ns |        30.7836 ns |      1.6874 ns |        45.904 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Unkno(...)empty [23] |         5.025 ns |         2.7407 ns |      0.1502 ns |         4.993 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)empty [23] |         8.013 ns |         3.8640 ns |      0.2118 ns |         7.931 ns |        - |        - |        - |         - |
| CalculateSize          | Unkno(...)empty [23] |         2.076 ns |         1.9684 ns |      0.1079 ns |         2.025 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)empty [23] |        20.667 ns |        14.7410 ns |      0.8080 ns |        20.746 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Unkno(...)empty [23] |        32.802 ns |         7.8629 ns |      0.4310 ns |        33.038 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Unkno(...)empty [23] |        26.829 ns |         4.7879 ns |      0.2624 ns |        26.690 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Unkno(...)lated [27]** |        **23.492 ns** |         **9.5618 ns** |      **0.5241 ns** |        **23.569 ns** |   **0.0021** |        **-** |        **-** |      **40 B** |
| ParseByteArray         | Unkno(...)lated [27] |       125.835 ns |        19.8536 ns |      1.0882 ns |       126.410 ns |   0.0126 |        - |        - |     240 B |
| ParseSpan              | Unkno(...)lated [27] |        69.516 ns |        84.6514 ns |      4.6400 ns |        71.428 ns |   0.0038 |        - |        - |      72 B |
| ToByteArray            | Unkno(...)lated [27] |        71.842 ns |        45.0350 ns |      2.4685 ns |        71.005 ns |   0.0051 |        - |        - |      96 B |
| WriteToSpan            | Unkno(...)lated [27] |        20.553 ns |         4.6379 ns |      0.2542 ns |        20.687 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)lated [27] |        33.034 ns |         3.8316 ns |      0.2100 ns |        32.977 ns |        - |        - |        - |         - |
| CalculateSize          | Unkno(...)lated [27] |         9.851 ns |         3.7669 ns |      0.2065 ns |         9.878 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)lated [27] |        20.062 ns |        14.2034 ns |      0.7785 ns |        19.727 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoNew           | Unkno(...)lated [27] |        34.348 ns |        12.0999 ns |      0.6632 ns |        34.665 ns |   0.0021 |        - |        - |      40 B |
| MergeIntoPopulated     | Unkno(...)lated [27] |        28.166 ns |         7.5949 ns |      0.4163 ns |        28.026 ns |   0.0021 |        - |        - |      40 B |
| **New**                    | **Unkno(...)/edge [22]** |        **28.379 ns** |         **6.6704 ns** |      **0.3656 ns** |        **28.173 ns** |   **0.0034** |        **-** |        **-** |      **64 B** |
| ParseByteArray         | Unkno(...)/edge [22] |       294.856 ns |       145.7503 ns |      7.9891 ns |       295.632 ns |   0.0167 |        - |        - |     312 B |
| ParseSpan              | Unkno(...)/edge [22] |       225.817 ns |        10.0258 ns |      0.5495 ns |       225.713 ns |   0.0076 |        - |        - |     144 B |
| ToByteArray            | Unkno(...)/edge [22] |       209.040 ns |        67.6581 ns |      3.7086 ns |       207.493 ns |   0.0093 |        - |        - |     176 B |
| WriteToSpan            | Unkno(...)/edge [22] |       123.329 ns |        30.8634 ns |      1.6917 ns |       122.709 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)/edge [22] |       314.683 ns |     1,626.7347 ns |     89.1668 ns |       364.857 ns |   0.0038 |        - |        - |      72 B |
| CalculateSize          | Unkno(...)/edge [22] |        25.560 ns |         3.5602 ns |      0.1951 ns |        25.553 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)/edge [22] |        27.391 ns |         5.3293 ns |      0.2921 ns |        27.284 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Unkno(...)/edge [22] |        38.998 ns |        11.6618 ns |      0.6392 ns |        39.323 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Unkno(...)/edge [22] |        35.003 ns |        12.9827 ns |      0.7116 ns |        35.130 ns |   0.0034 |        - |        - |      64 B |
| **New**                    | **Unkno(...)empty [23]** |        **29.289 ns** |        **14.5101 ns** |      **0.7953 ns** |        **29.689 ns** |   **0.0034** |        **-** |        **-** |      **64 B** |
| ParseByteArray         | Unkno(...)empty [23] |        85.478 ns |         3.2932 ns |      0.1805 ns |        85.557 ns |   0.0124 |        - |        - |     232 B |
| ParseSpan              | Unkno(...)empty [23] |        54.827 ns |       742.5578 ns |     40.7021 ns |        33.084 ns |   0.0034 |        - |        - |      64 B |
| ToByteArray            | Unkno(...)empty [23] |        46.768 ns |        19.3855 ns |      1.0626 ns |        47.125 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Unkno(...)empty [23] |         7.568 ns |         6.4759 ns |      0.3550 ns |         7.430 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)empty [23] |         9.296 ns |         1.9901 ns |      0.1091 ns |         9.240 ns |        - |        - |        - |         - |
| CalculateSize          | Unkno(...)empty [23] |         3.727 ns |         7.3532 ns |      0.4031 ns |         3.864 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)empty [23] |        26.202 ns |         6.1095 ns |      0.3349 ns |        26.285 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Unkno(...)empty [23] |        51.295 ns |       178.1383 ns |      9.7644 ns |        49.717 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Unkno(...)empty [23] |        31.720 ns |         4.3395 ns |      0.2379 ns |        31.786 ns |   0.0034 |        - |        - |      64 B |
| **New**                    | **Unkno(...)lated [27]** |        **28.202 ns** |        **19.0973 ns** |      **1.0468 ns** |        **28.030 ns** |   **0.0034** |        **-** |        **-** |      **64 B** |
| ParseByteArray         | Unkno(...)lated [27] |       212.569 ns |        36.8583 ns |      2.0203 ns |       211.527 ns |   0.0169 |        - |        - |     320 B |
| ParseSpan              | Unkno(...)lated [27] |       149.129 ns |        78.9307 ns |      4.3265 ns |       147.105 ns |   0.0081 |        - |        - |     152 B |
| ToByteArray            | Unkno(...)lated [27] |       126.982 ns |        33.8280 ns |      1.8542 ns |       126.204 ns |   0.0072 |        - |        - |     136 B |
| WriteToSpan            | Unkno(...)lated [27] |        55.851 ns |         5.5203 ns |      0.3026 ns |        55.735 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Unkno(...)lated [27] |       124.309 ns |         8.7756 ns |      0.4810 ns |       124.558 ns |   0.0019 |        - |        - |      40 B |
| CalculateSize          | Unkno(...)lated [27] |        22.179 ns |         0.6836 ns |      0.0375 ns |        22.190 ns |        - |        - |        - |         - |
| Clone                  | Unkno(...)lated [27] |        25.394 ns |        13.9450 ns |      0.7644 ns |        25.429 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoNew           | Unkno(...)lated [27] |        41.385 ns |        17.1670 ns |      0.9410 ns |        41.527 ns |   0.0034 |        - |        - |      64 B |
| MergeIntoPopulated     | Unkno(...)lated [27] |        37.716 ns |        12.3448 ns |      0.6767 ns |        37.787 ns |   0.0034 |        - |        - |      64 B |
| **New**                    | **Wrapp(...)esent [30]** |        **31.922 ns** |         **0.2249 ns** |      **0.0123 ns** |        **31.917 ns** |   **0.0047** |        **-** |        **-** |      **88 B** |
| ParseByteArray         | Wrapp(...)esent [30] |       171.495 ns |        53.2514 ns |      2.9189 ns |       169.859 ns |   0.0136 |        - |        - |     256 B |
| ParseSpan              | Wrapp(...)esent [30] |       112.703 ns |        20.3663 ns |      1.1163 ns |       113.011 ns |   0.0045 |        - |        - |      88 B |
| ToByteArray            | Wrapp(...)esent [30] |       146.962 ns |       179.9715 ns |      9.8648 ns |       151.206 ns |   0.0055 |        - |        - |     104 B |
| WriteToSpan            | Wrapp(...)esent [30] |        64.286 ns |        10.5795 ns |      0.5799 ns |        64.343 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Wrapp(...)esent [30] |        83.666 ns |        40.0815 ns |      2.1970 ns |        82.527 ns |        - |        - |        - |         - |
| CalculateSize          | Wrapp(...)esent [30] |        27.193 ns |         1.9499 ns |      0.1069 ns |        27.157 ns |        - |        - |        - |         - |
| Clone                  | Wrapp(...)esent [30] |        31.730 ns |         9.1289 ns |      0.5004 ns |        31.498 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoNew           | Wrapp(...)esent [30] |        45.299 ns |         5.6870 ns |      0.3117 ns |        45.179 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoPopulated     | Wrapp(...)esent [30] |        43.102 ns |         6.1466 ns |      0.3369 ns |        43.207 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Wrappers/empty**       |        **63.272 ns** |       **600.0709 ns** |     **32.8919 ns** |        **45.247 ns** |   **0.0047** |        **-** |        **-** |      **88 B** |
| ParseByteArray         | Wrappers/empty       |        95.942 ns |        87.7023 ns |      4.8073 ns |        93.997 ns |   0.0136 |        - |        - |     256 B |
| ParseSpan              | Wrappers/empty       |        34.317 ns |        13.4943 ns |      0.7397 ns |        34.109 ns |   0.0047 |        - |        - |      88 B |
| ToByteArray            | Wrappers/empty       |        47.000 ns |        21.6476 ns |      1.1866 ns |        46.758 ns |   0.0047 |        - |        - |      88 B |
| WriteToSpan            | Wrappers/empty       |         6.883 ns |         4.8528 ns |      0.2660 ns |         6.774 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Wrappers/empty       |         8.825 ns |         1.7832 ns |      0.0977 ns |         8.769 ns |        - |        - |        - |         - |
| CalculateSize          | Wrappers/empty       |         4.162 ns |         0.7585 ns |      0.0416 ns |         4.157 ns |        - |        - |        - |         - |
| Clone                  | Wrappers/empty       |        33.032 ns |        13.0181 ns |      0.7136 ns |        33.319 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoNew           | Wrappers/empty       |        42.140 ns |         9.4755 ns |      0.5194 ns |        42.139 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoPopulated     | Wrappers/empty       |        40.125 ns |        17.9822 ns |      0.9857 ns |        40.228 ns |   0.0047 |        - |        - |      88 B |
| **New**                    | **Wrappers/populated**   |        **33.158 ns** |         **5.9290 ns** |      **0.3250 ns** |        **33.056 ns** |   **0.0047** |        **-** |        **-** |      **88 B** |
| ParseByteArray         | Wrappers/populated   |       270.930 ns |        75.0013 ns |      4.1111 ns |       269.569 ns |   0.0205 |        - |        - |     384 B |
| ParseSpan              | Wrappers/populated   |       204.714 ns |        22.1970 ns |      1.2167 ns |       204.084 ns |   0.0114 |        - |        - |     216 B |
| ToByteArray            | Wrappers/populated   |       548.996 ns |     8,524.7580 ns |    467.2708 ns |       286.935 ns |   0.0086 |        - |        - |     168 B |
| WriteToSpan            | Wrappers/populated   |       158.354 ns |        53.6231 ns |      2.9393 ns |       157.002 ns |        - |        - |        - |         - |
| WriteToSegmentedWriter | Wrappers/populated   |       210.555 ns |        30.2304 ns |      1.6570 ns |       209.608 ns |        - |        - |        - |         - |
| CalculateSize          | Wrappers/populated   |        53.288 ns |         4.5999 ns |      0.2521 ns |        53.372 ns |        - |        - |        - |         - |
| Clone                  | Wrappers/populated   |        31.542 ns |        14.2550 ns |      0.7814 ns |        31.855 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoNew           | Wrappers/populated   |        45.896 ns |         9.0395 ns |      0.4955 ns |        45.920 ns |   0.0047 |        - |        - |      88 B |
| MergeIntoPopulated     | Wrappers/populated   |        52.457 ns |         1.6660 ns |      0.0913 ns |        52.406 ns |   0.0047 |        - |        - |      88 B |
