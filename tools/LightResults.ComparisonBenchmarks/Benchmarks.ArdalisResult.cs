using BenchmarkDotNet.Attributes;

namespace LightResults.ComparisonBenchmarks;

public partial class Benchmarks
{
    // ReSharper disable RedundantTypeArgumentsOfMethod
    private static readonly Ardalis.Result.Result ArdalisResultResultOk = Ardalis.Result.Result.Success();
    private static readonly Ardalis.Result.Result ArdalisResultResultFail = Ardalis.Result.Result.Error();
    private static readonly Ardalis.Result.Result ArdalisResultResultFailWithErrorMessage = Ardalis.Result.Result.Error(ErrorMessage);
    private static readonly Ardalis.Result.Result<int> ArdalisResultResultTValueOk = Ardalis.Result.Result<int>.Success(ResultValue);
    private static readonly Ardalis.Result.Result<int> ArdalisResultResultTValueFail = Ardalis.Result.Result<int>.Error();
    private static readonly Ardalis.Result.Result<int> ArdalisResultResultTValueFailWithErrorMessage = Ardalis.Result.Result<int>.Error(ErrorMessage);

    [Benchmark]
    [BenchmarkCategory("Result.Ok()")]
    public void ArdalisResult_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result.Success();
    }

    [Benchmark]
    [BenchmarkCategory("Result.Ok().ToString()")]
    public void ArdalisResult_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Ok()")]
    public void ArdalisResult_ResultTValue_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result<int>.Success(ResultValue);
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Ok().ToString()")]
    public void ArdalisResult_ResultTValue_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultTValueOk.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail()")]
    public void ArdalisResult_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result.Error();
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail().ToString()")]
    public void ArdalisResult_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail()")]
    public void ArdalisResult_ResultTValue_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result<int>.Error();
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail().ToString()")]
    public void ArdalisResult_ResultTValue_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultTValueFail.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail(errorMessage)")]
    public void ArdalisResult_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result.Error(ErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("Result.Fail(errorMessage).ToString()")]
    public void ArdalisResult_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail(errorMessage)")]
    public void ArdalisResult_ResultTValue_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Ardalis.Result.Result<int>.Error(ErrorMessage);
    }

    [Benchmark]
    [BenchmarkCategory("Result<T>.Fail(errorMessage).ToString()")]
    public void ArdalisResult_ResultTValue_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark]
    [BenchmarkCategory("result.HasError<T>()")]
    public void ArdalisResult_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultFailWithErrorMessage.Errors.Any(errorMessage => errorMessage.Equals(ErrorMessage));
    }

    [Benchmark]
    [BenchmarkCategory("result.Errors[0]")]
    public void ArdalisResult_Result_FirstError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ArdalisResultResultFailWithErrorMessage.Errors.First();
    }
}
