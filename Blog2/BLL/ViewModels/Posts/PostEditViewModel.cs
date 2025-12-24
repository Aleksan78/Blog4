using Blog2.BLL.ViewModels.Tags;
using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.Posts
{
    public class PostEditViewModel
    {
        public Guid id { get; set; } 

        [Required(ErrorMessage = "Заголовок не может быть пустым")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Заголовок должен быть от 5 до 100 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок", Prompt = "Введите новый заголовок")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Текст статьи не может быть пустым")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Текст статьи слишком короткий")]
        [DataType(DataType.Text)]
        [Display(Name = "Описание", Prompt = "Отредактируйте текст статьи")]
        public string Body { get; set; } = string.Empty;

        [Display(Name = "Теги")]
        // Инициализируем список, чтобы избежать ошибок при отображении чекбоксов
        public List<TagViewModel> Tags { get; set; } = new();
    }
}
