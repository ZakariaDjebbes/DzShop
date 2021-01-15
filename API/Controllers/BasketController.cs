using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository, IMapper mapper)
		{
			_basketRepository = basketRepository;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<CustomerBasket>> GetBasketById([Required] string id)
		{
			var basket = await _basketRepository.GetBasketAsync(id);

			return Ok(basket ?? new CustomerBasket(id));
		}

		[HttpPost]
		public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto customerBasket)
		{
			var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(customerBasket);
			var basket = await _basketRepository.CreateOrUpdateBasketAsync(mappedBasket);

			return Ok(basket);
		}

		[HttpDelete]
		public async Task DeleteBasket([Required] string id)
		{
			await _basketRepository.DeleteBasketAsync(id);
		}
	}
}
