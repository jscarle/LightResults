using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using LightResults.Common;

namespace LightResults.CurrentBenchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[HideColumns(Column.Job, Column.Iterations, Column.Error, Column.StdDev, Column.Gen0, Column.Gen1, Column.Gen2)]
public class Benchmarks
{
    [Params(100_000)]
    public int Iterations { get; set; }

    private const int ResultValue = 0;
    private const string ErrorMessage = "An unknown error occured.";
    private static readonly Error ErrorWithMessage = new(ErrorMessage);
    private static readonly Result ResultOk = Result.Ok();
    private static readonly Result ResultFail = Result.Fail();
    private static readonly Result ResultFailWithMessage = Result.Fail(ErrorWithMessage);
    private static readonly Result<int> ResultTValueOk = Result<int>.Ok(ResultValue);
    private static readonly Result<int> ResultTValueFail = Result<int>.Fail();
    private static readonly Result<int> ResultTValueFailWithMessage = Result<int>.Fail(ErrorWithMessage);
    private static readonly CustomResult CustomResultOk = CustomResult.Ok();
    private static readonly CustomResult CustomResultFail = CustomResult.Fail();
    private static readonly CustomResult CustomResultFailWithMessage = CustomResult.Fail(ErrorWithMessage);

    [Benchmark]
    public void Develop_ResultBaseIndexer()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithMessage.Errors[0];
    }

    [Benchmark]
    public void Develop_ResultBaseHasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithMessage.HasError<Error>();
    }

    [Benchmark]
    public void Develop_ResultOkToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultOk.ToString();
    }

    [Benchmark]
    public void Develop_ResultFailToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFail.ToString();
    }

    [Benchmark]
    public void Develop_ResultFailWithMessageToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultFailWithMessage.ToString();
    }

    [Benchmark]
    public void Develop_ResultTValueOkToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueOk.ToString();
    }

    [Benchmark]
    public void Develop_ResultTValueFailToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueFail.ToString();
    }


    [Benchmark]
    public void Develop_ResultTValueFailWithMessageToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = ResultTValueFailWithMessage.ToString();
    }

    [Benchmark]
    public void Develop_CustomResultOkToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = CustomResultOk.ToString();
    }

    [Benchmark]
    public void Develop_CustomResultFailToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = CustomResultFail.ToString();
    }

    [Benchmark]
    public void Develop_CustomResultFailWithMessageToString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = CustomResultFailWithMessage.ToString();
    }

    [Benchmark]
    public void Develop_ResultOk()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Ok();
    }

    [Benchmark]
    public void Develop_ResultFail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail();
    }

    [Benchmark]
    public void Develop_ResultFailWithError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail(ErrorWithMessage);
    }

    [Benchmark]
    public void Develop_ResultTValueOk()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Ok(ResultValue);
    }

    [Benchmark]
    public void Develop_ResultTValueFail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Fail();
    }

    [Benchmark]
    public void Develop_ResultTValueFailWithError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Fail(ErrorWithMessage);
    }

    [Benchmark]
    public void Develop_ResultOkTValue()
    {
        // ReSharper disable once RedundantTypeArgumentsOfMethod
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Ok<int>(ResultValue);
    }

    [Benchmark]
    public void Develop_ResultFailTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail<int>();
    }

    [Benchmark]
    public void Develop_ResultFailWithErrorTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail<int>(ErrorWithMessage);
    }

    [Benchmark]
    public void Develop_NewError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = new Error();
    }

    [Benchmark]
    public void Develop_NewErrorWithString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = new Error(ErrorMessage);
    }

    private sealed class CustomResult : ResultBase
    {
        private CustomResult()
        {
        }

        private CustomResult(IError error) : base(error)
        {
        }

        public static CustomResult Ok()
        {
            return new CustomResult();
        }

        public static CustomResult Fail()
        {
            return new CustomResult(new Error());
        }

        public static CustomResult Fail(IError error)
        {
            return new CustomResult(error);
        }
    }
}