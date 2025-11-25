
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Persistence.Data.DataSeed;
using ECommerce.Persistence.Data.DbContexts;
using ECommerce.Persistence.Data.Repositories;
using ECommerce.Persistence.IdentityData.Dbcontexts;
using ECommerce.ServiceAbstraction;
using ECommerce.Services;
using ECommerce.Services.MappingProfiles;
using ECommerce.Web.CustomMiddlWares;
using ECommerce.Web.Extensions;
using ECommerce.Web.Factories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace ECommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddKeyedScoped<IDataInitialize, DataInitialize>("Default");
            builder.Services.AddKeyedScoped<IDataInitialize, IdentityDataIntializer>("Identity");
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddAutoMapper(p => p.AddProfile<ProductProfile>());
         
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(O => 
            { return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!); });
              builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddAutoMapper(p => p.AddProfile<BasketProfile>());
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.Configure<ApiBehaviorOptions>(options=>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
              
            });
            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
                //Add-Migration "IdentityTablesCreate" -OutDir "IdentityData/Migrations" -Context "StoredIdentityDbcontext"
            });
            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<StoreIdentityDbContext>();

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            var app = builder.Build();

            #region DataSeeding

           await app.MigrateDbAsync();
            await app.MigrateIdentityDbAsync();
            await app.SeedDbAsync();
            await app.SeedIdentityDbAsync();


            #endregion
            app.UseMiddleware<ExceptionHandlerMiddleWare>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
