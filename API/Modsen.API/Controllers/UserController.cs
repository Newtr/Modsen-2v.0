using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modsen.Application;
using Modsen.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RegisterUserUseCase _registerUserUseCase;
        private readonly LoginUserUseCase _loginUserUseCase;
        private readonly RefreshTokenUseCase _refreshTokenUseCase;
        private readonly GetAllUsersUseCase _getAllUsersUseCase;
        private readonly IMapper _mapper;

        public UserController(
            RegisterUserUseCase registerUserUseCase,
            LoginUserUseCase loginUserUseCase,
            RefreshTokenUseCase refreshTokenUseCase,
            GetAllUsersUseCase getAllUsersUseCase,
            IMapper mapper)
        {
            _registerUserUseCase = registerUserUseCase;
            _loginUserUseCase = loginUserUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
            _getAllUsersUseCase = getAllUsersUseCase;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto registrationDto, CancellationToken cancellationToken)
        {
            await _registerUserUseCase.Execute(registrationDto, cancellationToken);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto, CancellationToken cancellationToken)
        {
            var result = await _loginUserUseCase.Execute(loginDto, cancellationToken);

            if (!result.IsValid)
            {
                return Unauthorized(new { Error = "Invalid email or password" });
            }

            return Ok(new 
            { 
                AccessToken = result.AccessToken, 
                RefreshToken = result.RefreshToken 
            });
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
        public async Task<IActionResult> GetAllUsers(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var users = await _getAllUsersUseCase.Execute(page, pageSize, cancellationToken);
            var userDtos = _mapper.Map<IEnumerable<UserLoginDto>>(users);
            return Ok(userDtos);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRequestDto tokenRequest, CancellationToken cancellationToken)
        {
            var newAccessToken = await _refreshTokenUseCase.Execute(tokenRequest.RefreshToken, cancellationToken);
            return Ok(new { AccessToken = newAccessToken });
        }
    }
}
