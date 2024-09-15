using System.Net;
using System.Text;
using TestTask.WebApi.Entities;
using TestTask.WebApi.Exceptions;
using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.ResponseModels;

namespace TestTask.WebApi.Middleware
{
    public sealed class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                httpContext.Request.EnableBuffering();
                await _next(httpContext);
            }
            catch (SecureException ex)
            {
                var exceptionReport = await SaveReport(httpContext, ex);

                httpContext.Response.StatusCode = (int)ex.StatusCode;
                var response = CreateExceptionResponse("Secure", exceptionReport.EventId, ex.Message);
                await httpContext.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                var exceptionReport = await SaveReport(httpContext, ex);

                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = CreateExceptionResponse("Exception", exceptionReport.EventId, $"Internal server error ID = {exceptionReport.EventId}");
                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }

        private async Task<ExceptionReport> SaveReport(HttpContext httpContext, Exception ex)
        {
            using var scope = httpContext.RequestServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<TestTaskDbContext>();
            var exceptionReport = await dbContext.Exceptions.AddAsync(new ExceptionReport()
            {
                CreatedAt = DateTimeOffset.UtcNow,
                Text = await CreateExceptionReportMessage(httpContext, ex),
                EventId = httpContext.TraceIdentifier,
            });
            await dbContext.SaveChangesAsync();

            return exceptionReport.Entity;
        }

        private async Task<string> CreateExceptionReportMessage(HttpContext httpContext, Exception ex)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Path: {httpContext.Request.Path}");

            sb.AppendLine("Query: ");
            foreach(var query in httpContext.Request.Query)
            {
                sb.AppendLine($"{query.Key} = {query.Value}");
            }

            sb.AppendLine("Body: ");
            httpContext.Request.Body.Position = 0;
            using (var reader = new StreamReader(httpContext.Request.Body))
            {

                var bodyData = await reader.ReadToEndAsync();
                sb.AppendLine(bodyData);
            }            

            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine($"Stack trace: {ex.StackTrace}");

            return sb.ToString();
        }

        private ExceptionResponse CreateExceptionResponse(string type, string id, string message)
        {
            var response = new ExceptionResponse()
            {
                Type = type,
                Id = id,
                Message = message,
            };

            return response;
        }
    }
}
