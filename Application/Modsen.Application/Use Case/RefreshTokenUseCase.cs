using System.Security.Claims;
using Modsen.Domain;
using Modsen.Infrastructure;

namespace Modsen.Application
{
public class RefreshTokenUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TokenService _tokenService;

    public RefreshTokenUseCase(IUnitOfWork unitOfWork, TokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<string> Execute(string refreshToken)
    {
        var user = await _unitOfWork.UserRepository.GetByRefreshTokenAsync(refreshToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid or expired refresh token.");
        }

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