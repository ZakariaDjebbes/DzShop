using System;
using System.Linq.Expressions;
using Core.Entities.Order;

namespace Core.Specifications
{
    public class OrderWithItemsAndMethodSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsAndMethodSpecification(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            SetOrderByDescending(o => o.OrderDate);
        }

        public OrderWithItemsAndMethodSpecification(int id, string email) 
        : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}