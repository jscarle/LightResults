using BenchmarkDotNet.Attributes;

namespace LightResults.ComparisonBenchmarks;

public partial class Benchmarks
{
    // ReSharper disable RedundantTypeArgumentsOfMethod
    private static readonly Error LightResultsErrorWithErrorMessage = new(ErrorMessage);
    private static readonly Result LightResultsResultOk = Result.Ok();
    private static readonly Result LightResultsResultFail = Result.Fail();
    private static readonly Result LightResultsResultFailWithErrorMessage = Result.Fail(LightResultsErrorWithErrorMessage);
    private static readonly Result<int> LightResultsResultTValueOk = Result<int>.Ok(ResultValue);
    private static readonly Result<int> LightResultsResultTValueFail = Result<int>.Fail();
    private static readonly Result<int> LightResultsResultTValueFailWithErrorMessage = Result<int>.Fail(LightResultsErrorWithErrorMessage);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result.Ok()")]
    public void LightResults_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Ok();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result.Ok().ToString()")]
    public void LightResults_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultOk.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result<T>.Ok()")]
    public void LightResults_ResultTValue_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Ok(ResultValue);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result<T>.Ok().ToString()")]
    public void LightResults_ResultTValue_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultTValueOk.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result.Fail()")]
    public void LightResults_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result.Fail().ToString()")]
    public void LightResults_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultFail.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result<T>.Fail()")]
    public void LightResults_ResultTValue_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Fail();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result<T>.Fail().ToString()")]
    public void LightResults_ResultTValue_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultTValueFail.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result.Fail(errorMessage)")]
    public void LightResults_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail(LightResultsErrorWithErrorMessage);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result.Fail(errorMessage).ToString()")]
    public void LightResults_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultFailWithErrorMessage.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result<T>.Fail(errorMessage)")]
    public void LightResults_ResultTValue_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Fail(LightResultsErrorWithErrorMessage);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Result<T>.Fail(errorMessage).ToString()")]
    public void LightResults_ResultTValue_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("result.HasError<T>()")]
    public void LightResults_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultFailWithErrorMessage.HasError<Error>();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("result.Errors[0]")]
    public void LightResults_Result_FirstError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultFailWithErrorMessage.Errors[0];
    }
}
