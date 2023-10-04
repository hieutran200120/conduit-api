using ConduitAPI.Infrastructure.Exceptions;

namespace ConduitAPI.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if(ex is RestException)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)ex.Data[RestException.STATUS_CODE];
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Code = context.Response.StatusCode,
                        Message = ex.Message,
                    });
                } else
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Code = context.Response.StatusCode,
                        Message = "Internal Server Error",
                    });
                }
            }
            
        }
    }
}
