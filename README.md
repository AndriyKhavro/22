## Setup

There are 3 sharding setups: FDW, Citus and no sharding. They have corresponding folders. Every folder has setup.ps1 script that runs docker compose and creates needed objects in corresponding servers.

## Generate data (1m rows)
```
dotnet run --project .\SqlDataGenerator\SqlDataGenerator.csproj -- 5432
dotnet run --project .\SqlDataGenerator\SqlDataGenerator.csproj -- 15432
dotnet run --project .\SqlDataGenerator\SqlDataGenerator.csproj -- citus
```

## Benchmark

```
dotnet run --project .\PostgresSharding.Benchmark\PostgresSharding.Benchmark.csproj -c Release
```

### Results

[Benchmark results](./BenchmarkDotNet.Artifacts/results/PostgresSharding.Benchmark.ReadWriteBenchmark-report-github.md)

Results show that Insert is faster without sharding, which is expected, since the benchmark runs Insert queries sequentially, no queries were run in parallel.

GetCount is dramatically slower for FDW.

GetById performance depends on whether the query is cross-partition or not. GetByIdAndCategory includes category_id in the query, and database engine can run the query on one partition. In any case, Citus outperforms both FDW and the setup without sharding.
