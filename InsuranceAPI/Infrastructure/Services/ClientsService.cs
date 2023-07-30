using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Repositories.Interfaces;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Services
{
    public class ClientsService : IClientsService
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsService(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        public async Task Create(Client client)
        {
            await _clientsRepository.Add(client);
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _clientsRepository.GetAll();
        }

        public async Task<Client> GetById(string id)
        {
            var client = await _clientsRepository.GetById(new ObjectId(id));

            return client ?? throw new KeyNotFoundException(id.ToString());
        }
    }
}
