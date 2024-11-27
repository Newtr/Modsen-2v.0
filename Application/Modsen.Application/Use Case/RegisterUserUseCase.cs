using Modsen.Domain;
using Modsen.DTO;

namespace Modsen.Application
{
public class RegisterUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Execute(UserRegistrationDto registrationDto)
    {
        if (await _unitOfWork.UserRepository.AnyAsync(registrationDto.Email))
        {
            throw new BadRequestException("User with this email already exists.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);

        var newUser = new User
        {
            Username = registrationDto.Username,
            Email = registrationDto.Email,
            PasswordHash = passwordHash,
            RoleId = 1,
            RefreshToken = null,
            RefreshTokenExpiryTime = DateTime.UtcNow
        };

        await _unitOfWork.UserRepository.AddAsync(newUser);
        await _unitOfWork.CommitAsync();

        return true;
    }
}

}