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
| LightResults: `Result.Ok()`       |  18.80 μs |  1.00 |         - |             |
| FluentResults: `Result.Ok()`      | 981.87 μs | 52.22 |   5.34 MB |             |
| ArdalisResult: `Result.Success()` | 893.70 μs | 47.53 |  12.21 MB |             |

### String representation of a successful result
| Method                                       |        Mean |  Ratio | Allocated | Alloc Ratio |
|----------------------------------------------|------------:|-------:|----------:|------------:|
| LightResults: `Result.Ok().ToString()`       |    26.79 μs |   1.00 |         - |             |
| FluentResults: `Result.Ok().ToString()`      | 6,616.35 μs | 246.97 |  14.50 MB |             |
| ArdalisResult: `Result.Success().ToString()` |   187.72 μs |   7.01 |         - |             |

### Returning a successful result with a value
| Method                                    |        Mean | Ratio | Allocated | Alloc Ratio |
|-------------------------------------------|------------:|------:|----------:|------------:|
| LightResults: `Result<T>.Ok(value)`       |   192.40 μs |  1.00 |   3.05 MB |        1.00 |
| FluentResults: `Result.Ok<T>(value)`      | 3,426.51 μs | 17.81 |  11.44 MB |        3.75 |
| ArdalisResult: `Result<T>.Success(value)` |   878.27 μs |  4.57 |  11.44 MB |        3.75 |

### String representation of a successful result with a value
| Method                                               |        Mean | Ratio | Allocated | Alloc Ratio |
|------------------------------------------------------|------------:|------:|----------:|------------:|
| LightResults: `Result<T>.Ok(value).ToString()`       | 2,484.68 μs |  1.00 |  14.50 MB |        1.00 |
| FluentResults: `Result.Ok<T>(value).ToString()`      | 9,427.94 μs |  3.79 |  29.75 MB |        2.05 |
| ArdalisResult: `Result<T>.Success(value).ToString()` |   188.17 μs |  0.08 |         - |        0.00 |

### Returning a failed result
| Method                           |        Mean |  Ratio | Allocated | Alloc Ratio |
|----------------------------------|------------:|-------:|----------:|------------:|
| LightResults: `Result.Fail()`    |    18.87 μs |   1.00 |         - |             |
| FluentResults: `Result.Fail("")` | 4,691.18 μs | 248.60 |  25.18 MB |             |
| ArdalisResult: `Result.Error()`  |   729.63 μs |  38.66 |  12.21 MB |             |

### String representation of a failed result
| Method                                      |         Mean |  Ratio | Allocated | Alloc Ratio |
|---------------------------------------------|-------------:|-------:|----------:|------------:|
| LightResults: `Result.Fail().ToString()`    |     74.19 μs |   1.00 |         - |             |
| FluentResults: `Result.Fail("").ToString()` | 16,309.96 μs | 219.88 |  37.38 MB |             |
| ArdalisResult: `Result.Error().ToString()`  |    188.69 μs |   2.54 |         - |             |

### Returning a failed result with an error message
| Method                                      |        Mean | Ratio | Allocated | Alloc Ratio |
|---------------------------------------------|------------:|------:|----------:|------------:|
| LightResults: `Result.Fail(errorMessage)`   |   541.52 μs |  1.00 |   7.63 MB |        1.00 |
| FluentResults: `Result.Fail(errorMessage)`  | 2,195.71 μs |  4.05 |  10.68 MB |        1.40 |
| ArdalisResult: `Result.Error(errorMessage)` | 1,139.62 μs |  2.11 |  15.26 MB |        2.00 |

### String representation of a failed result with an error message
| Method                                                 |         Mean | Ratio | Allocated | Alloc Ratio |
|--------------------------------------------------------|-------------:|------:|----------:|------------:|
| LightResults: `Result.Fail(errorMessage).ToString()`   |  2,788.78 μs |  1.00 |  23.65 MB |        1.00 |
| FluentResults: `Result.Fail(errorMessage).ToString()`  | 21,893.27 μs |  7.85 |  89.27 MB |        3.77 |
| ArdalisResult: `Result.Error(errorMessage).ToString()` |    188.20 μs |  0.07 |         - |        0.00 |

### Returning a failed result of a value type
| Method                              |        Mean |  Ratio | Allocated | Alloc Ratio |
|-------------------------------------|------------:|-------:|----------:|------------:|
| LightResults: `Result<T>.Fail()`    |    18.86 μs |   1.00 |         - |             |
| FluentResults: `Result.Fail<T>("")` | 4,759.98 μs | 252.39 |  25.94 MB |             |
| ArdalisResult: `Result<T>.Error()`  |   863.81 μs |  45.80 |  11.44 MB |             |

### String representation of a failed result of a value type
| Method                                         |         Mean |  Ratio | Allocated | Alloc Ratio |
|------------------------------------------------|-------------:|-------:|----------:|------------:|
| LightResults: `Result<T>.Fail().ToString()`    |     77.61 μs |   1.00 |         - |             |
| FluentResults: `Result.Fail<T>("").ToString()` | 19,972.67 μs | 257.31 |  55.70 MB |             |
| ArdalisResult: `Result<T>.Error().ToString()`  |    188.23 μs |   2.43 |         - |             |

### Returning a failed result of a value type with an error message
| Method                                         |        Mean | Ratio | Allocated | Alloc Ratio |
|------------------------------------------------|------------:|------:|----------:|------------:|
| LightResults: `Result<T>.Fail(errorMessage)`   |   563.00 μs |  1.00 |   8.39 MB |        1.00 |
| FluentResults: `Result.Fail<T>(errorMessage)`  | 2,213.45 μs |  3.93 |  11.44 MB |        1.36 |
| ArdalisResult: `Result<T>.Error(errorMessage)` | 1,070.03 μs |  1.90 |  14.50 MB |        1.73 |

### String representation of a failed result of a value type with an error message
| Method                                                    |         Mean | Ratio | Allocated | Alloc Ratio |
|-----------------------------------------------------------|-------------:|------:|----------:|------------:|
| LightResults: `Result<T>.Fail(errorMessage).ToString()`   |  2,751.83 μs |  1.00 |  23.65 MB |        1.00 |
| FluentResults: `Result<T>.Error(errorMessage).ToString()` | 26,479.94 μs |  9.62 | 115.97 MB |        4.90 |
| ArdalisResult: `Result.Fail<T>(errorMessage).ToString()`  |    190.36 μs |  0.07 |         - |        0.00 |

### Determining if a result contains a specific error
| Method                                                                                |         Mean |  Ratio | Allocated | Alloc Ratio |
|---------------------------------------------------------------------------------------|-------------:|-------:|----------:|------------:|
| LightResults: `result.HasError<T>()`                                                  |     94.17 μs |   1.00 |         - |             |
| FluentResults: `result.HasError<T>()`                                                 | 12,515.35 μs | 132.91 |  40.44 MB |             |
| ArdalisResult: `result.Errors.Any(errorMessage => errorMessage.Equals(ErrorMessage))` |    706.06 μs |   7.50 |   3.05 MB |             |

### Retrieving the first error
| Method                                 |         Mean |  Ratio | Allocated | Alloc Ratio |
|----------------------------------------|-------------:|-------:|----------:|------------:|
| LightResults: `result.Errors[0]`       |     27.56 μs |   1.00 |         - |             |
| FluentResults: `result.Errors[0]`      |  4,776.30 μs | 173.31 |  17.55 MB |             |
| ArdalisResult: `result.Errors.First()` |  1,157.81 μs |  42.01 |         - |             |
