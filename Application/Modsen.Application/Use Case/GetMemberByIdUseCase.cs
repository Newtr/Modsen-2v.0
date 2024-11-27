using Microsoft.EntityFrameworkCore;
using Modsen.Domain;

namespace Modsen.Application
{
public class GetMemberByIdUseCase
{
    public async Task<Member> ExecuteAsync(int memberId)
    {
        var member = await FetchMemberByIdAsync(memberId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (member == null)
            throw new NotFoundException($"Участник с ID {memberId} не найден.");

        return member;
    }

    private IQueryable<Member> FetchMemberByIdAsync(int memberId)
    {
        var members = new List<Member>();
        return members.Where(m => m.Id == memberId).AsQueryable();
    }
}

}