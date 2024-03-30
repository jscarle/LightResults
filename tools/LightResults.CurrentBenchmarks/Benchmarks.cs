using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace LightResults.CurrentBenchmarks;

// ReSharper disable RedundantTypeArgumentsOfMethod
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[IterationTime(250)]
[HideColumns(Column.Job, Column.Iterations, Column.Error, Column.StdDev, Column.Median, Column.RatioSD, Column.Gen0, Column.Gen1, Column.Gen2)]
public class Benchmarks
{
    private const int ResultValue = 0;
    private const string ErrorMessage = "An unknown error occured.";
    private static readonly Error EmptyError = new();
    private static readonly Error ErrorWithErrorMessage = new(ErrorMessage);
    private static readonly Result ResultOk = Result.Ok();
    private static readonly Result ResultFail = Result.Fail();
    private static readonly Result ResultFailWithErrorMessage = Result.Fail(ErrorWithErrorMessage);
    private static readonly Result<int> ResultTValueOk = Result.Ok<int>(ResultValue);
    private static readonly Result<int> ResultTValueFail = Result.Fail<int>();
    private static readonly Result<int> ResultTValueFailWithErrorMessage = Result.Fail<int>(ErrorWithErrorMessage);

    [Params(10)]
    public int Iterations { get; set; }

    [Benchmark]
    public void Current_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Ok();
    }

    [Benchmark]
    public void Current_Result_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultOk.ToString();
    }

    [Benchmark]
    public void Current_Result_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail();
    }

    [Benchmark]
    public void Current_Result_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFail.ToString();
    }

    [Benchmark]
    public void Current_Result_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail(ErrorWithErrorMessage);
    }

    [Benchmark]
    public void Current_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithErrorMessage.ToString();
    }

    [Benchmark]
    public void Current_Result_OkTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Ok<int>(ResultValue);
    }

    [Benchmark]
    public void Current_Result_OkTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueOk.ToString();
    }

    [Benchmark]
    public void Current_Result_FailTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail<int>();
    }

    [Benchmark]
    public void Current_Result_FailTValue_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueFail.ToString();
    }

    [Benchmark]
    public void Current_Result_FailTValue_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail<int>(ErrorWithErrorMessage);
    }

    [Benchmark]
    public void Current_Result_FailTValue_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueFailWithErrorMessage.ToString();
    }

    [Benchmark]
    public void Current_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithErrorMessage.HasError<Error>();
    }

    [Benchmark]
    public void Current_Result_Error()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithErrorMessage.Error;
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
