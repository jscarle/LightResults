using System.Text;

namespace LightResults.Common;

/// <summary>Base class for implementing the <see cref="IResult" /> interface.</summary>
public abstract class ResultBase : IResult
{
    /// <inheritdoc />
    public bool IsSuccess => _errors.Count == 0;

    /// <inheritdoc />
    public bool IsFailed => _errors.Count != 0;

    /// <inheritdoc />
    public IReadOnlyList<IError> Errors => _errors;

    private readonly ErrorArray _errors;

    /// <summary>Initializes a new instance of the <see cref="ResultBase" /> class.</summary>
    protected ResultBase()
    {
        _errors = ErrorArray.Empty;
    }

    /// <summary>Initializes a new instance of the <see cref="ResultBase" /> class with the specified error.</summary>
    protected ResultBase(IError error)
    {
        _errors = new ErrorArray(error);
    }

    /// <summary>Initializes a new instance of the <see cref="ResultBase" /> class with the specified errors.</summary>
    protected ResultBase(IEnumerable<IError> errors)
    {
        _errors = new ErrorArray(errors);
    }

    /// <inheritdoc />
    public bool HasError<TError>() where TError : IError
    {
        // Do not convert to LINQ, this creates unnecessary heap allocations.
        // For is the most efficient way to loop. It is the fastest and does not allocate.
        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < _errors.Count; index++)
        {
            var error = _errors[index];
            if (error is TError)
                return true;
        }

        return false;
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
