using EComerce.Shared.CommonResult;
using EComerce.Shared.DTOS.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceAbstraction
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email);
        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string Email);
        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods();
        Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string Email);
    }
}
