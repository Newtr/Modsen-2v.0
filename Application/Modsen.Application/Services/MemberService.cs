using Modsen.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Modsen.Application
{
    public class MemberService
    {
        public async Task<IEnumerable<Member>> GetEventMembersAsync(int eventId)
        {
            var eventExists = await CheckEventExistsAsync(eventId);
            if (!eventExists)
                throw new NotFoundException($"Событие с ID {eventId} не найдено.");

            return await FetchMembersByEventId(eventId);
        }

        public async Task<Member> GetMemberByIdAsync(int memberId)
        {
            var member = await FetchMemberByIdAsync(memberId);
            if (member == null)
                throw new NotFoundException($"Участник с ID {memberId} не найден.");

            return member;
        }

        public async Task<string> RegisterMemberAsync(int eventId, int memberId)
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

        public async Task<string> UnregisterMemberAsync(int eventId, int memberId)
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
        private Task<bool> IsMemberRegisteredAsync(int eventId, int memberId) => Task.FromResult(false); 
        private Task<IEnumerable<Member>> FetchMembersByEventId(int eventId)
        {
            var members = new List<Member>(); 
            return Task.FromResult<IEnumerable<Member>>(members.AsEnumerable());
        } 
        private Task<Member> FetchMemberByIdAsync(int memberId) => Task.FromResult<Member>(null); 
        private Task RegisterMemberToEvent(int eventId, int memberId) => Task.CompletedTask; 
        private Task UnregisterMemberFromEvent(int eventId, int memberId) => Task.CompletedTask; 
    }
}
