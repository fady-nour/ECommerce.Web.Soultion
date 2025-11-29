using EComerce.Shared;
using EComerce.Shared.DTOS.ProductDtos;
using ECommerce.Presentation.Attributes;
using ECommerce.ServiceAbstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            
            _productService = productService;
        }
        #region GetAllProduct
        [Authorize]
        [HttpGet]
        [RedisCache]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProduct([FromQuery]ProductQueryParams queryParams)
        {
            var Products = await  _productService.GetAllProductsAsync(queryParams);
            return Ok(Products);

        }

        #endregion
        #region GetProductById
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
           
                var Result = await _productService.GetProductByIdAsync(id);

            return HandleResult<ProductDTO>(Result);
         

        }

        #endregion
        #region GetAllBrands
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var Brands = await _productService.GetAllBrandsAsync();
            return Ok(Brands);

        }
        #endregion
        #region GetAllTypes
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var Types = await _productService.GetAllTypesAsync();
            return Ok(Types);

        } 
        #endregion


    }
}
