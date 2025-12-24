using Blog2.DAL.Models;

namespace Blog2.DAL.Repositories.IRepositories
{
    public interface IPostRepository
    {
        //List<Post> GetAllPosts();
        Task<List<Post>> GetAllPosts();
        //Post GetPost(Guid id);
        Task<Post?> GetPost(Guid id); // Добавили Task и возможность null
        Task AddPost(Post post);
        Task UpdatePost(Post post);
        Task RemovePost(Guid id);
        Task<bool> SaveChangesAsync();
    }
}
