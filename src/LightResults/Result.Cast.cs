using System.Diagnostics.CodeAnalysis;

namespace LightResults;

partial struct Result
{
    /// <summary>Implicitly converts an <see cref="Error"/> to a failure <see cref="Result{TValue}"/>.</summary>
    /// <param name="error">The error to convert into a failure result.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> representing a failure result with the specified error.</returns>
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates", Justification = "We don't want to expose named alternates in this case.")]
    public static implicit operator Result(Error error)
    {
        return new Result(error);
    }
}
