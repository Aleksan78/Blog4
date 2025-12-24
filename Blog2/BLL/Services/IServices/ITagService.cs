using Blog2.BLL.ViewModels.Tags;
using Blog2.DAL.Models;

namespace Blog2.BLL.Services.IServices
{
    public interface ITagService
    {
        Task<Guid> CreateTag(TagCreateViewModel model);
        Task EditTag(TagEditViewModel model);
        Task RemoveTag(Guid id);
        Task<List<Tag>> GetTags();
    }
}
