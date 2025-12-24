using Blog2.BLL.ViewModels.Tags;
using System.ComponentModel.DataAnnotations;

namespace Blog2.BLL.ViewModels.Posts
{
    public class PostCreateViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string AuthorId { get; set; } = string.Empty;
        public List<TagViewModel> Tags { get; set; } = new();


        [Required(ErrorMessage = "Поле Заголовок обязательно для заполнения")]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Заголовок должен быть от 5 до 100 символов")]
        [Display(Name = "Заголовок", Prompt = "Введите название статьи")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле Описание обязательно для заполнения")]
        [DataType(DataType.Text)]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Текст статьи должен быть не короче 10 символов")]
        [Display(Name = "Описание", Prompt = "Напишите текст вашей статьи")]
        public string Body { get; set; } = string.Empty;
    }
}
