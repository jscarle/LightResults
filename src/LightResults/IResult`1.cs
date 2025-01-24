using System.Diagnostics.CodeAnalysis;

namespace LightResults;

/// <summary>Defines a result with a value.</summary>
/// <typeparam name="TValue">The type of value.</typeparam>
public interface IResult<TValue> : IResult
{
    /// <summary>Determines whether the result was successful.</summary>
    /// <param name="value">The value of the result.</param>
    /// <returns><c>true</c> if the result was successful; otherwise, <c>false</c>.</returns>
    bool IsSuccess([MaybeNullWhen(false)] out TValue value);

    /// <summary>Determines whether the result failure.</summary>
    /// <param name="value">The value of the result.</param>
    /// <param name="error">The error of the result.</param>
    /// <returns><c>true</c> if the result was successful; otherwise, <c>false</c>.</returns>
    bool IsSuccess([MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(true)] out IError error);

    /// <summary>Determines whether the result failure.</summary>
    /// <param name="error">The error of the result.</param>
    /// <param name="value">The value of the result.</param>
    /// <returns><c>true</c> if the result failure; otherwise, <c>false</c>.</returns>
    bool IsFailure([MaybeNullWhen(false)] out IError error, [MaybeNullWhen(true)] out TValue value);

    /// <summary>
    /// Evaluate the result in style of a discriminated union.
    /// </summary>
    /// <param name="onSuccess">Invoked when result is a success</param>
    /// <param name="onError">Invoked when result is a failure</param>
    void Switch(Action<TValue> onSuccess, Action<IError> onError);
    
    /// <summary>
    /// Evaluate the result in style of a discriminated union.
    /// </summary>
    /// <param name="onSuccess">Invoked when result is a success</param>
    /// <param name="onError">Invoked when result is a failure</param>
    /// <returns>A task for the async operation</returns>
    Task SwitchAsync(Func<TValue, Task> onSuccess, Func<IError, Task> onError);

    /// <summary>
    /// Evaluate the result in style of a discriminated union. A result must be returned
    /// </summary>
    /// <param name="onSuccess">Invoked when result is a success</param>
    /// <param name="onError">Invoked when result is a failure</param>
    /// <typeparam name="TReturn">Type to be returned</typeparam>
    /// <returns>The result of the operation</returns>
    TReturn Match<TReturn>(Func<TValue, TReturn> onSuccess,
	    Func<IError, TReturn> onError);

    /// <summary>
    /// Evaluate the result in style of a discriminated union. A result must be returned
    /// </summary>
    /// <param name="onSuccess">Invoked when result is a success</param>
    /// <param name="onError">Invoked when result is a failure</param>
    /// <typeparam name="TReturn">Type to be returned</typeparam>
    /// <returns>The result of the operation</returns>
    Task<TReturn> MatchAsync<TReturn>(Func<TValue, Task<TReturn>> onSuccess, Func<IError, Task<TReturn>> onError);
}
