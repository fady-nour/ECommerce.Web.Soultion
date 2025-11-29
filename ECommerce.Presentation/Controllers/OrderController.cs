using EComerce.Shared.DTOS.OrderDTO;
using ECommerce.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class OrderController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        //post api/order
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var Result = await _orderService.CreateOrderAsync(orderDTO, GetEmailFromToken());
            return HandleResult(Result);
        }
        //GetAllOrder 
        //Get : BaseUrl/api/Order
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {
            var Orders = await _orderService.GetAllOrdersAsync(GetEmailFromToken());
            return HandleResult(Orders);


        }
        //GetOrderById
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
        {
            var Order = await _orderService.GetOrderByIdAsync(id, GetEmailFromToken());
            return HandleResult(Order);
        }
        //Get Delivery Methods
        //Get : BaseUrl/api/Order/deliveryMethods
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _orderService.GetDeliveryMethods();
            return HandleResult(DeliveryMethods);
        }
    }
}
