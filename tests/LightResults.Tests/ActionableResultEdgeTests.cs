#if NET7_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using LightResults.Common;
using Shouldly;

namespace LightResults.Tests;

public sealed class ActionableResultEdgeTests
{
    private readonly struct CustomResult : IActionableResult<CustomResult>
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

    [Fact]
    public void CustomActionableResult_Success_ShouldCreateSuccessResult()
    {
        // Act
        var result = CustomResult.Success();

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsFailure().ShouldBeFalse();
    }

    [Fact]
    public void CustomActionableResult_Failure_ShouldCreateFailureResult()
    {
        // Act
        var result = CustomResult.Failure();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.Errors.ShouldHaveSingleItem();
    }
}
#endif
