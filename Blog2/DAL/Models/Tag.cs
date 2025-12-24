using Microsoft.Extensions.Hosting;

namespace Blog2.DAL.Models
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = String.Empty;
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
