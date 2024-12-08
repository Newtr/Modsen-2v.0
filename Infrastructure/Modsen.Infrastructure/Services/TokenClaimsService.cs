using System.Security.Claims;
using Modsen.Domain;

namespace Modsen.Infrastructure
{
public class TokenClaimsService : ITokenClaimsService
{
    private readonly ITokenService _tokenService;

    public TokenClaimsService(ITokenService tokenService)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    public (string AccessToken, string RefreshToken) GenerateTokens(User user, Member member)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleName),
            new Claim("MemberId", member.Id.ToString())
        };

        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        return (accessToken, refreshToken);
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        };

        return _tokenService.GenerateAccessToken(claims);
    }
}
}