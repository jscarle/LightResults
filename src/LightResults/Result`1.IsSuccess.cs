using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result<TValue>
{
    /// <inheritdoc/>
    public bool IsSuccess()
    {
        return _isSuccess;
    }

    /// <inheritdoc/>
    public bool IsSuccess([MaybeNullWhen(false)] out TValue value)
    {
        value = _valueOrDefault;
        return _isSuccess;
    }

    /// <inheritdoc/>
    public bool IsSuccess([MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(true)] out IError error)
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

        return _isSuccess;
    }
}
