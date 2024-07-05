using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result<TValue>
{
    /// <inheritdoc/>
    public bool IsFailed()
    {
        return !IsSuccessInternal;
    }

    /// <inheritdoc/>
    public bool IsFailed([MaybeNullWhen(false)] out IError error)
    {
        if (IsSuccessInternal)
            error = default;
        else if (ErrorsInternal is not null)
            error = ErrorsInternal[0];
        else
            error = Error.Empty;

        return !IsSuccessInternal;
    }

    /// <inheritdoc/>
    public bool IsFailed([MaybeNullWhen(false)] out IError error, [MaybeNullWhen(true)] out TValue value)
    {
        if (IsSuccessInternal)
        {
            value = ValueOrDefaultInternal;
            error = default;
        }
        else
        {
            value = default;
            if (ErrorsInternal is not null)
                error = ErrorsInternal[0];
            else
                error = Error.Empty;
        }

        return !IsSuccessInternal;
    }
}
