using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComerce.Shared.DTOS.BasketDTOs
{
    public record BasketDTO
    {
        public BasketDTO() { }  // AutoMapper needs this

        public string Id { get; init; }
        public ICollection<BasketItemDTO> Items { get; init; }
        public string ClientSecret { get; init; }
        public string? PaymentIntentId { get; init; }
        public int? DeliveryMethodId { get; init; }
        public decimal? ShoppingPrice { get; init; }
    }

}
