using System.ComponentModel.DataAnnotations;

namespace disease_outbreaks_detector.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(500, ErrorMessage = "Full name cannot exceed 500 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format (RFC 822).")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Phone must be in format +380XXXXXXXXX")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be 8-16 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$",
            ErrorMessage = "Password must contain: 1 uppercase, 1 lowercase, 1 digit, 1 special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}