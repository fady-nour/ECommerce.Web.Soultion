using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string Email) : base(o => o.UserEmail == Email)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            AddOrderByDescending(o => o.OrderDate);
        }
        public OrderSpecification(Guid id, string Email) : base(o => o.Id == id && (string.IsNullOrEmpty(Email) || o.UserEmail.ToLower() == Email.ToLower()))
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
        }
    }
}
