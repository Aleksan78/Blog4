using Blog2.BLL.ViewModels.Tags;

namespace Blog2.BLL.ViewModels.Posts
{
    public class ShowPostViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Инициализируем строки, чтобы избежать NullReferenceException при рендеринге страницы
        public string AuthorId { get; set; } = string.Empty;

        // Инициализируем список, чтобы цикл foreach во View не упал, если тегов нет
        public List<TagViewModel> Tags { get; set; } = new();
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
