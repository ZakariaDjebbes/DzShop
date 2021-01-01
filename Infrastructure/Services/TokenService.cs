using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _config;
		private readonly SymmetricSecurityKey _key;

		public TokenService(IConfiguration config)
		{
			_config = config;
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
		}

		public string CreateToken(AppUser user)
		{
			double tokenExpiry = Convert.ToDouble(_config["Token:Expiry"]);

			var claims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
				new Claim(JwtRegisteredClaimNames.NameId, user.Id)
			};

			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDesc = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(tokenExpiry),
				SigningCredentials = creds,
				Issuer = _config["Token:Issuer"]
			};

			var tokenHanlder = new JwtSecurityTokenHandler();
			var token = tokenHanlder.CreateToken(tokenDesc);

			return tokenHanlder.WriteToken(token);
		}
	}
}
