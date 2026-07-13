using System.Collections;
using Shouldly;
using LightResults.Common;

namespace LightResults.Tests;

public sealed class ResultTests
{
    private static readonly IError EmptyError = Error.Empty;

    [Fact]
    public void DefaultStruct_ShouldBeFailureResult()
    {
        // Arrange
        Result result = default;

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out var resultError)
            .ShouldBeTrue();
        resultError.ShouldBeEquivalentTo(EmptyError);

        result.Errors.ShouldHaveSingleItem();
        result.Errors
            .First()
            .ShouldBeOfType<Error>();

        result.HasError<Error>()
            .ShouldBeTrue();
        result.HasError<Error>(out var error)
            .ShouldBeTrue();
        error.ShouldBeEquivalentTo(EmptyError);

        result.HasError<ValidationError>()
            .ShouldBeFalse();
        result.HasError<ValidationError>(out var validationError)
            .ShouldBeFalse();
        validationError.ShouldBeNull();
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Assert
        result.IsSuccess()
            .ShouldBeTrue();
    }

    [Fact]
    public void IsFailure_WhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failure();

        // Assert
        result.IsFailure()
            .ShouldBeTrue();
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
        result.IsSuccess()
            .ShouldBeTrue();
        result.IsFailure()
            .ShouldBeFalse();
        result.IsFailure(out var resultError)
            .ShouldBeFalse();
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
        result.IsSuccess()
            .ShouldBeTrue();
        result.IsSuccess(out var resultValue)
            .ShouldBeTrue();
        resultValue.ShouldBe(value);
        result.IsFailure()
            .ShouldBeFalse();
        result.IsFailure(out var resultError)
            .ShouldBeFalse();
        resultError.ShouldBeNull();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void Failure_ShouldCreateFailureResultWithSingleError()
    {
        // Act
        var result = Result.Failure();

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("");
    }

    [Fact]
    public void Failure_DefaultErrors_ShouldNotAllowSharedStateMutation()
    {
        // Arrange
        var result = Result.Failure();
        var errors = (IList<IError>)result.Errors;

        // Act & Assert
        errors.IsReadOnly.ShouldBeTrue();
        Should.Throw<NotSupportedException>(() => errors[0] = new Error("Replacement"));
        Result.Failure().Errors.ShouldHaveSingleItem().ShouldBeSameAs(Error.Empty);
    }

    [Fact]
    public void Failure_WithErrorMessage_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result.Failure(errorMessage);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);
    }

    [Fact]
    public void Failure_WithReadOnlyCollection_ShouldPreallocateFromCount()
    {
        // Arrange
        var firstError = new Error("first");
        var secondError = new Error("second");
        var errors = new TestReadOnlyCollection(firstError, secondError);

        // Act
        var result = Result.Failure(errors);

        // Assert
        errors.CountAccesses.ShouldBe(1);
        result.Errors.ShouldBe([firstError, secondError]);
    }

    [Fact]
    public void Failure_WithICollection_ShouldCopyUsingCopyTo()
    {
        // Arrange
        var firstError = new Error("first");
        var secondError = new Error("second");
        var errors = new CopyTrackingCollection(firstError, secondError);

        // Act
        var result = Result.Failure(errors);

        // Assert
        errors.CopyToCalls.ShouldBe(1);
        result.Errors.ShouldBe([firstError, secondError]);
    }

    [Fact]
    public void FailureTValue_WithReadOnlyCollection_ShouldPreallocateFromCount()
    {
        // Arrange
        var firstError = new Error("first");
        var secondError = new Error("second");
        var errors = new TestReadOnlyCollection(firstError, secondError);

        // Act
        var result = Result.Failure<int>(errors);

        // Assert
        errors.CountAccesses.ShouldBe(1);
        result.Errors.ShouldBe([firstError, secondError]);
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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata
            .Single()
            .ShouldBe(new KeyValuePair<string, object?>("Key", 0));
    }

    [Fact]
    public void Failure_WithErrorMessageAndKeyValuePairMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new KeyValuePair<string, object?>("Key", 0);

        // Act
        var result = Result.Failure(errorMessage, metadata);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata
            .Single()
            .ShouldBe(new KeyValuePair<string, object?>("Key", 0));
    }

    [Fact]
    public void Failure_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        IReadOnlyDictionary<string, object?> metadata = new Dictionary<string, object?>
        {
            { "Key", 0 },
        };

        // Act
        var result = Result.Failure(errorMessage, metadata);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata
            .Single()
            .ShouldBe(new KeyValuePair<string, object?>("Key", 0));
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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure()
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe($"{exception.GetType().Name}: {exceptionMessage}");

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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure()
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        var metadata = singleError.Metadata.Single();
        metadata.Key.ShouldBe("Exception");
        metadata.Value.ShouldBe(exception);
    }

    [Fact]
    public void Failure_WithNullException_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        Exception? exception = null;

        // Act
        var result = Result.Failure(exception);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("");
        singleError.Metadata.Count.ShouldBe(0);
    }

    [Fact]
    public void Failure_WithMessageAndNullException_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        Exception? exception = null;

        // Act
        var result = Result.Failure(errorMessage, exception);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);
        singleError.Metadata.Count.ShouldBe(0);
    }

    [Fact]
    public void Failure_WithErrorObject_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBe(errors);
    }

    [Fact]
    public void Failure_WithEmptyErrorsEnumerable_ShouldCreateFailureWithoutErrors()
    {
        // Arrange
        var result = Result.Failure(Enumerable.Empty<IError>());

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out var resultError)
            .ShouldBeTrue();
        resultError.ShouldBeEquivalentTo(EmptyError);
        result.Errors.ShouldBeEmpty();

        result.HasError<Error>()
            .ShouldBeTrue();
        result.HasError<Error>(out var error)
            .ShouldBeTrue();
        error.ShouldBeEquivalentTo(EmptyError);

        result.HasError<ValidationError>()
            .ShouldBeFalse();
        result.HasError<ValidationError>(out var validationError)
            .ShouldBeFalse();
        validationError.ShouldBeNull();

        result.ToString()
            .ShouldBe("Result { IsSuccess = False }");
    }

    [Fact]
    public void Failure_WithCustomIterator_ShouldMaterializeErrors()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var secondError = new Error("Error 2");
        var iterator = new TestIterator(firstError, secondError);

        // Act
        var result = Result.Failure(iterator);

        // Assert
        iterator.EnumerationCount.ShouldBe(1);
        result.Errors.ShouldBe([firstError, secondError]);
        result.Errors.ShouldBe([firstError, secondError]);
        iterator.EnumerationCount.ShouldBe(1);
    }

    [Fact]
    public void Failure_WithErrorsEnumerable_ShouldReuseListInstance()
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
        result.Errors.ShouldBeSameAs(errors);
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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBe(errors);
    }

    [Fact]
    public void FailureTValue_ShouldCreateFailureResultWithSingleError()
    {
        // Act
        var result = Result.Failure<object>();

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsSuccess(out _)
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsSuccess(out _)
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsSuccess(out _)
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata
            .Single()
            .ShouldBe(new KeyValuePair<string, object?>("Key", 0));
    }

    [Fact]
    public void FailureTValue_WithErrorMessageAndKeyValuePairMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        var metadata = new KeyValuePair<string, object?>("Key", 0);

        // Act
        var result = Result.Failure<object>(errorMessage, metadata);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsSuccess(out _)
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata
            .Single()
            .ShouldBe(new KeyValuePair<string, object?>("Key", 0));
    }

    [Fact]
    public void FailureTValue_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        IReadOnlyDictionary<string, object?> metadata = new Dictionary<string, object?>
        {
            { "Key", 0 },
        };

        // Act
        var result = Result.Failure<object>(errorMessage, metadata);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsSuccess(out _)
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata
            .Single()
            .ShouldBe(new KeyValuePair<string, object?>("Key", 0));
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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure()
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe($"{exception.GetType().Name}: {exceptionMessage}");

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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure()
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);

        singleError.Metadata.Count.ShouldBe(1);
        var metadata = singleError.Metadata.Single();
        metadata.Key.ShouldBe("Exception");
        metadata.Value.ShouldBe(exception);
    }

    [Fact]
    public void FailureTValue_WithNullException_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        Exception? exception = null;

        // Act
        var result = Result.Failure<object>(exception);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("");
        singleError.Metadata.Count.ShouldBe(0);
    }

    [Fact]
    public void FailureTValue_WithMessageAndNullException_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        Exception? exception = null;

        // Act
        var result = Result.Failure<object>(errorMessage, exception);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe(errorMessage);
        singleError.Metadata.Count.ShouldBe(0);
    }

    [Fact]
    public void FailureTValue_WithErrorObject_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Failure<object>(error);

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsSuccess(out _)
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsSuccess(out _)
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBe(errors);
    }

    [Fact]
    public void FailureTValue_WithCustomIterator_ShouldMaterializeErrors()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var secondError = new Error("Error 2");
        var iterator = new TestIterator(firstError, secondError);

        // Act
        var result = Result.Failure<object>(iterator);

        // Assert
        iterator.EnumerationCount.ShouldBe(1);
        result.Errors.ShouldBe([firstError, secondError]);
        result.Errors.ShouldBe([firstError, secondError]);
        iterator.EnumerationCount.ShouldBe(1);
    }

    [Fact]
    public void HasError_WithMatchingErrorType_ShouldReturnTrue()
    {
        // Arrange
        var result = Result.Failure(new ValidationError("Validation error"));

        // Assert
        result.HasError<ValidationError>()
            .ShouldBeTrue();
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
        result.HasError<ValidationError>()
            .ShouldBeFalse();
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
        result.HasError<ValidationError>()
            .ShouldBeFalse();
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
        nonGenericResult.IsSuccess()
            .ShouldBeFalse();
        nonGenericResult.IsFailure()
            .ShouldBeTrue();
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
        nonGenericResult.IsSuccess()
            .ShouldBeFalse();
        nonGenericResult.IsFailure()
            .ShouldBeTrue();
        nonGenericResult.Errors.ShouldHaveSingleItem();
        nonGenericResult.Errors
            .Single()
            .ShouldBeEquivalentTo(EmptyError);
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
        genericResult.IsSuccess()
            .ShouldBeFalse();
        genericResult.IsFailure()
            .ShouldBeTrue();
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
        genericResult.IsSuccess()
            .ShouldBeFalse();
        genericResult.IsFailure()
            .ShouldBeTrue();
        genericResult.Errors.ShouldHaveSingleItem();
        genericResult.Errors
            .Single()
            .ShouldBeEquivalentTo(EmptyError);
    }

    [Fact]
    public void ImplicitCast_ShouldCreateFailureResultFromError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        Result result = error;

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out var resultError)
            .ShouldBeTrue();
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
        result1.Equals(result2)
            .ShouldBeTrue();
    }

    [Fact]
    public void Equals_Result_ShouldReturnFalseForUnequalResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Failure("Error");

        // Assert
        result1.Equals(result2)
            .ShouldBeFalse();
    }

    [Fact]
    public void Equals_Object_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Success();

        // Assert
        result1.Equals((object)result2)
            .ShouldBeTrue();
    }

    [Fact]
    public void Equals_Object_ShouldReturnFalseForUnequalResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Failure("Error");

        // Assert
        result1.Equals((object)result2)
            .ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForEqualResults()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Success();

        // Assert
        result1.GetHashCode()
            .ShouldBe(result2.GetHashCode());
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
    public void Equals_Result_DefaultResults_ShouldReturnTrue()
    {
        // Arrange
        Result result1 = default;
        Result result2 = default;

        // Assert
        result1.Equals(result2)
            .ShouldBeTrue();
    }

    [Fact]
    public void GetHashCode_Result_DefaultResults_ShouldReturnSameHashCode()
    {
        // Arrange
        Result result1 = default;
        Result result2 = default;

        // Assert
        result1.GetHashCode()
            .ShouldBe(result2.GetHashCode());
    }

    [Fact]
    public void Equals_Result_DefaultAndSuccess_ShouldReturnFalse()
    {
        // Arrange
        Result result1 = default;
        var result2 = Result.Success();

        // Assert
        result1.Equals(result2)
            .ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_Result_DefaultAndSuccess_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        Result result1 = default;
        var result2 = Result.Success();

        // Assert
        result1.GetHashCode()
            .ShouldNotBe(result2.GetHashCode());
    }

    [Fact]
    public void Equals_Result_DefaultAndFailure_ShouldReturnFalse()
    {
        // Arrange
        Result result1 = default;
        var result2 = Result.Failure();

        // Assert
        result1.Equals(result2)
            .ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_Result_DefaultAndFailure_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        Result result1 = default;
        var result2 = Result.Failure();

        // Assert
        result1.GetHashCode()
            .ShouldNotBe(result2.GetHashCode());
    }

    [Fact]
    public void ToString_WhenSuccess_ShouldReturnStringRepresentation()
    {
        // Arrange
        var result = Result.Success();

        // Assert
        result.ToString()
            .ShouldBe("Result { IsSuccess = True }");
    }

    [Theory]
    [InlineData("")]
    [InlineData("An unknown error occurred!")]
    public void ToString_WhenFailure_ShouldReturnStringRepresentation(string errorMessage)
    {
        // Arrange
        var result = Result.Failure(errorMessage);

        // Assert
        result.ToString()
            .ShouldBe(errorMessage.Length > 0 ? $"Result {{ IsSuccess = False, Error = \"{errorMessage}\" }}" : "Result { IsSuccess = False }");
    }

    private class ValidationError(string errorMessage) : Error(errorMessage);

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
        result.IsSuccess()
            .ShouldBeTrue();
        result.IsFailure()
            .ShouldBeFalse();
        result.IsFailure(out var resultError)
            .ShouldBeFalse();
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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("Sample error message");

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata
            .Single()
            .ShouldBe(new KeyValuePair<string, object?>("Key", 0));
    }

    [Fact]
    public void InterfaceFailure_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result Fail<TResult>()
            where TResult : IActionableResult<Result>
        {
            const string errorMessage = "Sample error message";
            IReadOnlyDictionary<string, object?> metadata = new Dictionary<string, object?>
            {
                { "Key", 0 },
            };
            return TResult.Failure(errorMessage, metadata);
        }

        // Act
        var result = Fail<Result>();

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.Message.ShouldBe("Sample error message");

        singleError.Metadata.Count.ShouldBe(1);
        singleError.Metadata
            .Single()
            .ShouldBe(new KeyValuePair<string, object?>("Key", 0));
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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        var singleError = result.Errors.ShouldHaveSingleItem();
        singleError.ShouldBeEquivalentTo(new Error("Sample error"));
    }

    [Fact]
    public void InterfaceFailure_WithErrorsEnumerable_ShouldCreateFailureResultWithMultipleErrors()
    {
        // Arrange
        static (Result Result, List<IError> Errors) Fail<TResult>()
            where TResult : IActionableResult<Result>
        {
            var errors = new List<IError>
            {
                new Error("Error 1"),
                new Error("Error 2"),
            };
            return (TResult.Failure(errors.AsEnumerable()), errors);
        }

        // Act
        var (result, errors) = Fail<Result>();

        // Assert
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBeSameAs(errors);
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
        result.IsSuccess()
            .ShouldBeFalse();
        result.IsFailure()
            .ShouldBeTrue();
        result.IsFailure(out _)
            .ShouldBeTrue();

        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBeEquivalentTo(new List<IError>
            {
                new Error("Error 1"),
                new Error("Error 2"),
            }
        );
    }

    private sealed class CopyTrackingCollection : ICollection<IError>
    {
        private readonly IError[] _errors;

        internal CopyTrackingCollection(params IError[] errors)
        {
            _errors = errors;
        }

        internal int CopyToCalls { get; private set; }

        public int Count => _errors.Length;

        public bool IsReadOnly => false;

        public void Add(IError item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(IError item)
        {
            return _errors.Contains(item);
        }

        public void CopyTo(IError[] array, int arrayIndex)
        {
            CopyToCalls++;
            _errors.CopyTo(array, arrayIndex);
        }

        public bool Remove(IError item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<IError> GetEnumerator()
        {
            return ((IEnumerable<IError>)_errors).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    private sealed class TestReadOnlyCollection : IReadOnlyCollection<IError>
    {
        private readonly IError[] _errors;

        internal TestReadOnlyCollection(params IError[] errors)
        {
            _errors = errors;
        }

        internal int CountAccesses { get; private set; }

        public int Count
        {
            get
            {
                CountAccesses++;
                return _errors.Length;
            }
        }

        public IEnumerator<IError> GetEnumerator()
        {
            return ((IEnumerable<IError>)_errors).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    private sealed class TestIterator : IEnumerable<IError>
    {
        private readonly IError[] _errors;

        internal TestIterator(params IError[] errors)
        {
            _errors = errors;
        }

        internal int EnumerationCount { get; private set; }

        public IEnumerator<IError> GetEnumerator()
        {
            EnumerationCount++;

            foreach (var t in _errors)
            {
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
