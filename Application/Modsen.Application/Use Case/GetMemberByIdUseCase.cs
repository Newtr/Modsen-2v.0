using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Modsen.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
    public class GetMemberByIdUseCase
{
    private readonly ModsenContext _context;

    public GetMemberByIdUseCase(ModsenContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Member> ExecuteAsync(int memberId, CancellationToken cancellationToken)
    {
        var member = await FetchMemberByIdAsync(memberId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (member == null)
            throw new NotFoundException($"Участник с ID {memberId} не найден.");

        return member;
    }

    private IQueryable<Member> FetchMemberByIdAsync(int memberId)
    {
        return _context.Set<Member>().Where(m => m.Id == memberId);
    }
}

}
