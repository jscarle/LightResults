using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result<TValue>
{
    /// <inheritdoc/>
    public bool IsFailure()
    {
        return !_isSuccess;
    }

    /// <inheritdoc/>
    public bool IsFailure([MaybeNullWhen(false)] out IError error)
    {
        if (_isSuccess)
            error = default;
        else if (_errors is not null)
            error = _errors[0];
        else
            error = Error.Empty;

        return !_isSuccess;
    }

    /// <inheritdoc/>
    public bool IsFailure([MaybeNullWhen(false)] out IError error, [MaybeNullWhen(true)] out TValue value)
    {
        if (_isSuccess)
        {
            value = _valueOrDefault;
            error = default;
        }
        else
        {
            value = default;
            if (_errors is not null)
                error = _errors[0];
            else
                error = Error.Empty;
        }

        return !_isSuccess;
    }
}
