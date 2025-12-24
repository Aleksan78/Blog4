using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.User
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от 2 до 50 символов")]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Никнейм обязательно для заполнения")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Никнейм должен быть от 3 до 30 символов")]
        [Display(Name = "Никнейм", Prompt = "Введите Никнейм")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
        //[EmailAddress]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        [Display(Name = "Email", Prompt = "example.com")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Обязательно подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль", Prompt = "Введите пароль повторно")]
        public string PasswordReg { get; set; } = string.Empty;
    }
}
