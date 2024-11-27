using Modsen.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
    public class RegisterMemberUseCase
    {
        public async Task<string> ExecuteAsync(int eventId, int memberId, CancellationToken cancellationToken)
        {
            var eventExists = await CheckEventExistsAsync(eventId, cancellationToken);
            if (!eventExists)
                throw new NotFoundException($"Событие с ID {eventId} не найдено.");

            var memberExists = await CheckMemberExistsAsync(memberId, cancellationToken);
            if (!memberExists)
                throw new NotFoundException($"Участник с ID {memberId} не найден.");

            var alreadyRegistered = await IsMemberRegisteredAsync(eventId, memberId, cancellationToken);
            if (alreadyRegistered)
                throw new BadRequestException($"Участник с ID {memberId} уже зарегистрирован на событие с ID {eventId}.");

            await RegisterMemberToEvent(eventId, memberId, cancellationToken);
            return $"Участник с ID {memberId} успешно зарегистрирован на событие с ID {eventId}.";
        }

        private Task<bool> CheckEventExistsAsync(int eventId, CancellationToken cancellationToken) 
            => Task.FromResult(true); 

        private Task<bool> CheckMemberExistsAsync(int memberId, CancellationToken cancellationToken) 
            => Task.FromResult(true); 

        private Task<bool> IsMemberRegisteredAsync(int eventId, int memberId, CancellationToken cancellationToken) 
            => Task.FromResult(false); 

        private Task RegisterMemberToEvent(int eventId, int memberId, CancellationToken cancellationToken) 
            => Task.CompletedTask;
    }
}
