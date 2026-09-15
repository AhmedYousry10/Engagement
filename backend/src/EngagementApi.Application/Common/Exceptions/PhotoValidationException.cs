namespace EngagementApi.Application.Common.Exceptions;

public class PhotoValidationException : Exception
{
    public PhotoValidationException(string message) : base(message)
    {
    }
}
