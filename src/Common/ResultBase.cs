using System.Collections.Immutable;
using System.Text;

namespace LightResults.Common;

/// <summary>Base class for implementing the <see cref="IResult" /> interface.</summary>
public abstract class ResultBase : IResult
{
    /// <inheritdoc />
    public bool IsSuccess => _errors.Length == 0;

    /// <inheritdoc />
    public bool IsFailed => _errors.Length != 0;

    /// <inheritdoc />
    public IReadOnlyList<IError> Errors => _errors;

    private readonly ImmutableArray<IError> _errors;

    /// <summary>Initializes a new instance of the <see cref="ResultBase" /> class.</summary>
    protected ResultBase()
    {
        _errors = ImmutableArray<IError>.Empty;
    }

    /// <summary>Initializes a new instance of the <see cref="ResultBase" /> class with the specified error.</summary>
    protected ResultBase(IError error)
    {
        _errors = ImmutableArray.Create(error);
    }

    /// <summary>Initializes a new instance of the <see cref="ResultBase" /> class with the specified errors.</summary>
    protected ResultBase(IEnumerable<IError> errors)
    {
        _errors = errors.ToImmutableArray();
    }

    /// <inheritdoc />
    public bool HasError<TError>() where TError : IError
    {
        return Errors.OfType<TError>().Any();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(GetType().Name);
        builder.Append(" { ");
        builder.Append("IsSuccess = ");
        builder.Append(IsSuccess);

        if (IsFailed && Errors[0].Message.Length > 0)
        {
            builder.Append(", Error = ");
            builder.Append('"');
            builder.Append(Errors[0].Message);
            builder.Append('"');
        }

        builder.Append(" }");
        return builder.ToString();
    }
}
