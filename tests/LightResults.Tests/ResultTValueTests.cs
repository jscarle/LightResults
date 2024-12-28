using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
#if NET7_0_OR_GREATER
using LightResults.Common;
#endif

// ReSharper disable SuspiciousTypeConversion.Global

namespace LightResults.Tests;

public sealed class ResultTValueTests
{
    [Fact]
    public void DefaultStruct_ShouldBeFailureResultWithDefaultValue()
    {
        // Arrange
        Result<int> result = default;

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out var resultValue)
                .Should()
                .BeFalse();
            resultValue.Should()
                .Be(0);
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out var resultError)
                .Should()
                .BeTrue();
            resultError.Should()
                .Be(Error.Empty);
            result.Errors
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeOfType<Error>();
            result.HasError<Error>()
                .Should()
                .BeTrue();
            result.HasError<Error>(out var error)
                .Should()
                .BeTrue();
            error.Should()
                .Be(Error.Empty);
            result.HasError<ValidationError>()
                .Should()
                .BeFalse();
            result.HasError<ValidationError>(out var validationError)
                .Should()
                .BeFalse();
            validationError.Should()
                .Be(null);
        }
    }

    [Fact]
    public void DefaultStruct_ShouldBeFailureResultWithNullValue()
    {
        // Arrange
        Result<object> result = default;

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out var resultValue)
                .Should()
                .BeFalse();
            resultValue.Should()
                .Be(null);
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out var resultError)
                .Should()
                .BeTrue();
            resultError.Should()
                .Be(Error.Empty);
            result.Errors
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeOfType<Error>();
            result.HasError<Error>()
                .Should()
                .BeTrue();
            result.HasError<Error>(out var error)
                .Should()
                .BeTrue();
            error.Should()
                .Be(Error.Empty);
            result.HasError<ValidationError>()
                .Should()
                .BeFalse();
            result.HasError<ValidationError>(out var validationError)
                .Should()
                .BeFalse();
            validationError.Should()
                .Be(null);
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success(42);

        // Assert
        result.IsSuccess()
            .Should()
            .BeTrue();
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess_ShouldReturnAssignedValue()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var isSuccess = result.IsSuccess(out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should()
                .BeTrue();
            resultValue.Should()
                .Be(42);
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess_ShouldReturnAssignedValueAndNullError()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var isSuccess = result.IsSuccess(out var resultValue, out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should()
                .BeTrue();
            resultValue.Should()
                .Be(42);
            resultError.Should()
                .Be(null);
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailure_ShouldReturnDefaultValue()
    {
        // Arrange
        var result = Result.Failure<int>("Error message");

        // Act
        var isSuccess = result.IsSuccess(out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should()
                .BeFalse();
            resultValue.Should()
                .Be(0);
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailure_ShouldReturnNullValue()
    {
        // Arrange
        var result = Result.Failure<object>("Error message");

        // Act
        var isSuccess = result.IsSuccess(out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should()
                .BeFalse();
            resultValue.Should()
                .Be(null);
        }
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
        using (new AssertionScope())
        {
            isSuccess.Should()
                .BeFalse();
            resultValue.Should()
                .Be(0);
            resultError.Should()
                .Be(firstError);
        }
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
        using (new AssertionScope())
        {
            isSuccess.Should()
                .BeFalse();
            resultValue.Should()
                .Be(null);
            resultError.Should()
                .Be(firstError);
        }
    }

    [Fact]
    public void IsSuccess_WhenResultIsFailure_ShouldReturnDefaultValueAndDefaultError()
    {
        // Arrange
        Result<object> result = default;

        // Act
        var isSuccess = result.IsSuccess(out var resultValue, out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isSuccess.Should()
                .BeFalse();
            resultValue.Should()
                .Be(null);
            resultError.Should()
                .Be(Error.Empty);
        }
    }

    [Fact]
    public void IsFailure_WhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failure<int>();

        // Assert
        result.IsFailure()
            .Should()
            .BeTrue();
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
        using (new AssertionScope())
        {
            isFailure.Should()
                .BeTrue();
            resultError.Should()
                .Be(firstError);
        }
    }

    [Fact]
    public void IsFailure_WhenResultIsSuccess_ShouldReturnNullError()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var isFailure = result.IsFailure(out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isFailure.Should()
                .BeFalse();
            resultError.Should()
                .Be(null);
        }
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
        using (new AssertionScope())
        {
            isFailure.Should()
                .BeTrue();
            resultError.Should()
                .Be(firstError);
            resultValue.Should()
                .Be(0);
        }
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
        using (new AssertionScope())
        {
            isFailure.Should()
                .BeTrue();
            resultError.Should()
                .Be(firstError);
            resultValue.Should()
                .Be(null);
        }
    }

    [Fact]
    public void IsFailure_WhenResultIsFailure_ShouldReturnDefaultErrorAndNullValue()
    {
        // Arrange
        Result<object> result = default;

        // Act
        var isFailure = result.IsFailure(out var resultError, out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isFailure.Should()
                .BeTrue();
            resultError.Should()
                .Be(Error.Empty);
            resultValue.Should()
                .Be(null);
        }
    }

    [Fact]
    public void IsFailure_WhenResultIsSuccess_ShouldReturnNullErrorAndAssignedValue()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var isFailure = result.IsFailure(out var resultError, out var resultValue);

        // Assert
        using (new AssertionScope())
        {
            isFailure.Should()
                .BeFalse();
            resultError.Should()
                .Be(null);
            resultValue.Should()
                .Be(42);
        }
    }

    [Fact]
    public void Success_WithValue_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Result.Success(value);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeTrue();
            result.IsSuccess(out var resultValue)
                .Should()
                .BeTrue();
            resultValue.Should()
                .Be(value);
            result.IsFailure()
                .Should()
                .BeFalse();
            result.IsFailure(out var resultError)
                .Should()
                .BeFalse();
            resultError.Should()
                .Be(null);
            result.Errors
                .Should()
                .BeEmpty();
        }
    }

    [Fact]
    public void Failure_ShouldCreateFailureResultWithSingleError()
    {
        // Act
        var result = Result.Failure<int>();

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            result.Errors
                .Should()
                .ContainSingle()
                .Which
                .Message
                .Should()
                .Be("");
        }
    }

    [Fact]
    public void Failure_WithErrorMessage_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result.Failure<int>(errorMessage);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            result.Errors
                .Should()
                .ContainSingle()
                .Which
                .Message
                .Should()
                .Be(errorMessage);
        }
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
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            var error = result.Errors
                .Should()
                .ContainSingle()
                .Which;
            error.Message
                .Should()
                .Be(errorMessage);
            error.Metadata
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
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
        var result = Result.Failure<object>(errorMessage, metadata);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            var error = result.Errors
                .Should()
                .ContainSingle()
                .Which;
            error.Message
                .Should()
                .Be(errorMessage);
            error.Metadata
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
    }

    [Fact]
    public void Failure_WithErrorObject_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Failure<int>(error);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            result.Errors
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo(error);
        }
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
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            result.Errors
                .Should()
                .HaveCount(2)
                .And
                .BeEquivalentTo(errors);
        }
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
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            result.Errors
                .Should()
                .HaveCount(2)
                .And
                .BeEquivalentTo(errors);
        }
    }

    [Fact]
    public void HasError_WithMatchingErrorType_ShouldReturnTrue()
    {
        // Arrange
        var result = Result.Failure<int>(new ValidationError("Validation error"));

        // Assert
        result.HasError<ValidationError>()
            .Should()
            .BeTrue();
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
        using (new AssertionScope())
        {
            hasError.Should()
                .BeTrue();
            error.Should()
                .Be(firstError);
        }
    }

    [Fact]
    public void HasError_WithNonMatchingErrorType_ShouldReturnFalse()
    {
        // Arrange
        var result = Result.Failure<int>(new Error("Generic error"));

        // Assert
        result.HasError<ValidationError>()
            .Should()
            .BeFalse();
    }

    [Fact]
    public void HasError_WithNonMatchingErrorType_ShouldOutDefaultError()
    {
        // Arrange
        var result = Result.Failure<int>(new Error("Generic error"));

        // Act
        var hasError = result.HasError<ValidationError>(out var error);

        // Assert
        using (new AssertionScope())
        {
            hasError.Should()
                .BeFalse();
            error.Should()
                .Be(null);
        }
    }

    [Fact]
    public void HasError_WhenIsSuccess_ShouldReturnFalse()
    {
        // Arrange
        var result = Result.Success(42);

        // Assert
        result.HasError<ValidationError>()
            .Should()
            .BeFalse();
    }

    [Fact]
    public void HasError_WhenIsSuccess_ShouldOutDefaultError()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var hasError = result.HasError<ValidationError>(out var error);

        // Assert
        using (new AssertionScope())
        {
            hasError.Should()
                .BeFalse();
            error.Should()
                .Be(null);
        }
    }

    [Fact]
    public void ImplicitOperator_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        Result<int> result = value;

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeTrue();
            result.IsSuccess(out var resultValue)
                .Should()
                .BeTrue();
            resultValue.Should()
                .Be(value);
            result.IsFailure()
                .Should()
                .BeFalse();
            result.IsFailure(out _)
                .Should()
                .BeFalse();
            result.Errors
                .Should()
                .BeEmpty();
        }
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
        using (new AssertionScope())
        {
            nonGenericResult.IsSuccess()
                .Should()
                .BeFalse();
            nonGenericResult.IsFailure()
                .Should()
                .BeTrue();
            nonGenericResult.Errors
                .Should()
                .HaveCount(2)
                .And
                .BeEquivalentTo(errors);
        }
    }

    [Fact]
    public void AsFailure_ShouldConvertDefaultResultToNonGenericResult()
    {
        // Arrange
        Result<int> result = default;

        // Act
        var nonGenericResult = result.AsFailure();

        // Assert
        using (new AssertionScope())
        {
            nonGenericResult.IsSuccess()
                .Should()
                .BeFalse();
            nonGenericResult.IsFailure()
                .Should()
                .BeTrue();
            nonGenericResult.Errors
                .Should()
                .HaveCount(1)
                .And
                .HaveElementAt(0, Error.Empty);
        }
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
        using (new AssertionScope())
        {
            genericResult.IsSuccess()
                .Should()
                .BeFalse();
            genericResult.IsFailure()
                .Should()
                .BeTrue();
            genericResult.Errors
                .Should()
                .HaveCount(2)
                .And
                .BeEquivalentTo(errors);
        }
    }

    [Fact]
    public void AsFailure_ShouldConvertDefaultResultToGenericResult()
    {
        // Arrange
        Result<int> result = default;

        // Act
        var genericResult = result.AsFailure<object>();

        // Assert
        using (new AssertionScope())
        {
            genericResult.IsSuccess()
                .Should()
                .BeFalse();
            genericResult.IsFailure()
                .Should()
                .BeTrue();
            genericResult.Errors
                .Should()
                .HaveCount(1)
                .And
                .HaveElementAt(0, Error.Empty);
        }
    }
    
    [Fact]
    public void ImplicitCast_ShouldCreateSuccessResultFromValue()
    {
        // Arrange
        const int value = 42;

        // Act
        Result<int> result = value;

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeTrue();
            result.IsSuccess(out var resultValue)
                .Should()
                .BeTrue();
            resultValue.Should()
                .Be(value);
            result.IsFailure()
                .Should()
                .BeFalse();
            result.IsFailure(out _)
                .Should()
                .BeFalse();
            result.Errors
                .Should()
                .BeEmpty();
        }
    }

    [Fact]
    public void ImplicitCast_ShouldCreateFailureResultFromError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        Result<int> result = error;

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out var resultError)
                .Should()
                .BeTrue();
            resultError.Should()
                .BeEquivalentTo(error);
            result.Errors
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo(error);
        }
    }

    [Fact]
    public void Equals_ResultInt_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(42);

        // Assert
        result1.Equals(result2)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Equals_ResultInt_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(43);

        // Assert
        result1.Equals(result2)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void Equals_ResultObject_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test");
        var result2 = Result.Success<object>("test");

        // Assert
        result1.Equals(result2)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Equals_ResultObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test1");
        var result2 = Result.Success<object>("test2");

        // Assert
        result1.Equals(result2)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void Equals_ResultIntToObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success<object>(42);

        // Assert
        result1.Equals(result2)
            .Should()
            .BeFalse();
    }

    [Fact]
    public void GetHashCode_ResultInt_ShouldReturnSameHashCodeForEqualResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(42);

        // Assert
        result1.GetHashCode()
            .Should()
            .Be(result2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ResultInt_ShouldReturnDifferentHashCodeForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(43);

        // Assert
        result1.GetHashCode()
            .Should()
            .NotBe(result2.GetHashCode());
    }

    [Fact]
    public void op_Equality_ResultInt_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(42);

        // Assert
        (result1 == result2).Should()
            .BeTrue();
    }

    [Fact]
    public void op_Equality_ResultInt_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(43);

        // Assert
        (result1 == result2).Should()
            .BeFalse();
    }

    [Fact]
    public void op_Inequality_ResultInt_ShouldReturnFalseForEqualResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(42);

        // Assert
        (result1 != result2).Should()
            .BeFalse();
    }

    [Fact]
    public void op_Inequality_ResultInt_ShouldReturnTrueForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success(43);

        // Assert
        (result1 != result2).Should()
            .BeTrue();
    }

    [Fact]
    public void op_Equality_ResultObject_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test");
        var result2 = Result.Success<object>("test");

        // Assert
        (result1 == result2).Should()
            .BeTrue();
    }

    [Fact]
    public void op_Equality_ResultObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test1");
        var result2 = Result.Success<object>("test2");

        // Assert
        (result1 == result2).Should()
            .BeFalse();
    }

    [Fact]
    public void op_Inequality_ResultObject_ShouldReturnFalseForEqualResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test");
        var result2 = Result.Success<object>("test");

        // Assert
        (result1 != result2).Should()
            .BeFalse();
    }

    [Fact]
    public void op_Inequality_ResultObject_ShouldReturnTrueForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success<object>("test1");
        var result2 = Result.Success<object>("test2");

        // Assert
        (result1 != result2).Should()
            .BeTrue();
    }

    [Fact]
    public void op_Equality_ResultIntToObject_ShouldReturnFalseForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success<object>(42);

        // Assert
        (result1 == result2).Should()
            .BeFalse();
    }

    [Fact]
    public void op_Inequality_ResultIntToObject_ShouldReturnTrueForDifferentResults()
    {
        // Arrange
        var result1 = Result.Success(42);
        var result2 = Result.Success<object>(42);

        // Assert
        (result1 != result2).Should()
            .BeTrue();
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = True", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForBoolean(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(true) : Result.Failure<bool>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForSByte(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<sbyte>(1) : Result.Failure<sbyte>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForByte(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<byte>(1) : Result.Failure<byte>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForInt16(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<short>(1) : Result.Failure<short>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForUInt16(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<ushort>(1) : Result.Failure<ushort>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForInt32(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(1) : Result.Failure<int>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForUInt32(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<uint>(1) : Result.Failure<uint>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForInt64(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<long>(1) : Result.Failure<long>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForUInt64(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<ulong>(1) : Result.Failure<ulong>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1.1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForDecimal(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(1.1m) : Result.Failure<decimal>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1.1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForFloat(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(1.1f) : Result.Failure<float>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1.1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForDouble(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(1.1d) : Result.Failure<double>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"2024-04-05T12:30:00Z\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForDateTime(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(new DateTime(2024, 04, 05, 12, 30, 00, DateTimeKind.Utc)) : Result.Failure<DateTime>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"2024-04-05T12:30:00+00:00\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForDateTimeOffset(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(new DateTimeOffset(2024, 04, 05, 12, 30, 00, TimeSpan.Zero)) : Result.Failure<DateTimeOffset>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 'c'", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForChar(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success('c') : Result.Failure<char>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"StringValue\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForString(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success("StringValue") : Result.Failure<string>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForObject(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(new object()) : Result.Failure<object>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForNullableValueTypes(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<int?>(1) : Result.Failure<int?>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    private class ValidationError(string errorMessage) : Error(errorMessage);

#if NET7_0_OR_GREATER
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
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeTrue();
            result.IsSuccess(out var resultValue)
                .Should()
                .BeTrue();
            resultValue.Should()
                .Be(value);
            result.IsFailure()
                .Should()
                .BeFalse();
            result.IsFailure(out var resultError)
                .Should()
                .BeFalse();
            resultError.Should()
                .Be(null);
            result.Errors
                .Should()
                .BeEmpty();
        }
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
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            result.Errors
                .Should()
                .ContainSingle()
                .Which
                .Message
                .Should()
                .Be("");
        }
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
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            result.Errors
                .Should()
                .ContainSingle()
                .Which
                .Message
                .Should()
                .Be("Sample error message");
        }
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
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            var error = result.Errors
                .Should()
                .ContainSingle()
                .Which;
            error.Message
                .Should()
                .Be("Sample error message");
            error.Metadata
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
    }

    [Fact]
    public void InterfaceFailure_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailureResultWithSingleError()
    {
        // Arrange
        static Result<TValue> Fail<TValue, TResult>()
            where TResult : IActionableResult<TValue, Result<TValue>>
        {
            const string errorMessage = "Sample error message";
            IReadOnlyDictionary<string, object> metadata = new Dictionary<string, object>
            {
                { "Key", 0 },
            };
            return TResult.Failure(errorMessage, metadata);
        }

        // Act
        var result = Fail<int, Result<int>>();

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            var error = result.Errors
                .Should()
                .ContainSingle()
                .Which;
            error.Message
                .Should()
                .Be("Sample error message");
            error.Metadata
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
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
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            result.Errors
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo(new Error("Sample error"));
        }
    }

    [Fact]
    public void InterfaceFailure_WithErrorsEnumerable_ShouldCreateFailureResultWithMultipleErrors()
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
            return TResult.Failure(errors.AsEnumerable());
        }

        // Act
        var result = Fail<int, Result<int>>();

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            result.Errors
                .Should()
                .HaveCount(2)
                .And
                .BeEquivalentTo(new List<IError>
                    {
                        new Error("Error 1"),
                        new Error("Error 2"),
                    }
                );
        }
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
        using (new AssertionScope())
        {
            result.IsSuccess()
                .Should()
                .BeFalse();
            result.IsSuccess(out _)
                .Should()
                .BeFalse();
            result.IsFailure()
                .Should()
                .BeTrue();
            result.IsFailure(out _)
                .Should()
                .BeTrue();
            result.Errors
                .Should()
                .HaveCount(2)
                .And
                .BeEquivalentTo(new List<IError>
                    {
                        new Error("Error 1"),
                        new Error("Error 2"),
                    }
                );
        }
    }
#endif

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"2024-04-05\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForDateOnly(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(new DateOnly(2024, 04, 05)) : Result.Failure<DateOnly>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = \"12:30:00\"", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForTimeOnly(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success(new TimeOnly(12, 30, 00)) : Result.Failure<TimeOnly>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }
#endif

#if NET7_0_OR_GREATER
    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForInt128(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<Int128>(1) : Result.Failure<Int128>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }

    [Theory]
    [InlineData(true, "IsSuccess = True, Value = 1", "")]
    [InlineData(false, "IsSuccess = False", "")]
    [InlineData(false, "IsSuccess = False, Error = \"An unknown error occured!\"", "An unknown error occured!")]
    public void ToString_ShouldReturnProperRepresentationForUInt128(bool success, string expected, string errorMessage)
    {
        // Arrange
        var result = success ? Result.Success<UInt128>(1) : Result.Failure<UInt128>(errorMessage);

        // Assert
        result.ToString()
            .Should()
            .Be($"Result {{ {expected} }}");
    }
#endif
}
