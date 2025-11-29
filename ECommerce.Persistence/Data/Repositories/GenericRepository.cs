using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TEntity entity)
       =>await _dbContext.Set<TEntity>().AddAsync(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
       => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(int id)
       =>await _dbContext.Set<TEntity>().FindAsync(id);

        public  void Remove(TEntity entity)
      =>  _dbContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity)
       => _dbContext.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, Tkey> specification)
        {
            var Query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification);
            return await Query.ToListAsync();
           
        }

        public async Task<TEntity?> GetByIdAsync(Tkey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }
       

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, Tkey> specification)
        {
           return await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity, Tkey> specification)
        {
            return await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).CountAsync();
        }
    }
}
