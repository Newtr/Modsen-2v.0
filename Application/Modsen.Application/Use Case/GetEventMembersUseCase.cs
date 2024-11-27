using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Member = Modsen.Domain.Member;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
    public class GetEventMembersUseCase
    {
        public async Task<IEnumerable<Member>> ExecuteAsync(int eventId, CancellationToken cancellationToken)
        {
            var eventExists = await CheckEventExistsAsync(eventId, cancellationToken);
            if (!eventExists)
                throw new NotFoundException($"Событие с ID {eventId} не найдено.");

            return await FetchMembersByEventId(eventId, cancellationToken)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        private Task<bool> CheckEventExistsAsync(int eventId, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        private IQueryable<Member> FetchMembersByEventId(int eventId, CancellationToken cancellationToken)
        {
            var members = new List<Member>();
            return members.AsQueryable();
        }
    }
}
