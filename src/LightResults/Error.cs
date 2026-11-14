using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LightResults.Common;

namespace LightResults;

/// <summary>Represents an error with a message and associated metadata.</summary>
[SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed",
    Justification = "This class is not sealed to allow for inheritance."
)]
public class Error : IError, IEquatable<Error>
{
    private static readonly IReadOnlyDictionary<string, object?> EmptyMetaData = ReadOnlyDictionary<string, object?>.Empty;
    private readonly IReadOnlyDictionary<string, object?> _metadata;

    /// <summary>Gets an empty <see cref="Error"/> instance.</summary>
    public static IError Empty { get; } = new Error("", EmptyMetaData);

    internal static IReadOnlyList<IError> EmptyErrorList { get; } = ReadOnlyCollection<IError>.Empty;
    internal static IReadOnlyList<IError> DefaultErrorList { get; } = Array.AsReadOnly<IError>([Empty]);

    /// <inheritdoc/>
    public string Message { get; init; }

    /// <inheritdoc/>
    public Exception? Exception
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var metadata = Metadata;
            if (metadata.Count == 0)
                return null;

            if (metadata is SingleItemMetadataDictionary singleItemMetadata)
            {
                if (string.Equals(singleItemMetadata.Key, ExceptionKey, StringComparison.Ordinal) && singleItemMetadata.Value is Exception singleException)
                    return singleException;

                return null;
            }

            if (metadata.TryGetValue(ExceptionKey, out var value) && value is Exception ex)
                return ex;

