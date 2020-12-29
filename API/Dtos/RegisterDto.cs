using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
	public class RegisterDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		[RegularExpression("^(.{0,}(([a-zA-Z][^a-zA-Z])|([^a-zA-Z][a-zA-Z])).{4,})|(.{1,}" +
			"(([a-zA-Z][^a-zA-Z])|([^a-zA-Z][a-zA-Z])).{3,})|(.{2,}(([a-zA-Z][^a-zA-Z])|(" +
			"[^a-zA-Z][a-zA-Z])).{2,})|(.{3,}(([a-zA-Z][^a-zA-Z])|([^a-zA-Z][a-zA-Z])).{1" +
			",})|(.{4,}(([a-zA-Z][^a-zA-Z])|([^a-zA-Z][a-zA-Z])).{0,})$",
			ErrorMessage = "Password must contain at least 1 letter, 1 non letter and at least 6 characters")]
		public string Password { get; set; }
		[Required]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
	}
}
