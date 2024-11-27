using System.Data.Entity;
using System.Security.Claims;
using Modsen.Domain;
using Modsen.DTO;
using Modsen.Infrastructure;

namespace Modsen.Application
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenService _tokenService;

        public UserService(IUnitOfWork unitOfWork, TokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<bool> RegisterUser(UserRegistrationDto registrationDto)
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

        public async Task<(bool IsValid, string AccessToken, string RefreshToken)> LoginUser(UserLoginDto loginDto)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return (false, null, null);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
            await _unitOfWork.CommitAsync();

            return (true, accessToken, refreshToken);
        }

        public async Task<string> RefreshToken(string refreshToken)
        {
            var user = await _unitOfWork.UserRepository.GetByRefreshTokenAsync(refreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Invalid or expired refresh token.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            return _tokenService.GenerateAccessToken(claims);
        }

        public async Task<IEnumerable<User>> GetAllUsers(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                throw new BadRequestException("Page and pageSize must be greater than zero.");
            }

            var usersQuery = await _unitOfWork.UserRepository.GetUsersAsync(page, pageSize);
            var users = await usersQuery
                .AsNoTracking()
                .ToListAsync();

            if (!users.Any())
            {
                throw new NotFoundException("No users found.");
            }

            return users;
        }
    }
}
