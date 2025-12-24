#pragma warning disable CS1591
using Blog2.BLL.Services;
using Blog2.BLL.Services.IServices;
using Blog2.DAL;
using Blog2.DAL.Repositories;
using Blog2.DAL.Repositories.IRepositories;
using Blog2.BLL.ViewModels;
using Blog2.Controllers;
using AutoMapper;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // 1. Настройка подключения к БД (берет строку из appsettings.json проекта API)
                // Мы вызываем метод расширения из Blog2
                builder.Services.AddDataAccess(builder.Configuration);

                // 2. Регистрация AutoMapper (берем профиль маппинга из проекта Blog2)
                builder.Services.AddAutoMapper(typeof(Blog2.BLL.MappingProfile));

                // 3. Регистрация Репозиториев
                builder.Services.AddTransient<ITagRepository, TagRepository>();
                builder.Services.AddTransient<IPostRepository, PostRepository>();
                builder.Services.AddTransient<ICommentRepository, CommentRepository>();

                // 4. Регистрация Сервисов (бизнес-логика)
                builder.Services.AddTransient<ITagService, TagService>();
                builder.Services.AddTransient<IPostService, PostService>();
                builder.Services.AddTransient<IAccountService, AccountService>();
                builder.Services.AddTransient<IRoleService, RoleService>();
                builder.Services.AddTransient<ICommentService, CommentService>();

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                //builder.Services.AddSwaggerGen();

                builder.Services.AddSwaggerGen(options =>
                {
                    // Указываем общую информацию об API
                    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Blog Engine API",
                        Version = "v1",
                        Description = "Документация API для управления блогом (Статьи, Теги, Комментарии, Аккаунты)"
                    });

                    // Настраиваем чтение XML-комментариев
                    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                });


                builder.Services.AddSwaggerGen(c =>
                {
                    // Инструктируем Swagger показывать только те контроллеры, 
                    // которые находятся в пространстве имен вашего нового API
                    c.DocInclusionPredicate((docName, apiDesc) =>
                    {
                        return apiDesc.RelativePath!.StartsWith("api/");
                    });
                });

                var app = builder.Build();

                // Включаем Swagger всегда для тестов, либо оставляем только в Development
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                // Важно: если в API будет авторизация, порядок должен быть таким:
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }

            catch (Exception ex)
            {
                // ЭТО ВАЖНО: Выводим ошибку в консоль, если приложение не смогло даже запуститься
                Console.WriteLine("КРИТИЧЕСКАЯ ОШИБКА ЗАПУСКА:");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Внутренняя ошибка: " + ex.InnerException.Message);

                Console.WriteLine(ex.StackTrace);
                // Не даем окну закрыться сразу
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}
#pragma warning disable CS1591