using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace LightResults.ComparisonBenchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net90)]
[IterationTime(250)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[HideColumns(Column.Job, Column.Iterations, Column.Error, Column.StdDev, Column.Median, Column.RatioSD, Column.Gen0, Column.Gen1, Column.Gen2)]
public partial class Benchmarks
{
    [Params(10)]
    public int Iterations { get; set; }

    private const int ResultValue = 0;
    private const string ErrorMessage = "An unknown error occured.";
}
