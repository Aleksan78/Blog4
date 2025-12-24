using Blog2.BLL.ViewModels.Comments;
using Blog2.DAL.Models;

namespace Blog2.BLL.Services.IServices
{
    public interface ICommentService
    {
        Task<Guid> CreateComment(CommentCreateViewModel model, Guid UserId);
        Task EditComment(CommentEditViewModel model);
        Task RemoveComment(Guid id);
        Task<List<Comment>> GetComments();
    }
}
