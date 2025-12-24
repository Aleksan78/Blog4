using Blog2.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для работы с тэгами блога
    /// </summary>
    /// <param name="tagService"></param>
    [ApiController]
    [Route("api/[controller]")] // Это значит, что адрес будет: api/tags
    public class TagsController(ITagService tagService) : ControllerBase
    {
        
        /// <summary>
        /// Метод для получения всех тегов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await tagService.GetTags();

            // Если тегов нет, возвращаем пустой список со статусом 200
            // Если всё ок, возвращаем данные в формате JSON
            return Ok(tags);
        }

       
        /// <summary>
        /// Метод для получения одного тега по его ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag(Guid id)
        {
            var tags = await tagService.GetTags();
            var tag = tags.FirstOrDefault(t => t.Id == id);

            if (tag == null)
            {
                return NotFound(new { message = $"Тег с ID {id} не найден" });
            }

            return Ok(tag);
        }
    }
}
