using Shouldly;
#if NET7_0_OR_GREATER
using LightResults.Common;
#endif

namespace LightResults.Tests;

public sealed class ResultTests
{
    private static readonly Error EmptyError = new();

    [Fact]
    public void DefaultStruct_ShouldBeFailureResult()
    {
        // Arrange
        Result result = default;

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out var resultError).ShouldBeTrue();
        resultError.ShouldBeEquivalentTo(EmptyError);

        result.Errors.ShouldHaveSingleItem();
        result.Errors.First().ShouldBeOfType<Error>();

        result.HasError<Error>().ShouldBeTrue();
        result.HasError<Error>(out var error).ShouldBeTrue();
        error.ShouldBeEquivalentTo(EmptyError);

        result.HasError<ValidationError>().ShouldBeFalse();
        result.HasError<ValidationError>(out var validationError).ShouldBeFalse();
        validationError.ShouldBeNull();
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Assert
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public void IsFailure_WhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failure();

        // Assert
        result.IsFailure().ShouldBeTrue();
    }

    [Fact]
    public void IsFailure_WhenResultIsFailure_ShouldReturnFirstError()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError>
        {
            firstError,
            new Error("Error 2"),
        };
        var result = Result.Failure(errors);

        // Act
        var isFailure = result.IsFailure(out var resultError);

        // Assert
        isFailure.ShouldBeTrue();
        resultError.ShouldBe(firstError);
    }

    [Fact]
    public void IsFailure_WhenResultIsSuccess_ShouldReturnDefaultValue()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var isFailure = result.IsFailure(out var resultError);

        // Assert
        isFailure.ShouldBeFalse();
        resultError.ShouldBeNull();
    }

    [Fact]
    public void IsFailure_WhenResultIsSuccess_ShouldReturnNullValue()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var isFailure = result.IsFailure(out var resultError);

        // Assert
        isFailure.ShouldBeFalse();
        resultError.ShouldBeNull();
    }

    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsFailure().ShouldBeFalse();
        result.IsFailure(out var resultError).ShouldBeFalse();
        resultError.ShouldBeNull();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void SuccessTValue_WithValue_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Result.Success(value);

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsSuccess(out var resultValue).ShouldBeTrue();
        resultValue.ShouldBe(value);
        result.IsFailure().ShouldBeFalse();
        result.IsFailure(out var resultError).ShouldBeFalse();
        resultError.ShouldBeNull();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void Failure_ShouldCreateFailureResultWithSingleError()
    {
        // Act
        var result = Result.Failure();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("");
    }

    [Fact]
    public void Failure_WithErrorMessage_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result.Failure(errorMessage);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);
    }

    [Fact]
    public void Failure_WithErrorMessageAndTupleMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        (string Key, object Value) metadata = ("Key", 0);

        // Act
        var result = Result.Failure(errorMessage, metadata);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata.Single().ShouldBe(new KeyValuePair<string, object>("Key", 0));
    }

    [Fact]
    public void Failure_WithErrorMessageAndKeyValuePairMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new KeyValuePair<string, object>("Key", 0);

        // Act
        var result = Result.Failure(errorMessage, metadata);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata.Single().ShouldBe(new KeyValuePair<string, object>("Key", 0));
    }

    [Fact]
    public void Failure_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        IReadOnlyDictionary<string, object> metadata = new Dictionary<string, object>
        {
            { "Key", 0 },
        };

        // Act
        var result = Result.Failure(errorMessage, metadata);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata.Single().ShouldBe(new KeyValuePair<string, object>("Key", 0));
    }

    [Fact]
    public void Failure_WithException_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string exceptionMessage = "Sample exception message";
        var exception = new InvalidOperationException(exceptionMessage);

        // Act
        var result = Result.Failure(exception);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure().ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(exceptionMessage);

        singleError.Metadata.Count.ShouldBe(1);
        var metadata = singleError.Metadata.Single();
        metadata.Key.ShouldBe("Exception");
        metadata.Value.ShouldBe(exception);
    }

    [Fact]
    public void Failure_WithMessageAndException_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        const string exceptionMessage = "Sample exception message";
        var exception = new InvalidOperationException(exceptionMessage);

        // Act
        var result = Result.Failure(errorMessage, exception);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure().ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        var metadata = singleError.Metadata.Single();
        metadata.Key.ShouldBe("Exception");
        metadata.Value.ShouldBe(exception);
    }

    [Fact]
    public void Failure_WithNullException_ShouldThrow()
    {
        // Arrange
        Exception? exception = null;

        // Act
        Func<object?> act = () => Result.Failure(exception!);

        // Assert
        Should.Throw<ArgumentNullException>(act);
    }

    [Fact]
    public void Failure_WithMessageAndNullException_ShouldThrow()
    {
        // Arrange
        Exception? exception = null;

        // Act
        Func<object?> act = () => Result.Failure("", exception!);

        // Assert
        Should.Throw<ArgumentNullException>(act);
    }

    [Fact]
    public void Failure_WithErrorObject_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.ShouldBe(error);
    }

    [Fact]
    public void Failure_WithErrorsEnumerable_ShouldCreateFailureResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2"),
        };

        // Act
        var result = Result.Failure(errors.AsEnumerable());

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBe(errors);
    }

    [Fact]
    public void Failure_WithErrorsReadOnlyList_ShouldCreateFailureResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2"),
        };

        // Act
        var result = Result.Failure(errors);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBe(errors);
    }

    [Fact]
    public void FailureTValue_ShouldCreateFailureResultWithSingleError()
    {
        // Act
        var result = Result.Failure<object>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("");
    }

    [Fact]
    public void FailureTValue_WithErrorMessage_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result.Failure<object>(errorMessage);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);
    }

    [Fact]
    public void FailureTValue_WithErrorMessageAndTupleMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        (string Key, object Value) metadata = ("Key", 0);

        // Act
        var result = Result.Failure<object>(errorMessage, metadata);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata.Single().ShouldBe(new KeyValuePair<string, object>("Key", 0));
    }

    [Fact]
    public void FailureTValue_WithErrorMessageAndKeyValuePairMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new KeyValuePair<string, object>("Key", 0);

        // Act
        var result = Result.Failure<object>(errorMessage, metadata);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata.Single().ShouldBe(new KeyValuePair<string, object>("Key", 0));
    }

    [Fact]
    public void FailureTValue_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        IReadOnlyDictionary<string, object> metadata = new Dictionary<string, object>
        {
            { "Key", 0 },
        };

        // Act
        var result = Result.Failure<object>(errorMessage, metadata);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata.Single().ShouldBe(new KeyValuePair<string, object>("Key", 0));
    }

    [Fact]
    public void FailureTValue_WithException_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string exceptionMessage = "Sample exception message";
        var exception = new InvalidOperationException(exceptionMessage);

        // Act
        var result = Result.Failure<object>(exception);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure().ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(exceptionMessage);

        singleError.Metadata.Count.ShouldBe(1);
        var metadata = singleError.Metadata.Single();
        metadata.Key.ShouldBe("Exception");
        metadata.Value.ShouldBe(exception);
    }

    [Fact]
    public void FailureTValue_WithMessageAndException_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        const string exceptionMessage = "Sample exception message";
        var exception = new InvalidOperationException(exceptionMessage);

        // Act
        var result = Result.Failure<object>(errorMessage, exception);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure().ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        var metadata = singleError.Metadata.Single();
        metadata.Key.ShouldBe("Exception");
        metadata.Value.ShouldBe(exception);
    }

    [Fact]
    public void FailureTValue_WithNullException_ShouldThrow()
    {
        // Arrange
        Exception? exception = null;

        // Act
        Func<object?> act = () => Result.Failure<object>(exception!);

        // Assert
        Should.Throw<ArgumentNullException>(act);
    }

    [Fact]
    public void FailureTValue_WithMessageAndNullException_ShouldThrow()
    {
        // Arrange
        Exception? exception = null;

        // Act
        Func<object?> act = () => Result.Failure<object>("", exception!);

        // Assert
        Should.Throw<ArgumentNullException>(act);
    }

    [Fact]
    public void FailureTValue_WithErrorObject_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Failure<object>(error);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.ShouldBe(error);
    }

    [Fact]
    public void FailureTValue_WithErrorsEnumerable_ShouldCreateFailureResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2"),
        };

        // Act
        var result = Result.Failure<object>(errors);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBe(errors);
    }

    [Fact]
    public void HasError_WithMatchingErrorType_ShouldReturnTrue()
    {
        // Arrange
        var result = Result.Failure(new ValidationError("Validation error"));

        // Assert
        result.HasError<ValidationError>().ShouldBeTrue();
    }

    [Fact]
    public void HasError_WithMatchingErrorType_ShouldOutFirstMatch()
    {
        // Arrange
        var firstError = new ValidationError("Validation error");
        var errors = new List<IError>
        {
            firstError,
            new ValidationError("Validation error 2"),
        };
        var result = Result.Failure(errors);

        // Act
        var hasError = result.HasError<ValidationError>(out var error);

        // Assert
        hasError.ShouldBeTrue();
        error.ShouldBe(firstError);
    }

    [Fact]
    public void HasError_WithNonMatchingErrorType_ShouldReturnFalse()
    {
        // Arrange
        var result = Result.Failure(new Error("Generic error"));

        // Assert
        result.HasError<ValidationError>().ShouldBeFalse();
    }

    [Fact]
    public void HasError_WithNonMatchingErrorType_ShouldOutDefaultError()
    {
        // Arrange
        var result = Result.Failure(new Error("Generic error"));

        // Act
        var hasError = result.HasError<ValidationError>(out var error);

        // Assert
        hasError.ShouldBeFalse();
        error.ShouldBeNull();
    }

    [Fact]
    public void HasError_WhenIsSuccess_ShouldReturnFalse()
    {
        // Arrange
        var result = Result.Success();

        // Assert
        result.HasError<ValidationError>().ShouldBeFalse();
    }

    [Fact]
    public void HasError_WhenIsSuccess_ShouldOutDefaultError()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var hasError = result.HasError<ValidationError>(out var error);

        // Assert
        hasError.ShouldBeFalse();
        error.ShouldBeNull();
    }

    [Fact]
    public void AsFailure_ShouldConvertResultToNonGenericResultWithSameErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2"),
        };
        var result = Result.Failure(errors);

        // Act
        var nonGenericResult = result.AsFailure();

        // Assert
        nonGenericResult.IsSuccess().ShouldBeFalse();
        nonGenericResult.IsFailure().ShouldBeTrue();
        nonGenericResult.Errors.Count.ShouldBe(2);
        nonGenericResult.Errors.ShouldBe(errors);
    }

    [Fact]
    public void AsFailure_ShouldConvertDefaultResultToNonGenericResult()
    {
        // Arrange
        Result result = default;

        // Act
        var nonGenericResult = result.AsFailure();

        // Assert
        nonGenericResult.IsSuccess().ShouldBeFalse();
        nonGenericResult.IsFailure().ShouldBeTrue();
        nonGenericResult.Errors.ShouldHaveSingleItem();
        nonGenericResult.Errors.Single().ShouldBeEquivalentTo(EmptyError);
    }

    [Fact]
    public void AsFailure_ShouldConvertResultToGenericResultWithSameErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2"),
        };
        var result = Result.Failure(errors);

        // Act
        var genericResult = result.AsFailure<object>();

        // Assert
        genericResult.IsSuccess().ShouldBeFalse();
        genericResult.IsFailure().ShouldBeTrue();
        genericResult.Errors.Count.ShouldBe(2);
        genericResult.Errors.ShouldBe(errors);
    }

    [Fact]
    public void AsFailure_ShouldConvertDefaultResultToGenericResult()
    {
        // Arrange
        Result result = default;

        // Act
        var genericResult = result.AsFailure<object>();

        // Assert
        genericResult.IsSuccess().ShouldBeFalse();
        genericResult.IsFailure().ShouldBeTrue();
        genericResult.Errors.ShouldHaveSingleItem();
        genericResult.Errors.Single().ShouldBeEquivalentTo(EmptyError);
    }

    [Fact]
    public void ImplicitCast_ShouldCreateFailureResultFromError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        Result result = error;

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out var resultError).ShouldBeTrue();
        resultError.ShouldBe(error);

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.ShouldBe(error);
    }

    [Fact]
    public void Equals_Result_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Success();

        // Assert
        result1.Equals(result2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_Result_ShouldReturnFalseForUnequalResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Failure("Error");

        // Assert
        result1.Equals(result2).ShouldBeFalse();
    }

    [Fact]
    public void Equals_Object_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Success();

        // Assert
        result1.Equals((object)result2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_Object_ShouldReturnFalseForUnequalResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Failure("Error");

        // Assert
        result1.Equals((object)result2).ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForEqualResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Success();

        // Assert
        result1.GetHashCode().ShouldBe(result2.GetHashCode());
    }

    [Fact]
    public void op_Equality_Result_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Success();

        // Assert
        (result1 == result2).ShouldBeTrue();
    }

    [Fact]
    public void op_Equality_Result_ShouldReturnFalseForUnequalResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Failure("Error");

        // Assert
        (result1 == result2).ShouldBeFalse();
    }

    [Fact]
    public void op_Inequality_Result_ShouldReturnFalseForEqualResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Success();

        // Assert
        (result1 != result2).ShouldBeFalse();
    }

    [Fact]
    public void op_Inequality_Result_ShouldReturnTrueForUnequalResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Failure("Error");

        // Assert
        (result1 != result2).ShouldBeTrue();
    }

    [Fact]
    public void ToString_WhenSuccess_ShouldReturnStringRepresentation()
    {
        // Arrange
        var result = Result.Success();

        // Assert
        result.ToString().ShouldBe("Result { IsSuccess = True }");
    }

    [Theory]
    [InlineData("")]
    [InlineData("An unknown error occured!")]
    public void ToString_WhenFailure_ShouldReturnStringRepresentation(string errorMessage)
    {
        // Arrange
        var result = Result.Failure(errorMessage);

        // Assert
        result.ToString().ShouldBe(
            errorMessage.Length > 0
                ? $"Result {{ IsSuccess = False, Error = \"{errorMessage}\" }}"
                : "Result { IsSuccess = False }"
        );
    }

    private class ValidationError(string errorMessage) : Error(errorMessage);

