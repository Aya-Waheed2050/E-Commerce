namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager , IConfiguration _configuration , IMapper _mapper) 
        : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
             var user = await _userManager.FindByEmailAsync(email);
             return user != null;
        }

        public async Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email) ??
                throw new UserNotFoundException(email);
            if(user.Address is not null)
            {
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Country = addressDto.Country;
                user.Address.City = addressDto.City;
                user.Address.Street = addressDto.Street;
            }
            else
            {
                user.Address =  _mapper.Map<AddressDto , Address>(addressDto);
            }

            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException(email);
            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await CreateTokenAsync(user),
            };
        }

        public async Task<AddressDto> GetUserAddressAsync(string email)
        {
            var user = await _userManager.Users.Include(u => u.Address)
                               .FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException(email);
            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        public async Task<UserDto> LoginAsync(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email) ??
                      throw new UserNotFoundException(login.Email);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, login.Password);
            if (isPasswordValid)
            {
                return new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await CreateTokenAsync(user),
                };
            }
            else
              throw new UnAuthorizedException();
        }  

        public async Task<UserDto> RegisterAsync(RegisterDto register)
        {
            var user = new ApplicationUser()
            {
                Email = register.Email,
                UserName = register.UserName,
                DisplayName = register.DisplayName,
                PhoneNumber = register.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user , register.Password);

            if (result.Succeeded)
            {
                return new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await CreateTokenAsync(user),
                };
            }
            else
            {
                var Error = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(Error);
            }

        }
   
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Email , user.Email!),
                new(ClaimTypes.Name , user.UserName!),
                new(ClaimTypes.NameIdentifier , user.Id!),
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWTOptions")["Issuer"],
                audience: _configuration.GetSection("JWTOptions")["Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
}
