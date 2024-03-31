#if NET7_0_OR_GREATER
using LightResults.Common;
#endif

namespace LightResults;

partial struct Result<TValue>
{
    private static readonly Result<TValue> FailedResult = new(Error.Empty);

    internal static Result<TValue> Fail()
    {
        return FailedResult;
    }

    internal static Result<TValue> Fail(string errorMessage)
    {
        var error = new Error(errorMessage);
        return Fail(error);
    }

    internal static Result<TValue> Fail(string errorMessage, (string Key, object Value) metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Fail(error);
    }

    internal static Result<TValue> Fail(string errorMessage, IDictionary<string, object> metadata)
    {
        var error = new Error(errorMessage, metadata);
        return Fail(error);
    }

    internal static Result<TValue> Fail(IError error)
    {
        return new Result<TValue>(error);
    }

    internal static Result<TValue> Fail(IEnumerable<IError> errors)
    {
        return new Result<TValue>(errors);
    }

#if NET7_0_OR_GREATER
    /// <summary>Creates a failed result.</summary>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail()
    {
        return Fail();
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(string errorMessage)
    {
        return Fail(errorMessage);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(string errorMessage, (string Key, object Value) metadata)
    {
        return Fail(errorMessage, metadata);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(string errorMessage, IDictionary<string, object> metadata)
    {
        return Fail(errorMessage, metadata);
    }

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(IError error)
    {
        return Fail(error);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified errors.</returns>
    static Result<TValue> IActionableResult<TValue, Result<TValue>>.Fail(IEnumerable<IError> errors)
    {
        return Fail(errors);
    }
#endif
}
