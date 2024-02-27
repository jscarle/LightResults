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

### Returning a successful result
| Method                            |      Mean | Ratio | Allocated | Alloc Ratio |
|-----------------------------------|----------:|------:|----------:|------------:|
| LightResults: `Result.Ok()`       |  18.75 μs |  1.00 |         - |             |
| FluentResults: `Result.Ok()`      | 970.51 μs | 51.75 |   5.34 MB |             |
| ArdalisResult: `Result.Success()` | 884.94 μs | 47.20 |  12.21 MB |             |

### String representation of a successful result
| Method                                       |       Mean |  Ratio | Allocated | Alloc Ratio |
|----------------------------------------------|-----------:|-------:|----------:|------------:|
| LightResults: `Result.Ok().ToString()`       |   19.25 μs |   1.00 |         - |             |
| FluentResults: `Result.Ok().ToString()`      | 6596.66 μs | 342.67 |  14.50 MB |             |
| ArdalisResult: `Result.Success().ToString()` |  176.13 μs |   9.15 |         - |             |

### Returning a successful value result
| Method                                    |        Mean | Ratio | Allocated | Alloc Ratio |
|-------------------------------------------|------------:|------:|----------:|------------:|
| LightResults: `Result<T>.Ok(value)`       |   301.55 μs |  1.00 |   3.05 MB |        1.00 |
| FluentResults: `Result.Ok<T>(value)`      | 3,319.44 μs | 11.01 |  11.44 MB |        3.75 |
| ArdalisResult: `Result<T>.Success(value)` |   869.52 μs |  2.88 |  11.44 MB |        3.75 |

### String representation of a successful value result
| Method                                               |        Mean | Ratio | Allocated | Alloc Ratio |
|------------------------------------------------------|------------:|------:|----------:|------------:|
| LightResults: `Result<T>.Ok(value).ToString()`       | 2,264.87 μs |  1.00 |  14.50 MB |        1.00 |
| FluentResults: `Result.Ok<T>(value).ToString()`      | 9,240.23 μs |  4.08 |  29.75 MB |        2.05 |
| ArdalisResult: `Result<T>.Success(value).ToString()` |   180.07 μs |  0.08 |         - |        0.00 |

### Returning a failed result
| Method                           |        Mean |  Ratio | Allocated | Alloc Ratio |
|----------------------------------|------------:|-------:|----------:|------------:|
| LightResults: `Result.Fail()`    |    18.73 μs |   1.00 |         - |             |
| FluentResults: `Result.Fail("")` | 4,605.57 μs | 245.94 |  25.18 MB |             |
| ArdalisResult: `Result.Error()`  |   721.64 μs |  38.53 |  12.21 MB |             |

### String representation of a failed result
| Method                                      |         Mean |  Ratio | Allocated | Alloc Ratio |
|---------------------------------------------|-------------:|-------:|----------:|------------:|
| LightResults: `Result.Fail().ToString()`    |     59.70 μs |   1.00 |         - |             |
| FluentResults: `Result.Fail("").ToString()` | 16,118.10 μs | 270.01 |  37.38 MB |             |
| ArdalisResult: `Result.Error().ToString()`  |    180.96 μs |   3.03 |         - |             |

### Returning a failed result with an error message
| Method                                      |        Mean | Ratio | Allocated | Alloc Ratio |
|---------------------------------------------|------------:|------:|----------:|------------:|
| LightResults: `Result.Fail(errorMessage)`   |   358.16 μs |  1.00 |   5.34 MB |        1.00 |
| FluentResults: `Result.Fail(errorMessage)`  | 2,109.22 μs |  5.89 |  10.68 MB |        2.00 |
| ArdalisResult: `Result.Error(errorMessage)` | 1,116.45 μs |  3.12 |  15.26 MB |        2.86 |

