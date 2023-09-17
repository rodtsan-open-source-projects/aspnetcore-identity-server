using AspNetCore.Identity.Core.Interfaces;
using AspNetCore.Identity.Infrastructure.ConfigurationSettings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCore.Identity.Infrastructure;
public class TokenProvider : ITokenProvider
{
	private readonly JwtSettings _jwtSettings;
	private readonly byte[] _key;
	public TokenProvider(IOptions<JwtSettings> jwtSettingsOptions)
	{
		_jwtSettings = jwtSettingsOptions.Value;
		_key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
	}

	public string GenerateJwtToken(IList<Claim> claims)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);
		var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresIn);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Issuer = _jwtSettings.Issuer,
			Subject = new ClaimsIdentity(claims),
			Expires = expires,
			SigningCredentials = signingCredentials /* HmacSha256Signature */
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}

	public string GenerateRefreshToken()
	{
		var randomNumber = new byte[32];
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
	}

	public async Task<ClaimsIdentity> GetPrincipalFromExpiredToken(string expiredToken)
	{
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
			ValidateIssuer = false,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(_key),
			ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
		};
		var tokenHandler = new JwtSecurityTokenHandler();
		// SecurityToken securityToken;
		// var token = tokenHandler.ReadJwtToken(expiredToken);
		// var principal = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));
		
		var result = await tokenHandler.ValidateTokenAsync(expiredToken, tokenValidationParameters);
		if (!result.IsValid)
			throw new SecurityTokenException("Invalid token");
		//var jwtSecurityToken = securityToken as JwtSecurityToken;
		//if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
		//	throw new SecurityTokenException("Invalid token");
		return result.ClaimsIdentity; ;
	}
}

