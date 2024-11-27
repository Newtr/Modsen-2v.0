using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Modsen.Domain;
using Modsen.DTO;

namespace Modsen.Application
{
    public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
            RefreshTokenExpiryTime = DateTime.Now
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

        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
        await _unitOfWork.CommitAsync();

        return (true, accessToken, refreshToken);
    }


    public async Task<string> RefreshToken(string refreshToken)
    {
        var user = await _unitOfWork.UserRepository.GetByRefreshTokenAsync(refreshToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new UnauthorizedException("Invalid or expired refresh token.");
        }

        return GenerateAccessToken(user);
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


    private string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Yhsfddbqyajsdismasdpfwuaxjdfreqsadadyur"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "Modsen-2v.0",
            audience: "User",
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token); 
    }
   private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber); 
    }
}

}