using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.Comments
{
    public class CommentCreateViewModel
    {
        [Required(ErrorMessage = "Поле Заголовок обязательно для заполнения")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Заголовок должен быть от 2 до 50 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок", Prompt = "Заголовок")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Описание обязательно для заполнения")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Описание должно быть от 10 до 500 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Описание", Prompt = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Автор обязательно для заполнения")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Поле автор должно быть от 2 до 50 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Автор", Prompt = "Автор")]
        public string Author { get; set; }= string.Empty;

        // Добавляем { get; set; }, чтобы ID поста не терялся при отправке формы
        public Guid PostId { get; set; }
    }
}
