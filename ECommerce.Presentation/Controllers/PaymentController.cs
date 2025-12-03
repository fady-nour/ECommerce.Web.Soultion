using EComerce.Shared.DTOS.BasketDTOs;
using ECommerce.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class PaymentController:ApiBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
           _paymentService = paymentService;
        }
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            var Basket = await _paymentService.CreateOrUpdatePaymentIntentAsync(BasketId);
            return Ok(Basket);

        }   
    }
}
