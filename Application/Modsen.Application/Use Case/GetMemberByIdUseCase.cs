using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
    public class GetMemberByIdUseCase
    {
        public async Task<Member> ExecuteAsync(int memberId, CancellationToken cancellationToken)
        {
            var member = await FetchMemberByIdAsync(memberId, cancellationToken)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException($"Участник с ID {memberId} не найден.");

            return member;
        }

        private IQueryable<Member> FetchMemberByIdAsync(int memberId, CancellationToken cancellationToken)
        {
            var members = new List<Member>();
            return members.Where(m => m.Id == memberId).AsQueryable();
        }
    }
}
