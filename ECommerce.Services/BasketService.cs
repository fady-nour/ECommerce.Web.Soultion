using AutoMapper;
using EComerce.Shared.DTOS.BasketDTOs;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
            this._basketRepository = basketRepository;
            this._mapper = mapper;
        }
        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var CustomerBasket = _mapper.Map<CustomerBasket>(basket);
            var CreateOrUpdateBasket = await _basketRepository.CreateOrUpdateBasketAsync(CustomerBasket);
            return _mapper.Map<BasketDTO>(CreateOrUpdateBasket);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }

        public async Task<BasketDTO> GetBasketAsync(string id)
        {
          var Basket= await _basketRepository.GetBasketAsync(id);
            return _mapper.Map<BasketDTO>(Basket);
        }
    }
}