            return null;
        }
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, object?> Metadata
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _metadata;
        init => _metadata = CreateMetadataSnapshot(value);
    }

    private const string ErrorTypeName = nameof(Error);
    private const string ExceptionKey = "Exception";

    /// <summary>Initializes a new instance of the <see cref="Error"/> class.</summary>
    public Error()
    {
        Message = "";
        _metadata = EmptyMetaData;
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message.</summary>
    /// <param name="message">The error message.</param>
    public Error(string message)
    {
        Message = message;
        _metadata = EmptyMetaData;
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified exception.</summary>
    /// <param name="exception">The <see cref="Exception"/> associated with the error.</param>
    /// <remarks>The <paramref name="exception"/> is added to <see cref="Metadata"/> under the key of "Exception" and the <see cref="Message"/> is set to the exception message if present.</remarks>
    public Error(Exception? exception)
    {
        if (exception is null)
        {
            Message = "";
            _metadata = EmptyMetaData;
            return;
        }

        var errorMessage = exception switch
        {
            _ when string.IsNullOrEmpty(exception.Message) => $"An exception of type {exception.GetType().Name} was thrown.",
            _ => $"{exception.GetType().Name}: {exception.Message}",
        };

        Message = errorMessage;
        _metadata = new SingleItemMetadataDictionary(ExceptionKey, exception);
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and exception.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The <see cref="Exception"/> associated with the error.</param>
    /// <remarks>The <paramref name="exception"/> is added to <see cref="Metadata"/> under the key of "Exception".</remarks>
    public Error(string message, Exception? exception)
    {
        if (exception is null)
        {
            Message = message;
            _metadata = EmptyMetaData;
            return;
        }

        Message = message;
        _metadata = new SingleItemMetadataDictionary(ExceptionKey, exception);
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, (string Key, object? Value) metadata)
    {
        Message = message;
        _metadata = new SingleItemMetadataDictionary(metadata.Key, metadata.Value);
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, KeyValuePair<string, object?> metadata)
    {
        Message = message;
        _metadata = new SingleItemMetadataDictionary(metadata.Key, metadata.Value);
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, IEnumerable<KeyValuePair<string, object?>> metadata)
    {
        Message = message;
        if (metadata is IReadOnlyDictionary<string, object?> readOnlyDictionary)
        {
            _metadata = CreateMetadataSnapshot(readOnlyDictionary);
            return;
        }

        if (metadata is KeyValuePair<string, object?>[] array)
        {
            _metadata = CreateMetadataSnapshot(array.AsSpan());
            return;
        }

        if (metadata is List<KeyValuePair<string, object?>> list)
        {
            _metadata = CreateMetadataSnapshot(CollectionsMarshal.AsSpan(list));
            return;
        }

        if (metadata is IReadOnlyList<KeyValuePair<string, object?>> readOnlyList)
        {
            var metadataCount = readOnlyList.Count;
            if (metadataCount == 0)
            {
                _metadata = EmptyMetaData;
                return;
            }

            if (metadataCount == 1)
            {
                var item = readOnlyList[0];
                _metadata = CreateMetadataSnapshot([item]);
                return;
            }

            var copiedMetadata = new Dictionary<string, object?>(metadataCount, StringComparer.Ordinal);
            for (var index = 0; index < metadataCount; index++)
            {
                var item = readOnlyList[index];
                copiedMetadata.Add(item.Key, item.Value);
            }

            _metadata = copiedMetadata.AsReadOnly();
            return;
        }

        var dictionary = new Dictionary<string, object?>(metadata, StringComparer.Ordinal);
        if (dictionary.Count == 0)
        {
            _metadata = EmptyMetaData;
            return;
        }

        if (dictionary.Count == 1)
        {
            using var enumerator = dictionary.GetEnumerator();
            enumerator.MoveNext();
            var singleMetadata = enumerator.Current;
            _metadata = new SingleItemMetadataDictionary(singleMetadata.Key, singleMetadata.Value);
            return;
        }

        _metadata = dictionary.AsReadOnly();
    }

    /// <summary>Initializes a new instance of the <see cref="Error"/> class with the specified error message and metadata.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata associated with the error.</param>
    public Error(string message, IReadOnlyDictionary<string, object?> metadata)
    {
        Message = message;
        _metadata = CreateMetadataSnapshot(metadata);
    }

    // Keep array calls unambiguous for C# 12 and C# 13 consumers.
    /// <summary>Initializes an error with the specified message and a snapshot of the metadata array.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata entries to snapshot.</param>
    /// <remarks>Referenced metadata values are not cloned.</remarks>
    public Error(string message, KeyValuePair<string, object?>[] metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        Message = message;
        _metadata = CreateMetadataSnapshot(metadata.AsSpan());
    }

    /// <summary>Initializes an error with the specified message and a snapshot of the metadata span.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">The metadata entries to snapshot.</param>
    /// <remarks>Referenced metadata values are not cloned.</remarks>
    public Error(string message, ReadOnlySpan<KeyValuePair<string, object?>> metadata)
    {
        Message = message;
        _metadata = CreateMetadataSnapshot(metadata);
    }

    [SuppressMessage("Major Code Smell", "S3236:Caller information arguments should not be provided explicitly",
        Justification = "Preserve the Dictionary constructor's parameter name when rejecting a null metadata key.")]
    private static IReadOnlyDictionary<string, object?> CreateMetadataSnapshot(ReadOnlySpan<KeyValuePair<string, object?>> metadata)
    {
        if (metadata.Length == 0)
            return EmptyMetaData;

        if (metadata.Length == 1)
        {
            var item = metadata[0];
            // ReSharper disable once NotResolvedInText
            ArgumentNullException.ThrowIfNull(item.Key, "key");
            return new SingleItemMetadataDictionary(item.Key, item.Value);
        }

        var dictionary = new Dictionary<string, object?>(metadata.Length, StringComparer.Ordinal);
        for (var index = 0; index < metadata.Length; index++)
        {
            var item = metadata[index];
            dictionary.Add(item.Key, item.Value);
        }

        return dictionary.AsReadOnly();
    }

    private static IReadOnlyDictionary<string, object?> CreateMetadataSnapshot(IReadOnlyDictionary<string, object?> metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        if (ReferenceEquals(metadata, EmptyMetaData) || metadata is SingleItemMetadataDictionary)
            return metadata;

        var metadataCount = metadata.Count;
        if (metadataCount == 0)
            return EmptyMetaData;

        if (metadataCount == 1)
        {
            if (metadata is Dictionary<string, object?> dictionary)
            {
                using var dictionaryEnumerator = dictionary.GetEnumerator();
                dictionaryEnumerator.MoveNext();
                var dictionaryMetadata = dictionaryEnumerator.Current;

                return new SingleItemMetadataDictionary(dictionaryMetadata.Key, dictionaryMetadata.Value);
            }

            using var enumerator = metadata.GetEnumerator();
            enumerator.MoveNext();
            var singleMetadata = enumerator.Current;

            return new SingleItemMetadataDictionary(singleMetadata.Key, singleMetadata.Value);
        }

        if (metadata is ImmutableDictionary<string, object?> immutableMetadata
            && (ReferenceEquals(immutableMetadata.KeyComparer, StringComparer.Ordinal)
                || ReferenceEquals(immutableMetadata.KeyComparer, EqualityComparer<string>.Default)))
            return immutableMetadata;

        if (metadata is FrozenDictionary<string, object?> frozenMetadata
            && (ReferenceEquals(frozenMetadata.Comparer, StringComparer.Ordinal)
                || ReferenceEquals(frozenMetadata.Comparer, EqualityComparer<string>.Default)))
            return frozenMetadata;

        var copiedMetadata = new Dictionary<string, object?>(metadata, StringComparer.Ordinal);
        return copiedMetadata.AsReadOnly();
    }

    /// <summary>Determines whether the specified <see cref="Error"/> is equal to this instance.</summary>
    /// <param name="other">The <see cref="Error"/> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="Error"/> is equal to this instance; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Error? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (!Message.Equals(other.Message, StringComparison.Ordinal))
            return false;

        var metadata = Metadata;
        var otherMetadata = other.Metadata;
        if (ReferenceEquals(metadata, otherMetadata))
            return true;

        var metadataCount = metadata.Count;
        if (metadataCount == 0 && otherMetadata.Count == 0)
            return true;

        if (metadataCount != otherMetadata.Count)
            return false;

        if (metadata is SingleItemMetadataDictionary singleItemMetadata)
            return singleItemMetadata.HasSameItem(otherMetadata);

        if (otherMetadata is SingleItemMetadataDictionary otherSingleItemMetadata)
            return otherSingleItemMetadata.HasSameItem(metadata);

        foreach (var kvp in metadata)
        {
            if (!otherMetadata.TryGetValue(kvp.Key, out var otherValue))
                return false;

            if (!Equals(kvp.Value, otherValue))
                return false;
        }

        return true;
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Error other && Equals(other);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Message, StringComparer.Ordinal);

        var metadata = Metadata;
        if (metadata is SingleItemMetadataDictionary singleItemMetadata)
        {
            hash.Add(singleItemMetadata.Key, StringComparer.Ordinal);
            hash.Add(singleItemMetadata.Value);
            return hash.ToHashCode();
        }

        var metadataCount = metadata.Count;
        if (metadataCount == 0)
            return hash.ToHashCode();

        if (metadataCount == 1)
        {
            foreach (var kvp in metadata)
            {
                hash.Add(kvp.Key, StringComparer.Ordinal);
                hash.Add(kvp.Value);
            }

            return hash.ToHashCode();
        }

        var combinedMetadataHashCode = 0;
        foreach (var kvp in metadata)
        {
            var keyHashCode = StringComparer.Ordinal.GetHashCode(kvp.Key);
            var metadataHashCode = HashCode.Combine(keyHashCode, kvp.Value);
            combinedMetadataHashCode = unchecked(combinedMetadataHashCode + metadataHashCode);
        }

        hash.Add(metadataCount);
        hash.Add(combinedMetadataHashCode);
        return hash.ToHashCode();
    }

    /// <summary>Determines whether two <see cref="Error"/> instances are equal.</summary>
    /// <param name="left">The first <see cref="Error"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Error"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Error"/> instances are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Error? left, Error? right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        var type = GetType();
        var errorType = type == typeof(Error) ? ErrorTypeName : type.Name;

        if (Message.Length == 0)
            return errorType;

        return StringHelper.GetErrorString(errorType, Message);
    }
}
