using System;
using System.Globalization;
using System.Runtime.CompilerServices;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetResultValueString<T>(T value)
    {
        switch (value)
        {
            case bool booleanValue:
                return GetResultBooleanValueString(booleanValue);
            case sbyte sbyteValue:
                return GetResultSignedIntegerValueString(sbyteValue);
            case byte byteValue:
                return GetResultUnsignedIntegerValueString(byteValue);
            case short shortValue:
                return GetResultSignedIntegerValueString(shortValue);
            case ushort uShortValue:
                return GetResultUnsignedIntegerValueString(uShortValue);
            case int intValue:
                return GetResultSignedIntegerValueString(intValue);
            case uint uintValue:
                return GetResultUnsignedIntegerValueString(uintValue);
            case long longValue:
                return GetResultSignedIntegerValueString(longValue);
            case ulong ulongValue:
                return GetResultUnsignedIntegerValueString(ulongValue);
            case Int128 int128Value:
                return GetResultValueValueString(int128Value.ToString(CultureInfo.InvariantCulture));
            case UInt128 uint128Value:
                return GetResultValueValueString(uint128Value.ToString(CultureInfo.InvariantCulture));
            case decimal decimalValue:
                return GetResultDecimalValueString(decimalValue);
            case float floatValue:
                return GetResultValueValueString(floatValue.ToString(CultureInfo.InvariantCulture));
            case double doubleValue:
                return GetResultValueValueString(doubleValue.ToString(CultureInfo.InvariantCulture));
            case DateTime dateTimeValue:
                return GetResultStringValueString(dateTimeValue.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK", CultureInfo.InvariantCulture));
            case DateTimeOffset dateTimeOffsetValue:
                return GetResultStringValueString(dateTimeOffsetValue.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK", CultureInfo.InvariantCulture));
            case DateOnly dateOnlyValue:
                return GetResultStringValueString(dateOnlyValue.ToString("yyyy'-'MM'-'dd", CultureInfo.InvariantCulture));
            case TimeOnly timeOnlyValue:
                return GetResultStringValueString(timeOnlyValue.ToString("HH':'mm':'ss", CultureInfo.InvariantCulture));
            case char charValue:
                return GetResultCharValueString(charValue.ToString());
            case string stringValue:
                return GetResultStringValueString(stringValue);
            case IFormattable formattableValue:
                return GetResultValueString(formattableValue);
            default:
                return "Result { IsSuccess = True }";
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetResultValueString(IFormattable value)
    {
        return GetResultValueValueString(value.ToString(null, CultureInfo.InvariantCulture));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetResultCharValueString(string valueString)
    {
        var stringLength = PreResultStrLength
                           + SuccessResultStrLength
                           + PreValueStrLength
                           + CharStrLength
                           + valueString.Length
                           + CharStrLength
                           + PostResultStrLength;
        return string.Create(stringLength, valueString, GetResultValueCharSpan);
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
    private static string GetResultBooleanValueString(bool value)
    {
        var valueLength = value ? SuccessResultStrLength : FailureResultStrLength;
        var stringLength = ResultValuePrefixLength + valueLength + PostResultStrLength;
        return string.Create(stringLength, (value, valueLength), static (span, state) =>
        {
            span = WriteResultValuePrefix(span);
            var valueSpan = span[..state.valueLength];
            (state.value ? SuccessResultStr : FailureResultStr).AsSpan()
                .CopyTo(valueSpan);
            span = span[state.valueLength..];
            PostResultStr.AsSpan()
                .CopyTo(span);
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetResultSignedIntegerValueString(long value)
    {
        var valueLength = GetSignedIntegerLength(value);
        var stringLength = ResultValuePrefixLength + valueLength + PostResultStrLength;
        return string.Create(stringLength, (value, valueLength), static (span, state) =>
        {
            span = WriteResultValuePrefix(span);
            state.value.TryFormat(span[..state.valueLength], out _, default, CultureInfo.InvariantCulture);
            span = span[state.valueLength..];
            PostResultStr.AsSpan()
                .CopyTo(span);
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetResultUnsignedIntegerValueString(ulong value)
    {
        var valueLength = GetUnsignedIntegerLength(value);
        var stringLength = ResultValuePrefixLength + valueLength + PostResultStrLength;
        return string.Create(stringLength, (value, valueLength), static (span, state) =>
        {
            span = WriteResultValuePrefix(span);
            state.value.TryFormat(span[..state.valueLength], out _, default, CultureInfo.InvariantCulture);
            span = span[state.valueLength..];
            PostResultStr.AsSpan()
                .CopyTo(span);
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetResultDecimalValueString(decimal value)
    {
        var valueLength = GetDecimalFormattedLength(value);
        var stringLength = ResultValuePrefixLength + valueLength + PostResultStrLength;
        return string.Create(stringLength, (value, valueLength), static (span, state) =>
        {
            span = WriteResultValuePrefix(span);
            state.value.TryFormat(span[..state.valueLength], out _, default, CultureInfo.InvariantCulture);
            span = span[state.valueLength..];
            PostResultStr.AsSpan()
                .CopyTo(span);
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetResultErrorString(string errorMessage)
    {
        var stringLength = PreResultStrLength + FailureResultStrLength + PreErrorStrLength + errorMessage.Length + PostErrorStrLength + PostResultStrLength;
        return string.Create(stringLength, errorMessage, GetResultErrorSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetErrorString(string type, string message)
    {
        var stringLength = type.Length + PreMessageStrLength + message.Length + PostMessageStrLength;
        return string.Create(stringLength, (errorType: type, errorMessage: message), GetErrorSpan);
    }

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
    private const int ResultValuePrefixLength = PreResultStrLength + SuccessResultStrLength + PreValueStrLength;
    private const int DecimalStackBufferLength = 50;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Span<char> WriteResultValuePrefix(Span<char> span)
    {
        PreResultStr.AsSpan()
            .CopyTo(span);
        span = span[PreResultStrLength..];
        SuccessResultStr.AsSpan()
            .CopyTo(span);
        span = span[SuccessResultStrLength..];
        PreValueStr.AsSpan()
            .CopyTo(span);
        return span[PreValueStrLength..];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetSignedIntegerLength(long value)
    {
        if (value >= 0)
        {
            return GetUnsignedIntegerLength((ulong)value);
        }

        var positiveValue = (ulong)(-(value + 1)) + 1UL;
        return 1 + GetUnsignedIntegerLength(positiveValue);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetUnsignedIntegerLength(ulong value)
    {
        var length = 1;
        while (value >= 10)
        {
            value /= 10;
            length++;
        }

        return length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetDecimalFormattedLength(decimal value)
    {
        Span<char> buffer = stackalloc char[DecimalStackBufferLength];
        if (!value.TryFormat(buffer, out var charsWritten, default, CultureInfo.InvariantCulture))
        {
            throw new InvalidOperationException("Decimal formatting buffer too small.");
        }

        return charsWritten;
    }

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
