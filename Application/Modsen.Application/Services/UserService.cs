using System.Data.Entity;
using System.Security.Claims;
using Modsen.Domain;
using Modsen.DTO;
using Modsen.Infrastructure;

namespace Modsen.Application
{
    public class UserService : IUserService
{
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly LoginUserUseCase _loginUserUseCase;
    private readonly RefreshTokenUseCase _refreshTokenUseCase;
    private readonly GetAllUsersUseCase _getAllUsersUseCase;

    public UserService(
        RegisterUserUseCase registerUserUseCase,
        LoginUserUseCase loginUserUseCase,
        RefreshTokenUseCase refreshTokenUseCase,
        GetAllUsersUseCase getAllUsersUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
        _loginUserUseCase = loginUserUseCase;
        _refreshTokenUseCase = refreshTokenUseCase;
        _getAllUsersUseCase = getAllUsersUseCase;
    }

    public Task<bool> RegisterUser(UserRegistrationDto registrationDto)
    {
        return _registerUserUseCase.Execute(registrationDto);
    }

    public Task<(bool IsValid, string AccessToken, string RefreshToken)> LoginUser(UserLoginDto loginDto)
    {
        return _loginUserUseCase.Execute(loginDto);
    }

    public Task<string> RefreshToken(string refreshToken)
    {
        return _refreshTokenUseCase.Execute(refreshToken);
    }

    public Task<IEnumerable<User>> GetAllUsers(int page, int pageSize)
    {
        return _getAllUsersUseCase.Execute(page, pageSize);
    }
}
}
