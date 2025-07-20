# Performance

This library prioritizes performance as a core design principle. While it doesn't aim to replace feature-rich libraries like 
[Michael Altmann](https://github.com/altmann)'s excellent [FluentResults](https://github.com/altmann/FluentResults) or 
[Steve Smith](https://github.com/ardalis)'s popular [Ardalis.Result](https://github.com/ardalis/result). LightResults strives 
for exceptional performance by intentionally simplifying its API.

## Comparison

Below are comparisons of LightResults against other result pattern implementations.

```
BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4652/24H2/2024Update/HudsonValley)
13th Gen Intel Core i7-13700KF 3.40GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.303
  [Host]   : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  .NET 9.0 : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2

Job=.NET 9.0  Runtime=.NET 9.0  IterationTime=250ms
Iterations=10
```
These results were produced using **LightResults 9.0.5** and **BenchmarkDotNet 0.15.2**.
The comparison implementations used the following package versions:

- **FluentResults** 4.0.0
- **Ardalis.Result** 10.1.0
### Returning results

#### Returning a successful result
| Method                            |      Mean | Ratio | Allocated |
|-----------------------------------|----------:|------:|----------:|
| LightResults: `Result.Success()`  |  2.382 ns |  1.00 |         - |
| FluentResults: `Result.Ok()`      | 46.564 ns | 19.55 |     560 B |
| ArdalisResult: `Result.Success()` | 42.761 ns | 17.96 |     720 B |

#### Returning a successful value result
| Method                                    |       Mean | Ratio | Allocated |
|-------------------------------------------|-----------:|------:|----------:|
| LightResults: `Result.Success<T>(value)`  |   2.308 ns |  1.00 |         - |
| FluentResults: `Result.Ok<T>(value)`      | 148.860 ns | 64.50 |    1120 B |
| ArdalisResult: `Result<T>.Success(value)` |  41.865 ns | 18.14 |     640 B |

#### Returning a failed result
| Method                           |       Mean |  Ratio | Allocated |
|----------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Failure()` |   2.099 ns |   1.00 |         - |
| FluentResults: `Result.Fail("")` | 282.372 ns | 134.55 |    2640 B |
| ArdalisResult: `Result.Error()`  |  47.295 ns |  22.54 |     720 B |

#### Returning a failed result with an error message
| Method                                       |       Mean | Ratio | Allocated | Alloc Ratio |
|----------------------------------------------|-----------:|------:|----------:|------------:|
| LightResults: `Result.Failure(errorMessage)` |  21.614 ns |  1.00 |     240 B |        1.00 |
| FluentResults: `Result.Fail(errorMessage)`   | 120.913 ns |  5.60 |    1120 B |        4.67 |
| ArdalisResult: `Result.Error(errorMessage)`  |  64.953 ns |  3.01 |    1040 B |        4.33 |

#### Returning a failed value result
| Method                              |       Mean |  Ratio | Allocated |
|-------------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Failure<T>()` |   2.500 ns |   1.00 |         - |
| FluentResults: `Result.Fail<T>("")` | 285.110 ns | 114.06 |    2720 B |
| ArdalisResult: `Result<T>.Error()`  |  53.565 ns |  21.43 |     640 B |

#### Returning a failed value result with an error message
| Method                                          |       Mean | Ratio | Allocated | Alloc Ratio |
|-------------------------------------------------|-----------:|------:|----------:|------------:|
| LightResults: `Result.Failure<T>(errorMessage)` |  18.487 ns |  1.00 |     240 B |        1.00 |
| FluentResults: `Result.Fail<T>(errorMessage)`   | 122.375 ns |  6.62 |    1200 B |        5.00 |
| ArdalisResult: `Result<T>.Error(errorMessage)`  |  61.796 ns |  3.34 |     960 B |        4.00 |

### Checking results

#### Determining if a result is successful
| Method                             |       Mean | Ratio | Allocated |
|------------------------------------|-----------:|------:|----------:|
| LightResults: `result.IsSuccess()` |   2.219 ns |  1.00 |         - |
| FluentResults: `result.IsSuccess`  | 102.670 ns | 46.29 |     480 B |
| ArdalisResult: `result.IsSuccess`  |   2.383 ns |  1.07 |         - |

#### Retrieving the value
| Method                                          |      Mean | Ratio | Allocated |
|-------------------------------------------------|----------:|------:|----------:|
| LightResults: `result.IsSuccess(out var value)` |  2.238 ns |  1.00 |         - |
| FluentResults: `result.Value`                   | 95.136 ns | 42.52 |     480 B |
| ArdalisResult: `result.Value`                   |  2.240 ns |  1.00 |         - |

#### Determining if a result is failed
| Method                             |       Mean | Ratio | Allocated |
|------------------------------------|-----------:|------:|----------:|
| LightResults: `result.IsFailure()` |   2.196 ns |  1.00 |         - |
| FluentResults: `result.IsFailed`   | 148.292 ns | 67.55 |     880 B |
| ArdalisResult: `!result.IsSuccess` |   2.334 ns |  1.06 |         - |

#### Determining if a result contains a specific error
| Method                                                                                |       Mean | Ratio | Allocated |
|---------------------------------------------------------------------------------------|-----------:|------:|----------:|
| LightResults: `result.HasError<T>()`                                                  |  12.887 ns |  1.00 |         - |
| FluentResults: `result.HasError<T>()`                                                 | 789.100 ns | 61.23 |    3840 B |
| ArdalisResult: `result.Errors.Any(errorMessage => errorMessage.Equals(ErrorMessage))` |  20.733 ns |  1.61 |         - |

#### Retrieving the first error
| Method                                 |       Mean |  Ratio | Allocated |
|----------------------------------------|-----------:|-------:|----------:|
| LightResults: `result.Error`           |   3.817 ns |   1.00 |         - |
| FluentResults: `result.Errors[0]`      | 325.418 ns |  85.25 |    1760 B |
| ArdalisResult: `result.Errors.First()` |  94.840 ns |  24.85 |         - |

### Getting results as strings

#### String representation of a successful result
| Method                                       |       Mean |  Ratio | Allocated |
|----------------------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Success().ToString()`  |   2.595 ns |   1.00 |         - |
| FluentResults: `Result.Ok().ToString()`      | 329.141 ns | 126.82 |    1200 B |
| ArdalisResult: `Result.Success().ToString()` |  14.724 ns |   5.67 |         - |

#### String representation of a successful value result
| Method                                               |       Mean | Ratio | Allocated | Alloc Ratio |
|------------------------------------------------------|-----------:|------:|----------:|------------:|
| LightResults: `Result.Success<T>(value).ToString()`  |  98.429 ns |  1.00 |    1040 B |        1.00 |
| FluentResults: `Result.Ok<T>(value).ToString()`      | 557.393 ns |  5.66 |    2800 B |        2.69 |
| ArdalisResult: `Result<T>.Success(value).ToString()` |  14.593 ns |  0.15 |         - |        0.00 |

#### String representation of a failed result
| Method                                      |       Mean |  Ratio | Allocated |
|---------------------------------------------|-----------:|-------:|----------:|
| LightResults: `Result.Failure().ToString()` |   5.324 ns |   1.00 |         - |
| FluentResults: `Result.Fail("").ToString()` | 957.111 ns | 179.76 |    3600 B |
| ArdalisResult: `Result.Error().ToString()`  |  15.269 ns |   2.87 |         - |

#### String representation of a failed result with an error message
| Method                                                  |         Mean | Ratio | Allocated | Alloc Ratio |
|---------------------------------------------------------|-------------:|------:|----------:|------------:|
| LightResults: `Result.Failure(errorMessage).ToString()` |   108.405 ns |  1.00 |    1600 B |        1.00 |
| FluentResults: `Result.Fail(errorMessage).ToString()`   | 1,417.229 ns | 13.08 |    9120 B |        5.70 |
| ArdalisResult: `Result.Error(errorMessage).ToString()`  |    14.672 ns |  0.14 |         - |        0.00 |

#### String representation of a failed value result
| Method                                         |         Mean |  Ratio | Allocated |
|------------------------------------------------|-------------:|-------:|----------:|
| LightResults: `Result.Failure<T>().ToString()` |     5.331 ns |   1.00 |         - |
| FluentResults: `Result.Fail<T>("").ToString()` | 1,177.864 ns | 220.96 |    5520 B |
| ArdalisResult: `Result<T>.Error().ToString()`  |    15.082 ns |   2.83 |         - |

#### String representation of a failed value result with an error message
| Method                                                     |         Mean | Ratio | Allocated | Alloc Ratio |
|------------------------------------------------------------|-------------:|------:|----------:|------------:|
| LightResults: `Result.Failure<T>(errorMessage).ToString()` |   105.022 ns |  1.00 |    1600 B |        1.00 |
| FluentResults: `Result<T>.Error(errorMessage).ToString()`  | 1,720.919 ns | 16.39 |   11920 B |        7.45 |
| ArdalisResult: `Result.Fail<T>(errorMessage).ToString()`   |    14.643 ns |  0.14 |         - |        0.00 |
