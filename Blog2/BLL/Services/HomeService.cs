using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Blog2.BLL.ViewModels.User;
using Microsoft.Extensions.Logging;

namespace Blog2.BLL.Services; // Совет 4: Сокращенный namespace

// Совет 4: Primary Constructor
public class HomeService(
    RoleManager<Role> roleManager,
    IMapper mapper,
    UserManager<User> userManager,
    ILogger<HomeService> logger) : IHomeService
{
    public async Task GenerateUsers()
    {
        const string adminRoleName = "Администратор";
        const string moderRoleName = "Модератор";
        const string userRoleName = "Пользователь";

        // Проверка и создание ролей
        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            logger.LogInformation("Инициализация базовых ролей в базе данных...");

            await roleManager.CreateAsync(new Role { Name = adminRoleName, SecurityLvl = 3 });
            await roleManager.CreateAsync(new Role { Name = moderRoleName, SecurityLvl = 1 });
            await roleManager.CreateAsync(new Role { Name = userRoleName, SecurityLvl = 0 });

            logger.LogInformation("Базовые роли успешно созданы");
        }

        const string testUserName = "Test";

        // Проверка и создание тестовых пользователей
        if (await userManager.FindByNameAsync(testUserName) == null)
        {
            logger.LogInformation("Создание тестовых учетных записей...");

            var testUsers = new[]
            {
                (VM: new UserRegisterViewModel { UserName = "Test", Email = "Test@gmail.com", Password = "1234aB", FirstName = "Test", LastName = "Testov" }, Role: userRoleName),
                (VM: new UserRegisterViewModel { UserName = "Test2", Email = "Test2@gmail.com", Password = "12343aB", FirstName = "Test2", LastName = "Testov2" }, Role: moderRoleName),
                (VM: new UserRegisterViewModel { UserName = "Test3", Email = "Test3@gmail.com", Password = "12342aB", FirstName = "Test3", LastName = "Testov3" }, Role: adminRoleName)
            };

            foreach (var item in testUsers)
            {
                var user = mapper.Map<User>(item.VM);
                var result = await userManager.CreateAsync(user, item.VM.Password ?? string.Empty);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, item.Role);
                    // Совет 5: Логирование с параметрами
                    logger.LogInformation("Создан тестовый пользователь {UserName} с ролью {Role}", user.UserName, item.Role);
                }
            }
        }
    }
}