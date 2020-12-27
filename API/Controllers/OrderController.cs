using System;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities.Order;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using API.Errors;
using System.Collections.Generic;

namespace API.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User?.GetEmailFromClaims();
            var address = _mapper.Map<AddressDto, Address>(orderDto.shippingAddress);
            
            var order = 
            await _orderService.CreateOrderAsync(email, orderDto.deliveryMethodId, orderDto.basketId, address);

            if(order == null)
                return BadRequest(new ApiResponse(400, "Problem creating order"));
            
            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersOfUser() 
        {
            var email = HttpContext.User?.GetEmailFromClaims();
            var orders = await _orderService.GetOrdersOfUserAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
        {
            var email = HttpContext.User?.GetEmailFromClaims();
            var order = await _orderService.GetOrderByIdAsync(id, email);

            if(order == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }
        
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var methods = await _orderService.GetDeliveryMethodsAsync();

            return Ok(methods);
        }

        [HttpGet("deliveryMethod/{id}")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod(int id)
        {
            var method = await _orderService.GetDeliveryMethodAsync(id);

            return Ok(method);
        }
    }
}