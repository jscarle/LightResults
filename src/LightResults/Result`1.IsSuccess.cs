using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result<TValue>
{
    /// <inheritdoc/>
    public bool IsSuccess()
    {
        return IsSuccessInternal;
    }

    /// <inheritdoc/>
    public bool IsSuccess([MaybeNullWhen(false)] out TValue value)
    {
        value = ValueOrDefaultInternal;
        return IsSuccessInternal;
    }

    /// <inheritdoc/>
    public bool IsSuccess([MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(true)] out IError error)
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

        return IsSuccessInternal;
    }
}
