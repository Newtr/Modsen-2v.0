using System.Security.Claims;
using Modsen.Domain;
using Modsen.DTO;

namespace Modsen.Application
{
public class LoginUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService; 
    private readonly IMemberRepository _memberRepository; 

    public LoginUserUseCase(IUnitOfWork unitOfWork, ITokenService tokenService, IMemberRepository memberRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
    }

    public async Task<(bool IsValid, string AccessToken, string RefreshToken)> Execute(UserLoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginDto.Email, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return (false, null, null);
        }

        var member = await _memberRepository.GetMemberByUserIdAsync(user.Id, cancellationToken);
        if (member == null)
        {
            throw new NotFoundException($"Member associated with user ID {user.Id} was not found.");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleName),
            new Claim("MemberId", member.Id.ToString()) 
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
