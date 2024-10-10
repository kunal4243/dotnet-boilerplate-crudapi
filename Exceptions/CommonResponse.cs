using System.ComponentModel;

namespace BoilerPlate.Exceptions;

public enum ErrorCode
{
    [Description("Success")]
    Success = 0,

    [Description("User not found.")]
    UserNotFound = 1,

    [Description("Input validation failed.")]
    ValidationFailed = 2,

    [Description("User authorization failed.")]
    AuthorizationFailed = 3,

    [Description("Something went wrong. Try again later")]
    InternalServerError = 4
}

public static class EnumExtensions
{
    public static string GetMessage(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();
        var attributes = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        if (attributes == null) return value.ToString();
        var attribute = (DescriptionAttribute)attributes;
        return attribute.Description;
    }
}

public class CommonResponse<T>
{
    public int ErrorCode { get; set; }
    public required string ErrorMessage { get; set; }
    public required T Data { get; set; }


    public static CommonResponse<T> CreateResponse(ErrorCode errorCode, T data = default)
    {
        return new CommonResponse<T>
        {
            ErrorCode = (int)errorCode,
            ErrorMessage = errorCode.GetMessage(),
            Data = data
        };
    }

}
