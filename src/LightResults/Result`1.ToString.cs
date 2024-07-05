using LightResults.Common;

namespace LightResults;

partial struct Result<TValue>
{
    /// <inheritdoc/>
    public override string ToString()
    {
        if (IsSuccessInternal)
            return StringHelper.GetResultValueString(ValueOrDefaultInternal);

        if (ErrorsInternal is not null && ErrorsInternal[0].Message.Length > 0)
            return StringHelper.GetResultErrorString(ErrorsInternal[0].Message);

        return $"{nameof(Result)} {{ IsSuccess = False }}";
    }
}
