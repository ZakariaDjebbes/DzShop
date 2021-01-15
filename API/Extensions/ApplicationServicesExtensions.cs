using System.Linq;
using API.Errors;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddSingleton<IResponseCacheService, ResponseCacheService>();
			services.AddSingleton<IEmailSenderService, EmailSenderService>();
			services.AddScoped<IPaymentService, PaymentService>();
			services.AddScoped<IReviewService, ReviewService>();
			services.AddScoped<IBasketRepository, BasketRepository>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<ITokenService, TokenService>();
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = actionContext =>
				{
					var errors = actionContext.ModelState
					.Where(e => e.Value.Errors.Count > 0)
					.SelectMany(x => x.Value.Errors)
					.Select(x => x.ErrorMessage);

					var errorResponse = new ApiValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(errorResponse);
				};
			});

			return services;
		}
	}
}