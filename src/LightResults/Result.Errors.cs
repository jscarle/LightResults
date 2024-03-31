namespace LightResults;

partial struct Result
{
    /// <inheritdoc/>
    public IReadOnlyCollection<IError> Errors
    {
        get
        {
            if (_errors.HasValue)
                return _errors;

            if (_isSuccess)
                return Error.EmptyCollection;

            return Error.DefaultCollection;
        }
    }

    /// <inheritdoc/>
    public bool HasError<TError>()
        where TError : IError
    {
        if (_isSuccess)
            return false;

        if (_errors is null)
            return typeof(TError) == typeof(Error);

        // Do not convert to LINQ, this creates unnecessary heap allocations.
        // For is the most efficient way to loop. It is the fastest and does not allocate.
        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < _errors.Value.Length; index++)
        {
            var error = _errors.Value[index];
            if (error is TError)
                return true;
        }

        return false;
    }
}
