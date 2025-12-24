using Blog2.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog2.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            // Берем строку подключения
            //string connection = configuration.GetConnectionString("DefaultConnection");
            string connection = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            // Настраиваем DbContext
            services.AddDbContext<Blog2DbContext>(options =>
                options.UseSqlServer(connection));

            // Настраиваем Identity
            services.AddIdentity<User, Role>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<Blog2DbContext>();

            return services;
        }
    }
}
