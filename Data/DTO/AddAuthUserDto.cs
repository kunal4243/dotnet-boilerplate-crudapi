using System.ComponentModel.DataAnnotations;

namespace BoilerPlate.Data.DTO;

public class AddAuthUserDto
{
    [StringLength(20, MinimumLength = 3, ErrorMessage = "User name Length is Not valid")]
    public required string UserName { get; set; }

    public required string PasswordHash { get; set; }

    public string? RefreshToken { get; set; }

}
