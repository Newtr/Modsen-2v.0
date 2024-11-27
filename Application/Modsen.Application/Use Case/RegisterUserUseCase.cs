using Modsen.Domain;
using Modsen.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
    public class RegisterUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(UserRegistrationDto registrationDto, CancellationToken cancellationToken = default)
        {
            if (await _unitOfWork.UserRepository.AnyAsync(registrationDto.Email, cancellationToken))
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

            await _unitOfWork.UserRepository.AddAsync(newUser, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
    }
}
