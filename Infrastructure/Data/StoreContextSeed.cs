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
        private const string USERS_PATH = "../Infrastructure/Data/SeedData/users.json";
        private const string ROLES_PATH = "../Infrastructure/Data/SeedData/roles.json";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
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
                    var userData = File.ReadAllText(USERS_PATH);
                    var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

                    var rolesData = File.ReadAllText(ROLES_PATH);
                    var roles = JsonSerializer.Deserialize<List<AppRole>>(rolesData);

                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }

                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, "passw0rd");
                        await userManager.AddToRoleAsync(user, "Client");

                        if (user.UserName == "Admin")
                            await userManager.AddToRolesAsync(user, new[] { "Administrator", "Moderator" });
                        if (user.UserName == "Moderator")
                            await userManager.AddToRoleAsync(user, "Moderator");

                    }
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