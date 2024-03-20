using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace LightResults.Tests;

public sealed class ResultExtensionsTests
{
    [Fact]
    public void Bind_WithSuccessfulSource_ShouldInvokeFunctionAndReturnNewResult()
    {
        // Arrange
        var sourceResult = Result.Ok(42);
        var bindFunctionCalled = false;

        // Act
        var result = sourceResult.Bind(value =>
        {
            bindFunctionCalled = true;
            return Result.Ok(value.ToString());
        });

        // Assert
        using (new AssertionScope())
        {
            bindFunctionCalled.Should().BeTrue();
            result.Should().BeOfType<Result<string>>();
            result.IsSuccess(out var resultValue).Should().BeTrue();
            resultValue.Should().Be("42");
        }
    }

    [Fact]
    public void Bind_WithFailedSource_ShouldNotInvokeFunctionAndReturnFailedResult()
    {
        // Arrange
        var sourceResult = Result.Fail<int>("Generic error");
        var bindFunctionCalled = false;

        // Act
        var result = sourceResult.Bind(value =>
        {
            bindFunctionCalled = true;
            return Result.Ok(value.ToString());
        });

        // Assert
        using (new AssertionScope())
        {
            bindFunctionCalled.Should().BeFalse();
            result.Should().BeOfType<Result<string>>();
            result.IsFailed(out var resultError).Should().BeTrue();
            resultError.Should().BeOfType<Error>().Which.Message.Should().Be("Generic error");
        }
    }

    [Fact]
    public async Task BindAsync_WithSuccessfulSource_ShouldInvokeFunctionAndReturnNewResult()
    {
        // Arrange
        var sourceResult = Result.Ok(42);
        var bindFunctionCalled = false;

        // Act
        var result = await sourceResult.BindAsync(async value =>
        {
            bindFunctionCalled = true;
            await Task.Delay(10); // Simulate some asynchronous operation
            return Result.Ok(value.ToString());
        });

        // Assert
        using (new AssertionScope())
        {
            bindFunctionCalled.Should().BeTrue();
            result.Should().BeOfType<Result<string>>();
            result.IsSuccess(out var resultValue).Should().BeTrue();
            resultValue.Should().Be("42");
        }
    }

    [Fact]
    public async Task BindAsync_WithFailedSource_ShouldNotInvokeFunctionAndReturnFailedResult()
    {
        // Arrange
        var sourceResult = Result.Fail<int>("Generic error");
        var bindFunctionCalled = false;

        // Act
        var result = await sourceResult.BindAsync(async value =>
        {
            bindFunctionCalled = true;
            await Task.Delay(10); // Simulate some asynchronous operation
            return Result.Ok(value.ToString());
        });

        // Assert
        using (new AssertionScope())
        {
            bindFunctionCalled.Should().BeFalse();
            result.Should().BeOfType<Result<string>>();
            result.IsFailed(out var resultError).Should().BeTrue();
            resultError.Should().BeOfType<Error>().Which.Message.Should().Be("Generic error");
        }
    }
}
