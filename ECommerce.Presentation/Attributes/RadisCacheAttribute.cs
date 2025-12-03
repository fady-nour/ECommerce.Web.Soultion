using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.ServiceAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Presentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationInMin;
        public RedisCacheAttribute(int DurationInMin = 5)
        {
            _durationInMin = DurationInMin;
        }


        // Executed Asynchronously before , after Endpoint
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get Cache Service
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            // Check If Cached Data Exist ?
            // Create CacheKey
            var cacheKey = CreateCacheKey(context.HttpContext.Request);

            // If Exist =>
            var CacheValue = await cacheService.GetAsync(cacheKey);
            if (CacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = CacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            // If Not Exist => Continue Executing the endpoint
            var ExecutedContext = await next.Invoke();

            if (ExecutedContext.Result is OkObjectResult okObjectResult)
            {
                // Call SetAsync to Cache the data
                await cacheService.SetAsync(cacheKey, okObjectResult.Value!, TimeSpan.FromMinutes(5));
            }
        }
        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder Key = new StringBuilder(); // api/Products
            Key.Append(request.Path);

            foreach (var item in request.Query.OrderBy(X => X.Key))
            {
                // api/products/brandId=1&typeId=2
                // api/products/typeId=2&brandId=1
                Key.Append($"|{item.Key}-{item.Value}");

            }
            return Key.ToString();
        }
    }
}