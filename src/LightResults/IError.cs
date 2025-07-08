namespace LightResults;

/// <summary>Defines an error with a message and associated metadata.</summary>
public interface IError
{
    /// <summary>Gets the error message.</summary>
    /// <returns>A <see cref="string"/> representing the error message.</returns>
    string Message { get; }

    /// <summary>Gets the <see cref="Exception"/> associated with the error if one exists.</summary>
    /// <returns>An <see cref="Exception"/> instance when the metadata contains an entry named <c>"Exception"</c> with a value of type <see cref="Exception"/>; otherwise, <see langword="null"/>.</returns>
    Exception? Exception { get; }

    /// <summary>Gets the metadata associated with the error.</summary>
    /// <returns>An <see cref="IReadOnlyDictionary{TKey, TValue}"/> containing the metadata.</returns>
    /// <remarks>
    /// The metadata is represented as a read-only dictionary of key-value pairs, where the keys are <see cref="string"/> and the values are
    /// <see cref="object"/> which may be <see langword="null"/>.
    /// </remarks>
    IReadOnlyDictionary<string, object?> Metadata { get; }
}
