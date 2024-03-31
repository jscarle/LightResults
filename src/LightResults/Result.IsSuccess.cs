namespace LightResults;

partial struct Result
{
    /// <inheritdoc/>
    public bool IsSuccess()
    {
        return _isSuccess;
    }
}
