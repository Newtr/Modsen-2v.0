using Modsen.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
    public class UnregisterMemberUseCase
    {
        public async Task<string> ExecuteAsync(int eventId, int memberId, CancellationToken cancellationToken)
        {
            var eventExists = await CheckEventExistsAsync(eventId, cancellationToken);
            if (!eventExists)
                throw new NotFoundException($"Событие с ID {eventId} не найдено.");

            var memberExists = await CheckMemberExistsAsync(memberId, cancellationToken);
            if (!memberExists)
                throw new NotFoundException($"Участник с ID {memberId} не найден.");

            await UnregisterMemberFromEvent(eventId, memberId, cancellationToken);
            return $"Участник с ID {memberId} успешно удален из события с ID {eventId}.";
        }

        private Task<bool> CheckEventExistsAsync(int eventId, CancellationToken cancellationToken) 
            => Task.FromResult(true); 

        private Task<bool> CheckMemberExistsAsync(int memberId, CancellationToken cancellationToken) 
            => Task.FromResult(true); 

        private Task UnregisterMemberFromEvent(int eventId, int memberId, CancellationToken cancellationToken) 
            => Task.CompletedTask; 
    }
}
