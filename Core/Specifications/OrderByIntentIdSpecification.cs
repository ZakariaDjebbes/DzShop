using System;
using System.Linq.Expressions;
using Core.Entities.Order;

namespace Core.Specifications
{
    public class OrderByIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByIntentIdSpecification(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}