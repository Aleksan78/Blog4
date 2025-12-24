using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Tags;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog2.Controllers
{
    [Authorize(Roles = "Администратор, Модератор")]
    public class TagController(
    ITagService tagService,
    ILogger<TagController> logger) : Controller
    {

        /// <summary>
        /// Отображает форму для создания нового тега.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <returns>Представление формы создания тега.</returns>
        [Route("Tag/Create")]
        [HttpGet]
        public IActionResult CreateTag() => View();

        /// <summary>
        /// Обрабатывает отправку формы создания тега.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="model">Модель с данными нового тега.</param>
        /// <returns>Перенаправляет на главную страницу в случае успеха, иначе возвращает форму с ошибкой.</returns>
        [Route("Tag/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTag(TagCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var tagId = await tagService.CreateTag(model);

            // Используем параметр 'logger' из конструктора напрямую
            logger.LogInformation("Пользователь создал тег: {TagName}", model.Name);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Отображает форму для редактирования существующего тега по ID.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="id">ID тега для редактирования.</param>
        /// <returns>Представление формы редактирования.</returns>
        [HttpGet("Tag/Edit/{id}")]
        public async Task<IActionResult> EditTag(Guid id)
        {
            var tags = await tagService.GetTags();
            var tag = tags.FirstOrDefault(t => t.Id == id);

            if (tag == null) return NotFound();

            return View(new TagEditViewModel { Id = tag.Id, Name = tag.Name });
        }

        /// <summary>
        /// Обрабатывает отправку формы редактирования тега.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="model">Модель с измененными данными тега.</param>
        /// <returns>Перенаправляет на главную страницу в случае успеха, иначе возвращает форму с ошибкой.</returns>        
        [HttpPost("Tag/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTag(TagEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await tagService.EditTag(model);
            logger.LogDebug("Тег изменен на: {TagName}", model.Name);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Удаляет тег (POST-запрос для безопасного удаления).
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="id">ID тега для удаления.</param>
        /// <returns>Перенаправляет на главную страницу.</returns>
        // Оставляем только POST для безопасности
        [HttpPost("Tag/Remove/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTag(Guid id)
        {
            await tagService.RemoveTag(id);
            logger.LogInformation("Удален тег с ID: {TagId}", id);

            return RedirectToAction("GetTags");
        }

        /// <summary>
        /// Получает и отображает список всех тегов.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <returns>Представление со списком тегов.</returns>
        [HttpGet("Tag/Get")]
        public async Task<IActionResult> GetTags()
        {
            var tags = await tagService.GetTags();
            return View(tags);
        }
    }
}
