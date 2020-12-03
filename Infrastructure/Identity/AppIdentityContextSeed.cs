using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
	public class AppIdentityContextSeed
	{
		public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new AppUser
				{
					DisplayName = "Zakaria",
					Email = "orochi255@hotmail.fr",
					UserName = "Lanathanel",
					Address = new Address
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
	}
}
