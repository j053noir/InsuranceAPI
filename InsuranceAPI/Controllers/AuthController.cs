using AutoMapper;
using InsuranceAPI.Infrastructure.DTOs.Users;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using InsuranceAPI.Utils.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, IUserService userService, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticationRequestDTO model)
        {
            var ipAddress = GetIPAddress();
            var response = await _userService.Authenticate(model, ipAddress);
            return Json(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequestDTO model)
        {
            model.Role = Infrastructure.Models.UserRole.Client;
            await _userService.Register(model);

            var authenticateModel = _mapper.Map<AuthenticationRequestDTO>(model);

            var ipAddress = GetIPAddress();
            var response = await _userService.Authenticate(authenticateModel, ipAddress);

            return Json(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO model)
        {
            var token = model.Token;

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await _userService.RefreshToken(token, GetIPAddress());
            return Json(response);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RevokeRefreshTokenDTO model)
        {
            var token = model.Token;

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

             await _userService.RevokeToken(token, GetIPAddress());
            return Json(new { message = "Token revoked"});
        }

        private string GetIPAddress()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

            if (string.IsNullOrEmpty(ipAddress)) throw new ApplicationException("IP Address cannot be found");

            return ipAddress;
        }
    }
}
