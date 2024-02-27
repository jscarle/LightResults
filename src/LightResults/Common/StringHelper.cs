using System.Collections.Immutable;

namespace LightResults.Common;

internal static class StringHelper
{
    public static string GetResultString(string typeName, string successString, string informationString)
    {
        const string preResultStr = " { IsSuccess = ";
        const string postResultStr = " }";
#if NET6_0_OR_GREATER
        var stringLength = typeName.Length + preResultStr.Length + successString.Length + informationString.Length + postResultStr.Length;

        var str = string.Create(stringLength, (typeName, successString, informationString), (span, state) => { span.TryWrite($"{state.typeName}{preResultStr}{state.successString}{state.informationString}{postResultStr}", out _); });

        return str;
#else
        return $"{typeName}{preResultStr}{successString}{informationString}{postResultStr}";
#endif
    }

    public static string GetResultValueString<T>(T value)
    {
        var valueString = value?.ToString() ?? "";

        const string preValueStr = ", Value = ";
        const string charStr = "'";
        const string stringStr = "\"";

        if (value is bool || value is sbyte || value is byte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong ||
#if NET7_0_OR_GREATER
            value is Int128 || value is UInt128 ||
#endif
            value is decimal || value is float || value is double)
        {
#if NET6_0_OR_GREATER
            var stringLength = preValueStr.Length + valueString.Length;

            var str = string.Create(stringLength, valueString, (span, state) => { span.TryWrite($"{preValueStr}{state}", out _); });

            return str;
#else
            return $"{preValueStr}{valueString}";
#endif
        }

        if (value is char)
        {
#if NET6_0_OR_GREATER
            var stringLength = preValueStr.Length + charStr.Length + valueString.Length + charStr.Length;

            var str = string.Create(stringLength, valueString, (span, state) => { span.TryWrite($"{preValueStr}{charStr}{state}{charStr}", out _); });

            return str;
#else
            return $"{preValueStr}{charStr}{valueString}{charStr}";
#endif
        }

        if (value is string)
        {
#if NET6_0_OR_GREATER
            var stringLength = preValueStr.Length + stringStr.Length + valueString.Length + stringStr.Length;

            var str = string.Create(stringLength, valueString, (span, state) => { span.TryWrite($"{preValueStr}{stringStr}{state}{stringStr}", out _); });

            return str;
#else
            return $"{preValueStr}{stringStr}{valueString}{stringStr}";
#endif
        }

        return "";
    }

    public static string GetResultErrorString(ImmutableArray<IError> errors)
    {
        if (errors[0].Message.Length <= 0)
            return "";

        var errorMessage = errors[0].Message;

        const string preErrorStr = ", Error = \"";
        const string postErrorStr = "\"";
#if NET6_0_OR_GREATER
        var stringLength = preErrorStr.Length + errorMessage.Length + postErrorStr.Length;

        var str = string.Create(stringLength, errorMessage, (span, state) => { span.TryWrite($"{preErrorStr}{state}{postErrorStr}", out _); });

        return str;
#else
        return $"{preErrorStr}{errorMessage}{postErrorStr}";
#endif
    }

    public static string GetErrorString(IError error)
    {
        var errorType = error.GetType().Name;

        if (error.Message.Length <= 0)
            return errorType;

        var errorMessage = error.Message;

        const string preErrorStr = " { Message = \"";
        const string postErrorStr = "\" }";
#if NET6_0_OR_GREATER
        var stringLength = errorType.Length + preErrorStr.Length + errorMessage.Length + postErrorStr.Length;

        var str = string.Create(stringLength, (errorType, errorMessage), (span, state) => { span.TryWrite($"{state.errorType}{preErrorStr}{state.errorMessage}{postErrorStr}", out _); });

        return str;
#else
        return $"{errorType}{preErrorStr}{errorMessage}{postErrorStr}";
#endif
    }
}