using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для работы с ролями блога
    /// </summary>
    /// <param name="roleService"></param>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Администратор")] // Ограничиваем доступ на уровне всего контроллера
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        /// <summary>
        /// Получить список всех ролей
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await roleService.GetRoles();
            // Возвращаем список ролей (id и имя)
            return Ok(roles);
        }

        /// <summary>
        /// Создать новую роль
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roleId = await roleService.CreateRole(model);
            return StatusCode(201, new { id = roleId, message = "Роль успешно создана" });
        }

        /// <summary>
        /// Редактировать существующую роль
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] RoleEditViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await roleService.EditRole(model);
            return Ok(new { message = "Роль успешно обновлена" });
        }

        /// <summary>
        /// Удалить роль по ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Проверяем наличие роли перед удалением (опционально, зависит от логики DAL)
            await roleService.RemoveRole(id);
            return NoContent(); // Стандартный ответ 204 для успешного удаления
        }
    }
}