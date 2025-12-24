using Blog2.DAL.Models;

namespace Blog2.DAL.Repositories.IRepositories
{
    public interface ICommentRepository
    {
        //List<Comment> GetAllComments();
        Task<List<Comment>> GetAllComments();
        //Comment GetComment(Guid id);
        Task<Comment?> GetComment(Guid id); // Добавили Task и Nullable
        //List<Comment> GetCommentsByPostId(Guid id);
        Task<List<Comment>> GetCommentsByPostId(Guid id);
        Task AddComment(Comment item);
        Task UpdateComment(Comment item);
        Task RemoveComment(Guid id);
        Task<bool> SaveChangesAsync();
    }
}
