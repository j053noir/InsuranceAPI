using AutoMapper;
using InsuranceAPI.Infrastructure.DTOs.Client;
using InsuranceAPI.Infrastructure.DTOs.Users;
using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using InsuranceAPI.Utils.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InsuranceAPI.Controllers
{
    //[Authorize(UserRole.Administrator)]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IMapper _mapper;
        private readonly Infrastructure.Models.MongoDatabaseSettings _databaseSettings;
        private readonly IClientsService _clientService;
        private readonly IUserService _userService;

        public ClientController
        (
            ILogger<ClientController> logger,
            IMapper mapper,
            IOptions<Infrastructure.Models.MongoDatabaseSettings> databaseSettings,
            IClientsService clientService,
            IUserService userService
        )
        {
            _logger = logger;
            _mapper = mapper;
            _databaseSettings = databaseSettings.Value;
            _clientService = clientService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAll();

            foreach (var client in clients)
            {
                var userId = client.UserId.Id.AsObjectId;
                client.User = await _userService.GetById(userId);
            }

            return Json(_mapper.Map<IEnumerable<ClientResponseDTO>>(clients));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var client = await _clientService.GetById(id);

            if (client != null)
            {
                var userId = client.UserId.Id.AsObjectId;
                client.User = await _userService.GetById(userId);
            }

            return Json(_mapper.Map<ClientResponseDTO>(client));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientRequestDTO model)
        {
            // Create user
            var userModel = _mapper.Map<RegistrationRequestDTO>(model);
            await _userService.Register(userModel);
            var user = await _userService.GetByUsername(userModel.UserName);

            // Create Client
            var address = _mapper.Map<Address>(model);
            address.AdditionalInfo = new Dictionary<string, string>();
            var contactInformation = _mapper.Map<ContactInformation>(model);
            contactInformation.Address = address;
            contactInformation.AdditionalInfo = new Dictionary<string, string>();
            var client = _mapper.Map<Client>(model);
            client.ContactInformation = contactInformation;
            client.UserId = new MongoDBRef(_databaseSettings.UsersCollectionName, user.Id);

            await _clientService.Create(client);

            return Json(new { message = "Client created successfully " });
        }
    }
}
