using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ResetPasswordDto
    {
        [Required]
        [RegularExpression("^(.{0,}(([a-zA-Z][^a-zA-Z])|([^a-zA-Z][a-zA-Z])).{4,})|(.{1,}" +
        "(([a-zA-Z][^a-zA-Z])|([^a-zA-Z][a-zA-Z])).{3,})|(.{2,}(([a-zA-Z][^a-zA-Z])|(" +
        "[^a-zA-Z][a-zA-Z])).{2,})|(.{3,}(([a-zA-Z][^a-zA-Z])|([^a-zA-Z][a-zA-Z])).{1" +
        ",})|(.{4,}(([a-zA-Z][^a-zA-Z])|([^a-zA-Z][a-zA-Z])).{0,})$",
        ErrorMessage = "Password must contain at least 1 letter, 1 non letter and at least 6 characters")]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required] 
        public string Token { get; set; }
    }
}