### String representation of a failed result with an error message
| Method                                                 |         Mean | Ratio | Allocated | Alloc Ratio |
|--------------------------------------------------------|-------------:|------:|----------:|------------:|
| LightResults: `Result.Fail(errorMessage).ToString()`   |  2,636.35 μs |  1.00 |  23.65 MB |        1.00 |
| FluentResults: `Result.Fail(errorMessage).ToString()`  | 21,792.47 μs |  8.27 |  89.27 MB |        3.77 |
| ArdalisResult: `Result.Error(errorMessage).ToString()` |    176.21 μs |  0.07 |         - |        0.00 |

### Returning a failed value result
| Method                              |        Mean |  Ratio | Allocated | Alloc Ratio |
|-------------------------------------|------------:|-------:|----------:|------------:|
| LightResults: `Result<T>.Fail()`    |    18.72 μs |   1.00 |         - |             |
| FluentResults: `Result.Fail<T>("")` | 4,800.67 μs | 256.39 |  25.94 MB |             |
| ArdalisResult: `Result<T>.Error()`  |   857.74 μs |  45.81 |  11.44 MB |             |

### String representation of a failed value result
| Method                                         |         Mean |  Ratio | Allocated | Alloc Ratio |
|------------------------------------------------|-------------:|-------:|----------:|------------:|
| LightResults: `Result<T>.Fail().ToString()`    |     59.57 μs |   1.00 |         - |             |
| FluentResults: `Result.Fail<T>("").ToString()` | 19,730.34 μs | 331.37 |  55.70 MB |             |
| ArdalisResult: `Result<T>.Error().ToString()`  |    176.75 μs |   2.97 |         - |             |

### Returning a failed value result with an error message
| Method                                         |        Mean | Ratio | Allocated | Alloc Ratio |
|------------------------------------------------|------------:|------:|----------:|------------:|
| LightResults: `Result<T>.Fail(errorMessage)`   |   373.73 μs |  1.00 |   6.11 MB |        1.00 |
| FluentResults: `Result.Fail<T>(errorMessage)`  | 2,116.39 μs |  5.66 |  11.44 MB |        1.88 |
| ArdalisResult: `Result<T>.Error(errorMessage)` | 1,048.89 μs |  2.81 |  14.50 MB |        2.38 |

### String representation of a failed value result with an error message
| Method                                                    |         Mean | Ratio | Allocated | Alloc Ratio |
|-----------------------------------------------------------|-------------:|------:|----------:|------------:|
| LightResults: `Result<T>.Fail(errorMessage).ToString()`   |  2,867.35 μs |  1.00 |  23.65 MB |        1.00 |
| FluentResults: `Result<T>.Error(errorMessage).ToString()` | 26,214.19 μs |  9.14 | 115.97 MB |        4.90 |
| ArdalisResult: `Result.Fail<T>(errorMessage).ToString()`  |    179.75 μs |  0.06 |         - |        0.00 |

### Determining if a result contains a specific error
| Method                                                                                |         Mean |  Ratio | Allocated | Alloc Ratio |
|---------------------------------------------------------------------------------------|-------------:|-------:|----------:|------------:|
| LightResults: `result.HasError<T>()`                                                  |     93.69 μs |   1.00 |         - |             |
| FluentResults: `result.HasError<T>()`                                                 | 12,697.37 μs | 135.53 |  40.44 MB |             |
| ArdalisResult: `result.Errors.Any(errorMessage => errorMessage.Equals(ErrorMessage))` |    761.70 μs |   8.13 |   3.05 MB |             |

### Retrieving the first error
| Method                                 |        Mean |  Ratio | Allocated | Alloc Ratio |
|----------------------------------------|------------:|-------:|----------:|------------:|
| LightResults: `result.Error`           |    28.10 μs |   1.00 |         - |             |
| FluentResults: `result.Errors[0]`      | 4,429.89 μs | 157.65 |  17.55 MB |             |
| ArdalisResult: `result.Errors.First()` | 1,267.42 μs |  45.11 |         - |             |
