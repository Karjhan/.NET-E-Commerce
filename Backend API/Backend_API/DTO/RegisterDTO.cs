using System.ComponentModel.DataAnnotations;

namespace Backend_API.DTO;

public class RegisterDTO
{
    [Required]
    public string DisplayName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression("\t\n^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s).{4,8}$", ErrorMessage = "Password must have 1 uppercase, 1 lowercase and 1 digit and at least 6 characters!")]
    public string Password { get; set; }
}