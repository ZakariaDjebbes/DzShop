using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Order;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration config)
        {
            this._basketRepository = basketRepository;
            this._unitOfWork = unitOfWork;
            this._config = config;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if(basket == null)
            {
                return null;
            }

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);

                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var opts = new PaymentIntentCreateOptions 
                {
                    Amount = (long)(shippingPrice * 100) + (long)(basket.Items.Sum(i => (i.Price * 100) * i.Quantity)),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(opts);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var opts = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(shippingPrice * 100) + (long)(basket.Items.Sum(i => (i.Price * 100) * i.Quantity)),
                    Currency = "usd",
                };

                await service.UpdateAsync(basket.PaymentIntentId, opts);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return basket;
        }

        public async Task<Core.Entities.Order.Order> UpdateOrderPaymentSuccessAsync(string paymentIntentId)
        {
            var spec = new OrderByIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Core.Entities.Order.Order>().GetEntityWithSpecAsync(spec);

            if (order == null)
            {
                return null;
            }

            order.Status = OrderStatus.PayementReceived;
            _unitOfWork.Repository<Core.Entities.Order.Order>().Update(order);
            
            await _unitOfWork.Complete();
            return order;
        }

        public async Task<Core.Entities.Order.Order> UpdateOrderPaymentFailedAsync(string paymentIntentId)
        {
            var spec = new OrderByIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Core.Entities.Order.Order>().GetEntityWithSpecAsync(spec);

            if (order == null)
            {
                return null;
            }

            order.Status = OrderStatus.PayementFailed;
            
            await _unitOfWork.Complete();
            return order;
        }
    }
}