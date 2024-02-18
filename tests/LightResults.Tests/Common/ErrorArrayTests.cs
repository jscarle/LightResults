using System.Collections;
using FluentAssertions;
using LightResults.Common;
using Xunit;

namespace LightResults.Tests.Common;

public class ErrorArrayTests
{
    [Fact]
    public void ShouldImplementIReadOnlyListCorrectly()
    {
        // Arrange
        var error = new Error("Sample error");
        var errorArray = new ErrorArray(error);

        // Act & Assert
        errorArray.Count.Should().Be(1);
        errorArray[0].Should().BeEquivalentTo(error);
    }

    [Fact]
    public void Empty_ShouldReturnEmptyArray()
    {
        // Arrange & Act
        var errorArray = ErrorArray.Empty;

        // Assert
        errorArray.Count.Should().Be(0);
        errorArray.Should().BeEmpty();
    }

    [Fact]
    public void SingleErrorConstructor_ShouldCreateArrayWithSingleError()
    {
        // Arrange
        var error = new Error("Sample error");

        // Act
        var errorArray = new ErrorArray(error);

        // Assert
        errorArray.Count.Should().Be(1);
        errorArray[0].Should().BeEquivalentTo(error);
    }

    [Fact]
    public void EnumerableConstructor_ShouldCreateArrayFromAllErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2")
        };

        // Act
        var errorArray = new ErrorArray(errors);

        // Assert
        errorArray.Count.Should().Be(2);
        errorArray.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void Indexer_AccessNegativeIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var errorArray = new ErrorArray(new Error("Sample error"));

        // Act & Assert
        var act = () => errorArray[-1];
        act.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void Indexer_AccessIndexBeyondEnd_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var errorArray = new ErrorArray(new Error("Sample error"));

        // Act & Assert
        var act = () => errorArray[1];
        act.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void Indexer_AccessSingleElement_ReturnsCorrectError()
    {
        // Arrange
        var error = new Error("Sample error");
        var errorArray = new ErrorArray(error);

        // Act
        var retrievedError = errorArray[0];

        // Assert
        retrievedError.Should().BeEquivalentTo(error);
    }

    [Fact]
    public void Indexer_AccessElementInEnumerable_ReturnsCorrectError()
    {
        // Arrange
        var errors = new List<IError> { new Error("Error 1"), new Error("Error 2") };
        var errorArray = new ErrorArray(errors);

        // Act
        var retrievedError = errorArray[1];

        // Assert
        retrievedError.Should().BeEquivalentTo(errors[1]);
    }

    [Fact]
    public void GetEnumerator_ShouldEnumerateErrorsEfficiently()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Error 1"),
            new Error("Error 2")
        };
        var errorArray = new ErrorArray(errors);

        // Act & Assert
        var index = 0;
        foreach (var error in errorArray) error.Should().BeEquivalentTo(errors[index++]);
    }

    [Fact]
    public void GetEnumerator_ShouldNotModifyOriginalArray()
    {
        // Arrange
        var errors = new List<IError> { new Error("Error 1"), new Error("Error 2") };
        var originalErrors = errors.ToList(); // Create a copy to compare later

        // Act
        var errorArray = new ErrorArray(errors);
        using var enumerator = errorArray.GetEnumerator();

        // Assert
        while (enumerator.MoveNext())
        {
            // Do something with the error (e.g., log it)
        }

        // Ensure original errors haven't been modified
        errors.Should().BeEquivalentTo(originalErrors);
    }

    [Fact]
    public void GetEnumerator_ImplicitImplementation_ShouldEnumerateErrors()
    {
        // Arrange
        var errors = new List<IError> { new Error("Error 1"), new Error("Error 2") };
        var errorArray = new ErrorArray(errors);
        var enumerator = ((IEnumerable)errorArray).GetEnumerator();
        using var disposable = enumerator as IDisposable;

        // Assert
        var index = 0;
        while (enumerator.MoveNext()) enumerator.Current.Should().BeEquivalentTo(errors[index++]);
    }
}
