#if NET7_0_OR_GREATER
namespace LightResults.Common;

/// <summary>Defines an actionable result.</summary>
public interface IActionableResult<out TResult> : IResult
    where TResult : IResult
{
    /// <summary>Creates a success result.</summary>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a success result with the specified value.</returns>
    static abstract TResult Success();

    /// <summary>Creates a failure result.</summary>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result.</returns>
    static abstract TResult Failure();

    /// <summary>Creates a failure result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified error message.</returns>
    static abstract TResult Failure(string errorMessage);

    /// <summary>Creates a failure result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified error message and metadata.</returns>
    static abstract TResult Failure(string errorMessage, (string Key, object Value) metadata);

    /// <summary>Creates a failure result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified error message and metadata.</returns>
    static abstract TResult Failure(string errorMessage, KeyValuePair<string, object> metadata);

    /// <summary>Creates a failure result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified error message and metadata.</returns>
    static abstract TResult Failure(string errorMessage, IReadOnlyDictionary<string, object> metadata);

    /// <summary>Creates a failure result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified error.</returns>
    static abstract TResult Failure(IError error);

    /// <summary>Creates a failure result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified errors.</returns>
    static abstract TResult Failure(IEnumerable<IError> errors);

    /// <summary>Creates a failure result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified errors.</returns>
    static abstract TResult Failure(IReadOnlyList<IError> errors);
}

/// <summary>Defines an actionable result.</summary>
public interface IActionableResult<TValue, out TResult> : IResult<TValue>
    where TResult : IResult<TValue>
{
    /// <summary>Creates a success result with the given value.</summary>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a success result with the specified value.</returns>
    static abstract TResult Success(TValue value);

    /// <summary>Creates a failure result.</summary>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result.</returns>
    static abstract TResult Failure();

    /// <summary>Creates a failure result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified error message.</returns>
    static abstract TResult Failure(string errorMessage);

    /// <summary>Creates a failure result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified error message and metadata.</returns>
    static abstract TResult Failure(string errorMessage, (string Key, object Value) metadata);

    /// <summary>Creates a failure result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified error message and metadata.</returns>
    static abstract TResult Failure(string errorMessage, KeyValuePair<string, object> metadata);

    /// <summary>Creates a failure result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified error message and metadata.</returns>
    static abstract TResult Failure(string errorMessage, IReadOnlyDictionary<string, object> metadata);

    /// <summary>Creates a failure result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified error.</returns>
    static abstract TResult Failure(IError error);

    /// <summary>Creates a failure result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified errors.</returns>
    static abstract TResult Failure(IEnumerable<IError> errors);

    /// <summary>Creates a failure result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <typeparamref name="TResult"/> representing a failure result with the specified errors.</returns>
    static abstract TResult Failure(IReadOnlyList<IError> errors);
}
#endif
