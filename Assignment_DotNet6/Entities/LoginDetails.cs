using System.ComponentModel.DataAnnotations;

namespace Assignment_DotNet6.Entities
{
    public class LoginDetails
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
