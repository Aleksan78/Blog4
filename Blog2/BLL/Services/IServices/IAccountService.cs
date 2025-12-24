using Blog2.DAL.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Blog2.BLL.ViewModels.User;

namespace Blog2.BLL.Services.IServices
{
    public interface IAccountService
    {
        Task<IdentityResult> Register(UserRegisterViewModel model);
        Task<SignInResult> Login(UserLoginViewModel model);
        Task<IdentityResult> EditAccount(UserEditViewModel model);
        Task<UserEditViewModel?> EditAccount(Guid id);
        Task RemoveAccount(Guid id);
        Task<List<User>> GetAccounts();
        Task LogoutAccount();
    }
}
