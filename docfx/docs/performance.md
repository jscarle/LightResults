# Performance

This library prioritizes performance as a core design principle. While it doesn't aim to replace feature-rich libraries like 
[Michael Altmann](https://github.com/altmann)'s excellent [FluentResults](https://github.com/altmann/FluentResults) or 
[Steve Smith](https://github.com/ardalis)'s popular [Ardalis.Result](https://github.com/ardalis/result). LightResults strives 
for exceptional performance by intentionally simplifying its API.

## Comparison

Below are comparisons of LightResults against other result pattern implementations.

```
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2605)
13th Gen Intel Core i7-13700KF, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.101
  [Host]   : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2
  .NET 9.0 : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2

Job=.NET 9.0  Runtime=.NET 9.0  IterationTime=250ms
Iterations=10
``` 
### Returning results

#### Returning a successful result
| Method                            |      Mean | Ratio | Allocated |
|-----------------------------------|----------:|------:|----------:|
| LightResults: `Result.Success()`  |  2.229 ns |  1.00 |         - |
| FluentResults: `Result.Ok()`      | 46.144 ns | 20.70 |     560 B |
| ArdalisResult: `Result.Success()` | 43.977 ns | 19.73 |     720 B |

#### Returning a successful value result
| Method                                    |        Mean | Ratio | Allocated |
|-------------------------------------------|------------:|------:|----------:|
| LightResults: `Result.Success<T>(value)`  |    2.279 ns |  1.00 |         - |
| FluentResults: `Result.Ok<T>(value)`      |  146.461 ns | 64.28 |    1120 B |
| ArdalisResult: `Result<T>.Success(value)` |   40.390 ns | 17.73 |     640 B |

#### Returning a failed result
| Method                           |       Mean |  Ratio | Allocated |
|----------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Failure()` |   2.171 ns |   1.00 |         - |
| FluentResults: `Result.Fail("")` | 245.472 ns | 113.05 |    2640 B |
| ArdalisResult: `Result.Error()`  |  44.872 ns |  20.66 |     720 B |

#### Returning a failed result with an error message
| Method                                       |       Mean | Ratio | Allocated | Alloc Ratio |
|----------------------------------------------|-----------:|------:|----------:|------------:|
| LightResults: `Result.Failure(errorMessage)` |  17.455 ns |  1.00 |     240 B |        1.00 |
| FluentResults: `Result.Fail(errorMessage)`   | 119.614 ns |  5.89 |    1120 B |        4.67 |
| ArdalisResult: `Result.Error(errorMessage)`  |  63.892 ns |  3.12 |    1040 B |        4.33 |

#### Returning a failed value result
| Method                              |       Mean |  Ratio | Allocated |
|-------------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Failure<T>()` |   2.462 ns |   1.00 |         - |
| FluentResults: `Result.Fail<T>("")` | 247.613 ns | 100.62 |    2720 B |
| ArdalisResult: `Result<T>.Error()`  |  44.701 ns |  18.16 |     640 B |

#### Returning a failed value result with an error message
| Method                                          |       Mean | Ratio | Allocated | Alloc Ratio |
|-------------------------------------------------|-----------:|------:|----------:|------------:|
| LightResults: `Result.Failure<T>(errorMessage)` |  21.735 ns |  1.00 |     240 B |        1.00 |
| FluentResults: `Result.Fail<T>(errorMessage)`   | 120.527 ns |  5.55 |    1200 B |        5.00 |
| ArdalisResult: `Result<T>.Error(errorMessage)`  |  58.888 ns |  2.71 |     960 B |        4.00 |

### Checking results

#### Determining if a result is successful
| Method                             |      Mean | Ratio | Allocated |
|------------------------------------|----------:|------:|----------:|
| LightResults: `result.IsSuccess()` |  2.759 ns |  1.00 |         - |
| FluentResults: `result.IsSuccess`  | 95.084 ns | 34.47 |     480 B |
| ArdalisResult: `result.IsSuccess`  |  2.299 ns |  0.83 |         - |

#### Retrieving the value
| Method                                          |      Mean | Ratio | Allocated |
|-------------------------------------------------|----------:|------:|----------:|
| LightResults: `result.IsSuccess(out var value)` |  2.276 ns |  1.00 |         - |
| FluentResults: `result.Value`                   | 92.877 ns | 40.81 |     480 B |
| ArdalisResult: `result.Value`                   |  2.769 ns |  1.22 |         - |

#### Determining if a result is failed
| Method                             |       Mean | Ratio | Allocated |
|------------------------------------|-----------:|------:|----------:|
| LightResults: `result.IsFailure()` |   2.771 ns |  1.00 |         - |
| FluentResults: `result.IsFailed`   | 144.895 ns | 52.28 |     880 B |
| ArdalisResult: `!result.IsSuccess` |   2.288 ns |  0.83 |         - |

#### Determining if a result contains a specific error
| Method                                                                                |       Mean | Ratio | Allocated |
|---------------------------------------------------------------------------------------|-----------:|------:|----------:|
| LightResults: `result.HasError<T>()`                                                  |  11.698 ns |  1.00 |         - |
| FluentResults: `result.HasError<T>()`                                                 | 735.001 ns | 62.83 |    3600 B |
| ArdalisResult: `result.Errors.Any(errorMessage => errorMessage.Equals(ErrorMessage))` |  20.135 ns |  1.72 |         - |

#### Retrieving the first error
| Method                                 |       Mean |  Ratio | Allocated |
|----------------------------------------|-----------:|-------:|----------:|
| LightResults: `result.Error`           |   2.484 ns |   1.00 |         - |
| FluentResults: `result.Errors[0]`      | 249.327 ns | 100.36 |    1520 B |
| ArdalisResult: `result.Errors.First()` |  95.526 ns |  38.45 |         - |

### Getting results as strings

#### String representation of a successful result
| Method                                       |       Mean |  Ratio | Allocated |
|----------------------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Success().ToString()`  |   2.336 ns |   1.00 |         - |
| FluentResults: `Result.Ok().ToString()`      | 457.159 ns | 195.72 |    1440 B |
| ArdalisResult: `Result.Success().ToString()` |  12.930 ns |   5.54 |         - |

#### String representation of a successful value result
| Method                                               |       Mean | Ratio | Allocated | Alloc Ratio |
|------------------------------------------------------|-----------:|------:|----------:|------------:|
| LightResults: `Result.Success<T>(value).ToString()`  |  94.038 ns |  1.00 |    1040 B |        1.00 |
| FluentResults: `Result.Ok<T>(value).ToString()`      | 672.014 ns | 17.15 |    3040 B |        2.92 |
| ArdalisResult: `Result<T>.Success(value).ToString()` |  12.597 ns |  0.13 |         - |        0.00 |

#### String representation of a failed result
| Method                                      |         Mean |  Ratio | Allocated |
|---------------------------------------------|-------------:|-------:|----------:|
| LightResults: `Result.Failure().ToString()` |     4.965 ns |   1.00 |         - |
| FluentResults: `Result.Fail("").ToString()` | 1,000.265 ns | 201.46 |    3840 B |
| ArdalisResult: `Result.Error().ToString()`  |    12.659 ns |   2.55 |         - |

#### String representation of a failed result with an error message
| Method                                                  |         Mean | Ratio | Allocated | Alloc Ratio |
|---------------------------------------------------------|-------------:|------:|----------:|------------:|
| LightResults: `Result.Failure(errorMessage).ToString()` |   100.338 ns |  1.00 |    1520 B |        1.00 |
| FluentResults: `Result.Fail(errorMessage).ToString()`   | 1,564.981 ns | 15.60 |    9280 B |        6.11 |
| ArdalisResult: `Result.Error(errorMessage).ToString()`  |    12.787 ns |  0.13 |         - |        0.00 |

#### String representation of a failed value result
| Method                                         |         Mean |  Ratio | Allocated |
|------------------------------------------------|-------------:|-------:|----------:|
| LightResults: `Result.Failure<T>().ToString()` |     5.016 ns |   1.00 |         - |
| FluentResults: `Result.Fail<T>("").ToString()` | 1,302.858 ns | 259.72 |    5760 B |
| ArdalisResult: `Result<T>.Error().ToString()`  |    12.742 ns |   2.54 |         - |

#### String representation of a failed value result with an error message
| Method                                                     |         Mean | Ratio | Allocated | Alloc Ratio |
|------------------------------------------------------------|-------------:|------:|----------:|------------:|
| LightResults: `Result.Failure<T>(errorMessage).ToString()` |   101.344 ns |  1.00 |    1520 B |        1.00 |
| FluentResults: `Result<T>.Error(errorMessage).ToString()`  | 1,789.207 ns | 17.65 |   12080 B |        7.95 |
| ArdalisResult: `Result.Fail<T>(errorMessage).ToString()`   |    12.848 ns |  0.13 |         - |        0.00 |
