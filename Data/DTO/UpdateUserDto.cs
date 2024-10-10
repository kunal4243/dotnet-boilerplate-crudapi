using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoilerPlate.Data.DTO
{
    public class UpdateUserDto
    {
        [Required]
        public int UserId { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "First name Length is Not valid")]
        public string? FirstName { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Middle name Length is Not valid")]
        public string? MiddleName { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Last name Length is Not valid")]
        public string? LastName { get; set; }

        [StringLength(60, MinimumLength = 3, ErrorMessage = "Country Length is Not valid")]
        public string? Country { get; set; }
    }
}
