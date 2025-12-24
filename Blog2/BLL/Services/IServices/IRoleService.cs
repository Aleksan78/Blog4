using Blog2.BLL.ViewModels.Roles;
using Blog2.DAL.Models;

namespace Blog2.BLL.Services.IServices
{
    public interface IRoleService
    {
        Task<Guid> CreateRole(RoleCreateViewModel model);
        Task EditRole(RoleEditViewModel model);
        Task RemoveRole(Guid id);
        Task<List<Role>> GetRoles();
    }
}
