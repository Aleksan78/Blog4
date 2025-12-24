namespace Blog2.DAL.Models
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public string realAuthorName { get; set; } = string.Empty;
        // ДОБАВЛЕНО: Дата создания комментария
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
