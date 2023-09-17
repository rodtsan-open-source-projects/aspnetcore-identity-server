using AspNetCore.Identity.Core.Models;
using System.Security.Claims;

namespace AspNetCore.Identity.Core.Interfaces;
public interface ITokenProvider
{
	string GenerateJwtToken(IList<Claim> claims);
	string GenerateRefreshToken();
	Task<ClaimsIdentity> GetPrincipalFromExpiredToken(string expiredToken);
}
