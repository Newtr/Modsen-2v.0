using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modsen.Domain;
using Modsen.DTO;

namespace Modsen.API
{
    [Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistrationDto registrationDto)
    {
        await _userService.RegisterUser(registrationDto);
        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        var (isValid, accessToken, refreshToken) = await _userService.LoginUser(loginDto);

        if (!isValid)
        {
            return Unauthorized(new { Error = "Invalid email or password" });
        }

        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    [HttpGet("check-information")]
    [Authorize]
    public IActionResult CheckInformation()
    {
        var username = User.Identity?.Name;
        return Ok($"Hello {username}, this is a super cool page. You can see it because you have been logged in.");
    }

    [HttpGet("admin-resource")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult GetAdminResource()
    {
        return Ok("This is a protected admin resource.");
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers(int page = 1, int pageSize = 10)
    {
        var users = await _userService.GetAllUsers(page, pageSize);
        return Ok(users);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenRequestDto tokenRequest)
    {
        var newAccessToken = await _userService.RefreshToken(tokenRequest.RefreshToken);
        return Ok(new { AccessToken = newAccessToken });
    }
}

}