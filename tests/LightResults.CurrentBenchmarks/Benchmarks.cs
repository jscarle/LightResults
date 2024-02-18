﻿using BenchmarkDotNet.Attributes;

namespace LightResults.CurrentBenchmarks;

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
    public void Current_ResultBaseIndexer()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FailedResult.Errors[0];
    }

    [Benchmark]
    public void Current_ResultBaseHasError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = FailedResult.HasError<Error>();
    }

    [Benchmark]
    public void Current_ResultOk()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Ok();
    }

    [Benchmark]
    public void Current_ResultFail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail();
    }

    [Benchmark]
    public void Current_ResultFailWithError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail(Error);
    }

    [Benchmark]
    public void Current_ResultTValueOk()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Ok(ResultValue);
    }

    [Benchmark]
    public void Current_ResultTValueFail()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Fail();
    }

    [Benchmark]
    public void Current_ResultTValueFailWithError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result<int>.Fail(Error);
    }

    [Benchmark]
    public void Current_ResultOkTValue()
    {
        // ReSharper disable once RedundantTypeArgumentsOfMethod
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Ok<int>(ResultValue);
    }

    [Benchmark]
    public void Current_ResultFailTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail<int>();
    }

    [Benchmark]
    public void Current_ResultFailWithErrorTValue()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = Result.Fail<int>(Error);
    }

    [Benchmark]
    public void Current_NewError()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = new Error();
    }

    [Benchmark]
    public void Current_NewErrorWithString()
    {
        for (var iteration = 0; iteration < Iterations; iteration++)
            _ = new Error(ErrorMessage);
    }
}
