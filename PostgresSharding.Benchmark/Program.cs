using BenchmarkDotNet.Running;
using PostgresSharding.Benchmark;

BenchmarkRunner.Run<ReadWriteBenchmark>();
