using Back.Interfaces;
using Back.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Back.Miscellaneous
{
	public sealed class JWTTokenHelper
	{
		public static JWTTokenHelper tokenHelper;

		public static JWTTokenHelper GetInstance() 
		{
			if(tokenHelper == null) 
			{
				tokenHelper = new JWTTokenHelper();
			}

			return tokenHelper;
		}
		public string GetJWTToken(Person person, string secretKey, IEnumerable<Claim> claims = null)
		{
			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var tokenOptions = new JwtSecurityToken(
				issuer: "http://localhost:5000",
				claims: claims,
				expires: DateTime.Now.AddMinutes(60),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
				);

			string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
			return token;
		}
		public string GetEmail(string tokenString) 
		{
			JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
			Claim claim = token.Claims.Where(x => x.Type == "email").First();
			return claim.Value;
		}
	}
}
