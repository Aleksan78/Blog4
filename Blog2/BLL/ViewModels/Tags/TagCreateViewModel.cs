using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.Tags
{
    public class TagCreateViewModel
    {
        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        [DataType(DataType.Text)]
        [StringLength(20)]
        [Display(Name = "Название", Prompt = "Название")]
        public string Name { get; set; } = string.Empty;
    }
}
