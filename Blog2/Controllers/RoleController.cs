using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog2.Controllers
{
    // Можно вынести Authorize на уровень класса, если ВСЕ методы только для админов
    [Authorize(Roles = "Администратор, Модератор")]
    public class RoleController(
    IRoleService roleService) : Controller
    {

        /// <summary>
        /// Отображает форму для создания новой роли.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <returns>Представление формы создания роли.</returns>
        [Route("Role/Create")]
        [HttpGet]
        public IActionResult CreateRole() => View();

        /// <summary>
        /// Обрабатывает отправку формы создания роли.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="model">Модель с данными новой роли.</param>
        /// <returns>Перенаправляет на главную страницу в случае успеха, иначе возвращает форму с ошибкой.</returns>
        [Route("Role/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resultId = await roleService.CreateRole(model);
            if (resultId == Guid.Empty)
            {
                ModelState.AddModelError("", "Ошибка при создании роли или роль уже существует");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Отображает форму для редактирования существующей роли по ID.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="id">ID роли для редактирования.</param>
        /// <returns>Представление формы редактирования.</returns>
        [HttpGet("Role/Edit/{id}")]
        public async Task<IActionResult> EditRole(Guid id)
        {
            var roles = await roleService.GetRoles();
            var role = roles.FirstOrDefault(x => x.Id == id.ToString());

            if (role == null) return NotFound();

            var model = new RoleEditViewModel
            {
                Id = id,
                Name = role.Name ?? string.Empty,
                SecurityLvl = role.SecurityLvl
            };
            return View(model);
        }

        /// <summary>
        /// Обрабатывает отправку формы редактирования роли.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="model">Модель с измененными данными роли.</param>
        /// <returns>Перенаправляет на главную страницу в случае успеха, иначе возвращает форму с ошибкой.</returns>
        [HttpPost("Role/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(RoleEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await roleService.EditRole(model);
            return RedirectToAction("GetRoles");
        }


        /// <summary>
        /// Удаляет роль (POST-запрос для безопасного удаления).
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="id">ID роли для удаления.</param>
        /// <returns>Перенаправляет на главную страницу.</returns>
        // Оставляем ТОЛЬКО Post для удаления
        [HttpPost("Role/Remove/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(Guid id)
        {
            await roleService.RemoveRole(id);
            return RedirectToAction("GetRoles"); // Возвращаемся к списку ролей
        }

        /// <summary>
        /// Получает и отображает список всех ролей.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <returns>Представление со списком ролей.</returns>
        [HttpGet("Role/GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await roleService.GetRoles();
            return View(roles);
        }
    }
}
