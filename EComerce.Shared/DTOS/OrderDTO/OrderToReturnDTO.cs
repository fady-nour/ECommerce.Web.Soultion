using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComerce.Shared.DTOS.OrderDTO
{
    public class OrderToReturnDTO
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public AddressDTO Address { get; set; } = null!;
        public string DeliveryMethod { get; set; } = null!;
        public string OrderStatus { get; set; } = null!;
        public ICollection<OrderItemDTO> Items { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }


    }
}
