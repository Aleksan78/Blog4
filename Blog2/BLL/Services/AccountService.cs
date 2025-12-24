using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.DAL.Models;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Blog2.BLL.ViewModels.User;
using Blog2.BLL.ViewModels.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog2.BLL.Services; // Совет 4: Сокращенный namespace

// Совет 4: Primary Constructor (зависимости сразу в заголовке класса)
public class AccountService(    
    RoleManager<Role> roleManager,
    IMapper mapper,
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ILogger<AccountService> logger) : IAccountService
{
    public async Task<IdentityResult> Register(UserRegisterViewModel model)
    {
        var user = mapper.Map<User>(model);
        var result = await userManager.CreateAsync(user, model.Password ?? string.Empty);

        if (result.Succeeded)
        {
            // Совет 5: Логирование через параметры (Email попадет в лог как отдельное поле)
            logger.LogInformation("Новый пользователь зарегистрирован: {Email}", user.Email);

            await signInManager.SignInAsync(user, false);

            if (!await roleManager.RoleExistsAsync("Пользователь"))
            {
                await roleManager.CreateAsync(new Role { Name = "Пользователь", SecurityLvl = 0 });
            }

            await userManager.AddToRoleAsync(user, "Пользователь");
        }
        return result;
    }

    public async Task<SignInResult> Login(UserLoginViewModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email ?? string.Empty);
        if (user == null) return SignInResult.Failed;

        var result = await signInManager.PasswordSignInAsync(user, model.Password ?? string.Empty, true, false);

        if (result.Succeeded)
        {
            logger.LogInformation("Пользователь {Email} вошел в систему", user.Email);
        }

        return result;
    }

    public async Task<UserEditViewModel?> EditAccount(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null) return null;

        var allRoles = await roleManager.Roles.ToListAsync();
        var model = mapper.Map<UserEditViewModel>(user);

        var userRoles = new List<RoleViewModel>();
        foreach (var r in allRoles)
        {
            var roleName = r.Name ?? string.Empty;
            userRoles.Add(new RoleViewModel
            {
                Id = Guid.Parse(r.Id),
                Name = roleName,
                IsSelected = await userManager.IsInRoleAsync(user, roleName)
            });
        }

        model.Roles = userRoles;
        return model;
    }

    public async Task<IdentityResult> EditAccount(UserEditViewModel model)
    {
        var user = await userManager.FindByIdAsync(model.Id.ToString());
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Description = "Пользователь не найден" });

        user.FirstName = model.FirstName ?? user.FirstName;
        user.LastName = model.LastName ?? user.LastName;
        user.Email = model.Email ?? user.Email;
        user.UserName = model.UserName ?? user.UserName;

        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.NewPassword);
        }

        foreach (var role in model.Roles)
        {
            var roleEntity = await roleManager.FindByIdAsync(role.Id.ToString());
            if (roleEntity?.Name != null)
            {
                var isInRole = await userManager.IsInRoleAsync(user, roleEntity.Name);
                if (role.IsSelected && !isInRole) await userManager.AddToRoleAsync(user, roleEntity.Name);
                else if (!role.IsSelected && isInRole) await userManager.RemoveFromRoleAsync(user, roleEntity.Name);
            }
        }

        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            logger.LogInformation("Профиль пользователя {Email} обновлен", user.Email);
        }
        return result;
    }

    public async Task RemoveAccount(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            var email = user.Email;
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                logger.LogInformation("Аккаунт пользователя {Email} (ID: {Id}) удален", email, id);
            }
        }
    }

    public async Task<List<User>> GetAccounts()
    {
        var accounts = await userManager.Users.Include(u => u.Posts).ToListAsync();
        foreach (var user in accounts)
        {
            var roles = await userManager.GetRolesAsync(user);
            user.Roles = roles.Select(r => new Role { Name = r }).ToList();
        }
        return accounts;
    }

    public async Task LogoutAccount()
    {
        var userName = signInManager.Context.User.Identity?.Name ?? "Unknown";
        await signInManager.SignOutAsync();
        logger.LogInformation("Пользователь {Name} вышел из системы", userName);
    }
}