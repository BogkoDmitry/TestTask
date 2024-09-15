namespace TestTask.WebApi.Middleware
{
    public class UniqueRequestIdMiddleware
    {
        private readonly RequestDelegate _next;

        public UniqueRequestIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.TraceIdentifier = Guid.NewGuid().ToString();
            await _next(httpContext);
        }
    }
}
