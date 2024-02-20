using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using LightResults.ComparisonBenchmarks;

BenchmarkRunner.Run<Benchmarks>(ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.DisableOptimizationsValidator));