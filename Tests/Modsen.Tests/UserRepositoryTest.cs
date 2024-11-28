using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Modsen.Infrastructure;

namespace Modsen.Tests
{
    public class UserRepositoryTests
{
    private readonly UserRepository _repository;
    private readonly ModsenContext _context;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ModsenContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ModsenContext(options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        var testUser = new User { Id = 1, Email = "test@example.com", RefreshToken = "token123" };
        _context.Users.Add(testUser);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByEmailAsync("test@example.com");

        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }
}

}