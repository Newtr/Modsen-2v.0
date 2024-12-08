using FluentValidation;
using Modsen.Domain;
using Modsen.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
public class RegisterUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IValidator<UserRegistrationDto> _validator;

    public RegisterUserUseCase(IUnitOfWork unitOfWork, IPasswordHashingService passwordHashingService, IValidator<UserRegistrationDto> validator)
    {
        _unitOfWork = unitOfWork;
        _passwordHashingService = passwordHashingService;
        _validator = validator;
    }

    public async Task<bool> Execute(UserRegistrationDto registrationDto, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(registrationDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        if (await _unitOfWork.UserRepository.AnyAsync(registrationDto.Email, cancellationToken))
        {
            throw new BadRequestException("User with this email already exists.");
        }

        var passwordHash = _passwordHashingService.HashPassword(registrationDto.Password);

        var newUser = new User
        {
            Username = registrationDto.Username,
            Email = registrationDto.Email,
            PasswordHash = passwordHash,
            RoleId = 1,
            RefreshToken = null,
            RefreshTokenExpiryTime = DateTime.UtcNow
        };

        await _unitOfWork.UserRepository.AddAsync(newUser, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}


}
