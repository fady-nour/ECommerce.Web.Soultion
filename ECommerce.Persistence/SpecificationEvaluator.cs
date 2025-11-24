using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence
{
    public class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity,TKey>(IQueryable<TEntity> EntryPont,
            ISpecification<TEntity,TKey> specification) where TEntity : BaseEntity<TKey>{
            var Query = EntryPont;
            if (specification != null)
            {
                if(specification.Criteria != null)
                {
                    Query = Query.Where(specification.Criteria);
                }

                if (specification.IncludeExpression != null && specification.IncludeExpression.Any())
                {
                    foreach (var IncludeExp in specification.IncludeExpression)
                    {
                        Query = Query.Include(IncludeExp);

                    }

                }
                //Query = specification.IncludeExpression.Aggregate(Query, (CurrentQuery, IncludeExp)) =>
                //CurrentQuery.Include(IncludeExp);   
                if(specification.OrderBy is not null)
                {
                    Query = Query.OrderBy(specification.OrderBy);
                }
                if(specification.OrderByDescending is not null)
                {
                    Query = Query.OrderByDescending(specification.OrderByDescending);
                }
                if(specification.IsPaginated)
                {
                    Query = Query.Skip(specification.Skip).Take(specification.Take);
                }

            }
            return Query;
        }

    }
}
