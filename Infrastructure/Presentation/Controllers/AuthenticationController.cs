namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager _serviceManager) : ApiBaseController
    {

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
         => Ok(await _serviceManager.authenticationService.LoginAsync(login));


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
         => Ok(await _serviceManager.authenticationService.RegisterAsync(register));

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
         => Ok(await _serviceManager.authenticationService.CheckEmailAsync(email));

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var appUser = await _serviceManager.authenticationService.GetCurrentUserAsync(GetEmailFromToken());
            return Ok(appUser);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var address = await _serviceManager.authenticationService.GetUserAddressAsync(GetEmailFromToken());
            return Ok(address);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto addressDto)
        {
            var Updatedaddress = await _serviceManager.authenticationService.UpdateCurrentUserAddressAsync(GetEmailFromToken(), addressDto);
            return Ok(Updatedaddress);
        }


    }


}
