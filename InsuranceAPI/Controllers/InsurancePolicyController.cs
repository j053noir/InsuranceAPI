using Microsoft.AspNetCore.Mvc;

namespace InsuranceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InsurancePolicyController : ControllerBase
    {
        private readonly ILogger<InsurancePolicyController> _logger;

        public InsurancePolicyController(ILogger<InsurancePolicyController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetInsurancePolicies")]
        public IEnumerable<object> Get()
        {
            throw new NotImplementedException();
        }
    }
}