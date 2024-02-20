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
        var typeName = GetType().Name;
        if (IsSuccess)
            return GetResultString(typeName, "True", "");

        var errorString = GetErrorString();
        return GetResultString(typeName, "False", errorString);
    }

    internal static string GetResultString(string typeName, string successString, string informationString)
    {
        const string preResultStr = " { IsSuccess = ";
        const string postResultStr = " }";
#if NET6_0_OR_GREATER
        var stringLength = typeName.Length + preResultStr.Length + successString.Length + informationString.Length + postResultStr.Length;

        var str = string.Create(stringLength, (typeName, successString, informationString), (span, state) => { span.TryWrite($"{state.typeName}{preResultStr}{state.successString}{state.informationString}{postResultStr}", out _); });

        return str;
#else
        return $"{typeName}{preResultStr}{successString}{informationString}{postResultStr}";
#endif
    }

    internal string GetErrorString()
    {
        if (IsSuccess || Errors[0].Message.Length <= 0)
            return "";

        var errorMessage = Errors[0].Message;

        const string preErrorStr = ", Error = \"";
        const string postErrorStr = "\"";
#if NET6_0_OR_GREATER
        var stringLength = preErrorStr.Length + errorMessage.Length + postErrorStr.Length;

        var str = string.Create(stringLength, errorMessage, (span, state) => { span.TryWrite($"{preErrorStr}{state}{postErrorStr}", out _); });

        return str;
#else
        return $"{preErrorStr}{errorMessage}{postErrorStr}";
#endif
    }
}
