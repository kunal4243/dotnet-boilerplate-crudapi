using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoilerPlate.Data.Entities;

public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Terminated = 3
}

[Table("authuser")]
public class AuthUser
{

    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("user_name")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "First name Length is Not valid")]
    public required string UserName { get; set; }

    [Column("password_hash")]
    public required string PasswordHash { get; set; }

    [Column("refresh_token")]
    public string? RefreshToken { get; set; }

    [Column("user_status")]
    public UserStatus UserStatus { get; set; }

}

