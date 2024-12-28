using System.Diagnostics.CodeAnalysis;

namespace LightResults;

/// <summary>Defines a result.</summary>
public interface IResult
{
    /// <summary>Gets a read-only collection of errors associated with the result.</summary>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> of <see cref="IError"/> instances representing the errors.</returns>
    IReadOnlyCollection<IError> Errors { get; }

    /// <summary>Determines whether the result was successful.</summary>
    /// <returns><c>true</c> if the result was successful; otherwise, <c>false</c>.</returns>
    bool IsSuccess();

    /// <summary>Determines whether the result failure.</summary>
    /// <returns><c>true</c> if the result failure; otherwise, <c>false</c>.</returns>
    bool IsFailure();

    /// <summary>Determines whether the result failure.</summary>
    /// <param name="error">The error of the result.</param>
    /// <returns><c>true</c> if the result failure; otherwise, <c>false</c>.</returns>
    bool IsFailure([MaybeNullWhen(false)] out IError error);

    /// <summary>Checks if the result contains an error of the specified type.</summary>
    /// <typeparam name="TError">The type of error to check for.</typeparam>
    /// <returns><c>true</c> if an error of the specified type exists; otherwise, <c>false</c>.</returns>
    bool HasError<TError>()
        where TError : IError;

    /// <summary>Checks if the result contains an error of the specified type.</summary>
    /// <param name="error">The error of the specified type.</param>
    /// <typeparam name="TError">The type of error to check for.</typeparam>
    /// <returns><c>true</c> if an error of the specified type exists; otherwise, <c>false</c>.</returns>
    bool HasError<TError>([MaybeNullWhen(false)] out TError error)
        where TError : IError;
}
