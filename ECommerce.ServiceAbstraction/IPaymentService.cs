using EComerce.Shared.DTOS.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceAbstraction
{
    public interface IPaymentService
    {
        // service [basketid] 
        Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string BasketId);
    }
}
