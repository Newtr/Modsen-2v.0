using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Modsen.Application;
using Modsen.Domain;
using Modsen.DTO;

namespace Modsen.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyMembersController : ControllerBase
    {
        private readonly MemberService _memberService;
        private readonly IMapper _mapper;

        public MyMembersController(MemberService memberService, IMapper mapper)
        {
            _memberService = memberService;
            _mapper = mapper;
        }

        [HttpGet("{eventId}/members")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetEventMembers(int eventId, CancellationToken cancellationToken)
        {
            var members = await _memberService.GetEventMembersAsync(eventId, cancellationToken);
            var memberDtos = _mapper.Map<IEnumerable<MemberDto>>(members);
            return Ok(memberDtos);
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<MemberDto>> GetMemberById(int memberId, CancellationToken cancellationToken)
        {
            var member = await _memberService.GetMemberByIdAsync(memberId, cancellationToken);
            var memberDto = _mapper.Map<MemberDto>(member);
            return Ok(memberDto);
        }

        [HttpPost("{eventId}/register")]
        public async Task<IActionResult> RegisterMember(int eventId, [FromBody] int memberId, CancellationToken cancellationToken)
        {
            var result = await _memberService.RegisterMemberAsync(eventId, memberId, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("unregister")]
        public async Task<IActionResult> UnregisterFromEvent(int eventId, int memberId, CancellationToken cancellationToken)
        {
            var result = await _memberService.UnregisterMemberAsync(eventId, memberId, cancellationToken);
            return Ok(result);
        }
    }
}
