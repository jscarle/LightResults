using BenchmarkDotNet.Attributes;
using Rascal;
using Rascal.Errors;

namespace LightResults.ComparisonBenchmarks;

public partial class Benchmarks
{
    // ReSharper disable RedundantTypeArgumentsOfMethod
    // ReSharper disable RedundantNameQualifier
    private static readonly Rascal.Error RascalError = new Rascal.Errors.StringError("");
    private static readonly Rascal.Error RascalErrorWithErrorMessage = new Rascal.Errors.StringError(ErrorMessage);
    private static readonly Rascal.Result<bool> RascalResultOk = Prelude.Ok(true);
    private static readonly Rascal.Result<bool> RascalResultFail = Prelude.Err<bool>(RascalError);
    private static readonly Rascal.Result<bool> RascalResultFailWithErrorMessage = Prelude.Err<bool>(RascalErrorWithErrorMessage);
    private static readonly Rascal.Result<int> RascalResultTValueOk = Prelude.Ok<int>(ResultValue);
    private static readonly Rascal.Result<int> RascalResultTValueFail = Prelude.Err<int>(RascalError);
    private static readonly Rascal.Result<int> RascalResultTValueFailWithErrorMessage = Prelude.Err<int>(RascalErrorWithErrorMessage);

    [Benchmark]
    [BenchmarkCategory("Result.Ok()")]
    public void Rascal_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Ok(true);
    }

    [Benchmark]
    [BenchmarkCategory("Result.Ok().ToString()")]
    public void Rascal_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Ok()")]
    public void Rascal_ResultTValue_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Ok<int>(ResultValue);
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Ok().ToString()")]
    public void Rascal_ResultTValue_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultTValueOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail()")]
    public void Rascal_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Err<bool>("");
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail().ToString()")]
    public void Rascal_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail()")]
    public void Rascal_ResultTValue_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Err<int>("");
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail().ToString()")]
    public void Rascal_ResultTValue_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultTValueFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail(errorMessage)")]
    public void Rascal_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Err<bool>(RascalErrorWithErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail(errorMessage).ToString()")]
    public void Rascal_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail(errorMessage)")]
    public void Rascal_ResultTValue_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Prelude.Err<int>(RascalErrorWithErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail(errorMessage).ToString()")]
    public void Rascal_ResultTValue_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("result.HasError<T>()")]
    public void Rascal_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultFailWithErrorMessage.TryGetError(out var error) && error is StringError;
    }

    [Benchmark]
    [BenchmarkCategory("result.Errors[0]")]
    public void Rascal_Result_FirstError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = RascalResultFailWithErrorMessage.TryGetError(out _);
    }
}