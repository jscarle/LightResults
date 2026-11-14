# LightResults benchmarks

`LightResults.CurrentBenchmarks` measures the published LightResults NuGet package. `LightResults.DevelopBenchmarks` measures the local library project. `SpanFactoryBenchmarks` compares collection and span overloads using identical inputs in the same build.

## Running benchmarks

Run benchmark processes sequentially in Release mode from the repository root:

```powershell
dotnet run --project tools/LightResults.CurrentBenchmarks --configuration Release -- --filter '*' --artifacts ./tools/LightResults.CurrentBenchmarks/bin/results
dotnet run --project tools/LightResults.DevelopBenchmarks --configuration Release -- --filter '*Benchmarks.Develop_*' --artifacts ./tools/LightResults.DevelopBenchmarks/bin/results
dotnet run --project tools/LightResults.DevelopBenchmarks --configuration Release -- --filter '*SpanFactoryBenchmarks*' --artifacts ./tools/LightResults.DevelopBenchmarks/bin/span-results
```

Use a method-name filter such as `'*Metadata*'` for a focused run. Each benchmark returns its output to prevent dead-code elimination. Inputs are prepared outside the measured operation except in methods named `Inline`, which include collection-expression construction. The paired collection-overload cases explicitly select collection interfaces, while their span counterparts use collection expressions.

## Measurement environment

Measurements collected November 13, 2026:

- .NET SDK 11.0.100 and .NET runtime 11.0.0, x64 RyuJIT.
- Windows 11, Intel Core i7-13700KF.
- BenchmarkDotNet 0.16.0-preview.2, which supports .NET 11.
- Three warmup iterations and twelve measured iterations of 150 ms, with memory diagnostics.
- Package baseline: LightResults 11.0.0.

Results are specific to the listed inputs, runtime, and hardware. Small timing differences can reflect generated code layout and benchmark overhead.

## Published package and local source

The table covers the 36 core operations. Error and standard-deviation columns describe the local source measurement. Allocations are managed bytes per operation.

