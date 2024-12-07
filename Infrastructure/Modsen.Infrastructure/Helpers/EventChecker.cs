using Modsen.Application;

namespace Modsen.Infrastructure
{
public class EventChecker : IEventChecker
{
    private readonly ModsenContext _dbContext;

    public EventChecker(ModsenContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool EventExists(int id)
    {
        return _dbContext.Events.Any(e => e.Id == id);
    }
}

}