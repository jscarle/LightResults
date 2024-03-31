using System.Globalization;

namespace LightResults.Common;

internal static class StringHelper
{
    private const string PreResultStr = "Result { IsSuccess = ";
    private const string SuccessResultStr = "True";
    private const string FailedResultStr = "False";
    private const string PreValueStr = ", Value = ";
    private const string CharStr = "'";
    private const string StringStr = "\"";
    private const string PreErrorStr = ", Error = \"";
    private const string PostErrorStr = "\"";
    private const string PostResultStr = " }";
    private const string PreMessageStr = " { Message = \"";
    private const string PostMessageStr = "\" }";

    public static string GetResultString(string successString, string informationString)
    {
#if NET6_0_OR_GREATER
        var stringLength = PreResultStrLength + successString.Length + informationString.Length + PostResultStrLength;
        return string.Create(stringLength, (successString, informationString), GetResultSpan);
#else
        return $"{PreResultStr}{successString}{informationString}{PostResultStr}";
#endif
    }

    public static string GetResultValueString<T>(T value)
    {
        switch (value)
        {
            case bool booleanValue:
                return GetResultValueString(booleanValue.ToString());
            case sbyte sbyteValue:
                return GetResultValueString(sbyteValue.ToString(CultureInfo.InvariantCulture));
            case byte byteValue:
                return GetResultValueString(byteValue.ToString(CultureInfo.InvariantCulture));
            case short shortValue:
                return GetResultValueString(shortValue.ToString(CultureInfo.InvariantCulture));
            case ushort uShortValue:
                return GetResultValueString(uShortValue.ToString(CultureInfo.InvariantCulture));
            case int intValue:
                return GetResultValueString(intValue.ToString(CultureInfo.InvariantCulture));
            case uint uintValue:
                return GetResultValueString(uintValue.ToString(CultureInfo.InvariantCulture));
            case long longValue:
                return GetResultValueString(longValue.ToString(CultureInfo.InvariantCulture));
            case ulong ulongValue:
                return GetResultValueString(ulongValue.ToString(CultureInfo.InvariantCulture));
#if NET7_0_OR_GREATER
            case Int128 int128Value:
                return GetResultValueString(int128Value.ToString(CultureInfo.InvariantCulture));
            case UInt128 uint128Value:
                return GetResultValueString(uint128Value.ToString(CultureInfo.InvariantCulture));
#endif
            case decimal decimalValue:
                return GetResultValueString(decimalValue.ToString(CultureInfo.InvariantCulture));
            case float floatValue:
                return GetResultValueString(floatValue.ToString(CultureInfo.InvariantCulture));
            case double doubleValue:
                return GetResultValueString(doubleValue.ToString(CultureInfo.InvariantCulture));
            case DateTime dateTimeValue:
                return GetResultValueString(dateTimeValue.ToString("u", CultureInfo.InvariantCulture));
            case DateTimeOffset dateTimeOffsetValue:
                return GetResultValueString(dateTimeOffsetValue.ToString("u", CultureInfo.InvariantCulture));
#if NET6_0_OR_GREATER
            case DateOnly dateOnlyValue:
                return GetResultValueString(dateOnlyValue.ToString("u", CultureInfo.InvariantCulture));
            case TimeOnly timeOnlyValue:
                return GetResultValueString(timeOnlyValue.ToString("u", CultureInfo.InvariantCulture));
#endif
            case char charString:
            {
                var valueString = charString.ToString();
#if NET6_0_OR_GREATER
                var stringLength = PreResultStrLength + SuccessResultStrLength + PreValueStrLength + CharStrLength + valueString.Length + CharStrLength +
                                   PostResultStrLength;
                return string.Create(stringLength, valueString, GetResultValueCharSpan);
#else
                return $"{PreResultStr}{SuccessResultStr}{PreValueStr}{CharStr}{valueString}{CharStr}{PostResultStr}";
#endif
            }
            case string valueString:
            {
#if NET6_0_OR_GREATER
                var stringLength = PreResultStrLength + SuccessResultStrLength + PreValueStrLength + StringStrLength + valueString.Length + StringStrLength +
                                   PostResultStrLength;
                return string.Create(stringLength, valueString, GetResultValueStringSpan);
#else
                return $"{PreResultStr}{SuccessResultStr}{PreValueStr}{StringStr}{valueString}{StringStr}{PostResultStr}";
#endif
            }
            default:
                return "Result { IsSuccess = True }";
        }
    }

