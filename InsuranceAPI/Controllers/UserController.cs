using AutoMapper;
using InsuranceAPI.Infrastructure.DTOs.Users;
using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using InsuranceAPI.Utils.Attributes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace InsuranceAPI.Controllers
{
    [Authorize(UserRole.Administrator)]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IMapper mapper, IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();

            var response = _mapper.Map<IEnumerable<UserReponseDTO>>(users);

            return Json(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetById(new ObjectId(id));

            var response = _mapper.Map<UserReponseDTO>(user);

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegistrationRequestDTO model)
        {
            await _userService.Register(model);
            return Json(new { message = "User created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateRequestDTO model)
        {
            await _userService.Update(new ObjectId(id), model);
            return Json(new { message = "User updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.Delete(new ObjectId(id));
            return Json(new { message = "User deleted successfully" });
        }

        [HttpGet("user-roles")]
        public IActionResult GetAllUserRoles()
        {
            var userRoles = new List<UserRoleResponseDTO>();

            var roles = Enum.GetValues<UserRole>();

            for (int i = 0; i < roles.Length; i++)
            {
                userRoles.Add(new UserRoleResponseDTO
                {
                    Name = roles[i].ToString(),
                    Value = (int)roles[i]
                });
            }

            return Json(userRoles);
        }
    }
}
