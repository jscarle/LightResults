using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result<TValue>
{
    /// <inheritdoc/>
    public bool IsFailed()
    {
        return !_isSuccess;
    }

    /// <inheritdoc/>
    public bool IsFailed([MaybeNullWhen(false)] out IError error)
    {
        if (_isSuccess)
            error = default;
        else if (_errors.HasValue)
            error = _errors.Value[0];
        else
            error = Error.Empty;

        return !_isSuccess;
    }

    /// <inheritdoc/>
    public bool IsFailed([MaybeNullWhen(false)] out IError error, [MaybeNullWhen(true)] out TValue value)
    {
        if (_isSuccess)
        {
            value = _valueOrDefault;
            error = default;
        }
        else
        {
            value = default;
            if (_errors.HasValue)
                error = _errors.Value[0];
            else
                error = Error.Empty;
        }

        return !_isSuccess;
    }
}
