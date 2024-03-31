namespace LightResults;

partial struct Result
{
    /// <summary>Matches the result and executes an action based on whether the result is successful or failed.</summary>
    /// <param name="success">A action to execute if the result is successful.</param>
    /// <param name="failure">A action to execute if the result is failed.</param>
    public void Match(Action success, Action<IError> failure)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(success);
        ArgumentNullException.ThrowIfNull(failure);
#else
        if (success is null)
            throw new ArgumentNullException(nameof(success));
        if (failure is null)
            throw new ArgumentNullException(nameof(failure));
#endif
        if (_isSuccess)
            success();
        else if (_errors.HasValue)
            failure(_errors.Value[0]);
        else
            failure(Error.Empty);
    }

    /// <summary>Matches the result and executes a function based on whether the result is successful or failed.</summary>
    /// <typeparam name="TResult">The type of the value to return.</typeparam>
    /// <param name="success">A function to execute if the result is successful.</param>
    /// <param name="failure">A function to execute if the result is failed.</param>
    /// <returns>The value of the executed function.</returns>
    public TResult Match<TResult>(Func<TResult> success, Func<IError, TResult> failure)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(success);
        ArgumentNullException.ThrowIfNull(failure);
#else
        if (success is null)
            throw new ArgumentNullException(nameof(success));
        if (failure is null)
            throw new ArgumentNullException(nameof(failure));
#endif
        if (_isSuccess)
            return success();

        if (_errors.HasValue)
            return failure(_errors.Value[0]);

        return failure(Error.Empty);
    }
}
