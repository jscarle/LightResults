using BenchmarkDotNet.Attributes;

namespace LightResults.DevelopBenchmarks;

[MemoryDiagnoser]
public class Benchmarks
{
    private const int ResultValue = 0;
    private const string ErrorMessage = "An unknown error occured.";
    private static readonly Error Error = new(ErrorMessage);
    private static readonly Result FailedResult = Result.Fail(Error);
    
    [Params(100_000)]
    public int Iterations { get; set; }

    [Benchmark]
    public void Develop_ResultBaseHasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FailedResult.HasError<Error>();
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
            _ = Result.Fail(Error);
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
            _ = Result<int>.Fail(Error);
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
            _ = Result.Fail<int>(Error);
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
}
