using AutoMapper;
using EComerce.Shared.DTOS.OrderDTO;
using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTO, OrderAddress>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ForMember(D=>D.ProductName
            ,O=>O.MapFrom(S=>S.ItemOrdered.ProductName)).ForMember(D=>D.PictureUrl,p=>p.MapFrom(S=>S.ItemOrdered.PictureUrl));
            CreateMap<Order, OrderToReturnDTO>().
                ForMember(D=>D.DeliveryMethod,p=>p.MapFrom(S=>S.DeliveryMethod.ShortName));
            CreateMap<DeliveryMethod, DeliveryMethodDTO>();


        }
    }
}
