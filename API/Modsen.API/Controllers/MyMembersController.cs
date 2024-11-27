using Microsoft.AspNetCore.Mvc;
using Modsen.Application;
using Modsen.Domain;

namespace Modsen.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyMembersController : ControllerBase
    {
        private readonly MemberService _memberService;

        public MyMembersController(MemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("{eventId}/members")]
        public async Task<ActionResult<IEnumerable<Member>>> GetEventMembers(int eventId)
        {
            var members = await _memberService.GetEventMembersAsync(eventId);
            return Ok(members);
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<Member>> GetMemberById(int memberId)
        {
            var member = await _memberService.GetMemberByIdAsync(memberId);
            return Ok(member);
        }

        [HttpPost("{eventId}/register")]
        public async Task<IActionResult> RegisterMember(int eventId, [FromBody] int memberId)
        {
            var result = await _memberService.RegisterMemberAsync(eventId, memberId);
            return Ok(result);
        }

        [HttpDelete("unregister")]
        public async Task<IActionResult> UnregisterFromEvent(int eventId, int memberId)
        {
            var result = await _memberService.UnregisterMemberAsync(eventId, memberId);
            return Ok(result);
        }
    }
}
