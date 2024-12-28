using BenchmarkDotNet.Attributes;
using Rascal;
using Rascal.Errors;

namespace LightResults.ComparisonBenchmarks;

// ReSharper disable RedundantTypeArgumentsOfMethod
// ReSharper disable RedundantNameQualifier
public partial class Benchmarks
{
    private static readonly Rascal.Error RascalError = new Rascal.Errors.StringError("");
    private static readonly Rascal.Error RascalErrorWithErrorMessage = new Rascal.Errors.StringError(ErrorMessage);
    private static readonly Rascal.Result<bool> RascalResultOk = Prelude.Ok(true);
    private static readonly Rascal.Result<bool> RascalResultFail = Prelude.Err<bool>(RascalError);
    private static readonly Rascal.Result<bool> RascalResultFailWithErrorMessage = Prelude.Err<bool>(RascalErrorWithErrorMessage);
    private static readonly Rascal.Result<int> RascalResultTValueOk = Prelude.Ok<int>(ResultValue);
    private static readonly Rascal.Result<int> RascalResultTValueFail = Prelude.Err<int>(RascalError);
    private static readonly Rascal.Result<int> RascalResultTValueFailWithErrorMessage = Prelude.Err<int>(RascalErrorWithErrorMessage);

    [Benchmark]
    [BenchmarkCategory("A01: Returning a successful result")]
    public void E_Rascal_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Ok(true);
    }

    [Benchmark]
    [BenchmarkCategory("B01: String representation of a successful result")]
    public void E_Rascal_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A02: Returning a successful value result")]
    public void E_Rascal_Result_OkTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Ok<int>(ResultValue);
    }

    [Benchmark]
    [BenchmarkCategory("B02: String representation of a successful value result")]
    public void E_Rascal_Result_OkTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultTValueOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A03: Returning a failed result")]
    public void E_Rascal_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Err<bool>("");
    }

    [Benchmark]
    [BenchmarkCategory("B03: String representation of a failed result")]
    public void E_Rascal_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A04: Returning a failed result with an error message")]
    public void E_Rascal_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Err<bool>(RascalErrorWithErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("B04: String representation of a failed result with an error message")]
    public void E_Rascal_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A05: Returning a failed value result")]
    public void E_Rascal_Result_FailTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Err<int>("");
    }

    [Benchmark]
    [BenchmarkCategory("B05: String representation of a failed value result")]
    public void E_Rascal_Result_FailTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultTValueFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("A06: Returning a failed value result with an error message")]
    public void E_Rascal_Result_FailTValue_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Err<int>(RascalErrorWithErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("B06: String representation of a failed value result with an error message")]
    public void E_Rascal_Result_FailTValue_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("C01: Determining if a result is successful")]
    public void E_Rascal_Result_IsSuccess()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultOk.IsOk;
    }

    [Benchmark]
    [BenchmarkCategory("C02: Retrieving the value")]
    public void E_Rascal_Result_Value()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultTValueOk.GetValueOrDefault();
    }

    [Benchmark]
    [BenchmarkCategory("C03: Determining if a result is failed")]
    public void E_Rascal_Result_IsFailed()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultFail.IsError;
    }

    [Benchmark]
    [BenchmarkCategory("C04: Determining if a result contains a specific error")]
    public void E_Rascal_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultFailWithErrorMessage.TryGetError(out var error) && error is StringError;
    }

    [Benchmark]
    [BenchmarkCategory("C05: Retrieving the first error")]
    public void E_Rascal_Result_FirstError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultFailWithErrorMessage.TryGetError(out _);
    }
}
