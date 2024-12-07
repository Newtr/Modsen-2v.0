using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Modsen.Application;
using Modsen.DTO;

namespace Modsen.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyMembersController : ControllerBase
    {
        private readonly GetEventMembersUseCase _getEventMembersUseCase;
        private readonly GetMemberByIdUseCase _getMemberByIdUseCase;
        private readonly RegisterMemberUseCase _registerMemberUseCase;
        private readonly UnregisterMemberUseCase _unregisterMemberUseCase;
        private readonly IMapper _mapper;

        public MyMembersController(
            GetEventMembersUseCase getEventMembersUseCase,
            GetMemberByIdUseCase getMemberByIdUseCase,
            RegisterMemberUseCase registerMemberUseCase,
            UnregisterMemberUseCase unregisterMemberUseCase,
            IMapper mapper)
        {
            _getEventMembersUseCase = getEventMembersUseCase;
            _getMemberByIdUseCase = getMemberByIdUseCase;
            _registerMemberUseCase = registerMemberUseCase;
            _unregisterMemberUseCase = unregisterMemberUseCase;
            _mapper = mapper;
        }

        [HttpGet("{eventId}/members")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetEventMembers(int eventId, CancellationToken cancellationToken)
        {
            var members = await _getEventMembersUseCase.ExecuteAsync(eventId, cancellationToken);
            var memberDtos = _mapper.Map<IEnumerable<MemberDto>>(members);
            return Ok(memberDtos);
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<MemberDto>> GetMemberById(int memberId, CancellationToken cancellationToken)
        {
            var member = await _getMemberByIdUseCase.ExecuteAsync(memberId, cancellationToken);
            var memberDto = _mapper.Map<MemberDto>(member);
            return Ok(memberDto);
        }

        [HttpPost("{eventId}/register")]
        public async Task<IActionResult> RegisterMember(int eventId, [FromBody] int memberId, CancellationToken cancellationToken)
        {
            var result = await _registerMemberUseCase.ExecuteAsync(eventId, memberId, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("unregister")]
        public async Task<IActionResult> UnregisterFromEvent(int eventId, int memberId, CancellationToken cancellationToken)
        {
            var result = await _unregisterMemberUseCase.ExecuteAsync(eventId, memberId, cancellationToken);
            return Ok(result);
        }
    }
}
