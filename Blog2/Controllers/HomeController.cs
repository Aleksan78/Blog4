using AutoMapper.Internal;
using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Blog2.BLL.ViewModels;

namespace Blog2.Controllers
{
    // Совет 4: Primary Constructor (зависимости сокращены до минимума)
    public class HomeController(
        IHomeService homeService,
        ILogger<HomeController> logger) : Controller
    {

        /// <summary>
        /// Отображает главную страницу сайта.
        /// Выполняет инициализацию тестовых данных (пользователей и ролей) в режиме разработки.
        /// </summary>
        /// <returns>Представление главной страницы (View).</returns>
        public async Task<IActionResult> Index()
        {
            // Совет 2: Тонкий action — только вызов сервиса и возврат View
            await homeService.GenerateUsers();
            return View(new MainViewModel());
        }

        /// <summary>
        /// Страница личного кабинета пользователя. 
        /// Доступна только для авторизованных пользователей.
        /// </summary>
        /// <returns>Представление личной страницы пользователя.</returns>
        [Authorize]
        public IActionResult MyPage() => View();

        /// <summary>
        /// Отображает страницу политики конфиденциальности.
        /// </summary>
        /// <returns>Представление политики конфиденциальности.</returns>
        public IActionResult Privacy() => View();

        /// <summary>
        /// Отображает страницы ошибок (404, 500 и другие).
        /// </summary>
        /// <param name="statusCode">Код ошибки (например, 404 или 500).</param>
        /// <returns>Специальное представление для ошибки.</returns>
        [Route("Home/Error")]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode == 404)
            {
                // Совет 5: Параметризованное логирование
                logger.LogWarning("Запрошена несуществующая страница (404)");
                return View("404");
            }

            logger.LogError("Критическая ошибка сервера (500)");
            return View("500");
        }
    }
    
}
