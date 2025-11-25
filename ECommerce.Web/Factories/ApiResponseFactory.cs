using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {
            var Errors = actionContext.ModelState.Where(x => x.Value.Errors.Count > 0)
                                                                     .ToDictionary(o => o.Key, o => o.Value.Errors
                                                                     .Select(o => o.ErrorMessage).ToArray());
            var Problem = new ProblemDetails
            {
                Title = "Validation Error",
                Detail = "One or More Validatiion error Occured !",
                Status = StatusCodes.Status400BadRequest,
                Extensions =
                        {
                            {"Error",Errors }

                        }
            };
            return new BadRequestObjectResult(Problem);
        }

    }
}
