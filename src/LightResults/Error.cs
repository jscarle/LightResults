using LightResults.Common;

namespace LightResults;

/// <summary>Represents an error with a message and associated metadata.</summary>
public class Error : IError
{
    /// <summary>Gets an empty error.</summary>
    public static IError Empty { get; } = new Error();

    /// <inheritdoc/>
    public string Message { get; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, object> Metadata { get; }

    internal static IReadOnlyList<IError> EmptyErrorList { get; } = [];
    internal static IReadOnlyList<IError> DefaultErrorList { get; } = [Empty];

    private static readonly IReadOnlyDictionary<string, object> EmptyMetaData = new Dictionary<string, object>();

    /// <summary>Initializes a new instance of the <see cref="Error"/> class.</summary>
    public Error()
        : this("")
    {
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message.</summary>
    /// <param name="message">The error message.</param>
    public Error(string message)
    {
        Message = message;
        Metadata = EmptyMetaData;
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified metadata.</summary>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error((string Key, object Value) metadata)
        : this("", metadata)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, (string Key, object Value) metadata)
    {
        Message = message;
        Metadata = new Dictionary<string, object>(1)
        {
            { metadata.Key, metadata.Value },
        };
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified metadata.</summary>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(IReadOnlyDictionary<string, object> metadata)
        : this("", metadata)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, IReadOnlyDictionary<string, object> metadata)
    {
        Message = message;
        Metadata = metadata;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var errorType = GetType()
            .Name;

        if (Message.Length == 0)
            return errorType;

        return StringHelper.GetErrorString(errorType, Message);
    }
}
