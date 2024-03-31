namespace LightResults;

/// <summary>Provides extension methods for working with <see cref="Result{TValue}" />.</summary>
public static class ResultExtensions
{
    /// <summary>Binds a transformation to the result, invoking the transformation if the result is successful.</summary>
    /// <typeparam name="TSource">The type of the value in the source result.</typeparam>
    /// <typeparam name="TDestination">The type of the value in the destination result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="transform">The transformation to invoke on the source result.</param>
    /// <returns>A new instance of <see cref="Result{TResult}" /> representing the result, or a failed result if the source result is failed.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the function is <see langword="null" />.</exception>
    public static Result<TDestination> Bind<TSource, TDestination>(this Result<TSource> source, Func<TSource, Result<TDestination>> transform)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(transform);
#else
        if (transform is null)
            throw new ArgumentNullException(nameof(transform));
#endif
        return source.IsFailed(out var error, out var value) ? Result.Fail<TDestination>(error) : transform(value);
    }

    /// <summary>Binds an asynchronous transformation to the result, invoking the transformation if the result is successful.</summary>
    /// <typeparam name="TSource">The type of the value in the source result.</typeparam>
    /// <typeparam name="TDestination">The type of the value in the destination result.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="transform">The transformation to invoke on the source result.</param>
    /// <returns>A new instance of <see cref="Result{TResult}" /> representing the result, or a failed result if the source result is failed.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the function is <see langword="null" />.</exception>
    public static async ValueTask<Result<TDestination>> BindAsync<TSource, TDestination>(this Result<TSource> source, Func<TSource, ValueTask<Result<TDestination>>> transform)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(transform);
#else
        if (transform is null)
            throw new ArgumentNullException(nameof(transform));
#endif
        return source.IsFailed(out var error, out var value) ? Result.Fail<TDestination>(error) : await transform(value).ConfigureAwait(false);
    }
}
