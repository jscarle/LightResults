namespace LightResults;

partial struct Result
{
    /// <summary>Creates a failed result.</summary>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result.</returns>
    public static Result<TValue> Failure<TValue>()
    {
        return Result<TValue>.FailureResult;
    }

    /// <summary>Creates a failed result with the given error message.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message.</returns>
    public static Result<TValue> Failure<TValue>(string errorMessage)
    {
        var error = new Error(errorMessage);
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message and metadata.</returns>
    public static Result<TValue> Failure<TValue>(string errorMessage, (string Key, object Value) metadata)
    {
        var error = new Error(errorMessage, metadata);
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given error message and metadata.</summary>
    /// <param name="errorMessage">The error message associated with the failure.</param>
    /// <param name="metadata">The metadata associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error message and metadata.</returns>
    public static Result<TValue> Failure<TValue>(string errorMessage, IReadOnlyDictionary<string, object> metadata)
    {
        var error = new Error(errorMessage, metadata);
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given error.</summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified error.</returns>
    public static Result<TValue> Failure<TValue>(IError error)
    {
        return new Result<TValue>(error);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified errors.</returns>
    public static Result<TValue> Failure<TValue>(IEnumerable<IError> errors)
    {
        return new Result<TValue>(errors);
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    /// <param name="errors">A collection of errors associated with the failure.</param>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failed result with the specified errors.</returns>
    public static Result<TValue> Failure<TValue>(IReadOnlyList<IError> errors)
    {
        return new Result<TValue>(errors);
    }
}
