using LightResults.Common;

namespace LightResults;

/// <summary>Represents an error with a message and associated metadata.</summary>
public class Error : IError
{
    /// <summary>Gets an empty <see cref="Error"/> instance.</summary>
    public static IError Empty { get; } = new Error("", new Dictionary<string, object?>());

    /// <inheritdoc/>
    public string Message { get; init; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, object?> Metadata { get; init; }

    /// <summary>Gets the <see cref="Exception"/> associated with the error if one exists.</summary>
    /// <returns>
    /// An <see cref="Exception"/> instance when the metadata contains an entry named
    /// <c>"Exception"</c> with a value of type <see cref="Exception"/>; otherwise, <see langword="null"/>.
    /// </returns>
    public Exception? Exception
    {
        get
        {
            if (Metadata.TryGetValue("Exception", out var value) && value is Exception ex)
                return ex;

            return null;
        }
    }

    private const string ErrorTypeName = nameof(Error);
    private static readonly IReadOnlyDictionary<string, object?> EmptyMetaData = new Dictionary<string, object?>();
    internal static IReadOnlyList<IError> EmptyErrorList { get; } = [];
    internal static IReadOnlyList<IError> DefaultErrorList { get; } = [Empty];

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

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, (string Key, object? Value) metadata)
    {
        Message = message;
        Metadata = new Dictionary<string, object?>(1)
        {
            { metadata.Key, metadata.Value },
        };
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, KeyValuePair<string, object?> metadata)
    {
        Message = message;
        Metadata = new Dictionary<string, object?>(1)
        {
            { metadata.Key, metadata.Value },
        };
    }

#if NET6_0_OR_GREATER
    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, IEnumerable<KeyValuePair<string, object?>> metadata)
    {
        Message = message;
        Metadata = new Dictionary<string, object?>(metadata);
    }
#endif

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, IReadOnlyDictionary<string, object?> metadata)
    {
        Message = message;
        Metadata = metadata;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var type = GetType();
        var errorType = type == typeof(Error)
            ? ErrorTypeName
            : type.Name;

        if (Message.Length == 0)
            return errorType;

        return StringHelper.GetErrorString(errorType, Message);
    }
}
