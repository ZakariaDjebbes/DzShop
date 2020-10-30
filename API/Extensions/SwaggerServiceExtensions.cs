using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
	public static class SwaggerServiceExtensions
	{
		public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
		{
			services.AddSwaggerGen((options) =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "MedSupply API",
					Version = "V1"
				});
			});

			return services;
		}

		public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI((options) =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "MedSupply API v1");
			});

			return app;
		}
	}
}