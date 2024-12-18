using BenchmarkDotNet.Attributes;

namespace LightResults.ComparisonBenchmarks;

// ReSharper disable RedundantTypeArgumentsOfMethod
public partial class Benchmarks
{
    private static readonly Error LightResultsErrorWithErrorMessage = new(ErrorMessage);
    private static readonly Result LightResultsResultOk = Result.Success();
    private static readonly Result LightResultsResultFail = Result.Failure();
    private static readonly Result LightResultsResultFailWithErrorMessage = Result.Failure(LightResultsErrorWithErrorMessage);
    private static readonly Result<int> LightResultsResultTValueOk = Result.Success<int>(ResultValue);
    private static readonly Result<int> LightResultsResultTValueFail = Result.Failure<int>();
    private static readonly Result<int> LightResultsResultTValueFailWithErrorMessage = Result.Failure<int>(LightResultsErrorWithErrorMessage);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("A01: Returning a successful result")]
    public void A_LightResults_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Success();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("B01: String representation of a successful result")]
    public void A_LightResults_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultOk.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("A02: Returning a successful value result")]
    public void A_LightResults_Result_OkTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Success<int>(ResultValue);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("B02: String representation of a successful value result")]
    public void A_LightResults_Result_OkTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultTValueOk.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("A03: Returning a failed result")]
    public void A_LightResults_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("B03: String representation of a failed result")]
    public void A_LightResults_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultFail.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("A04: Returning a failed result with an error message")]
    public void A_LightResults_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure(LightResultsErrorWithErrorMessage);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("B04: String representation of a failed result with an error message")]
    public void A_LightResults_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultFailWithErrorMessage.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("A05: Returning a failed value result")]
    public void A_LightResults_Result_FailTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure<int>();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("B05: String representation of a failed value result")]
    public void A_LightResults_Result_FailTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultTValueFail.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("A06: Returning a failed value result with an error message")]
    public void A_LightResults_Result_FailTValue_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure<int>(LightResultsErrorWithErrorMessage);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("B06: String representation of a failed value result with an error message")]
    public void A_LightResults_Result_FailTValue_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("C01: Determining if a result is successful")]
    public void A_LightResults_Result_IsSuccess()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultOk.IsSuccess();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("C02: Retrieving the value")]
    public void A_LightResults_Result_Value()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultTValueOk.IsSuccess(out _);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("C03: Determining if a result is failed")]
    public void A_LightResults_Result_IsFailed()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultFail.IsSuccess();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("C04: Determining if a result contains a specific error")]
    public void A_LightResults_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultFailWithErrorMessage.HasError<Error>();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("C05: Retrieving the first error")]
    public void A_LightResults_Result_FirstError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = LightResultsResultFailWithErrorMessage.IsFailure(out _);
    }
}
