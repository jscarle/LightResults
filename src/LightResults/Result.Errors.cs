using System.Diagnostics.CodeAnalysis;

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

        if (_errors.HasValue)
        {
            // Do not convert to LINQ, this creates unnecessary heap allocations.
            // For is the most efficient way to loop. It is the fastest and does not allocate.
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < _errors.Value.Length; index++)
            {
                if (_errors.Value[index] is TError)
                    return true;
            }
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

        if (_errors.HasValue)
        {
            // Do not convert to LINQ, this creates unnecessary heap allocations.
            // For is the most efficient way to loop. It is the fastest and does not allocate.
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < _errors.Value.Length; index++)
            {
                if (_errors.Value[index] is not TError)
                    continue;

                error = (TError)_errors.Value[index];
                return true;
            }
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
