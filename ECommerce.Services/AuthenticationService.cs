using EComerce.Shared.CommonResult;
using EComerce.Shared.DTOS.IdentityDTOS;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.ServiceAbstraction;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
          var User =await _userManager.FindByEmailAsync(loginDTO.Email);
            if (User == null)
                return Error.InvalidCredentials("Invalid Email Or Passwordd");
            var IsPasswordValid=await _userManager.CheckPasswordAsync(User,loginDTO.Password);
            if (IsPasswordValid)
                return Error.InvalidCredentials("Password Invalid");
            return new UserDTO(User.Email!, User.DisplayName, "Token");

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
                return new UserDTO(User.Email, User.DisplayName, "Token");
            return IdentityResult.Errors.Select(E => Error.Validaion(E.Code, E.Description)).ToList();

        }
    }
}
