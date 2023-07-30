using InsuranceAPI.Infrastructure.Models;
using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Repositories.Interfaces
{
    public interface IInsurancePoliciesRepository : IGenericRepository<InsurancePolicy>
    { 
    }
}
