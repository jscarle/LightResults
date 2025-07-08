using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace LightResults.CurrentBenchmarks;

// ReSharper disable RedundantTypeArgumentsOfMethod
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net90)]
[IterationTime(250)]
[HideColumns(Column.Job, Column.Iterations, Column.Error, Column.StdDev, Column.Median, Column.RatioSD, Column.Gen0, Column.Gen1, Column.Gen2)]
public class Benchmarks
{
    [Params(10)]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int Iterations { get; set; }

    private const int ResultValue = 0;
    private const string ErrorMessage = "An unknown error occurred.";
    private static readonly Error EmptyError = new();
    private static readonly Error ErrorWithErrorMessage = new(ErrorMessage);
    private static readonly Result ResultSuccess = Result.Success();
    private static readonly Result ResultFailure = Result.Failure();
    private static readonly Result ResultFailureWithErrorMessage = Result.Failure(ErrorWithErrorMessage);
    private static readonly Result<int> ResultTValueSuccess = Result.Success<int>(ResultValue);
    private static readonly Result<int> ResultTValueFailure = Result.Failure<int>();
    private static readonly Result<int> ResultTValueFailureWithErrorMessage = Result.Failure<int>(ErrorWithErrorMessage);

    [Benchmark]
    public void Current_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Success();
    }

    [Benchmark]
    public void Current_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultSuccess.ToString();
    }

    [Benchmark]
    public void Current_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure();
    }

    [Benchmark]
    public void Current_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailure.ToString();
    }

    [Benchmark]
    public void Current_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure(ErrorWithErrorMessage);
    }

    [Benchmark]
    public void Current_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailureWithErrorMessage.ToString();
    }

    [Benchmark]
    public void Current_Result_OkTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Success<int>(ResultValue);
    }

    [Benchmark]
    public void Current_Result_OkTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueSuccess.ToString();
    }

    [Benchmark]
    public void Current_Result_FailTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure<int>();
    }

    [Benchmark]
    public void Current_Result_FailTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueFailure.ToString();
    }

    [Benchmark]
    public void Current_Result_FailTValue_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Failure<int>(ErrorWithErrorMessage);
    }

    [Benchmark]
    public void Current_Result_FailTValue_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueFailureWithErrorMessage.ToString();
    }

    [Benchmark]
    public void Current_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailureWithErrorMessage.HasError<Error>();
    }

    [Benchmark]
    public void Current_Result_Error()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailureWithErrorMessage.Errors.First();
    }

    [Benchmark]
    public void Current_Error_New()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = new Error();
    }

    [Benchmark]
    public void Current_Error_New_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = EmptyError.ToString();
    }

    [Benchmark]
    public void Current_Error_New_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = new Error(ErrorMessage);
    }

    [Benchmark]
    public void Current_Error_New_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ErrorWithErrorMessage.ToString();
    }
}