#if NET7_0_OR_GREATER
    [Fact]
    public void InterfaceSuccess_ShouldCreateSuccessResult()
    {
        // Arrange
        static Result Success<TResult>()
            where TResult : IActionableResult<Result>
        {
            return TResult.Success();
        }

        // Act
        var result = Success<Result>();

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsFailure().ShouldBeFalse();
        result.IsFailure(out var resultError).ShouldBeFalse();
        resultError.ShouldBeNull();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void InterfaceFailure_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result Fail<TResult>()
            where TResult : IActionableResult<Result>
        {
            return TResult.Failure();
        }

        // Act
        var result = Fail<Result>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("");
    }

    [Fact]
    public void InterfaceFailure_WithErrorMessage_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result Fail<TResult>()
            where TResult : IActionableResult<Result>
        {
            const string errorMessage = "Sample error message";
            return TResult.Failure(errorMessage);
        }

        // Act
        var result = Fail<Result>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("Sample error message");
    }

    [Fact]
    public void InterfaceFailure_WithErrorMessageAndTupleMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result Fail<TResult>()
            where TResult : IActionableResult<Result>
        {
            const string errorMessage = "Sample error message";
            (string Key, object Value) metadata = ("Key", 0);
            return TResult.Failure(errorMessage, metadata);
        }

        // Act
        var result = Fail<Result>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("Sample error message");

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata.Single().ShouldBe(new KeyValuePair<string, object>("Key", 0));
    }

    [Fact]
    public void InterfaceFailure_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result Fail<TResult>()
            where TResult : IActionableResult<Result>
        {
            const string errorMessage = "Sample error message";
            IReadOnlyDictionary<string, object> metadata = new Dictionary<string, object>
            {
                { "Key", 0 },
            };
            return TResult.Failure(errorMessage, metadata);
        }

        // Act
        var result = Fail<Result>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("Sample error message");

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata.Single().ShouldBe(new KeyValuePair<string, object>("Key", 0));
    }

    [Fact]
    public void InterfaceFailure_WithErrorObject_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result Fail<TResult>()
            where TResult : IActionableResult<Result>
        {
            var error = new Error("Sample error");
            return TResult.Failure(error);
        }

        // Act
        var result = Fail<Result>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.ShouldBeEquivalentTo(new Error("Sample error"));
    }

    [Fact]
    public void InterfaceFailure_WithErrorsEnumerable_ShouldCreateFailureResultWithMultipleErrors()
    {
        // Arrange
        static Result Fail<TResult>()
            where TResult : IActionableResult<Result>
        {
            var errors = new List<IError>
            {
                new Error("Error 1"),
                new Error("Error 2"),
            };
            return TResult.Failure(errors.AsEnumerable());
        }

        // Act
        var result = Fail<Result>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBeEquivalentTo(new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2"),
        }.ToArray());
    }

    [Fact]
    public void InterfaceFailure_WithErrorsReadOnlyList_ShouldCreateFailureResultWithMultipleErrors()
    {
        // Arrange
        static Result Fail<TResult>()
            where TResult : IActionableResult<Result>
        {
            var errors = new List<IError>
            {
                new Error("Error 1"),
                new Error("Error 2"),
            };
            return TResult.Failure(errors);
        }

        // Act
        var result = Fail<Result>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();

        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBeEquivalentTo(new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2"),
        });
    }
#endif
}
