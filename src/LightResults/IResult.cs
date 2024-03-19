using System.Diagnostics.CodeAnalysis;

namespace LightResults;

/// <summary>Defines a result.</summary>
public interface IResult
{
    /// <summary>Gets whether the result was successful or not.</summary>
    /// <returns><c>true</c> if the result was successful; otherwise, <c>false</c>.</returns>
    bool IsSuccess { get; }

    /// <summary>Gets whether the result failed or not.</summary>
    /// <returns><c>true</c> if the result failed; otherwise, <c>false</c>.</returns>
    bool IsFailed { get; }

    /// <summary>Gets a collection of errors associated with the result.</summary>
    /// <returns>An <see cref="IReadOnlyCollection{T}" /> of <see cref="IError" /> representing the errors.</returns>
    IReadOnlyCollection<IError> Errors { get; }

    /// <summary>Gets the error of the result, throwing an exception if the result is successful.</summary>
    /// <returns>The error of the result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when attempting to get or set the value of a failed result.</exception>
    IError Error { get; }

    /// <summary>Checks if the result contains an error of the specific type.</summary>
    /// <typeparam name="TError">The type of error to check for.</typeparam>
    /// <returns><c>true</c> if an error of the specified type is present; otherwise, <c>false</c>.</returns>
    bool HasError<TError>() where TError : IError;
}

/// <summary>Defines a result with a value.</summary>
public interface IResult<TValue> : IResult
{
    /// <summary>Gets whether the result was successful or not.</summary>
    /// <param name="value">The value of the result.</param>
    /// <returns><c>true</c> if the result was successful; otherwise, <c>false</c>.</returns>
    new bool IsSuccess([MaybeNullWhen(false)] out TValue value);
}
