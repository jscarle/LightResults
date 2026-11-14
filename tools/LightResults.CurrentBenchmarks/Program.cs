using BenchmarkDotNet.Running;
using LightResults.CurrentBenchmarks;

BenchmarkSwitcher.FromAssembly(typeof(Benchmarks).Assembly).Run(args);
