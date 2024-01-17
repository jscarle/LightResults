using System.Collections.Immutable;

namespace LightResults.Common;

/// <summary>
/// Base class for implementing the <see cref="IResult" /> interface.
/// </summary>
public abstract class ResultBase : IResult
{
    /// <inheritdoc />
    public bool IsSuccess => !IsFailed;

    /// <inheritdoc />
    public bool IsFailed => Errors.Any();

    /// <inheritdoc />
    public IReadOnlyList<IError> Errors => _errors;

    private readonly ImmutableList<IError> _errors;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultBase" /> class.
    /// </summary>
    protected ResultBase()
    {
        _errors = ImmutableList<IError>.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultBase" /> class with the specified error message.
    /// </summary>
    protected ResultBase(string errorMessage)
    {
        _errors = ImmutableList.Create<IError>(new Error(errorMessage));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultBase" /> class with the specified error.
    /// </summary>
    protected ResultBase(IError error)
    {
        _errors = ImmutableList.Create(error);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultBase" /> class with the specified errors.
    /// </summary>
    protected ResultBase(IEnumerable<IError> errors)
    {
        _errors = errors.ToImmutableList();
    }

    /// <summary>
    /// Checks if the result contains an error of the specific type.
    /// </summary>
    /// <typeparam name="TError">The type of error to check for.</typeparam>
    /// <returns>
    /// <c>true</c> if an error of the specified type is present; otherwise, <c>false</c>.
    /// </returns>
    public bool HasError<TError>()
        where TError : IError
    {
        return Errors.OfType<TError>()
            .Any();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{GetType().Name} {{ IsSuccess = {IsSuccess} }}";
    }
}
