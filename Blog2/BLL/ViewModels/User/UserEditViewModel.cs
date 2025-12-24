using Blog2.BLL.ViewModels.Roles;
using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.User
{
    public class UserEditViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string? FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string? LastName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Никнейм", Prompt = "Введите никнейм")]
        public string? UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Почта")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string? NewPassword { get; set; }

        [Display(Name = "Роли")]
        //public List<RoleViewModel> Roles { get; set; }
        public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();

        public Guid Id { get; set; }
    }
}
