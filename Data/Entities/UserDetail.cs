using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoilerPlate.Data.Entities;

[Table("userdetails")]
public class UserDetail
{

    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("first_name")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "First name Length is Not valid")]
    public string? FirstName { get; set; }

    [Column("middle_name")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Middle name Length is Not valid")]
    public string? MiddleName { get; set; }

    [Column("last_name")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Last name Length is Not valid")]
    public string? LastName { get; set; }

    [Column("country")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "Country Length is Not valid")]
    public string? Country { get; set; }

    [Column("email_id")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Email Length is Not valid")]
    public required string Email { get; set; }

}

