using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using LightResults.Common;
using Shouldly;

namespace LightResults.Tests;

public sealed class ActionableResultEdgeTests
{
    public readonly struct CustomResult : IActionableResult<CustomResult>
    {
        private readonly Result _inner;

        private CustomResult(Result inner)
        {
            _inner = inner;
        }

        public static CustomResult Success()
        {
            return new CustomResult(Result.Success());
        }

        public static CustomResult Failure()
        {
            return new CustomResult(Result.Failure());
        }

        public static CustomResult Failure(string errorMessage)
        {
            return new CustomResult(Result.Failure(errorMessage));
        }

        public static CustomResult Failure(string errorMessage, (string Key, object? Value) metadata)
        {
            return new CustomResult(Result.Failure(errorMessage, metadata));
        }

        public static CustomResult Failure(string errorMessage, KeyValuePair<string, object?> metadata)
        {
            return new CustomResult(Result.Failure(errorMessage, metadata));
        }

        public static CustomResult Failure(string errorMessage, IReadOnlyDictionary<string, object?> metadata)
        {
            return new CustomResult(Result.Failure(errorMessage, metadata));
        }

        public static CustomResult Failure(Exception? ex)
        {
            return new CustomResult(Result.Failure(ex));
        }

        public static CustomResult Failure(string errorMessage, Exception? ex)
        {
            return new CustomResult(Result.Failure(errorMessage, ex));
        }

        public static CustomResult Failure(IError error)
        {
            return new CustomResult(Result.Failure(error));
        }

        public static CustomResult Failure(IEnumerable<IError> errors)
        {
            return new CustomResult(Result.Failure(errors));
        }

        public static CustomResult Failure(IReadOnlyList<IError> errors)
        {
            return new CustomResult(Result.Failure(errors));
        }

        public IReadOnlyCollection<IError> Errors => _inner.Errors;

        public bool IsSuccess() => _inner.IsSuccess();

        public bool IsFailure() => _inner.IsFailure();

        public bool IsFailure([MaybeNullWhen(false)] out IError error) => _inner.IsFailure(out error);

        public bool HasError<TError>() where TError : IError => _inner.HasError<TError>();

        public bool HasError<TError>([MaybeNullWhen(false)] out TError error) where TError : IError => _inner.HasError(out error);
    }

    [Test]
    public void CustomActionableResult_Success_ShouldCreateSuccessResult()
    {
        // Act
        var result = CustomResult.Success();

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsFailure().ShouldBeFalse();
    }

    [Test]
    public void CustomActionableResult_Failure_ShouldCreateFailureResult()
    {
        // Act
        var result = CustomResult.Failure();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.Errors.ShouldHaveSingleItem();
    }

    [Test]
    [MethodDataSource(nameof(FailureMetadataCases))]
    public void CustomActionableResult_Failure_WithMetadata_ShouldPropagateMetadata(
        CustomResult result,
        string expectedKey,
        object? expectedValue)
    {
        // Act
        result.IsFailure(out var error).ShouldBeTrue();

        // Assert
        error.ShouldNotBeNull();
        error.Metadata.ContainsKey(expectedKey).ShouldBeTrue();
        error.Metadata[expectedKey].ShouldBe(expectedValue);

        result.HasError<Error>(out var typedError).ShouldBeTrue();
        typedError.ShouldBeSameAs(error);

        result.HasError<CustomError>().ShouldBeFalse();
        result.HasError<CustomError>(out _).ShouldBeFalse();
    }

    [Test]
    [MethodDataSource(nameof(FailureExceptionCases))]
    public void CustomActionableResult_Failure_WithException_ShouldExposeException(
        CustomResult result,
        Exception exception)
    {
        // Act
        result.IsFailure(out var error).ShouldBeTrue();

        // Assert
        error.ShouldNotBeNull();
        error.Exception.ShouldBeSameAs(exception);
        error.Metadata.ContainsKey("Exception").ShouldBeTrue();
        error.Metadata["Exception"].ShouldBeSameAs(exception);

        result.HasError<Error>(out var typedError).ShouldBeTrue();
        typedError.ShouldBeSameAs(error);

        result.HasError<CustomError>().ShouldBeFalse();
        result.HasError<CustomError>(out _).ShouldBeFalse();
    }

