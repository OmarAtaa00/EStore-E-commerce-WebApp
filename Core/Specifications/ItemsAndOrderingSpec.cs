using System;
using System.Linq.Expressions;
using Core.OrderAggregate;

namespace Core.Specifications
{
    public class ItemsAndOrderingSpec : BaseSpecification<Order>
    {
        public ItemsAndOrderingSpec(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }


        public ItemsAndOrderingSpec(int id, string email)
            : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
