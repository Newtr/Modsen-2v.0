using Modsen.Domain;

namespace Modsen.Application
{
public class RegisterMemberUseCase
{
    public async Task<string> ExecuteAsync(int eventId, int memberId)
    {
        var eventExists = await CheckEventExistsAsync(eventId);
        if (!eventExists)
            throw new NotFoundException($"Событие с ID {eventId} не найдено.");

        var memberExists = await CheckMemberExistsAsync(memberId);
        if (!memberExists)
            throw new NotFoundException($"Участник с ID {memberId} не найден.");

        var alreadyRegistered = await IsMemberRegisteredAsync(eventId, memberId);
        if (alreadyRegistered)
            throw new BadRequestException($"Участник с ID {memberId} уже зарегистрирован на событие с ID {eventId}.");

        await RegisterMemberToEvent(eventId, memberId);
        return $"Участник с ID {memberId} успешно зарегистрирован на событие с ID {eventId}.";
    }

    private Task<bool> CheckEventExistsAsync(int eventId) => Task.FromResult(true);
    private Task<bool> CheckMemberExistsAsync(int memberId) => Task.FromResult(true);
    private Task<bool> IsMemberRegisteredAsync(int eventId, int memberId) => Task.FromResult(false);
    private Task RegisterMemberToEvent(int eventId, int memberId) => Task.CompletedTask;
}

}