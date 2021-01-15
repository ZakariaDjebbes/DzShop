using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Infrastructure.Data.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;
		private readonly IConfiguration _config;

		public BasketRepository(IConnectionMultiplexer redis, IConfiguration config)
		{
			_database = redis.GetDatabase();
			_config = config;
		}

		public async Task<bool> DeleteBasketAsync(string basketId)
		{
			return await _database.KeyDeleteAsync(basketId);
		}

		public async Task<CustomerBasket> GetBasketAsync(string basketId)
		{
			var data = await _database.StringGetAsync(basketId);

			return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data.ToString());
		}

		public async Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket)
		{
			double daysToLive = Convert.ToDouble(_config.GetValue<int>("BasketLifeSpanInDays"));

			var created = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket),
				TimeSpan.FromDays(daysToLive));

			return created ? await GetBasketAsync(basket.Id) : null;
		}
	}
}
