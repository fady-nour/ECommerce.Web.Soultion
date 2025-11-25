using EComerce.Shared.DTOS.BasketDTOs;
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
    [Route("api/[controller]")]
    public class BasketController:ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            this._basketService = basketService;
        }

        #region GETBASKETBYID
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket(string id)
        {
            var Basket = await _basketService.GetBasketAsync(id);
            return Ok(Basket);
        }
        #endregion
        #region Create or update Basket
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basket) 
        {
           var Basket =await _basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }

        #endregion
        #region DeleteBasket
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var Result=await _basketService.DeleteBasketAsync(id);
            return Ok(Result);

        }



        #endregion


    }
}
