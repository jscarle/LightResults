using BenchmarkDotNet.Attributes;

namespace LightResults.ComparisonBenchmarks;

// ReSharper disable RedundantTypeArgumentsOfMethod
public partial class Benchmarks
{
    private static readonly Ardalis.Result.Result ArdalisResultResultOk = Ardalis.Result.Result.Success();
    private static readonly Ardalis.Result.Result ArdalisResultResultFail = Ardalis.Result.Result.Error();
    private static readonly Ardalis.Result.Result ArdalisResultResultFailWithErrorMessage = Ardalis.Result.Result.Error(ErrorMessage);
    private static readonly Ardalis.Result.Result<int> ArdalisResultResultTValueOk = Ardalis.Result.Result<int>.Success(ResultValue);
    private static readonly Ardalis.Result.Result<int> ArdalisResultResultTValueFail = Ardalis.Result.Result<int>.Error();
    private static readonly Ardalis.Result.Result<int> ArdalisResultResultTValueFailWithErrorMessage = Ardalis.Result.Result<int>.Error(ErrorMessage);

    [Benchmark]
    [BenchmarkCategory("A01: Returning a successful result")]
    public void C_ArdalisResult_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result.Success();
    }

    [Benchmark]
    [BenchmarkCategory("B01: String representation of a successful result")]
    public void C_ArdalisResult_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A02: Returning a successful value result")]
    public void C_ArdalisResult_Result_OkTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result<int>.Success(ResultValue);
    }

    [Benchmark]
    [BenchmarkCategory("B02: String representation of a successful value result")]
    public void C_ArdalisResult_Result_OkTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultTValueOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A03: Returning a failed result")]
    public void C_ArdalisResult_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result.Error();
    }

    [Benchmark]
    [BenchmarkCategory("B03: String representation of a failed result")]
    public void C_ArdalisResult_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A04: Returning a failed result with an error message")]
    public void C_ArdalisResult_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result.Error(ErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("B04: String representation of a failed result with an error message")]
    public void C_ArdalisResult_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A05: Returning a failed value result")]
    public void C_ArdalisResult_Result_FailTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result<int>.Error();
    }

    [Benchmark]
    [BenchmarkCategory("B05: String representation of a failed value result")]
    public void C_ArdalisResult_Result_FailTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultTValueFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A06: Returning a failed value result with an error message")]
    public void C_ArdalisResult_Result_FailTValue_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result<int>.Error(ErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("B06: String representation of a failed value result with an error message")]
    public void C_ArdalisResult_Result_FailTValue_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("C01: Determining if a result is successful")]
    public void C_ArdalisResult_Result_IsSuccess()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultOk.IsSuccess;
    }

    [Benchmark]
    [BenchmarkCategory("C02: Retrieving the value")]
    public void C_ArdalisResult_Result_Value()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultTValueOk.Value;
    }

    [Benchmark]
    [BenchmarkCategory("C03: Determining if a result is failed")]
    public void C_ArdalisResult_Result_IsFailed()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = !ArdalisResultResultFail.IsSuccess;
    }

    [Benchmark]
    [BenchmarkCategory("C04: Determining if a result contains a specific error")]
    public void C_ArdalisResult_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultFailWithErrorMessage.Errors.Any(errorMessage => errorMessage.Equals(ErrorMessage));
    }

    [Benchmark]
    [BenchmarkCategory("C05: Retrieving the first error")]
    public void C_ArdalisResult_Result_FirstError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultFailWithErrorMessage.Errors.First();
    }
}
