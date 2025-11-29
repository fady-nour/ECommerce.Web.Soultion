using AutoMapper;
using EComerce.Shared.DTOS.ProductDtos;
using ECommerce.Domain.Entities.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() {
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest=>dest.ProductBrand , src => src.MapFrom(p=>p.ProductBrands.Name)).
                ForMember(dest=>dest.ProductType , src=>src.MapFrom(p=>p.ProductTypes.Name));
            CreateMap<ProductType, TypeDTO>();
         
        }
    }
}
