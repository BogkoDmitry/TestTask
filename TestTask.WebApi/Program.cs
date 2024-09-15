using Microsoft.EntityFrameworkCore;
using TestTask.WebApi.Extensions;
using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.Middleware;

namespace TestTask.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TestTaskDbContext>(options =>
                options.UseNpgsql(connection));

            builder.Services.AddControllers();

            builder.Services.AddSwaggerConfig();

            var app = builder.Build();

            app.UseMiddleware<UniqueRequestIdMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.AddMigration();

            app.MapControllers();

            app.Run();
        }
    }
}
