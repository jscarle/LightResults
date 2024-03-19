#if NETSTANDARD2_0_OR_GREATER

// ReSharper disable once CheckNamespace
namespace System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(AttributeTargets.Constructor)]
internal sealed class SetsRequiredMembersAttribute : Attribute;

[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class MaybeNullWhenAttribute(bool returnValue) : Attribute
{
    public bool ReturnValue { get; } = returnValue;
}

#endif
