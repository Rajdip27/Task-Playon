using System.ComponentModel.DataAnnotations;

namespace TaskPlayon.Domain.Model.Auth;

public class LoginModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = "admin@localhost.com";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "P@ssword1";

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
