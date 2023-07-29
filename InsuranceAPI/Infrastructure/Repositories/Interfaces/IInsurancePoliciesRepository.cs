using InsuranceAPI.Infrastructure.Models;
using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Respositories.Interfaces
{
    public interface IInsurancePoliciesRepository : IGenericRepository<InsurancePolicy>
    { 
    }
}
