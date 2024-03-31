using LightResults.Common;

namespace LightResults;

partial struct Result<TValue>
{
    /// <inheritdoc/>
    public override string ToString()
    {
        if (_isSuccess)
            return StringHelper.GetResultValueString(_valueOrDefault);

        if (_errors.HasValue && _errors.Value[0].Message.Length > 0)
            return StringHelper.GetResultErrorString(_errors.Value[0].Message);

        return $"{nameof(Result)} {{ IsSuccess = False }}";
    }
}
