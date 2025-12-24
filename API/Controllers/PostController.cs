using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для работы с постами блога
    /// </summary>
    /// <param name="postService"></param>
    [Route("api/[controller]")]
    [ApiController]
    
    public class PostsController(IPostService postService) : ControllerBase
    {
        
        /// <summary>
        /// Получить все статьи
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await postService.GetPosts();
            return Ok(posts);
        }

        
        /// <summary>
        /// Получить одну статью по ID (используем ваш ShowPost)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var post = await postService.ShowPost(id); // Исправлено на ShowPost
            if (post == null) return NotFound(new { message = "Статья не найдена" });

            return Ok(post);
        }

        
        /// <summary>
        /// Создать новую статью
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await postService.CreatePost(model);

            // Возвращаем статус 201 Created и ссылку на получение этой статьи
            return CreatedAtAction(nameof(Get), new { id = id }, model);
        }

        
        /// <summary>
        /// Обновить статью
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, PostEditViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var post = await postService.ShowPost(id);
            if (post == null) return NotFound();

            await postService.EditPost(model, id);
            return Ok(new { message = "Статья обновлена" });
        }

        
        /// <summary>
        /// Удалить статью
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await postService.ShowPost(id);
            if (post == null) return NotFound();

            await postService.RemovePost(id);
            return NoContent(); // Стандарт для успешного удаления (204 No Content)
        }
    }
}