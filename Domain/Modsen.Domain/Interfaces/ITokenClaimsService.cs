namespace Modsen.Domain
{
public interface ITokenClaimsService
{
    (string AccessToken, string RefreshToken) GenerateTokens(User user, Member member);
    string GenerateAccessToken(User user);
}
}