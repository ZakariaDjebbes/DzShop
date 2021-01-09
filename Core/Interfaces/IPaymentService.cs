using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Order;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntentAsync(string basketId);
        Task<Order> UpdateOrderPaymentSuccessAsync(string paymentIntentId);
        Task<Order> UpdateOrderPaymentFailedAsync(string paymentIntentId);
    }
}