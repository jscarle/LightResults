# Performance

This library prioritizes performance as a core design principle. While it doesn't aim to replace feature-rich libraries like 
[Michael Altmann](https://github.com/altmann)'s excellent [FluentResults](https://github.com/altmann/FluentResults) or 
[Steve Smith](https://github.com/ardalis)'s popular [Ardalis.Result](https://github.com/ardalis/result). LightResults strives 
for exceptional performance by intentionally simplifying its API.

## Comparison

Below are comparisons of LightResults against other result pattern implementations.

```
BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8655/25H2/2025Update/HudsonValley2)
13th Gen Intel Core i7-13700KF 3.40GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.109
  [Host]    : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v3

Job=.NET 10.0  Runtime=.NET 10.0  IterationTime=250ms
Iterations=10
```
These comparison results were produced using **LightResults 9.0.5** and **BenchmarkDotNet 0.15.8**.
The LightResults rows below have been refreshed with the current development benchmark values from `tools/LightResults.DevelopBenchmarks`.
The comparison implementations used the following package versions:

- **FluentResults** 4.0.0
- **Ardalis.Result** 10.1.0
### Returning results

#### Returning a successful result
| Method                            |      Mean | Ratio | Allocated |
|-----------------------------------|----------:|------:|----------:|
| LightResults: `Result.Success()`  |  2.532 ns |  1.00 |         - |
| FluentResults: `Result.Ok()`      | 46.564 ns | 18.39 |     560 B |
| ArdalisResult: `Result.Success()` | 42.761 ns | 16.89 |     720 B |

#### Returning a successful value result
| Method                                    |       Mean | Ratio | Allocated |
|-------------------------------------------|-----------:|------:|----------:|
| LightResults: `Result.Success<T>(value)`  |   2.448 ns |  1.00 |         - |
| FluentResults: `Result.Ok<T>(value)`      | 148.860 ns | 60.81 |    1120 B |
| ArdalisResult: `Result<T>.Success(value)` |  41.865 ns | 17.10 |     640 B |

#### Returning a failed result
| Method                           |       Mean |  Ratio | Allocated |
|----------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Failure()` |   2.532 ns |   1.00 |         - |
| FluentResults: `Result.Fail("")` | 282.372 ns | 111.52 |    2640 B |
| ArdalisResult: `Result.Error()`  |  47.295 ns |  18.68 |     720 B |

#### Returning a failed result with an error message
| Method                                       |       Mean | Ratio | Allocated | Alloc Ratio |
|----------------------------------------------|-----------:|------:|----------:|------------:|
| LightResults: `Result.Failure(error)`        |  22.796 ns |  1.00 |     240 B |        1.00 |
| FluentResults: `Result.Fail(errorMessage)`   | 120.913 ns |  5.30 |    1120 B |        4.67 |
| ArdalisResult: `Result.Error(errorMessage)`  |  64.953 ns |  2.85 |    1040 B |        4.33 |

#### Returning a failed value result
| Method                              |       Mean |  Ratio | Allocated |
|-------------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Failure<T>()` |   2.389 ns |   1.00 |         - |
| FluentResults: `Result.Fail<T>("")` | 285.110 ns | 119.34 |    2720 B |
| ArdalisResult: `Result<T>.Error()`  |  53.565 ns |  22.42 |     640 B |

#### Returning a failed value result with an error message
| Method                                          |       Mean | Ratio | Allocated | Alloc Ratio |
|-------------------------------------------------|-----------:|------:|----------:|------------:|
| LightResults: `Result.Failure<T>(error)`        |  23.757 ns |  1.00 |     240 B |        1.00 |
| FluentResults: `Result.Fail<T>(errorMessage)`   | 122.375 ns |  5.15 |    1200 B |        5.00 |
| ArdalisResult: `Result<T>.Error(errorMessage)`  |  61.796 ns |  2.60 |     960 B |        4.00 |

### Checking results

#### Determining if a result is successful
| Method                             |       Mean | Ratio | Allocated |
|------------------------------------|-----------:|------:|----------:|
| LightResults: `result.IsSuccess()` |   2.532 ns |  1.00 |         - |
| FluentResults: `result.IsSuccess`  | 102.670 ns | 40.55 |     480 B |
| ArdalisResult: `result.IsSuccess`  |   2.383 ns |  0.94 |         - |

#### Retrieving the value
| Method                                          |      Mean | Ratio | Allocated |
|-------------------------------------------------|----------:|------:|----------:|
| LightResults: `result.IsSuccess(out var value)` |  2.448 ns |  1.00 |         - |
| FluentResults: `result.Value`                   | 95.136 ns | 38.86 |     480 B |
| ArdalisResult: `result.Value`                   |  2.240 ns |  0.92 |         - |

#### Determining if a result is failed
| Method                             |       Mean | Ratio | Allocated |
|------------------------------------|-----------:|------:|----------:|
| LightResults: `result.IsFailure()` |   2.532 ns |  1.00 |         - |
| FluentResults: `result.IsFailed`   | 148.292 ns | 58.57 |     880 B |
| ArdalisResult: `!result.IsSuccess` |   2.334 ns |  0.92 |         - |

#### Determining if a result contains a specific error
| Method                                                                                |       Mean | Ratio | Allocated |
|---------------------------------------------------------------------------------------|-----------:|------:|----------:|
| LightResults: `result.HasError<T>()`                                                  |   2.227 ns |  1.00 |         - |
| FluentResults: `result.HasError<T>()`                                                 | 789.100 ns | 354.33 |    3840 B |
| ArdalisResult: `result.Errors.Any(errorMessage => errorMessage.Equals(ErrorMessage))` |  20.733 ns |   9.31 |         - |

#### Retrieving the first error
| Method                                          |       Mean |  Ratio | Allocated |
|-------------------------------------------------|-----------:|-------:|----------:|
| LightResults: `result.IsFailure(out var error)` |   4.178 ns |   1.00 |         - |
| FluentResults: `result.Errors[0]`               | 325.418 ns |  77.89 |    1760 B |
| ArdalisResult: `result.Errors.First()`          |  94.840 ns |  22.70 |         - |

### Getting results as strings

#### String representation of a successful result
| Method                                       |       Mean |  Ratio | Allocated |
|----------------------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Success().ToString()`  |   2.645 ns |   1.00 |         - |
| FluentResults: `Result.Ok().ToString()`      | 329.141 ns | 124.44 |    1200 B |
| ArdalisResult: `Result.Success().ToString()` |  14.724 ns |   5.57 |         - |

#### String representation of a successful value result
| Method                                               |       Mean | Ratio | Allocated | Alloc Ratio |
|------------------------------------------------------|-----------:|------:|----------:|------------:|
| LightResults: `Result.Success<T>(value).ToString()`  |  94.276 ns |  1.00 |    1040 B |        1.00 |
| FluentResults: `Result.Ok<T>(value).ToString()`      | 557.393 ns |  5.91 |    2800 B |        2.69 |
| ArdalisResult: `Result<T>.Success(value).ToString()` |  14.593 ns |  0.15 |         - |        0.00 |

#### String representation of a failed result
| Method                                      |       Mean |  Ratio | Allocated |
|---------------------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Failure().ToString()` |   4.615 ns |   1.00 |         - |
| FluentResults: `Result.Fail("").ToString()` | 957.111 ns | 207.39 |    3600 B |
| ArdalisResult: `Result.Error().ToString()`  |  15.269 ns |   3.31 |         - |

#### String representation of a failed result with an error message
| Method                                                  |         Mean | Ratio | Allocated | Alloc Ratio |
|---------------------------------------------------------|-------------:|------:|----------:|------------:|
| LightResults: `Result.Failure(error).ToString()`        |   103.255 ns |  1.00 |    1600 B |        1.00 |
| FluentResults: `Result.Fail(errorMessage).ToString()`   | 1,417.229 ns | 13.73 |    9120 B |        5.70 |
| ArdalisResult: `Result.Error(errorMessage).ToString()`  |    14.672 ns |  0.14 |         - |        0.00 |

#### String representation of a failed value result
| Method                                         |         Mean |  Ratio | Allocated |
|------------------------------------------------|-------------:|-------:|----------:|
| LightResults: `Result.Failure<T>().ToString()` |     4.718 ns |   1.00 |         - |
| FluentResults: `Result.Fail<T>("").ToString()` | 1,177.864 ns | 249.65 |    5520 B |
| ArdalisResult: `Result<T>.Error().ToString()`  |    15.082 ns |   3.20 |         - |

#### String representation of a failed value result with an error message
| Method                                                     |         Mean | Ratio | Allocated | Alloc Ratio |
|------------------------------------------------------------|-------------:|------:|----------:|------------:|
| LightResults: `Result.Failure<T>(error).ToString()`        |   106.682 ns |  1.00 |    1600 B |        1.00 |
| FluentResults: `Result<T>.Error(errorMessage).ToString()`  | 1,720.919 ns | 16.13 |   11920 B |        7.45 |
| ArdalisResult: `Result.Fail<T>(errorMessage).ToString()`   |    14.643 ns |  0.14 |         - |        0.00 |
