using System.Security.Claims;
using Modsen.Domain;

namespace Modsen.Application
{
public class RefreshTokenUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenClaimsService _tokenClaimsService;

    public RefreshTokenUseCase(IUnitOfWork unitOfWork, ITokenClaimsService tokenClaimsService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _tokenClaimsService = tokenClaimsService ?? throw new ArgumentNullException(nameof(tokenClaimsService));
    }

    public async Task<string> Execute(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid or expired refresh token.");
        }

        return _tokenClaimsService.GenerateAccessToken(user);
    }
}

}