using System.Diagnostics.CodeAnalysis;

namespace LightResults;

/// <summary>Defines a result with a value.</summary>
/// <typeparam name="TValue">The type of value.</typeparam>
public interface IResult<TValue> : IResult
{
    /// <summary>Gets the value of the result, throwing an exception if the result is failed.</summary>
    /// <returns>The value of the result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when attempting to get or set the value of a failed result.</exception>
    TValue Value { get; }

    /// <summary>Gets whether the result was successful or not.</summary>
    /// <param name="value">The value of the result.</param>
    /// <returns><c>true</c> if the result was successful; otherwise, <c>false</c>.</returns>
    bool IsSuccess([MaybeNullWhen(false)] out TValue value);

    /// <summary>Gets whether the result was successful or not.</summary>
    /// <param name="value">The value of the result.</param>
    /// <param name="error">The error of the result.</param>
    /// <returns><c>true</c> if the result was successful; otherwise, <c>false</c>.</returns>
    bool IsSuccess([MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(true)] out IError error);

    /// <summary>Gets whether the result failed or not.</summary>
    /// <param name="error">The error of the result.</param>
    /// <param name="value">The value of the result.</param>
    /// <returns><c>true</c> if the result failed; otherwise, <c>false</c>.</returns>
    bool IsFailed([MaybeNullWhen(false)] out IError error, [MaybeNullWhen(true)] out TValue value);
}
