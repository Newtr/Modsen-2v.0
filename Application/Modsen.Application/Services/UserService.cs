using Modsen.Domain;
using Modsen.DTO;

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

    public Task RegisterUser(UserRegistrationDto registrationDto, CancellationToken cancellationToken)
    {
        return _registerUserUseCase.Execute(registrationDto, cancellationToken);
    }

    public Task<(bool isValid, string accessToken, string refreshToken)> LoginUser(UserLoginDto loginDto, CancellationToken cancellationToken)
    {
        return _loginUserUseCase.Execute(loginDto, cancellationToken);
    }

    public Task<IEnumerable<User>> GetAllUsers(int page, int pageSize, CancellationToken cancellationToken)
    {
        return _getAllUsersUseCase.Execute(page, pageSize, cancellationToken);
    }

    public Task<string> RefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        return _refreshTokenUseCase.Execute(refreshToken, cancellationToken);
    }
}

}
