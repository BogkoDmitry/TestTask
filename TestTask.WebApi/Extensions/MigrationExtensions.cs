using Microsoft.EntityFrameworkCore;
using TestTask.WebApi.Infrastructure;

namespace TestTask.WebApi.Extensions
{
    public static class MigrationExtensions
    {
        public static void AddMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<TestTaskDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
