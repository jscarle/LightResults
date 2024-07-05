using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result<TValue>
{
    /// <inheritdoc/>
    public IReadOnlyList<IError> Errors
    {
        get
        {
            if (ErrorsInternal is not null)
                return ErrorsInternal;

            if (IsSuccessInternal)
                return Error.EmptyErrorList;

            return Error.DefaultErrorList;
        }
    }

    /// <inheritdoc/>
    public bool HasError<TError>()
        where TError : IError
    {
        if (IsSuccessInternal)
            return false;

        if (ErrorsInternal is not null)
            // Do not convert to LINQ, this creates unnecessary heap allocations.
            // For is the most efficient way to loop. It is the fastest and does not allocate.
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < ErrorsInternal.Count; index++)
            {
                if (ErrorsInternal[index] is TError)
                    return true;
            }

        return typeof(TError) == typeof(Error);
    }

    /// <inheritdoc/>
    public bool HasError<TError>([MaybeNullWhen(false)] out TError error)
        where TError : IError
    {
        if (IsSuccessInternal)
        {
            error = default;
            return false;
        }

        if (ErrorsInternal is not null)
            // Do not convert to LINQ, this creates unnecessary heap allocations.
            // For is the most efficient way to loop. It is the fastest and does not allocate.
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < ErrorsInternal.Count; index++)
            {
                if (ErrorsInternal[index] is not TError tError)
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
