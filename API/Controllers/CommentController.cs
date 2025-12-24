using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Comments; // Убедитесь, что папка с ViewModel комментариев называется так
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для работы с комментариями блога
    /// </summary>
    /// <param name="commentService"></param>
    [Route("api/[controller]")]
    [ApiController]
    // Используем первичный конструктор для внедрения сервиса
    public class CommentController(ICommentService commentService) : ControllerBase
    {
        /// <summary>
        /// Получить все комментарии к конкретной статье
        /// </summary>
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetByPostId(Guid postId)
        {
            // Предполагаем, что в вашем сервисе есть метод получения комментариев по ID поста
            var comments = await commentService.GetComments();
            var postComments = comments.Where(c => c.PostId == postId).ToList();

            return Ok(postComments);
        }

        /// <summary>
        /// Добавить новый комментарий
        /// </summary>
        [HttpPost]
        [Authorize] // Только авторизованные пользователи могут комментировать
        public async Task<IActionResult> Create([FromBody] CommentCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1. Извлекаем ID текущего пользователя из Claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("Не удалось определить пользователя");

            // 2. Преобразуем строку в Guid
            if (!Guid.TryParse(userIdString, out Guid userId))
                return BadRequest("Некорректный формат ID пользователя");

            // 3. Передаем модель и UserId в сервис
            var commentId = await commentService.CreateComment(model, userId);

            return StatusCode(201, new { id = commentId, message = "Комментарий добавлен" });
        }

        /// <summary>
        /// Удалить комментарий
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Вставьте имя метода удаления из вашего ICommentService
            await commentService.RemoveComment(id);
            return NoContent();
        }
    }
}