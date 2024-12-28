using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace LightResults.DevelopBenchmarks;

// ReSharper disable RedundantTypeArgumentsOfMethod
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net90)]
[IterationTime(250)]
[HideColumns(Column.Job, Column.Iterations, Column.Error, Column.StdDev, Column.Median, Column.RatioSD, Column.Gen0, Column.Gen1, Column.Gen2)]
public class Benchmarks
{
    [Params(10)]
    public int Iterations { get; set; }

    private const int ResultValue = 0;
    private const string ErrorMessage = "An unknown error occured.";
    private static readonly Error EmptyError = new();
    private static readonly Error ErrorWithErrorMessage = new(ErrorMessage);
    private static readonly Result ResultOk = Result.Success();
    private static readonly Result ResultFail = Result.Failure();
    private static readonly Result ResultFailWithErrorMessage = Result.Failure(ErrorWithErrorMessage);
    private static readonly Result<int> ResultTValueOk = Result.Success<int>(ResultValue);
    private static readonly Result<int> ResultTValueFail = Result.Failure<int>();
    private static readonly Result<int> ResultTValueFailWithErrorMessage = Result.Failure<int>(ErrorWithErrorMessage);

    [Benchmark]
    public void Develop_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Success();
    }

    [Benchmark]
    public void Develop_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultOk.ToString();
    }

    [Benchmark]
    public void Develop_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure();
    }

    [Benchmark]
    public void Develop_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFail.ToString();
    }

    [Benchmark]
    public void Develop_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure(ErrorWithErrorMessage);
    }

    [Benchmark]
    public void Develop_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithErrorMessage.ToString();
    }

    [Benchmark]
    public void Develop_Result_OkTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Success<int>(ResultValue);
    }

    [Benchmark]
    public void Develop_Result_OkTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueOk.ToString();
    }

    [Benchmark]
    public void Develop_Result_FailTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure<int>();
    }

    [Benchmark]
    public void Develop_Result_FailTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueFail.ToString();
    }

    [Benchmark]
    public void Develop_Result_FailTValue_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure<int>(ErrorWithErrorMessage);
    }

    [Benchmark]
    public void Develop_Result_FailTValue_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark]
    public void Develop_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithErrorMessage.HasError<Error>();
    }

    [Benchmark]
    public void Develop_Result_HasError_Out()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithErrorMessage.HasError<Error>(out _);
    }

    [Benchmark]
    public void Develop_Result_Error()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithErrorMessage.IsFailure(out _);
    }

    [Benchmark]
    public void Develop_Error_New()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = new Error();
    }

    [Benchmark]
    public void Develop_Error_New_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = EmptyError.ToString();
    }

    [Benchmark]
    public void Develop_Error_New_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = new Error(ErrorMessage);
    }

    [Benchmark]
    public void Develop_Error_New_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ErrorWithErrorMessage.ToString();
    }
}
