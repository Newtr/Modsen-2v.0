using System.Security.Claims;

namespace Modsen.Domain
{
public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
}
}