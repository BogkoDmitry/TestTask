using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using TestTask.WebApi.Infrastructure;

namespace TestTask.WebApi.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Test task",
                    Description = "Test task",
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.DocumentFilter<TagDescriptionsDocumentFilter>();
                options.EnableAnnotations();
            });
        }

        public sealed class TagDescriptionsDocumentFilter : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                swaggerDoc.Tags = new List<OpenApiTag> {
                new OpenApiTag { Name = Tags.TreeNode, Description = "Represents tree node API" },
                new OpenApiTag { Name = Tags.Tree, Description = "Represents entire tree API" },
                new OpenApiTag { Name = Tags.Journal, Description = "Represents journal API" }
            };
            }
        }
    }
}
