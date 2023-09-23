```

BenchmarkDotNet v0.13.8, Windows 11 (10.0.22621.2283/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900HX with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.306
  [Host]     : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2


```
| Method             | Connection | Mean         | Error      | StdDev     | Median       |
|------------------- |----------- |-------------:|-----------:|-----------:|-------------:|
| **Insert**             | **Citus**      |     **1.391 ms** |  **0.0350 ms** |  **0.0945 ms** |     **1.359 ms** |
| GetCount           | Citus      |    35.936 ms |  1.2284 ms |  3.6028 ms |    35.209 ms |
| GetById            | Citus      |    31.744 ms |  1.0659 ms |  3.0925 ms |    31.158 ms |
| GetByIdAndCategory | Citus      |    20.813 ms |  0.5438 ms |  1.5948 ms |    20.780 ms |
| **Insert**             | **Fdw**        |     **2.009 ms** |  **0.0399 ms** |  **0.1151 ms** |     **1.973 ms** |
| GetCount           | Fdw        | 1,135.720 ms | 18.1303 ms | 16.0720 ms | 1,136.902 ms |
| GetById            | Fdw        |    64.511 ms |  1.2327 ms |  1.6028 ms |    64.045 ms |
| GetByIdAndCategory | Fdw        |    32.854 ms |  0.4601 ms |  0.3842 ms |    32.681 ms |
| **Insert**             | **NoSharding** |     **1.170 ms** |  **0.0233 ms** |  **0.0611 ms** |     **1.152 ms** |
| GetCount           | NoSharding |    52.039 ms |  1.4899 ms |  4.3929 ms |    50.930 ms |
| GetById            | NoSharding |    40.533 ms |  1.1325 ms |  3.3215 ms |    40.131 ms |
| GetByIdAndCategory | NoSharding |    40.706 ms |  1.0783 ms |  3.1795 ms |    40.391 ms |
