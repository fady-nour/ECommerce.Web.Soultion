using AutoMapper;
using EComerce.Shared;
using EComerce.Shared.DTOS.ProductDtos;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.ServiceAbstraction;
using ECommerce.Services.Exceptions;
using ECommerce.Services.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService( IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
           var Brands = await _unitOfWork.GetRepositoryAsync<ProductBrand,int>().GetAllAsync();  
            return _mapper.Map<IEnumerable<BrandDTO>>(Brands);
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            
           var spc = new ProductWithBrandTypeSpecification(queryParams);
            var Products = await _unitOfWork.GetRepositoryAsync<Product,int>().GetAllAsync(spc);
            var DataToReturn= _mapper.Map<IEnumerable<ProductDTO>>(Products);
            var CounToReturn = DataToReturn.Count();
            var CountSpec = new ProductCountSpecification(queryParams);
            var countOfProducts = await _unitOfWork.GetRepositoryAsync<Product,int>().CountAsync(CountSpec);
            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, CounToReturn, countOfProducts, DataToReturn);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types= await _unitOfWork.GetRepositoryAsync<ProductType,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(Types);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var spc = new ProductWithBrandTypeSpecification(id);
            var Product = await _unitOfWork.GetRepositoryAsync<Product,int>().GetByIdAsync(spc);
            if(Product == null)
            {
                throw new ProductNotFoundException(id);
              
            }
            
            return _mapper.Map<ProductDTO>(Product);
        }
    }
}
