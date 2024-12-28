using BenchmarkDotNet.Attributes;

namespace LightResults.ComparisonBenchmarks;

// ReSharper disable RedundantTypeArgumentsOfMethod
public partial class Benchmarks
{
    private static readonly FluentResults.Error FluentResultsErrorWithErrorMessage = new(ErrorMessage);
    private static readonly FluentResults.Result FluentResultsResultOk = FluentResults.Result.Ok();
    private static readonly FluentResults.Result FluentResultsResultFail = FluentResults.Result.Fail("");
    private static readonly FluentResults.Result FluentResultsResultFailWithErrorMessage = FluentResults.Result.Fail(FluentResultsErrorWithErrorMessage);
    private static readonly FluentResults.Result<int> FluentResultsResultTValueOk = FluentResults.Result.Ok<int>(ResultValue);
    private static readonly FluentResults.Result<int> FluentResultsResultTValueFail = FluentResults.Result.Fail<int>("");

    private static readonly FluentResults.Result<int> FluentResultsResultTValueFailWithErrorMessage =
        FluentResults.Result.Fail<int>(FluentResultsErrorWithErrorMessage);

    [Benchmark]
    [BenchmarkCategory("A01: Returning a successful result")]
    public void B_FluentResults_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Ok();
    }

    [Benchmark]
    [BenchmarkCategory("B01: String representation of a successful result")]
    public void B_FluentResults_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A02: Returning a successful value result")]
    public void B_FluentResults_Result_OkTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Ok<int>(ResultValue);
    }

    [Benchmark]
    [BenchmarkCategory("B02: String representation of a successful value result")]
    public void B_FluentResults_Result_OkTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultTValueOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A03: Returning a failed result")]
    public void B_FluentResults_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Fail("");
    }

    [Benchmark]
    [BenchmarkCategory("B03: String representation of a failed result")]
    public void B_FluentResults_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A04: Returning a failed result with an error message")]
    public void B_FluentResults_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Fail(FluentResultsErrorWithErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("B04: String representation of a failed result with an error message")]
    public void B_FluentResults_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A05: Returning a failed value result")]
    public void B_FluentResults_Result_FailTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Fail<int>("");
    }

    [Benchmark]
    [BenchmarkCategory("B05: String representation of a failed value result")]
    public void B_FluentResults_Result_FailTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultTValueFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A06: Returning a failed value result with an error message")]
    public void B_FluentResults_Result_FailTValue_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResults.Result.Fail<int>(FluentResultsErrorWithErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("B06: String representation of a failed value result with an error message")]
    public void B_FluentResults_Result_FailTValue_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("C01: Determining if a result is successful")]
    public void B_FluentResults_Result_IsSuccess()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultOk.IsSuccess;
    }

    [Benchmark]
    [BenchmarkCategory("C02: Retrieving the value")]
    public void B_FluentResults_Result_Value()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultTValueOk.Value;
    }

    [Benchmark]
    [BenchmarkCategory("C03: Determining if a result is failed")]
    public void B_FluentResults_Result_IsFailed()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultFail.IsFailed;
    }

    [Benchmark]
    [BenchmarkCategory("C04: Determining if a result contains a specific error")]
    public void B_FluentResults_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultFailWithErrorMessage.HasError<FluentResults.Error>();
    }

    [Benchmark]
    [BenchmarkCategory("C05: Retrieving the first error")]
    public void B_FluentResults_Result_FirstError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FluentResultsResultFailWithErrorMessage.Errors[0];
    }
}
