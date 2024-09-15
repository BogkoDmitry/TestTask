using Microsoft.EntityFrameworkCore;
using TestTask.WebApi.Entities;

namespace TestTask.WebApi.Infrastructure
{
    public class TestTaskDbContext : DbContext
    {
        public TestTaskDbContext(DbContextOptions<TestTaskDbContext> options) : base(options) { }

        public DbSet<TreeNode> Nodes { get; set; }
        public DbSet<ExceptionReport> Exceptions { get; set; }
    }
}
