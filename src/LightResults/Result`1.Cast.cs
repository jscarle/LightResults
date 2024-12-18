using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result<TValue>
{
    /// <summary>Implicitly converts a value to a success <see cref="Result{TValue}"/>.</summary>
    /// <param name="value">The value to convert into a success result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a success result with the specified value.</returns>
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = $"{nameof(Success)} is the named alternate.")]
    public static implicit operator Result<TValue>(TValue value)
    {
        return Success(value);
    }
}
