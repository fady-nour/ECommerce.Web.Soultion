using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIntentSpecification(string IntentId) : base(o=>o.PaymentIntentId == IntentId )
        {


        }
    }
}
