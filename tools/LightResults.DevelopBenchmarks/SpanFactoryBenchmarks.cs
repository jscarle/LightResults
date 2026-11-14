using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;

namespace LightResults.DevelopBenchmarks;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 12)]
[IterationTime(150)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[HideColumns(Column.Job, Column.Gen0, Column.Gen1, Column.Gen2)]
public class SpanFactoryBenchmarks
{
    private const string ErrorMessage = "Validation failed.";
    private static readonly IError FirstError = new Error("Name is required.");
    private static readonly IError SecondError = new Error("Email is required.");
    private static readonly KeyValuePair<string, object?> FirstMetadata = new("Field", "Email");
    private static readonly KeyValuePair<string, object?> SecondMetadata = new("Code", "Required");

    [Benchmark(Baseline = true), BenchmarkCategory("Error metadata")]
    public Error LegacyErrorMetadata() => new(ErrorMessage, (IEnumerable<KeyValuePair<string, object?>>)[FirstMetadata, SecondMetadata]);

    [Benchmark, BenchmarkCategory("Error metadata")]
    public Error SpanErrorMetadata() => new(ErrorMessage, [FirstMetadata, SecondMetadata]);

    [Benchmark(Baseline = true), BenchmarkCategory("Result errors")]
    public Result LegacyResultErrors() => Result.Failure((IReadOnlyList<IError>)[FirstError, SecondError]);

    [Benchmark, BenchmarkCategory("Result errors")]
    public Result SpanResultErrors() => Result.Failure([FirstError, SecondError]);

    [Benchmark(Baseline = true), BenchmarkCategory("Result<T> errors")]
    public Result<int> LegacyValueResultErrors() => Result.Failure<int>((IReadOnlyList<IError>)[FirstError, SecondError]);

    [Benchmark, BenchmarkCategory("Result<T> errors")]
    public Result<int> SpanValueResultErrors() => Result.Failure<int>([FirstError, SecondError]);

    [Benchmark(Baseline = true), BenchmarkCategory("Result metadata")]
    public Result LegacyResultMetadata()
    {
        var error = new Error(ErrorMessage, (IEnumerable<KeyValuePair<string, object?>>)[FirstMetadata, SecondMetadata]);
        return Result.Failure(error);
    }

    [Benchmark, BenchmarkCategory("Result metadata")]
    public Result SpanResultMetadata() => Result.Failure(ErrorMessage, [FirstMetadata, SecondMetadata]);

    [Benchmark(Baseline = true), BenchmarkCategory("Result<T> metadata")]
    public Result<int> LegacyValueResultMetadata()
    {
        var error = new Error(ErrorMessage, (IEnumerable<KeyValuePair<string, object?>>)[FirstMetadata, SecondMetadata]);
        return Result.Failure<int>(error);
    }

    [Benchmark, BenchmarkCategory("Result<T> metadata")]
    public Result<int> SpanValueResultMetadata() => Result.Failure<int>(ErrorMessage, [FirstMetadata, SecondMetadata]);
}
