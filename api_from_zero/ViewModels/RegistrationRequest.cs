using System.ComponentModel.DataAnnotations;
namespace api_from_zero.ViewModels
{
    public class RegistrationRequest
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password can not be empty")]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Birth date is required")]
        public DateOnly BirthDate {  get; set; }
    }
}
