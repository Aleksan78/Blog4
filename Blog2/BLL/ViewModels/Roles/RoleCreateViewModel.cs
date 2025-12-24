using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.Roles
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Название")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Уровень доступа обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Уровень доступа", Prompt = "Уровень")]
        public int SecurityLvl { get; set; }
    }
}
