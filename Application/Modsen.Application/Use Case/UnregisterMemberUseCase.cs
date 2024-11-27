using Modsen.Domain;

namespace Modsen.Application
{
public class UnregisterMemberUseCase
{
    public async Task<string> ExecuteAsync(int eventId, int memberId)
    {
        var eventExists = await CheckEventExistsAsync(eventId);
        if (!eventExists)
            throw new NotFoundException($"Событие с ID {eventId} не найдено.");

        var memberExists = await CheckMemberExistsAsync(memberId);
        if (!memberExists)
            throw new NotFoundException($"Участник с ID {memberId} не найден.");

        await UnregisterMemberFromEvent(eventId, memberId);
        return $"Участник с ID {memberId} успешно удален из события с ID {eventId}.";
    }

    private Task<bool> CheckEventExistsAsync(int eventId) => Task.FromResult(true);
    private Task<bool> CheckMemberExistsAsync(int memberId) => Task.FromResult(true);
    private Task UnregisterMemberFromEvent(int eventId, int memberId) => Task.CompletedTask;
}

}