using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace LightResults.DevelopBenchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[HideColumns(Column.Job, Column.Iterations, Column.Error, Column.StdDev, Column.Gen0, Column.Gen1, Column.Gen2)]
public class Benchmarks
{
    [Params(100_000)]
    public int Iterations { get; set; } // ReSharper disable RedundantTypeArgumentsOfMethod

    private const int ResultValue = 0;
    private const string ErrorMessage = "An unknown error occured.";
    private static readonly Error EmptyError = new();
    private static readonly Error ErrorWithErrorMessage = new(ErrorMessage);
    private static readonly Result ResultOk = Result.Ok();
    private static readonly Result ResultFail = Result.Fail();
    private static readonly Result ResultFailWithErrorMessage = Result.Fail(ErrorWithErrorMessage);
    private static readonly Result<int> ResultTValueOk = Result<int>.Ok(ResultValue);
    private static readonly Result<int> ResultTValueFail = Result<int>.Fail();
    private static readonly Result<int> ResultTValueFailWithErrorMessage = Result<int>.Fail(ErrorWithErrorMessage);

    [Benchmark]
    public void Develop_Result_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Ok();
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
            _ = Result.Fail();
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
            _ = Result.Fail(ErrorWithErrorMessage);
    }

    [Benchmark]
    public void Develop_Result_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithErrorMessage.ToString();
    }

    [Benchmark]
    public void Develop_Result_HasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithErrorMessage.HasError<Error>();
    }

    [Benchmark]
    public void Develop_Result_Error()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithErrorMessage.Error;
    }

    [Benchmark]
    public void Develop_ResultTValue_Ok()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Ok(ResultValue);
    }

    [Benchmark]
    public void Develop_ResultTValue_Ok_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueOk.ToString();
    }

    [Benchmark]
    public void Develop_ResultTValue_Fail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Fail();
    }

    [Benchmark]
    public void Develop_ResultTValue_Fail_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueFail.ToString();
    }

    [Benchmark]
    public void Develop_ResultTValue_Fail_WithErrorMessage()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Fail(ErrorWithErrorMessage);
    }

    [Benchmark]
    public void Develop_ResultTValue_Fail_WithErrorMessage_ToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueFailWithErrorMessage.ToString();
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