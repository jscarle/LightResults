using System.Globalization;
using System.Runtime.CompilerServices;

namespace LightResults.Common;

internal static class StringHelper
{
    internal const string ResultSuccessString = "Result { IsSuccess = True }";
    internal const string ResultFailureString = "Result { IsSuccess = False }";

    private const string PreResultStr = "Result { IsSuccess = ";
    private const string SuccessResultStr = "True";
    private const string FailureResultStr = "False";
    private const string PreValueStr = ", Value = ";
    private const string CharStr = "'";
    private const string StringStr = "\"";
    private const string PostResultStr = " }";
    private const string PreMessageStr = " { Message = \"";
    private const string PostMessageStr = "\" }";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetResultValueString<T>(T value)
    {
        switch (value)
        {
            case bool booleanValue:
                return GetResultValueValueString(booleanValue ? SuccessResultStr : FailureResultStr);
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
            case Int128 int128Value:
                return GetResultFormattableValueString(int128Value);
            case UInt128 uint128Value:
                return GetResultFormattableValueString(uint128Value);
            case decimal decimalValue:
                return GetResultFormattableValueString(decimalValue);
            case float floatValue:
                return GetResultFormattableValueString(floatValue);
            case double doubleValue:
                return GetResultFormattableValueString(doubleValue);
            case DateTime dateTimeValue:
                return GetResultFormattableValueString(dateTimeValue, "yyyy'-'MM'-'dd'T'HH':'mm':'ssK", true);
            case DateTimeOffset dateTimeOffsetValue:
                return GetResultFormattableValueString(dateTimeOffsetValue, "yyyy'-'MM'-'dd'T'HH':'mm':'ssK", true);
            case DateOnly dateOnlyValue:
                return GetResultFormattableValueString(dateOnlyValue, "yyyy'-'MM'-'dd", true);
            case TimeOnly timeOnlyValue:
                return GetResultFormattableValueString(timeOnlyValue, "HH':'mm':'ss", true);
            case char charValue:
                return GetResultCharValueString(charValue);
            case string stringValue:
                return GetResultStringValueString(stringValue);
            case IFormattable formattableValue:
                return GetResultValueString(formattableValue);
            default:
                return ResultSuccessString;
        }
    }

    private static string GetResultFormattableValueString<TValue>(TValue value, ReadOnlySpan<char> format = default, bool quoted = false)
        where TValue : ISpanFormattable
    {
        // Primitive numeric values and the fixed date/time formats above fit in 64 characters.
        Span<char> buffer = stackalloc char[64];
        if (!value.TryFormat(buffer, out var charsWritten, format, CultureInfo.InvariantCulture))
        {
            var valueString = value.ToString(format.IsEmpty ? null : new string(format), CultureInfo.InvariantCulture);
            return quoted ? GetResultStringValueString(valueString) : GetResultValueValueString(valueString);
        }

        var prefix = quoted ? "Result { IsSuccess = True, Value = \"" : "Result { IsSuccess = True, Value = ";
        var suffix = quoted ? "\" }" : PostResultStr;
        return string.Concat(prefix.AsSpan(), buffer[..charsWritten], suffix.AsSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetResultValueString(IFormattable value)
    {
        return GetResultValueValueString(value.ToString(null, CultureInfo.InvariantCulture));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetResultCharValueString(char value)
    {
        var stringLength = PreResultStrLength
                           + SuccessResultStrLength
                           + PreValueStrLength
                           + CharStrLength
                           + 1
                           + CharStrLength
                           + PostResultStrLength;
        return string.Create(stringLength, value, GetResultValueCharSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetResultStringValueString(string valueString)
    {
        var stringLength = PreResultStrLength
                           + SuccessResultStrLength
                           + PreValueStrLength
                           + StringStrLength
                           + valueString.Length
                           + StringStrLength
                           + PostResultStrLength;
        return string.Create(stringLength, valueString, GetResultValueStringSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetResultValueValueString(string valueString)
    {
        var stringLength = PreResultStrLength + SuccessResultStrLength + PreValueStrLength + valueString.Length + PostResultStrLength;
        return string.Create(stringLength, valueString, GetResultValueSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetResultErrorString(string errorMessage)
    {
        return string.Concat("Result { IsSuccess = False, Error = \"", errorMessage, "\" }");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetErrorString(string type, string message)
    {
        var stringLength = type.Length + PreMessageStrLength + message.Length + PostMessageStrLength;
        return string.Create(stringLength, (errorType: type, errorMessage: message), GetErrorSpan);
    }

    private const int PreResultStrLength = 21;
    private const int SuccessResultStrLength = 4;
    private const int PreValueStrLength = 10;
    private const int CharStrLength = 1;
    private const int StringStrLength = 1;
    private const int PostResultStrLength = 2;
    private const int PreMessageStrLength = 14;
    private const int PostMessageStrLength = 3;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void GetResultValueCharSpan(Span<char> span, char state)
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
        span[0] = state;
        span = span[1..];
        CharStr.AsSpan()
            .CopyTo(span);
        span = span[CharStrLength..];
        PostResultStr.AsSpan()
            .CopyTo(span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
}
