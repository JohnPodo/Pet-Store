using PetMeUp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetMeUp.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 15 character in length.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 character in length.")]
        public string? Password { get; set; }
    }
}