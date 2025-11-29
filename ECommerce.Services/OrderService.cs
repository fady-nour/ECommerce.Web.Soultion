using AutoMapper;
using EComerce.Shared.CommonResult;
using EComerce.Shared.DTOS.OrderDTO;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.ServiceAbstraction;
using ECommerce.Services.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IMapper mapper,IUnitOfWork unitOfWork,IBasketRepository basketRepository)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            this._basketRepository = basketRepository;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email)
        {
            //MAP DTO aDDRESS eNTITYorder
            var OrderAddress = _mapper.Map<OrderAddress>(orderDTO.address);
            var Basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if(Basket == null) return Error.NotFound("Basket Not Found!");
            List<OrderItem> OrderItems = new List<OrderItem>();
            foreach (var item in Basket.Items)
            {
                var Product = await _unitOfWork.GetRepositoryAsync<Product, int>().GetByIdAsync(item.Id);
                if (Product == null) return Error.NotFound($"Product with id {item.Id} not found!");
                OrderItems.Add(CrateOrderItem(item, Product));

            }
            var DeliveryMethod = await _unitOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if(DeliveryMethod is null)
            return Error.NotFound($"Delivery Method with id {orderDTO.DeliveryMethodId} not found!");
            var Subtotal = OrderItems.Sum(item => item.Price * item.Quantity);
            var Order =new Order()
            {
                UserEmail = Email,
                Address = OrderAddress,
                DeliveryMethod = DeliveryMethod,
                Subtotal = Subtotal,
                Items = OrderItems,

            };
            await _unitOfWork.GetRepositoryAsync<Order,Guid>().AddAsync(Order);
           int Result = await _unitOfWork.SaveChangeAsync();
            if(Result == 0) return Error.Failure("Failed to create order!");
           return _mapper.Map<OrderToReturnDTO>(Order);
        }

        private static OrderItem CrateOrderItem(BasketItem item, Product Product)
        {
            return new OrderItem()
            {
                ItemOrdered = new ProductItemOrdered()
                {
                    ProductId = Product.Id,
                    ProductName = Product.Name,
                    PictureUrl = Product.PictureUrl
                },
                Price = Product.Price,
                Quantity = item.Quantity
            };
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string Email)
        {
            var Spec =new OrderSpecification(Email);
            var Orders = await _unitOfWork.GetRepositoryAsync<Order, Guid>().GetAllAsync(Spec);
            if(!Orders.Any())
            {
                return Error.NotFound("No orders found for this user!");
            }
            var Data = _mapper.Map<IEnumerable<OrderToReturnDTO>>(Orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(Data);
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
          var DeliveryMethods = await _unitOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetAllAsync();
          if (!DeliveryMethods.Any())
          {
              return Error.NotFound("No delivery methods found!");
          }
          var Data = _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDTO>>(DeliveryMethods);
          return Result<IEnumerable<DeliveryMethodDTO>>.Ok(Data);

        }

        public async Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string Email)
        {
            var Spec = new OrderSpecification(id, Email);
            var Order = await _unitOfWork.GetRepositoryAsync<Order, Guid>().GetByIdAsync(Spec);
            if (Order == null)
            {
                return Error.NotFound("Order not found!");
            }
           return _mapper.Map<OrderToReturnDTO>(Order);
           

        }

    }
}
