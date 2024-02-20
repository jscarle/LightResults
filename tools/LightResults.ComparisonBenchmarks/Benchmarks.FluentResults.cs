using BenchmarkDotNet.Attributes;

namespace LightResults.ComparisonBenchmarks;

public partial class Benchmarks
{
    // ReSharper disable RedundantTypeArgumentsOfMethod
    private static readonly FluentResults.Error FluentResultsErrorWithErrorMessage = new(ErrorMessage);
    private static readonly FluentResults.Result FluentResultsResultOk = FluentResults.Result.Ok();
    private static readonly FluentResults.Result FluentResultsResultFail = FluentResults.Result.Fail("");
    private static readonly FluentResults.Result FluentResultsResultFailWithErrorMessage = FluentResults.Result.Fail(FluentResultsErrorWithErrorMessage);
    private static readonly FluentResults.Result<int> FluentResultsResultTValueOk = FluentResults.Result.Ok<int>(ResultValue);
    private static readonly FluentResults.Result<int> FluentResultsResultTValueFail = FluentResults.Result.Fail<int>("");
    private static readonly FluentResults.Result<int> FluentResultsResultTValueFailWithErrorMessage = FluentResults.Result.Fail<int>(FluentResultsErrorWithErrorMessage);

    [Benchmark]
    [BenchmarkCategory("Result.Ok()")]
    public void FluentResults_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Ok();
    }

    [Benchmark]
    [BenchmarkCategory("Result.Ok().ToString()")]
    public void FluentResults_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Ok()")]
    public void FluentResults_ResultTValue_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Ok<int>(ResultValue);
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Ok().ToString()")]
    public void FluentResults_ResultTValue_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultTValueOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail()")]
    public void FluentResults_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Fail("");
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail().ToString()")]
    public void FluentResults_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail()")]
    public void FluentResults_ResultTValue_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Fail<int>("");
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail().ToString()")]
    public void FluentResults_ResultTValue_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultTValueFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail(errorMessage)")]
    public void FluentResults_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Fail(FluentResultsErrorWithErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail(errorMessage).ToString()")]
    public void FluentResults_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail(errorMessage)")]
    public void FluentResults_ResultTValue_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Fail<int>(FluentResultsErrorWithErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail(errorMessage).ToString()")]
    public void FluentResults_ResultTValue_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("result.HasError<T>()")]
    public void FluentResults_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultFailWithErrorMessage.HasError<FluentResults.Error>();
    }

    [Benchmark]
    [BenchmarkCategory("result.Errors[0]")]
    public void FluentResults_Result_FirstError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultFailWithErrorMessage.Errors[0];
    }
}
