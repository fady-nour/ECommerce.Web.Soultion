using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class DataInitialize : IDataInitialize
    {
        private readonly StoreDbContext _dbcontext;

        public DataInitialize(StoreDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var HasProducts = await _dbcontext.Products.AnyAsync();
                var HasBrands = await _dbcontext.ProductBrands.AnyAsync();
                var HasTypes =await _dbcontext.ProductTypes.AnyAsync();
                var HasDelivery = await _dbcontext.Set<DeliveryMethod>().AnyAsync();

                if (HasProducts && HasBrands && HasTypes && HasDelivery) return;
                if (!HasBrands)
                    await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbcontext.ProductBrands);
                if (!HasTypes)
                    await SeedDataFromJsonAsync<ProductType, int>("types.json", _dbcontext.ProductTypes);
                _dbcontext.SaveChanges();
                if (!HasProducts)
                    await SeedDataFromJsonAsync<Product, int>("products.json", _dbcontext.Products);
                _dbcontext.SaveChanges();
                if (!HasDelivery)
                    await SeedDataFromJsonAsync<DeliveryMethod, int>("delivery.json", _dbcontext.Set<DeliveryMethod>());
                _dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Data Seeding Failed : {ex}");
            }

        }
        private async Task SeedDataFromJsonAsync<T,TKey>(string fileName , DbSet<T> dbset) where T : BaseEntity<TKey>
        {
            //D:\Dot.Net\Dot.net\ECommerce.Web.Soultion\ECommerce.Persistence\Data\DataSeed\JSONFiles\
            var FilePath = @"..\ECommerce.Persistence\Data\DataSeed\JSONFiles\"+fileName;
            if (!File.Exists(FilePath)) throw new FileNotFoundException($"File {fileName} Not Found !");
            try
            {
                using var DataStream = File.OpenRead(FilePath);
                var Data = JsonSerializer.Deserialize<List<T>>(DataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (Data != null)
                {
                 await dbset.AddRangeAsync(Data);                
                
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed To read Data From Jeson  : {ex}");
            }
            

        }
    }
}
