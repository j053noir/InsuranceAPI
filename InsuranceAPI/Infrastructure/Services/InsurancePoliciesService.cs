using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Repositories.Interfaces;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Services
{
    public class InsurancePoliciesService : IInsurancePoliciesService
    {
        private readonly IInsurancePoliciesRepository _insurancePoliciesRepository;

        public InsurancePoliciesService(IInsurancePoliciesRepository insurancePoliciesRepository)
        {
            _insurancePoliciesRepository = insurancePoliciesRepository;
        }

        public async Task Create(InsurancePolicy client)
        {
            await _insurancePoliciesRepository.Add(client);
        }

        public async Task<IEnumerable<InsurancePolicy>> GetAll()
        {
            return await _insurancePoliciesRepository.GetAll();
        }

        public async Task<InsurancePolicy> GetById(string id)
        {
            var insurancePolicy = await _insurancePoliciesRepository.GetById(new ObjectId(id));

            return insurancePolicy ?? throw new KeyNotFoundException(id.ToString());
        }
    }
}