    [Test]
    public void CustomActionableResult_Failure_WithSingleError_ShouldPropagateError()
    {
        // Arrange
        var customError = new CustomError();

        // Act
        var result = CustomResult.Failure(customError);

        // Assert
        result.IsFailure(out var error).ShouldBeTrue();
        error.ShouldBeSameAs(customError);
        result.Errors.ShouldHaveSingleItem();

        result.HasError<CustomError>(out var typedError).ShouldBeTrue();
        typedError.ShouldBeSameAs(customError);

        result.HasError<AnotherCustomError>().ShouldBeFalse();
        result.HasError<AnotherCustomError>(out _).ShouldBeFalse();
    }

    [Test]
    [MethodDataSource(nameof(FailureEnumerableErrorsCases))]
    public void CustomActionableResult_Failure_WithEnumerableErrors_ShouldPropagateErrors(
        CustomResult result,
        IReadOnlyList<IError> expectedErrors)
    {
        // Act
        result.IsFailure(out var firstError).ShouldBeTrue();

        // Assert
        firstError.ShouldBeSameAs(expectedErrors[0]);
        result.Errors.ShouldBe(expectedErrors);

        result.HasError<CustomError>().ShouldBeTrue();
        result.HasError<AnotherCustomError>().ShouldBeTrue();
        result.HasError<Error>(out var typedError).ShouldBeTrue();
        typedError.ShouldBeSameAs(firstError);

        result.HasError<UnrelatedError>().ShouldBeFalse();
        result.HasError<UnrelatedError>(out _).ShouldBeFalse();
    }

    public static IEnumerable<object[]> FailureMetadataCases()
    {
        yield return
        [
            CustomResult.Failure("Metadata tuple", ("TupleKey", "TupleValue")),
            "TupleKey",
            "TupleValue",
        ];

        yield return
        [
            CustomResult.Failure("Metadata key value pair", new KeyValuePair<string, object?>("PairKey", 123)),
            "PairKey",
            123,
        ];

        object dictionaryValue = Guid.NewGuid();
        var dictionary = new Dictionary<string, object?>
        {
            { "DictKey", dictionaryValue },
        };

        yield return
        [
            CustomResult.Failure("Metadata dictionary", dictionary),
            "DictKey",
            dictionaryValue,
        ];
    }

    public static IEnumerable<object[]> FailureExceptionCases()
    {
        var exception = new InvalidOperationException("Operation failed");
        yield return [CustomResult.Failure(exception), exception];

        var exceptionWithMessage = new ArgumentException("Bad argument");
        yield return [CustomResult.Failure("Custom message", exceptionWithMessage), exceptionWithMessage];
    }

    public static IEnumerable<object[]> FailureEnumerableErrorsCases()
    {
        var firstError = new CustomError();
        var secondError = new AnotherCustomError();
        var arrayErrors = new IError[] { firstError, secondError };

        yield return
        [
            CustomResult.Failure((IEnumerable<IError>)arrayErrors),
            arrayErrors,
        ];

        var thirdError = new CustomError();
        var fourthError = new AnotherCustomError();
        IReadOnlyList<IError> readOnlyListErrors = new List<IError> { thirdError, fourthError };

        yield return
        [
            CustomResult.Failure(readOnlyListErrors),
            readOnlyListErrors,
        ];
    }

    private sealed class CustomError() : Error("Custom error");

    private sealed class AnotherCustomError() : Error("Another custom error");

    [UsedImplicitly]
    private sealed class UnrelatedError() : Error("Unrelated error");
}
