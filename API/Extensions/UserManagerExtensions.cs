﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
	public static class UserManagerExtensions
	{
		public static async Task<AppUser> FindByClaimsWithAddressAsync(this UserManager<AppUser> userManager,
			ClaimsPrincipal claimsPrincipal)
		{
			var email = claimsPrincipal?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

			return await userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
		}

		public static async Task<AppUser> FindByClaimsAsync(this UserManager<AppUser> userManager,
			ClaimsPrincipal claimsPrincipal)
		{
			var email = claimsPrincipal?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

			return await userManager.Users.SingleOrDefaultAsync(x => x.Email == email);
		}
	}
}
