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
					Title = "DzShop API",
					Version = "V1"
				});

				var securitySchema = new OpenApiSecurityScheme
				{
					Description = "JWT Auth Bearer scheme",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				};

				options.AddSecurityDefinition("Bearer", securitySchema);

				var securityRequirements = new OpenApiSecurityRequirement
				{
					{
						securitySchema, new [] {"Bearer"}
					}
				};

				options.AddSecurityRequirement(securityRequirements);
			});

			return services;
		}

		public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI((options) =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "DzShop API v1");
			});

			return app;
		}
	}
}