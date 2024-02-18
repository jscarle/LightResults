﻿#if NET8_0_OR_GREATER
using System.Collections.Frozen;

#else
using System.Collections.Immutable;

#endif
namespace LightResults;

/// <summary>Represents an error with a message and associated metadata.</summary>
public class Error : IError
{
    internal static Error Empty { get; } = new();
    
    /// <inheritdoc />
    public string Message { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object> Metadata => _metadata;

#if NET8_0_OR_GREATER
    private readonly FrozenDictionary<string, object> _metadata;
#else
    private readonly ImmutableDictionary<string, object> _metadata;
#endif

    /// <summary>Initializes a new instance of the <see cref="Error" /> class.</summary>
    public Error() : this("")
    {
    }

    /// <summary>Initializes a new instance of the <see cref="Error" /> class with the specified error message.</summary>
    /// <param name="message">The error message.</param>
    public Error(string message)
    {
        Message = message;
#if NET8_0_OR_GREATER
        _metadata = FrozenDictionary<string, object>.Empty;
#else
        _metadata = ImmutableDictionary<string, object>.Empty;
#endif
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
#if NET8_0_OR_GREATER
        var dictionary = new Dictionary<string, object> { { metadata.Key, metadata.Value } };
        _metadata = dictionary.ToFrozenDictionary();
#else
        var builder = ImmutableDictionary.CreateBuilder<string, object>();
        builder.Add(metadata.Key, metadata.Value);
        _metadata = builder.ToImmutable();
#endif
    }

    /// <summary>Initializes a new instance of the <see cref="Error" /> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, IDictionary<string, object> metadata)
    {
        Message = message;
#if NET8_0_OR_GREATER
        _metadata = metadata.ToFrozenDictionary();
#else
        _metadata = metadata.ToImmutableDictionary();
#endif
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if (Message.Length > 0)
            return Message;

        return base.ToString() ?? "";
    }
}
