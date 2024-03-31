using LightResults.Common;

namespace LightResults;

partial struct Result
{
    /// <inheritdoc/>
    public override string ToString()
    {
        if (_isSuccess)
            return $"{nameof(Result)} {{ IsSuccess = True }}";

        if (_errors.HasValue && _errors.Value[0].Message.Length > 0)
            return StringHelper.GetResultErrorString(_errors.Value[0].Message);

        return $"{nameof(Result)} {{ IsSuccess = False }}";
    }
}
