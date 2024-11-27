using Modsen.DTO;

namespace Modsen.Domain
{
public interface IUserService
{
    Task RegisterUser(UserRegistrationDto registrationDto, CancellationToken cancellationToken); 
    Task<(bool isValid, string accessToken, string refreshToken)> LoginUser(UserLoginDto loginDto, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllUsers(int page, int pageSize, CancellationToken cancellationToken);
    Task<string> RefreshToken(string refreshToken, CancellationToken cancellationToken);
}

}