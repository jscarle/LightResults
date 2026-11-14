using System.Collections.Frozen;
using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;

namespace LightResults.DevelopBenchmarks;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 12)]
[IterationTime(150)]
[HideColumns(Column.Job, Column.Gen0, Column.Gen1, Column.Gen2)]
public class Benchmarks
{
    private const string ErrorMessage = "An unknown error occurred.";
    private static readonly Error EmptyError = new();
    private static readonly Error ErrorWithMessage = new(ErrorMessage);
    private static readonly Result SuccessfulResult = Result.Success();
    private static readonly Result FailedResult = Result.Failure();
    private static readonly Result FailedResultWithMessage = Result.Failure(ErrorWithMessage);
    private static readonly Result<int> SuccessfulValueResult = Result.Success(0);
    private static readonly Result<int> FailedValueResult = Result.Failure<int>();
    private static readonly Result<int> FailedValueResultWithMessage = Result.Failure<int>(ErrorWithMessage);
    private static readonly Result<int> LargeIntegerResult = Result.Success(123456789);
    private static readonly Result<double> DoubleResult = Result.Success(12345.6789);
    private static readonly Result<decimal> DecimalResult = Result.Success(12345.6789m);
    private static readonly Result<float> FloatResult = Result.Success(12345.6789f);
    private static readonly Result<Int128> Int128Result = Result.Success(Int128.MaxValue);
    private static readonly Result<DateTime> DateTimeResult = Result.Success(new DateTime(2026, 11, 13, 12, 34, 56, DateTimeKind.Utc));
    private static readonly KeyValuePair<string, object?>[] EmptyMetadata = [];
    private static readonly KeyValuePair<string, object?>[] SingleMetadata = [new("CorrelationId", "abc123")];
    private static readonly KeyValuePair<string, object?>[] MultipleMetadata = [new("CorrelationId", "abc123"), new("Operation", "Save")];
    private static readonly IError[] MultipleErrors = [new Error("First error"), new Error("Second error")];

    private static readonly List<KeyValuePair<string, object?>> SingleMetadataList = new(SingleMetadata);
    private static readonly IReadOnlyList<KeyValuePair<string, object?>> SingleMetadataReadOnlyList = Array.AsReadOnly(SingleMetadata);
    private static readonly IReadOnlyDictionary<string, object?> ImmutableMetadata = MultipleMetadata.ToImmutableDictionary(StringComparer.Ordinal);
    private static readonly IReadOnlyDictionary<string, object?> FrozenMetadata = MultipleMetadata.ToFrozenDictionary(StringComparer.Ordinal);

    // Return every result so the benchmark harness observes the operation's output.
    [Benchmark] public Result Develop_Result_Ok() => Result.Success();
    [Benchmark] public string Develop_Result_Ok_ToString() => SuccessfulResult.ToString();
    [Benchmark] public Result Develop_Result_Fail() => Result.Failure();
    [Benchmark] public string Develop_Result_Fail_ToString() => FailedResult.ToString();
    [Benchmark] public Result Develop_Result_Fail_WithErrorMessage() => Result.Failure(ErrorWithMessage);
    [Benchmark] public string Develop_Result_Fail_WithErrorMessage_ToString() => FailedResultWithMessage.ToString();
    [Benchmark] public Result<int> Develop_Result_OkTValue() => Result.Success(0);
    [Benchmark] public string Develop_Result_OkTValue_ToString() => SuccessfulValueResult.ToString();
    [Benchmark] public Result<int> Develop_Result_FailTValue() => Result.Failure<int>();
    [Benchmark] public string Develop_Result_FailTValue_ToString() => FailedValueResult.ToString();
    [Benchmark] public Result<int> Develop_Result_FailTValue_WithErrorMessage() => Result.Failure<int>(ErrorWithMessage);
    [Benchmark] public string Develop_Result_FailTValue_WithErrorMessage_ToString() => FailedValueResultWithMessage.ToString();
    [Benchmark] public bool Develop_Result_HasError() => FailedResultWithMessage.HasError<Error>();
    [Benchmark] public IError? Develop_Result_HasError_Out()
    {
        FailedResultWithMessage.HasError<Error>(out var error);
        return error;
    }
    [Benchmark] public IError? Develop_Result_Error()
    {
        FailedResultWithMessage.IsFailure(out var error);
        return error;
    }
    [Benchmark] public Error Develop_Error_New() => new();
    [Benchmark] public string Develop_Error_New_ToString() => EmptyError.ToString();
    [Benchmark] public Error Develop_Error_New_WithErrorMessage() => new(ErrorMessage);
    [Benchmark] public string Develop_Error_New_WithErrorMessage_ToString() => ErrorWithMessage.ToString();
    [Benchmark] public string Develop_Result_Int32_ToString() => LargeIntegerResult.ToString();
    [Benchmark] public string Develop_Result_Double_ToString() => DoubleResult.ToString();
    [Benchmark] public string Develop_Result_DateTime_ToString() => DateTimeResult.ToString();
    [Benchmark] public Error Develop_Error_EmptyMetadata() => new(ErrorMessage, EmptyMetadata);
    [Benchmark] public Error Develop_Error_SingleMetadata() => new(ErrorMessage, SingleMetadata);
    [Benchmark] public Error Develop_Error_MultipleMetadata() => new(ErrorMessage, MultipleMetadata);
    [Benchmark] public Result Develop_Result_MultipleErrors() => Result.Failure(MultipleErrors);
    [Benchmark] public Error Develop_Error_SingleMetadataList() => new(ErrorMessage, SingleMetadataList);
    [Benchmark] public Error Develop_Error_ImmutableMetadata() => new(ErrorMessage, ImmutableMetadata);
    [Benchmark] public Error Develop_Error_FrozenMetadata() => new(ErrorMessage, FrozenMetadata);
    [Benchmark] public Result Develop_Result_InlineErrors() => Result.Failure([MultipleErrors[0], MultipleErrors[1]]);
    [Benchmark] public Result<int> Develop_Result_InlineErrorsTValue() => Result.Failure<int>([MultipleErrors[0], MultipleErrors[1]]);
    [Benchmark] public Error Develop_Error_InlineMetadata() => new(ErrorMessage, [SingleMetadata[0], MultipleMetadata[1]]);
    [Benchmark] public string Develop_Result_Decimal_ToString() => DecimalResult.ToString();
    [Benchmark] public string Develop_Result_Float_ToString() => FloatResult.ToString();
    [Benchmark] public string Develop_Result_Int128_ToString() => Int128Result.ToString();
    [Benchmark] public Error Develop_Error_SingleMetadataReadOnlyList() => new(ErrorMessage, SingleMetadataReadOnlyList);
}
