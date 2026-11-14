using BenchmarkDotNet.Running;
using LightResults.DevelopBenchmarks;

BenchmarkSwitcher.FromAssembly(typeof(Benchmarks).Assembly).Run(args);
