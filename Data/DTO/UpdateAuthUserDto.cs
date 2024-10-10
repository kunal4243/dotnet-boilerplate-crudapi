namespace BoilerPlate.Data.DTO;

public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Terminated = 3
}

public class UpdateAuthUserDto
{
    public int UserId { get; set; }

    public string? PasswordHash { get; set; }

    public string? RefreshToken { get; set; }

    public UserStatus? UserStatus { get; set; }
}
