using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Posts;
using Blog2.DAL.Models;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog2.Controllers
{
    public class PostController(
        IPostService postService,
        UserManager<User> userManager) : Controller
    {

        /// <summary>
        /// Отображает детальную страницу конкретного поста по его ID.
        /// </summary>
        /// <param name="id">Уникальный идентификатор (GUID) поста.</param>
        /// <returns>Представление поста с его содержимым и комментариями.</returns>
        [Route("Post/Show")]
        [HttpGet]
        public async Task<IActionResult> ShowPost(Guid id)
        {
            var post = await postService.ShowPost(id);
            return post == null ? NotFound() : View(post);
        }

        /// <summary>
        /// Отображает форму для создания нового поста.
        /// Требует авторизации.
        /// </summary>
        /// <returns>Представление формы создания поста, включая список доступных тегов.</returns>
        [Route("Post/Create")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreatePost()
        {
            var model = await postService.CreatePost();
            return View(model);
        }

        /// <summary>
        /// Обрабатывает отправку формы создания поста.
        /// Требует авторизации.
        /// </summary>
        /// <param name="model">Модель с данными нового поста.</param>
        /// <returns>Перенаправляет на главную страницу в случае успеха или возвращает форму с ошибкой валидации.</returns>
        [Route("Post/Create")]
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(PostCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var refreshModel = await postService.CreatePost();
                model.Tags = refreshModel.Tags;
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            model.AuthorId = user.Id;
            await postService.CreatePost(model);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Отображает форму для редактирования существующего поста по ID.
        /// </summary>
        /// <param name="id">Уникальный идентификатор (GUID) поста для редактирования.</param>
        /// <returns>Представление формы редактирования с текущими данными поста.</returns>
        [Route("Post/Edit")]
        [HttpGet]
        [Authorize] // Редактировать тоже должен только авторизованный пользователь
        public async Task<IActionResult> EditPost(Guid id)
        {
            var model = await postService.EditPost(id);
            return model == null ? NotFound() : View(model);
        }

        /// <summary>
        /// Обрабатывает отправку формы редактирования поста.
        /// Требует авторизации.
        /// </summary>
        /// <param name="model">Модель с измененными данными поста.</param>
        /// <param name="Id">Уникальный идентификатор (GUID) поста.</param>
        /// <returns>Перенаправляет на главную страницу в случае успеха или возвращает форму с ошибкой валидации.</returns>
        [HttpPost("Post/Edit/{id}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(PostEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await postService.EditPost(model, model.id);
            return RedirectToAction("GetPosts");
        }


        /// <summary>
        /// Удаляет пост (POST-запрос для безопасного удаления).
        /// </summary>
        /// <param name="id">Уникальный идентификатор (GUID) поста для удаления.</param>
        /// <returns>Перенаправляет на главную страницу.</returns>
        // Оставляем только POST для безопасности
        [HttpPost("Post/Remove/{id}")]
        [Authorize(Roles = "Администратор, Модератор")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePost(Guid id)
        {
            await postService.RemovePost(id);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Получает и отображает список всех постов.
        /// </summary>
        /// <returns>Представление со списком постов.</returns>
        [HttpGet]
        [Route("Post/Get")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await postService.GetPosts();
            return View(posts);
        }
    }
}
