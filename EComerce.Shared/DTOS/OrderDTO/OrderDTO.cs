using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComerce.Shared.DTOS.OrderDTO
{
    public class OrderDTO
    {
        public string BasketId { get; set; } = null!;
        public int DeliveryMethodId { get; set; }
        public AddressDTO address { get; set; } 

    }
}
