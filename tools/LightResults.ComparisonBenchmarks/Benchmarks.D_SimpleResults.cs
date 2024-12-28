using BenchmarkDotNet.Attributes;
using SimpleResults;

namespace LightResults.ComparisonBenchmarks;

// ReSharper disable RedundantTypeArgumentsOfMethod
public partial class Benchmarks
{
    private static readonly SimpleResults.Result SimpleResultsResultOk = SimpleResults.Result.Success();
    private static readonly SimpleResults.Result SimpleResultsResultFail = SimpleResults.Result.Failure();
    private static readonly SimpleResults.Result SimpleResultsResultFailWithErrorMessage = SimpleResults.Result.Failure(ErrorMessage);
    private static readonly SimpleResults.Result<int> SimpleResultsResultTValueOk = SimpleResults.Result.Success<int>(ResultValue);
    private static readonly SimpleResults.Result<int> SimpleResultsResultTValueFail = SimpleResults.Result.Failure();
    private static readonly SimpleResults.Result<int> SimpleResultsResultTValueFailWithErrorMessage = SimpleResults.Result.Failure(ErrorMessage);

    [Benchmark]
    [BenchmarkCategory("A01: Returning a successful result")]
    public void D_SimpleResults_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResults.Result.Success();
    }

    [Benchmark]
    [BenchmarkCategory("B01: String representation of a successful result")]
    public void D_SimpleResults_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A02: Returning a successful value result")]
    public void D_SimpleResults_Result_OkTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResults.Result.Success<int>(ResultValue);
    }

    [Benchmark]
    [BenchmarkCategory("B02: String representation of a successful value result")]
    public void D_SimpleResults_Result_OkTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultTValueOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A03: Returning a failed result")]
    public void D_SimpleResults_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResults.Result.Failure();
    }

    [Benchmark]
    [BenchmarkCategory("B03: String representation of a failed result")]
    public void D_SimpleResults_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A04: Returning a failed result with an error message")]
    public void D_SimpleResults_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResults.Result.Failure(ErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("B04: String representation of a failed result with an error message")]
    public void D_SimpleResults_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A05: Returning a failed value result")]
    public void D_SimpleResults_Result_FailTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = (SimpleResults.Result<int>)SimpleResults.Result.Failure();
    }

    [Benchmark]
    [BenchmarkCategory("B05: String representation of a failed value result")]
    public void D_SimpleResults_Result_FailTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultTValueFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A06: Returning a failed value result with an error message")]
    public void D_SimpleResults_Result_FailTValue_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = (SimpleResults.Result<int>)SimpleResults.Result.Failure(ErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("B06: String representation of a failed value result with an error message")]
    public void D_SimpleResults_Result_FailTValue_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("C01: Determining if a result is successful")]
    public void D_SimpleResults_Result_IsSuccess()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultOk.IsSuccess;
    }

    [Benchmark]
    [BenchmarkCategory("C02: Retrieving the value")]
    public void D_SimpleResults_Result_Value()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultTValueOk.Data;
    }

    [Benchmark]
    [BenchmarkCategory("C03: Determining if a result is failed")]
    public void D_SimpleResults_Result_IsFailed()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultFail.IsFailed;
    }

    [Benchmark]
    [BenchmarkCategory("C04: Determining if a result contains a specific error")]
    public void D_SimpleResults_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultFailWithErrorMessage.Status == ResultStatus.Failure;
    }

    [Benchmark]
    [BenchmarkCategory("C05: Retrieving the first error")]
    public void D_SimpleResults_Result_FirstError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = SimpleResultsResultFailWithErrorMessage.Errors.FirstOrDefault();
    }
}
