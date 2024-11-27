using System.Security.Claims;
using Modsen.Domain;
using Modsen.DTO;
using Modsen.Infrastructure;

namespace Modsen.Application
{
    public class LoginUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenService _tokenService;

        public LoginUserUseCase(IUnitOfWork unitOfWork, TokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<(bool IsValid, string AccessToken, string RefreshToken)> Execute(UserLoginDto loginDto, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginDto.Email, cancellationToken);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return (false, null, null);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
            await _unitOfWork.CommitAsync(cancellationToken);

            return (true, accessToken, refreshToken);
        }
    }
}
