using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result<TValue>
{
    /// <summary>Implicitly converts a value to a success <see cref="Result{TValue}"/>.</summary>
    /// <param name="value">The value to convert into a success result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a success result with the specified value.</returns>
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = "We don't want to expose named alternates in this case.")]
    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value);
    }

    /// <summary>Implicitly converts an <see cref="Error"/> to a failure <see cref="Result{TValue}"/>.</summary>
    /// <param name="error">The error to convert into a failure result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failure result with the specified error.</returns>
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = "We don't want to expose named alternates in this case.")]
    public static implicit operator Result<TValue>(Error error)
    {
        return new Result<TValue>(error);
    }
}
