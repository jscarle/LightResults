namespace LightResults;

/// <summary>
/// Defines a result.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets a value indicating whether the result was successful.
    /// </summary>
    /// <returns><c>true</c> if the result was successful; otherwise, <c>false</c>.</returns>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result failed.
    /// </summary>
    /// <returns><c>true</c> if the result failed; otherwise, <c>false</c>.</returns>
    bool IsFailed { get; }

    /// <summary>
    /// Gets a list of errors associated with the result.
    /// </summary>
    /// <returns>An <see cref="IReadOnlyList{T}" /> of <see cref="IError" /> representing the errors.</returns>
    IReadOnlyList<IError> Errors { get; }

    /// <summary>
    /// Checks if the result contains an error of the specific type.
    /// </summary>
    /// <typeparam name="TError">The type of error to check for.</typeparam>
    /// <returns>
    /// <c>true</c> if an error of the specified type is present; otherwise, <c>false</c>.
    /// </returns>
    bool HasError<TError>()
        where TError : IError;
}
#if NET8_0_OR_GREATER
/// <summary>
/// Defines a result.
/// </summary>
public interface IResult<out TResult>
{
    /// <summary>
    /// Creates a success result.
    /// </summary>
    /// <returns></returns>
    static abstract TResult Ok();

    /// <summary>
    /// Creates a failed result with the given error message.
    /// </summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error message.</returns>
    static abstract TResult Fail(string errorMessage);

    /// <summary>
    /// Creates a failed result with the given error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified error.</returns>
    static abstract TResult Fail(IError error);

    /// <summary>
    /// Creates a failed result with the given errors.
    /// </summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failed result with the specified errors.</returns>
    static abstract TResult Fail(IEnumerable<IError> errors);
}
#endif