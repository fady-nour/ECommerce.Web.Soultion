using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.DbContexts;
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
        public async static Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
           using var Scope = app.Services.CreateAsyncScope();
            var DataIntializerService = Scope.ServiceProvider.GetRequiredService<IDataInitialize>();
            await DataIntializerService.InitializeAsync();
            return app;
        }
    }
}
