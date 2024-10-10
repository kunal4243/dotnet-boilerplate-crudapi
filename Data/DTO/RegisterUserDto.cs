using System.ComponentModel.DataAnnotations;

namespace BoilerPlate.Data.DTO;

public class RegisterUserDto
{
    /// <summary>
    /// Users desired username(required and unique)
    /// </summary>
    /// <example>Kunal1</example>
    [StringLength(20, MinimumLength = 3, ErrorMessage = "User Name Length is Not valid")]
    public required string UserName { get; set; }

    /// <summary>
    /// Users desired password (required)
    /// </summary>
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Password should be between 8 to 20")]
    [RegularExpression(@"^(?=.*[0-9])[a-zA-Z0-9\-\?\$\%]*$", ErrorMessage = "Password must include at least one number and can only contain letters, numbers, and special characters (-, ?, $, %).")]
    public required string Password { get; set; }

    /// <summary>
    /// Users first name
    /// </summary>
    /// <example>Kunal</example>
    [StringLength(20, MinimumLength = 3, ErrorMessage = "First name Length is Not valid")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Users middle name
    /// </summary>
    /// <example>kumar</example>
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Middle name Length is Not valid")]
    public string? MiddleName { get; set; }

    /// <summary>
    /// Users last name
    /// </summary>
    /// <example>Gupta</example>
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Last name Length is Not valid")]
    public string? LastName { get; set; }

    /// <summary>
    /// Users country
    /// </summary>
    /// <example>India</example>
    [StringLength(60, MinimumLength = 3, ErrorMessage = "Country Length is Not valid")]
    public string? Country { get; set; }

    /// <summary>
    /// Users email (required and unique)
    /// </summary>
    /// <example>kunal1@user.com</example>
    [Required(ErrorMessage = "Invalid email id")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Email Address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }


}
