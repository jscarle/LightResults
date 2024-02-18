using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using LightResults.CurrentBenchmarks;

BenchmarkRunner.Run<Benchmarks>(ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.DisableOptimizationsValidator));
