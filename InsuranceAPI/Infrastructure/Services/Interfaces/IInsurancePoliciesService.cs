using InsuranceAPI.Infrastructure.Models;

namespace InsuranceAPI.Infrastructure.Services.Interfaces
{
    public interface IInsurancePoliciesService
    {
        Task<IEnumerable<InsurancePolicy>> GetAll();
        Task<InsurancePolicy> GetById(string id);
        Task Create(InsurancePolicy client);
    }
}
