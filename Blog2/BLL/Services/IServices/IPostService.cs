using Blog2.BLL.ViewModels.Posts;
using Blog2.DAL.Models;

namespace Blog2.BLL.Services.IServices
{
    public interface IPostService
    {
        Task<PostCreateViewModel> CreatePost();
        Task<Guid> CreatePost(PostCreateViewModel model);
        Task<PostEditViewModel?> EditPost(Guid Id);
        Task EditPost(PostEditViewModel model, Guid Id);
        Task RemovePost(Guid id);
        Task<List<Post>> GetPosts();
        Task<Post?> ShowPost(Guid id);
    }
}
