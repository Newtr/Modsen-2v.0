using Modsen.DTO;

namespace Modsen.Domain
{
    public interface IUserService
    {
        Task<bool> RegisterUser(UserRegistrationDto registrationDto);
        Task<(bool IsValid, string AccessToken, string RefreshToken)> LoginUser(UserLoginDto loginDto);
        Task<string> RefreshToken(string refreshToken);
        Task<IEnumerable<User>> GetAllUsers(int page, int pageSize);
    }
}