namespace LightResults;

partial struct Result<TValue>
{
    /// <summary>Determines whether two <see cref="Result{TValue}"/> instances are equal.</summary>
    /// <param name="other">The <see cref="Result{TValue}"/> instance to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="Result{TValue}"/> is equal to this instance; otherwise, <c>false</c>.</returns>
    public bool Equals(Result<TValue> other)
    {
        return Nullable.Equals(ErrorsInternal, other.ErrorsInternal) && EqualityComparer<TValue?>.Default.Equals(ValueOrDefaultInternal, other.ValueOrDefaultInternal);
    }

    /// <summary>Determines whether the specified object is equal to this instance.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Result<TValue> other && Equals(other);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(ErrorsInternal, ValueOrDefaultInternal);
    }

    /// <summary>Determines whether two <see cref="Result{TValue}"/> instances are equal.</summary>
    /// <param name="left">The first <see cref="Result{TValue}"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Result{TValue}"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Result{TValue}"/> instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Result<TValue> left, Result<TValue> right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="Result{TValue}"/> instances are not equal.</summary>
    /// <param name="left">The first <see cref="Result{TValue}"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Result{TValue}"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Result{TValue}"/> instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Result<TValue> left, Result<TValue> right)
    {
        return !left.Equals(right);
    }
}
