using Modsen.Domain;
using Modsen.DTO;

namespace Modsen.Application
{
public class LoginUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenClaimsService _tokenClaimsService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IMemberRepository _memberRepository;

    public LoginUserUseCase(IUnitOfWork unitOfWork, ITokenClaimsService tokenClaimsService, IPasswordHashingService passwordHashingService, IMemberRepository memberRepository)
    {
        _unitOfWork = unitOfWork;
        _tokenClaimsService = tokenClaimsService;
        _passwordHashingService = passwordHashingService;
        _memberRepository = memberRepository;
    }

    public async Task<(bool IsValid, string AccessToken, string RefreshToken)> Execute(UserLoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginDto.Email, cancellationToken);

        if (user == null || !_passwordHashingService.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return (false, null, null);
        }

        var member = await _memberRepository.GetMemberByUserIdAsync(user.Id, cancellationToken);
        if (member == null)
        {
            throw new NotFoundException($"Member associated with user ID {user.Id} was not found.");
        }

        var (accessToken, refreshToken) = _tokenClaimsService.GenerateTokens(user, member);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
        await _unitOfWork.CommitAsync(cancellationToken);

        return (true, accessToken, refreshToken);
    }
}

}
