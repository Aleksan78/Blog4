using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog2.BLL.ViewModels.User;

namespace Blog2.Controllers
{
    public class AccountController(
        IAccountService accountService,
        ILogger<AccountController> logger) : Controller
    {


        /// <summary>
        /// Отображает форму для входа в учетную запись.
        /// </summary>
        /// <returns>Представление формы входа (View).</returns>       
        [Route("Account/Login")]
        public IActionResult Login() => View();

        /// <summary>
        /// Обрабатывает отправку формы входа. 
        /// Осуществляет аутентификацию пользователя.
        /// </summary>
        /// <param name="model">Модель с учетными данными пользователя (логин и пароль).</param>
        /// <returns>
        /// Перенаправляет на главную страницу в случае успешного входа,
        /// иначе возвращает форму с ошибкой.
        /// </returns>
        [Route("Account/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await accountService.Login(model);
            if (result.Succeeded) return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty, "Неправильный логин и (или) пароль");
            return View(model);
        }

        /// <summary>
        /// Отображает форму для регистрации новой учетной записи.
        /// </summary>
        /// <returns>Представление формы регистрации (View).</returns>
        [HttpGet("Account/Register")] // Объединяем маршрут и метод
        public IActionResult Register() => View();


        /// <summary>
        /// Обрабатывает отправку формы регистрации.
        /// Создает нового пользователя и, при необходимости, назначает ему базовую роль.
        /// </summary>
        /// <param name="model">Модель с данными нового пользователя.</param>
        /// <returns>
        /// Перенаправляет на главную страницу в случае успешной регистрации,
        /// иначе возвращает форму с ошибками валидации.
        /// </returns>
        [Route("Account/Register")]
        [HttpPost]
        [ValidateAntiForgeryToken] // Добавляем защиту от CSRF
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await accountService.Register(model);
            if (result.Succeeded) return RedirectToAction("Index", "Home");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        /// <summary>
        /// Отображает форму для редактирования учетной записи по ID.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="id">Уникальный идентификатор (GUID) пользователя для редактирования.</param>
        /// <returns>Представление формы редактирования с данными пользователя.</returns>
        [Route("Account/Edit")]
        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        public async Task<IActionResult> EditAccount(Guid id)
        {
            var model = await accountService.EditAccount(id);
            return model == null ? NotFound() : View(model);
        }

        /// <summary>
        /// Обрабатывает отправку формы редактирования учетной записи.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="model">Модель с измененными данными пользователя.</param>
        /// <returns>
        /// Перенаправляет на главную страницу в случае успеха,
        /// иначе возвращает форму редактирования с ошибками.
        /// </returns>
        [Route("Account/Edit")]
        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(UserEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await accountService.EditAccount(model);
            if (result.Succeeded)
            {
                logger.LogDebug("Аккаунт {UserName} изменен", model.UserName);
                return RedirectToAction(nameof(GetAccounts));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }
        

        /// <summary>
        /// Удаляет учетную запись пользователя. (POST-метод для безопасного удаления).
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <param name="id">Уникальный идентификатор (GUID) пользователя для удаления.</param>
        /// <returns>Перенаправляет на главную страницу.</returns>
        [Route("Account/Remove")]
        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAccount(Guid id)
        {
            await accountService.RemoveAccount(id);
            logger.LogDebug("Удален аккаунт с ID: {Id}", id);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Выход из учетной записи (Logout).
        /// </summary>
        /// <param name="id">Не используется, но сохранено в сигнатуре.</param>
        /// <returns>Перенаправляет на главную страницу.</returns>
        [Route("Account/Logout")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LogoutAccount(Guid id)
        {
            await accountService.LogoutAccount();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Получает список всех учетных записей.
        /// Доступно только для Администратора и Модератора.
        /// </summary>
        /// <returns>Представление со списком пользователей.</returns>
        [Route("Account/Get")]
        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            return View(await accountService.GetAccounts());
        }
    }
}
