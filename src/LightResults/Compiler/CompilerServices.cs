#if NETSTANDARD2_0_OR_GREATER
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace
namespace System.Runtime.CompilerServices;

[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
internal static class IsExternalInit;

[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
internal sealed class RequiredMemberAttribute : Attribute;

[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
internal sealed class CompilerFeatureRequiredAttribute(string featureName) : Attribute
{
    public const string RefStructs = nameof(RefStructs);
    public const string RequiredMembers = nameof(RequiredMembers);
    public string FeatureName { get; } = featureName;

    public bool IsOptional { get; init; }
}

#endif
