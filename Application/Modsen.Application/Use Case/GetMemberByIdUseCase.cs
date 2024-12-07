using Modsen.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
public class GetMemberByIdUseCase
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByIdUseCase(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
    }

    public async Task<Member> ExecuteAsync(int memberId, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetMemberByIdAsync(memberId, cancellationToken);

        if (member == null)
            throw new NotFoundException($"Участник с ID {memberId} не найден.");

        return member;
    }
}


}
