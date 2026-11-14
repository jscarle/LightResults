# Performance

LightResults uses readonly result structs, shared empty values, and immutable snapshots of caller-owned collections. Successful results and parameterless failures have no per-operation managed allocation after initialization. Errors with messages, retained collections, and formatted strings can allocate.

## .NET 11 measurements

These measurements compare the published LightResults 11.0.0 package with the local library source. They were collected on November 13, 2026 using .NET SDK 11.0.100, .NET runtime 11.0.0, and BenchmarkDotNet 0.16.0-preview.2 on Windows 11 with an Intel Core i7-13700KF.

Each benchmark returns one observed operation result. Jobs use three warmup iterations and twelve measured iterations of 150 ms. Inputs are prepared outside the measured operation except in benchmarks named `Inline`, which include input collection construction.

| Operation | Published 11.0.0 | Local source | Allocation: package / source |
|---|---:|---:|---:|
| Empty metadata array | 7.7938 ns | 3.2814 ns | 112 B / 32 B |
| One metadata entry in an array | 34.1250 ns | 7.0444 ns | 280 B / 64 B |
| One metadata entry in a list | 33.9262 ns | 10.3935 ns | 280 B / 64 B |
| One metadata entry in a read-only list | 39.10 ns | 6.3601 ns | 312 B / 64 B |
| Two entries in an immutable dictionary | 116.5997 ns | 3.3184 ns | 440 B / 32 B |
| Two entries in a frozen dictionary | 38.3665 ns | 3.1791 ns | 328 B / 32 B |
| Format a failure result with a message | 10.0020 ns | 7.7686 ns | 160 B / 160 B |
| Format a successful DateTime result | 58.4526 ns | 54.9844 ns | 208 B / 144 B |
| Format a successful double result | 63.5358 ns | 64.3641 ns | 168 B / 120 B |
| Format a successful decimal result | 31.64 ns | 30.2645 ns | 168 B / 120 B |
| Format a successful Int128 result | 32.93 ns | 31.7412 ns | 280 B / 176 B |
| Two errors in a collection expression | 17.7307 ns | 8.1273 ns | 128 B / 64 B |

Metadata collections retain ordinal key semantics and snapshot ownership. Immutable and frozen dictionaries are reused only when their comparers preserve ordinal equality. Referenced metadata values are not cloned.

Formatting uses invariant culture. Floating-point, decimal, Int128, and fixed date/time formats avoid an intermediate value string.

## Span factories

Span overloads avoid temporary input collections while retaining owned snapshots. The following paired benchmarks compare collection and span call paths in the same build using identical error or metadata entries:

| Operation | Collection overload | Span overload | Allocation: collection / span |
|---|---:|---:|---:|
| Error metadata constructor | 33.414 ns | 26.637 ns | 368 B / 288 B |
| Result error collection | 19.206 ns | 8.717 ns | 128 B / 64 B |
| Value result error collection | 18.907 ns | 8.788 ns | 128 B / 64 B |
| Result metadata factory | 38.900 ns | 30.296 ns | 392 B / 312 B |
| Value result metadata factory | 40.873 ns | 29.932 ns | 392 B / 312 B |

## Reproduce

Run benchmark processes sequentially in Release mode from the repository root:

```powershell
dotnet run --project tools/LightResults.CurrentBenchmarks --configuration Release -- --filter '*' --artifacts ./tools/LightResults.CurrentBenchmarks/bin/results
dotnet run --project tools/LightResults.DevelopBenchmarks --configuration Release -- --filter '*Benchmarks.Develop_*' --artifacts ./tools/LightResults.DevelopBenchmarks/bin/results
dotnet run --project tools/LightResults.DevelopBenchmarks --configuration Release -- --filter '*SpanFactoryBenchmarks*' --artifacts ./tools/LightResults.DevelopBenchmarks/bin/span-results
```

The [benchmark project documentation](https://github.com/jscarle/LightResults/tree/develop/tools/LightResults.DevelopBenchmarks) contains all measured core operations and their uncertainty. Results depend on the inputs, runtime, and hardware; differences near benchmark overhead can reflect generated code layout.
