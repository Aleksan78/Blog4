using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Roles;
using Blog2.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog2.BLL.Services;

public class RoleService(
    IMapper mapper,
    RoleManager<Role> roleManager,
    ILogger<RoleService> logger) : IRoleService
{
    public async Task<Guid> CreateRole(RoleCreateViewModel model)
    {
        var roleName = model.Name ?? string.Empty;

        if (await roleManager.RoleExistsAsync(roleName))
        {
            logger.LogWarning("Попытка создания уже существующей роли: {RoleName}", roleName);
            return Guid.Empty;
        }

        var role = mapper.Map<Role>(model);
        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("Ошибка при создании роли {RoleName}: {Errors}", roleName, errors);
            return Guid.Empty;
        }

        logger.LogInformation("Создана новая роль: {RoleName} (Level: {Level})", role.Name, role.SecurityLvl);
        return Guid.Parse(role.Id);
    }

    public async Task EditRole(RoleEditViewModel model)
    {
        var role = await roleManager.FindByIdAsync(model.Id.ToString());
        if (role == null)
        {
            logger.LogWarning("Роль ID: {RoleId} не найдена для редактирования", model.Id);
            return;
        }

        var oldName = role.Name;
        mapper.Map(model, role);

        var result = await roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
            logger.LogInformation("Роль {OldName} обновлена. Новое имя: {NewName}", oldName, role.Name);
        }
    }

    public async Task RemoveRole(Guid id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());
        if (role != null)
        {
            var roleName = role.Name;
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                logger.LogInformation("Роль {RoleName} (ID: {RoleId}) удалена", roleName, id);
            }
        }
    }

    public async Task<List<Role>> GetRoles() => await roleManager.Roles.ToListAsync();
}