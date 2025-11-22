using Shouldly;
using LightResults.Common;

// ReSharper disable SuspiciousTypeConversion.Global

namespace LightResults.Tests;

public sealed class ResultTValueTests
{
    private static readonly IError EmptyError = Error.Empty;

    [Fact]
    public void DefaultStruct_ShouldBeFailureResultWithDefaultValue()
    {
        // Arrange
        Result<int> result = default;

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out var resultValue).ShouldBeFalse();
        resultValue.ShouldBe(0);
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out var resultError).ShouldBeTrue();
        resultError.ShouldBeEquivalentTo(EmptyError);
        result.Errors.ShouldHaveSingleItem();
        result.Errors.Single().ShouldBeOfType<Error>();
        result.HasError<Error>().ShouldBeTrue();
        result.HasError<Error>(out var error).ShouldBeTrue();
        error.ShouldBeEquivalentTo(EmptyError);
        result.HasError<ValidationError>().ShouldBeFalse();
        result.HasError<ValidationError>(out var validationError).ShouldBeFalse();
        validationError.ShouldBeNull();
    }

    [Fact]
    public void DefaultStruct_ShouldBeFailureResultWithNullValue()
    {
        // Arrange
        Result<object> result = default;

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out var resultValue).ShouldBeFalse();
        resultValue.ShouldBeNull();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out var resultError).ShouldBeTrue();
        resultError.ShouldBeEquivalentTo(EmptyError);
        result.Errors.ShouldHaveSingleItem();
        result.Errors.Single().ShouldBeOfType<Error>();
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
        var result = Result.Success(42);

        // Assert
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess_ShouldReturnAssignedValue()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var isSuccess = result.IsSuccess(out var resultValue);

        // Assert
        isSuccess.ShouldBeTrue();
        resultValue.ShouldBe(42);
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess_ShouldReturnAssignedValueAndNullError()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var isSuccess = result.IsSuccess(out var resultValue, out var resultError);

        // Assert
        isSuccess.ShouldBeTrue();
        resultValue.ShouldBe(42);
        resultError.ShouldBeNull();
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailure_ShouldReturnDefaultValue()
    {
        // Arrange
        var result = Result.Failure<int>("Error message");

        // Act
        var isSuccess = result.IsSuccess(out var resultValue);

        // Assert
        isSuccess.ShouldBeFalse();
        resultValue.ShouldBe(0);
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailure_ShouldReturnNullValue()
    {
        // Arrange
        var result = Result.Failure<object>("Error message");

        // Act
        var isSuccess = result.IsSuccess(out var resultValue);

        // Assert
        isSuccess.ShouldBeFalse();
        resultValue.ShouldBeNull();
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailure_ShouldReturnDefaultValueAndFirstError()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError>
        {
            firstError,
            new Error("Error 2"),
        };
        var result = Result.Failure<int>(errors);

        // Act
        var isSuccess = result.IsSuccess(out var resultValue, out var resultError);

        // Assert
        isSuccess.ShouldBeFalse();
        resultValue.ShouldBe(0);
        resultError.ShouldBe(firstError);
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailure_ShouldReturnDefaultValueAndFirstOrEmptyError()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError>
        {
            firstError,
            new Error("Error 2"),
        };

        // Act
        var isSuccess = Result.Failure<int>(errors).IsSuccess(out var resultValue, out var resultError);

        // Assert
        isSuccess.ShouldBeFalse();
        resultValue.ShouldBe(default(int));
        resultError.ShouldBe(firstError);

        Result<int> defaultResult = default;
        defaultResult.IsSuccess(out var defaultValue, out var defaultError).ShouldBeFalse();
        defaultValue.ShouldBe(default(int));
        defaultError.ShouldBeEquivalentTo(EmptyError);
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailure_ShouldReturnNullValueAndFirstError()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError>
        {
            firstError,
            new Error("Error 2"),
        };
        var result = Result.Failure<object>(errors);

        // Act
        var isSuccess = result.IsSuccess(out var resultValue, out var resultError);

        // Assert
        isSuccess.ShouldBeFalse();
        resultValue.ShouldBeNull();
        resultError.ShouldBe(firstError);
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailure_ShouldReturnDefaultValueAndDefaultError()
    {
        // Arrange
        Result<object> result = default;

        // Act
        var isSuccess = result.IsSuccess(out var resultValue, out var resultError);

        // Assert
        isSuccess.ShouldBeFalse();
        resultValue.ShouldBeNull();
        resultError.ShouldBeEquivalentTo(EmptyError);
    }

    [Fact]
    public void IsFailure_WhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failure<int>();

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
        var result = Result.Failure<int>(errors);

        // Act
        var isFailure = result.IsFailure(out var resultError);

        // Assert
        isFailure.ShouldBeTrue();
        resultError.ShouldBe(firstError);
    }

    [Fact]
    public void IsFailure_WhenResultIsSuccess_ShouldReturnNullError()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var isFailure = result.IsFailure(out var resultError);

        // Assert
        isFailure.ShouldBeFalse();
        resultError.ShouldBeNull();
    }

    [Fact]
    public void IsFailure_WhenResultIsFailure_ShouldReturnFirstErrorAndDefaultValue()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError>
        {
            firstError,
            new Error("Error 2"),
        };
        var result = Result.Failure<int>(errors);

        // Act
        var isFailure = result.IsFailure(out var resultError, out var resultValue);

        // Assert
        isFailure.ShouldBeTrue();
        resultError.ShouldBe(firstError);
        resultValue.ShouldBe(0);
    }

    [Fact]
    public void IsFailure_WhenResultIsFailure_ShouldReturnFirstErrorAndNullValue()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError>
        {
            firstError,
            new Error("Error 2"),
        };
        var result = Result.Failure<object>(errors);

        // Act
        var isFailure = result.IsFailure(out var resultError, out var resultValue);

        // Assert
        isFailure.ShouldBeTrue();
        resultError.ShouldBe(firstError);
        resultValue.ShouldBeNull();
    }

    [Fact]
    public void IsFailure_WhenResultIsFailure_ShouldReturnDefaultErrorAndNullValue()
    {
        // Arrange
        Result<object> result = default;

        // Act
        var isFailure = result.IsFailure(out var resultError, out var resultValue);

        // Assert
        isFailure.ShouldBeTrue();
        resultError.ShouldBeEquivalentTo(EmptyError);
        resultValue.ShouldBeNull();
    }

    [Fact]
    public void IsFailure_WhenResultIsSuccess_ShouldReturnNullErrorAndAssignedValue()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var isFailure = result.IsFailure(out var resultError, out var resultValue);

        // Assert
        isFailure.ShouldBeFalse();
        resultError.ShouldBeNull();
        resultValue.ShouldBe(42);
    }

    [Fact]
    public void Success_WithValue_ShouldCreateSuccessResultWithValue()
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
        var result = Result.Failure<int>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.Single().Message.ShouldBe("");
    }

    [Fact]
    public void Failure_WithErrorMessage_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result.Failure<int>(errorMessage);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.Single().Message.ShouldBe(errorMessage);
    }

    [Fact]
    public void Failure_WithErrorMessageAndTupleMetadata_ShouldCreateFailureResultWithSingleError()
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
        result.Errors.ShouldHaveSingleItem();
        var error = result.Errors.Single();
        error.Message.ShouldBe(errorMessage);
        error.Metadata.ShouldHaveSingleItem();
        error.Metadata.Single().ShouldBeEquivalentTo(new KeyValuePair<string, object?>("Key", 0));
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
        var result = Result.Failure<object>(errorMessage, metadata);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.ShouldHaveSingleItem();
        var error = result.Errors.Single();
        error.Message.ShouldBe(errorMessage);
        error.Metadata.ShouldHaveSingleItem();
        error.Metadata.Single().ShouldBeEquivalentTo(new KeyValuePair<string, object?>("Key", 0));
    }

    [Fact]
    public void Failure_WithErrorObject_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Failure<int>(error);

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.Single().ShouldBeEquivalentTo(error);
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
        var result = Result.Failure<int>(errors.AsEnumerable());

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBe(errors);
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
        var result = Result.Failure<int>(errors.AsEnumerable());

        // Assert
        result.Errors.ShouldBeSameAs(errors);
    }

    [Fact]
    public void Failure_WithEmptyErrorsEnumerable_ShouldCreateFailureWithoutErrors()
    {
        // Arrange
        var result = Result<int>.Failure(Enumerable.Empty<IError>());

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsSuccess(out var resultValue).ShouldBeFalse();
        resultValue.ShouldBe(default(int));
        result.IsFailure(out var resultError).ShouldBeFalse();
        resultError.ShouldBeNull();
        result.Errors.ShouldBeEmpty();
        result.HasError<Error>().ShouldBeFalse();
        result.HasError<Error>(out var error).ShouldBeFalse();
        error.ShouldBeNull();
        result.HasError<ValidationError>().ShouldBeFalse();
        result.HasError<ValidationError>(out var validationError).ShouldBeFalse();
        validationError.ShouldBeNull();
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
        var result = Result.Failure<int>(errors);

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
        var result = Result.Failure<int>(new ValidationError("Validation error"));

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
            new ValidationError("Error 2"),
        };
        var result = Result.Failure<int>(errors);

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
        var result = Result.Failure<int>(new Error("Generic error"));

        // Assert
        result.HasError<ValidationError>().ShouldBeFalse();
    }

    [Fact]
    public void HasError_WithNonMatchingErrorType_ShouldOutDefaultError()
    {
        // Arrange
        var result = Result.Failure<int>(new Error("Generic error"));

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
        var result = Result.Success(42);

        // Assert
        result.HasError<ValidationError>().ShouldBeFalse();
    }

    [Fact]
    public void HasError_WhenIsSuccess_ShouldOutDefaultError()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var hasError = result.HasError<ValidationError>(out var error);

        // Assert
        hasError.ShouldBeFalse();
        error.ShouldBeNull();
    }

    [Fact]
    public void ImplicitOperator_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        Result<int> result = value;

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsSuccess(out var resultValue).ShouldBeTrue();
        resultValue.ShouldBe(value);
        result.IsFailure().ShouldBeFalse();
        result.IsFailure(out _).ShouldBeFalse();
        result.Errors.ShouldBeEmpty();
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
        var result = Result.Failure<int>(errors);

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
        Result<int> result = default;

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
        var result = Result.Failure<int>(errors);

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
        Result<int> result = default;

        // Act
        var genericResult = result.AsFailure<object>();

        // Assert
        genericResult.IsSuccess().ShouldBeFalse();
        genericResult.IsFailure().ShouldBeTrue();
        genericResult.Errors.ShouldHaveSingleItem();
        genericResult.Errors.Single().ShouldBeEquivalentTo(EmptyError);
    }

    [Fact]
    public void ImplicitCast_ShouldCreateSuccessResultFromValue()
    {
        // Arrange
        const int value = 42;

        // Act
        Result<int> result = value;

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.IsSuccess(out var resultValue).ShouldBeTrue();
        resultValue.ShouldBe(value);
        result.IsFailure().ShouldBeFalse();
        result.IsFailure(out _).ShouldBeFalse();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void ImplicitCast_ShouldCreateFailureResultFromError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        Result<int> result = error;

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out var resultError).ShouldBeTrue();
        resultError.ShouldBeEquivalentTo(error);
        result.Errors.ShouldHaveSingleItem();
        result.Errors.Single().ShouldBeEquivalentTo(error);
    }

    [Fact]
    public void Equals_ResultInt_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(42);

        // Assert
        result1.Equals(result2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_ResultInt_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(43);

        // Assert
        result1.Equals(result2).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ResultObject_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test");
        var result2 = Result.Success<object>("test");

        // Assert
        result1.Equals(result2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_ResultObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test1");
        var result2 = Result.Success<object>("test2");

        // Assert
        result1.Equals(result2).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ResultIntToObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success<object>(42);

        // Assert
        result1.Equals(result2).ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_ResultInt_ShouldReturnSameHashCodeForEqualResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(42);

        // Assert
        result1.GetHashCode().ShouldBe(result2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ResultInt_ShouldReturnDifferentHashCodeForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(43);

        // Assert
        result1.GetHashCode().ShouldNotBe(result2.GetHashCode());
    }

    [Fact]
    public void op_Equality_ResultInt_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(42);

        // Assert
        (result1 == result2).ShouldBeTrue();
    }

    [Fact]
    public void op_Equality_ResultInt_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(43);

        // Assert
        (result1 == result2).ShouldBeFalse();
    }

    [Fact]
    public void op_Inequality_ResultInt_ShouldReturnFalseForEqualResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(42);

        // Assert
        (result1 != result2).ShouldBeFalse();
    }

    [Fact]
    public void op_Inequality_ResultInt_ShouldReturnTrueForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(43);

        // Assert
        (result1 != result2).ShouldBeTrue();
    }

    [Fact]
    public void op_Equality_ResultObject_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test");
        var result2 = Result.Success<object>("test");

        // Assert
        (result1 == result2).ShouldBeTrue();
    }

    [Fact]
    public void op_Equality_ResultObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test1");
        var result2 = Result.Success<object>("test2");

        // Assert
        (result1 == result2).ShouldBeFalse();
    }

    [Fact]
    public void op_Inequality_ResultObject_ShouldReturnFalseForEqualResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test");
        var result2 = Result.Success<object>("test");

        // Assert
        (result1 != result2).ShouldBeFalse();
    }

    [Fact]
    public void op_Inequality_ResultObject_ShouldReturnTrueForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test1");
        var result2 = Result.Success<object>("test2");

        // Assert
        (result1 != result2).ShouldBeTrue();
    }

    [Fact]
    public void op_Equality_ResultIntToObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success<object>(42);

        // Assert
        (result1 == result2).ShouldBeFalse();
    }

    [Fact]
    public void op_Inequality_ResultIntToObject_ShouldReturnTrueForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success<object>(42);

        // Assert
        (result1 != result2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_Result_DefaultResults_ShouldReturnTrue()
    {
        // Arrange
        Result<int> result1 = default;
        Result<int> result2 = default;

        // Assert
        result1.Equals(result2).ShouldBeTrue();
    }

    [Fact]
    public void GetHashCode_Result_DefaultResults_ShouldReturnSameHashCode()
    {
        // Arrange
        Result<int> result1 = default;
        Result<int> result2 = default;

        // Assert
        result1.GetHashCode().ShouldBe(result2.GetHashCode());
    }

    [Fact]
    public void Equals_Result_DefaultAndFailure_ShouldReturnFalse()
    {
        // Arrange
        Result<int> result1 = default;
        var result2 = Result.Failure<int>();

        // Assert
        result1.Equals(result2).ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_Result_DefaultAndFailure_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        Result<int> result1 = default;
        var result2 = Result.Failure<int>();

        // Assert
        result1.GetHashCode().ShouldNotBe(result2.GetHashCode());
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = True", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForBoolean(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(true) : Result.Failure<bool>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForSByte(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<sbyte>(1) : Result.Failure<sbyte>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForByte(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<byte>(1) : Result.Failure<byte>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForInt16(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<short>(1) : Result.Failure<short>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForUInt16(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<ushort>(1) : Result.Failure<ushort>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForInt32(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(1) : Result.Failure<int>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForUInt32(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<uint>(1) : Result.Failure<uint>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForInt64(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<long>(1) : Result.Failure<long>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForUInt64(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<ulong>(1) : Result.Failure<ulong>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1.1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForDecimal(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(1.1m) : Result.Failure<decimal>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1.1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForFloat(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(1.1f) : Result.Failure<float>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1.1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForDouble(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(1.1d) : Result.Failure<double>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"2024-04-05T12:30:00Z\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForDateTime(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success
            ? Result.Success(new DateTime(2024, 04, 05, 12, 30, 00, DateTimeKind.Utc))
            : Result.Failure<DateTime>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"2024-04-05T12:30:00+00:00\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForDateTimeOffset(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success
            ? Result.Success(new DateTimeOffset(2024, 04, 05, 12, 30, 00, TimeSpan.Zero))
            : Result.Failure<DateTimeOffset>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 'c'", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForChar(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success('c') : Result.Failure<char>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"StringValue\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForString(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success("StringValue") : Result.Failure<string>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForObject(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(new object()) : Result.Failure<object>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForNullableValueTypes(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<int?>(1) : Result.Failure<int?>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 42", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForCustomFormattable(bool success, string expected, string errorMessage)
    {
        // Arrange
        var value = new CustomFormattable(42);
        var result = success ? Result.Success(value) : Result.Failure<CustomFormattable>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    private readonly struct CustomFormattable : IFormattable
    {
        private readonly int _value;

        public CustomFormattable(int value)
        {
            _value = value;
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return _value.ToString(formatProvider);
        }
    }

    private class ValidationError(string errorMessage) : Error(errorMessage);

    [Fact]
    public void InterfaceSuccess_WithValue_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        static Result<TValue> Success<TValue, TResult>(TValue value)
            where TResult : IActionableResult<TValue, Result<TValue>>
        {
            return TResult.Success(value);
        }

        // Act
        var result = Success<int, Result<int>>(value);

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
    public void InterfaceFailure_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result<TValue> Fail<TValue, TResult>()
            where TResult : IActionableResult<TValue, Result<TValue>>
        {
            return TResult.Failure();
        }

        // Act
        var result = Fail<int, Result<int>>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.Single().Message.ShouldBe("");
    }

    [Fact]
    public void InterfaceFailure_WithErrorMessage_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result<TValue> Fail<TValue, TResult>()
            where TResult : IActionableResult<TValue, Result<TValue>>
        {
            const string errorMessage = "Sample error message";
            return TResult.Failure(errorMessage);
        }

        // Act
        var result = Fail<int, Result<int>>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.Single().Message.ShouldBe("Sample error message");
    }

    [Fact]
    public void InterfaceFailure_WithErrorMessageAndTupleMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result<TValue> Fail<TValue, TResult>()
            where TResult : IActionableResult<TValue, Result<TValue>>
        {
            const string errorMessage = "Sample error message";
            (string Key, object Value) metadata = ("Key", 0);
            return TResult.Failure(errorMessage, metadata);
        }

        // Act
        var result = Fail<int, Result<int>>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        var error = result.Errors.ShouldHaveSingleItem();
        error.Message.ShouldBe("Sample error message");
        error.Metadata.ShouldHaveSingleItem();
        error.Metadata.Single().ShouldBeEquivalentTo(new KeyValuePair<string, object?>("Key", 0));
    }

    [Fact]
    public void InterfaceFailure_WithErrorMessageAndKeyValuePairMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result<TValue> Fail<TValue, TResult>()
            where TResult : IActionableResult<TValue, Result<TValue>>
        {
            const string errorMessage = "Sample error message";
            var metadata = new KeyValuePair<string, object?>("Key", 0);
            return TResult.Failure(errorMessage, metadata);
        }

        // Act
        var result = Fail<int, Result<int>>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        var error = result.Errors.ShouldHaveSingleItem();
        error.Message.ShouldBe("Sample error message");
        error.Metadata.ShouldHaveSingleItem();
        error.Metadata.Single().ShouldBeEquivalentTo(new KeyValuePair<string, object?>("Key", 0));
    }

    [Fact]
    public void InterfaceFailure_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result<TValue> Fail<TValue, TResult>()
            where TResult : IActionableResult<TValue, Result<TValue>>
        {
            const string errorMessage = "Sample error message";
            IReadOnlyDictionary<string, object?> metadata = new Dictionary<string, object?>
            {
                { "Key", 0 },
            };
            return TResult.Failure(errorMessage, metadata);
        }

        // Act
        var result = Fail<int, Result<int>>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        var error = result.Errors.ShouldHaveSingleItem();
        error.Message.ShouldBe("Sample error message");
        error.Metadata.ShouldHaveSingleItem();
        error.Metadata.Single().ShouldBeEquivalentTo(new KeyValuePair<string, object?>("Key", 0));
    }

    [Fact]
    public void InterfaceFailure_WithErrorObject_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result<TValue> Fail<TValue, TResult>()
            where TResult : IActionableResult<TValue, Result<TValue>>
        {
            var error = new Error("Sample error");
            return TResult.Failure(error);
        }

        // Act
        var result = Fail<int, Result<int>>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.Single().ShouldBeEquivalentTo(new Error("Sample error"));
    }

    [Fact]
    public void InterfaceFailure_WithErrorsEnumerable_ShouldCreateFailureResultWithMultipleErrors()
    {
        // Arrange
        static (Result<TValue> Result, List<IError> Errors) Fail<TValue, TResult>()
            where TResult : IActionableResult<TValue, Result<TValue>>
        {
            var errors = new List<IError>
            {
                new Error("Error 1"),
                new Error("Error 2"),
            };
            return (TResult.Failure(errors.AsEnumerable()), errors);
        }

        // Act
        var (result, errors) = Fail<int, Result<int>>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBeSameAs(errors);
    }

    [Fact]
    public void InterfaceFailure_WithErrorsReadOnlyList_ShouldCreateFailureResultWithMultipleErrors()
    {
        // Arrange
        static Result<TValue> Fail<TValue, TResult>()
            where TResult : IActionableResult<TValue, Result<TValue>>
        {
            var errors = new List<IError>
            {
                new Error("Error 1"),
                new Error("Error 2"),
            };
            return TResult.Failure(errors);
        }

        // Act
        var result = Fail<int, Result<int>>();

        // Assert
        result.IsSuccess().ShouldBeFalse();
        result.IsSuccess(out _).ShouldBeFalse();
        result.IsFailure().ShouldBeTrue();
        result.IsFailure(out _).ShouldBeTrue();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldBeEquivalentTo(new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2"),
        });
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"2024-04-05\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForDateOnly(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(new DateOnly(2024, 04, 05)) : Result.Failure<DateOnly>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"12:30:00\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForTimeOnly(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(new TimeOnly(12, 30, 00)) : Result.Failure<TimeOnly>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForInt128(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<Int128>(1) : Result.Failure<Int128>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occurred!\"", "An unknown error occurred!")]
    public void ToString_ShouldReturnProperRepresentationForUInt128(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<UInt128>(1) : Result.Failure<UInt128>(errorMessage);

        // Assert
        result.ToString().ShouldBe($"Result {{ {expected} }}");
    }
}
