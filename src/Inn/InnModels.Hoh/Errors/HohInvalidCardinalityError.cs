namespace Ingweland.Fog.Inn.Models.Hoh.Errors;

public class HohInvalidCardinalityError : InnSdkError
{
    public HohInvalidCardinalityError(string message) : base(message)
    {
    }

    public HohInvalidCardinalityError(string message, Exception exception) : base(message, exception)
    {
    }
}
