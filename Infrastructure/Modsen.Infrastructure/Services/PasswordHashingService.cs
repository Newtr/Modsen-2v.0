using Modsen.Domain;

namespace Modsen.Infrastructure
{
public class PasswordHashingService : IPasswordHashingService
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
}

}