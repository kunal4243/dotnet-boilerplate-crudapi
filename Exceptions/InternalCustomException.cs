namespace BoilerPlate.Exceptions;

public class InternalCustomException(string message, int statusCode = 400) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}