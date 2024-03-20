using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace LightResults.Tests;

public sealed class ResultTests
{
    [Fact]
    public void DefaultStruct_ShouldBeFailedResult()
    {
        // Arrange
        Result result = default;

        // Act & Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out var resultError).Should().BeTrue();
            resultError.Should().Be(Error.Empty);
            result.Errors.Should().ContainSingle();
            result.Errors.First().Should().BeOfType<Error>();
            result.HasError<Error>().Should().BeTrue();
            result.HasError<ValidationError>().Should().BeFalse();
        }
    }

    [Fact]
    public void Error_WhenResultIsFailed_ShouldReturnFirstError()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError>
        {
            firstError,
            new Error("Error 2")
        };

        // Act
        var result = Result.Fail(errors);

        // Assert
        ((IResult)result).Error.Should().Be(firstError);
    }

    [Fact]
    public void Error_WhenResultIsSuccess_ShouldThrowException()
    {
        // Arrange
        var result = Result.Ok();

        // Act & Assert
        result.Invoking(r => _ = ((IResult)r).Error).Should().Throw<InvalidOperationException>().WithMessage("Result is successful. Error is not set.");
    }

    [Fact]
    public void IsSuccess_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Ok();

        // Act & Assert
        result.IsSuccess().Should().BeTrue();
    }

    [Fact]
    public void IsFailed_WhenResultIsFailed()
    {
        // Arrange
        var result = Result.Fail();

        // Act & Assert
        result.IsFailed().Should().BeTrue();
    }

    [Fact]
    public void IsFailed_WhenResultIsFailed_ShouldReturnFirstError()
    {
        // Arrange
        var firstError = new Error("Error 1");
        var errors = new List<IError>
        {
            firstError,
            new Error("Error 2")
        };
        var result = Result.Fail(errors);

        // Act
        var isFailed = result.IsFailed(out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isFailed.Should().BeTrue();
            resultError.Should().Be(firstError);
        }
    }

    [Fact]
    public void IsFailed_WhenResultIsSuccess_ShouldReturnDefaultValue()
    {
        // Arrange
        var result = Result.Ok();

        // Act
        var isFailed = result.IsFailed(out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isFailed.Should().BeFalse();
            resultError.Should().Be(null);
        }
    }

    [Fact]
    public void IsFailed_WhenResultIsSuccess_ShouldReturnNullValue()
    {
        // Arrange
        var result = Result.Ok();

        // Act
        var isFailed = result.IsFailed(out var resultError);

        // Assert
        using (new AssertionScope())
        {
            isFailed.Should().BeFalse();
            resultError.Should().Be(null);
        }
    }

    [Fact]
    public void Ok_ShouldCreateSuccessResult()
    {
        // Act
        var result = Result.Ok();

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeTrue();
            result.IsFailed().Should().BeFalse();
            result.IsFailed(out var resultError).Should().BeFalse();
            resultError.Should().Be(null);
            result.Errors.Should().BeEmpty();
        }
    }

    [Fact]
    public void OkTValue_WithValue_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Result.Ok(value);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeTrue();
            result.IsSuccess(out var resultValue).Should().BeTrue();
            resultValue.Should().Be(value);
            result.IsFailed().Should().BeFalse();
            result.IsFailed(out var resultError).Should().BeFalse();
            resultError.Should().Be(null);
            result.Errors.Should().BeEmpty();
            ((IResult<int>)result).Value.Should().Be(value);
        }
    }

    [Fact]
    public void Fail_ShouldCreateFailedResultWithSingleError()
    {
        // Act
        var result = Result.Fail();

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be("");
        }
    }

    [Fact]
    public void FailTValue_ShouldCreateFailedResultWithSingleError()
    {
        // Act
        var result = Result.Fail<object>();

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be("");
        }
    }

    [Fact]
    public void Fail_WithErrorMessage_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result.Fail(errorMessage);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be(errorMessage);
        }
    }

    [Fact]
    public void FailTValue_WithErrorMessage_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";

        // Act
        var result = Result.Fail<object>(errorMessage);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be(errorMessage);
        }
    }

    [Fact]
    public void Fail_WithErrorMessageAndTupleMetadata_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        (string Key, object Value) metadata = ("Key", 0);

        // Act
        var result = Result.Fail(errorMessage, metadata);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            var error = result.Errors.Should().ContainSingle().Which;
            error.Message.Should().Be(errorMessage);
            error.Metadata.Should().ContainSingle().Which.Should().BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
    }

    [Fact]
    public void FailTValue_WithErrorMessageAndTupleMetadata_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        (string Key, object Value) metadata = ("Key", 0);

        // Act
        var result = Result.Fail<object>(errorMessage, metadata);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            var error = result.Errors.Should().ContainSingle().Which;
            error.Message.Should().Be(errorMessage);
            error.Metadata.Should().ContainSingle().Which.Should().BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
    }

    [Fact]
    public void Fail_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        IDictionary<string, object> metadata = new Dictionary<string, object> { { "Key", 0 } };

        // Act
        var result = Result.Fail(errorMessage, metadata);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            var error = result.Errors.Should().ContainSingle().Which;
            error.Message.Should().Be(errorMessage);
            error.Metadata.Should().ContainSingle().Which.Should().BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
    }

    [Fact]
    public void FailTValue_WithErrorMessageAndDictionaryMetadata_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        const string errorMessage = "Sample error message";
        IDictionary<string, object> metadata = new Dictionary<string, object> { { "Key", 0 } };

        // Act
        var result = Result.Fail<object>(errorMessage, metadata);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            var error = result.Errors.Should().ContainSingle().Which;
            error.Message.Should().Be(errorMessage);
            error.Metadata.Should().ContainSingle().Which.Should().BeEquivalentTo(new KeyValuePair<string, object>("Key", 0));
        }
    }

    [Fact]
    public void Fail_WithErrorObject_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Fail(error);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Should().BeEquivalentTo(error);
        }
    }

    [Fact]
    public void FailTValue_WithErrorObject_ShouldCreateFailedResultWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var result = Result.Fail<object>(error);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().ContainSingle().Which.Should().BeEquivalentTo(error);
        }
    }

    [Fact]
    public void Fail_WithErrorsEnumerable_ShouldCreateFailedResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2")
        };

        // Act
        var result = Result.Fail(errors);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().HaveCount(2).And.BeEquivalentTo(errors);
        }
    }

    [Fact]
    public void FailTValue_WithErrorsEnumerable_ShouldCreateFailedResultWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2")
        };

        // Act
        var result = Result.Fail<object>(errors);

        // Assert
        using (new AssertionScope())
        {
            result.IsSuccess().Should().BeFalse();
            result.IsSuccess(out _).Should().BeFalse();
            result.IsFailed().Should().BeTrue();
            result.IsFailed(out _).Should().BeTrue();
            result.Errors.Should().HaveCount(2).And.BeEquivalentTo(errors);
        }
    }

    [Fact]
    public void HasError_WithMatchingErrorType_ShouldReturnTrue()
    {
        // Arrange
        var result = Result.Fail(new ValidationError("Validation error"));

        // Act & Assert
        result.HasError<ValidationError>().Should().BeTrue();
    }

    [Fact]
    public void HasError_WithNonMatchingErrorType_ShouldReturnFalse()
    {
        // Arrange
        var result = Result.Fail(new Error("Generic error"));

        // Act & Assert
        result.HasError<ValidationError>().Should().BeFalse();
    }

    [Fact]
    public void HasError_WhenIsSuccess_ShouldReturnFalse()
    {
        // Arrange
        var result = Result.Ok();

        // Act & Assert
        result.HasError<ValidationError>().Should().BeFalse();
    }

    [Fact]
    public void Match_WithSuccessfulSource_ShouldInvokeSuccessAction()
    {
        // Arrange
        var sourceResult = Result.Ok();
        var successActionCalled = false;
        var failureActionCalled = false;
        IError actionError = null!;

        // Act
        sourceResult.Match(() =>
        {
            successActionCalled = true;
        }, error =>
        {
            failureActionCalled = true;
            actionError = error;
        });

        // Assert
        using (new AssertionScope())
        {
            successActionCalled.Should().BeTrue();
            failureActionCalled.Should().BeFalse();
            actionError.Should().Be(null);
        }
    }

    [Fact]
    public void Match_WithFailedSource_ShouldInvokeFailureAction()
    {
        // Arrange
        var sourceResult = Result.Fail();
        var successActionCalled = false;
        var actionValue = 0;
        var failureActionCalled = false;
        IError actionError = null!;

        // Act
        sourceResult.Match(() =>
        {
            successActionCalled = true;
        }, error =>
        {
            failureActionCalled = true;
            actionError = error;
        });

        // Assert
        using (new AssertionScope())
        {
            successActionCalled.Should().BeFalse();
            failureActionCalled.Should().BeTrue();
            actionValue.Should().Be(0);
            actionError.Should().Be(Error.Empty);
        }
    }

    [Fact]
    public void Match_WithSuccessfulSource_ShouldInvokeSuccessFunc()
    {
        // Arrange
        var sourceResult = Result.Ok();
        var successFuncCalled = false;
        var failureFuncCalled = false;
        IError funcError = null!;

        // Act
        var returnResult = sourceResult.Match(() =>
        {
            successFuncCalled = true;
            return $"Value";
        }, error =>
        {
            failureFuncCalled = true;
            funcError = error;
            return $"Error: {error.Message}";
        });

        // Assert
        using (new AssertionScope())
        {
            successFuncCalled.Should().BeTrue();
            failureFuncCalled.Should().BeFalse();
            funcError.Should().Be(null);
            returnResult.Should().Be("Value");
        }
    }

    [Fact]
    public void Match_WithFailedSource_ShouldInvokeFailureFunc()
    {
        // Arrange
        var sourceResult = Result.Fail("Generic error");
        var successFuncCalled = false;
        var funcValue = 0;
        var failureFuncCalled = false;
        IError funcError = null!;

        // Act
        var returnResult = sourceResult.Match(() =>
        {
            successFuncCalled = true;
            return $"Value";
        }, error =>
        {
            failureFuncCalled = true;
            funcError = error;
            return $"Error: {error.Message}";
        });

        // Assert
        using (new AssertionScope())
        {
            successFuncCalled.Should().BeFalse();
            failureFuncCalled.Should().BeTrue();
            funcValue.Should().Be(0);
            funcError.Should().BeOfType<Error>().Which.Message.Should().Be("Generic error");
            returnResult.Should().Be("Error: Generic error");
        }
    }

    [Fact]
    public void Equals_Result_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok();
        var result2 = Result.Ok();

        // Act & Assert
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void Equals_Result_ShouldReturnFalseForUnequalResults()
    {
        // Arrange
        var result1 = Result.Ok();
        var result2 = Result.Fail("Error");

        // Act & Assert
        result1.Equals(result2).Should().BeFalse();
    }

    [Fact]
    public void Equals_Object_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok();
        var result2 = Result.Ok();

        // Act & Assert
        result1.Equals((object)result2).Should().BeTrue();
    }

    [Fact]
    public void Equals_Object_ShouldReturnFalseForUnequalResults()
    {
        // Arrange
        var result1 = Result.Ok();
        var result2 = Result.Fail("Error");

        // Act & Assert
        result1.Equals((object)result2).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok();
        var result2 = Result.Ok();

        // Act & Assert
        result1.GetHashCode().Should().Be(result2.GetHashCode());
    }

    [Fact]
    public void op_Equality_Result_ShouldReturnTrueForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok();
        var result2 = Result.Ok();

        // Act & Assert
        (result1 == result2).Should().BeTrue();
    }

    [Fact]
    public void op_Equality_Result_ShouldReturnFalseForUnequalResults()
    {
        // Arrange
        var result1 = Result.Ok();
        var result2 = Result.Fail("Error");

        // Act & Assert
        (result1 == result2).Should().BeFalse();
    }

    [Fact]
    public void op_Inequality_Result_ShouldReturnFalseForEqualResults()
    {
        // Arrange
        var result1 = Result.Ok();
        var result2 = Result.Ok();

        // Act & Assert
        (result1 != result2).Should().BeFalse();
    }

    [Fact]
    public void op_Inequality_Result_ShouldReturnTrueForUnequalResults()
    {
        // Arrange
        var result1 = Result.Ok();
        var result2 = Result.Fail("Error");

        // Act & Assert
        (result1 != result2).Should().BeTrue();
    }

    [Fact]
    public void ToString_WhenSuccess_ShouldReturnStringRepresentation()
    {
        // Arrange
        var result = Result.Ok();

        // Act & Assert
        result.ToString().Should().Be("Result { IsSuccess = True }");
    }

    [Theory]
    [InlineData("")]
    [InlineData("An unknown error occured!")]
    public void ToString_WhenFailed_ShouldReturnStringRepresentation(string errorMessage)
    {
        // Arrange
        var result = Result.Fail(errorMessage);

        // Act & Assert
        result.ToString().Should().Be(errorMessage.Length > 0 ? $"Result {{ IsSuccess = False, Error = \"{errorMessage}\" }}" : "Result { IsSuccess = False }");
    }

    private class ValidationError(string errorMessage) : Error(errorMessage);
}
