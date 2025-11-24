using EComerce.Shared;
using ECommerce.Domain.Entities.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class ProductWithBrandTypeSpecification :BaseSpecification<Product,int>
    {
        public ProductWithBrandTypeSpecification(ProductQueryParams queryParams) : base(ProductSpecificationHelper.GetProductCriteria(queryParams))
            { 
            AddInclude(p => p.ProductTypes);
            AddInclude(p=>p.ProductBrands);

            switch (queryParams.Sort) {
            case ProductSortingOptions.NameAsc:
            AddOrderBy(p => p.Name);
            break;
            case ProductSortingOptions.NameDesc:
            AddOrderByDescending(p => p.Name);
            break;
            case ProductSortingOptions.PriceAsc:
            AddOrderBy(p => p.Price);
            break;
            case ProductSortingOptions.PriceDesc:
            AddOrderByDescending(p => p.Price);
            break;
               default:
              AddOrderBy(p=>p.Id);
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        
        }
        public ProductWithBrandTypeSpecification(int id):base(p=>p.Id== id)
        {
         
            AddInclude(p => p.ProductTypes);
            AddInclude(p=>p.ProductBrands);
        
        }

    }
}
