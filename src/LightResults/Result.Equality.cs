namespace LightResults;

partial struct Result
{
    /// <summary>Determines whether two <see cref="Result"/> instances are equal.</summary>
    /// <param name="other">The <see cref="Result"/> instance to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="Result"/> is equal to this instance; otherwise, <c>false</c>.</returns>
    public bool Equals(Result other)
    {
        return Equals(_errors, other._errors);
    }

    /// <summary>Determines whether the specified object is equal to this instance.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Result other && Equals(other);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return _errors?.GetHashCode() ?? 0;
    }

    /// <summary>Determines whether two <see cref="Result"/> instances are equal.</summary>
    /// <param name="left">The first <see cref="Result"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Result"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Result"/> instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Result left, Result right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="Result"/> instances are not equal.</summary>
    /// <param name="left">The first <see cref="Result"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Result"/> instance to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Result"/> instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Result left, Result right)
    {
        return !left.Equals(right);
    }
}
