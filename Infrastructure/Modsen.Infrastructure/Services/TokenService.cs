using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Modsen.Infrastructure
{
    public class TokenService
    {
        private readonly GenerateAccessTokenUseCase _generateAccessTokenUseCase;
        private readonly GenerateRefreshTokenUseCase _generateRefreshTokenUseCase;

        public TokenService(
            GenerateAccessTokenUseCase generateAccessTokenUseCase,
            GenerateRefreshTokenUseCase generateRefreshTokenUseCase)
        {
            _generateAccessTokenUseCase = generateAccessTokenUseCase;
            _generateRefreshTokenUseCase = generateRefreshTokenUseCase;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            return _generateAccessTokenUseCase.Execute(claims);
        }

        public string GenerateRefreshToken()
        {
            return _generateRefreshTokenUseCase.Execute();
        }
    }
}