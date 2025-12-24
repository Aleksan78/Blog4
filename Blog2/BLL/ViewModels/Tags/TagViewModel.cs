using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.Tags
{
    public class TagViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Название тега обязательно")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Тег должен быть от 2 до 20 символов")]
        [Display(Name = "Тег")]
        public string Name { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
    }
}
