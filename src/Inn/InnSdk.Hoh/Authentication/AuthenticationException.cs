namespace Ingweland.Fog.InnSdk.Hoh.Authentication;

public class AuthenticationException : Exception
{
    public AuthenticationException(AuthErrorCode errorCode, Exception? innerException = null) : base(
        $"Error code: {errorCode}", innerException)
    {
        ErrorCode = errorCode;
    }

    public AuthenticationException(AuthErrorCode errorCode) : base($"Error code: {errorCode}")
    {
        ErrorCode = errorCode;
    }

    public AuthenticationException(AuthErrorCode errorCode, string message, Exception? innerException = null) : base(
        $"Error code: {errorCode}. Message: {message}", innerException)
    {
        ErrorCode = errorCode;
    }

    public AuthErrorCode ErrorCode { get; }
}