| Operation | Package mean | Source mean | Source error | Source StdDev | Allocation: package / source |
|---|---:|---:|---:|---:|---:|
| `Result_Ok` | 0.7799 ns | 0.7782 ns | 0.0116 ns | 0.0090 ns | 0 B / 0 B |
| `Result_Ok_ToString` | 0.7785 ns | 0.7836 ns | 0.0078 ns | 0.0057 ns | 0 B / 0 B |
| `Result_Fail` | 0.7561 ns | 0.7838 ns | 0.0156 ns | 0.0122 ns | 0 B / 0 B |
| `Result_Fail_ToString` | 1.1827 ns | 1.1895 ns | 0.0044 ns | 0.0032 ns | 0 B / 0 B |
| `Result_Fail_WithErrorMessage` | 3.2928 ns | 3.3734 ns | 0.0317 ns | 0.0229 ns | 24 B / 24 B |
| `Result_Fail_WithErrorMessage_ToString` | 10.0020 ns | 7.7686 ns | 0.0621 ns | 0.0484 ns | 160 B / 160 B |
| `Result_OkTValue` | 0.7666 ns | 0.7820 ns | 0.0078 ns | 0.0061 ns | 0 B / 0 B |
| `Result_OkTValue_ToString` | 8.6619 ns | 9.1570 ns | 0.0718 ns | 0.0519 ns | 104 B / 104 B |
| `Result_FailTValue` | 0.7736 ns | 0.7694 ns | 0.0078 ns | 0.0061 ns | 0 B / 0 B |
| `Result_FailTValue_ToString` | 1.1914 ns | 1.1801 ns | 0.0069 ns | 0.0050 ns | 0 B / 0 B |
| `Result_FailTValue_WithErrorMessage` | 3.2689 ns | 3.3010 ns | 0.0512 ns | 0.0400 ns | 24 B / 24 B |
| `Result_FailTValue_WithErrorMessage_ToString` | 9.8118 ns | 7.8392 ns | 0.0679 ns | 0.0530 ns | 160 B / 160 B |
| `Result_HasError` | 0.7656 ns | 0.7731 ns | 0.0066 ns | 0.0052 ns | 0 B / 0 B |
| `Result_HasError_Out` | 1.2819 ns | 1.2776 ns | 0.0035 ns | 0.0023 ns | 0 B / 0 B |
| `Result_Error` | 1.1782 ns | 1.1956 ns | 0.0090 ns | 0.0070 ns | 0 B / 0 B |
| `Error_New` | 2.9739 ns | 2.9793 ns | 0.0344 ns | 0.0269 ns | 32 B / 32 B |
| `Error_New_ToString` | 1.1824 ns | 1.1959 ns | 0.0049 ns | 0.0039 ns | 0 B / 0 B |
| `Error_New_WithErrorMessage` | 2.9078 ns | 2.8893 ns | 0.0259 ns | 0.0202 ns | 32 B / 32 B |
| `Error_New_WithErrorMessage_ToString` | 10.0348 ns | 10.2804 ns | 0.0789 ns | 0.0616 ns | 120 B / 120 B |
| `Result_Int32_ToString` | 13.8124 ns | 14.5626 ns | 0.2534 ns | 0.1979 ns | 160 B / 160 B |
| `Result_Double_ToString` | 63.5358 ns | 64.3641 ns | 1.0300 ns | 0.8042 ns | 168 B / 120 B |
| `Result_DateTime_ToString` | 58.4526 ns | 54.9844 ns | 0.6472 ns | 0.5053 ns | 208 B / 144 B |
| `Error_EmptyMetadata` | 7.7938 ns | 3.2814 ns | 0.0429 ns | 0.0310 ns | 112 B / 32 B |
| `Error_SingleMetadata` | 34.1250 ns | 7.0444 ns | 0.0669 ns | 0.0484 ns | 280 B / 64 B |
| `Error_MultipleMetadata` | 33.7882 ns | 30.3410 ns | 0.4685 ns | 0.3658 ns | 288 B / 288 B |
| `Result_MultipleErrors` | 8.6386 ns | 7.4577 ns | 0.0455 ns | 0.0329 ns | 64 B / 64 B |
| `Error_SingleMetadataList` | 33.9262 ns | 10.3935 ns | 0.1084 ns | 0.0784 ns | 280 B / 64 B |
| `Error_ImmutableMetadata` | 116.5997 ns | 3.3184 ns | 0.0277 ns | 0.0216 ns | 440 B / 32 B |
| `Error_FrozenMetadata` | 38.3665 ns | 3.1791 ns | 0.0380 ns | 0.0275 ns | 328 B / 32 B |
| `Result_InlineErrors` | 17.7307 ns | 8.1273 ns | 0.1612 ns | 0.1165 ns | 128 B / 64 B |
| `Result_InlineErrorsTValue` | 18.2726 ns | 8.1576 ns | 0.1406 ns | 0.1098 ns | 128 B / 64 B |
| `Error_InlineMetadata` | 39.5466 ns | 28.4611 ns | 0.3943 ns | 0.3078 ns | 368 B / 288 B |
| `Result_Decimal_ToString` | 31.64 ns | 30.2645 ns | 0.2624 ns | 0.2048 ns | 168 B / 120 B |
| `Result_Float_ToString` | 61.17 ns | 59.2708 ns | 0.8717 ns | 0.6806 ns | 160 B / 120 B |
| `Result_Int128_ToString` | 32.93 ns | 31.7412 ns | 0.2493 ns | 0.1803 ns | 280 B / 176 B |
| `Error_SingleMetadataReadOnlyList` | 39.10 ns | 6.3601 ns | 0.0409 ns | 0.0296 ns | 312 B / 64 B |

## Collection and span factories

The paired benchmarks compare the overloads in the local library. They include temporary input collection construction and preserve identical retained errors or metadata entries.

| Operation | Collection overload | Span overload | Allocation: collection / span |
|---|---:|---:|---:|
| Error metadata constructor | 33.414 ns | 26.637 ns | 368 B / 288 B |
| Result error collection | 19.206 ns | 8.717 ns | 128 B / 64 B |
| Value result error collection | 18.907 ns | 8.788 ns | 128 B / 64 B |
| Result metadata factory | 38.900 ns | 30.296 ns | 392 B / 312 B |
| Value result metadata factory | 40.873 ns | 29.932 ns | 392 B / 312 B |
