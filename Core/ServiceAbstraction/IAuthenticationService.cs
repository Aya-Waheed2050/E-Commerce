namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {

        Task<UserDto> LoginAsync(LoginDto login);
        Task<UserDto> RegisterAsync(RegisterDto register);
        Task<bool> CheckEmailAsync(string email);
        Task<AddressDto> GetUserAddressAsync(string email);
        Task<AddressDto> UpdateCurrentUserAddressAsync(string email , AddressDto addressDto);
        Task<UserDto> GetCurrentUserAsync(string email);
    }
}
