using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity, Tkey> GetRepositoryAsync<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
           var EntityType= typeof(TEntity);
            if(_repositories.TryGetValue(EntityType, out var repository))
            {
                return (IGenericRepository<TEntity, Tkey>)repository;
            }
            var NewRepo= new GenericRepository<TEntity, Tkey>(_dbContext);
            _repositories[EntityType]=NewRepo;
            return NewRepo;
        }

        public async Task<int> SaveChangeAsync()
        =>await _dbContext.SaveChangesAsync();
    }
}
