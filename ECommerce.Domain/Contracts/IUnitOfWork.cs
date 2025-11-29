using ECommerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;
namespace ECommerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity, Tkey> GetRepositoryAsync<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;
        Task<int> SaveChangeAsync();
    }
}
