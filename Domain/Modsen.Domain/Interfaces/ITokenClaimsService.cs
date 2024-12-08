namespace Modsen.Domain
{
public interface ITokenClaimsService
{
    (string AccessToken, string RefreshToken) GenerateTokens(User user);
    string GenerateAccessToken(User user);
}
}