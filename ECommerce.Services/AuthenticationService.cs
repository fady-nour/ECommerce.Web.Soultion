using EComerce.Shared.CommonResult;
using EComerce.Shared.DTOS.IdentityDTOS;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.ServiceAbstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager,IConfiguration configuration)
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
           var User=await  _userManager.FindByEmailAsync(email);
            return User != null;
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null)
                return Error.NotFound("User Not Found");
            return new UserDTO(User.Email!, User.DisplayName, await CreateTokenAsync(User));
           


        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
          var User =await _userManager.FindByEmailAsync(loginDTO.Email);
            if (User == null)
                return Error.InvalidCredentials("Invalid Email Or Passwordd");
            var IsPasswordValid=await _userManager.CheckPasswordAsync(User,loginDTO.Password);
            if (!IsPasswordValid)
                return Error.InvalidCredentials("Password Invalid");
            var Token = await CreateTokenAsync(User);
            return new UserDTO(User.Email!, User.DisplayName, Token);

        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,
            };
            var IdentityResult =await _userManager.CreateAsync(User,registerDTO.Password);
            if (IdentityResult.Succeeded)
            {
                var Token = await CreateTokenAsync(User);
                return new UserDTO(User.Email, User.DisplayName, Token);
            }
            return IdentityResult.Errors.Select(E => Error.Validaion(E.Code, E.Description)).ToList();

        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            //claims ?=>UserEmail ,UserName
            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName !),
            };
            //Roles ?
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim("roles", role));
            }
            // Secretkey ?
            var SecretKey =  _configuration["JWTOptions:SecretKey"]; // This should be stored securely
             var Key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(
                issuer:_configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                expires:DateTime.UtcNow.AddHours(1),
                claims:Claims,
                signingCredentials:Cred);
            return new JwtSecurityTokenHandler().WriteToken(Token);


        }
    }
}
