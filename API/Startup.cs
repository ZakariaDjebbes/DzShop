using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace API
{
	public class Startup
	{
		private readonly IConfiguration _config;

		public Startup(IConfiguration config)
		{
			_config = config;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddCors(opt =>
			{
				opt.AddPolicy("CorsPolicy", policy =>
				{
					policy
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials()
					.WithOrigins("https://localhost:4200");
				});
			});
			services.AddAutoMapper(typeof(MappingProfiles));
			services.AddDbContext<StoreContext>(ctx =>
			{
				ctx.UseSqlite(_config.GetConnectionString("DefaultConnection"));
			});
			services.AddDbContext<AppIdentityDbContext>(ctx =>
			{
				ctx.UseSqlite(_config.GetConnectionString("IdentityConnection"));
			});
			services.AddSingleton<IConnectionMultiplexer>(c => {
				var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
				return ConnectionMultiplexer.Connect(configuration);
			});

			services.AddSwaggerDocumentation();
			services.AddApplicationServices();
			services.AddIdentityService(_config);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseMiddleware<ExceptionMiddleware>();
			app.UseStatusCodePagesWithReExecute("/error/{0}");
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseStaticFiles();
			app.UseCors("CorsPolicy");
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseSwaggerDocumentation();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}