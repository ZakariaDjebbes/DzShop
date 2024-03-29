using System;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
	public class Program
	{
		public async static Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				var servies = scope.ServiceProvider;
				var loggerFactory = servies.GetRequiredService<ILoggerFactory>();

				try
				{
					var context = servies.GetRequiredService<StoreContext>();
					await context.Database.MigrateAsync();
					await StoreContextSeed.SeedAsync(context, loggerFactory);

					var userManager = servies.GetRequiredService<UserManager<AppUser>>();
					var identityContext = servies.GetRequiredService<AppIdentityDbContext>();
					await identityContext.Database.MigrateAsync();
					await AppIdentityContextSeed.SeedUsersAsync(userManager);
				
				}
				catch(Exception ex)
				{
					var logger = loggerFactory.CreateLogger<Program>();
					logger.LogError(ex, "An error occured during migration");
				}
			}

			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}