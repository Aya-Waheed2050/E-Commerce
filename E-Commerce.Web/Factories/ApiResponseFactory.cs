using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.Web.Factories
{
    static public class ApiResponseFactory
    {

        static public IActionResult GenerateApiValidationErrorsResponse(ActionContext context)
        {
            var errors = context.ModelState.Where(m => m.Value.Errors.Any())
                    .Select(m => new ValidationError()
                    {
                        Errors = m.Value.Errors.Select(e => e.ErrorMessage),
                        Field = m.Key
                    });
            var response = new ValidationErrorToReturn()
            {
                ValidationErrors = errors,
            };
            return new BadRequestObjectResult(response);
        }

    }
}
