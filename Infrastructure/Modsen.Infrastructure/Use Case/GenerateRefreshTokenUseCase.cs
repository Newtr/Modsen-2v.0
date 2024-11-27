namespace Modsen.Infrastructure
{
    public class GenerateRefreshTokenUseCase
    {
        public string Execute()
        {
            return Guid.NewGuid().ToString();
        }
    }
}