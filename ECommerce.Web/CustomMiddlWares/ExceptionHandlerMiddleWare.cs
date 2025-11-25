using ECommerce.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerce.Web.CustomMiddlWares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate Next,ILogger<ExceptionHandlerMiddleWare> logger) 
        {
            _next = Next;
            _logger = logger;
        } 
        public async Task InvokeAsync(HttpContext context)
        {
   

			try
            {
                await _next.Invoke(context);
                await HandleNotFoundEndPointAsync(context);
            }
            catch (Exception ex)
			{
                _logger.LogError(ex,"Something went wrong ");
                
                var Problem = new ProblemDetails()
                {
                    Title = "An Unexpected Error Ocuured!",
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                     Status =ex switch
                     {
                         NotFoundException => StatusCodes.Status404NotFound,
                         _=>StatusCodes.Status500InternalServerError

                     }
                };
                context.Response.StatusCode = Problem.Status.Value;
                await context.Response.WriteAsJsonAsync(Problem);


            }
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Problem = new ProblemDetails()
                {

                    Title = "Resource Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "The Resource You Are Looking For Is Not Found",
                    Instance = context.Request.Path
                };
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }
    }
}
