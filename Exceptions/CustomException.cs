using System.Text.Json;

namespace BoilerPlate.Exceptions;


public class CustomException(ErrorCode errorCode, string data = "null") : Exception
{
    public ErrorCode ErrorCode { get; } = errorCode;
    public CommonResponse<string> CustomMessage { get; } = CommonResponse<string>.CreateResponse(errorCode, data);
}


