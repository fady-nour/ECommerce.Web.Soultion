using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.DbContexts;
using ECommerce.Persistence.IdentityData.Dbcontexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Web.Extensions
{
    public static class WebAppRegistration
    {
        public static async Task<WebApplication> MigrateDbAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var dbcontextServices = Scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var pendingMigrations = await dbcontextServices.Database.GetPendingMigrationsAsync();
             
            if (pendingMigrations.Any())
                await dbcontextServices.Database.MigrateAsync();
            return app;
        }
        public static async Task<WebApplication> MigrateIdentityDbAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var dbcontextServices = Scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();
            var pendingMigrations = await dbcontextServices.Database.GetPendingMigrationsAsync();
             
            if (pendingMigrations.Any())
                await dbcontextServices.Database.MigrateAsync();
            return app;
        }
        public async static Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
           using var Scope = app.Services.CreateAsyncScope();
            var DataIntializerService = Scope.ServiceProvider.GetRequiredKeyedService<IDataInitialize>("Default");
            await DataIntializerService.InitializeAsync();
            return app;
        }
        public async static Task<WebApplication> SeedIdentityDbAsync(this WebApplication app)
        {
           using var Scope = app.Services.CreateAsyncScope();
            var DataIntializerService = Scope.ServiceProvider.GetRequiredKeyedService<IDataInitialize>("Identity");
            await DataIntializerService.InitializeAsync();
            return app;
        }
    }
}
