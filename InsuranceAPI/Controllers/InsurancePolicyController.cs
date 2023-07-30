using AutoMapper;
using InsuranceAPI.Infrastructure.DTOs.InsurancePolicy;
using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using InsuranceAPI.Utils.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InsuranceAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InsurancePolicyController : Controller
    {
        private readonly ILogger<InsurancePolicyController> _logger;
        private readonly IMapper _mapper;
        private readonly Infrastructure.Models.MongoDatabaseSettings _databaseSettings;
        private readonly IInsurancePoliciesService _insurancePoliciesService;
        private readonly IClientsService _clientsService;

        public InsurancePolicyController
        (
            ILogger<InsurancePolicyController> logger,
            IMapper mapper,
            IOptions<Infrastructure.Models.MongoDatabaseSettings> databaseSettings,
            IInsurancePoliciesService insurancePoliciesService,
            IClientsService clientsService
        )
        {
            _logger = logger;
            _mapper = mapper;
            _databaseSettings = databaseSettings.Value;
            _insurancePoliciesService = insurancePoliciesService;
            _clientsService = clientsService;
        }

        [Authorize(UserRole.Administrator, UserRole.SalesTeam)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var insurancePolicies = await _insurancePoliciesService.GetAll();

            var response = new List<InsurancePolicyResponseDTO>();

            foreach (var insurancePolicy in insurancePolicies)
            {
                var responseItem = _mapper.Map<InsurancePolicyResponseDTO>(insurancePolicy);
                SetClients(responseItem, insurancePolicy);

                response.Add(responseItem);
            }

            return Json(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var insurancePolicy = await _insurancePoliciesService.GetById(id);

            if (!CanSeeInsurancePolicy(insurancePolicy)) return Unauthorized("Token is not valid");

            var response = _mapper.Map<InsurancePolicyResponseDTO>(insurancePolicy);
            SetClients(response, insurancePolicy);

            return Json(response);
        }

        [Authorize(UserRole.Administrator, UserRole.SalesTeam)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateInsurancePolicyRequestDTO model)
        {
            var insurancePolicy = _mapper.Map<InsurancePolicy>(model);
            insurancePolicy.Clients = new List<ClientReference>();

            foreach (var clientRole in model.Clients)
            {
                await _clientsService.GetById(clientRole.Key);

                var clientReference = new ClientReference
                {
                    ClientId = new MongoDBRef(_databaseSettings.ClientsCollectionName, clientRole.Key),
                    Role = clientRole.Value,
                };

                insurancePolicy.Clients.Add(clientReference);
            }

            if (!insurancePolicy.Clients.Any()) throw new ApplicationException("Policy should have at least one client");

            insurancePolicy.Plan = _mapper.Map<PolicyPlan>(model);
            insurancePolicy.Vehicle = _mapper.Map<Vehicle>(model);
            insurancePolicy.Inspection = _mapper.Map<Inspection>(model);

            await _insurancePoliciesService.Create(insurancePolicy);

            return Json(new { message = "Insurance Policy created successfully " });
        }

        private void SetClients(InsurancePolicyResponseDTO response, InsurancePolicy insurancePolicy)
        {
            response.Clients = new Dictionary<string, string>();
            foreach (var clientReference in insurancePolicy.Clients)
            {
                var clientId = clientReference.ClientId.Id.AsString;
                response.Clients.Add(clientId, clientReference.Role.ToString());
            }
        }

        private bool CanSeeInsurancePolicy(InsurancePolicy insurancePolicy)
        {
            var user = (User)HttpContext.Items["User"];

            if (user.Role != UserRole.Client) return true;

            bool canSee = false;
            var clientsReference = insurancePolicy.Clients;
            foreach (var clientReference in clientsReference)
            {
                var userId = clientReference.Client.UserId.Id.AsObjectId;
                canSee = userId == user.Id;
            }

            return canSee; ;
        }
    }
}