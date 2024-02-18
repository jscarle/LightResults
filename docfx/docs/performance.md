# Performance

This library prioritizes performance as a core design principle. While it doesn't aim to replace feature-rich libraries like 
[Michael Altmann](https://github.com/altmann)'s excellent [FluentResults](https://github.com/altmann/FluentResults) or 
[Steve Smith](https://github.com/ardalis)'s popular [Ardalis.Result](https://github.com/ardalis/result). LightResults strives 
for exceptional performance by intentionally simplifying its API.

## Comparison

Below are comparisons of LightResults against other result pattern implementations.

```
BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3155/23H2/2023Update/SunValley3)
13th Gen Intel Core i7-13700KF, 1 CPU, 24 logical and 16 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  Iterations : 100 000
```

### vs FluentResults

| LightResults Method           |      Mean | Allocated | FluentResults Method          |        Mean | Allocated |
|:------------------------------|----------:|----------:|-------------------------------|------------:|----------:|
| `result.Errors[0]`            |  27.48 μs |         - | `result.Errors[0]`            |  4,550.7 μs |  17.55 MB |
| `result.HasError<TError>()`   |  94.08 μs |         - | `result.HasError<TError>()`   | 12,508.8 μs |  40.44 MB |
| `Result.Ok()`                 |  18.84 μs |         - | `Result.Ok()`                 |    984.4 μs |   5.34 MB |
| `Result.Fail()`               |  18.83 μs |         - | `Result.Fail("")`             |  4,678.3 μs |  25.18 MB |
| `Result.Fail(IError)`         | 539.99 μs |   7.63 MB | `Result.Fail(IError)`         |  2,337.3 μs |  10.68 MB |
| `Result.Ok<TValue>(TValue)`   | 192.49 μs |   3.05 MB | `Result.Ok<TValue>(TValue)`   |  3,260.1 μs |  11.44 MB |
| `Result.Fail<TValue>()`       |  18.77 μs |         - | `Result.Fail<TValue>("")`     |  4,700.1 μs |  25.94 MB |
| `Result.Fail<TValue>(IError)` | 551.69 μs |   8.39 MB | `Result.Fail<TValue>(IError)` |  2,212.3 μs |  11.44 MB |
| `new Error()`                 | 224.80 μs |   3.05 MB | `new Error("")`               |  1,829.7 μs |   14.5 MB |
| `new Error(errorMessage)`     | 225.54 μs |   3.05 MB | `new Error(errorMessage)`     |  1,851.8 μs |   14.5 MB |

### vs Ardalis.Result

| LightResults Method           |      Mean | Allocated | Ardalis.Result Method                            |       Mean | Allocated |
|:------------------------------|----------:|----------:|--------------------------------------------------|-----------:|----------:|
| `result.Errors[0]`            |  27.48 μs |         - | `result.Errors.First()`                          | 1,260.4 μs |         - |
| `result.HasError<TError>()`   |  94.08 μs |         - | `result.Errors.Any(x => x.Equals(errorMessage))` |   725.0 μs |   3.05 MB |
| `Result.Ok()`                 |  18.84 μs |         - | `Result.Success()`                               |   890.7 μs |  12.21 MB |
| `Result.Fail()`               |  18.83 μs |         - | `Result.Error()`                                 |   751.4 μs |  12.21 MB |
| `Result.Fail(IError)`         | 539.99 μs |   7.63 MB | `Result.Error(errorMessage)`                     | 1,143.1 μs |  15.26 MB |
| `Result.Ok<TValue>(TValue)`   | 192.49 μs |   3.05 MB | `Result.Success<TValue>(TValue)`                 |   860.0 μs |  11.45 MB |
| `Result.Fail<TValue>()`       |  18.77 μs |         - | `Result.Error<TValue>()`                         |   887.8 μs |  11.45 MB |
| `Result.Fail<TValue>(IError)` | 551.69 μs |   8.39 MB | `Result.Error<TValue>(errorMessage)`             | 1,079.1 μs |  14.50 MB |
