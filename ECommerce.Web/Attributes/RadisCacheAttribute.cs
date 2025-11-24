using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Web.Attributes
{
    public class RadisCacheAttribute :ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }

    }
}
