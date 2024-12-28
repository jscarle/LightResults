using System.Globalization;

namespace LightResults.Common;

internal static class StringHelper
{
    private const string PreResultStr = "Result { IsSuccess = ";
    private const string SuccessResultStr = "True";
    private const string FailureResultStr = "False";
    private const string PreValueStr = ", Value = ";
    private const string CharStr = "'";
    private const string StringStr = "\"";
    private const string PreErrorStr = ", Error = \"";
    private const string PostErrorStr = "\"";
    private const string PostResultStr = " }";
    private const string PreMessageStr = " { Message = \"";
    private const string PostMessageStr = "\" }";
    
    public static string GetResultValueString<T>(T value)
    {
        switch (value)
        {
            case bool booleanValue:
                return GetResultValueValueString(booleanValue.ToString());
            case sbyte sbyteValue:
                return GetResultValueValueString(sbyteValue.ToString(CultureInfo.InvariantCulture));
            case byte byteValue:
                return GetResultValueValueString(byteValue.ToString(CultureInfo.InvariantCulture));
            case short shortValue:
                return GetResultValueValueString(shortValue.ToString(CultureInfo.InvariantCulture));
            case ushort uShortValue:
                return GetResultValueValueString(uShortValue.ToString(CultureInfo.InvariantCulture));
            case int intValue:
                return GetResultValueValueString(intValue.ToString(CultureInfo.InvariantCulture));
            case uint uintValue:
                return GetResultValueValueString(uintValue.ToString(CultureInfo.InvariantCulture));
            case long longValue:
                return GetResultValueValueString(longValue.ToString(CultureInfo.InvariantCulture));
            case ulong ulongValue:
                return GetResultValueValueString(ulongValue.ToString(CultureInfo.InvariantCulture));
#if NET7_0_OR_GREATER
            case Int128 int128Value:
                return GetResultValueValueString(int128Value.ToString(CultureInfo.InvariantCulture));
            case UInt128 uint128Value:
                return GetResultValueValueString(uint128Value.ToString(CultureInfo.InvariantCulture));
#endif
            case decimal decimalValue:
                return GetResultValueValueString(decimalValue.ToString(CultureInfo.InvariantCulture));
            case float floatValue:
                return GetResultValueValueString(floatValue.ToString(CultureInfo.InvariantCulture));
            case double doubleValue:
                return GetResultValueValueString(doubleValue.ToString(CultureInfo.InvariantCulture));
            case DateTime dateTimeValue:
                return GetResultStringValueString(dateTimeValue.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK", CultureInfo.InvariantCulture));
            case DateTimeOffset dateTimeOffsetValue:
                return GetResultStringValueString(dateTimeOffsetValue.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK", CultureInfo.InvariantCulture));
#if NET6_0_OR_GREATER
            case DateOnly dateOnlyValue:
                return GetResultStringValueString(dateOnlyValue.ToString("yyyy'-'MM'-'dd", CultureInfo.InvariantCulture));
            case TimeOnly timeOnlyValue:
                return GetResultStringValueString(timeOnlyValue.ToString("HH':'mm':'ss", CultureInfo.InvariantCulture));
#endif
            case char charValue:
                return GetResultCharValueString(charValue.ToString());
            case string stringValue:
                return GetResultStringValueString(stringValue);
            default:
                return "Result { IsSuccess = True }";
        }
    }

    private static string GetResultCharValueString(string valueString)
    {
#if NET6_0_OR_GREATER
        var stringLength = PreResultStrLength
                           + SuccessResultStrLength
                           + PreValueStrLength
                           + CharStrLength
                           + valueString.Length
                           + CharStrLength
                           + PostResultStrLength;
        return string.Create(stringLength, valueString, GetResultValueCharSpan);
#else
        return $"{PreResultStr}{SuccessResultStr}{PreValueStr}{CharStr}{valueString}{CharStr}{PostResultStr}";
#endif
    }

    private static string GetResultStringValueString(string valueString)
    {
#if NET6_0_OR_GREATER
        var stringLength = PreResultStrLength
                           + SuccessResultStrLength
                           + PreValueStrLength
                           + StringStrLength
                           + valueString.Length
                           + StringStrLength
                           + PostResultStrLength;
        return string.Create(stringLength, valueString, GetResultValueStringSpan);
#else
        return $"{PreResultStr}{SuccessResultStr}{PreValueStr}{StringStr}{valueString}{StringStr}{PostResultStr}";
#endif
    }

    private static string GetResultValueValueString(string valueString)
    {
#if NET6_0_OR_GREATER
        var stringLength = PreResultStrLength + SuccessResultStrLength + PreValueStrLength + valueString.Length + PostResultStrLength;
        return string.Create(stringLength, valueString, GetResultValueSpan);
#else
        return $"{PreResultStr}{SuccessResultStr}{PreValueStr}{valueString}{PostResultStr}";
#endif
    }

    public static string GetResultErrorString(string errorMessage)
    {
#if NET6_0_OR_GREATER
        var stringLength = PreResultStrLength + FailureResultStrLength + PreErrorStrLength + errorMessage.Length + PostErrorStrLength + PostResultStrLength;
        return string.Create(stringLength, errorMessage, GetResultErrorSpan);
#else
        return $"{PreResultStr}{FailureResultStr}{PreErrorStr}{errorMessage}{PostErrorStr}{PostResultStr}";
#endif
    }

    public static string GetErrorString(string type, string message)
    {
#if NET6_0_OR_GREATER
        var stringLength = type.Length + PreMessageStrLength + message.Length + PostMessageStrLength;
        return string.Create(stringLength, (errorType: type, errorMessage: message), GetErrorSpan);
#else
        return $"{type}{PreMessageStr}{message}{PostMessageStr}";
#endif
    }

#if NET6_0_OR_GREATER
    private const int PreResultStrLength = 21;
    private const int SuccessResultStrLength = 4;
    private const int FailureResultStrLength = 5;
    private const int PreValueStrLength = 10;
    private const int CharStrLength = 1;
    private const int StringStrLength = 1;
    private const int PreErrorStrLength = 11;
    private const int PostErrorStrLength = 1;
    private const int PostResultStrLength = 2;
    private const int PreMessageStrLength = 14;
    private const int PostMessageStrLength = 3;

    private static void GetResultValueSpan(Span<char> span, string state)
    {
        PreResultStr.AsSpan()
            .CopyTo(span);
        span = span[PreResultStrLength..];
        SuccessResultStr.AsSpan()
            .CopyTo(span);
        span = span[SuccessResultStrLength..];
        PreValueStr.AsSpan()
            .CopyTo(span);
        span = span[PreValueStrLength..];
        state
            .CopyTo(span);
        span = span[state.Length..];
        PostResultStr.AsSpan()
            .CopyTo(span);
    }

    private static void GetResultValueCharSpan(Span<char> span, string state)
    {
        PreResultStr.AsSpan()
            .CopyTo(span);
        span = span[PreResultStrLength..];
        SuccessResultStr.AsSpan()
            .CopyTo(span);
        span = span[SuccessResultStrLength..];
        PreValueStr.AsSpan()
            .CopyTo(span);
        span = span[PreValueStrLength..];
        CharStr.AsSpan()
            .CopyTo(span);
        span = span[CharStrLength..];
        state.AsSpan()
            .CopyTo(span);
        span = span[state.Length..];
        CharStr.AsSpan()
            .CopyTo(span);
        span = span[CharStrLength..];
        PostResultStr.AsSpan()
            .CopyTo(span);
    }

    private static void GetResultValueStringSpan(Span<char> span, string state)
    {
        PreResultStr.AsSpan()
            .CopyTo(span);
        span = span[PreResultStrLength..];
        SuccessResultStr.AsSpan()
            .CopyTo(span);
        span = span[SuccessResultStrLength..];
        PreValueStr.AsSpan()
            .CopyTo(span);
        span = span[PreValueStrLength..];
        StringStr.AsSpan()
            .CopyTo(span);
        span = span[StringStrLength..];
        state.AsSpan()
            .CopyTo(span);
        span = span[state.Length..];
        StringStr.AsSpan()
            .CopyTo(span);
        span = span[StringStrLength..];
        PostResultStr.AsSpan()
            .CopyTo(span);
    }

    private static void GetResultErrorSpan(Span<char> span, string state)
    {
        PreResultStr.AsSpan()
            .CopyTo(span);
        span = span[PreResultStrLength..];
        FailureResultStr.AsSpan()
            .CopyTo(span);
        span = span[FailureResultStrLength..];
        PreErrorStr.AsSpan()
            .CopyTo(span);
        span = span[PreErrorStrLength..];
        state.AsSpan()
            .CopyTo(span);
        span = span[state.Length..];
        PostErrorStr.AsSpan()
            .CopyTo(span);
        span = span[PostErrorStrLength..];
        PostResultStr.AsSpan()
            .CopyTo(span);
    }

    private static void GetErrorSpan(Span<char> span, (string errorType, string errorMessage) state)
    {
        state.errorType
            .AsSpan()
            .CopyTo(span);
        span = span[state.errorType.Length..];
        PreMessageStr.AsSpan()
            .CopyTo(span);
        span = span[PreMessageStrLength..];
        state.errorMessage
            .AsSpan()
            .CopyTo(span);
        span = span[state.errorMessage.Length..];
        PostMessageStr.AsSpan()
            .CopyTo(span);
    }
#endif
}
