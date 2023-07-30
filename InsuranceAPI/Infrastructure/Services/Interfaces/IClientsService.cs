using InsuranceAPI.Infrastructure.Models;

namespace InsuranceAPI.Infrastructure.Services.Interfaces
{
    public interface IClientsService
    {
        Task<IEnumerable<Client>> GetAll();
        Task<Client> GetById(string id);
        Task Create(Client client);
    }
}
