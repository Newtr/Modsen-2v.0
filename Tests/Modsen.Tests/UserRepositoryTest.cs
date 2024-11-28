using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modsen.Application;
using Modsen.Domain;
using Modsen.DTO;
using Modsen.Infrastructure;
using Moq;
using Xunit;

namespace Modsen.Tests
{
    public class UserTests
    {
        private readonly UserRepository _repository;
        private readonly ModsenContext _context;
        private readonly LoginUserUseCase _loginUserUseCase;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenService _tokenService;

        public UserTests()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration
                .Setup(config => config.GetSection("JwtSettings")["Secret"])
                .Returns("supersecretkey");

            mockConfiguration
                .Setup(config => config.GetSection("JwtSettings")["AccessTokenExpiration"])
                .Returns("60");

            mockConfiguration
                .Setup(config => config.GetSection("JwtSettings")["Issuer"])
                .Returns("YourIssuer");

            mockConfiguration
                .Setup(config => config.GetSection("JwtSettings")["Audience"])
                .Returns("YourAudience");

            var generateAccessTokenUseCase = new GenerateAccessTokenUseCase(mockConfiguration.Object);
            var generateRefreshTokenUseCase = new GenerateRefreshTokenUseCase();

            var options = new DbContextOptionsBuilder<ModsenContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ModsenContext(options);
            _repository = new UserRepository(_context);
            _tokenService = new TokenService(generateAccessTokenUseCase, generateRefreshTokenUseCase);
            _unitOfWork = new UnitOfWork(_context);
            _loginUserUseCase = new LoginUserUseCase(_unitOfWork, _tokenService);
        }

        [Fact]
        public async Task RegisterUser_ShouldAddUserToDatabase()
        {
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                RoleId = 1
            };

            // Act
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var savedUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == "test@example.com");

            Console.WriteLine($"Saved user: {savedUser?.Username}, {savedUser?.Email}");

            Assert.NotNull(savedUser);
            Assert.Equal("testuser", savedUser.Username);
            Assert.Equal("test@example.com", savedUser.Email);
        }

        [Fact]
        public async Task RegisterAdmin_ShouldAddAdminToDatabase()
        {
            var admin = new User
            {
                Username = "adminuser",
                Email = "adminuser@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("adminpassword"),
                RoleId = 2
            };

            _context.Users.Add(admin);
            await _context.SaveChangesAsync();

            var savedAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Email == "adminuser@example.com");

            Assert.NotNull(savedAdmin);
            Assert.Equal("adminuser", savedAdmin.Username);
            Assert.Equal(2, savedAdmin.RoleId);
        }


        [Fact]
        public async Task LoginUser_ShouldFail_WhenPasswordIsIncorrect()
        {
            var user = new User
            {
                Username = "wrongpassworduser",
                Email = "wrongpassworduser@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
                RoleId = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new UserLoginDto
            {
                Email = "wrongpassworduser@example.com",
                Password = "incorrectpassword"
            };

            var result = await _loginUserUseCase.Execute(loginDto);

            Assert.False(result.IsValid);
            Assert.Null(result.AccessToken);
            Assert.Null(result.RefreshToken);
        }
    }
}
