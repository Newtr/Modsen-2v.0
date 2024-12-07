using System.Security.Claims;
using Modsen.Domain;

namespace Modsen.Application
{
public class RefreshTokenUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService; 

    public RefreshTokenUseCase(IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    public async Task<string> Execute(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);

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