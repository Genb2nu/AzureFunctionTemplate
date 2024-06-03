using Core.Business;
using Core.Business.Services.Implementations;
using Core.Business.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public class BusinessServiceRegister
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<AppDbContext>(options =>
            {
                string connectionString = configuration.GetConnectionString("AppDbContext") ?? "";
                options.UseSqlServer(connectionString, opt =>
                {
                    opt.MigrationsAssembly("Core.Database");
                });
            });

            services.AddScoped<IBlogService, BlogService>();
        }
    }
}
