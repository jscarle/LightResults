using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result<TValue>
{
    /// <inheritdoc/>
    public IReadOnlyList<IError> Errors
    {
        get
        {
            if (_errors is not null)
                return _errors;

            if (_isSuccess)
                return Error.EmptyErrorList;

            return Error.DefaultErrorList;
        }
    }

    /// <inheritdoc/>
    public bool HasError<TError>()
        where TError : IError
    {
        if (_isSuccess)
            return false;

        if (_errors is not null)
            // Do not convert to LINQ, this creates unnecessary heap allocations.
            // For is the most efficient way to loop. It is the fastest and does not allocate.
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < _errors.Count; index++)
            {
                if (_errors[index] is TError)
                    return true;
            }

        return typeof(TError) == typeof(Error);
    }

    /// <inheritdoc/>
    public bool HasError<TError>([MaybeNullWhen(false)] out TError error)
        where TError : IError
    {
        if (_isSuccess)
        {
            error = default;
            return false;
        }

        if (_errors is not null)
            // Do not convert to LINQ, this creates unnecessary heap allocations.
            // For is the most efficient way to loop. It is the fastest and does not allocate.
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < _errors.Count; index++)
            {
                if (_errors[index] is not TError tError)
                    continue;

                error = tError;
                return true;
            }

        if (typeof(TError) == typeof(Error))
        {
            error = (TError)Error.Empty;
            return true;
        }

        error = default;
        return false;
    }
}
