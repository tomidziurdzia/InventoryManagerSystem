using System.ComponentModel.DataAnnotations;
namespace Application.DTO.Request.Identity;

public class LoginUserRequestDTO
{
    [EmailAddress]
    [RegularExpression("[^@\\t\\r\\n]+@[^@\\t\\r\\n]+\\.[^@\\t\\r\\n]+", ErrorMessage = "Your email is not valid")]
    public string Email { get; set; }
    
    [Required]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %*&*-]).{8,}$", ErrorMessage = "You password")]
    [MinLength(8), MaxLength(100)]
    public string Password { get; set; }
}