namespace LightResults;

partial struct Result<TValue>
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

        if (_errors.HasValue)
            // Do not convert to LINQ, this creates unnecessary heap allocations.
            // For is the most efficient way to loop. It is the fastest and does not allocate.
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < _errors.Value.Length; index++)
            {
                if (_errors.Value[index] is TError)
                    return true;
            }

        return typeof(TError) == typeof(Error);
    }
}
