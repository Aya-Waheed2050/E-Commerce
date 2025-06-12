namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public abstract class ApiBaseController : ControllerBase
    {
       
        protected string GetEmailFromToken() => User.FindFirstValue(ClaimTypes.Email)!;

    }
}
