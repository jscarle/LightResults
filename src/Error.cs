using System.Collections.Immutable;

namespace LightResults;

/// <summary>Represents an error with a message and associated metadata.</summary>
public class Error : IError
{
    /// <inheritdoc />
    public string Message { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object> Metadata => _metadata;

    private readonly ImmutableDictionary<string, object> _metadata;

    /// <summary>Initializes a new instance of the <see cref="Error" /> class.</summary>
    public Error() : this("")
    {
    }

    /// <summary>Initializes a new instance of the <see cref="Error" /> class with the specified error message.</summary>
    /// <param name="message">The error message.</param>
    public Error(string message)
    {
        Message = message;
        _metadata = ImmutableDictionary<string, object>.Empty;
    }

    /// <summary>Initializes a new instance of the <see cref="Error" /> class with the specified metadata.</summary>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error((string Key, object Value) metadata) : this("", metadata)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="Error" /> class with the specified metadata.</summary>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(IDictionary<string, object> metadata) : this("", metadata)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="Error" /> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, (string Key, object Value) metadata)
    {
        Message = message;
        var builder = ImmutableDictionary.CreateBuilder<string, object>();
        builder.Add(metadata.Key, metadata.Value);
        _metadata = builder.ToImmutable();
    }

    /// <summary>Initializes a new instance of the <see cref="Error" /> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, IDictionary<string, object> metadata)
    {
        Message = message;
        _metadata = metadata.ToImmutableDictionary();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if (Message.Length > 0)
            return Message;

        return base.ToString() ?? "";
    }
}