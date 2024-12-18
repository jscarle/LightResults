namespace LightResults;

/// <summary>Defines an error with a message and associated metadata.</summary>
public interface IError
{
    /// <summary>Gets the error message.</summary>
    /// <returns>A <see cref="string"/> representing the error message.</returns>
    string Message { get; }

    /// <summary>Gets the metadata associated with the error.</summary>
    /// <returns>An <see cref="IReadOnlyDictionary{TKey, TValue}"/> containing the metadata.</returns>
    /// <remarks>
    /// The metadata is represented as a read-only dictionary of key-value pairs, where the keys are <see cref="string"/> and the values are
    /// <see cref="object"/>.
    /// </remarks>
    IReadOnlyDictionary<string, object> Metadata { get; }
}
