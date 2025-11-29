using EComerce.Shared.CommonResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleProblem(result.Errors);

        }
        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if(result.IsSuccess)
                return Ok(result.Value);
            else 
                return HandleProblem(result.Errors);
        }
        protected string GetEmailFromToken()
        {
            return User.FindFirstValue(ClaimTypes.Email)!;
        }
        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            //if no errors => return default error 
            //if there is one error => handle it 
            // if there is  more one error  =>handle as validation 

            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error",
                    detail: "Unexpected Error Ocuured !");

            if (errors.All(o => o.Type == ErrorType.Validation))
                return HandleValidationProblem(errors);

            return HandleSingleErrorProblem(errors[0]);
        }
        private ActionResult HandleSingleErrorProblem(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.Type.ToString(),
                statusCode: MapErrorTypeToStatusCode(error.Type));
                

        }
        private static int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation=>StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized=>StatusCodes.Status401Unauthorized,
           ErrorType.Forbidden=>StatusCodes.Status403Forbidden,
            ErrorType.InvalidCredentails=>StatusCodes.Status401Unauthorized,
            ErrorType.Failure=>StatusCodes.Status500InternalServerError,
            _=>StatusCodes.Status500InternalServerError

        };
        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var ModelState = new ModelStateDictionary();
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(ModelState);
        }
    } 
}
