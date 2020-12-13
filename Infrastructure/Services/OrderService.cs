using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Order;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            //get the basket
            var basket = await _basketRepository.GetBasketAsync(basketId);
           
            //get the items
            List<OrderItem> items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {                
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, product.Price, item.Quantity);
                items.Add(orderItem);
            }
           
            //get the delivery method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
           
            //subtotal
            var subtotal = items.Sum(i => i.Price * i.Quantity);
           
            //create order
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);
            _unitOfWork.Repository<Order>().Add(order);
           
            //save order to db
            var results = await _unitOfWork.Complete();
            if(results <= 0)
            {
                return null;
            }    

            //delete basket
            await _basketRepository.DeleteBasketAsync(basketId);

            //return order
            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return _unitOfWork.Repository<DeliveryMethod>().GetListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithItemsAndMethodSpecification(id, buyerEmail);
            return  await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersOfUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndMethodSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().GetListAllWithSpecAsync(spec);
        }
    }
}