using EComerce.Shared.DTOS.ProductDtos;
using ECommerce.ServiceAbstraction;
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
    public class ProductsController :ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            
            _productService = productService;
        }
        #region GetAllProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProduct()
        {
            var Products = await  _productService.GetAllProductsAsync();
            return Ok(Products);

        }

        #endregion
        #region GetProductById
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var Product = await _productService.GetProductByIdAsync(id);
            return Ok(Product);

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
