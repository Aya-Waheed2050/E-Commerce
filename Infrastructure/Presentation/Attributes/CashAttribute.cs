using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Attributes
{
    internal class CashAttribute(int DurationInSec =90) : ActionFilterAttribute
    {

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string cashKey = CreateCashKey(context.HttpContext.Request);

            ICashService cashService = context.HttpContext.RequestServices.GetRequiredService<ICashService>();
            var cashValue = await cashService.GetAsync(cashKey);

            if(cashValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cashValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var executedContext = await next.Invoke();
            if (executedContext.Result is ObjectResult Result)
            {
                await cashService.SetAsync(cashKey, Result.Value, TimeSpan.FromMinutes(DurationInSec));
            }

        }


        private string CreateCashKey(HttpRequest request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append(request.Path + '?');
            foreach (var item in request.Query.OrderBy(o => o.Key))
            {
                Key.Append($"{item.Key}={item.Value}&");
            }
            return Key.ToString();
        }
    }

}
