using AutoMapper;
using Blog2.BLL;
using Blog2.BLL.Services;
using Blog2.BLL.Services.IServices;
using Blog2.DAL;
using Blog2.DAL.Models;
using Blog2.DAL.Repositories;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;


// 1. Инициализируем логгер до создания builder
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // 2. ПОДКЛЮЧАЕМ NLOG 
    builder.Host.UseNLog();

    // Настройка сервисов
    builder.Services.AddControllersWithViews();

    var mapperConfig = new MapperConfiguration((v) => {
        v.AddProfile(new MappingProfile());
    });
    IMapper mapper = mapperConfig.CreateMapper();

    builder.Services.AddDataAccess(builder.Configuration);

    builder.Services
                .AddSingleton(mapper)
                .AddTransient<ICommentRepository, CommentRepository>()
                .AddTransient<ITagRepository, TagRepository>()
                .AddTransient<IPostRepository, PostRepository>()
                .AddTransient<IAccountService, AccountService>()
                .AddTransient<ICommentService, CommentService>()
                .AddTransient<IHomeService, HomeService>()
                .AddTransient<IPostService, PostService>()
                .AddTransient<ITagService, TagService>()
                .AddTransient<IRoleService, RoleService>();

    var app = builder.Build();

    app.UseMiddleware<ExceptionMappingMiddleware>();

    //// Временно выносим это из if/else для проверки 404
    //app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");//    ТЕСТ2
    //app.UseExceptionHandler("/Home/Error");//ТЕСТ2

    // 3. НАСТРОЙКА MIDDLEWARE
    if (app.Environment.IsDevelopment())// ТЕСТ
    { 

        // В режиме разработки используем детальную страницу ошибки
        app.UseDeveloperExceptionPage(); //ТЕСТ

        // Логика инициализации БД (только для Development)
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<Blog2DbContext>();
            await context.Database.MigrateAsync();
            var initializer = services.GetRequiredService<IHomeService>();
            await initializer.GenerateUsers();
        }
    } 
    else
    {
        // В ПРОДАКШНЕ: Глобальный обработчик перенаправляет на страницу "Что-то пошло не так"
        app.UseExceptionHandler("/Home/Error");
        app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

   
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    logger.Info("Приложение успешно запущено");
    app.Run();
}
catch (Exception exception)
{
    // 4. ЛОГИРОВАНИЕ ОШИБОК СТАРТА
    logger.Error(exception, "Приложение остановлено из-за критической ошибки при запуске");
    throw;
}
finally
{
    // 5. ЗАКРЫТИЕ ЛОГГЕРА
    LogManager.Shutdown();
}

