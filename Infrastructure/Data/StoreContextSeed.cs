using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        private const string PRODUCTS_BRANDS_PATH = "../Infrastructure/Data/SeedData/brands.json";
        private const string PRODUCTS_TYPES_PATH = "../Infrastructure/Data/SeedData/types.json";
        private const string PRODUCTS_PATH = "../Infrastructure/Data/SeedData/products.json";
        private const string DELIVERY_METHOD_PATH = "../Infrastructure/Data/SeedData/delivery.json";

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
		public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory, UserManager<AppUser> userManager)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText(PRODUCTS_BRANDS_PATH);
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach (var brand in brands)
                    {
                        context.ProductBrands.Add(brand);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText(PRODUCTS_TYPES_PATH);
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach (var type in types)
                    {
                        context.ProductTypes.Add(type);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText(PRODUCTS_PATH);
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    foreach (var product in products)
                    {
                        context.Products.Add(product);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.DeliveryMethods.Any())
                {
                    var dmData = File.ReadAllText(DELIVERY_METHOD_PATH);
                    var dms = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

                    foreach (var dm in dms)
                    {
                        context.DeliveryMethods.Add(dm);
                    }

                    await context.SaveChangesAsync();
                }

                if (!userManager.Users.Any())
                {
                    var user = new AppUser
                    {
                        UserName = "Zakaria",
                        Email = "orochi255@hotmail.fr",
                        Address = new Core.Entities.Identity.Address
                        {
                            FirstName = "Zakaria",
                            LastName = "Djebbes",
                            Street = "250 Logement",
                            City = "Constantine",
                            ZipCode = "12345",
                            Country = "Algeria"
                        }
                    };

                    await userManager.CreateAsync(user, "zakaria159");
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex, "An error occured while seeding data");
            }
        }
    }
}