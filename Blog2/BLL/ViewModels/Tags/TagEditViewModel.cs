using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.Tags
{
    public class TagEditViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Название тега не может быть пустым")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 20 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите новое название тега")]
        public string Name { get; set; } = string.Empty;
    }
}
