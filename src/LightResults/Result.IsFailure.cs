using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result
{
    /// <inheritdoc/>
    public bool IsFailure()
    {
        return !_isSuccess;
    }

    /// <inheritdoc/>
    public bool IsFailure([MaybeNullWhen(false)] out IError error)
    {
        // ReSharper disable once PreferConcreteValueOverDefault
        // Easier to refactor if this changes.
        if (_isSuccess)
            error = default;
        else if (_errors is not null)
            error = _errors[0];
        else
            error = Error.Empty;

        return !_isSuccess;
    }
}
