using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Globalization;
using Shouldly;

namespace LightResults.Tests;

public sealed class PerformanceBehaviorTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Error_WithMetadataArray_ShouldSnapshotValues(int count)
    {
        // Arrange
        var metadata = new KeyValuePair<string, object?>[count];
        for (var index = 0; index < count; index++)
            metadata[index] = new($"Key{index}", $"Value{index}");

        // Act
        var error = new Error("Failure", metadata);
        Array.Clear(metadata);

        // Assert
        error.Metadata.Count.ShouldBe(count);
        for (var index = 0; index < count; index++)
            error.Metadata[$"Key{index}"].ShouldBe($"Value{index}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Error_WithMetadataList_ShouldSnapshotValues(int count)
    {
        // Arrange
        var metadata = new List<KeyValuePair<string, object?>>();
        for (var index = 0; index < count; index++)
            metadata.Add(new($"Key{index}", $"Value{index}"));

        // Act
        var error = new Error("Failure", metadata);
        metadata.Clear();

        // Assert
        error.Metadata.Count.ShouldBe(count);
        for (var index = 0; index < count; index++)
            error.Metadata[$"Key{index}"].ShouldBe($"Value{index}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Error_WithReadOnlyMetadataList_ShouldSnapshotValues(int count)
    {
        // Arrange
        var metadata = new KeyValuePair<string, object?>[count];
        for (var index = 0; index < count; index++)
            metadata[index] = new($"Key{index}", $"Value{index}");

        // Act
        var error = new Error("Failure", Array.AsReadOnly(metadata));
        Array.Clear(metadata);

        // Assert
        error.Metadata.Count.ShouldBe(count);
        for (var index = 0; index < count; index++)
            error.Metadata[$"Key{index}"].ShouldBe($"Value{index}");
    }

    [Fact]
    public void Error_WithDuplicateMetadataKeys_ShouldRejectDuplicates()
    {
        // Arrange
        KeyValuePair<string, object?>[] metadata = [new("Key", "first"), new("Key", "second")];

        // Act & Assert
        Should.Throw<ArgumentException>(() => new Error("Failure", metadata));
        Should.Throw<ArgumentException>(() => new Error("Failure", new List<KeyValuePair<string, object?>>(metadata)));
        Should.Throw<ArgumentException>(() => new Error("Failure", Array.AsReadOnly(metadata)));
    }

    [Fact]
    public void Error_WithNullMetadataKey_ShouldRejectKey()
    {
        // Arrange
        KeyValuePair<string, object?>[] metadata = [default];

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => new Error("Failure", metadata));
        Should.Throw<ArgumentNullException>(() => new Error("Failure", new List<KeyValuePair<string, object?>>(metadata)));
        Should.Throw<ArgumentNullException>(() => new Error("Failure", Array.AsReadOnly(metadata)));
    }

    [Fact]
    public void Error_WithImmutableMetadata_ShouldPreserveOrdinalLookup()
    {
        // Arrange
        var metadata = new Dictionary<string, object?> { ["Key"] = "value", ["Other"] = null };
        IReadOnlyDictionary<string, object?>[] sources =
        [
            metadata.ToImmutableDictionary(StringComparer.Ordinal),
            metadata.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase),
            metadata.ToFrozenDictionary(StringComparer.Ordinal),
            metadata.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase),
        ];

        foreach (var source in sources)
        {
            // Act
            var error = new Error("Failure", source);

            // Assert
            error.Metadata.Count.ShouldBe(2);
            error.Metadata["Key"].ShouldBe("value");
            error.Metadata["Other"].ShouldBeNull();
            error.Metadata.ContainsKey("key").ShouldBeFalse();
        }
    }

    [Fact]
    public void ToString_WithNumericBoundaryValues_ShouldPreserveInvariantRepresentation()
    {
        // Arrange
        object[] values =
        [
            sbyte.MinValue, byte.MaxValue, short.MinValue, ushort.MaxValue,
            int.MinValue, uint.MaxValue, long.MinValue, ulong.MaxValue,
            Int128.MinValue, UInt128.MaxValue, decimal.MinValue,
            float.Epsilon, float.NaN, float.NegativeInfinity, -0.0f,
            double.Epsilon, double.MaxValue, double.NaN, double.PositiveInfinity, -0.0d,
        ];

        foreach (var value in values)
        {
            // Act
            var text = Result.Success(value).ToString();

            // Assert
            var expectedValue = ((IFormattable)value).ToString(null, CultureInfo.InvariantCulture);
            text.ShouldBe($"Result {{ IsSuccess = True, Value = {expectedValue} }}");
        }
    }
}
