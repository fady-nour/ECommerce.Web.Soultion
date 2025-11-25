using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class IdentityDataIntializer : IDataInitialize
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataIntializer> _logger;

        public IdentityDataIntializer(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager ,ILogger<IdentityDataIntializer> logger )
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._logger = logger;
        }
        public async Task InitializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any()) 
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                
                
                }
                if (!_userManager.Users.Any()) 
                {
                    var User01 = new ApplicationUser()
                    {
                        DisplayName = "Fady Nour",
                        UserName = "FadyNour",
                        Email = "fadynour194@gmail.com",
                        PhoneNumber = "01066762584"
                    };
                    var User02 = new ApplicationUser()
                    {
                        DisplayName = "Koko ll",
                        UserName = "Kokoll",
                        Email = "Kokoll@gmail.com",
                        PhoneNumber = "01280859833"
                    };
                   await _userManager.CreateAsync(User01 , "P@ssw0rd");
                   await _userManager.CreateAsync(User02 , "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");

                
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Ocuured During Seeding Identity Data:{ex.Message}" );

              
            }
        }
    }
}
