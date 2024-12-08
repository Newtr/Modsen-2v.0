using FluentValidation;
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
    private readonly IValidator<UserLoginDto> _validator;

    public LoginUserUseCase(
        IUnitOfWork unitOfWork,
        ITokenClaimsService tokenClaimsService,
        IPasswordHashingService passwordHashingService,
        IMemberRepository memberRepository,
        IValidator<UserLoginDto> validator)
    {
        _unitOfWork = unitOfWork;
        _tokenClaimsService = tokenClaimsService;
        _passwordHashingService = passwordHashingService;
        _memberRepository = memberRepository;
        _validator = validator;
    }

    public async Task<(bool IsValid, string AccessToken, string RefreshToken)> Execute(UserLoginDto loginDto, CancellationToken cancellationToken = default)
    {
        // Валидация DTO
        var validationResult = await _validator.ValidateAsync(loginDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginDto.Email, cancellationToken);
        if (user == null || !_passwordHashingService.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return (false, null, null);
        }

        var (accessToken, refreshToken) = _tokenClaimsService.GenerateTokens(user);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
        await _unitOfWork.CommitAsync(cancellationToken);

        return (true, accessToken, refreshToken);
    }
}

}
