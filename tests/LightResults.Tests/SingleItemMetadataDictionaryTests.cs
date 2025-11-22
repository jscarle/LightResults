using System.Collections.Generic;
using Shouldly;

namespace LightResults.Tests;

public sealed class SingleItemMetadataDictionaryTests
{
    private const string StoredKey = "correlation-id";
    private const string StoredValue = "abc123";

    [Fact]
    public void CountAndLookup_ShouldReflectSingleStoredItem()
    {
        // Arrange
        var dictionary = CreateDictionary();

        // Act & Assert
        dictionary.Count.ShouldBe(1);

        dictionary.ContainsKey(StoredKey)
            .ShouldBeTrue();
        dictionary.ContainsKey("other")
            .ShouldBeFalse();

        dictionary.TryGetValue(StoredKey, out var storedValue)
            .ShouldBeTrue();
        storedValue.ShouldBe(StoredValue);

        dictionary.TryGetValue("missing", out var missingValue)
            .ShouldBeFalse();
        missingValue.ShouldBeNull();
    }

    [Fact]
    public void Indexer_ShouldReturnValueOrThrow()
    {
        // Arrange
        var dictionary = CreateDictionary();

        // Act & Assert
        dictionary[StoredKey]
            .ShouldBe(StoredValue);

        Should.Throw<KeyNotFoundException>(() => _ = dictionary["missing"]);
    }

    [Fact]
    public void Enumerators_ShouldYieldSingleItemOnce()
    {
        // Arrange
        var dictionary = CreateDictionary();

        // Act
        var keys = dictionary.Keys.ToList();
        var values = dictionary.Values.ToList();
        var items = dictionary.ToList();

        using var keyEnumerator = dictionary.Keys.GetEnumerator();
        using var valueEnumerator = dictionary.Values.GetEnumerator();
        var itemEnumerator = dictionary.GetEnumerator();

        // Assert
        keys.ShouldBe(new[] { StoredKey });
        values.ShouldBe(new[] { (object?)StoredValue });
        items.ShouldBe(new[] { new KeyValuePair<string, object?>(StoredKey, StoredValue) });

        keyEnumerator.MoveNext()
            .ShouldBeTrue();
        keyEnumerator.Current.ShouldBe(StoredKey);
        keyEnumerator.MoveNext()
            .ShouldBeFalse();

        valueEnumerator.MoveNext()
            .ShouldBeTrue();
        valueEnumerator.Current.ShouldBe(StoredValue);
        valueEnumerator.MoveNext()
            .ShouldBeFalse();

        itemEnumerator.MoveNext()
            .ShouldBeTrue();
        itemEnumerator.Current.Key.ShouldBe(StoredKey);
        itemEnumerator.Current.Value.ShouldBe(StoredValue);
        itemEnumerator.MoveNext()
            .ShouldBeFalse();
    }

    private static IReadOnlyDictionary<string, object?> CreateDictionary()
    {
        var type = typeof(Result).Assembly.GetType("LightResults.Common.SingleItemMetadataDictionary")!;
        return (IReadOnlyDictionary<string, object?>)Activator.CreateInstance(type, StoredKey, StoredValue)!;
    }
}
