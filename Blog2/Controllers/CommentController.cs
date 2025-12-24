using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Comments;
using Blog2.DAL.Models;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog2.Controllers
{
    // Совет 4: Primary Constructor
    public class CommentController(
        ICommentService commentService,
        UserManager<User> userManager) : Controller
    {
        /// <summary>
        /// Отображает форму для создания нового комментария.
        /// </summary>
        /// <param name="postId">ID поста, к которому будет добавлен комментарий.</param>
        /// <returns>Представление формы создания комментария.</returns>
        [HttpGet]
        [Authorize]
        [Route("Comment/CreateComment")]
        public IActionResult CreateComment(Guid postId)
        {
            return View(new CommentCreateViewModel { PostId = postId });
        }
        /// <summary>
        /// Обрабатывает отправку формы создания комментария.
        /// </summary>
        /// <param name="model">Модель с данными комментария.</param>
        /// <param name="PostId">ID поста, к которому привязывается комментарий (передается в маршруте).</param>
        /// <returns>Перенаправляет на главную страницу.</returns>

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("Comment/CreateComment")]
        public async Task<IActionResult> CreateComment(CommentCreateViewModel model)
        {
            // Совет 2: Тонкий action. Валидация и вызов сервиса.
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.GetUserAsync(User);
            if (user == null) return Challenge(); // Если вдруг сессия просрочена

            await commentService.CreateComment(model, Guid.Parse(user.Id));
            return RedirectToAction("ShowPost", "Post", new { id = model.PostId });
        }

        /// <summary>
        /// Отображает форму для редактирования существующего комментария.
        /// </summary>
        /// <param name="id">ID комментария для редактирования.</param>
        /// <returns>Представление формы редактирования.</returns>
        [HttpGet]
        [Route("Comment/Edit")]
        [Authorize]   
        public IActionResult EditComment(Guid id)
        {
            return View(new CommentEditViewModel { Id = id });
        }

        /// <summary>
        /// Обрабатывает отправку формы редактирования комментария.
        /// Требует авторизации.
        /// </summary>
        /// <param name="model">Модель с измененными данными комментария.</param>
        /// <returns>Перенаправляет на главную страницу в случае успеха, иначе возвращает форму с ошибкой.</returns>

        [HttpPost]
        [Authorize]
        [Route("Comment/Edit")]        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(CommentEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await commentService.EditComment(model);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Удаляет комментарий (предпочтительный HTTP DELETE).
        /// </summary>
        /// <param name="id">ID комментария для удаления.</param>
        /// <returns>Перенаправляет на главную страницу.</returns>
        //[HttpDelete]
        //[Route("Comment/Remove")]
        // Оставляем только POST для безопасности, убираем GET
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("Comment/Remove/{id}")]
        public async Task<IActionResult> RemoveComment(Guid id)
        {
            await commentService.RemoveComment(id);
            return RedirectToAction("Index", "Home"); ;
        }
    }
}
