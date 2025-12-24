using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.User; // Подключаем ваши модели
using Blog2.DAL.Models;          // Для класса User (Identity)
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер для работы с аккаунтами блога
    /// </summary>
    /// <param name="accountService"></param>
    /// <param name="userManager"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(
        IAccountService accountService,
        UserManager<User> userManager) : ControllerBase
    {
        /// <summary>
        /// Вход в систему
        /// </summary>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await accountService.Login(model);

            if (!result.Succeeded)
                return Unauthorized("Неверный логин или пароль");

            var user = await userManager.FindByEmailAsync(model.Email);
            var roles = await userManager.GetRolesAsync(user!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user!.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return Ok(new { message = "Авторизация успешна" });
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await accountService.Register(model);
            if (result.Succeeded)
                return StatusCode(201, new { message = "Пользователь создан" });

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Редактирование профиля
        /// </summary>
        [Authorize(Roles = "Администратор")]
        [HttpPatch("EditUser")]
        public async Task<IActionResult> EditUser([FromBody] UserEditViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await accountService.EditAccount(model);
            if (result.Succeeded)
                return Ok(new { message = "Данные обновлены" });

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        [Authorize(Roles = "Администратор")]
        [HttpDelete("RemoveUser/{id}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            await accountService.RemoveAccount(id);
            return NoContent();
        }

        /// <summary>
        /// Список всех пользователей (только для админа)
        /// </summary>
        [Authorize(Roles = "Администратор")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await accountService.GetAccounts();
            // Возвращаем список анонимных объектов, чтобы не тянуть лишние данные
            var response = users.Select(u => new { u.Id, u.UserName, u.Email });
            return Ok(response);
        }
    }
}