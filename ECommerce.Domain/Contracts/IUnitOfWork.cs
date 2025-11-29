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
        Task<int> SaveChangeAsync();

        IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>()where TEntity : BaseEntity<Tkey>;

    }
}
