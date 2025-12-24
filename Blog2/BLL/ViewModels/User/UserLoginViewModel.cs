using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.User
{
    public class UserLoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Введите email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