    private static string GetResultValueString(string valueString)
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
        var stringLength = PreResultStrLength + FailedResultStrLength + PreErrorStrLength + errorMessage.Length + PostErrorStrLength + PostResultStrLength;
        return string.Create(stringLength, errorMessage, GetResultErrorSpan);
#else
        return $"{PreResultStr}{FailedResultStr}{PreErrorStr}{errorMessage}{PostErrorStr}{PostResultStr}";
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
    private const int FailedResultStrLength = 5;
    private const int PreValueStrLength = 10;
    private const int CharStrLength = 1;
    private const int StringStrLength = 1;
    private const int PreErrorStrLength = 11;
    private const int PostErrorStrLength = 1;
    private const int PostResultStrLength = 2;
    private const int PreMessageStrLength = 14;
    private const int PostMessageStrLength = 3;

    private static void GetResultSpan(Span<char> span, (string successString, string informationString) state)
    {
        PreResultStr.AsSpan().CopyTo(span);
        span = span.Slice(PreResultStrLength);
        state.successString.AsSpan().CopyTo(span);
        span = span.Slice(state.successString.Length);
        state.informationString.AsSpan().CopyTo(span);
        span = span.Slice(state.informationString.Length);
        PostResultStr.AsSpan().CopyTo(span);
    }

    private static void GetResultValueSpan(Span<char> span, string state)
    {
        PreResultStr.AsSpan().CopyTo(span);
        span = span.Slice(PreResultStrLength);
        SuccessResultStr.AsSpan().CopyTo(span);
        span = span.Slice(SuccessResultStrLength);
        PreValueStr.AsSpan().CopyTo(span);
        span = span.Slice(PreValueStrLength);
        state.AsSpan().CopyTo(span);
        span = span.Slice(state.Length);
        PostResultStr.AsSpan().CopyTo(span);
    }

    private static void GetResultValueCharSpan(Span<char> span, string state)
    {
        PreResultStr.AsSpan().CopyTo(span);
        span = span.Slice(PreResultStrLength);
        SuccessResultStr.AsSpan().CopyTo(span);
        span = span.Slice(SuccessResultStrLength);
        PreValueStr.AsSpan().CopyTo(span);
        span = span.Slice(PreValueStrLength);
        CharStr.AsSpan().CopyTo(span);
        span = span.Slice(CharStrLength);
        state.AsSpan().CopyTo(span);
        span = span.Slice(state.Length);
        CharStr.AsSpan().CopyTo(span);
        span = span.Slice(CharStrLength);
        PostResultStr.AsSpan().CopyTo(span);
    }

    private static void GetResultValueStringSpan(Span<char> span, string state)
    {
        PreResultStr.AsSpan().CopyTo(span);
        span = span.Slice(PreResultStrLength);
        SuccessResultStr.AsSpan().CopyTo(span);
        span = span.Slice(SuccessResultStrLength);
        PreValueStr.AsSpan().CopyTo(span);
        span = span.Slice(PreValueStrLength);
        StringStr.AsSpan().CopyTo(span);
        span = span.Slice(StringStrLength);
        state.AsSpan().CopyTo(span);
        span = span.Slice(state.Length);
        StringStr.AsSpan().CopyTo(span);
        span = span.Slice(StringStrLength);
        PostResultStr.AsSpan().CopyTo(span);
    }

    private static void GetResultErrorSpan(Span<char> span, string state)
    {
        PreResultStr.AsSpan().CopyTo(span);
        span = span.Slice(PreResultStrLength);
        FailedResultStr.AsSpan().CopyTo(span);
        span = span.Slice(FailedResultStrLength);
        PreErrorStr.AsSpan().CopyTo(span);
        span = span.Slice(PreErrorStrLength);
        state.AsSpan().CopyTo(span);
        span = span.Slice(state.Length);
        PostErrorStr.AsSpan().CopyTo(span);
        span = span.Slice(PostErrorStrLength);
        PostResultStr.AsSpan().CopyTo(span);
    }

    private static void GetErrorSpan(Span<char> span, (string errorType, string errorMessage) state)
    {
        state.errorType.AsSpan().CopyTo(span);
        span = span.Slice(state.errorType.Length);
        PreMessageStr.AsSpan().CopyTo(span);
        span = span.Slice(PreMessageStrLength);
        state.errorMessage.AsSpan().CopyTo(span);
        span = span.Slice(state.errorMessage.Length);
        PostMessageStr.AsSpan().CopyTo(span);
    }
#endif
}
