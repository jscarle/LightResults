using System;
using LightResults.Common;

namespace LightResults;

/// <summary>Represents an error with a message and associated metadata.</summary>
public class Error : IError, IEquatable<Error>
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
        Metadata = new SingleItemReadOnlyDictionary<string, object?>(metadata.Key, metadata.Value);
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, KeyValuePair<string, object?> metadata)
    {
        Message = message;
        Metadata = new SingleItemReadOnlyDictionary<string, object?>(metadata.Key, metadata.Value);
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

    /// <summary>Determines whether the specified <see cref="Error"/> is equal to this instance.</summary>
    /// <param name="other">The <see cref="Error"/> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="Error"/> is equal to this instance; otherwise, <c>false</c>.</returns>
    public bool Equals(Error? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (!Message.Equals(other.Message, StringComparison.Ordinal))
            return false;

        if (Metadata.Count != other.Metadata.Count)
            return false;

        foreach (var kvp in Metadata)
        {
            if (!other.Metadata.TryGetValue(kvp.Key, out var otherValue))
                return false;

            if (!Equals(kvp.Value, otherValue))
                return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Error other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Message, StringComparer.Ordinal);

        var metadataHash = 0;
        foreach (var kvp in Metadata)
        {
            metadataHash ^= HashCode.Combine(kvp.Key, kvp.Value);
        }

        hash.Add(metadataHash);
        return hash.ToHashCode();
    }

    /// <summary>Determines whether two <see cref="Error"/> instances are equal.</summary>
    /// <param name="left">The first <see cref="Error"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Error"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Error"/> instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Error? left, Error? right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="Error"/> instances are not equal.</summary>
    /// <param name="left">The first <see cref="Error"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Error"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Error"/> instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Error? left, Error? right)
    {
        return !(left == right);
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
